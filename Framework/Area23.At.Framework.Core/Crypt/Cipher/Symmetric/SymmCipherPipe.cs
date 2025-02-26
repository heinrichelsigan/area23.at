using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{


    /// <summary>
    /// Provides a simple crypt pipe for <see cref="SymmCipherEnum"/>
    /// </summary>
    public class SymmCipherPipe
    {
        private readonly SymmCipherEnum[] inPipe;
        public readonly SymmCipherEnum[] outPipe;
        private readonly string pipeString;

        public SymmCipherEnum[] InPipe { get => inPipe; }

        public SymmCipherEnum[] OutPipe { get => outPipe; }

        public string PipeString { get => pipeString; }

#if DEBUG
        public Dictionary<SymmCipherEnum, byte[]> stageDictionary;

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

        #region ctor

        public SymmCipherPipe(SymmCipherEnum[] symmCipherEnums)
        {
            inPipe = new List<SymmCipherEnum>(symmCipherEnums).ToArray();
            outPipe = symmCipherEnums.Reverse<SymmCipherEnum>().ToArray();
            foreach (SymmCipherEnum symmCipher in inPipe)
                pipeString += symmCipher.GetSymmCipherChar();

        }

        /// <summary>
        /// SymmCipherPipe ctor with array of user key bytes
        /// </summary>
        /// <param name="keyBytes">user key bytes</param>
        /// <param name="maxpipe">maximum lentgh <see cref="Constants.MAX_PIPE_LEN"/></param>
        public SymmCipherPipe(byte[] keyBytes, uint maxpipe = 8)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = (maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe; // if somebody wants more, he/she/it gets less

            ushort scnt = 0;
            List<SymmCipherEnum> pipeList = new List<SymmCipherEnum>();
            Dictionary<char, SymmCipherEnum> symDict = new Dictionary<char, SymmCipherEnum>();
            foreach (SymmCipherEnum symmC in Enum.GetValues(typeof(SymmCipherEnum)))
            {
                string hex = $"{((ushort)symmC):x1}";
                scnt++;
                symDict.Add(hex[0], symmC);
            }

            string hexString = string.Empty;
            HashSet<char> hashBytes = new HashSet<char>();
            foreach (byte bb in keyBytes)
            {
                hexString = string.Format("{0:x2}", bb);
                if (hexString.Length > 0 && !hashBytes.Contains(hexString[0]))
                    hashBytes.Add(hexString[0]);
                if (hexString.Length > 0 && !hashBytes.Contains(hexString[1]))
                    hashBytes.Add(hexString[1]);
            }

            hexString = string.Empty;
            for (int kcnt = 0; kcnt < hashBytes.Count && pipeList.Count < maxpipe; kcnt++)
            {
                hexString += hashBytes.ElementAt(kcnt).ToString();
                SymmCipherEnum sym0 = symDict[hashBytes.ElementAt(kcnt)];
                pipeList.Add(sym0);
            }

            inPipe = new List<SymmCipherEnum>(pipeList).ToArray();
            outPipe = pipeList.Reverse<SymmCipherEnum>().ToArray();

            foreach (SymmCipherEnum symmCipher in inPipe)
                pipeString += symmCipher.GetSymmCipherChar();

        }

        #endregion ctor

        #region static members EncryptBytesFast DecryptBytesFast

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytesFast(byte[] inBytes,
            SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes,
            string secretKey = "heinrich.elsigan@area23.at",
            string hashIv = "6865696e726963682e656c736967616e406172656132332e6174")
        {
            byte[] encryptBytes = inBytes;

            string algo = cipherAlgo.ToString();

            switch (cipherAlgo)
            {
                //case SymmCipherEnum.Des3:
                //    Des3.Des3GenWithKeyHash(secretKey, hashIv, true);
                //    encryptBytes = Des3.Encrypt(inBytes);
                //    break;
                //case SymmCipherEnum.Fish2:
                //    Fish2.Fish2GenWithKeyHash(secretKey, hashIv, true);
                //    encryptBytes = Fish2.Encrypt(inBytes);
                //    break;
                //case SymmCipherEnum.Fish3:
                //    Fish3.Fish3GenWithKeyHash(secretKey, hashIv, true);
                //    encryptBytes = Fish3.Encrypt(inBytes);
                //    break;
                //case SymmCipherEnum.Rijndael:
                //    Rijndael.RijndaelGenWithNewKey(secretKey, hashIv, true);
                //    encryptBytes = Rijndael.Encrypt(inBytes);
                //    break;
                case SymmCipherEnum.Serpent:
                    Serpent.SerpentGenWithKey(secretKey, hashIv, true);
                    encryptBytes = Serpent.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.ZenMatrix:
                    encryptBytes = (new ZenMatrix(secretKey, hashIv, false)).Encrypt(inBytes);
                    break;
                case SymmCipherEnum.Aes:
                //Aes.AesGenWithKeyHash(secretKey, hashIv, true);
                //encryptBytes = Aes.Encrypt(inBytes);
                //break;
                case SymmCipherEnum.BlowFish:
                case SymmCipherEnum.Fish2:
                case SymmCipherEnum.Fish3:
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.RC532:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                default:
                    CryptParamsPrefered cpParams = CryptHelper.GetPreferedCryptParams(cipherAlgo);
                    cpParams.Key = secretKey;
                    cpParams.Hash = hashIv;

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
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/>both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytesFast(byte[] cipherBytes,
            SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes,
            string secretKey = "heinrich.elsigan@area23.at",
            string hashIv = "6865696e726963682e656c736967616e406172656132332e6174",
            bool fishOnAesEngine = false)
        {
            bool sameKey = true;
            string algorithmName = cipherAlgo.ToString();
            byte[] decryptBytes = cipherBytes;

            switch (cipherAlgo)
            {

                //case SymmCipherEnum.Des3:
                //    sameKey = Des3.Des3GenWithKeyHash(secretKey, hashIv, true);
                //    decryptBytes = Des3.Decrypt(cipherBytes);
                //    break;
                //case SymmCipherEnum.Fish2:
                //    sameKey = Fish2.Fish2GenWithKeyHash(secretKey, hashIv, true);
                //    decryptBytes = Fish2.Decrypt(cipherBytes);
                //    break;
                //case SymmCipherEnum.Fish3:
                //    sameKey = Fish3.Fish3GenWithKeyHash(secretKey, hashIv, true);
                //    decryptBytes = Fish3.Decrypt(cipherBytes);
                //    break;
                //case SymmCipherEnum.Rijndael:
                // sameKey = Rijndael.RijndaelGenWithNewKey(secretKey, hashIv, true);
                // decryptBytes = Rijndael.Decrypt(cipherBytes);
                // break;
                case SymmCipherEnum.Serpent:
                    sameKey = Serpent.SerpentGenWithKey(secretKey, hashIv, true);
                    decryptBytes = Serpent.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.ZenMatrix:
                    decryptBytes = (new ZenMatrix(secretKey, hashIv, false)).Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Aes:
                // sameKey = Aes.AesGenWithKeyHash(secretKey, hashIv, true);
                // decryptBytes = Aes.Decrypt(cipherBytes);
                // break;
                case SymmCipherEnum.BlowFish:
                case SymmCipherEnum.Fish2:
                case SymmCipherEnum.Fish3:
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.Des3:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.RC532:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                default:
                    CryptParamsPrefered cpParams = CryptHelper.GetPreferedCryptParams(cipherAlgo, fishOnAesEngine);
                    cpParams.Key = secretKey;
                    cpParams.Hash = hashIv;

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
        /// <returns>encrypted byte[]</returns>
        public byte[] MerryGoRoundEncrpyt(byte[] inBytes,
            string secretKey = "heinrich.elsigan@area23.at",
            string hashIv = "6865696e726963682e656c736967616e406172656132332e6174")
        {
            byte[] encryptedBytes = new byte[inBytes.Length * 3 + 1];
#if DEBUG
            stageDictionary = new Dictionary<SymmCipherEnum, byte[]>();
            // stageDictionary.Add(SymmCipherEnum.ZenMatrix, inBytes);
#endif
            foreach (SymmCipherEnum symmCipher in InPipe)
            {
                encryptedBytes = EncryptBytesFast(inBytes, symmCipher, secretKey, hashIv);
                inBytes = encryptedBytes;
#if DEBUG
                stageDictionary.Add(symmCipher, encryptedBytes);
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
        public byte[] DecrpytRoundGoMerry(byte[] cipherBytes,
            string secretKey = "heinrich.elsigan@area23.at",
            string hashIv = "6865696e726963682e656c736967616e406172656132332e6174",
            bool fishOnAesEngine = false)
        {

            byte[] decryptedBytes = new byte[cipherBytes.Length * 3 + 1];
#if DEBUG
            stageDictionary = new Dictionary<SymmCipherEnum, byte[]>();
            // stageDictionary.Add(SymmCipherEnum.ZenMatrix, cipherBytes);
#endif 
            foreach (SymmCipherEnum symmCipher in OutPipe)
            {
                decryptedBytes = DecryptBytesFast(cipherBytes, symmCipher, secretKey, hashIv, fishOnAesEngine);
                cipherBytes = decryptedBytes;
#if DEBUG
                stageDictionary.Add(symmCipher, cipherBytes);
#endif
            }

            return cipherBytes;
        }

    }

}
