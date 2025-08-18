using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{


    /// <summary>
    /// Serpent static class implementing Serpent symetric chiffer algorithm
    /// </summary>
    public static class Serpent
    {

        #region fields

        private static string privateKey = string.Empty;
        private static string userHash = string.Empty;

        #endregion fields

        #region Properties

        internal static byte[] SerpentKey { get; private set; }
        internal static byte[] SerpentIv { get; private set; }

        internal static int Size { get; private set; }
        internal static string Mode { get; private set; }

        internal static IBlockCipherPadding BlockCipherPadding { get; private set; }

        #endregion Properties

        #region ctor_gen

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
            // SerpentGenWithKey(string.Empty, true);
        }

        /// <summary>
        /// SerpentGenWithKey => Generates new <see cref="Serpent"/> with secret key
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="usrHash">user key hash</param>
        /// <param name="init">init <see cref="Serpent"/> first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool SerpentGenWithKey(string secretKey = "", string usrHash = "", bool init = true)
        {
            byte[] iv = new byte[16]; // Serpent > IV <= 128 bit
            byte[] key = new byte[16];

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
                    key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                }
                else
                {
                    privateKey = secretKey;
                    userHash = usrHash;
                    key = CryptHelper.GetUserKeyBytes(secretKey, userHash, 16);
                    // iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                    iv = CryptHelper.GetUserKeyBytes(userHash, secretKey, 16);
                }

                SerpentKey = new byte[16];
                SerpentIv = new byte[16];
                Array.Copy(key, SerpentKey, 16);
                Array.Copy(iv, SerpentIv, 16);
            }


            return true;
        }

        #endregion ctor_gen

        #region EncryptDecryptBytes

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

            cipherMode.Init(true, keyParam);
            //if (Mode == "ECB")
            //    cipherMode.Init(true, keyParam);
            //else
            //    cipherMode.Init(true, keyParamIV);

            int outputSize = cipherMode.GetOutputSize(plainData.Length);
            byte[] cipherTextData = new byte[outputSize];
            try
            {
                int result = cipherMode.ProcessBytes(plainData, 0, plainData.Length, cipherTextData, 0);
                cipherMode.DoFinal(cipherTextData, result);
            }
            catch (Exception exEncrypt)
            {
                Area23Log.LogOriginMsgEx("CryptBounceCastle", $"CryptBounceCastle {cipherMode.AlgorithmName}: Exceptíon on encrypting final block", exEncrypt);
                try
                {
                    cipherTextData = new byte[outputSize];
                    cipherTextData = cipherMode.ProcessBytes(plainData, 0, plainData.Length);
                }
                catch (Exception exEncrypt2)
                {
                    Area23Log.LogOriginMsgEx("CryptBounceCastle", $"CryptBounceCastle {cipherMode.AlgorithmName}: Exceptíon on 2x encrypting final block", exEncrypt2);
                    cipherTextData = new byte[outputSize];
                    cipherTextData = cipherMode.ProcessBytes(plainData);
                }
            }

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
            cipherMode.Init(false, keyParam);
            //if (Mode == "ECB")
            //    cipherMode.Init(false, keyParam);
            //else
            //    cipherMode.Init(false, keyParamIV);

            int outputSize = cipherMode.GetOutputSize(cipherData.Length);
            byte[] plainTextData = new byte[outputSize];
            try
            {
                int result = cipherMode.ProcessBytes(cipherData, 0, cipherData.Length, plainTextData, 0);
                cipherMode.DoFinal(plainTextData, result);
            }
            catch (Exception exDecrypt)
            {
                Area23Log.LogOriginMsgEx("CryptBounceCastle", $"CryptBounceCastle {cipherMode.AlgorithmName}: Exceptíon on decrypting final block", exDecrypt);
                try
                {
                    plainTextData = new byte[outputSize];
                    plainTextData = cipherMode.ProcessBytes(cipherData, 0, cipherData.Length);
                }
                catch (Exception exDecrypt2)
                {
                    Area23Log.LogOriginMsgEx("CryptBounceCastle", $"CryptBounceCastle {cipherMode.AlgorithmName}: Exceptíon on 2x decrypting final block", exDecrypt2);
                    plainTextData = new byte[outputSize];
                    plainTextData = cipherMode.ProcessBytes(cipherData);
                }
            }

            return plainTextData; // Encoding.ASCII.GetString(pln).TrimEnd('\0');
        }

        #endregion EncryptDecryptBytes

        #region EnDecryptString

        /// <summary>
        /// Serpent Encrypt String method
        /// </summary>
        /// <param name="inString">plain string to encrypt</param>
        /// <returns>base64 encoded encrypted string</returns>
        public static string EncryptString(string inPlainString)
        {
            byte[] plainTextData = EnDeCodeHelper.GetBytes(inPlainString);
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
            string plainTextString = EnDeCodeHelper.GetString(plainTextData).TrimEnd('\0');

            return plainTextString;
        }

        #endregion EnDecryptString

    }

}
