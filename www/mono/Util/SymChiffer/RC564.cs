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
    public static class RC564
    {
        public static byte[] Key { get; private set; }
        public static byte[] Iv { get; private set; }
        public static int Size { get; private set; }
        public static string Mode { get; private set; } 

        static RC564()
        {
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            byte[] key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
            Key = new byte[16];
            Iv = new byte[16];
            Array.Copy(iv, Iv, 16);
            Array.Copy(key, Key, 16);
            Size = 128;
            Mode = "CFB"; 
        }

        public static byte[] Encrypt(byte[] plainTextData)
        {            
            var cipher = new RC564Engine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());
            
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), new Pkcs7Padding());

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

            // int outputSize = cipherMode.GetOutputSize(plainTextData.Length);
            // byte[] cipherTextData = new byte[outputSize];
            // int result = cipherMode.ProcessBytes(plainTextData, 0, plainTextData.Length, cipherTextData, 0);
            // cipherMode.DoFinal(cipherTextData, result);
            byte[] chipherData = cipherMode.ProcessBytes(plainTextData);
            
            return chipherData;
        }

        public static string EncryptString(string inString)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = Convert.ToBase64String(encryptedData); 
                // System.Text.Encoding.ASCII.GetString(encryptedData).TrimEnd('\0');
            return encryptedString;
        }

        public static byte[] Decrypt(byte[] cipherTextData)
        {
            var cipher = new RC564Engine();

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());
            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, Size), new Pkcs7Padding());
            
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

            // int outputSize = cipherMode.GetOutputSize(cipherTextData.Length);
            // outputSize = outputSize + (int)(outputSize / 2);
            // byte[] plainTextData = new byte[outputSize];
            // int result = cipherMode.ProcessBytes(cipherTextData, 0, cipherTextData.Length, plainTextData, 0);
            // cipherMode.DoFinal(plainTextData, result);
            byte[] plainData = cipherMode.ProcessBytes(cipherTextData);

            var pln = plainData;

            return pln; // System.Text.Encoding.ASCII.GetString(pln).TrimEnd('\0');
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