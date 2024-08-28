using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Symchiffer;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;

namespace Area23.At.Mono.Util.SymChiffer
{
    /// <summary>
    /// static class for standard DES encryption / decryption methods
    /// </summary>
    public static class DesEde
    {
        public static byte[] Key { get; private set; }
        public static byte[] Iv { get; private set; }
        public static int Size { get; private set; }
        public static string Mode { get; private set; }
        public static IBlockCipherPadding BlockCipherPadding { get; private set; }

        /// <summary>
        /// static constructor for simple DES (data encryption standard)
        /// </summary>
        static DesEde()
        {
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
            byte[] key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            BlockCipherPadding = new ZeroBytePadding();
            Key = new byte[16];
            Iv = new byte[16];
            Array.Copy(iv, Iv, 16);
            Array.Copy(key, Key, 16);
            Size = 128;
            Mode = "ECB";            
        }

        /// <summary>
        /// DesEde Encrypt member function
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public static byte[] Encrypt(byte[] plainData)
        {            
            var cipher = new DesEdeEngine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            CipherKeyGenerator keyGen = new CipherKeyGenerator();
            keyGen.Init(new KeyGenerationParameters(new SecureRandom(), Size));
            byte[] ekey = keyGen.GenerateKey();
            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(Key);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, Iv);

            if (Mode == "ECB")
            {
                cipherMode.Init(true, keyParam);
            }
            else
            {
                cipherMode.Init(true, keyParamIV);
            }

            int outputSize = cipherMode.GetOutputSize(plainData.Length);
            byte[] ciphertData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(plainData, 0, plainData.Length, ciphertData, 0);
            cipherMode.DoFinal(ciphertData, result);

            return ciphertData;
        }

        /// <summary>
        /// DesEde Decrypt member function
        /// </summary>
        /// <param name="cipherData">encrypted <see cref="byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public static byte[] Decrypt(byte[] cipherData)
        {
            var cipher = new DesEdeEngine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

            CipherKeyGenerator keyGen = new CipherKeyGenerator();
            keyGen.Init(new KeyGenerationParameters(new SecureRandom(), Size));
            KeyParameter keyParameter = keyGen.GenerateKeyParameter();
            byte[] key = keyParameter.GetKey();
            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(Key);
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


            return plainData; 
        }

        #region EnDecryptString

        /// <summary>
        /// DesEde Encrypt String method
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
        /// DesEde Decrypt String method
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