using Area23.At.Framework.Library.Core.Crypt.EnDeCoding;

namespace Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// Basic class for symmetric cipher encryption
    /// </summary>
    public class SymmCrypt : Area23.At.Framework.Library.Core.Crypt.Cipher.Crypt
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
                CryptParamsPrefered cpParams = CryptHelper.GetPreferedCryptParams(cipherAlgo);
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
                algorithmName == "Idea" || algorithmName == "RC532" || algorithmName == "Cast6" || algorithmName == "Seed" || 
                algorithmName == "SkipJack" || algorithmName == "Tea" || algorithmName == "XTea" ||

                cipherAlgo == SymmCipherEnum.BlowFish || cipherAlgo == SymmCipherEnum.Fish2 || cipherAlgo == SymmCipherEnum.Fish3 || 
                cipherAlgo == SymmCipherEnum.Camellia || cipherAlgo == SymmCipherEnum.Gost28147 || cipherAlgo == SymmCipherEnum.Idea ||
                cipherAlgo == SymmCipherEnum.RC532 || cipherAlgo == SymmCipherEnum.Cast6 || cipherAlgo == SymmCipherEnum.Seed || 
                cipherAlgo == SymmCipherEnum.SkipJack || cipherAlgo == SymmCipherEnum.Tea || cipherAlgo == SymmCipherEnum.XTea)
            {
                CryptParamsPrefered cpParams = CryptHelper.GetPreferedCryptParams(cipherAlgo);
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
                    CryptParamsPrefered cpParams = CryptHelper.GetPreferedCryptParams(cipherAlgo);
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
                    CryptParamsPrefered cpParams = CryptHelper.GetPreferedCryptParams(cipherAlgo);
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
                case SymmCipherEnum.Fish2: return '2';
                case SymmCipherEnum.Fish3: return '3';
                case SymmCipherEnum.RC532: return '5';
                case SymmCipherEnum.Cast6: return '6';
                case SymmCipherEnum.Aes: return 'A';
                case SymmCipherEnum.BlowFish: return 'b';
                case SymmCipherEnum.Camellia: return 'C';
                case SymmCipherEnum.Gost28147: return 'g';
                case SymmCipherEnum.Idea: return 'I';
                case SymmCipherEnum.Seed: return 's';
                case SymmCipherEnum.Serpent: return 'S';
                case SymmCipherEnum.SkipJack: return 'J';
                case SymmCipherEnum.Tea: return 't';
                case SymmCipherEnum.XTea: return 'X';
                case SymmCipherEnum.ZenMatrix: return 'z';
                case SymmCipherEnum.Des3: return 'T';


            }
            return ((char)('0'));
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

            SymmCipherPipe spipe = new SymmCipherPipe(keyHashBytes, 8);
            foreach (var symmCipher in spipe.InPipe)
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

            SymmCipherPipe dspipe = new SymmCipherPipe(keyHashBytes, 8);
            foreach (var symmCipher in dspipe.OutPipe)
            {
                outBytes = DecryptBytesFast(cipherBytes, symmCipher, secretKey, keyIv);
                cipherBytes = outBytes;
            }

            return outBytes;
        }


        #endregion merry_go_rount

    }

}
