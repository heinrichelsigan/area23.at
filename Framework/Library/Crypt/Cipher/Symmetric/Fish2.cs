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
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Util;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{
    [Obsolete("Fish2 is implemented via BouncyCastle", true)]
    public static class Fish2
    {

        #region fields

        private static string privateKey = string.Empty;

        private static string userHash = string.Empty;

        #endregion fields

        #region Properties

        public static byte[] FishKey { get; private set; }
        public static byte[] FishIv { get; private set; }

        public static int Size { get; private set; }
        public static string Mode { get; private set; }
        public static IBlockCipherPadding BlockCipherPadding { get; private set; }

        #endregion Properties

        #region ctor_gen

        /// <summary>
        /// static Fish2 constructor
        /// </summary>
        static Fish2()
        {
            byte[] key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            FishKey = new byte[16];
            FishIv = new byte[16];
            Array.Copy(iv, FishIv, 16);
            Array.Copy(key, FishKey, 16);
            Size = 128;
            Mode = "ECB";
            BlockCipherPadding = new ZeroBytePadding();

            // TwoFishGenWithKey(string.Empty, true);
        }

        /// <summary>
        /// Fish2GenWithKey - Generate new <see cref="Fish2"/> with secret key
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="usrHash">user key hash</param>
        /// <param name="init">init <see cref="Fish2"/> first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        [Obsolete("Fish2GenWithKey is implemented via BouncyCastle", true)]
        public static bool Fish2GenWithKey(string secretKey = "", string usrHash = "", bool init = true)
        {
            byte[] key;
            byte[] iv = new byte[16];

            if (!init)
            {
                if (string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(secretKey) ||
                    !privateKey.Equals(secretKey, StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            if (init)
            {
                if (string.IsNullOrEmpty(secretKey))
                {
                    privateKey = string.Empty;
                    key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                }
                else
                {
                    privateKey = secretKey;
                    userHash = usrHash;
                    key = CryptHelper.GetUserKeyBytes(secretKey, userHash, 16);
                    iv = CryptHelper.GetUserKeyBytes(userHash, secretKey, 16);
                    // iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                }

                FishKey = new byte[16];
                FishIv = new byte[16];
                Array.Copy(key, FishKey, 16);
                Array.Copy(iv, FishIv, 16);
            }

            return true;
        }

        #endregion ctor_gen

        #region EncryptDecryptBytes

        /// <summary>
        /// Fish2 Encrypt member function
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public static byte[] Encrypt(byte[] plainData)
        {
            var cipher = new TwofishEngine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            KeyParameter keyParam = new KeyParameter(FishKey, 0, 16);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, FishIv);

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
        /// Fish2 Decrypt member function
        /// </summary>
        /// <param name="cipherData">encrypted <see cref="byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public static byte[] Decrypt(byte[] cipherData)
        {
            var cipher = new TwofishEngine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            KeyParameter keyParam = new KeyParameter(FishKey, 0, 16);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, FishIv);
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

            return plainData; // Encoding.ASCII.GetString(pln).TrimEnd('\0');
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
            byte[] plainTextData = EnDeCoder.GetBytes(inString);
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
            string plainTextString = EnDeCoder.GetString(plainTextData).TrimEnd('\0');

            return plainTextString;
        }

        #endregion EnDecryptString

    }

}
