using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zfx;

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
        private string symmCipherKey = "", symmCipherHash = "";

        public SymmCipherEnum[] InPipe { get => inPipe; }

        public SymmCipherEnum[] OutPipe { get => outPipe; }

        public string PipeString { get => pipeString; }

#if DEBUG
        public Dictionary<SymmCipherEnum, byte[]> stageDictionary = new Dictionary<SymmCipherEnum, byte[]>();

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

        #region ctor SymmCipherPipe

        /// <summary>
        /// SymmCipherPipe constructor with an array of <see cref="SymmCipherEnum[]"/> as inpipe
        /// </summary>
        /// <param name="symmCipherEnums">array of <see cref="SymmCipherEnum[]"/> as inpipe</param>
        public SymmCipherPipe(SymmCipherEnum[] symmCipherEnums)
        {
            inPipe = new List<SymmCipherEnum>(symmCipherEnums).ToArray();
            outPipe = symmCipherEnums.Reverse<SymmCipherEnum>().ToArray();
            pipeString = "";
            foreach (SymmCipherEnum symmCipher in inPipe)
                pipeString += symmCipher.GetSymmCipherChar();
        }

        /// <summary>
        /// SymmCipherPipe constructor with an array of <see cref="string[]"/> as inpipe
        /// </summary>
        /// <param name="symmCipherAlgos">array of <see cref="string[]"/> as inpipe</param>
        public SymmCipherPipe(string[] symmCipherAlgos)
        {
            List<SymmCipherEnum> symmCipherEnums = new List<SymmCipherEnum>();
            foreach (string algo in symmCipherAlgos)
            {
                if (!string.IsNullOrEmpty(algo))
                {
                    SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes;
                    if (!Enum.TryParse<SymmCipherEnum>(algo, out cipherAlgo))
                        cipherAlgo = SymmCipherEnum.Aes;

                    symmCipherEnums.Add(cipherAlgo);
                }
            }

            inPipe = new List<SymmCipherEnum>(symmCipherEnums).ToArray();
            outPipe = symmCipherEnums.Reverse<SymmCipherEnum>().ToArray();
            pipeString = "";
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
            Dictionary<char, SymmCipherEnum> symDict = SymmCipherEnumExtensions.GetCharSymmCipherDict();
            SymmCipherEnum[] symmCiphers = SymmCipherEnumExtensions.GetSymmCipherTypes();

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
            Area23Log.LogOriginMsg("SymmCipherPipe", $"Generating symmetric encryption cipher pipe: {hexString}");

            inPipe = new List<SymmCipherEnum>(pipeList).ToArray();
            outPipe = pipeList.Reverse<SymmCipherEnum>().ToArray();
            pipeString = "";
            foreach (SymmCipherEnum symmCipher in inPipe)
                pipeString += symmCipher.GetSymmCipherChar();
        }

        /// <summary>
        /// Constructs a <see cref="SymmCipherPipe"/> from key and hash
        /// by getting <see cref="byte[]">byte[] keybytes</see> with <see cref="CryptHelper.GetUserKeyBytes(string, string, int)"/>
        /// </summary>
        /// <param name="key">secret key to generate pipe</param>
        /// <param name="hash">hash value of secret key</param>
        public SymmCipherPipe(string key = "heinrich.elsigan@area23.at", string hash = "6865696e726963682e656c736967616e406172656132332e6174")
            : this(CryptHelper.GetUserKeyBytes(key, hash, 16), Constants.MAX_PIPE_LEN)
        {
            symmCipherKey = key;
            symmCipherHash = hash;
        }

        /// <summary>
        /// SymmCipherPipe ctor with only key
        /// </summary>
        /// <param name="key"></param>
        public SymmCipherPipe(string key = "heinrich.elsigan@area23.at") : this(key, EnDeCodeHelper.KeyToHex(key))
        {
            symmCipherKey = key;
        }

        #endregion ctor SymmCipherPipe

        #region static members EncryptBytesFast DecryptBytesFast

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytesFast(byte[] inBytes, SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes,
            string secretKey = "heinrich.elsigan@area23.at", string hashIv = "")
        {
            byte[] encryptBytes = inBytes;
            string hash = (string.IsNullOrEmpty(hashIv)) ? EnDeCodeHelper.KeyToHex(secretKey) : hashIv;

            switch (cipherAlgo)
            {
                case SymmCipherEnum.Serpent:
                    Serpent.SerpentGenWithKey(secretKey, hash, true);
                    encryptBytes = Serpent.Encrypt(inBytes);
                    break;                                    
                case SymmCipherEnum.Aes:
                case SymmCipherEnum.BlowFish:
                case SymmCipherEnum.Fish2:
                case SymmCipherEnum.Fish3:
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.Des3:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.RC532:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                case SymmCipherEnum.ZenMatrix:
                default:
                    CryptParamsPrefered cpParams = new CryptParamsPrefered(cipherAlgo, secretKey, hash);
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
        public static byte[] DecryptBytesFast(byte[] cipherBytes, SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes,
            string secretKey = "heinrich.elsigan@area23.at", string hashIv = "", bool fishOnAesEngine = false)
        {
            bool sameKey = true;
            string hash = (string.IsNullOrEmpty(hashIv)) ? EnDeCodeHelper.KeyToHex(secretKey) : hashIv;
            byte[] decryptBytes = cipherBytes;

            switch (cipherAlgo)
            {
                case SymmCipherEnum.Serpent:
                    sameKey = Serpent.SerpentGenWithKey(secretKey, hash, true);
                    decryptBytes = Serpent.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Aes:
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
                case SymmCipherEnum.ZenMatrix:
                default:
                    CryptParamsPrefered cpParams = new CryptParamsPrefered(cipherAlgo, secretKey, hash, fishOnAesEngine);
                    Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);
                    decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
                    break;
            }

            return EnDeCodeHelper.GetBytesTrimNulls(decryptBytes);
        }

		#endregion static members EncryptBytesFast DecryptBytesFast

		#region multiple rounds en-de-cryption

		/// <summary>
		/// MerryGoRoundEncrpyt starts merry to go arround from left to right in clock hour cycle
		/// </summary>
		/// <param name="inBytes">plain <see cref="byte[]"/ to encrypt></param>
		/// <param name="secretKey">user secret key to use for all symmetric cipher algorithms in the pipe</param>
		/// <param name="hashIv">hash key iv relational to secret key</param>
		/// <param name="zipBefore"><see cref="ZipType"/> and <see cref="ZipTypeExtensions.Zip(ZipType, byte[])"/></param>
		/// <returns>encrypted byte[]</returns>
		public byte[] MerryGoRoundEncrpyt(byte[] inBytes, string secretKey = "", string hashIv = "", ZipType zipBefore = ZipType.None)
        {
            if (string.IsNullOrEmpty(secretKey) && string.IsNullOrEmpty(symmCipherKey))
                throw new ArgumentNullException("secretKey");
            if (string.IsNullOrEmpty(hashIv) && string.IsNullOrEmpty(symmCipherHash))
                hashIv = EnDeCodeHelper.KeyToHex(secretKey);
            symmCipherKey = secretKey;
            symmCipherHash = hashIv;

            byte[] encryptedBytes = new byte[inBytes.Length];
            Array.Copy(inBytes, 0, encryptedBytes, 0, inBytes.Length);
#if DEBUG
            stageDictionary = new Dictionary<SymmCipherEnum, byte[]>();
            // stageDictionary.Add(SymmCipherEnum.ZenMatrix, inBytes);
#endif
            if (zipBefore != ZipType.None)
            {
                encryptedBytes = zipBefore.Zip(inBytes);
                inBytes = encryptedBytes;
            }

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
        /// <param name="unzipAfter"><see cref="ZipType"/> and <see cref="ZipTypeExtensions.Unzip(ZipType, byte[])"/></param>
        /// <returns><see cref="byte[]"/> plain bytes</returns>
        public byte[] DecrpytRoundGoMerry(byte[] cipherBytes, string secretKey = "", string hashIv = "", ZipType unzipAfter = ZipType.None)
        {
            if (string.IsNullOrEmpty(symmCipherKey) && string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("secretKey");

            symmCipherKey = string.IsNullOrEmpty(secretKey) ? symmCipherKey : secretKey;

            if (string.IsNullOrEmpty(hashIv) && string.IsNullOrEmpty(symmCipherHash))
                hashIv = EnDeCodeHelper.KeyToHex(symmCipherKey);

            symmCipherHash = string.IsNullOrEmpty(hashIv) ? symmCipherHash : hashIv;

            long outByteLen = (OutPipe == null || OutPipe.Length == 0) ? cipherBytes.Length : ((cipherBytes.Length * 3) + 1);
            byte[] decryptedBytes = new byte[outByteLen];
#if DEBUG
            stageDictionary = new Dictionary<SymmCipherEnum, byte[]>();
            // stageDictionary.Add(SymmCipherEnum.ZenMatrix, cipherBytes);
#endif 
            if (OutPipe == null || OutPipe.Length == 0)
                Array.Copy(cipherBytes, 0, decryptedBytes, 0, cipherBytes.Length);
            else
                foreach (SymmCipherEnum symmCipher in OutPipe)
                {
                    decryptedBytes = DecryptBytesFast(cipherBytes, symmCipher, secretKey, hashIv);
                    cipherBytes = decryptedBytes;
#if DEBUG
                    stageDictionary.Add(symmCipher, cipherBytes);
#endif
                }

            if (unzipAfter != ZipType.None)
                decryptedBytes = unzipAfter.Unzip(cipherBytes);

            return decryptedBytes;
        }


        public byte[] EncrpytGoRounds(byte[] inBytes, string secretKey = "", ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
            => MerryGoRoundEncrpyt(inBytes, secretKey, keyHash.Hash(secretKey), zipBefore);

        public byte[] DecrpytRoundsGo(byte[] cipherBytes, string secretKey = "", ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
            => DecrpytRoundGoMerry(cipherBytes, secretKey, keyHash.Hash(secretKey), unzipAfter);

        #region static en-de-crypt members

        /// <summary>
        /// EncrpytToStringd
        /// </summary>
        /// <param name="inString">string to encrypt multiple times</param>
        /// <param name="cryptKey">Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline 
        /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe</param>
        /// <param name="pipeStrig">out parameter for setting hash to compare entities encryption</param>
        /// <param name="encoding"><see cref="EncodingType"/ type for encoding encrypted bytes back in plain text></param>
        /// <param name="zipBefore">Zip bytes with <see cref="ZipType"/> before passing them in encrypted stage pipeline. <see cref="ZipTypeExtensions.Zip(ZipType, byte[])"/></param>
        /// <param name="keyHash"><see cref="KeyHash"/> hashing key algorithm</param>
        /// <returns>encrypted string</returns>        
        /// <returns></returns>
        public static string EncrpytToString(string inString, string cryptKey, out string pipeString,
            EncodingType encoding = EncodingType.Base64, ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            // construct symmetric cipher pipeline with cryptKey and pass pipeString as out param            
            SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
            pipeString = symmPipe.PipeString;

            // Transform string to bytes
            byte[] inBytes = EnDeCodeHelper.GetBytesFromString(inString);
            // perform multi crypt pipe stages
            byte[] encryptedBytes = symmPipe.EncrpytGoRounds(inBytes, cryptKey, zipBefore, keyHash);
            // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
            string encrypted = encoding.GetEnCoder().Encode(encryptedBytes);

            return encrypted;
        }

		public static byte[] EncrpytStringToBytes(string inString, string cryptKey, out string pipeString,
			EncodingType encoding = EncodingType.Base64, ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
			// construct symmetric cipher pipeline with cryptKey and pass pipeString as out param            
			SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
			pipeString = symmPipe.PipeString;

			// Transform string to bytes
			byte[] inBytes = EnDeCodeHelper.GetBytesFromString(inString);
			// perform multi crypt pipe stages
			byte[] encryptedBytes = symmPipe.EncrpytGoRounds(inBytes, cryptKey, zipBefore, keyHash);			

			return encryptedBytes;
		}

		public static string EncrpytBytesToString(byte[] plainBytes, string cryptKey, out string pipeString,
            EncodingType encoding = EncodingType.Base64, ZipType zipBefore = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            // construct symmetric cipher pipeline with cryptKey and pass pipeString as out param            
            SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
            pipeString = symmPipe.PipeString;

            // perform multi crypt pipe stages
            byte[] encryptedBytes = symmPipe.EncrpytGoRounds(plainBytes, cryptKey, zipBefore, keyHash);
            // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
            string encrypted = encoding.GetEnCoder().Encode(encryptedBytes);

            return encrypted;
        }


        /// <summary>
        /// DecrpytToString
        /// </summary>
        /// <param name="cryptedEncodedMsg">encrypted message</param>
        /// <param name="cryptKey">Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline 
        /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe</param>
        /// <param name="pipeStrig">out parameter for setting hash to compare entities encryption</param>
        /// <param name="decoding"><see cref="EncodingType"/> type for encoding encrypted bytes back in plain text></param>
        /// <param name="unzipAfter"><see cref="ZipType"/> and <see cref="ZipTypeExtensions.Unzip(ZipType, byte[])"/></param>
        /// <param name="keyHash"><see cref="KeyHash"/> hashing key algorithm</param>
        /// <returns>Decrypted stirng</returns>
        public static string DecrpytToString(string cryptedEncodedMsg, string cryptKey, out string pipeString,
            EncodingType decoding = EncodingType.Base64, ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            // create symmetric cipher pipe for decryption with crypt key and pass pipeString as out param
            SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
            pipeString = symmPipe.PipeString;

            // get bytes from encrypted encoded string dependent on the encoding type (uu, base64, base32,..)
            byte[] cipherBytes = decoding.GetEnCoder().Decode(cryptedEncodedMsg);
            // staged decryption of bytes
            byte[] unroundedMerryBytes = symmPipe.DecrpytRoundsGo(cipherBytes, cryptKey, unzipAfter, keyHash);

            // Get string from decrypted bytes
            string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes); 
            // find first \0 = NULL char in string and truncate all after first \0 apperance in string
            while (decrypted[decrypted.Length - 1] == '\0')
                decrypted = decrypted.Substring(0, decrypted.Length - 1);

            return decrypted;
        }

        public static byte[] DecrpytStringToBytes(string cryptedEncodedMsg, string cryptKey, out string pipeString,
            EncodingType decoding = EncodingType.Base64, ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
            // create symmetric cipher pipe for decryption with crypt key and pass pipeString as out param
            SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
            pipeString = symmPipe.PipeString;

            // get bytes from encrypted encoded string dependent on the encoding type (uu, base64, base32,..)
            byte[] cipherBytes = decoding.GetEnCoder().Decode(cryptedEncodedMsg);
            // staged decryption of bytes
            byte[] unroundedMerryBytes = symmPipe.DecrpytRoundsGo(cipherBytes, cryptKey, unzipAfter, keyHash);

            return unroundedMerryBytes;
        }

		public static string DecrpytBytesToString(byte[] cipherBytes, string cryptKey, out string pipeString,
			EncodingType decoding = EncodingType.Base64, ZipType unzipAfter = ZipType.None, KeyHash keyHash = KeyHash.Hex)
        {
			// create symmetric cipher pipe for decryption with crypt key and pass pipeString as out param
			SymmCipherPipe symmPipe = new SymmCipherPipe(cryptKey);
			pipeString = symmPipe.PipeString;

			// staged decryption of bytes
			byte[] unroundedMerryBytes = symmPipe.DecrpytRoundsGo(cipherBytes, cryptKey, unzipAfter, keyHash);

            // Get string from decrypted bytes
            string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes);
			// find first \0 = NULL char in string and truncate all after first \0 apperance in string
			while (decrypted[decrypted.Length - 1] == '\0')
				decrypted = decrypted.Substring(0, decrypted.Length - 1);
            
            return decrypted;
		}

		#endregion static en-de-crypt members

		#endregion multiple rounds en-de-cryption

	}

}
