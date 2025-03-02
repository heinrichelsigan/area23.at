using Area23.At.Framework.Core.Crypt.EnDeCoding;
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
using Area23.At.Framework.Core.Static;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// static class RC564, that implements RC564 static Encrypt & Decrypt members
    /// </summary>
    public static class RC564
    {

        #region fields

        private static string privateKey = string.Empty;

        private static string userHash = string.Empty;

        #endregion fields

        #region Properties

        public static byte[] Key { get; private set; }
        public static byte[] Iv { get; private set; }

        public static int Size { get; private set; }
        public static string Mode { get; private set; }
        public static IBlockCipherPadding BlockCipherPadding { get; private set; }

        #endregion Properties

        #region ctor_gen

        /// <summary>
        /// static RC564 constructor
        /// </summary>
        static RC564()
        {
            byte[] key = Convert.FromBase64String(ResReader.GetValue(Constants.AES_KEY));
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));
            Key = new byte[32];
            Iv = new byte[32];
            Array.Copy(key, Iv, 32);
            Array.Copy(key, Key, 32);
            Size = 256;
            Mode = "ECB";
            BlockCipherPadding = new ZeroBytePadding();

            // TwoFishGenWithKey(string.Empty, true);
        }

        /// <summary>
        /// RC564GenWithKey - Generates a new <see cref="RC564"/> with secret key
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="usrHash">user key hash</param>
        /// <param name="init">init <see cref="RC564"/> first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool RC564GenWithKey(string secretKey = "heinrich.elsigan@area23.at", string usrHash = "https://cqrxs.eu/net", bool init = true)
        {
            byte[] key;
            byte[] iv = new byte[64];

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
                    userHash = usrHash;
                    key = CryptHelper.GetUserKeyBytes(secretKey, userHash, 32);
                    // iv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));
                    iv = CryptHelper.GetUserKeyBytes(userHash, secretKey, 32);
                }

                Key = new byte[32];
                Iv = new byte[32];
                Array.Copy(key, Key, 32);
                Array.Copy(iv, Iv, 32);
            }

            return true;
        }

        #endregion ctor_gen

        #region EncryptDecryptBytes

        /// <summary>
        /// RC564 Encrypt with <see cref="RC564Engine"/>
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public static byte[] Encrypt(byte[] plainData)
        {
            byte[] plainScratched = Area23.At.Framework.Core.Crypt.EnDeCoding.EnDeCodeHelper.GetBytesFromBytes(plainData);

            var cipher = new RC564Engine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.RC5Parameters(Key, 2);
            ICipherParameters keyParamIV = new Org.BouncyCastle.Crypto.Parameters.ParametersWithIV(keyParam, Iv);

            if (Mode == "ECB")
            {
                cipherMode.Init(true, keyParam);
            }
            else
            {
                cipherMode.Init(true, keyParamIV);
            }

            int outputSize = cipherMode.GetOutputSize(plainScratched.Length);
            byte[] cipherTextData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(plainScratched, 0, plainScratched.Length, cipherTextData, 0);
            cipherMode.DoFinal(cipherTextData, result);

            return cipherTextData;
        }

        /// <summary>
        /// RC564 Decrypt with <see cref="RC564Engine"/>
        /// </summary>
        /// <param name="cipherData">encrypted <see cref="byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public static byte[] Decrypt(byte[] cipherData)
        {
            var cipher = new RC564Engine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.RC5Parameters(Key, 2);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, Iv);
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

            byte[] plainShrinken = Area23.At.Framework.Core.Crypt.EnDeCoding.EnDeCodeHelper.GetBytesTrimNulls(plainData);
            return plainShrinken; // Encoding.ASCII.GetString(pln).TrimEnd('\0');
        }

        #endregion EncryptDecryptBytes

        #region EnDecryptString

        /// <summary>
        /// RC564 Encrypt String method
        /// </summary>
        /// <param name="inString">plain string to encrypt</param>
        /// <returns>base64 encoded encrypted string</returns>
        public static string EncryptString(string inString)
        {
            byte[] plainTextData = EnDeCodeHelper.GetBytes(inString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = Convert.ToBase64String(encryptedData);

            return encryptedString;
        }


        /// <summary>
        /// RC564 Decrypt String method
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
