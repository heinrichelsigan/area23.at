using Area23.At.Framework.Library;
using NLog;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Area23.At.Mono.Util.SymChiffer
{
    /// <summary>
    /// Generic CryptBounceCastle Encryption / Decryption class
    /// supports <see cref="Org.BouncyCastle.Crypto.Engines.CamelliaEngine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.Gost28147Engine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.RC2Engine"/>,
    /// <see cref="Org.BouncyCastle.Crypto.Engines.RC532Engine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.RC6Engine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.RijndaelEngine">RijndaelEngine is standard AES</see>, 
    /// <see cref="Org.BouncyCastle.Crypto.Engines.SkipjackEngine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.TeaEngine"/>, <see cref="Org.BouncyCastle.Crypto.Engines.TnepresEngine"/>,
    /// <see cref="Org.BouncyCastle.Crypto.Engines.XteaEngine"/>, ... and many more
    /// </summary>
    public static class BounceCastle
    {
        private static string privateKey = string.Empty;

        internal static byte[] Key { get; private set; }
        internal static byte[] Iv { get; private set; }



        static byte[] tmpKey;
        static byte[] tmpIv;

        /// <summary>
        /// Block Size
        /// </summary>
        internal static int Size { get; private set; }

        /// <summary>
        /// KeyLen byte[KeyLen] of Key and Iv
        /// </summary>
        internal static int KeyLen { get; private set; }

        /// <summary>
        /// Base symmetric key block cipher interface, contains at runtime block cipher instance to constructor
        /// </summary>
        internal static IBlockCipher CryptoBlockCipher { get; private set; }

        /// <summary>
        /// IBlockCipherPadding BlockCipherPadding mode
        /// </summary>
        internal static IBlockCipherPadding CryptoBlockCipherPadding { get; private set; }

        internal static PaddedBufferedBlockCipher PadBufBChipger { get; private set; }

        /// <summary>
        /// Valid modes are currently "CBC", "ECB", "CFB", "CCM", "CTS", "EAX", "GOFB"
        /// <see cref="Org.BouncyCastle.Crypto.Modes"/> for crypto modes details.
        /// </summary>
        internal static string Mode { get; private set; }

        static BounceCastle()
        {
            IBlockCipher blockCipher = new AesEngine();
            int size = 256;
            int keyLen = 32;
            string mode = "ECB";

            tmpKey = new byte[keyLen];
            tmpIv = new byte[keyLen];
            CryptoBlockCipher = (blockCipher == null) ? new AesEngine() : blockCipher;
            CryptoBlockCipherPadding = new ZeroBytePadding();

            if (string.IsNullOrEmpty(privateKey))
            {
                privateKey = string.Empty;
                tmpKey = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
                tmpIv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            }
            else
            {
                tmpKey = Encoding.UTF8.GetByteCount(privateKey) == keyLen ? Encoding.UTF8.GetBytes(privateKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(privateKey));
                tmpIv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                // iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            }

            Key = new byte[keyLen];
            Iv = new byte[keyLen];
            Array.Copy(tmpIv, Iv, keyLen);
            Array.Copy(tmpKey, Key, keyLen);
            KeyLen = keyLen;
            Size = size;
            Mode = mode;

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
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="init">init <see cref="ThreeFish"/> first time with a new key</param>
        public static void InitBounceCastleAlgo(IBlockCipher blockCipher, int size = 256, int keyLen = 32, string mode = "ECB", string secretKey = "", bool init = true)
        {            
            CryptoBlockCipher = (blockCipher == null) ? new AesEngine() : blockCipher;
            CryptoBlockCipherPadding = new ZeroBytePadding();
            KeyLen = keyLen;
            Size = size;
            Mode = mode;

            if (init)
            {
                tmpKey = new byte[keyLen];
                tmpIv = new byte[keyLen];

                if (string.IsNullOrEmpty(secretKey))
                {
                    privateKey = string.Empty;
                    tmpIv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                    tmpKey = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
                }
                else
                {
                    privateKey = secretKey;
                    tmpKey = Encoding.UTF8.GetByteCount(secretKey) == keyLen ? Encoding.UTF8.GetBytes(secretKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                    tmpIv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                    //    RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
                    //    randomNumGen.GetBytes(tmpIv, 0, tmpIv.Length);
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
        /// CryptBounceCastleGenWithKey => Generates new <see cref="CryptBounceCastle"/> with secret key
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="init">init <see cref="ThreeFish"/> first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool CryptBounceCastleGenWithKey(string secretKey = "", bool init = true)
        {
            if (!init)
            {
                if ((string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(secretKey)) ||
                    (!privateKey.Equals(secretKey, StringComparison.InvariantCultureIgnoreCase)))
                    return false;
            }
            
            if (init)
            {
                tmpKey = new byte[KeyLen];
                tmpIv = new byte[KeyLen];

                if (string.IsNullOrEmpty(secretKey))
                {
                    privateKey = string.Empty;
                    tmpKey = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
                    tmpIv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4)); ;
                }
                else
                {
                    privateKey = secretKey;
                    tmpKey = Encoding.UTF8.GetByteCount(secretKey) == KeyLen ? Encoding.UTF8.GetBytes(secretKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                    tmpIv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                    // RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
                    // randomNumGen.GetBytes(tmpIv, 0, tmpIv.Length);
                }

                Key = new byte[KeyLen];
                Iv = new byte[KeyLen];
                Array.Copy(tmpKey, Key, KeyLen);
                Array.Copy(tmpIv, Iv, KeyLen);
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

            return true;
        }




        /// <summary>
        /// Generic CryptBounceCastle Encrypt member function
        /// difference between out parameter encryptedData and return value, are 2 different encryption methods, but with the same result at the end
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public static byte[] Encrypt(byte[] plainData)
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

            KeyParameter keyParam = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(Key);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, Iv);

            cipherMode.Init(true, keyParam);
            //if (Mode == "ECB")
            //    cipherMode.Init(true, keyParam);
            //else
            //    cipherMode.Init(true, keyParamIV);


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
        public static byte[] Decrypt(byte[] cipherData)
        {
            // var cipher = CryptoBlockCipher;
            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);

            if (PadBufBChipger == null)
            {
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
            }
            else
                cipherMode = PadBufBChipger;


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
        public static string EncryptString(string inString)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = Convert.ToBase64String(encryptedData);
            
            return encryptedString;
        }

        /// <summary>
        /// Generic CryptBounceCastle Decrypt String method
        /// </summary>
        /// <param name="inCryptString">base64 encrypted string</param>
        /// <returns>plain text decrypted string</returns>
        public static string DecryptString(string inCryptString)
        {
            byte[] cryptData = Convert.FromBase64String(inCryptString);
            byte[] decryptedData;
            byte[] plainData = Decrypt(cryptData);
            string plainTextString = System.Text.Encoding.ASCII.GetString(plainData).TrimEnd('\0');

            return plainTextString;
        }
        
        #endregion EnDecryptString

    }

}