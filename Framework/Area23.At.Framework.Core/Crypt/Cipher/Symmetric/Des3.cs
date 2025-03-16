using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Static;
using System.Security.Cryptography;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// static Des3 encryption helper
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.tripledes.-ctor?view=net-8.0" />
    /// <seealso cref="https://www.c-sharpcorner.com/article/tripledes-encryption-and-decryption-in-c-sharp/ "/>
    /// </summary>
    [Obsolete("Please use TripleDes from CryptBounceCastle https://www.bouncycastle.org/", false)]
    public static class Des3
    {

        #region fields

        private static string key = string.Empty;

        private static string keyHash = string.Empty;

        #endregion fields

        #region Properties

        internal static byte[] KeyBytes { get; private set; }
        internal static byte[] HashBytes { get; private set; }

        internal static int KeyLen { get; private set; } = 24;

        internal static int HashLen { get; private set; } = 8;

        internal static byte[] toDecryptArray;

        #endregion Properties

        #region ctor_gen

        /// <summary>
        /// static constructor
        /// </summary>
        static Des3()
        {
            byte[] pKey = Convert.FromBase64String(ResReader.GetValue(Constants.DES3_KEY));
            byte[] pHash = Convert.FromBase64String(ResReader.GetValue(Constants.DES3_IV));

            KeyBytes = new byte[KeyLen];
            HashBytes = new byte[HashLen];
            Array.Copy(pKey, KeyBytes, KeyLen);
            Array.Copy(pHash, HashBytes, HashLen);
        }

        /// <summary>
        /// Des3GenWithKeyHash generates 3Des Enginge with key and hash
        /// </summary>
        /// <param name="secretKey">your plain text secret key</param>
        /// <param name="userHash">user key hash</param>
        /// <param name="init">init TripleDes first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool Des3GenWithKeyHash(string secretKey = "", string userHash = "", bool init = true)
        {
            byte[] pKey;
            byte[] pHash = new byte[HashLen];

            if (!init)
            {
                if ((string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(secretKey)) ||
                    (!key.Equals(secretKey, StringComparison.InvariantCultureIgnoreCase)))
                    return false;
            }

            if (init)
            {
                if (string.IsNullOrEmpty(secretKey))
                {
                    key = string.Empty;
                    pKey = Convert.FromBase64String(ResReader.GetValue(Constants.DES3_KEY));
                    pHash = Convert.FromBase64String(ResReader.GetValue(Constants.DES3_IV));
                }
                else
                {
                    key = secretKey;
                    keyHash = userHash;
                    // MD5 md5 = new MD5CryptoServiceProvider();
                    // pKey = md5.ComputeHash(EnDeCoder.GetBytes(secretKey));
                    pKey = CryptHelper.GetUserKeyBytes(key, keyHash, 24);
                    pHash = CryptHelper.GetUserKeyBytes(key, keyHash, 8);
                }

                KeyBytes = new byte[KeyLen];
                HashBytes = new byte[HashLen];
                Array.Copy(pKey, KeyBytes, KeyLen);
                Array.Copy(pHash, HashBytes, HashLen);
            }

            return true;
        }

        #endregion ctor_gen

        #region EncryptDecryptBytes

        /// <summary>
        /// 3Des encrypt bytes
        /// </summary>
        /// <param name="inBytes">Hex bytes</param>
        /// <returns>byte[] encrypted bytes</returns>
        public static byte[] Encrypt(byte[] inBytes)
        {
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = KeyBytes;
            tdes.IV = HashBytes;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.Zeros;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] cryptedBytes;
            cryptedBytes = cTransform.TransformFinalBlock(inBytes, 0, inBytes.Length);
            tdes.Clear();

            return cryptedBytes;
        }

        /// <summary>
        /// 3Des decrypt bytes
        /// </summary>
        /// <param name="inBytes">Hex bytes encrypted</param>
        /// <returns>byte[] decrypted bytes</returns>
        public static byte[] Decrypt(byte[] cipherBytes)
        {
            // Check arguments. 
            if (cipherBytes == null || cipherBytes.Length <= 0)
                throw new ArgumentNullException("cipherBytes");

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = KeyBytes;
            tdes.IV = HashBytes;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.Zeros;

            toDecryptArray = new byte[(cipherBytes.Length * 3) + 1];
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] decryptedBytes = cTransform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            tdes.Clear();

            // return decrypted byte[]
            return decryptedBytes;
        }

        #endregion EncryptDecryptBytes

        #region EnDeCryptString

        /// <summary>
        /// 3Des encrypt string
        /// </summary>
        /// <param name="inString">string in plain text</param>
        /// <returns>Base64 encoded encrypted byte array</returns>
        public static string EncryptString(string inString, EncodingType encType = EncodingType.Base64)
        {
            byte[] inBytes = EnDeCodeHelper.GetBytes(inString);
            byte[] encryptedBytes = Encrypt(inBytes);
            string encryptedText = EnDeCodeHelper.EncodeBytes(encryptedBytes, encType);
            return encryptedText;
        }

        /// <summary>
        /// 3Des decrypts string
        /// </summary>
        /// <param name="cipherText">Base64 encoded encrypted byte[]</param>
        /// <returns>plain text string</returns>
        public static string DecryptString(string cipherText, EncodingType encType = EncodingType.Base64)
        {
            byte[] cipherBytes = EnDeCodeHelper.DecodeText(cipherText, encType);
            byte[] decryptedBytes = Decrypt(cipherBytes);
            string plaintext = EnDeCodeHelper.GetString(decryptedBytes);
            return plaintext;
        }

        #endregion EnDeCryptString       

    }

}

