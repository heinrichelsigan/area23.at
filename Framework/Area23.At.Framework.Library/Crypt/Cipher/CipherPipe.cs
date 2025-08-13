using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Zfx;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    /// <summary>
    /// Provides a simple crypt pipe for <see cref="CipherEnum"/>
    /// </summary>
    public class CipherPipe
    {

        private string cipherKey = "", cipherHash = "";
        private readonly CipherEnum[] inPipe;
        public readonly CipherEnum[] outPipe;
        private readonly string pipeString;

        public CipherEnum[] InPipe { get => inPipe; }

        public CipherEnum[] OutPipe { get => outPipe; }

        public string PipeString { get => pipeString; }

#if DEBUG
        public Dictionary<CipherEnum, byte[]> stageDictionary = new Dictionary<CipherEnum, byte[]>();

        public string HexStages
        {
            get
            {
                string hexOut = string.Empty;
                foreach (var stage in stageDictionary)
                {
                    hexOut += stage.Key.ToString() + "\r\n" + Hex16.ToHex16(stage.Value) + "\r\n";
                }

                return hexOut;
            }
        }
#endif

        #region ctor CipherPipe

        /// <summary>
        /// CipherPipe constructor with an array of <see cref="CipherEnum[]"/> as inpipe
        /// </summary>
        /// <param name="cipherEnums">array of <see cref="CipherEnum[]"/> as inpipe</param>
        public CipherPipe(CipherEnum[] cipherEnums)
        {
            inPipe = new List<CipherEnum>(cipherEnums).ToArray();
            outPipe = cipherEnums.Reverse<CipherEnum>().ToArray();
            pipeString = "";
            foreach (CipherEnum cipher in inPipe)
                pipeString += cipher.GetCipherChar();
        }

        /// <summary>
        /// CipherPipe constructor with an array of <see cref="string[]"/> cipherAlgos as inpipe
        /// </summary>
        /// <param name="cipherAlgos">array of <see cref="string[]"/> as inpipe</param>
        public CipherPipe(string[] cipherAlgos)
        {
            List<CipherEnum> cipherEnums = new List<CipherEnum>();
            foreach (string algo in cipherAlgos)
            {
                if (!string.IsNullOrEmpty(algo))
                {
                    CipherEnum cipherAlgo = CipherEnum.Aes;
                    if (!Enum.TryParse<CipherEnum>(algo, out cipherAlgo))
                        cipherAlgo = CipherEnum.Aes;

                    cipherEnums.Add(cipherAlgo);
                }
            }

            inPipe = new List<CipherEnum>(cipherEnums).ToArray();
            outPipe = cipherEnums.Reverse<CipherEnum>().ToArray();
            pipeString = "";
            foreach (CipherEnum cipher in inPipe)
                pipeString += cipher.GetCipherChar();
        }

        /// <summary>
        /// CipherPipe ctor with array of user key bytes
        /// </summary>
        /// <param name="keyBytes">user key bytes</param>
        /// <param name="maxpipe">maximum lentgh <see cref="Constants.MAX_PIPE_LEN"/></param>
        public CipherPipe(byte[] keyBytes, uint maxpipe = 8)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = (maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe; // if somebody wants more, he/she/it gets less

            ushort scnt = 0;
            List<CipherEnum> pipeList = new List<CipherEnum>();
            Dictionary<string, CipherEnum> symDict = new Dictionary<string, CipherEnum>();
            foreach (CipherEnum symmC in Enum.GetValues(typeof(CipherEnum)))
            {
                string hex = $"{((ushort)symmC):x2}";
                scnt++;
                symDict.Add(hex, symmC);
            }

            string hexString = string.Empty;
            HashSet<string> hashBytes = new HashSet<string>();
            foreach (byte bb in keyBytes)
            {
                byte cb = (byte)((int)((int)bb % 20));
                hexString = string.Format("{0:x2}", cb);
                if (hexString.Length > 0 && !hashBytes.Contains(hexString))
                    hashBytes.Add(hexString);
            }

            hexString = string.Empty;
            for (int kcnt = 0; kcnt < hashBytes.Count && pipeList.Count < maxpipe; kcnt++)
            {
                hexString += hashBytes.ElementAt(kcnt).ToString();
                CipherEnum sym0 = symDict[hashBytes.ElementAt(kcnt)];
                pipeList.Add(sym0);
            }

            inPipe = new List<CipherEnum>(pipeList).ToArray();
            outPipe = pipeList.Reverse<CipherEnum>().ToArray();
            pipeString = "";
            foreach (CipherEnum cipherE in inPipe)
                pipeString += cipherE.GetCipherChar();

        }

        /// <summary>
        /// Constructs a <see cref="CipherPipe"/> from key and hash
        /// by getting <see cref="byte[]">byte[] keybytes</see> with <see cref="CryptHelper.GetUserKeyBytes(string, string, int)"/>
        /// </summary>
        /// <param name="key">secret key to generate pipe</param>
        /// <param name="hash">hash value of secret key</param>
        public CipherPipe(string key = "heinrich.elsigan@area23.at", string hash = "6865696e726963682e656c736967616e406172656132332e6174")
            : this(CryptHelper.GetUserKeyBytes(key, hash, 16), Constants.MAX_PIPE_LEN)
        {
            cipherKey = key;
            cipherHash = hash;
        }

        /// <summary>
        /// CipherPipe ctor with only key
        /// </summary>
        /// <param name="key"></param>
        public CipherPipe(string key = "heinrich.elsigan@area23.at") : this(key, EnDeCodeHelper.KeyToHex(key))
        {
            cipherKey = key;
        }

        #endregion ctor CipherPipe

        #region static members EncryptBytesFast DecryptBytesFast

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytesFast(byte[] inBytes, CipherEnum cipherAlgo = CipherEnum.Aes,
            string secretKey = "heinrich.elsigan@area23.at", string hashIv = "")
        {
            byte[] encryptBytes = inBytes;
            string hash = (string.IsNullOrEmpty(hashIv)) ? EnDeCodeHelper.KeyToHex(secretKey) : hashIv;

            switch (cipherAlgo)
            {
                case CipherEnum.RC564:
                    RC564.RC564GenWithKey(secretKey, hash, true);
                    encryptBytes = RC564.Encrypt(inBytes);
                    break;
                case CipherEnum.Rfc3211:
                    Rfc3211Wrap.Rfc3211WrapGenWithKey(secretKey, hash, true);
                    encryptBytes = Rfc3211Wrap.Encrypt(inBytes);
                    break;
                case CipherEnum.Rsa:
                    var keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                    string privKey = keyPair.Private.ToString();
                    encryptBytes = Asymmetric.Rsa.Encrypt(inBytes);
                    break;
                case CipherEnum.Serpent:
                    Serpent.SerpentGenWithKey(secretKey, hash, true);
                    encryptBytes = Serpent.Encrypt(inBytes);
                    break;
                case CipherEnum.ZenMatrix:
                    encryptBytes = (new ZenMatrix(secretKey, hash, true)).Encrypt(inBytes);
                    break;
                case CipherEnum.ZenMatrix2:
                    encryptBytes = (new ZenMatrix2(secretKey, hash, false)).Encrypt(inBytes);
                    break;
                case CipherEnum.Aes:
                case CipherEnum.AesLight:
                case CipherEnum.Aria:
                case CipherEnum.BlowFish:
                case CipherEnum.Camellia:
                case CipherEnum.Cast5:
                case CipherEnum.Cast6:
                case CipherEnum.Des:
                case CipherEnum.Des3:
                case CipherEnum.Dstu7624:
                case CipherEnum.Fish2:
                case CipherEnum.Fish3:
                case CipherEnum.ThreeFish256:
                case CipherEnum.Gost28147:
                case CipherEnum.Idea:
                case CipherEnum.Noekeon:
                case CipherEnum.RC2:
                case CipherEnum.RC532:
                case CipherEnum.RC6:
                case CipherEnum.Seed:
                case CipherEnum.SM4:
                case CipherEnum.SkipJack:
                case CipherEnum.Tea:
                case CipherEnum.Tnepres:
                case CipherEnum.XTea:
                default:
                    CryptParams cpParams = new CryptParams(cipherAlgo, secretKey, hash);
                    Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);
                    encryptBytes = cryptBounceCastle.Encrypt(inBytes);
                    break;
            }

            return encryptBytes;
        }

        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/>both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytesFast(byte[] cipherBytes, CipherEnum cipherAlgo = CipherEnum.Aes,
            string secretKey = "heinrich.elsigan@area23.at", string hashIv = "", bool fishOnAesEngine = false)
        {
            bool sameKey = true;
            byte[] decryptBytes = cipherBytes;
            string hash = (string.IsNullOrEmpty(hashIv)) ? EnDeCodeHelper.KeyToHex(secretKey) : hashIv;

            switch (cipherAlgo)
            {
                case CipherEnum.Serpent:
                    sameKey = Serpent.SerpentGenWithKey(secretKey, hash, true);
                    decryptBytes = Serpent.Decrypt(cipherBytes);
                    break;
                case CipherEnum.RC564:
                    RC564.RC564GenWithKey(secretKey, hash, true);
                    decryptBytes = RC564.Decrypt(cipherBytes);
                    break;
                case CipherEnum.Rfc3211:
                    Rfc3211Wrap.Rfc3211WrapGenWithKey(secretKey, hash, false);
                    decryptBytes = Rfc3211Wrap.Decrypt(cipherBytes);
                    break;
                case CipherEnum.Rsa:
                    Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                    string privKey = keyPair.Private.ToString();
                    decryptBytes = Asymmetric.Rsa.Decrypt(cipherBytes);
                    break;
                case CipherEnum.ZenMatrix:
                    decryptBytes = (new ZenMatrix(secretKey, hash, true)).Decrypt(cipherBytes);
                    break;
                case CipherEnum.ZenMatrix2:
                    decryptBytes = (new ZenMatrix2(secretKey, hash, false)).Decrypt(cipherBytes);
                    break;
                case CipherEnum.Aes:
                case CipherEnum.AesLight:
                case CipherEnum.Aria:
                case CipherEnum.BlowFish:
                case CipherEnum.Camellia:
                case CipherEnum.Cast5:
                case CipherEnum.Cast6:
                case CipherEnum.Des:
                case CipherEnum.Des3:
                case CipherEnum.Dstu7624:
                case CipherEnum.Fish2:
                case CipherEnum.Fish3:
                case CipherEnum.ThreeFish256:
                case CipherEnum.Gost28147:
                case CipherEnum.Idea:
                case CipherEnum.Noekeon:
                case CipherEnum.RC2:
                case CipherEnum.RC532:
                case CipherEnum.RC6:
                case CipherEnum.Seed:
                case CipherEnum.SM4:
                case CipherEnum.SkipJack:
                case CipherEnum.Tea:
                case CipherEnum.Tnepres:
                case CipherEnum.XTea:
                default:
                    CryptParams cpParams = new CryptParams(cipherAlgo, secretKey, hash);
                    Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);
                    decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);

                    break;
            }


            return EnDeCodeHelper.GetBytesTrimNulls(decryptBytes);
        }

        #endregion static members EncryptBytesFast DecryptBytesFast

        /// <summary>
        /// MerryGoRoundEncrpyt starts merry to go arround from left to right in clock hour cycle
        /// </summary>
        /// <param name="inBytes">plain <see cref="byte[]"/ to encrypt></param>
        /// <param name="secretKey">user secret key to use for all symmetric cipher algorithms in the pipe</param>
        /// <param name="hashIv">hash key iv relational to secret key</param>
        /// <param name="zipBefore"><see cref="ZipType"/> and <see cref="ZipTypeExtensions.Zip(ZipType, byte[])"/></param>
        /// <returns>encrypted byte[]</returns>
        public byte[] MerryGoRoundEncrpyt(byte[] inBytes, 
            string secretKey = "heinrich.elsigan@area23.at",  string hashIv = "", 
            ZipType zipBefore = ZipType.None)
        {
            cipherKey = secretKey;
            cipherHash = (string.IsNullOrEmpty(hashIv)) ? EnDeCodeHelper.KeyToHex(secretKey) : hashIv;

            byte[] encryptedBytes = new byte[inBytes.Length];
            Array.Copy(inBytes, 0, encryptedBytes, 0, inBytes.Length);
#if DEBUG
            stageDictionary = new Dictionary<CipherEnum, byte[]>();
            // stageDictionary.Add(CipherEnum.ZenMatrix, inBytes);
#endif
            if (zipBefore != ZipType.None)
            {
                encryptedBytes = zipBefore.Zip(inBytes);
                inBytes = encryptedBytes;
            }

            foreach (CipherEnum cipher in InPipe)
            {
                encryptedBytes = EncryptBytesFast(inBytes, cipher, cipherKey, cipherHash);
                inBytes = encryptedBytes;
#if DEBUG
                stageDictionary.Add(cipher, encryptedBytes);
#endif
            }

            return encryptedBytes;
        }

        /// <summary>
        /// DecrpytRoundGoMerry against clock turn -
        /// starts merry to turn arround from right to left against clock hour cycle 
        /// </summary>
        /// <param name="cipherBytes">encrypted byte array</param>
        /// <param name="secretKey">user secret key, normally email address</param>
        /// <param name="hashIv">hash relational to secret kay</param>
        /// <returns><see cref="byte[]"/> plain bytes</returns>
        public byte[] DecrpytRoundGoMerry(byte[] cipherBytes, string secretKey = "heinrich.elsigan@area23.at", string hashIv = "", bool fishOnAesEngine = false)
        {
            cipherKey = secretKey;
            cipherHash = (string.IsNullOrEmpty(hashIv)) ? EnDeCodeHelper.KeyToHex(secretKey) : hashIv;
            long outByteLen = (OutPipe == null || OutPipe.Length == 0) ? cipherBytes.Length : ((cipherBytes.Length * 3) + 1);
            byte[] decryptedBytes = new byte[outByteLen];
#if DEBUG
            stageDictionary = new Dictionary<CipherEnum, byte[]>();
            // stageDictionary.Add(CipherEnum.ZenMatrix, cipherBytes);
#endif 
            if (OutPipe == null || OutPipe.Length == 0)
                Array.Copy(cipherBytes, 0, decryptedBytes, 0, cipherBytes.Length);
            else
                foreach (CipherEnum cipher in OutPipe)
                {
                    decryptedBytes = DecryptBytesFast(cipherBytes, cipher, cipherKey, cipherHash, fishOnAesEngine);
                    cipherBytes = decryptedBytes;
#if DEBUG
                    stageDictionary.Add(cipher, cipherBytes);
#endif
                }

            return cipherBytes;        
        }


        /// <summary>
        /// EncrpytToStringMerryGoRound
        /// </summary>
        /// <param name="inString"></param>
        /// <param name="serverKey"></param>
        /// <param name="encType"></param>
        /// <param name="unzipAfter"><see cref="ZipType"/> and <see cref="ZipTypeExtensions.Zip(ZipType, byte[])"/></param>
        /// <returns>encrypted string</returns>
        public static string EncrpytToStringMerryGoRound(string inString, string serverKey, EncodingType encoding = EncodingType.Base64, ZipType zipBefore = ZipType.None)
        {
            string encrypted = "", hash = EnDeCodeHelper.KeyToHex(serverKey);
            byte[] inBytes = EnDeCodeHelper.GetBytesFromString(inString);

            SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, hash);
            byte[] encryptedBytes = symmPipe.MerryGoRoundEncrpyt(inBytes, serverKey, hash, zipBefore);

            encrypted = encoding.GetEnCoder().EnCode(encryptedBytes);

            return encrypted;
        }

        /// <summary>
        /// DecrpytToStringRoundGoMerry
        /// </summary>
        /// <param name="cryptedEncodedMsg"></param>
        /// <param name="serverKey"></param>
        /// <param name="encType"></param>
        /// <param name="unzipAfter"><see cref="ZipType"/> and <see cref="ZipTypeExtensions.Unzip(ZipType, byte[])"/></param>
        /// <returns></returns>
        public static string DecrpytToStringRoundGoMerry(string cryptedEncodedMsg, string serverKey, EncodingType decoding = EncodingType.Base64, ZipType unzipAfter = ZipType.None)
        {
            string decrypted = "", hash = EnDeCodeHelper.KeyToHex(serverKey);
            byte[] cipherBytes = decoding.GetEnCoder().DeCode(cryptedEncodedMsg);

            SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, hash);
            byte[] unroundedMerryBytes = symmPipe.DecrpytRoundGoMerry(cipherBytes, serverKey, hash, unzipAfter);
            decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes); //DeEnCoder.GetStringFromBytesTrimNulls(unroundedMerryBytes);
            while (decrypted[decrypted.Length - 1] == '\0')
                decrypted = decrypted.Substring(0, decrypted.Length - 1);

            return decrypted;
        }


    }

}
