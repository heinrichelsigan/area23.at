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

namespace Area23.At.Mono.Util.SymChiffer
{
    public static class Serpent
    {
        public static byte[] Key { get; private set; }
        public static byte[] Iv { get; private set; }
        public static int Size { get; private set; }
        public static string Mode { get; private set; }
        public static IBlockCipherPadding BlockCipherPadding { get; private set; }

        static Serpent()
        {
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            byte[] key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
            BlockCipherPadding = new ZeroBytePadding();
            Key = new byte[16];
            Iv = new byte[16];
            Array.Copy(iv, Iv, 16);
            Array.Copy(key, Key, 16);
            Size = 128;
            Mode = "ECB"; 
        }

        public static byte[] Encrypt(byte[] plainData)
        {            
            var cipher = new SerpentEngine();

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

            int outputSize = cipherMode.GetOutputSize(plainData.Length);
            byte[] cipherTextData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(plainData, 0, plainData.Length, cipherTextData, 0);
            cipherMode.DoFinal(cipherTextData, result);
            var cipherData = cipherTextData;

            // byte[] cipherData = cipherMode.ProcessBytes(plainData);            

            return cipherData;
        }

        public static byte[] Decrypt(byte[] cipherData)
        {
            var cipher = new SerpentEngine();

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

            int outputSize = cipherMode.GetOutputSize(cipherData.Length);
            // int outputSize = (int)Math.Max(cipherMode.GetOutputSize(cipherData.Length), cipherMode.GetUpdateOutputSize(cipherData.Length));
            byte[] plainTextData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(cipherData, 0, cipherData.Length, plainTextData, 0);
            cipherMode.DoFinal(plainTextData, result);
            var plainData = plainTextData;

            // byte[] plainData = cipherMode.ProcessBytes(cipherData);
            
            return plainData; // System.Text.Encoding.ASCII.GetString(pln).TrimEnd('\0');
        }


        public static string EncryptString(string inPlainString)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inPlainString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = Convert.ToBase64String(encryptedData);
            // System.Text.Encoding.ASCII.GetString(encryptedData).TrimEnd('\0');
            return encryptedString;
        }

        public static string DecryptString(string inCryptString)
        {
            byte[] cryptData = Convert.FromBase64String(inCryptString);
            //  System.Text.Encoding.UTF8.GetBytes(inCryptString);
            byte[] plainTextData = Decrypt(cryptData);
            string plainTextString = System.Text.Encoding.ASCII.GetString(plainTextData).TrimEnd('\0');
            return plainTextString;
        }


    }

}