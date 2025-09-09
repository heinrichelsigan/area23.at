using Area23.At.Framework.Core.Net.NameService;
using Area23.At.Framework.Core.Static;
using System.Security.Cryptography;
using System.Text;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{
    /// <summary>
    /// Des3Net native .Net triple des without bouncy castle
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.tripledes.-ctor?view=net-8.0" />
    /// <seealso cref="https://www.c-sharpcorner.com/article/tripledes-encryption-and-decryption-in-c-sharp/ "/>
    /// </summary>
    public class Des3Net
    {

        #region properties

        public static byte[] DesKey { get; private set; }
        public static int DesKeyLen { get; private set; }
        public static byte[] DesIv { get; private set; }

        public static TripleDESCryptoServiceProvider Des3;

        public static ICryptoTransform CryptTrans;

        #endregion properties

        #region ctor helpers

        protected internal void Gen3DesKey(ref byte[] keyBytes)
        {
            //var des3Tmp = new TripleDESCryptoServiceProvider();
            //for (int lz = 0; lz < des3Tmp.LegalKeySizes.Length; lz++)
            //{
            //    KeySizes keySze = des3Tmp.LegalKeySizes[lz];
            //    if (keySze.MinSize >= 8 && keySze.MinSize <= 256)
            //    {
            //        switch (keySze.MaxSize)
            //        {
            //            case 8:
            //            case 16:
            //            case 24:
            //            case 32:
            //                DesKeyLen = keySze.MaxSize;
            //                break;
            //            case 48:
            //            case 64:
            //            case 128:
            //            case 192:
            //            case 256:
            //                DesKeyLen = (DesKeyLen < 0) ? keySze.MaxSize : DesKeyLen;
            //                break;
            //            default:
            //                DesKeyLen = (DesKeyLen < 0) ? keySze.MaxSize : DesKeyLen;
            //                break;
            //        }
            //    }
            //}

            List<byte> span = new List<byte>(keyBytes);
            while (span.Count < DesKeyLen)
                span.AddRange(keyBytes);

            DesKey = new byte[DesKeyLen];
            Array.Copy(span.ToArray(), 0, DesKey, 0, DesKeyLen);

            keyBytes = new byte[DesKeyLen];
            Array.Copy(span.ToArray(), 0, keyBytes, 0, DesKeyLen);

            return;
        }

        protected internal void Gen3DesIv(byte[] keyBytes, ref byte[] ivBytes)
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
        /// static constructor
        /// </summary>
        static Des3Net()
        {
            DesKeyLen = 16;
        }

        public Des3Net() : this(Convert.FromBase64String(Constants.DES3_KEY), Convert.FromBase64String(Constants.DES3_IV)) { }

        public Des3Net(string desKey, string desIv)
        {
            if (string.IsNullOrEmpty(desKey))
                desKey = Constants.DES3_KEY;
            if (string.IsNullOrEmpty(desIv))
                desIv = Constants.DES3_IV;

            byte[] key3Des = Encoding.UTF8.GetBytes(desKey);
            byte[] iv3Des = Encoding.UTF8.GetBytes(desIv);
            Gen3DesKey(ref key3Des);
            Gen3DesIv(DesKey, ref iv3Des);

            // MD5 md5 = new MD5CryptoServiceProvider();
            // DesKey = md5.ComputeHash(desKey);
            Des3 = new TripleDESCryptoServiceProvider();
            // Des3.KeySize = DesKeyLen;
            Des3.Key = DesKey;
            Des3.IV = DesIv;
            Des3.Mode = CipherMode.ECB;
            Des3.Padding = PaddingMode.Zeros;
        }

        public Des3Net(byte[] desKey, byte[] desIv)
        {
            if (desKey == null || desKey.Length == 0)
            {
                desKey = Convert.FromBase64String(Constants.DES3_KEY);
                desIv = Encoding.UTF8.GetBytes(Constants.DES3_IV);
            }

            // MD5 md5 = new MD5CryptoServiceProvider(); // DesKey = md5.ComputeHash(desKey);
            Gen3DesKey(ref desKey);
            Gen3DesIv(DesKey, ref desIv);
            Des3 = new TripleDESCryptoServiceProvider();
            Des3.Key = DesKey;
            Des3.IV = DesIv;
            Des3.Mode = CipherMode.ECB;
            Des3.Padding = PaddingMode.Zeros;
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
				Des3 = new TripleDESCryptoServiceProvider() { Key = DesKey, IV = DesIv, Mode = CipherMode.ECB, Padding = PaddingMode.Zeros };
            
			CryptTrans = Des3.CreateEncryptor();
			
            byte[] cryptedBytes = CryptTrans.TransformFinalBlock(inBytes, 0, inBytes.Length);            
            Des3.Clear();

            return cryptedBytes;
        }

        /// <summary>
        /// 3Des decrypt bytes
        /// </summary>
        /// <param name="inBytes">Hex bytes encrypted</param>
        /// <returns>byte[] decrypted bytes</returns>
        public byte[] Decrypt(byte[] cipherBytes)
        {
            // Check arguments. 
            if (cipherBytes == null || cipherBytes.Length <= 0)
                throw new ArgumentNullException("cipherBytes");

			if (Des3 == null)
				Des3 = new TripleDESCryptoServiceProvider() { Key = DesKey, IV = DesIv, Mode = CipherMode.ECB, Padding = PaddingMode.Zeros };         
            
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
            // System.Text.Encoding.UTF8.GetString(encryptedBytes).TrimEnd('\0');
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
