using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Static;
using System.Security.Cryptography;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-8.0" />
    /// </summary>
    [Obsolete("Please use Aes in CryptBounceCastle https://www.bouncycastle.org/", false)]
    public static class Aes
    {

        #region fields

        private static string privateKey = string.Empty;
        private static string userHostIpAddress = string.Empty;

        #endregion fields

        #region Properties

        internal static byte[] AesKey { get; private set; }
        internal static byte[] AesIv { get; private set; }

        internal static string PrivateUserKey { get => string.Concat(privateKey, privateKey); }
        internal static string PrivateUserHostKey { get => string.Concat(privateKey, userHostIpAddress, privateKey, userHostIpAddress); }

        internal static System.Security.Cryptography.Aes AesAlgo { get; private set; }

        #endregion Properties

        #region Ctor_Gen

        /// <summary>
        /// static constructor
        /// </summary>
        static Aes()
        {
            byte[] key = Convert.FromBase64String(ResReader.GetValue(Constants.AES_KEY));
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));

            AesAlgo = new AesManaged();
            AesAlgo.Mode = CipherMode.ECB;
            AesAlgo.KeySize = 256;
            AesAlgo.Padding = PaddingMode.Zeros;


            AesKey = new byte[32];
            AesIv = new byte[16];
            Array.Copy(key, AesKey, 32);
            Array.Copy(iv, AesIv, 16);

            AesAlgo.Key = AesKey;
            AesAlgo.IV = AesIv;
            // AesGenWithNewKey(string.Empty, true);
        }

        /// <summary>
        /// AesGenWithKeyHash generates a new static Aes symetric encryption 
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="userHostAddr">user host address is here part of private key</param>
        /// <param name="init">init three fish first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool AesGenWithKeyHash(string secretKey = "heinrich.elsigan@area23.at", string userHash = "6865696e726963682e656c736967616e406172656132332e6174", bool init = true)
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
                    iv = CryptHelper.GetUserKeyBytes(secretKey, userHash, 16);
                    // iv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));
                }

                AesKey = new byte[32];
                AesIv = new byte[16];
                Array.Copy(key, AesKey, 32);
                Array.Copy(iv, AesIv, 16);
            }

            // AesAlgo.GenerateIV();
            // AesAlgo.GenerateKey();
            AesAlgo.Key = AesKey;
            AesAlgo.IV = AesIv;

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


        #region ObsoleteDeprecated 


        [Obsolete("EncryptWithStream(byte[] inBytes) is obsolete, use byte[] Encrypt(byte[] plainData) instead.", false)]
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

        [Obsolete("DecryptByStream(byte[] cipherBytes) is obsolete, use byte[] Decrypt(byte[] encryptedBytes) instead.", false)]
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
