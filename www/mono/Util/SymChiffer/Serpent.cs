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
        public static string Iv { get; private set; }
        public static int Size { get; private set; }
        public static string Mode { get; private set; } 

        static Serpent()
        {
            byte[] iv = Convert.FromBase64String(ResReader.GetValue(Constants.SERPENT_IV));
            Iv = ToHexString(iv);
            Size = 128;
            Mode = "CBC"; 
        }

        public static byte[] Encrypt(byte[] plainTextData)
        {            
            var cipher = new SerpentEngine();

            byte[] nonce = new byte[16];
            Arr

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());

            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, 128), new Pkcs7Padding());


            CipherKeyGenerator keyGen = new CipherKeyGenerator();
            keyGen.Init(new KeyGenerationParameters(new SecureRandom(), Size));
            KeyParameter keyParam = keyGen.GenerateKeyParameter();
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, nonce);
            if (Mode == "ECB")
            {
                cipherMode.Init(true, keyParam);
            }
            else
            {
                cipherMode.Init(true, keyParamIV);
            }

            int outputSize = cipherMode.GetOutputSize(plainTextData.Length);
            byte[] cipherTextData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(plainTextData, 0, plainTextData.Length, cipherTextData, 0);
            cipherMode.DoFinal(cipherTextData, result);
            var rtn = cipherTextData;
            return rtn;
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
            
            var cipher = new SerpentEngine();

            byte[] nonce = new byte[16];
            Arr

            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(cipher), new Pkcs7Padding());

            if (Mode == "ECB") cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(cipher), new Pkcs7Padding());
            else if (Mode == "CFB") cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(cipher, 128), new Pkcs7Padding());


            CipherKeyGenerator keyGen = new CipherKeyGenerator();
            keyGen.Init(new KeyGenerationParameters(new SecureRandom(), Size));
            KeyParameter keyParam = keyGen.GenerateKeyParameter();
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, nonce);
            //if (Mode == "ECB")
            //{
            //    cipherMode.Init(true, keyParam);
            //}
            //else
            //{
            //    cipherMode.Init(true, keyParamIV);
            //}



            // Decrypt
            cipherMode.Init(false, keyParam);

            int outputSize = cipherMode.GetOutputSize(cipherTextData.Length);
            byte[] plainTextData = new byte[outputSize];

            int result = cipherMode.ProcessBytes(cipherTextData, 0, cipherTextData.Length, plainTextData, 0);

            cipherMode.DoFinal(plainTextData, result);

            var pln = plainTextData;

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


        public static string ToHexString(byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }

        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        }

    }

}