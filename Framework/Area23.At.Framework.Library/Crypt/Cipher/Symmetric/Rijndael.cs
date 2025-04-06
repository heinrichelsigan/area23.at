using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// <see cref="System.Security.Cryptography.RijndaelManaged"/> implementation of Aes
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-8.0" />
    /// </summary>
    [Obsolete("Please use Aes from CryptBounceCastle https://www.bouncycastle.org/", false)]
    public static class Rijndael
    {

        #region fields

        private static string privateKey = string.Empty;
        private static string userHostIpAddress = string.Empty;

        #endregion fields

        #region Properties

        internal static byte[] RijndaelKey { get; private set; }
        internal static byte[] RijndaelIv { get; private set; }

        internal static System.Security.Cryptography.RijndaelManaged RijndaelAlgo { get; private set; }

        #endregion Properties

        #region Ctor_Gen

        /// <summary>
        /// static constructor
        /// </summary>
        static Rijndael()
        {
            byte[] key = CryptHelper.GetUserKeyBytes(Constants.AUTHOR_EMAIL, Constants.AUTHOR_IV, 32);
            byte[] iv = CryptHelper.GetUserKeyBytes(Constants.AUTHOR_IV, Constants.AUTHOR_EMAIL, 16); 

            RijndaelKey = new byte[32];
            RijndaelIv = new byte[16];
            Array.Copy(key, RijndaelKey, 32);
            Array.Copy(iv, RijndaelIv, 16);
            RijndaelAlgo = new RijndaelManaged()
            {
                // BlockSize = 256,
                KeySize = 256,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.ECB
            };
            RijndaelAlgo.Key = RijndaelKey;
            RijndaelAlgo.IV = RijndaelIv;
            // AesGenWithNewKey(string.Empty, true);
        }

        /// <summary>
        /// AesGenWithNewKey generates a new static Aes RijndaelManaged symetric encryption 
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="userHostAddr">user host address is here part of private key</param>
        /// <param name="init">init three fish first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool RijndaelGenWithNewKey(string secretKey = "", string userHash = "", bool init = true)
        {
            byte[] key = new byte[32];
            byte[] iv = new byte[16]; // AES > IV > 128 bit

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
                    key = Convert.FromBase64String(ResReader.GetValue(Constants.AES_KEY));
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));
                }
                else
                {
                    privateKey = secretKey;
                    key = CryptHelper.GetUserKeyBytes(secretKey, userHash, 32);
                    iv = CryptHelper.GetUserKeyBytes(userHash, secretKey, 16);
                }

                RijndaelKey = new byte[32];
                RijndaelIv = new byte[16];
                Array.Copy(key, RijndaelKey, 32);
                Array.Copy(iv, RijndaelIv, 16);
            }

            RijndaelAlgo.Key = RijndaelKey;
            RijndaelAlgo.IV = RijndaelIv;

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
                throw new ArgumentNullException("RijndaelAlgo byte[] Encrypt(byte[] plainData): ArgumentNullException plainData = null or Lenght 0.");

            // create a decryptor by AesAlgo.CreateEncrypto(AesAlgo.Key, AesAlgo.IV);
            ICryptoTransform encryptor = RijndaelAlgo.CreateEncryptor(RijndaelAlgo.Key, RijndaelAlgo.IV);
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
                throw new ArgumentNullException("RijndaelAlgo byte[] Decrypt(byte[] encryptedBytes): ArgumentNullException encryptedBytes = null or Lenght 0.");

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = RijndaelAlgo.CreateDecryptor(RijndaelAlgo.Key, RijndaelAlgo.IV);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return decryptedBytes;
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


        #region ObsoleteDeprecated 


        [Obsolete("EncryptWithStream(byte[] inBytes) is obsolete, use byte[] Encrypt(byte[] plainData) instead.", false)]
        public static byte[] EncryptWithStream(byte[] inBytes)
        {
            if (inBytes == null || inBytes.Length <= 0)
                throw new ArgumentNullException("inBytes");
            byte[] encrypted;

            // Create a encryptor with an RijndaelManaged object with the specified Key and IV to perform the stream transform.
            ICryptoTransform encryptor = RijndaelAlgo.CreateEncryptor(RijndaelAlgo.Key, RijndaelAlgo.IV);

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

        [Obsolete("DecryptByStream(byte[] cipherBytes) is obsolete, use byte[] Decrypt(byte[] encryptedBytes) instead.", false)]
        public static byte[] DecryptByStream(byte[] cipherBytes)
        {
            if (cipherBytes == null || cipherBytes.Length <= 0)
                throw new ArgumentNullException("cipherBytes");

            byte[] outBytes = null;
            // Create a decryptor with an RijndaelManaged object with the specified Key and IV to perform the stream transform.
            ICryptoTransform decryptor = RijndaelAlgo.CreateDecryptor(RijndaelAlgo.Key, RijndaelAlgo.IV);

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
            return EnDeCodeHelper.GetByteCount(inputString) == 32 ?
                EnDeCodeHelper.GetBytes(inputString) :
                SHA256.Create().ComputeHash(EnDeCodeHelper.GetBytes(inputString));
        }

        #endregion ObsoleteDeprecated 

    }

}
