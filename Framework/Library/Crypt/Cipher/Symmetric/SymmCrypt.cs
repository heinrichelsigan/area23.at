using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// Basic class for symmetric cipher encryption
    /// </summary>
    public class SymmCrypt : Area23.At.Framework.Library.Crypt.Cipher.Crypt
    {

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>encrypted byte Array</returns>
        public static new byte[] EncryptBytes(byte[] inBytes, SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes, 
            string secretKey = "postmaster@kernel.org", string hashIv = "")
        {
            byte[] encryptBytes = inBytes;

            string algo = cipherAlgo.ToString();            
            if (cipherAlgo == SymmCipherEnum.Des3 || algo == "3Des" || algo == "Des3")
            {
                Des3.Des3FromKey(secretKey, hashIv, true);
                encryptBytes = Des3.Encrypt(inBytes);
            }
            if (cipherAlgo == SymmCipherEnum.Aes || algo == "Aes")
            {
                Aes.AesGenWithNewKey(secretKey, hashIv, true);
                encryptBytes = Aes.Encrypt(inBytes);
            }
            if (cipherAlgo == SymmCipherEnum.Serpent || algo == "Serpent")
            {
                Serpent.SerpentGenWithKey(secretKey, hashIv, true);
                encryptBytes = Serpent.Encrypt(inBytes);
            }
            if (cipherAlgo == SymmCipherEnum.ZenMatrix || algo == "ZenMatrix")
            {
                ZenMatrix.ZenMatrixGenWithKey(secretKey, hashIv, true);
                encryptBytes = ZenMatrix.Encrypt(inBytes);
            }
            if (algo == "BlowFish" ||  algo == "2Fish" || algo == "Fish2" || algo == "3Fish" || algo == "Fish3" ||
                algo == "Camellia" ||
                algo == "Gost28147" || algo == "Idea" ||
                algo == "RC532" || algo == "Cast6" ||
                algo == "Seed" || algo == "SkipJack" ||
                algo == "Tea" || algo == "XTea" ||
                cipherAlgo == SymmCipherEnum.BlowFish || cipherAlgo == SymmCipherEnum.Fish2 || cipherAlgo == SymmCipherEnum.Fish3 ||
                cipherAlgo == SymmCipherEnum.Camellia ||
                cipherAlgo == SymmCipherEnum.Gost28147 || cipherAlgo == SymmCipherEnum.Idea ||
                cipherAlgo == SymmCipherEnum.RC532 || cipherAlgo == SymmCipherEnum.Cast6 ||
                cipherAlgo == SymmCipherEnum.Seed || cipherAlgo == SymmCipherEnum.SkipJack ||
                cipherAlgo == SymmCipherEnum.Tea || cipherAlgo == SymmCipherEnum.XTea)
            {
                CryptParamsPrefered cpParams = CryptHelper.GetPreferedBlockCipher(cipherAlgo);
                cpParams.Key = secretKey;
                cpParams.Hash = hashIv;
                Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);
                encryptBytes = cryptBounceCastle.Encrypt(inBytes);

            }

            return encryptBytes;
        }


        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="cipherAlgo">both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>decrypted byte Array</returns>
        public static new byte[] DecryptBytes(byte[] cipherBytes, SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes, 
            string secretKey = "postmaster@kernel.org", string hashIv = "")
        {
            bool sameKey = true;
            string algorithmName = cipherAlgo.ToString();
            byte[] decryptBytes = cipherBytes;

 
            if (cipherAlgo == SymmCipherEnum.Des3 || algorithmName == "3Des" || algorithmName == "Des3")
            {
                sameKey = Des3.Des3FromKey(secretKey, hashIv, true);
                decryptBytes = Des3.Decrypt(cipherBytes);
            }
            if (cipherAlgo == SymmCipherEnum.Aes || algorithmName == "Aes")
            {
                sameKey = Aes.AesGenWithNewKey(secretKey, hashIv, true);
                decryptBytes = Aes.Decrypt(cipherBytes);
            }
            if (cipherAlgo == SymmCipherEnum.Serpent || algorithmName == "Serpent")
            {
                sameKey = Serpent.SerpentGenWithKey(secretKey, hashIv, true);
                decryptBytes = Serpent.Decrypt(cipherBytes);
            }
            if (cipherAlgo == SymmCipherEnum.ZenMatrix || algorithmName == "ZenMatrix")
            {
                sameKey = ZenMatrix.ZenMatrixGenWithKey(secretKey, hashIv, true);
                decryptBytes = ZenMatrix.Decrypt(cipherBytes);
            }
            if (algorithmName == "BlowFish" || algorithmName == "2Fish" || algorithmName == "Fish2" || algorithmName == "3Fish" || algorithmName == "Fish3" ||
                algorithmName == "Camellia" || algorithmName == "Cast5" || algorithmName == "Gost28147" || 
                algorithmName == "Idea" || algorithmName == "RC532" || algorithmName == "RC564" || algorithmName == "Cast6" || algorithmName == "Seed" || 
                algorithmName == "SkipJack" || algorithmName == "Tea" || algorithmName == "XTea" ||

                cipherAlgo == SymmCipherEnum.BlowFish || cipherAlgo == SymmCipherEnum.Fish2 || cipherAlgo == SymmCipherEnum.Fish3 || 
                cipherAlgo == SymmCipherEnum.Camellia || cipherAlgo == SymmCipherEnum.Gost28147 || cipherAlgo == SymmCipherEnum.Idea ||
                cipherAlgo == SymmCipherEnum.RC532 || cipherAlgo == SymmCipherEnum.Cast6 || cipherAlgo == SymmCipherEnum.Seed || 
                cipherAlgo == SymmCipherEnum.SkipJack || cipherAlgo == SymmCipherEnum.Tea || cipherAlgo == SymmCipherEnum.XTea)
            {
                CryptParamsPrefered cpParams = CryptHelper.GetPreferedBlockCipher(cipherAlgo);
                cpParams.Key = secretKey;
                cpParams.Hash = hashIv;
                Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);
                decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
            }

            return DeEnCoder.GetBytesTrimNulls(decryptBytes);
            // return decryptBytes;
        }


        #region merry_go_rount

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytesFast(byte[] inBytes, SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes, 
            string secretKey = "postmaster@kernel.org", string hashIv = "")
        {
            byte[] encryptBytes = inBytes;

            string algo = cipherAlgo.ToString();

            switch (cipherAlgo)
            {
                case SymmCipherEnum.Des3:
                    Des3.Des3FromKey(secretKey, hashIv, true);
                    encryptBytes = Des3.Encrypt(inBytes);
                    break;                
                case SymmCipherEnum.Serpent:
                    Serpent.SerpentGenWithKey(secretKey, hashIv, true);
                    encryptBytes = Serpent.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.ZenMatrix:
                    ZenMatrix.ZenMatrixGenWithKey(secretKey, hashIv, true);
                    encryptBytes = ZenMatrix.Encrypt(inBytes);
                    break;
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
                    CryptParamsPrefered cpParams = CryptHelper.GetPreferedBlockCipher(cipherAlgo);
                    cpParams.Key = secretKey;
                    cpParams.Hash = hashIv;
                    Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);                    
                    encryptBytes = cryptBounceCastle.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.Aes:
                default:
                    Aes.AesGenWithNewKey(secretKey, hashIv, true);
                    encryptBytes = Aes.Encrypt(inBytes);
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
        public static byte[] DecryptBytesFast(byte[] cipherBytes, SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes, 
            string secretKey = "postmaster@kernel.org", string hashIv = "")
        {
            bool sameKey = true;
            string algorithmName = cipherAlgo.ToString();
            byte[] decryptBytes = cipherBytes;

            switch (cipherAlgo)
            {

                case SymmCipherEnum.Des3:
                    sameKey = Des3.Des3FromKey(secretKey, hashIv, true);
                    decryptBytes = Des3.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Serpent:
                    sameKey = Serpent.SerpentGenWithKey(secretKey, hashIv, true);
                    decryptBytes = Serpent.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.ZenMatrix:
                    sameKey = ZenMatrix.ZenMatrixGenWithKey(secretKey, hashIv, true);
                    decryptBytes = ZenMatrix.Decrypt(cipherBytes);
                    break;
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
                    CryptParamsPrefered cpParams = CryptHelper.GetPreferedBlockCipher(cipherAlgo);
                    cpParams.Key = secretKey;
                    cpParams.Hash = hashIv;
                    Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);                    
                    decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Aes:
                default:
                    sameKey = Aes.AesGenWithNewKey(secretKey, hashIv, false);
                    decryptBytes = Aes.Decrypt(cipherBytes);
                    break;
            }

            return DeEnCoder.GetBytesTrimNulls(decryptBytes);
            // return decryptBytes;
        }


        public static char GetSymmCipherChar(SymmCipherEnum symmCipher)
        {
            switch (symmCipher)            
            {
                case SymmCipherEnum.Aes: return 'A';

                case SymmCipherEnum.BlowFish: return 'b';
                case SymmCipherEnum.Camellia: return 'M';
                case SymmCipherEnum.Cast6: return 'C';
                case SymmCipherEnum.Des3: return 'D';
                case SymmCipherEnum.Fish2: return 'f';
                case SymmCipherEnum.Fish3: return 'F';
                case SymmCipherEnum.Gost28147: return 'g';

                case SymmCipherEnum.Idea: return 'I';
                case SymmCipherEnum.RC532: return '5';                             
                case SymmCipherEnum.Seed: return 's';
                case SymmCipherEnum.Serpent: return 'S';
                case SymmCipherEnum.SkipJack: return 'J';
                case SymmCipherEnum.Tea: return 't';
                case SymmCipherEnum.XTea: return 'X';

                case SymmCipherEnum.ZenMatrix: return 'z';               
            }

            return ((char)('A'));
        }

        public static string SymmCipherPipeString(SymmCipherEnum[] symmCiphers)
        {
            string hashSymms = string.Empty;

            foreach (SymmCipherEnum symmCipher in symmCiphers)
            {
                hashSymms += GetSymmCipherChar(symmCipher);
            }

            return hashSymms;
        }

        /// <summary>
        /// Gets a encrypted matrix pipeline for a byte arry
        /// </summary>
        /// <param name="keyBytes">private key or hased private key with iv</param>
        /// <param name="maxpipe">maximal numbers of encryption cycles</param>
        /// <returns>Array of <see cref="SymmCipherEnum"/>, that is used to perform <see cref="MerryGoRoundEncrpyt(byte[], string, string)"/> encryption cycles</returns>
        public static SymmCipherEnum[] KeyBytesToSymmCipherPipeline(byte[] keyBytes, uint maxpipe = 8)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = (maxpipe > 16) ? 8 : maxpipe; // if somebody wants more, he/she/it gets less

            Dictionary<char, SymmCipherEnum> symDict = new Dictionary<char, SymmCipherEnum>();
            List<SymmCipherEnum> symmMatrixPipe = new List<SymmCipherEnum>();

            ushort scnt = 0;
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
            for (int kcnt = 0; kcnt < hashBytes.Count && symmMatrixPipe.Count < maxpipe; kcnt++)
            {
                hexString += hashBytes.ElementAt(kcnt).ToString();
                SymmCipherEnum sym0 = symDict[hashBytes.ElementAt(kcnt)];
                symmMatrixPipe.Add(sym0);
            }

            return symmMatrixPipe.ToArray();
        }


        /// <summary>
        /// MerryGoRoundEncrpyt starts merry to go arround from left to right in clock hour cycle
        /// </summary>
        /// <param name="inBytes">plain <see cref="byte[]"/ to encrypt></param>
        /// <param name="secretKey">user secret key to use for all symmetric cipher algorithms in the pipe</param>
        /// <param name="keyIv">hash key iv relational to secret key</param>
        /// <returns>encrypted byte[]</returns>
        public static byte[] MerryGoRoundEncrpyt(byte[] inBytes, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] keyHashBytes = CryptHelper.GetUserKeyBytes(secretKey, keyIv, 16);
            byte[] encryptedBytes = new byte[inBytes.Length * 2];
            // Array.Copy(inBytes, 0, encryptedBytes, 0, inBytes.Length);

            foreach (var symmCipher in KeyBytesToSymmCipherPipeline(keyHashBytes))
            {
                encryptedBytes = EncryptBytesFast(inBytes, symmCipher, secretKey, keyIv);
                inBytes = encryptedBytes;
            }

            return encryptedBytes;
        }

        /// <summary>
        /// DecrpytRoundGoMerry against clock turn -
        /// starts merry to turn arround from right to left against clock hour cycle 
        /// </summary>
        /// <param name="cipherBytes">encrypted byte array</param>
        /// <param name="secretKey">user secret key, normally email address</param>
        /// <param name="keyIv">hash relational to secret kay</param>
        /// <returns><see cref="byte[]"/> plain bytes</returns>
        public static byte[] DecrpytRoundGoMerry(byte[] cipherBytes, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] keyHashBytes = CryptHelper.GetUserKeyBytes(secretKey, keyIv, 16);
            byte[] outBytes = new byte[cipherBytes.Length * 2];
            // Array.Copy(inBytes, 0, encryptedBytes, 0, inBytes.Length);

            SymmCipherEnum[] symCiphers = KeyBytesToSymmCipherPipeline(keyHashBytes);
            for (int scnt = symCiphers.Length - 1; scnt >= 0; scnt--)
            {
                outBytes = DecryptBytesFast(cipherBytes, symCiphers[scnt], secretKey, keyIv);
                cipherBytes = outBytes;
            }

            return outBytes;
        }


        #endregion merry_go_rount

    }

}
