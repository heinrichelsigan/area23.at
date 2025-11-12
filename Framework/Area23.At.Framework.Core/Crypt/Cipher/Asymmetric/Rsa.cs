using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Text;

namespace Area23.At.Framework.Core.Crypt.Cipher.Asymmetric
{

    /// <summary>
    /// Rsa Asymmetric cipher 
    /// see https://github.dev/Ulterius/server/blob/770d1821de43cf1d0a93c79025995bdd812a76ee/RemoteTaskServer/Utilities/Security/Rsa.cs#L71#L87
    /// https://github.dev/ThePBone/GalaxyBudsClient/blob/a06eff7713fde924466ae1a214571a2b6f0d1a42/GalaxyBudsClient/Utils/EncryptionUtils.cs#L18#L34
    /// https://github.dev/Ulterius/server/blob/770d1821de43cf1d0a93c79025995bdd812a76ee/RemoteTaskServer/Utilities/Security/Rsa.cs#L71#L87
    /// </summary>
    public static class Rsa
    {
        #region fields

        private static string privateKey = string.Empty;
        private static string publicKey = string.Empty;
        private static string userHostIpAddress = string.Empty;

        private static AsymmetricCipherKeyPair rsaKeyPair;

        #endregion fields

        #region Properties

        internal static AsymmetricCipherKeyPair RsaKeyPair
        {
            get => rsaKeyPair;
        }

        public static AsymmetricKeyParameter RsaPublicKey
        {
            get => RsaKeyPair.Public;
            // private set => rsaKeyPair.Public = value;
        }

        private static AsymmetricKeyParameter RsaPrivateKey
        {
            get => RsaKeyPair.Private;
        }


        #endregion Properties

        #region Ctor_Gen

        static Rsa()
        {
            if (rsaKeyPair == null)
                rsaKeyPair = GenerateNewRsaKeyPair();
        }

        public static AsymmetricCipherKeyPair RsaGenWithKey(string pub, string priv)
        {
            if (rsaKeyPair != null)
                return rsaKeyPair;
            rsaKeyPair = GetRsaKeyPair(pub, priv);

            return rsaKeyPair;
        }

        #endregion Ctor_Gen

        /// <summary>
        /// GenerateNewRsaKeyPair - generates a new rsa key pair
        /// </summary>
        /// <returns><see cref="AsymmetricCipherKeyPairy"/></returns>
        internal static AsymmetricCipherKeyPair GenerateNewRsaKeyPair()
        {
            if (rsaKeyPair != null)
                return rsaKeyPair;

            RsaKeyPairGenerator rsaKeyPairGen = new RsaKeyPairGenerator();
            IRandomGenerator randGen = new VmpcRandomGenerator();

            SecureRandom rand = new SecureRandom(randGen, 2048);
            KeyGenerationParameters rsaKeyParams = new KeyGenerationParameters(rand, 2048);
            rsaKeyPairGen.Init(rsaKeyParams);

            rsaKeyPair = rsaKeyPairGen.GenerateKeyPair();
            return rsaKeyPair;

        }

        /// <summary>
        /// Get Rsa Key Pair by private and public key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="privateKey"></param>
        /// <returns><see cref="AsymmetricCipherKeyPair"/></returns>
        internal static AsymmetricCipherKeyPair GetRsaKeyPair(string pubKey, string privKey)
        {
            privateKey = privKey;
            publicKey = pubKey;
            Pkcs1Encoding rsaCipher = new Pkcs1Encoding(new RsaEngine());
            AsymmetricCipherKeyPair keyPair;
            AsymmetricKeyParameter keyParameterPublic;
            RsaPrivateCrtKeyParameters keyParameterPrivate;

            using (StringReader stringReader = new StringReader(publicKey))
            {
                keyParameterPublic = (AsymmetricKeyParameter)new PemReader(stringReader).ReadObject();
            }

            using (var txtreader = new StringReader(privateKey))
            {
                keyParameterPrivate = (RsaPrivateCrtKeyParameters)new PemReader(txtreader).ReadObject();
            }

            rsaKeyPair = new AsymmetricCipherKeyPair(keyParameterPublic, keyParameterPrivate);

            return rsaKeyPair;
        }


