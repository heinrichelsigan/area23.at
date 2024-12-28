using Area23.At.Framework.Library.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Core.Util;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric
{

    [Obsolete("Fish3 is implemented via BouncyCastle", false)]
    public static class Fish3
    {

        #region fields

        private static string privateKey = string.Empty;
        private static string userHash = string.Empty;

        #endregion fields

        #region Properties

        internal static byte[] FishKey { get; private set; }
        internal static byte[] FishIv { get; private set; }

        internal static int Size { get; private set; }
        internal static string Mode { get; private set; }

        internal static IBlockCipherPadding BlockCipherPadding { get; private set; }

        #endregion Properties

        #region ctor_gen

        /// <summary>
        /// static Fish3 constructor
        /// </summary>
        static Fish3()
        {
            byte[] key = new byte[32];
            byte[] iv = new byte[32];
            key = CryptHelper.GetUserKeyBytes(ResReader.GetValue(Constants.BOUNCEK), ResReader.GetValue(Constants.BOUNCE4), 32);
            iv = CryptHelper.GetUserKeyBytes(ResReader.GetValue(Constants.BOUNCE4), ResReader.GetValue(Constants.BOUNCEK), 32);
            FishKey = new byte[32];
            FishIv = new byte[32];
            Array.Copy(iv, FishIv, 32);
            Array.Copy(key, FishKey, 32);
            Size = 256;
            Mode = "ECB";
            BlockCipherPadding = new ZeroBytePadding();
            // ThreeFishGenWithKey(string.Empty, true);
        }


        /// <summary>
        /// Fish3GenWithKey => Generates new <see cref="ThreeFish"/> with secret key
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="usrHash">user key hash</param>
        /// <param name="init">init <see cref="ThreeFish"/> first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        [Obsolete("Fish3GenWithKeyHash is implemented via BouncyCastle", false)]
        public static bool Fish3GenWithKeyHash(string secretKey = "", string usrHash = "", bool init = true)
        {
            byte[] key = new byte[32];
            byte[] iv = new byte[32]; // 3FISH > IV > 128 bit

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
                    key = CryptHelper.GetUserKeyBytes(ResReader.GetValue(Constants.BOUNCEK), ResReader.GetValue(Constants.BOUNCE4), 32);
                    iv = CryptHelper.GetUserKeyBytes(ResReader.GetValue(Constants.BOUNCE4), ResReader.GetValue(Constants.BOUNCEK), 32);
                }
                else
                {
                    privateKey = secretKey;
                    userHash = usrHash;
                    key = CryptHelper.GetUserKeyBytes(secretKey, userHash, 32);
                    iv = CryptHelper.GetUserKeyBytes(userHash, secretKey, 32);
                    // iv = CryptHelper.GetUserKeyBytes(ResReader.GetValue(Constants.BOUNCE4), ResReader.GetValue(Constants.BOUNCEK), 32);
                }

                FishKey = new byte[32];
                FishIv = new byte[32];
                Array.Copy(key, FishKey, 32);
                Array.Copy(iv, FishIv, 32);
            }

            return true;
        }

        #endregion ctor_gen

        #region EncryptDecryptBytes

        /// <summary>
        /// Fish3 Encrypt member function
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public static byte[] Encrypt(byte[] plainData)
        {
            var cipher = new TwofishEngine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(FishKey);
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
        /// Fish3 Decrypt member function
        /// </summary>
        /// <param name="cipherData">encrypted <see cref="byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public static byte[] Decrypt(byte[] cipherData)
        {
            var cipher = new TwofishEngine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(FishKey);
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

            int outputSize = (int)cipherMode.GetOutputSize(cipherData.Length); //  cipherMode.GetUpdateOutputSize(cipherData.Length));
            byte[] plainTextData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(cipherData, 0, cipherData.Length, plainTextData, 0);
            cipherMode.DoFinal(plainTextData, result);

            return plainTextData; // Encoding.ASCII.GetString(pln).TrimEnd('\0');
        }

        #endregion EncryptDecryptBytes

        #region EnDecryptString

        /// <summary>
        /// 3FISH Encrypt String method
        /// </summary>
        /// <param name="inString">plain string to encrypt</param>
        /// <returns>base64 encoded encrypted string</returns>
        public static string EncryptString(string inString, EncodingType encType = EncodingType.Base64)
        {
            byte[] plainTextData = EnDeCoder.GetBytes(inString);
            byte[] encryptedBytes = Encrypt(plainTextData);
            string encryptedString = DeEnCoder.EncodeBytes(encryptedBytes, encType);

            return encryptedString;
        }

        /// <summary>
        /// 3FISH Decrypt String method
        /// </summary>
        /// <param name="inCryptString">base64 encrypted string</param>
        /// <returns>plain text decrypted string</returns>
        public static string DecryptString(string inCryptString, EncodingType encType = EncodingType.Base64)
        {
            byte[] cipherBytes = DeEnCoder.DecodeText(inCryptString, encType);
            byte[] plainTextData = Decrypt(cipherBytes);
            string plainTextString = EnDeCoder.GetString(plainTextData).TrimEnd('\0');

            return plainTextString;
        }

        #endregion EnDecryptString

    }

}
