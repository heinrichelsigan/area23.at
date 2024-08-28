using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Symchiffer
{

    /// <summary>
    /// static Des3 encryption helper
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.tripledes.-ctor?view=net-8.0" />
    /// <seealso cref="https://www.c-sharpcorner.com/article/tripledes-encryption-and-decryption-in-c-sharp/ "/>
    /// </summary>
    public static class Des3
    {

        #region fields

        private static string privateKey = string.Empty;

        internal static byte[] DesKey { get; private set; }
        internal static byte[] DesIv { get; private set; }

        internal static byte[] toDecryptArray;

        #endregion fields

        #region ctor_gen

        /// <summary>
        /// static constructor
        /// </summary>
        static Des3()
        {
            DesKey = Convert.FromBase64String(ResReader.GetValue(Constants.DES3_KEY));
            DesIv = Convert.FromBase64String(ResReader.GetValue(Constants.DES3_IV));
            // Generate a key using SHA256 hash function
            // TripleDesFromKey(null);
        }

        /// <summary>
        /// Generates Des3FromKey 
        /// </summary>
        /// <param name="secretKey">your plain text secret key</param>
        /// <param name="init">init TripleDes first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool Des3FromKey(string secretKey = "", bool init = true)
        {
            byte[] key;
            byte[] iv = new byte[8];

            if (!init)
            {
                if ((string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(secretKey)) ||
                    (!privateKey.Equals(secretKey, StringComparison.InvariantCultureIgnoreCase)))
                    return false;
            }

            if (init)
            {
                if (string.IsNullOrEmpty(secretKey))
                {
                    privateKey = string.Empty;
                    key = Convert.FromBase64String(ResReader.GetValue(Constants.DES3_KEY));
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.DES3_IV));
                }
                else
                {
                    privateKey = secretKey;
                    MD5 md5 = new MD5CryptoServiceProvider();
                    key = md5.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.DES3_IV));
                }

                DesIv = iv;
                DesKey = key;
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
            tdes.Key = DesKey;
            tdes.IV = DesIv;
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
            tdes.Key = DesKey;
            tdes.IV = DesIv;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.Zeros;
            toDecryptArray = new byte[cipherBytes.Length * 2];
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
        public static string EncryptString(string inString)
        {
            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(inString);
            byte[] encryptedBytes = Encrypt(inBytes);
            string encryptedText = Convert.ToBase64String(encryptedBytes);
            // System.Text.Encoding.UTF8.GetString(encryptedBytes).TrimEnd('\0');
            return encryptedText;
        }

        /// <summary>
        /// 3Des decrypts string
        /// </summary>
        /// <param name="cipherText">Base64 encoded encrypted byte[]</param>
        /// <returns>plain text string</returns>
        public static string DecryptString(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = Decrypt(cipherBytes);
            string plaintext = System.Text.Encoding.UTF8.GetString(decryptedBytes);
            return plaintext;
        }

        #endregion EnDeCryptString       

    }

}

