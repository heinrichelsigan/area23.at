using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Input;

namespace Area23.At.Mono.Util.SymChiffer
{
    /// <summary>
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-8.0" />
    /// </summary>
    public static class Aes
    {
        private static string privateKey = string.Empty;

        internal static byte[] AesKey { get; private set; }
        internal static byte[] AesIv { get; private set; }

        internal static RijndaelManaged AesAlgo { get; private set; }


        /// <summary>
        /// static constructor
        /// </summary>
        static Aes()
        {
            AesAlgo = new RijndaelManaged();
            AesAlgo.Mode = CipherMode.ECB;
            AesAlgo.KeySize = 256;
            AesAlgo.Padding = PaddingMode.Zeros;

            AesKey = Convert.FromBase64String(ResReader.GetValue(Constants.AES_KEY));
            AesIv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));
            
            AesAlgo.Key = AesKey;
            AesAlgo.IV = AesIv;
            // AesGenWithNewKey(string.Empty, true);
        }

        /// <summary>
        /// AesGenWithNewKey generates a new static Aes RijndaelManaged symetric encryption 
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="init">init three fish first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool AesGenWithNewKey(string secretKey = "", bool init = true)
        {
            byte[] iv = new byte[16]; // AES > IV > 128 bit

            if (!init)
            {
                if ((string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(secretKey)) ||
                    (!privateKey.Equals(secretKey, StringComparison.InvariantCultureIgnoreCase)))
                    return false;
            }

            privateKey = secretKey;

            if (string.IsNullOrEmpty(secretKey))
            {
                AesKey = Convert.FromBase64String(ResReader.GetValue(Constants.AES_KEY));
                if (AesIv == null || AesIv.Length == 0)
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));
            }
            else
            {
                AesKey = Encoding.UTF8.GetByteCount(secretKey) == 32 ? Encoding.UTF8.GetBytes(secretKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                if (AesIv == null || AesIv.Length == 0)
                {
                    RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
                    randomNumGen.GetBytes(iv, 0, iv.Length);
                }
            }

            if (AesIv == null || AesIv.Length == 0)
            {
                AesIv = new byte[16];
                Array.Copy(iv, AesIv, 16);
            }

            // AesAlgo.GenerateIV();
            // AesAlgo.GenerateKey();
            AesAlgo.Key = AesKey;
            AesAlgo.IV = AesIv;

            return true;
        }

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
                throw new ArgumentNullException("Aes byte[] Encrypt(byte[] plainData): ArgumentNullException plainData = null or Lenght 0.");

            // create a decryptor by AesAlgo.CreateEncrypto(AesAlgo.Key, AesAlgo.IV);
            ICryptoTransform encryptor = AesAlgo.CreateEncryptor(AesAlgo.Key, AesAlgo.IV);
            byte[] encryptedBytes = encryptor.TransformFinalBlock(plainData, 0, plainData.Length);

            // return the encrypted bytes
            return encryptedBytes;
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
                throw new ArgumentNullException("Aes byte[] Decrypt(byte[] encryptedBytes): ArgumentNullException encryptedBytes = null or Lenght 0.");

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = AesAlgo.CreateDecryptor(AesAlgo.Key, AesAlgo.IV);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return decryptedBytes;
        }

        #region EnDecryptString

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="inPlainString">plain text string</param>
        /// <returns>Base64 encoded encrypted byte[]</returns>
        public static string EncryptString(string inPlainString)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inPlainString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = Convert.ToBase64String(encryptedData);
            // System.Text.Encoding.ASCII.GetString(encryptedData).TrimEnd('\0');
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
            //  System.Text.Encoding.UTF8.GetBytes(inCryptString);
            byte[] plainTextData = Decrypt(cryptData);
            string plainTextString = System.Text.Encoding.ASCII.GetString(plainTextData).TrimEnd('\0');

            return plainTextString;
        }

        #endregion EnDecryptString

        #region EnDecryptWithStream

        public static byte[] EncryptWithStream(byte[] inBytes)
        {
            if (inBytes == null || inBytes.Length <= 0)
                throw new ArgumentNullException("inBytes");
            byte[] encrypted;

            // Create a encryptor with an RijndaelManaged object with the specified Key and IV to perform the stream transform.
            ICryptoTransform encryptor = AesAlgo.CreateEncryptor(AesAlgo.Key, AesAlgo.IV);

            // Create the streams used for encryption. 
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(inBytes, 0, inBytes.Length);
                    csEncrypt.Flush();
                    encrypted = msEncrypt.ToArray();
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }

        public static byte[] DecryptByStream(byte[] cipherBytes)
        {
            if (cipherBytes == null || cipherBytes.Length <= 0)
                throw new ArgumentNullException("cipherBytes");

            byte[] outBytes = null;
            // Create a decryptor with an RijndaelManaged object with the specified Key and IV to perform the stream transform.
            ICryptoTransform decryptor = AesAlgo.CreateDecryptor(AesAlgo.Key, AesAlgo.IV);

            using (MemoryStream msDecryptStr = new MemoryStream(cipherBytes))
            {
                using (CryptoStream csDecryptStr = new CryptoStream(msDecryptStr, decryptor, CryptoStreamMode.Read))
                {
                    csDecryptStr.Read(outBytes, 0, (int)csDecryptStr.Length);
                    //using (var msPlain = new System.IO.MemoryStream())
                    //{
                    //    csDecryptStr.CopyTo(msPlain, (int)csDecryptStr.Length);
                    //    outBytes = msPlain.ToArray();
                    //}
                }
            }

            return outBytes;
        }

        #endregion EnDecryptWithStream

        #region ObsoleteDeprecated 

        [Obsolete("byte[] GenerateRandomPublicKey() is deprecated, and will be used only inside void AesGenWithNewKey(string inputKey = null).")]
        private static byte[] GenerateRandomPublicKey()
        {
            byte[] iv = new byte[16]; // AES > IV > 128 bit
            var randomNumGen = RandomNumberGenerator.Create();
            randomNumGen.GetBytes(iv, 0, iv.Length);
            // iv = RandomNumberGenerator.GetBytes(iv.Length);
            return iv;
        }

        [Obsolete("byte[] CreateAesKey(string inputString) is deprecated, please use void AesGenWithNewKey(string inputKey = null)", false)]
        private static byte[] CreateAesKey(string inputString)
        {
            return Encoding.UTF8.GetByteCount(inputString) == 32 ? Encoding.UTF8.GetBytes(inputString) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        #endregion ObsoleteDeprecated 
    }

}
