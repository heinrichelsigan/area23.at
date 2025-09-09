using Area23.At.Framework.Library.Crypt;
using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{
    /// <summary>
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-8.0" />
    /// </summary>
    public class AesNet
    {

        #region properties

        public static byte[] AesKey { get; private set; }
        public static int AesKeyLen { get; private set; }
        public static byte[] AesIv { get; private set; }

        public static RijndaelManaged AesAlgo { get; private set; }

        #endregion properties

        #region ctor helpers

        protected internal void GenAesKey(ref byte[] keyBytes)
        {
            
            List<byte> span = new List<byte>(keyBytes);
            while (span.Count < AesKeyLen)
                span.AddRange(keyBytes);

            AesKey = new byte[AesKeyLen];
            Array.Copy(span.ToArray(), 0, AesKey, 0, AesKeyLen);
            keyBytes = new byte[AesKeyLen];
            Array.Copy(span.ToArray(), 0, keyBytes, 0, AesKeyLen);
         
        }

        protected internal void GenAesIv(byte[] keyBytes, ref byte[] ivBytes)
        {
            var aesHelper = new RijndaelManaged();
            aesHelper.Key = keyBytes;
            aesHelper.GenerateIV();
            int iVLenght = aesHelper.IV.Length;
            AesIv = new byte[iVLenght];
            if (iVLenght > AesKeyLen)
            {
                while (ivBytes.Length < iVLenght)
                    ivBytes = ivBytes.TarBytes(ivBytes);
                Array.Copy(ivBytes, 0, AesIv, 0, iVLenght);
            }
            else
                Array.Copy(ivBytes, 0, AesIv, 0, iVLenght);
            
            ivBytes = new byte[iVLenght];
            Array.Copy(AesIv, 0, ivBytes, 0, iVLenght);
            
        }

        #endregion ctor helpers

        #region ctor

        static AesNet()
        {
            AesKeyLen = 32;
        }

        public AesNet() : this(Convert.FromBase64String(Constants.AES_KEY), Convert.FromBase64String(Constants.AES_IV)) { }

        public AesNet(string key, string hash)
        {
            if (string.IsNullOrEmpty(key) && string.IsNullOrEmpty(hash))
            {
                key = Constants.AES_KEY;
                hash = Constants.AES_IV;
            }
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] hashBytes = Encoding.UTF8.GetBytes(hash);

            try
            {
                GenAesKey(ref keyBytes);
                GenAesIv(AesKey, ref hashBytes);
            }
            catch (Exception e)
            {
                Area23Log.LogOriginEx("AesNet.ctor(key, hash)", e);
                // TODO: what shell we do with the drunken sailor
                AesKey = Convert.FromBase64String(Constants.AES_KEY);
                AesIv = Encoding.UTF8.GetBytes(Constants.AES_IV);
            }

            AesAlgo = new RijndaelManaged();
            // AesAlgo.KeySize = AesKeyLen;
            AesAlgo.Key = AesKey;
            AesAlgo.IV = AesIv;
            AesAlgo.Mode = CipherMode.ECB;
            AesAlgo.Padding = PaddingMode.Zeros;
        }

        public AesNet(byte[] aesKey, byte[] aesIv)
        {
            if (aesKey == null || aesKey.Length == 0)
                aesKey = Convert.FromBase64String(Constants.AES_KEY);
            if (aesIv == null || aesIv.Length == 0)
                aesIv = Encoding.UTF8.GetBytes(Constants.AES_IV);

            GenAesKey(ref aesKey);
            GenAesIv(aesKey, ref aesIv);

            AesAlgo = new RijndaelManaged();            
            AesAlgo.Key = AesKey;
            AesAlgo.IV = AesIv;
            AesAlgo.Mode = CipherMode.ECB;
            AesAlgo.Padding = PaddingMode.Zeros;

        }

        #endregion ctor

        #region en-/decrypt

        /// <summary>
        /// AES Encrypt by using RijndaelManaged
        /// </summary>
        /// <param name="plainData">Array of plain data byte</param>
        /// <returns>Array of encrypted data byte</returns>
        /// <exception cref="ArgumentNullException">is thrown when input enrypted <see cref="byte[]"/> is null or zero length</exception>
        public byte[] Encrypt(byte[] plainData)
        {
            // Check arguments. 
            if (plainData == null || plainData.Length <= 0)
                throw new ArgumentNullException("plainData is null or length = 0 in static byte[] EncryptBytes(byte[] plainData)...");

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
        public byte[] Decrypt(byte[] encryptedBytes) 
        {
            // Check arguments. 
            if (encryptedBytes == null || encryptedBytes.Length <= 0)
                throw new ArgumentNullException("ArgumentNullException encryptedBytes = null or Lenght 0 in static string DecryptBytes(byte[] encryptedBytes)...");

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = AesAlgo.CreateDecryptor(AesAlgo.Key, AesAlgo.IV);
            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return decryptedBytes;
        }

        #endregion en-/decrypt

        #region EnDecryptString

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="inPlainString">plain text string</param>
        /// <returns>Base64 encoded encrypted byte[]</returns>
        public string EncryptString(string inPlainString)
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
        public string DecryptString(string inCryptString)
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
            // Check arguments. 
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
            // Check arguments. 
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

    }
}