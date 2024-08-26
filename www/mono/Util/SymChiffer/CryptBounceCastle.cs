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
    public class CryptBounceCastle
    {
        private string privateKey = string.Empty;

        internal byte[] Key { get; private set; }
        internal byte[] Iv { get; private set; }

        /// <summary>
        /// Block Size
        /// </summary>
        internal int Size { get; private set; }

        /// <summary>
        /// KeyLen byte[KeyLen] of Key and Iv
        /// </summary>
        internal int KeyLen { get; private set; }

        /// <summary>
        /// Base symmetric key block cipher interface, contains at runtime block cipher instance to constructor
        /// </summary>
        internal IBlockCipher CryptoBlockCipher { get; private set; }

        /// <summary>
        /// IBlockCipherPadding BlockCipherPadding mode
        /// </summary>
        internal IBlockCipherPadding CryptoBlockCipherPadding { get; private set; }

        /// <summary>
        /// Valid modes are currently "CBC", "ECB", "CFB", "CCM", "CTS", "EAX", "GOFB"
        /// <see cref="Org.BouncyCastle.Crypto.Modes"/> for crypto modes details.
        /// </summary>
        internal static string Mode { get; private set; }

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
        public CryptBounceCastle(IBlockCipher blockCipher, int size = 256, int keyLen = 32, string mode = "ECB", string secretKey = "", bool init = true)
        {
            byte[] key = new byte[keyLen];
            byte[] iv = new byte[keyLen];
            CryptoBlockCipher = (blockCipher == null) ? new AesEngine() : blockCipher;            
            CryptoBlockCipherPadding = new ZeroBytePadding();
            string algoName = CryptoBlockCipher.AlgorithmName;

            if (string.IsNullOrEmpty(secretKey))
            {
                privateKey = string.Empty;
                iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                key = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCEK));
            }
            else
            {
                privateKey = secretKey;
                if (HttpContext.Current.Session[algoName + secretKey] == null)
                {
                    HttpContext.Current.Session[algoName + secretKey] = Encoding.UTF8.GetByteCount(secretKey) == keyLen ? Encoding.UTF8.GetBytes(secretKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                }
                key = (byte[])HttpContext.Current.Session[algoName + secretKey];

                if (HttpContext.Current.Session[algoName + Constants.BOUNCE4] == null)
                {
                    RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
                    randomNumGen.GetBytes(iv, 0, iv.Length);
                    HttpContext.Current.Session[algoName + Constants.BOUNCE4] = iv;
                }
                iv = (byte[])HttpContext.Current.Session[algoName + Constants.BOUNCE4];
                // iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
            }

            Key = new byte[keyLen];
            Iv = new byte[keyLen];
            Array.Copy(iv, Iv, keyLen);
            Array.Copy(key, Key, keyLen);
            KeyLen = keyLen;
            Size = size;
            Mode = mode;
        }

        /// <summary>
        /// CryptBounceCastleGenWithKey => Generates new <see cref="CryptBounceCastle"/> with secret key
        /// </summary>
        /// <param name="secretKey">key param for encryption</param>
        /// <param name="init">init <see cref="ThreeFish"/> first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public bool CryptBounceCastleGenWithKey(string secretKey = "", bool init = true)
        {
            byte[] key = new byte[KeyLen];
            byte[] iv = new byte[KeyLen];
            string algoName = CryptoBlockCipher.AlgorithmName;

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
                    iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4)); ;
                }
                else
                {
                    privateKey = secretKey;
                    // key = Encoding.UTF8.GetByteCount(secretKey) == KeyLen ? Encoding.UTF8.GetBytes(secretKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                    if (HttpContext.Current.Session[algoName + secretKey] == null)
                    {
                        HttpContext.Current.Session[algoName + secretKey] = Encoding.UTF8.GetByteCount(secretKey) == KeyLen ? Encoding.UTF8.GetBytes(secretKey) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                    }
                    key = (byte[])HttpContext.Current.Session[algoName + secretKey];

                    if (HttpContext.Current.Session[algoName + Constants.BOUNCE4] == null)
                    {
                        RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
                        randomNumGen.GetBytes(iv, 0, iv.Length);
                        HttpContext.Current.Session[algoName + Constants.BOUNCE4] = iv;
                    }
                    iv = (byte[])HttpContext.Current.Session[algoName + Constants.BOUNCE4];
                    // iv = Convert.FromBase64String(ResReader.GetValue(Constants.BOUNCE4));
                }

                Key = new byte[KeyLen];
                Iv = new byte[KeyLen];
                Array.Copy(key, Key, KeyLen);
                Array.Copy(iv, Iv, KeyLen);
            }            

            return true;
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
            var cipher = CryptoBlockCipher;
            PaddedBufferedBlockCipher cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);

            switch (Mode)
            {
                case "CBC":
                    cipherMode = new PaddedBufferedBlockCipher(new CbcBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    break;
                case "ECB": cipherMode = new PaddedBufferedBlockCipher(new EcbBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    break;
                case "CFB": cipherMode = new PaddedBufferedBlockCipher(new CfbBlockCipher(CryptoBlockCipher, Size), CryptoBlockCipherPadding);
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
            var cipher = CryptoBlockCipher;
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