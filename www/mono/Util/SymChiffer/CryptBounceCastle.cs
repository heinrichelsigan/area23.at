using System;
using System.Text;
using System.Collections.Generic;
using System.Windows.Interop;
using System.Linq;
using System.Web;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Crypto.Engines;
using static Org.BouncyCastle.Crypto.Engines.RsaEngine;
using Org.BouncyCastle.Asn1.Cms;


namespace Area23.At.Mono.Util.SymChiffer
{
    public class CryptBounceCastle
    {
        public byte[] Key { get; private set; }
        public byte[] Iv { get; private set; }
        public int Size { get; private set; }
        public IBlockCipher BlockCipher { get; private set; }
        public IBlockCipherPadding BlockCipherPadding { get; private set; }
        public static string Mode { get; private set; }

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

            int outputSize = cipherMode.GetOutputSize(plainData.Length);
            byte[] cipherData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(plainData, 0, plainData.Length, cipherData, 0);
            cipherMode.DoFinal(cipherData, result);
            
            encryptedData = cipherMode.ProcessBytes(plainData);            
            
            return cipherData;
        }

        public string EncryptString(string inString)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inString);
            byte[] encryptedData;
            byte[] cipherData = this.Encrypt(plainTextData, out encryptedData);
            string encryptedString = Convert.ToBase64String(encryptedData);
            // System.Text.Encoding.ASCII.GetString(encryptedData).TrimEnd('\0');
            return encryptedString;
        }

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

            int outputSize = cipherMode.GetOutputSize(cipherData.Length);
            // int outputSize = (int)Math.Max(cipherMode.GetOutputSize(cipherData.Length), cipherMode.GetUpdateOutputSize(cipherData.Length));
            byte[] plainData = new byte[outputSize];
            int result = cipherMode.ProcessBytes(cipherData, 0, cipherData.Length, plainData, 0);
            cipherMode.DoFinal(plainData, result);

            decryptedData = cipherMode.ProcessBytes(cipherData);            

            return plainData; // System.Text.Encoding.ASCII.GetString(pln).TrimEnd('\0');
        }

        public string DecryptString(string inCryptString)
        {
            byte[] cryptData = Convert.FromBase64String(inCryptString);
            //  System.Text.Encoding.UTF8.GetBytes(inCryptString);
            byte[] decryptedData;
            byte[] plainData = Decrypt(cryptData, out decryptedData);
            string plainTextString = System.Text.Encoding.ASCII.GetString(plainData).TrimEnd('\0');
            return plainTextString;
        }

    }

}