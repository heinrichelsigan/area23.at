﻿using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Static;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
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

        #region fields

        private string privateKey = string.Empty;

        private string privateHash = string.Empty;

        private byte[] tmpIv;
        private byte[] tmpKey;

        #endregion fields

        #region properties

        // internal string PrivateUserKey { get => privateKey; }
        // internal string PrivateUserHostKey { get => privateHash; }

        internal byte[] Key { get; private set; }
        internal byte[] Iv { get; private set; }

        /// <summary>
        /// Block Size
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// KeyLen byte[KeyLen] of Key and Iv
        /// </summary>
        public int KeyLen { get; private set; }

        /// <summary>
        /// Base symmetric key block cipher interface, contains at runtime block cipher instance to constructor
        /// </summary>
        public IBlockCipher CryptoBlockCipher { get; private set; }

        /// <summary>
        /// IBlockCipherPadding BlockCipherPadding mode
        /// </summary>
        public IBlockCipherPadding CryptoBlockCipherPadding { get; private set; }

        internal PaddedBufferedBlockCipher PadBufBChipger { get; private set; }

        /// <summary>
        /// Valid modes are currently "CBC", "ECB", "CFB", "CCM", "CTS", "EAX", "GOFB"
        /// <see cref="Org.BouncyCastle.Crypto.Modes"/> for crypto modes details.
        /// </summary>
        public string Mode { get; private set; }

        #endregion properties

        #region ctor_init_gen

        /// <summary>
        /// parameterless default constructor
        /// </summary>
        public CryptBounceCastle()
        {
            CryptoBlockCipher = null;
            CryptoBlockCipherPadding = null;
            KeyLen = 32;
            Size = 256;
            Mode = "ECB";

            privateKey = string.Empty;
            privateHash = string.Empty;
            tmpKey = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
            tmpIv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));

            Key = new byte[KeyLen];
            Iv = new byte[KeyLen];
            Array.Copy(tmpIv, Iv, KeyLen);
            Array.Copy(tmpKey, Key, KeyLen);

            tmpKey = null;
            tmpIv = null;
        }

        /// <summary>
        /// Generic CryptBounceCastle constructor
        /// </summary>
        /// <param name="blockCipher">Base symmetric key block cipher interface, pass instance to constructor, e.g. 
        /// <code>CryptBounceCastle cryptCastle = new CryptBounceCastle(new Org.BouncyCastle.Crypto.Engines.CamelliaEngine());</code></param>
        /// <param name="size">block size with default value 256</param>
        /// <param name="keyLen">key length with default value 32</param>
        /// <param name="mode">cipher mode string, default value "ECB"</param>
        /// <param name="userHostAddr">user host address</param>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="init">init <see cref="ThreeFish"/> first time with a new key</param>
        [Obsolete("this constructor is obsolete, please use public CryptBounceCastle(CryptParams cparams, bool init = true) instead", true)]
        public CryptBounceCastle(IBlockCipher blockCipher, int size = 256, int keyLen = 32, string mode = "ECB",
            string secretKey = "", string privateHash = "", bool init = true)
        {
            CryptoBlockCipher = (blockCipher == null) ? new AesEngine() : blockCipher;
            if ((CryptoBlockCipher.AlgorithmName == "RC564"))
                CryptoBlockCipherPadding = new ISO7816d4Padding();
            else CryptoBlockCipherPadding = new ZeroBytePadding();
            KeyLen = keyLen;
            Size = Math.Min(size, CryptoBlockCipher.GetBlockSize());
            Mode = mode;

            if (init)
            {
                tmpKey = new byte[keyLen];
                tmpIv = new byte[keyLen];

                privateHash = (string.IsNullOrEmpty(privateHash)) ? string.Empty : privateHash;
                if (string.IsNullOrEmpty(secretKey))
                {
                    privateKey = string.Empty;
                    tmpKey = GetUserKeyBytes(ResReader.GetValue(Constants.BOUNCEK), privateHash);
                    tmpIv = GetUserKeyBytes(ResReader.GetValue(Constants.BOUNCE4), privateHash);
                }
                else
                {
                    privateKey = secretKey;
                    tmpKey = GetUserKeyBytes(secretKey, privateHash);
                    tmpIv = GetUserKeyBytes(privateHash, secretKey);
                }

                Key = new byte[keyLen];
                Iv = new byte[keyLen];
                Array.Copy(tmpIv, Iv, keyLen);
                Array.Copy(tmpKey, Key, keyLen);
            }
            else
            {
                if (tmpKey == null || tmpIv == null || tmpKey.Length <= 1 || tmpIv.Length <= 1)
                {
                    tmpKey = new byte[keyLen];
                    tmpIv = new byte[keyLen];
                    Array.Copy(Iv, tmpIv, keyLen);
                    Array.Copy(Key, tmpKey, keyLen);
                }
            }
        }


        /// <summary>
        /// Generic CryptBounceCastle constructor
        /// </summary>
        /// <param name="cparams">parameters to crypt</param>
        /// <param name="init">init <see cref="ThreeFish"/> first time with a new key</param>
        public CryptBounceCastle(CryptParams cparams, bool init = true)
        {
            CryptoBlockCipher = (cparams.BlockCipher == null) ? new AesEngine() : cparams.BlockCipher;
            if ((CryptoBlockCipher.AlgorithmName == "RC564"))
                CryptoBlockCipherPadding = new ISO7816d4Padding();
            else CryptoBlockCipherPadding = new ZeroBytePadding();
            KeyLen = cparams.KeyLen;
            Size = Math.Min(cparams.Size, CryptoBlockCipher.GetBlockSize());
            Mode = cparams.Mode;

            if (init)
            {
                tmpKey = new byte[KeyLen];
                tmpIv = new byte[KeyLen];

                privateHash = (string.IsNullOrEmpty(cparams.Hash)) ? string.Empty : cparams.Hash;
                if (string.IsNullOrEmpty(cparams.Key))
                {
                    privateKey = string.Empty;
                    tmpKey = GetUserKeyBytes(ResReader.GetValue(Constants.BOUNCEK), privateHash);
                    tmpIv = GetUserKeyBytes(ResReader.GetValue(Constants.BOUNCE4), privateHash);
                }
                else
                {
                    privateKey = cparams.Key;
                    privateHash = cparams.Hash;
                    tmpKey = GetUserKeyBytes(privateKey, privateHash);
                    tmpIv = GetUserKeyBytes(privateHash, privateKey);
                }

                Key = new byte[KeyLen];
                Iv = new byte[KeyLen];
                Array.Copy(tmpIv, Iv, KeyLen);
                Array.Copy(tmpKey, Key, KeyLen);
            }
            else
            {
                if (tmpKey == null || tmpIv == null || tmpKey.Length <= 1 || tmpIv.Length <= 1)
                {
                    tmpKey = new byte[KeyLen];
                    tmpIv = new byte[KeyLen];
                    Array.Copy(Iv, tmpIv, KeyLen);
                    Array.Copy(Key, tmpKey, KeyLen);
                }
            }
        }

        #endregion ctor_init_gen

        /// <summary>
        /// GetUserKeyBytes gets symetric chiffer private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="secretKey">user secret key, default email address</param>
        /// <param name="secretHash">user host ip address</param>
        /// <returns>Array of byte with length KeyLen</returns>
        internal byte[] GetUserKeyBytes(string secretKey = "postmaster@localhost", string secretHash = "127.0.0.1")
        {
            privateKey = secretKey;
            privateHash = secretHash;

            string keyByteHashString = privateKey;
            tmpKey = new byte[KeyLen];
            tmpKey = CryptHelper.GetUserKeyBytes(privateKey, privateHash, KeyLen);
            if (tmpKey.Length < KeyLen)
                throw new ApplicationException($"key {tmpKey.ToHexString()} is shorten then KeyLen {KeyLen}");

            return tmpKey;

        }

        #region EncryptDecryptBytes

        /// <summary>
        /// Generic CryptBounceCastle Encrypt member function
        /// difference between out parameter encryptedData and return value, are 2 different encryption methods, but with the same result at the end
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public byte[] Encrypt(byte[] plainData)
        {
            // var cipher = CryptoBlockCipher;
            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);

            switch (Mode)
            {
                case "CBC":
                    cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    break;
                case "ECB":
                    cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    break;
                case "CFB":
                    cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(CryptoBlockCipher, Size), CryptoBlockCipherPadding);
                    break;
                case "CCM":
                    Org.BouncyCastle.Crypto.Modes.CcmBlockCipher ccmCipher = new CcmBlockCipher(CryptoBlockCipher);
                    cipherMode = new PaddedBufferedBlockCipher((IBlockCipher)ccmCipher, CryptoBlockCipherPadding);
                    break;
                case "CTS":
                    Org.BouncyCastle.Crypto.Modes.CtsBlockCipher ctsCipher = new CtsBlockCipher(CryptoBlockCipher);
                    cipherMode = new PaddedBufferedBlockCipher((IBlockCipher)ctsCipher, CryptoBlockCipherPadding);
                    break;
                case "EAX":
                    Org.BouncyCastle.Crypto.Modes.EaxBlockCipher eaxCipher = new EaxBlockCipher(CryptoBlockCipher);
                    cipherMode = new PaddedBufferedBlockCipher((IBlockCipher)eaxCipher, CryptoBlockCipherPadding);
                    break;
                case "GOFB":
                    Org.BouncyCastle.Crypto.Modes.GOfbBlockCipher gOfbCipher = new GOfbBlockCipher(CryptoBlockCipher);
                    cipherMode = new PaddedBufferedBlockCipher((IBlockCipher)gOfbCipher, CryptoBlockCipherPadding);
                    break;
                default:
                    break;
            }

            if (CryptoBlockCipher.AlgorithmName == "RC564")
            {
                RC5Parameters rc5Params = new RC5Parameters(Key, 1);
                Org.BouncyCastle.Crypto.Engines.RC564Engine rc564 = new RC564Engine();
                cipherMode.Init(true, rc5Params);
            }
            else
            {

                KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(Key);
                ICipherParameters keyParamIV = new ParametersWithIV(keyParam, Iv);

                cipherMode.Init(true, keyParam);
            }
            // if (Mode == "ECB")
            //     cipherMode.Init(true, keyParam);
            // else
            //      cipherMode.Init(true, keyParamIV);


            if (PadBufBChipger == null && cipherMode != null)
                PadBufBChipger = cipherMode;

            // encryptedData = cipherMode.ProcessBytes(plainData);

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
        /// <returns>decrypted plain byte[] data</returns>
        public byte[] Decrypt(byte[] cipherData)
        {
            // var cipher = CryptoBlockCipher;
            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);

            switch (Mode)
            {
                case "CBC":
                    cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    break;
                case "ECB":
                    cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    break;
                case "CFB":
                    cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(CryptoBlockCipher, Size), CryptoBlockCipherPadding);
                    break;
                case "CCM":
                    Org.BouncyCastle.Crypto.Modes.CcmBlockCipher ccmCipher = new CcmBlockCipher(CryptoBlockCipher);
                    cipherMode = new PaddedBufferedBlockCipher((IBlockCipher)ccmCipher, CryptoBlockCipherPadding);
                    break;
                case "CTS":
                    Org.BouncyCastle.Crypto.Modes.CtsBlockCipher ctsCipher = new CtsBlockCipher(CryptoBlockCipher);
                    cipherMode = new PaddedBufferedBlockCipher((IBlockCipher)ctsCipher, CryptoBlockCipherPadding);
                    break;
                case "EAX":
                    Org.BouncyCastle.Crypto.Modes.EaxBlockCipher eaxCipher = new EaxBlockCipher(CryptoBlockCipher);
                    cipherMode = new PaddedBufferedBlockCipher((IBlockCipher)eaxCipher, CryptoBlockCipherPadding);
                    break;
                case "GOFB":
                    Org.BouncyCastle.Crypto.Modes.GOfbBlockCipher gOfbCipher = new GOfbBlockCipher(CryptoBlockCipher);
                    cipherMode = new PaddedBufferedBlockCipher((IBlockCipher)gOfbCipher, CryptoBlockCipherPadding);
                    break;
                default:
                    break;
            }
            // cipherMode.Reset()                

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(Key);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, Iv);

            // Decrypt
            cipherMode.Init(false, keyParam);
            //if (Mode == "ECB")
            //    cipherMode.Init(false, keyParam);
            //else
            //    cipherMode.Init(false, keyParamIV);

            // decryptedData = cipherMode.ProcessBytes(cipherData);
            if (cipherMode != null)
                PadBufBChipger = cipherMode;

            int outputSize = cipherMode.GetOutputSize(cipherData.Length);
            byte[] plainData = new byte[outputSize];
            byte[] decryptedData = new byte[outputSize];
            try
            {
                int result = cipherMode.ProcessBytes(cipherData, 0, cipherData.Length, plainData, 0);
                cipherMode.DoFinal(plainData, result);
            }
            catch (Exception exDecrypt)
            {
                Area23Log.Logger.LogOriginMsgEx("CryptBounceCastle", $"CryptBounceCastle {cipherMode.AlgorithmName}: Exceptíon on decrypting final block", exDecrypt);
                try
                {
                    plainData = new byte[outputSize];
                    plainData = cipherMode.ProcessBytes(cipherData, 0, cipherData.Length);
                }
                catch (Exception exDecrypt2)
                {
                    Area23Log.Logger.LogOriginMsgEx("CryptBounceCastle", $"CryptBounceCastle {cipherMode.AlgorithmName}: Exceptíon on 2x decrypting final block", exDecrypt2);
                    plainData = new byte[outputSize];
                    plainData = cipherMode.ProcessBytes(cipherData);
                }
            }

            return plainData;
        }

        #endregion EncryptDecryptBytes

        #region EnDecryptString

        /// <summary>
        /// Generic CryptBounceCastle Encrypt String method
        /// </summary>
        /// <param name="inString">plain string to encrypt</param>
        /// <param name="encodingType">
        /// beware of using <see cref="EncodingType.Uu"/>; 
        /// default <see cref="EncodingType.Base64"/>
        /// </param>
        /// <returns>encoded encrypted string, default base64 encoded</returns>
        public string EncryptString(string inString, EncodingType encodingType = EncodingType.Base64)
        {
            byte[] plainTextData = EnDeCodeHelper.GetBytes(inString);
            byte[] encryptedBytes = Encrypt(plainTextData);
            string encryptedString = EnDeCodeHelper.EncodeBytes(encryptedBytes, encodingType);

            return encryptedString;
        }

        /// <summary>
        /// Generic CryptBounceCastle Decrypt String method
        /// </summary>
        /// <param name="inCryptString">encoded encrypted string, default base64 encoded</param>
        /// <param name="encodingType">
        /// beware of using <see cref="EncodingType.Uu"/>; 
        /// default <see cref="EncodingType.Base64"/>
        /// </param>
        /// <returns>plain text decrypted string</returns>
        public string DecryptString(string inCryptString, EncodingType encodingType = EncodingType.Base64)
        {
            byte[] cipherBytes = EnDeCodeHelper.DecodeText(inCryptString, encodingType);
            byte[] plainData = Decrypt(cipherBytes);
            string plainTextString = EnDeCodeHelper.GetString(plainData).TrimEnd('\0');

            return plainTextString;
        }

        #endregion EnDecryptString

    }


}
