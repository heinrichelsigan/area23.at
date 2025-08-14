using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// <see cref="Org.BouncyCastle.Crypto.Engines.Rfc3211WrapEngine"/> implementation of BlockCipher Wrapper 
    /// </summary>
    public static class Rfc3211Wrap
    {

        #region fields

        private static string privateKey = string.Empty;
        private static string userHostIpAddress = string.Empty;

        #endregion fields

        #region Properties

        internal static byte[] WrapKey { get; private set; }
        internal static byte[] WrapIv { get; private set; }

        internal static IWrapper Wrapper { get; private set; }

        #endregion Properties

        #region Ctor_Gen

        /// <summary>
        /// static constructor
        /// </summary>
        static Rfc3211Wrap()
        {
            byte[] key = CryptHelper.GetUserKeyBytes(Constants.AUTHOR_EMAIL, Constants.AUTHOR_IV, 32);
            byte[] iv = CryptHelper.GetUserKeyBytes(Constants.AUTHOR_IV, Constants.AUTHOR_EMAIL, 16);

            WrapKey = new byte[16];
            WrapIv = new byte[16];
            Array.Copy(key, WrapKey, 16);
            Array.Copy(iv, WrapIv, 16);
            Wrapper = new Rfc3211WrapEngine(new DesEngine());
            // AesGenWithNewKey(string.Empty, true);
        }

        /// <summary>
        /// Rfc3211WrapGenWithKey generates a new static Aes Rfc3211Wrapper for symetric encryption 
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="userHash">user host address is here part of private key</param>
        /// <param name="init">init three fish first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool Rfc3211WrapGenWithKey(string secretKey = "", string userHash = "", bool init = true)
        {
            byte[] key = new byte[16];
            byte[] iv = new byte[16]; // AES > IV > 128 bit

            if (!init)
            {
                if ((string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(secretKey)) ||
                    (!privateKey.Equals(secretKey, StringComparison.InvariantCultureIgnoreCase)))
                    return false;
            }

            if (Wrapper == null)
                Wrapper = new Rfc3211WrapEngine(new AesEngine());

            if (init)
            {
                if (string.IsNullOrEmpty(secretKey))
                {
                    privateKey = string.Empty;
                    key = Convert.FromBase64String(ResReader.GetValue(Constants.AES_KEY));
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));
                }
            }

            if (!string.IsNullOrEmpty(secretKey) && userHash != null)
            {
                privateKey = secretKey;
                key = CryptHelper.GetUserKeyBytes(secretKey, userHash, 16);
                iv = CryptHelper.GetUserKeyBytes(userHash, secretKey, 16);
                WrapKey = new byte[16];
                WrapIv = new byte[16];
                Array.Copy(key, WrapKey, 16);
                Array.Copy(iv, WrapIv, 16);
            }

            Wrapper.Init(init, new ParametersWithIV(new KeyParameter(WrapKey), WrapIv));

            return true;
        }

        #endregion Ctor_Gen


        #region EncryptDecryptBytes

        /// <summary>
        /// AES Encrypt by using RijndaelManaged
        /// </summary>
        /// <param name="plainData">Array of plain data byte</param>
        /// <returns>Array of encrypted data byte</returns>
        /// <exception cref="ArgumentNullException">is thrown when input enrypted <see cref="byte[]"/> is null or zero length</exception>
        public static byte[] Encrypt(byte[] plainData)
        {
            // Check arguments.
            if (plainData == null || plainData.Length <= 0)
                throw new ArgumentNullException("Rfc3211Wrap byte[] Encrypt(byte[] plainData): plainData == null or plainData.Lenght == 0.");
            if (Wrapper == null)
            {
                Wrapper = new Rfc3211WrapEngine(new DesEngine());                   // init the wrapper true for wrap with KeyParameter
                Wrapper.Init(true, new ParametersWithIV(new KeyParameter(WrapKey), WrapIv));

            }
            
            byte[] encryptedBytes = Wrapper.Wrap(plainData, 0, plainData.Length);   // wrap plainData   

            return encryptedBytes;                                                  // return the encrypted bytes
        }

        /// <summary>
        /// AES Decrypt by using RijndaelManaged
        /// </summary>
        /// <param name="encryptedBytes">Array of encrypted data byte</param>
        /// <returns>Array of plain data byte</returns>
        /// <exception cref="ArgumentNullException">is thrown when input enrypted <see cref="byte[]"/> is null or zero length</exception>
        public static byte[] Decrypt(byte[] encryptedBytes)
        {
            if (encryptedBytes == null || encryptedBytes.Length <= 0)
                throw new ArgumentNullException("Rfc3211Wrap byte[] Decrypt(byte[] encryptedBytes): encryptedBytes == null or encryptedBytes.Lenght == 0.");
            if (Wrapper == null)
            {
                Wrapper = new Rfc3211WrapEngine(new DesEngine());   // init the wrapper false for unwrap with KeyParameter
                Wrapper.Init(false, new ParametersWithIV(new KeyParameter(WrapKey), WrapIv));
            }
                                                                    // unwrap encryptedBytes
            byte[] decryptedBytes = Wrapper.Unwrap(encryptedBytes, 0, encryptedBytes.Length);   

            return decryptedBytes;                                  // return the decrypted bytes
        }

        #endregion EncryptDecryptBytes

        #region EnDecryptString

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="inPlainString">plain text string</param>
        /// <returns>Base64 encoded encrypted byte[]</returns>
        public static string EncryptString(string inPlainString, EncodingType encType = EncodingType.Base64)
        {
            byte[] plainTextData = EnDeCodeHelper.GetBytes(inPlainString);
            byte[] encryptedBytes = Encrypt(plainTextData);
            string encryptedString = EnDeCodeHelper.EncodeBytes(encryptedBytes, encType);

            return encryptedString;
        }

        /// <summary>
        /// Decrypts a string, that is truely a base64 encoded encrypted byte[]
        /// </summary>
        /// <param name="cipherText">base64 encoded string from encrypted byte[]</param>
        /// <returns>plain text string (decrypted)</returns>
        public static string DecryptString(string cipherText, EncodingType encType = EncodingType.Base64)
        {
            byte[] cryptData = EnDeCodeHelper.DecodeText(cipherText, encType);
            byte[] decryptedBytes = Decrypt(cryptData);
            string plainTextString = EnDeCodeHelper.GetString(decryptedBytes).TrimEnd('\0');

            return plainTextString;
        }

        #endregion EnDecryptString

    }

}
