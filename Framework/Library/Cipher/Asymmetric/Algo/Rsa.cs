using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;

namespace Area23.At.Framework.Library.Cipher.Asymmetric.Algo
{
    public static class Rsa
    {
        #region fields

        private static string privateKey = string.Empty;
        private static string userHostIpAddress = string.Empty;

        private static AsymmetricCipherKeyPair rsaKeyPair;

        #endregion fields

        #region Properties

        internal static AsymmetricCipherKeyPair RsaKeyPair
        {
            get => GetRsaKeyPair();
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
                rsaKeyPair = GetRsaKeyPair();
        }

        public static string InitGetPublicKey()
        {
            return (RsaPrivateKey != null && RsaPrivateKey != null) ? RsaPublicKey.ToString() : null;
        }

        #endregion Ctor_Gen

        internal static AsymmetricCipherKeyPair GetRsaKeyPair()
        {
            if (rsaKeyPair != null)
                return rsaKeyPair;

            RsaKeyPairGenerator rsaKeyPairGen = new RsaKeyPairGenerator();
            IRandomGenerator randGen = new VmpcRandomGenerator();

            SecureRandom rand = new SecureRandom(randGen, 16384);
            KeyGenerationParameters rsaKeyParams = new KeyGenerationParameters(rand, 16384);
            rsaKeyPairGen.Init(rsaKeyParams);

            rsaKeyPair = rsaKeyPairGen.GenerateKeyPair();
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
            rsaCipher.Init(true, RsaPublicKey);
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
            rsaCipher.Init(false, RsaPrivateKey);
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
            byte[] plainTextData = Encoding.UTF8.GetBytes(inPlainString);
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
            //  Encoding.UTF8.GetBytes(inCryptString);
            byte[] plainTextData = Decrypt(cryptData);
            string plainTextString = Encoding.ASCII.GetString(plainTextData).TrimEnd('\0');

            return plainTextString;
        }

        #endregion EnDecryptString

    }



}
