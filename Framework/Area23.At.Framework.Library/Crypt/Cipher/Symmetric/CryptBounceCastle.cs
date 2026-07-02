using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// Generic CryptBounceCastle Encryption / Decryption class
    /// supports <see cref="CamelliaEngine"/>, <see cref="Gost28147Engine"/>, <see cref="RC2Engine"/>,
    /// <see cref="RC532Engine"/>, <see cref="RC6Engine"/>, <see cref="RijndaelEngine">RijndaelEngine is standard AES</see>, 
    /// <see cref="SkipjackEngine"/>, <see cref="TeaEngine"/>, <see cref="TnepresEngine"/>,
    /// <see cref="XteaEngine"/>, ... and many more
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>code changes</listheader>
    /// <item>
    /// 2026-02-11 alert-fix-13 changed mode from "ECB" to "CFB"     
    /// Reason: Git security scans
    /// consequences: no more fully deterministic math bijective proper symmertric cipher en-/decryption in pipe
    /// fixed attacks: not so easy REPLY attacks with binary format header and heuristic key collection
    /// </item>
    /// <item>
    /// 2026-mm-dd [enter pull request name here] [enter what you did here]
    /// Reason: [enter a senseful reason]
    /// consequences: [describe most impactful consequences of bugfix or code change request]
    /// fixed [vulnerability, code smell]: [Describe understandable precise in 1-2 setences]
    /// </item>
    /// </list>
    /// </remarks>
    public class CryptBounceCastle
    {

        #region fields

        private string privateKey = string.Empty;

        private string privateHash = string.Empty;

        private byte[] tmpIv;
        private byte[] tmpKey;

        #endregion fields

        #region properties

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

        public int IvLen { get; private set; }

        /// <summary>
        /// Base symmetric key block cipher interface, contains at runtime block cipher instance to constructor
        /// </summary>
        public IBlockCipher CryptoBlockCipher { get; private set; }

        /// <summary>
        /// IBlockCipherPadding BlockCipherPadding mode
        /// </summary>
        public IBlockCipherPadding CryptoBlockCipherPadding { get; private set; }

        internal BufferedBlockCipher BufBlockCiffre { get; private set; }

        /// <summary>
        /// Valid modes are currently "CBC", "ECB", "CFB", "CCM", "CTS", "EAX", "GOFB"
        /// <see cref="Org.BouncyCastle.Crypto.Modes"/> for crypto modes details.
        /// </summary>
        public string Mode { get; private set; }

        public CipherMode2 CMode { get; private set; }

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
            IvLen = 32;
            Size = 256;
            Mode = "ECB";
            CMode = CipherMode2.ECB;

            InitKeys();
        }


        /// <summary>
        /// Generic CryptBounceCastle constructor
        /// </summary>
        /// <param name="cparams">parameters to crypt</param>
        /// <param name="init">init <see cref="CryptBounceCastle"/> first time with a new key</param>
        public CryptBounceCastle(CryptParams cparams, bool init = true)
        {
            CryptoBlockCipher = cparams.BlockCipher == null ? new AesEngine() : cparams.BlockCipher;
            if (CryptoBlockCipher.AlgorithmName == "RC564" || CryptoBlockCipher.AlgorithmName == "RC5-64")
                CryptoBlockCipher = new RC564Engine();
            CryptoBlockCipherPadding = new ZeroBytePadding();
            KeyLen = cparams.KeyLen;
            IvLen = cparams.IvLen;
            Size = Math.Min(cparams.Size, CryptoBlockCipher.GetBlockSize());

            Mode = cparams.Mode;
            CMode = cparams.CMode2;

            if (init)
            {
                InitKeys();

                if (string.IsNullOrEmpty(cparams.Key))
                    throw new ArgumentNullException("cparams.Key");

                privateKey = cparams.Key;
                privateHash = string.IsNullOrEmpty(cparams.Hash) ? "" : cparams.Hash;

                tmpKey = System.Text.Encoding.UTF8.GetBytes(privateKey);
                tmpIv = System.Text.Encoding.UTF8.GetBytes(privateHash);

                int minKeyLen = KeyLen < tmpKey.Length ? KeyLen : tmpKey.Length;
                int minIvLen = IvLen < tmpIv.Length ? IvLen : tmpIv.Length;

                Array.Copy(tmpKey, Key, minKeyLen);
                Array.Copy(tmpIv, Iv, minIvLen);
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

        protected void InitKeys()
        {
            privateKey = string.Empty;
            privateHash = string.Empty;
            tmpKey = new byte[KeyLen];
            tmpIv = new byte[IvLen];

            Key = new byte[KeyLen];
            Iv = new byte[IvLen];
            //for (int i = 0; i < KeyLen; i++)
            //{
            //    tmpKey[i] = 0;
            //    Key[i] = 0;
            //    if (i < IvLen)
            //    {
            //        Iv[i] = 0;
            //        tmpIv[i] = 0;
            //    }
            //}
        }


        /// <summary>
        /// GetUserKeyBytes gets symetric chiffer private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="secretKey">user secret key, default email address</param>
        /// <param name="secretHash">user host ip address</param>
        /// <returns>Array of byte with length KeyLen</returns>
        internal byte[] GetUserKeyBytes(string secretKey, string secretHash)
        {
            if (!string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(secretKey);

            privateKey = secretKey;
            privateHash = string.IsNullOrEmpty(secretHash) ? "" : secretHash;

            tmpKey = new byte[KeyLen];
            tmpIv = new byte[IvLen];
            for (int i = 0; i < KeyLen; i++)
            {
                tmpKey[i] = 0;
                if (i < IvLen)
                    tmpIv[i] = 0;
            }

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(privateKey);
            Array.Copy(bytes, 0, tmpKey, 0, Math.Min(bytes.Length, KeyLen));

            bytes = System.Text.Encoding.UTF8.GetBytes(privateHash);
            Array.Copy(bytes, 0, tmpIv, 0, Math.Min(bytes.Length, IvLen));

            return tmpKey.TarBytes(tmpIv);
        }

        /// <summary>
        /// can this SymmBlockCipherAlgo key with initialization vector?
        /// </summary>
        /// <returns>true, if it hanldes init with key and initialization vector iv, otherwise false</returns>
        protected bool CanAlgoKeyIV(IBlockCipher cryptoBlockCipher = null)
        {
            if (cryptoBlockCipher == null)
                cryptoBlockCipher = CryptoBlockCipher;
            string algoName = cryptoBlockCipher.AlgorithmName.ToUpper();
            if (algoName.StartsWith("AES", StringComparison.CurrentCultureIgnoreCase) || algoName.StartsWith("RIJNDAEL", StringComparison.CurrentCultureIgnoreCase))
                return false;
            if (algoName.StartsWith("ARIA", StringComparison.CurrentCultureIgnoreCase) || algoName.StartsWith("ASCON", StringComparison.CurrentCultureIgnoreCase))
                return false;
            if (algoName.StartsWith("CAST", StringComparison.CurrentCultureIgnoreCase) || algoName.StartsWith("GOST", StringComparison.CurrentCultureIgnoreCase))
                return false;
            if (algoName.StartsWith("IDEA", StringComparison.CurrentCultureIgnoreCase) || algoName.StartsWith("RC", StringComparison.CurrentCultureIgnoreCase))
                return false;
            if (algoName.StartsWith("SKIPJACK", StringComparison.CurrentCultureIgnoreCase) || algoName.Equals("DESEDE"))
                return false;
            if (algoName.StartsWith("TEA", StringComparison.CurrentCultureIgnoreCase) || algoName.StartsWith("XTEA", StringComparison.CurrentCultureIgnoreCase))
                return false;
            return true;
        }

        #region EncryptDecryptBytes

        /// <summary>
        /// Generic CryptBounceCastle Encrypt member function
        /// difference between out parameter encryptedData and return value, are 2 different encryption methods, but with the same result at the end
        /// </summary>
        /// <param name="plainData">plain data as <see cref="T:byte[]"/></param>
        /// <returns>encrypted data <see cref="T:byte[]">bytes</see></returns>
        public byte[] Encrypt(byte[] plainData)
        {
            var cipher = CryptoBlockCipher;
            plainData = CryptoBlockCipher.AlgorithmName == "RC564" || CryptoBlockCipher.AlgorithmName == "RC5-64" ?
                EnDeCodeHelper.GetBytesFromBytes(plainData) : plainData;
            BufBlockCiffre = new PaddedBufferedBlockCipher(new EcbBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);

            switch (CMode)
            {
                case CipherMode2.CBC:
                    BufBlockCiffre = new PaddedBufferedBlockCipher(new CbcBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    break;
                case CipherMode2.ECB:
                    BufBlockCiffre = new PaddedBufferedBlockCipher(new EcbBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    break;
                case CipherMode2.CFB:
                    BufBlockCiffre = new PaddedBufferedBlockCipher(new CfbBlockCipher(CryptoBlockCipher, Size), CryptoBlockCipherPadding);
                    break;
                case CipherMode2.CCM:
                    CcmBlockCipher ccmCipher = new CcmBlockCipher(CryptoBlockCipher);
                    BufBlockCiffre = new PaddedBufferedBlockCipher((IBlockCipher)ccmCipher, CryptoBlockCipherPadding);
                    break;
                case CipherMode2.CTS:
                    CtsBlockCipher ctsCipher = new CtsBlockCipher(CryptoBlockCipher);
                    BufBlockCiffre = ctsCipher;
                    break;
                case CipherMode2.EAX:
                    EaxBlockCipher eaxCipher = new EaxBlockCipher(CryptoBlockCipher);
                    BufBlockCiffre = new PaddedBufferedBlockCipher((IBlockCipher)eaxCipher, CryptoBlockCipherPadding);
                    break;
                case CipherMode2.GOFB:
                    GOfbBlockCipher gOfbCipher = new GOfbBlockCipher(CryptoBlockCipher);
                    BufBlockCiffre = new PaddedBufferedBlockCipher((IBlockCipher)gOfbCipher, CryptoBlockCipherPadding);
                    break;
                default:
                    if (Iv.IsNullByteArray())
                    {
                        Mode = "ECB";
                        CMode = CipherMode2.ECB;
                        BufBlockCiffre = new PaddedBufferedBlockCipher(new EcbBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    }
                    break;
            }

            KeyParameter keyParam = CryptoBlockCipher.AlgorithmName == "RC564" || CryptoBlockCipher.AlgorithmName == "RC5-64" ?
                new RC5Parameters(Key, 2) : // RC5-64 with 2 rounds key initialization
                new KeyParameter(Key);      // default KeyParameter
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, Iv);  // KeyParameter with init vector

            // cipherMode init with initialization vector only when Mode isn't ECB and Algo is IV init capable
            if (CMode == CipherMode2.ECB || !CanAlgoKeyIV(CryptoBlockCipher))
                BufBlockCiffre.Init(true, keyParam);
            else
            {
                try
                {
                    BufBlockCiffre.Init(true, keyParamIV);
                }
                catch (Exception exInit)
                {
                    Area23Log.LogOriginMsgEx("CryptBounceCastle", $"CryptBounceCastle {BufBlockCiffre.AlgorithmName}: Exceptíon on cipherMode.Init with IV, trying without IV", exInit);
                    BufBlockCiffre.Init(true, keyParam);
                }
            }

            // encryptedData = cipherMode.ProcessBytes(plainData);

            int outputSize = BufBlockCiffre.GetOutputSize(plainData.Length);
            byte[] cipherData = new byte[outputSize];
            int result = BufBlockCiffre.ProcessBytes(plainData, 0, plainData.Length, cipherData, 0);
            BufBlockCiffre.DoFinal(cipherData, result);

            return cipherData;
        }

        /// <summary>
        /// Generic CryptBounceCastle Decrypt member function
        /// difference between out parameter decryptedData and return value, are 2 different decryption methods, but with the same result at the end
        /// </summary>
        /// <param name="cipherData">encrypted <see cref="T:byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public byte[] Decrypt(byte[] cipherData)
        {
            var cipher = CryptoBlockCipher;
            BufBlockCiffre = new PaddedBufferedBlockCipher(new EcbBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);

            switch (CMode)
            {
                case CipherMode2.CBC:
                    BufBlockCiffre = new PaddedBufferedBlockCipher(new CbcBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    break;
                case CipherMode2.ECB:
                    BufBlockCiffre = new PaddedBufferedBlockCipher(new EcbBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    break;
                case CipherMode2.CFB:
                    BufBlockCiffre = new PaddedBufferedBlockCipher(new CfbBlockCipher(CryptoBlockCipher, Size), CryptoBlockCipherPadding);
                    break;
                case CipherMode2.CCM:
                    CcmBlockCipher ccmCipher = new CcmBlockCipher(CryptoBlockCipher);
                    BufBlockCiffre = new PaddedBufferedBlockCipher((IBlockCipher)ccmCipher, CryptoBlockCipherPadding);
                    break;
                case CipherMode2.CTS:
                    CtsBlockCipher ctsCipher = new CtsBlockCipher(CryptoBlockCipher);
                    BufBlockCiffre = ctsCipher;
                    break;
                case CipherMode2.EAX:
                    EaxBlockCipher eaxCipher = new EaxBlockCipher(CryptoBlockCipher);
                    BufBlockCiffre = new PaddedBufferedBlockCipher((IBlockCipher)eaxCipher, CryptoBlockCipherPadding);
                    break;
                case CipherMode2.GOFB:
                    GOfbBlockCipher gOfbCipher = new GOfbBlockCipher(CryptoBlockCipher);
                    BufBlockCiffre = new PaddedBufferedBlockCipher((IBlockCipher)gOfbCipher, CryptoBlockCipherPadding);
                    break;
                default:
                    if (Iv.IsNullByteArray())
                    {
                        Mode = "ECB";
                        CMode = CipherMode2.ECB;
                        BufBlockCiffre = new PaddedBufferedBlockCipher(new EcbBlockCipher(CryptoBlockCipher), CryptoBlockCipherPadding);
                    }
                    break;
            }
            // cipherMode.Reset()                

            KeyParameter keyParam = CryptoBlockCipher.AlgorithmName == "RC564" || CryptoBlockCipher.AlgorithmName == "RC5-64" ?
                                        new RC5Parameters(Key, 2) :
                                        new KeyParameter(Key);
            ICipherParameters keyParamIV = new ParametersWithIV(keyParam, Iv);

            // Decrypt with initialization vector only when !ECB + algorithm is IV capable + iv is not null byte array          
            if (CMode == CipherMode2.ECB || !CanAlgoKeyIV(CryptoBlockCipher))
                BufBlockCiffre.Init(false, keyParam);
            else
            {
                try
                {
                    BufBlockCiffre.Init(false, keyParamIV);
                }
                catch (Exception exInit)
                {
                    Area23Log.LogOriginMsgEx("CryptBounceCastle", $"CryptBounceCastle {BufBlockCiffre.AlgorithmName}: Exceptíon on cipherMode.Init with IV, trying without IV", exInit);
                    BufBlockCiffre.Init(false, keyParam);
                }
            }

            int outputSize = BufBlockCiffre.GetOutputSize(cipherData.Length);
            byte[] plainData = new byte[outputSize];
            byte[] decryptedData = new byte[outputSize];
            try
            {
                int result = BufBlockCiffre.ProcessBytes(cipherData, 0, cipherData.Length, plainData, 0);
                BufBlockCiffre.DoFinal(plainData, result);
            }
            catch (Exception exDecrypt)
            {
                Area23Log.LogOriginMsgEx("CryptBounceCastle", $"CryptBounceCastle {BufBlockCiffre.AlgorithmName}: Exceptíon on decrypting final block", exDecrypt);
                try
                {
                    plainData = new byte[outputSize];
                    plainData = BufBlockCiffre.ProcessBytes(cipherData, 0, cipherData.Length);
                }
                catch (Exception exDecrypt2)
                {
                    Area23Log.LogOriginMsgEx("CryptBounceCastle", $"CryptBounceCastle {BufBlockCiffre.AlgorithmName}: Exceptíon on 2x decrypting final block", exDecrypt2);
                    plainData = new byte[outputSize];
                    plainData = BufBlockCiffre.ProcessBytes(cipherData);
                }
            }

            return CryptoBlockCipher.AlgorithmName == "RC564" || CryptoBlockCipher.AlgorithmName == "RC5-64" ?
                EnDeCodeHelper.GetBytesTrimNulls(plainData) : plainData;

            // return plainData;
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
