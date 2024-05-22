﻿using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;


namespace Area23.At.Mono.Util.SymChiffer
{
    /// <summary>
    /// Generic CryptBounceCastle Encryption / Decryption class
    /// supports <see cref="Org.BouncyCastle.Crypto.Engines.CamelliaEngine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.Gost28147Engine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.RC2Engine"/>,
    /// <see cref="Org.BouncyCastle.Crypto.Engines.RC532Engine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.RC6Engine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.RijndaelEngine">RijndaelEngine is standard AES</see>, 
    /// <see cref="Org.BouncyCastle.Crypto.Engines.SkipjackEngine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.TeaEngine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.TnepresEngine"/>,
    /// <see cref="Org.BouncyCastle.Crypto.Engines.XteaEngine"/>, ... and many more
    /// </summary>
    public class CryptBounceCastle
    {
        public byte[] Key { get; private set; }
        public byte[] Iv { get; private set; }
        public int Size { get; private set; }
        public IBlockCipher BlockCipher { get; private set; }
        public IBlockCipherPadding BlockCipherPadding { get; private set; }
        public static string Mode { get; private set; }

        /// <summary>
        /// Generic CryptBounceCastle constructor
        /// </summary>
        /// <param name="blockCipher">Base symmetric key block cipher interface, pass instance to constructor, e.g. 
        /// <code>CryptBounceCastle cryptCastle = new CryptBounceCastle(new Org.BouncyCastle.Crypto.Engines.CamelliaEngine());</code></param>
        public CryptBounceCastle(IBlockCipher blockCipher)
        {
            BlockCipher = (blockCipher == null) ? new AesEngine() : blockCipher;
            BlockCipherPadding = new ZeroBytePadding();
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            byte[] key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
            Key = new byte[32];
            Iv = new byte[32];
            Array.Copy(iv, Iv, 32);
            Array.Copy(key, Key, 32);
            Size = 256;
            Mode = "ECB";
        }

        /// <summary>
        /// Generic CryptBounceCastle Encrypt member function
        /// difference between out parameter encryptedData and return value, are 2 different encryption methods, but with the same result at the end
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <param name="encryptedData">encrypted data <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public byte[] Encrypt(byte[] plainData, out byte[] encryptedData)
        {
            var cipher = BlockCipher;

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

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

            encryptedData = cipherMode.ProcessBytes(plainData);

            int outputSize = cipherMode.GetOutputSize(plainData.Length);
            byte[] cipherData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(plainData, 0, plainData.Length, cipherData, 0);
            cipherMode.DoFinal(cipherData, result);          
            
            return cipherData;
        }

        /// <summary>
        /// Generic CryptBounceCastle Decrypt member function
        /// difference between out parameter decryptedData and return value, are 2 different decryption methods, but with the same result at the end
        /// </summary>
        /// <param name="cipherData">encrypted <see cref="byte[]">bytes</see></param>
        /// <param name="decryptedData">decrypted plain byte[] data</param>
        /// <returns>decrypted plain byte[] data</returns>
        public byte[] Decrypt(byte[] cipherData, out byte[] decryptedData)
        {
            var cipher = BlockCipher;

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), BlockCipherPadding);
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), BlockCipherPadding);
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), BlockCipherPadding);

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

            decryptedData = cipherMode.ProcessBytes(cipherData);

            int outputSize = cipherMode.GetOutputSize(cipherData.Length);
            byte[] plainData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(cipherData, 0, cipherData.Length, plainData, 0);
            cipherMode.DoFinal(plainData, result);

            return plainData; 
        }

        #region EnDecryptString

        /// <summary>
        /// Generic CryptBounceCastle Encrypt String method
        /// </summary>
        /// <param name="inString">plain string to encrypt</param>
        /// <returns>base64 encoded encrypted string</returns>
        public string EncryptString(string inString)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inString);
            byte[] encryptedData;
            byte[] cipherData = this.Encrypt(plainTextData, out encryptedData);
            string encryptedString = Convert.ToBase64String(encryptedData);
            
            return encryptedString;
        }

        /// <summary>
        /// Generic CryptBounceCastle Decrypt String method
        /// </summary>
        /// <param name="inCryptString">base64 encrypted string</param>
        /// <returns>plain text decrypted string</returns>
        public string DecryptString(string inCryptString)
        {
            byte[] cryptData = Convert.FromBase64String(inCryptString);
            byte[] decryptedData;
            byte[] plainData = Decrypt(cryptData, out decryptedData);
            string plainTextString = System.Text.Encoding.ASCII.GetString(plainData).TrimEnd('\0');

            return plainTextString;
        }
        
        #endregion EnDecryptString

    }

}