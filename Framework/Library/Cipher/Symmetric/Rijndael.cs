using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Cipher.Symmetric
{

    /// <summary>
    /// static class Fish2, that implements 2FISH static Encrypt & Decrypt members
    /// </summary>
    public static class Rijndael
    {

        #region fields

        private static string privateKey = string.Empty;

        private static string userHostIpAddress = string.Empty;

        #endregion fields

        #region Properties

        public static byte[] RijndaelKey { get; private set; }
        public static byte[] RijndaelIv { get; private set; }

        public static int Size { get; private set; }
        public static string Mode { get; private set; }
        public static IBlockCipherPadding BlockCipherPadding { get; private set; }

        #endregion Properties

        #region ctor_gen

        /// <summary>
        /// static Fish2 constructor
        /// </summary>
        static Rijndael()
        {
            byte[] key = Convert.FromBase64String(ResReader.GetValue(Constants.AES_KEY));
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));
            RijndaelKey = new byte[32];
            RijndaelIv = new byte[16];
            Array.Copy(iv, RijndaelIv, 16);
            Array.Copy(key, RijndaelKey, 32);
            Size = 256;
            Mode = "ECB";
            BlockCipherPadding = new ZeroBytePadding();

            // TwoFishGenWithKey(string.Empty, true);
        }

        /// <summary>
        /// RijndaelGenWithKey - Generate new <see cref="Rijndael"/> with secret key
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="init">init <see cref="Fish2"/> first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool RijndaelGenWithKey(string secretKey = "", string userHostAddr = "", bool init = true)
        {
            byte[] key;
            byte[] iv = new byte[32];

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
                    userHostIpAddress = userHostAddr;
                    key = CryptHelper.GetUserKeyBytes(secretKey, userHostAddr, 32);
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));
                }

                RijndaelKey = new byte[32];
                RijndaelIv = new byte[16];
                Array.Copy(key, RijndaelKey, 32);
                Array.Copy(iv, RijndaelIv, 16);
            }

            return true;
        }

        #endregion ctor_gen

        #region EncryptDecryptBytes

        /// <summary>
        /// Rijndael Encrypt member function
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public static byte[] Encrypt(byte[] plainData)
        {
            var cipher = new AesLightEngine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(RijndaelKey, 0, 32);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, RijndaelIv);

            if (Mode == "ECB")
            {
                cipherMode.Init(true, keyParam);
            }
            else
            {
                cipherMode.Init(true, keyParamIV);
            }

            int outputSize = cipherMode.GetOutputSize(plainData.Length);
            byte[] cipherTextData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(plainData, 0, plainData.Length, cipherTextData, 0);
            cipherMode.DoFinal(cipherTextData, result);

            return cipherTextData;
        }

        /// <summary>
        /// Rijndael Decrypt member function
        /// </summary>
        /// <param name="cipherData">encrypted <see cref="byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public static byte[] Decrypt(byte[] cipherData)
        {
            var cipher = new AesLightEngine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(RijndaelKey, 0, 32);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, RijndaelIv);
            // Decrypt
            if (Mode == "ECB")
            {
                cipherMode.Init(false, keyParam);
            }
            else
            {
                cipherMode.Init(false, keyParamIV);
            }

            int outputSize = cipherMode.GetOutputSize(cipherData.Length);
            byte[] plainData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(cipherData, 0, cipherData.Length, plainData, 0);
            cipherMode.DoFinal(plainData, result);

            return plainData; // System.Text.Encoding.ASCII.GetString(pln).TrimEnd('\0');
        }

        #endregion EncryptDecryptBytes

        #region EnDecryptString

        /// <summary>
        /// 2FISH Encrypt String method
        /// </summary>
        /// <param name="inString">plain string to encrypt</param>
        /// <returns>base64 encoded encrypted string</returns>
        public static string EncryptString(string inString)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = Convert.ToBase64String(encryptedData);

            return encryptedString;
        }


        /// <summary>
        /// 2FISH Decrypt String method
        /// </summary>
        /// <param name="inCryptString">base64 encrypted string</param>
        /// <returns>plain text decrypted string</returns>
        public static string DecryptString(string inCryptString)
        {
            byte[] cryptData = Convert.FromBase64String(inCryptString);
            byte[] plainTextData = Decrypt(cryptData);
            string plainTextString = System.Text.Encoding.ASCII.GetString(plainTextData).TrimEnd('\0');

            return plainTextString;
        }

        #endregion EnDecryptString

    }

}
