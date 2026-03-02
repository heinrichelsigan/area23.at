using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{


    /// <summary>
    /// Des3Net native .Net triple des without bouncy castle
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.tripledes.-ctor?view=net-8.0" />
    /// <seealso href="https://www.c-sharpcorner.com/article/tripledes-encryption-and-decryption-in-c-sharp/" />
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
    [Obsolete("Des3Net pure .NET Core is obsolete (too few bits, too old) => use DESEde from https://bouncy-castle.org instead.", false)]
    public class Des3Net
    {

        #region properties

        public static byte[] DesKey { get; private set; }

        public static int DesKeyLen = 16;

        public static byte[] DesIv { get; private set; }

        public static CipherMode CMode = CipherMode.CFB;

        public static TripleDESCryptoServiceProvider Des3;

        public static ICryptoTransform CryptTrans;

        #endregion properties

        #region ctor helpers

        /// <summary>
        /// GenDes3Key ctor helper method to generate a new key as keybytes
        /// </summary>
        /// <param name="keyBytes">ref passed keybytes</param>
        protected internal void GenDes3Key(ref byte[] keyBytes)
        {
            List<byte> span = new List<byte>(keyBytes);
            while (span.Count < DesKeyLen)
                span.AddRange(keyBytes);

            DesKey = new byte[DesKeyLen];
            Array.Copy(span.ToArray(), 0, DesKey, 0, DesKeyLen);

            keyBytes = new byte[DesKeyLen];
            Array.Copy(span.ToArray(), 0, keyBytes, 0, DesKeyLen);

            return;
        }

        /// <summary>
        /// GenDes3Iv ctor helper method, that generates initalization vector IV bytes
        /// generates 
        /// </summary>
        /// <param name="keyBytes">key bytes</param>
        /// <param name="ivBytes">ref parameter to pass ivBytes</param>
        protected internal void GenDes3Iv(byte[] keyBytes, ref byte[] ivBytes)
        {
            TripleDESCryptoServiceProvider desHelper = new TripleDESCryptoServiceProvider();
            desHelper.Key = keyBytes;
            desHelper.GenerateIV();
            int iVLenght = desHelper.IV.Length;

            DesIv = new byte[iVLenght];
            if (iVLenght > DesKeyLen)
            {
                while (ivBytes.Length < iVLenght)
                    ivBytes = ivBytes.TarBytes(ivBytes);
            }

            Array.Copy(ivBytes, 0, DesIv, 0, iVLenght);

            ivBytes = new byte[iVLenght];
            Array.Copy(DesIv, 0, ivBytes, 0, iVLenght);

            desHelper.Clear();

            return;
        }

        #endregion ctor helpers

        #region ctor


        /// <summary>
        /// default standard parameterless constructor of <see cref="Des3Net"/>
        /// </summary>
        public Des3Net() : this(Convert.FromBase64String(Constants.DES3_KEY), Convert.FromBase64String(Constants.DES3_IV)) { }

        /// <summary>
        /// Des3Net ctor with key, hash and <see cref="CipherMode" />
        /// </summary>
        public Des3Net(string desKey, string hash, CipherMode cipherMode = CipherMode.CFB)
        {
            if (string.IsNullOrEmpty(desKey))
                desKey = Constants.DES3_KEY;
            if (string.IsNullOrEmpty(hash))
                hash = Constants.DES3_IV;

            byte[] key3Des = Encoding.UTF8.GetBytes(desKey);
            byte[] iv3Des = Encoding.UTF8.GetBytes(hash);
            GenDes3Key(ref key3Des);
            GenDes3Iv(DesKey, ref iv3Des);
            CMode = cipherMode;

            // MD5 md5 = new MD5CryptoServiceProvider();
            // DesKey = md5.ComputeHash(desKey);
            Des3 = new TripleDESCryptoServiceProvider();
            // Des3.KeySize = DesKeyLen;
            Des3.Key = DesKey;
            Des3.IV = DesIv;
            Des3.Mode = cipherMode;
            Des3.Padding = PaddingMode.PKCS7;
        }

        /// <summary>
        /// ctor with byte[] for key, iv and <see cref="CipherMode">default now: CFB</see>
        /// </summary>
        public Des3Net(byte[] desKey, byte[] desIv, CipherMode cipherMode = CipherMode.CFB)
        {
            if (desKey == null || desKey.Length == 0)
            {
                desKey = Convert.FromBase64String(Constants.DES3_KEY);
                desIv = Encoding.UTF8.GetBytes(Constants.DES3_IV);
            }

            // MD5 md5 = new MD5CryptoServiceProvider(); // DesKey = md5.ComputeHash(desKey);
            GenDes3Key(ref desKey);
            GenDes3Iv(DesKey, ref desIv);
            Des3 = new TripleDESCryptoServiceProvider();
            Des3.Key = DesKey;
            Des3.IV = DesIv;
            CMode = cipherMode;
            Des3.Mode = cipherMode;
            Des3.Padding = PaddingMode.PKCS7;
        }


        /// <summary>
        /// ctor with standard <see cref="CryptParams"/>
        /// </summary>
        /// <param name="cparams"><see cref="CryptParams"/></param>
        public Des3Net(CryptParams cparams) : this(cparams.Key, cparams.Hash, cparams.CMode)
        {
        }

        #endregion ctor

        #region En-/DeCrypt

        /// <summary>
        /// 3Des encrypt bytes
        /// </summary>
        /// <param name="inBytes">Hex bytes</param>
        /// <returns>byte[] encrypted bytes</returns>
        public byte[] Encrypt(byte[] inBytes)
        {
            if (inBytes == null || inBytes.Length == 0)
                throw new ArgumentNullException("inBytes");

            if (Des3 == null)
                Des3 = new TripleDESCryptoServiceProvider() { Key = DesKey, IV = DesIv, Mode = CMode, Padding = PaddingMode.PKCS7 };

            CryptTrans = Des3.CreateEncryptor();

            byte[] cryptedBytes = CryptTrans.TransformFinalBlock(inBytes, 0, inBytes.Length);
            Des3.Clear();

            return cryptedBytes;
        }

        /// <summary>
        /// 3Des decrypt bytes
        /// </summary>
        /// <param name="cipherBytes">Hex bytes encrypted</param>
        /// <returns>byte[] decrypted bytes</returns>
        public byte[] Decrypt(byte[] cipherBytes)
        {
            // Check arguments. 
            if (cipherBytes == null || cipherBytes.Length <= 0)
                throw new ArgumentNullException("cipherBytes");

            if (Des3 == null)
                Des3 = new TripleDESCryptoServiceProvider() { Key = DesKey, IV = DesIv, Mode = CMode, Padding = PaddingMode.Zeros };

            CryptTrans = Des3.CreateDecryptor();

            byte[] decryptedBytes = CryptTrans.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            Des3.Clear();

            // return decrypted byte[]
            return decryptedBytes;
        }


        #endregion En-/DeCrypt

        #region EnDeCryptString

        /// <summary>
        /// 3Des encrypt string
        /// </summary>
        /// <param name="inString">string in plain text</param>
        /// <returns>Base64 encoded encrypted byte array</returns>
        public string EncryptString(string inString)
        {
            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(inString);
            byte[] encryptedBytes = Encrypt(inBytes);
            string encryptedText = Convert.ToBase64String(encryptedBytes);
            return encryptedText;
        }

        /// <summary>
        /// 3Des decrypts string
        /// </summary>
        /// <param name="cipherText">Base64 encoded encrypted byte[]</param>
        /// <returns>plain text string</returns>
        public string DecryptString(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = Decrypt(cipherBytes);
            string plaintext = System.Text.Encoding.UTF8.GetString(decryptedBytes);
            return plaintext;
        }

        #endregion EnDeCryptString       
    }

}