        #region EncryptDecryptBytes

        /// <summary>
        /// Rsa encrypt bytes with public key
        /// </summary>
        /// <param name="bytesToEncrypt"><see cref="byte[]">bytes to encrypt</see></param>
        /// <param name="pair"></param>
        /// <returns>encrypted <see cref="byte[]"/></returns>
        public static byte[] Encrypt(byte[] bytesToEncrypt, AsymmetricCipherKeyPair pair)
        {
            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            AsymmetricKeyParameter keyParameter = (rsaKeyPair != null) ? rsaKeyPair.Public : (AsymmetricKeyParameter)pair.Public;
            encryptEngine.Init(true, keyParameter);

            byte[] encryptedBytes = encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);
            return encryptedBytes;

        }

        public static byte[] EncryptWithPrivate(byte[] bytesToEncrypt, AsymmetricCipherKeyPair pair)
        {
            var encryptEngine = new Pkcs1Encoding(new RsaEngine());
            encryptEngine.Init(true, pair.Private);

            byte[] encryptedBytes = encryptEngine.ProcessBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);
            return encryptedBytes;
        }



        /// <summary>
        /// Rsa Decrypt
        /// </summary>
        /// <param name="bytesToDecrypt"></param>
        /// <param name="pair"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] bytesToDecrypt, AsymmetricCipherKeyPair pair)
        {
            var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            AsymmetricKeyParameter keyParameter = (rsaKeyPair != null) ? rsaKeyPair.Public : (AsymmetricKeyParameter)pair.Public;
            decryptEngine.Init(false, keyParameter);

            byte[] decrypted = decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length);
            return decrypted;
        }

        public static byte[] DecryptWithPrivate(byte[] bytesToDecrypt, AsymmetricCipherKeyPair pair)
        {
            AsymmetricCipherKeyPair keyPair;
            var decryptEngine = new Pkcs1Encoding(new RsaEngine());
            decryptEngine.Init(false, pair.Private);

            byte[] decryptedBytes = decryptEngine.ProcessBlock(bytesToDecrypt, 0, bytesToDecrypt.Length);
            return decryptedBytes;
        }

        #endregion EncryptDecryptBytes

        #region EnDecryptString

        /// <summary>
        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="inPlainString">plain text string</param>
        /// <param name="isPublic">true, if it's public key, false if it's private key</param>
        /// <returns>Base64 encoded encrypted byte[]</returns>
        public static string EncryptString(string inPlainString, bool isPublic = true)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inPlainString);
            byte[] encryptedData = (isPublic) ?
                Encrypt(plainTextData, RsaKeyPair) :
                EncryptWithPrivate(plainTextData, RsaKeyPair);
            string encryptedString = Convert.ToBase64String(encryptedData);

            return encryptedString;
        }

        /// <summary>
        /// Decrypts a string, that is truely a base64 encoded encrypted byte[]
        /// </summary>
        /// <param name="inCryptString">base64 encoded string from encrypted byte[]</param>
        /// <param name="isPublic">true, if it's public key, false if it's private key</param>
        /// <returns>plain text string (decrypted)</returns>
        public static string DecryptString(string inCryptString, bool isPublic = true)
        {
            byte[] cryptData = Convert.FromBase64String(inCryptString);
            //  EnDeCoder.GetBytes(inCryptString);
            byte[] decryptedBytes = (isPublic) ? Decrypt(cryptData, RsaKeyPair) : DecryptWithPrivate(cryptData, RsaKeyPair);
            string decrypted = Encoding.UTF8.GetString(decryptedBytes);
            string plainTextString = EnDeCodeHelper.GetString(decryptedBytes).TrimEnd('\0');

            return decrypted;
        }

        #endregion EnDecryptString

    }

}
