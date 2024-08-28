using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Symchiffer;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Crypto.Engines;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Interop;
using System.Security.Cryptography;

namespace Area23.At.Mono.Util.SymChiffer
{
    public static class RC564
    {
        private static string privateKey = string.Empty;

        internal static byte[] Key { get; private set; }
        internal static byte[] Iv { get; private set; }
        internal static int Size { get; private set; }
        internal static string Mode { get; private set; } 

        /// <summary>
        /// static ctor
        /// </summary>
        static RC564()
        {
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            byte[] key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));            
            Key = new byte[16];
            Iv = new byte[16];
            Array.Copy(iv, Iv, 16);
            Array.Copy(key, Key, 16);
            Size = 64;
            Mode = "CBC"; 
        }

        /// <summary>
        /// RC564GenWithKey => Generates new <see cref="RC564"/> with secret key
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="init">init <see cref="ThreeFish"/> first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool RC564GenWithKey(string secretKey = "", bool init = true)
        {
            byte[] key = new byte[16];
            byte[] iv = new byte[16]; // RC564 => IV = 64 bit

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
                    key = Encoding.UTF8.GetByteCount(secretKey) == 16 ? Encoding.UTF8.GetBytes(secretKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secretKey));                    
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                }

                Key = new byte[16];
                Iv = new byte[16];
                Array.Copy(key, Key, 16);
                Array.Copy(iv, Iv, 16);
            } 

            return true;
        }



        /// <summary>
        /// RC564 Encrypt member function
        /// </summary>
        /// <param name="plainTextData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public static byte[] Encrypt(byte[] plainTextData)
        {
            var cipher = new RC564Engine();
            
            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new ZeroBytePadding());
            
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new ZeroBytePadding());
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), new ZeroBytePadding());

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(Key, 0, 16);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, Iv);

            if (Mode == "ECB")
            {
                cipherMode.Init(true, keyParam);
            }
            else
            {
                cipherMode.Init(true, keyParamIV);
            }

            byte[] chipherData = cipherMode.ProcessBytes(plainTextData);
            
            return chipherData;
        }

        /// <summary>
        /// RC564 Decrypt member function
        /// </summary>
        /// <param name="cipherTextData">encrypted <see cref="byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public static byte[] Decrypt(byte[] cipherTextData)
        {
            var cipher = new RC564Engine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new ZeroBytePadding());
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new ZeroBytePadding());
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), new ZeroBytePadding());

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(Key, 0, 16);
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

            byte[] plainData = cipherMode.ProcessBytes(cipherTextData);

            return plainData; // System.Text.Encoding.ASCII.GetString(pln).TrimEnd('\0');
        }

        #region EnDecryptString

        /// <summary>
        /// RC564 enrypt string method
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
        /// RC564 decrypt string method
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