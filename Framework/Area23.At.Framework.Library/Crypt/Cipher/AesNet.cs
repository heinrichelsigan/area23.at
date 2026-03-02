using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{


    /// <summary>
    /// AesNet native .Net AesCng without bouncy castle
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aescng?view=net-8.0" />
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>code changes</listheader>
    /// <item>
    /// 2026-02-11 alert-fix-13 changed mode from "ECB" to "CFB"     
    /// Reason: Git security scans
    /// consequences: no more fully deterministic math bijective proper symmertric cipher en-/decryption in pipe
    /// fixed attacks: not so easy REPLY attacks with binary format header and heuristic key collection
    /// </item>
    /// <item>
    /// 2026-mm-dd [enter pull request name here] [enter what you did here]
    /// Reason: [enter a senseful reason]
    /// consequences: [describe most impactful consequences of bugfix or code change request]
    /// fixed [vulnerability, code smell]: [Describe understandable precise in 1-2 setences]
    /// </item>
    /// </list>
    /// </remarks>
    public class AesNet
    {

        #region properties

        public static byte[] AesKey { get; private set; }
        public static int AesKeyLen { get; private set; }
        public static byte[] AesIv { get; private set; }
        public static AesCng AesAlgo { get; private set; }

        public static KeyHash AesHash { get; private set; }

        public static CipherMode CMode { get; private set; }

        public static EncodingType EncodeType { get; private set; }

        #endregion properties


        #region ctor

        /// <summary>
        /// static AesNet constructor
        /// </summary>
        static AesNet()
        {
            AesKeyLen = 32;
        }

        /// <summary>
        /// standard parameterless ctor of AesNet
        /// </summary>
        public AesNet() : this(Convert.FromBase64String(Constants.AES_KEY), Convert.FromBase64String(Constants.AES_IV)) { }

        public AesNet(string key, string hash, EncodingType encodeType = EncodingType.None,
            CipherMode cipherMode = CipherMode.CFB)
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
                CreateAesKeyIv(ref keyBytes, ref hashBytes);
            }
            catch (Exception e)
            {
                Area23Log.LogOriginEx("AesNet.ctor", e, 2);
                // TODO: what shell we do with the drunken sailor
                AesKey = Convert.FromBase64String(Constants.AES_KEY);
                AesIv = Encoding.UTF8.GetBytes(Constants.AES_IV);
            }

            CMode = cipherMode;
            AesAlgo = new AesCng();
            // AesAlgo.KeySize = AesKeyLen;
            AesAlgo.Key = AesKey;
            AesAlgo.IV = AesIv;
            AesAlgo.Mode = cipherMode;
            AesAlgo.Padding = PaddingMode.ISO10126;
        }

        /// <summary>
        /// AesNet constructor with default crypto parameters
        /// </summary>
        /// <param name="cparams"></param>
        public AesNet(CryptParams cparams)
        {
            if (string.IsNullOrEmpty(cparams.Key) && string.IsNullOrEmpty(cparams.Hash))
            {
                cparams.Key = Constants.AES_KEY;
                cparams.Hash = Constants.AES_IV;
            }
            byte[] keyBytes = Encoding.UTF8.GetBytes(cparams.Key);
            byte[] hashBytes = Encoding.UTF8.GetBytes(cparams.Hash);
            CMode = cparams.CMode;
            try
            {
                CreateAesKeyIv(ref keyBytes, ref hashBytes);
            }
            catch (Exception e)
            {
                Area23Log.LogOriginEx("AesNet.ctor", e, 2);
                // TODO: what shell we do with the drunken sailor
                AesKey = Convert.FromBase64String(Constants.AES_KEY);
                AesIv = Encoding.UTF8.GetBytes(Constants.AES_IV);
            }

            AesAlgo = new AesCng();
            // AesAlgo.KeySize = AesKeyLen;
            AesAlgo.Key = AesKey;
            AesAlgo.IV = AesIv;
            AesAlgo.Mode = cparams.CMode;
            AesAlgo.Padding = PaddingMode.ISO10126;
        }

        public AesNet(byte[] aesKey, byte[] aesIv, CipherMode cipherMode = CipherMode.CFB)
        {
            if (aesKey == null || aesKey.Length == 0)
                aesKey = Convert.FromBase64String(Constants.AES_KEY);
            if (aesIv == null || aesIv.Length == 0)
                aesIv = Encoding.UTF8.GetBytes(Constants.AES_IV);

            CreateAesKeyIv(ref aesKey, ref aesIv);
            CMode = cipherMode;
            AesAlgo = new AesCng();
            AesAlgo.Key = AesKey;
            AesAlgo.IV = AesIv;
            AesAlgo.Mode = cipherMode;
            AesAlgo.Padding = PaddingMode.ISO10126;

        }

        #endregion ctor

        #region ctor helpers
        protected internal void CreateAesKeyIv(ref byte[] keyBytes, ref byte[] ivBytes)
        {
            List<byte> span = new List<byte>(keyBytes);
            while (span.Count < AesKeyLen)
                span.AddRange(keyBytes);

            AesKey = new byte[AesKeyLen];
            Array.Copy(span.ToArray(), 0, AesKey, 0, AesKeyLen);
            keyBytes = new byte[AesKeyLen];
            Array.Copy(span.ToArray(), 0, keyBytes, 0, AesKeyLen);

            AesCng aesHelper = new AesCng();
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

            KeySizes[] keySizes = aesHelper.LegalBlockSizes;
        }

        #endregion ctor helpers

        #region en-/decrypt

        /// <summary>
        /// AES Encrypt by using RijndaelManaged
        /// </summary>
        /// <param name="plainData">Array of plain data byte</param>
        /// <returns>Array of encrypted data byte</returns>
        /// <exception cref="ArgumentNullException">is thrown when input enrypted <see cref="T:byte[]"/> is null or zero length</exception>
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
        /// <exception cref="ArgumentNullException">is thrown when input enrypted <see cref="T:byte[]"/> is null or zero length</exception>
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
        /// <param name="encType"><see cref="EncodingType" /></param>
        /// <returns>Base64 encoded encrypted byte[]</returns>
        public string EncryptString(string inPlainString, EncodingType encType = EncodingType.Base64)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inPlainString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = encType.GetEnCoder().EnCode(encryptedData); // Convert.ToBase64String(encryptedData);
                                                                                 // System.Text.Encoding.ASCII.GetString(encryptedData).TrimEnd('\0');

            return encryptedString;
        }

        /// <summary>
        /// Decrypts a string, that is truely a base64 encoded encrypted byte[]
        /// </summary>
        /// <param name="inCryptString">base64 encoded string from encrypted byte[]</param>
        /// <returns>plain text string (decrypted)</returns>
        public string DecryptString(string inCryptString, EncodingType encType = EncodingType.Base64)
        {
            byte[] cryptData = encType.GetEnCoder().DeCode(inCryptString); // Convert.FromBase64String(inCryptString);
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