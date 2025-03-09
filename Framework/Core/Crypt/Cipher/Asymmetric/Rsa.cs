using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Area23.At.Framework.Core.Crypt.Cipher.Asymmetric
{
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
            AsymmetricKeyParameter keyParameterPublic, keyParameterPrivate;

            using (StringReader stringReader = new StringReader(publicKey))
            {
                keyParameterPublic = (AsymmetricKeyParameter)new PemReader(stringReader).ReadObject();
                // rsaCipher.Init(true, keyParameterPublic);
            }

            using (StringReader stringReader = new StringReader(CryptHelper.PrivateKeyWithUserHash(pubKey, privKey)))
            {
                keyPair = (AsymmetricCipherKeyPair)new PemReader(stringReader).ReadObject();
                keyParameterPrivate = keyPair.Private;
                // rsaCipher.Init(false, keyParameterPrivate);   
            }

            rsaKeyPair = new AsymmetricCipherKeyPair(keyParameterPublic, keyParameterPrivate);

            return rsaKeyPair;
        }


        #region EncryptDecryptBytes

        /// <summary>
        /// Rsa Encrypt
        /// </summary>
        /// <param name="plainInBytes">plain input byte[]</param>
        /// <returns>encryptedOutBytes</returns>
        public static byte[] Encrypt(byte[] plainInBytes)
        {
            Pkcs1Encoding rsaCipher = new Pkcs1Encoding(new RsaEngine());

            // using (StringReader stringReader = new StringReader(pubKey)) {
            // var keyParameter = (AsymmetricKeyParameter)new PemReader(stringReader).ReadObject();

            rsaCipher.Init(true, RsaKeyPair.Public);
            // }            

            byte[] encryptedOutBytes = rsaCipher.ProcessBlock(plainInBytes, 0, plainInBytes.Length);

            return encryptedOutBytes;

        }

        /// <summary>
        /// Rsa Decrypt
        /// </summary>
        /// <param name="encryptedInBytes">encrypted input byte array</param>
        /// <returns>plain out byte[]</returns>

        public static byte[] Decrypt(byte[] encryptedInBytes)
        {
            Pkcs1Encoding rsaCipher = new Pkcs1Encoding(new RsaEngine());

            // using (StringReader stringReader = new StringReader(privKey)) {
            // var keyPair = (AsymmetricCipherKeyPair)new PemReader(stringReader).ReadObject();

            rsaCipher.Init(false, RsaKeyPair.Private);
            // }

            byte[] plainOutBytes = rsaCipher.ProcessBlock(encryptedInBytes, 0, encryptedInBytes.Length);

            return plainOutBytes;
        }


        #endregion EncryptDecryptBytes

        #region EnDecryptString

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="inPlainString">plain text string</param>
        /// <returns>Base64 encoded encrypted byte[]</returns>
        public static string EncryptString(string inPlainString)
        {
            byte[] plainTextData = EnDeCodeHelper.GetBytes(inPlainString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = Convert.ToBase64String(encryptedData);

            return encryptedString;
        }

        /// <summary>
        /// Decrypts a string, that is truely a base64 encoded encrypted byte[]
        /// </summary>
        /// <param name="inCryptString">base64 encoded string from encrypted byte[]</param>
        /// <returns>plain text string (decrypted)</returns>
        public static string DecryptString(string inCryptString)
        {
            byte[] cryptData = Convert.FromBase64String(inCryptString);
            //  EnDeCoder.GetBytes(inCryptString);
            byte[] plainTextData = Decrypt(cryptData);
            string plainTextString = EnDeCodeHelper.GetString(plainTextData).TrimEnd('\0');

            return plainTextString;
        }

        #endregion EnDecryptString


    }

}
