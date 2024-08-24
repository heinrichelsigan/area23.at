using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Crypto.Engines;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Windows.Interop;
using System.Security.Cryptography;

namespace Area23.At.Mono.Util.SymChiffer
{
    public static class Serpent
    {
        private static string privateKey = string.Empty;

        internal static byte[] SerpentKey { get; private set; }
        internal static byte[] SerpentIv { get; private set; }

        internal static int Size { get; private set; }
        internal static string Mode { get; private set; }
        internal static IBlockCipherPadding BlockCipherPadding { get; private set; }

        /// <summary>
        /// static constructor for serpent encryption
        /// </summary>
        static Serpent()
        {
            byte[] key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            SerpentKey = new byte[16];
            SerpentIv = new byte[16];
            Array.Copy(iv, SerpentIv, 16);
            Array.Copy(key, SerpentKey, 16);
            Size = 128;
            Mode = "ECB";
            BlockCipherPadding = new ZeroBytePadding();
            SerpentGenWithKey(string.Empty, true);
        }


        /// <summary>
        /// ThreeFishGenWithKey => Generates new <see cref="Serpent"/> with secret key
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="init">init <see cref="Serpent"/> first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool SerpentGenWithKey(string secretKey = null, bool init = true)
        {
            byte[] iv = new byte[16]; // Serpent > IV <= 128 bit
            byte[] key; // = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));

            if (!init)
            {
                if ((string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(secretKey)) ||
                    (!privateKey.Equals(secretKey, StringComparison.InvariantCultureIgnoreCase)))
                    return false;
            }

            privateKey = secretKey;

            if (string.IsNullOrEmpty(secretKey))
            {
                key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
                if (SerpentIv == null || SerpentIv.Length == 0)
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            }
            else
            {
                key = Encoding.UTF8.GetByteCount(secretKey) == 32 ? Encoding.UTF8.GetBytes(secretKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                if (SerpentIv == null || SerpentIv.Length == 0)
                {
                    // iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                    RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
                    randomNumGen.GetBytes(iv, 0, iv.Length);
                }
                else
                    Array.Copy(SerpentIv, iv, 16);
            }

            SerpentKey = new byte[16];
            if (SerpentIv == null || SerpentIv.Length == 0)
            {
                SerpentIv = new byte[16];
                Array.Copy(iv, SerpentIv, 16);
            }
            Array.Copy(key, SerpentKey, 16);

            return true;
        }

        /// <summary>
        /// Serpent Encrypt member function
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public static byte[] Encrypt(byte[] plainData)
        {            
            var cipher = new SerpentEngine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(SerpentKey);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, SerpentIv);

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
        /// Serpent Decrypt member function
        /// </summary>
        /// <param name="cipherData">encrypted <see cref="byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public static byte[] Decrypt(byte[] cipherData)
        {
            var cipher = new SerpentEngine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(SerpentKey);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, SerpentIv);
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
            byte[] plainTextData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(cipherData, 0, cipherData.Length, plainTextData, 0);
            cipherMode.DoFinal(plainTextData, result);

            return plainTextData; // System.Text.Encoding.ASCII.GetString(pln).TrimEnd('\0');
        }

        #region EnDecryptString

        /// <summary>
        /// Serpent Encrypt String method
        /// </summary>
        /// <param name="inString">plain string to encrypt</param>
        /// <returns>base64 encoded encrypted string</returns>
        public static string EncryptString(string inPlainString)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inPlainString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = Convert.ToBase64String(encryptedData);

            return encryptedString;
        }

        /// <summary>
        /// Serpent Decrypt String method
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