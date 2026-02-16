using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Zfx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// Provides a simple crypt pipe for <see cref="CipherEnum"/>
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
    public class SecureCipherPipe
    {

        #region fields and properties

        protected internal string cipherKeyHash = "";
        protected internal ZipType zType = ZipType.None;
        protected internal CipherEnum[] inPipe;
        protected internal EncodingType encodeType = EncodingType.Base64;
        // private readonly string pipeString;

        /// <summary>
        /// ZType is current <see cref="ZipType"/>
        /// </summary>
        public ZipType ZType { get => zType; set => zType = value; }

        /// <summary>
        /// Current <see cref="EncodeType"/> 
        /// </summary>
        public EncodingType EncodeType { get => encodeType; set => encodeType = value; }

        /// <summary>
        /// InPipe is current encryption pipe
        /// </summary>
        public CipherEnum[] InPipe { get => inPipe; set => inPipe = value; }

        /// <summary>
        /// OutPipe will always be generated from <see cref="InPipe"/>
        /// </summary>
        public CipherEnum[] OutPipe { get => inPipe.ToList().Reverse<CipherEnum>().ToArray(); }

        public CipherMode2 CMode2 { get; set; } = CipherMode2.CFB;

        public CipherMode CMode { get => CMode2.ToCipherMode(); set => CMode2 = value.FromCipherMode(); }

        /// <summary>
        /// PipeString will always be generated on the fly from <see cref="InPipe"/>
        /// </summary>
        public string PipeString
        {
            get
            {
                string pipeString = "";
                foreach (CipherEnum cipher in inPipe)
                    pipeString += cipher.GetCipherChar();
                return pipeString;
            }
        }

        public string PipeFullExtension
        {
            get
            {
                string miniPipe = (InPipe == null || InPipe.Length == 0) ? "" : "." + PipeString;
                string miniPipeExt = zType.GetZipTypeExtension() + miniPipe + encodeType.GetEnCodingExtension();
                return miniPipeExt;
            }
        }

        #endregion fields and properties

        #region ctor SecureCipherPipe

        /// <summary>
        /// parameterless default constructor for <see cref="SecureCipherPipe"/>
        /// </summary>
        public SecureCipherPipe()
        {
            cipherKeyHash = ""; //
            inPipe = (new List<CipherEnum>()).ToArray();
            encodeType = EncodingType.Base64;
            zType = ZipType.None;
            CMode2 = CipherMode2.CFB;
        }


        /// <summary>
        /// SecureCipherPipe constructor with an array of <see cref="T:CipherEnum[]"/> as inpipe
        /// </summary>
        /// <param name="cipherEnums">array of <see cref="T:CipherEnum[]"/> as inpipe</param>
        /// <param name="maxpipe">size of max. pipe stages, can't be greater than 8</param>
        /// <param name="encType"><see cref="EncodeType"/></param>
        /// <param name="zpType"><see cref="ZipType"/></param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        public SecureCipherPipe(CipherEnum[] cipherEnums, uint maxpipe = 8,
            EncodingType encType = EncodingType.Base64, ZipType zpType = ZipType.None, CipherMode2 cmode2 = CipherMode2.CFB)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = (maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe; // if somebody wants more, he/she/it gets less

            int isize = Math.Min(((int)cipherEnums.Length), ((int)maxpipe));
            inPipe = new CipherEnum[isize];
            Array.Copy(cipherEnums, inPipe, isize);

            encodeType = encType;
            zType = zpType;
            CMode2 = cmode2;
        }

        /// <summary>
        /// SecureCipherPipe constructor with an array of <see cref="T:string[]"/> cipherAlgos as inpipe
        /// </summary>
        /// <param name="cipherAlgos">array of <see cref="T:string[]"/> as inpipe</param>
        /// <param name="maxpipe">maximum lentgh <see cref="Constants.MAX_PIPE_LEN"/></param>
        /// <param name="encType"><see cref="EncodeType"/></param>
        /// <param name="zpType"><see cref="Zip.ZipType"/></param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        public SecureCipherPipe(string[] cipherAlgos, uint maxpipe = 8, EncodingType encType = EncodingType.Base64,
            ZipType zpType = ZipType.None, CipherMode2 cmode2 = CipherMode2.CFB)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = (maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe; // if somebody wants more, he/she/it gets less

            List<CipherEnum> cipherEnums = new List<CipherEnum>();
            int cnt = 0;
            foreach (string algo in cipherAlgos)
            {
                if (!string.IsNullOrEmpty(algo))
                {
                    CipherEnum cipherAlgo = CipherEnum.Aes;
                    if (!Enum.TryParse<CipherEnum>(algo, out cipherAlgo))
                        cipherAlgo = CipherEnum.Aes;

                    cipherEnums.Add(cipherAlgo);

                    if (++cnt > maxpipe)
                        break;
                }
            }

            inPipe = cipherEnums.ToArray();

            encodeType = encType;
            zType = zpType;
            CMode2 = cmode2;
        }

        /// <summary>
        /// SecureCipherPipe ctor with array of user key bytes
        /// </summary>
        /// <param name="keyBytes">user key bytes</param>
        /// <param name="maxpipe">maximum lentgh <see cref="Constants.MAX_PIPE_LEN"/></param>
        /// <param name="encType"><see cref="EncodeType"/></param>
        /// <param name="zpType"><see cref="Zip.ZipType"/></param>
        /// <param name="kh"><see cref="KeyHash"/></param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        /// <param name="verbose"></param>
        /// <exception cref="ArgumentException"></exception>
        public SecureCipherPipe(byte[] keyBytes, uint maxpipe = 8,
            EncodingType encType = EncodingType.Base64, ZipType zpType = ZipType.None,
            CipherMode2 cmode2 = CipherMode2.CFB, bool verbose = false)
        {
            // What ever is entered here as parameter, maxpipe has to be not greater 8, because of no such agency
            maxpipe = (maxpipe > Constants.MAX_PIPE_LEN) ? Constants.MAX_PIPE_LEN : maxpipe; // if somebody wants more, he/she/it gets less

            ushort scnt = 0;
            List<CipherEnum> pipeList = new List<CipherEnum>();

            HashSet<byte> hashBytes = new HashSet<byte>();
            for (int i = 0; i < keyBytes.Length && pipeList.Count < maxpipe; i++)
            {
                byte cb = (byte)((int)((int)keyBytes[i] % 0x1d));
                // TODO: future design
                // if (hashBytes.Contains(cb)) // mit magic add to generate deterministic more on same bytes
                //     cb = (byte)((int)(cb + Math.Pow(2, i) + keyBytes.Length) % 0x1d);                
                if (!hashBytes.Contains(cb))
                {
                    hashBytes.Add(cb);
                    CipherEnum cipherEnm = CipherEnumExtensions.ByteCipherDict[cb];
                    pipeList.Add(cipherEnm);

                    if (verbose)
                        Console.Out.WriteLine("keybyts[" + i + "]=" + keyBytes[i] + " byte cb = " + (int)cb + " CipherEnum: " + cipherEnm);
                }
            }

            inPipe = pipeList.ToArray();

            zType = zpType;
            encodeType = encType;
            CMode2 = cmode2;

        }

        /// <summary>
        /// Constructs a <see cref="SecureCipherPipe"/> from key and hash
        /// by getting <see cref="T:byte[]">byte[] keybytes</see> with <see cref="CryptHelper.GetUserKeyBytes(string, string, int)"/>
        /// </summary>
        /// <param name="keyHash">secret key to generate pipe</param>
        /// <param name="encType"></param>
        /// <param name="zpType"></param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        /// <param name="verbose"></param>
        public SecureCipherPipe(string keyHash, EncodingType encType = EncodingType.Base64,
            ZipType zpType = ZipType.None, CipherMode2 cmode2 = CipherMode2.CFB, bool verbose = false)
            : this(CryptHelper.GetKeyBytesSingle(keyHash, 16), Constants.MAX_PIPE_LEN, encType, zpType, cmode2, verbose)
        {
            cipherKeyHash = keyHash;
        }

        /// <summary>
        /// SecureCipherPipe ctor with only key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="verbose"></param>
        public SecureCipherPipe(string key, bool verbose = false)
            : this(key, EncodingType.Base64, ZipType.None, CipherMode2.CFB, verbose)
        {
            cipherKeyHash = key;
        }

        public SecureCipherPipe(CipherPipe ciphPipe) : this()
        {
            if (ciphPipe != null)
            {
                this.inPipe = ciphPipe.InPipe;
                this.cipherKeyHash = ciphPipe.cipherKey;
                this.CMode = ciphPipe.CMode;
                this.CMode2 = ciphPipe.CMode2;
                this.encodeType = ciphPipe.EncodeType;
                this.zType = ciphPipe.ZType;
            }
        }

        #endregion ctor SecureCipherPipe

        #region json

        /// <summary>
        /// ToJson 
        /// </summary>
        /// <returns>serialized string</returns>
        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented);

        /// <summary>
        /// FromJson
        /// </summary>
        /// <param name="json">serialized json</param>
        /// <returns><see cref="SecureCipherPipe"/></returns>
        public SecureCipherPipe FromJson(string json)
        {
            SecureCipherPipe pipe = JsonConvert.DeserializeObject<SecureCipherPipe>(json);
            if (pipe == null)
            {
                this.inPipe = pipe.InPipe;
                this.encodeType = pipe.EncodeType;
                this.zType = pipe.ZType;
                this.cipherKeyHash = pipe.cipherKeyHash;
                this.CMode2 = pipe.CMode2;
            }
            return pipe;
        }

        #endregion json

        #region static members EncryptBytesFast DecryptBytesFast

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="cmode2"></param>
        /// <returns>encrypted byte Array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] EncryptBytesFast(byte[] inBytes, CipherEnum cipherAlgo,
            string secretKey, CipherMode2 cmode2 = CipherMode2.CFB)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("seretkey");

            CryptParams cpParams = new CryptParams(cipherAlgo, secretKey, secretKey) { CMode2 = cmode2 };
            byte[] encryptBytes = inBytes;

            switch (cipherAlgo)
            {
                case CipherEnum.AesNet:
                    AesNet aesNet = new AesNet(cpParams);
                    encryptBytes = aesNet.Encrypt(inBytes);
                    break;
                case CipherEnum.Des3Net:
                    Des3Net des3 = new Des3Net(cpParams);
                    encryptBytes = des3.Encrypt(inBytes);
                    break;
                case CipherEnum.ZenMatrix:
                    encryptBytes = (new ZenMatrix(secretKey, secretKey, false)).Encrypt(inBytes, true);
                    break;
                case CipherEnum.ZenMatrix2:
                    encryptBytes = (new ZenMatrix2(secretKey, secretKey, false)).Encrypt(inBytes);
                    break;
                default:
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
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytesFast(byte[] cipherBytes, CipherEnum cipherAlgo,
            string secretKey, CipherMode2 cmode2 = CipherMode2.CFB)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("seretkey");

            // bool sameKey = true;
            CryptParams cpParams = new CryptParams(cipherAlgo, secretKey, secretKey) { CMode2 = cmode2 };
            byte[] decryptBytes = cipherBytes;

            switch (cipherAlgo)
            {
                case CipherEnum.AesNet:
                    AesNet aesNet = new AesNet(cpParams);
                    decryptBytes = aesNet.Decrypt(cipherBytes);
                    break;
                case CipherEnum.Des3Net:
                    Des3Net des3 = new Des3Net(cpParams);
                    decryptBytes = des3.Decrypt(cipherBytes);
                    break;
                //case CipherEnum.Rsa:
                //    AsymmetricCipherKeyPair keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                //    decryptBytes = Asymmetric.Rsa.DecryptWithPrivate(cipherBytes, keyPair);
                //    break;
                case CipherEnum.ZenMatrix:
                    decryptBytes = (new ZenMatrix(secretKey, secretKey, false)).Decrypt(cipherBytes);
                    break;
                case CipherEnum.ZenMatrix2:
                    decryptBytes = (new ZenMatrix2(secretKey, secretKey, false)).Decrypt(cipherBytes);
                    break;

                default:
                    Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);
                    decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
                    break;
            }


            return EnDeCodeHelper.GetBytesTrimNulls(decryptBytes);
        }



        /// <summary>
        /// EncrpytT
        /// </summary>
        /// <typeparam name="TRet">
        ///     <see cref="T:string"/>
        ///     <see cref="T:char[]"/>  <see cref="T:IEnumerable{char}"/>
        ///     <see cref="T:bytes[]"/> <see cref="T:IEnumerable{byte}"/>
        /// </typeparam>
        /// <typeparam name="TIn">
        ///     <see cref="T:string"/>
        ///     <see cref="T:char[]"/>  <see cref="T:IEnumerable{char}"/>
        ///     <see cref="T:bytes[]"/> <see cref="T:IEnumerable{byte}"/>
        /// </typeparam>
        /// <param name="tinSource">plain string, char[], byte[], IEnumerable{char}, IEnumerable{bytes}</param>
        /// <param name="cryptKey">Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline 
        /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe</param>
        /// <param name="encoding"><see cref="EncodingType"/> type for encoding encrypted bytes back in plain text></param>
        /// <param name="zipBefore">Zip bytes with <see cref="ZipType"/> before passing them in encrypted stage pipeline. <see cref="ZipTypeExtensions.Zip(ZipType, byte[])"/></param>
        /// <param name="cmode2"></param>
        /// <returns>encrypted generic type</returns>
        /// <exception cref="CException">is thrown on unknown type</exception>
        public static TRet EncrpytT<TRet, TIn>(TIn tinSource, string cryptKey,
            EncodingType encoding = EncodingType.Base64, ZipType zipBefore = ZipType.None,
            CipherMode2 cmode2 = CipherMode2.CFB)
        {
            byte[] stringBytes = new List<byte>().ToArray();
            // construct symmetric cipher pipeline with cryptKey, keyIv, encopding, zipBefore, keyHash and cmode2
            SecureCipherPipe cipherPipe = new SecureCipherPipe(cryptKey, encoding, zipBefore, cmode2, true);

            if (tinSource is string inString)   // Transform string to bytes
                stringBytes = Encoding.UTF8.GetBytes(inString);
            else if (tinSource is char[] chars)
                stringBytes = Encoding.UTF8.GetBytes(new string(chars));
            else if (tinSource is IEnumerable<char> charsIEnumerable)
                stringBytes = Encoding.UTF8.GetBytes(new string(charsIEnumerable.ToArray()));
            else if (tinSource is byte[] inBytes)
                stringBytes = inBytes;
            else if (tinSource is IEnumerable<byte> bytesEnumerable)
                stringBytes = bytesEnumerable.ToArray();
            else throw new CqrException($"Unknown type Exception, type {typeof(TIn)} is not supported.");

            // zip if requested
            byte[] zippedBytes = (zipBefore != ZipType.None) ? zipBefore.Zip(stringBytes) : stringBytes;
            // encrypt in a marry go round way
            byte[] encryptedBytes = cipherPipe.MerryGoRoundEncrpyt(zippedBytes, cryptKey, cmode2);
            // encode after encryption pipe
            String encryptedString = encoding.GetEnCoder().EnCode(encryptedBytes);

            TRet result = default(TRet);
            if (typeof(TRet) == typeof(string))
                result = (TRet)(object)encryptedString;
            else if (typeof(TRet) == typeof(char[]))
                result = (TRet)(object)encryptedString.ToCharArray();
            else if (typeof(TRet) == typeof(IEnumerable<char>))
                result = (TRet)(object)encryptedString.ToCharArray();
            else if (typeof(TRet) == typeof(byte[]))
                result = (TRet)(object)System.Text.Encoding.UTF8.GetBytes(encryptedString);
            else if (typeof(TRet) == typeof(IEnumerable<byte>))
                result = (TRet)(object)System.Text.Encoding.UTF8.GetBytes(encryptedString);
            else throw new CqrException($"Unknown type Exception, type {typeof(TRet)} is not supported.");

            return result;
        }

        /// <summary>
        ///  DecrpytT generic decryption method
        /// </summary>
        /// <typeparam name="TRet">return type 
        ///     <see cref="T:string"/>
        ///     <see cref="T:char[]"/>  <see cref="T:IEnumerable{char}"/>
        ///     <see cref="T:bytes[]"/> <see cref="T:IEnumerable{byte}"/>
        /// </typeparam>
        /// <typeparam name="TIn"></typeparam>
        /// <param name="tinSource">encrypted message</param>
        /// <param name="cryptKey">Unique deterministic key for either generating the mix of symmetric cipher algorithms in the crypt pipeline 
        /// and unique crypt key for each symmetric cipher algorithm in each stage of the pipe</param>
        /// <param name="decoding"><see cref="EncodingType"/> type for encoding encrypted bytes back in plain text></param>
        /// <param name="unzipAfter"><see cref="ZipType"/> and <see cref="ZipTypeExtensions.Unzip(ZipType, byte[])"/></param>
        /// <param name="mode2"></param>
        /// <returns>Decrypted generic TRet</returns>
        /// <exception cref="CqrException">is thrown on unknown type</exception>
        public static TRet DecrpytT<TRet, TIn>(TIn tinSource, string cryptKey,
            EncodingType decoding = EncodingType.Base64, ZipType unzipAfter = ZipType.None,
            CipherMode2 cmode2 = CipherMode2.CFB)
        {
            byte[] stringBytes = new List<byte>().ToArray();
            // create symmetric cipher pipe for decryption with crypt key and pass pipeString as out param
            SecureCipherPipe cPipe = new SecureCipherPipe(cryptKey, decoding, unzipAfter, cmode2, true);
            string pipeString = cPipe.PipeString;
            string incomingEncoded = string.Empty;

            if (tinSource is string inString)
                incomingEncoded = inString;
            else if (tinSource is char[] chars)
                incomingEncoded = chars.ToString();
            else if (tinSource is IEnumerable<char> charsIEnumerable)
                incomingEncoded = new string(charsIEnumerable.ToArray());
            else if (tinSource is byte[] inBytes)
                incomingEncoded = System.Text.Encoding.UTF8.GetString(inBytes);
            else if (tinSource is IEnumerable<byte> bytesEnumerable)
                incomingEncoded = System.Text.Encoding.UTF8.GetString(bytesEnumerable.ToArray());
            else throw new CqrException($"Unknown type Exception, type {typeof(TIn)} is not supported.");

            // get bytes from encrypted encoded string dependent on the encoding type (uu, base64, base32,..)
            byte[] cipherBytes = decoding.GetEnCoder().DeCode(incomingEncoded);
            // staged decryption of bytes
            byte[] intermediatBytes = cPipe.DecrpytRoundGoMerry(cipherBytes, cryptKey, cmode2);
            // Unzip after if necessary
            byte[] decryptedBytes = (unzipAfter != ZipType.None) ? unzipAfter.Unzip(intermediatBytes) : intermediatBytes;

            TRet result = default(TRet);
            if (typeof(TRet) == typeof(string))
                result = (TRet)(object)System.Text.Encoding.UTF8.GetString(decryptedBytes);
            else if (typeof(TRet) == typeof(char[]))
                result = (TRet)(object)System.Text.Encoding.UTF8.GetString(decryptedBytes).ToCharArray();
            else if (result is IEnumerable<char> charsEnumerable)
                result = (TRet)(object)System.Text.Encoding.UTF8.GetString(decryptedBytes).ToCharArray();
            else if (typeof(TRet) == typeof(byte[]))
                result = (TRet)(object)decryptedBytes;
            else if (result is IEnumerable<byte> bytesIEnumerable)
                result = (TRet)(object)decryptedBytes;
            else throw new CqrException($"Unknown type Exception, type {typeof(TRet)} is not supported.");

            return result;
        }


        #endregion static members EncryptBytesFast DecryptBytesFast

        #region multiple rounds en-de-cryption

        /// <summary>
        /// MerryGoRoundEncrpyt starts merry to go arround from left to right in clock hour cycle
        /// </summary>
        /// <param name="inBytes">plain <see cref="T:byte[]"/> to encrypt</param>
        /// <param name="secretKey">user secret key to use for all symmetric cipher algorithms in the pipe</param>
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        /// <returns>encrypted byte[]</returns>
        public virtual byte[] MerryGoRoundEncrpyt(byte[] inBytes, string secretKey, CipherMode2 cmode2)
        {
            if (InPipe == null || inPipe.Length == 0)   // return immideate, when zero round cipher merry go round
                return inBytes;

            cipherKeyHash = string.IsNullOrEmpty(secretKey) ? secretKey : secretKey;
            CMode2 = cmode2;

            int merry = 0;
            KeyHash[] secureHashes = KeyHash_Extensions.GetSecureHashes();
            byte[] encryptedBytes = new byte[inBytes.Length];
            foreach (CipherEnum cipher in InPipe)
            {
                string cipherHashKey = secureHashes[merry % secureHashes.Length].Hash(cipherKeyHash);
                if ((++merry) > (secureHashes.Length - 1)) merry = 0;

                encryptedBytes = EncryptBytesFast(inBytes, cipher, cipherHashKey, CMode2);
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
        /// <param name="cmode2"><see cref="CipherMode2"/></param>
        /// <returns><see cref="T:byte[]"/> plain bytes</returns>
        public virtual byte[] DecrpytRoundGoMerry(byte[] cipherBytes, string secretKey, CipherMode2 cmode2)
        {
            if (OutPipe == null || OutPipe.Length == 0) // when 0 rounds carusell, return immideate inBytes
                return cipherBytes;

            cipherKeyHash = string.IsNullOrEmpty(secretKey) ? cipherKeyHash : secretKey;
            CMode2 = cmode2;

            KeyHash[] secureHashes = KeyHash_Extensions.GetSecureHashes();
            int merry = secureHashes.Length - 1;

            byte[] decryptedBytes = new byte[cipherBytes.Length];
            foreach (CipherEnum cipher in OutPipe)
            {
                string cipherHashKey = secureHashes[merry % secureHashes.Length].Hash(cipherKeyHash);
                if ((--merry) < 0) merry = secureHashes.Length - 1;

                decryptedBytes = DecryptBytesFast(cipherBytes, cipher, cipherHashKey, cmode2);
                cipherBytes = decryptedBytes;
            }

            return decryptedBytes;
        }


        /// <summary>
        /// EncrpytTextGoRounds encrypts text with cipher pipe pipeline
        /// </summary>
        /// <param name="inString">plain text to encrypt</param>
        /// <param name="cryptKey">prviate key for encryption</param>
        /// <param name="encoding"><see cref="EncodingType"/></param>
        /// <param name="zipBefore"><see cref="ZipType"/></param>
        /// <param name="cmode2"></param>
        /// <returns>UTF9 emcoded encrypted string without binary data</returns>
        public virtual string EncrpytTextGoRounds(string inString, string cryptKey,
            EncodingType encoding = EncodingType.Base64, ZipType zipBefore = ZipType.None,
            CipherMode2 cmode2 = CipherMode2.CFB)
        {
            cipherKeyHash = (string.IsNullOrEmpty(cryptKey)) ? cipherKeyHash : cryptKey;

            // Transform string to bytes
            // byte[] inBytes = EnDeCodeHelper.GetBytesFromString(inString);
            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(inString);
            // zip if requested
            byte[] zippedBytes = (zipBefore != ZipType.None) ? zipBefore.Zip(inBytes) : inBytes;

            // now encrypt with pipe
            byte[] encryptedBytes = MerryGoRoundEncrpyt(zippedBytes, cipherKeyHash, CMode2);

            // Encode pipes by encodingType, e.g. base64, uu, hex16, ...
            string encrypted = encoding.GetEnCoder().EnCode(encryptedBytes);

            return encrypted;
        }


        /// <summary>
        /// decrypt encoded encrypted text
        /// </summary>
        /// <param name="cryptedEncodedMsg">encoded encrypted ASCII string</param>
        /// <param name="cryptKey">prviate key for encryption</param>
        /// <param name="decoding"><see cref="EncodingType"/></param>
        /// <param name="unzipAfter"><see cref="ZipType"/></param>
        /// <param name="cmode2"></param>
        /// <returns>decrypted UTF8 string, containing no binary data</returns>
        public virtual string DecryptTextRoundsGo(string cryptedEncodedMsg, string cryptKey,
            EncodingType decoding = EncodingType.Base64, ZipType unzipAfter = ZipType.None,
            CipherMode2 cmode2 = CipherMode2.CFB)
        {

            cipherKeyHash = (string.IsNullOrEmpty(cryptKey)) ? cipherKeyHash : cryptKey;

            // Decoded encoded bytes first, if necessary
            byte[] cipherBytes = (decoding != EncodingType.None) ?
                decoding.GetEnCoder().DeCode(cryptedEncodedMsg) :
                System.Text.Encoding.UTF8.GetBytes(cryptedEncodedMsg);


            // perform multi crypt pipe stages
            byte[] intermediatBytes = DecrpytRoundGoMerry(cipherBytes, cipherKeyHash, CMode2);
            // Unzip after all, if it's necessary
            byte[] decryptedBytes = (unzipAfter != ZipType.None) ? unzipAfter.Unzip(intermediatBytes) : intermediatBytes;

            string decrypted = System.Text.Encoding.UTF8.GetString(decryptedBytes);

            // find first \0 = NULL char in string and truncate all after first \0 apperance in string
            // while (decrypted[decrypted.Length - 1] == '\0')
            //    decrypted = decrypted.Substring(0, decrypted.Length - 1);

            return decrypted;
        }


        public virtual byte[] EncrpytGoRounds(byte[] inBytes, string secretKey,
            ZipType zipBefore = ZipType.None, CipherMode2 cmode2 = CipherMode2.CFB)
        {
            cipherKeyHash = (string.IsNullOrEmpty(secretKey)) ? cipherKeyHash : secretKey;
            ZType = zipBefore;
            CMode2 = cmode2;

            // zip if requested
            byte[] zippedBytes = (zipBefore != ZipType.None) ? zipBefore.Zip(inBytes) : inBytes;
            // encrypt in a marry go round way
            return MerryGoRoundEncrpyt(zippedBytes, cipherKeyHash, cmode2);
        }


        public virtual byte[] DecrpytRoundsGo(byte[] cipherBytes, string secretKey,
            ZipType unzipAfter = ZipType.None, CipherMode2 cmode2 = CipherMode2.CFB)
        {
            cipherKeyHash = (string.IsNullOrEmpty(secretKey)) ? cipherKeyHash : secretKey;
            ZType = unzipAfter;
            CMode2 = cmode2;

            // perform multi crypt pipe stages
            byte[] intermediatBytes = DecrpytRoundGoMerry(cipherBytes, cipherKeyHash, cmode2);
            // Unzip after if necessary
            byte[] decryptedBytes = (unzipAfter != ZipType.None) ? unzipAfter.Unzip(intermediatBytes) : intermediatBytes;

            return decryptedBytes;
        }


        public virtual byte[] EncryptEncodeBytes(byte[] inBytes, string secretKey,
            EncodingType encType = EncodingType.Base64,
            ZipType zipBefore = ZipType.None, CipherMode2 cmode2 = CipherMode2.CFB)
        {
            cipherKeyHash = (string.IsNullOrEmpty(secretKey)) ? cipherKeyHash : secretKey;
            encodeType = encType;
            ZType = zipBefore;
            CMode2 = cmode2;

            // zip if requested
            byte[] zippedBytes = (zipBefore != ZipType.None) ? zipBefore.Zip(inBytes) : inBytes;
            // now encrypt with pipe
            byte[] outBytes = MerryGoRoundEncrpyt(zippedBytes, cipherKeyHash, CMode2);
            // encode after encryption pipe
            if (encType == EncodingType.None)
                return outBytes;

            return System.Text.Encoding.UTF8.GetBytes(encType.GetEnCoder().EnCode(outBytes));
        }

        public virtual byte[] DecodeDecrpytBytes(byte[] encodedBytes, string secretKey,
            EncodingType encType = EncodingType.Base64,
            ZipType unzipAfter = ZipType.None, CipherMode2 cmode2 = CipherMode2.CFB)
        {
            cipherKeyHash = (string.IsNullOrEmpty(secretKey)) ? cipherKeyHash : secretKey;
            encodeType = encType;
            ZType = unzipAfter;
            CMode2 = cmode2;

            // Decoded encoded bytes first, if necessary
            byte[] cipherBytes = (encType != EncodingType.None) ?
                encodeType.GetEnCoder().DeCode(System.Text.Encoding.UTF8.GetString(encodedBytes)) :
                encodedBytes;
            // perform multi crypt pipe stages
            byte[] intermediatBytes = DecrpytRoundGoMerry(cipherBytes, cipherKeyHash, CMode2);
            // Unzip after all, if it's necessary
            byte[] decryptedBytes = (unzipAfter != ZipType.None) ? unzipAfter.Unzip(intermediatBytes) : intermediatBytes;

            return decryptedBytes;
        }



        /// <summary>
        /// Multi functional 
        /// <see cref="EncryptEncodeBytes(byte[], string, EncodingType, ZipType, CipherMode2)"/>
        /// <see cref="DecodeDecrpytBytes(byte[], string, EncodingType, ZipType, CipherMode2)"/>
        /// </summary>
        /// <param name="inBytes">incoming bytes</param>
        /// <param name="secretKey">user private key</param>
        /// <param name="directionDecrypt">true for decryption, false for encryption</param>
        /// <param name="encType">encoding ascii type, e.g. base64, uu, xx</param>
        /// <param name="zip">compression method to zip before or unzip after pipe processed</param>
        /// <param name="cmode2"></param>
        /// <returns>transformed byte array</returns>
        public virtual byte[] CryptCodeBytes(byte[] inBytes, string secretKey,
            bool directionDecrypt = false, EncodingType encType = EncodingType.Base64,
            ZipType zip = ZipType.None, CipherMode2 cmode2 = CipherMode2.CFB)
        {
            return (!directionDecrypt) ?
                EncryptEncodeBytes(inBytes, secretKey, encType, zip, cmode2) :
                DecodeDecrpytBytes(inBytes, secretKey, encType, zip, cmode2);
        }


        #endregion multiple rounds en-de-cryption

        #region graphics bmp creation

        /// <summary>
        /// GenerateEncryptPipeImage - generates image for symmetric cipher encryption pipeline
        /// </summary>
        /// <returns><see cref="Image">the image</see></returns>
        public virtual Image GenerateEncryptPipeImage()
        {
            System.Drawing.Bitmap mergeimg = new Bitmap(Properties.Resource.BlankEncrypt_640x108, new Size(640, 108)), ximage;
            System.Drawing.Bitmap gifStartImage = new Bitmap(Properties.Resource.BlankEncrypt_640x96, new Size(640, 108));
            List<Bitmap> bitmaps = new List<Bitmap>();

            string bmpName = "";
            int w = 64, offset = 0, startset = 0;
            if (this.ZType != ZipType.None)
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
                {
                    w = 60;

                    ximage = new Bitmap(Properties.Resource.block_arrow_right_zip, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(0, 20, w, 64));

                    string drawString = this.ZType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                    SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#df0fef"));
                    float x = offset + 1.0F;
                    float y = 82.5F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                    startset += w;
                }
                gifStartImage = new Bitmap(mergeimg, 640, 108);
                bitmaps.Add(gifStartImage);
            }

            startset = offset;

            for (int i = 0; (i < this.InPipe.Length); i++)
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
                {
                    w = 60;
                    char ch = this.InPipe[i].GetCipherChar();
                    bmpName = $"arrow_right-{i}";
                    if (i < 2)
                        bmpName = (i == 0) ? "arrow_right-c" : "arrow_right-e";

                    object obj = Properties.Resource.ResourceManager.GetObject(bmpName, CultureInfo.CurrentCulture);
                    ximage = new Bitmap(((System.Drawing.Bitmap)(obj)));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));

                    offset += w;
                }
                if (gifStartImage == null)
                    gifStartImage = new Bitmap(mergeimg, 640, 108);
                bitmaps.Add(new Bitmap(mergeimg, 640, 108));
            }


            offset = startset;

            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
            {
                for (int i = 0; (i < this.InPipe.Length); i++)
                {

                    Color color = (i < 5) ? ColorTranslator.FromHtml("#0000ee") : ColorTranslator.FromHtml("#0000dd");
                    string drawString = this.InPipe[i].ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(color);
                    float x = offset + 1.0F;
                    float y = 2F + ((i % 4) * 23.0F);
                    switch (i)
                    {
                        case 1: y = 84F; break;
                        case 2: y = 1F; break;
                        case 3: y = 86F; break;
                        case 4: y = 2F; break;
                        case 5:
                        case 6:
                        case 7:
                            y = 1F + ((i % 4) * 23.0F);
                            drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold); break;
                        default: y = 1F + ((i % 4) * 23.0F); break;
                    }
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }
            }
            bitmaps.Add(new Bitmap(mergeimg, 640, 108));
            gifStartImage = new Bitmap(mergeimg, 640, 108);

            if (this.EncodeType != EncodingType.None)
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resource.encoding_right_end_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));
                    string drawString = this.EncodeType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                    SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#bf0fef"));
                    float x = offset + 1.0F;
                    float y = 4.0F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
                }
                bitmaps.Add(new Bitmap(mergeimg, 640, 108));
                gifStartImage = new Bitmap(mergeimg, 640, 108);
            }

            //TimeSpan ts = new TimeSpan(0, 0, 0, 0, 125);
            //GifEncoder gifAnimEncoder = new GifEncoder(bitmaps.ToArray(), 1, ts);
            //Bitmap animGif = new Bitmap(gifAnimEncoder._memoryStream, false);
            //return animGif;
            // animGif.Save("H:\\tmp\\" + DateTime.Now.ToString("yyyy-MM-DD_hhmmss") + ".gif");
            // gifAnimEncoder.Dispose();
            return gifStartImage;

        }


        /// <summary>
        /// GenerateDecryptPipeImage generates an image for decrypt symmetric cipher pipeline 
        /// </summary>
        /// <returns><see cref="Image">the image</see></returns>
        public virtual Image GenerateDecryptPipeImage()
        {
            System.Drawing.Bitmap mergeimg = new Bitmap(Properties.Resource.BlankDecrypt_640x108, new Size(640, 108)), ximage;
            string bmpName = "";
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
            {
                int w = 64, offset = 0, startset = 0;
                if (this.EncodeType != EncodingType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resource.encoding_right_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));

                    string drawString = this.EncodeType.ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#fa0ade"));
                    float x = offset + 1F;
                    float y = 86F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                    startset += w;
                }

                for (int i = 0; (i < this.OutPipe.Length); i++)
                {
                    w = 60;
                    int r = 7 - i;
                    char ch = this.OutPipe[i].GetCipherChar();
                    bmpName = $"arrow_right-{r}";
                    if (i >= 6)
                        bmpName = (i == 6) ? "arrow_right-e" : "arrow_right-c";

                    object obj = Properties.Resource.ResourceManager.GetObject(bmpName, CultureInfo.CurrentCulture);
                    ximage = new Bitmap(((System.Drawing.Bitmap)(obj)), new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));

                    offset += w;
                }

                offset = startset;
                for (int i = 0; (i < this.OutPipe.Length); i++)
                {
                    w = 60;
                    int r = 7 - i;

                    Color color = (i < 4) ? ColorTranslator.FromHtml("#2200aa") : ColorTranslator.FromHtml("#0000dd");
                    string drawString = this.OutPipe[i].ToString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(color);
                    float x = offset + 2.0F;
                    float y = 1.5F + ((i % 4) * 23.0F);
                    switch (i)
                    {
                        case 5: y = 84F; break;
                        case 6: y = 4F; break;
                        case 7: x = offset; y = 72F; break;
                        default:
                            y = 1.5F + ((i % 4) * 23.0F); break;
                    }
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.NoWrap;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }

                if (this.ZType != ZipType.None)
                {
                    w = 60;
                    ximage = new Bitmap(Properties.Resource.compress_right_end_0, new Size(64, 64));
                    g.DrawImage(ximage, new System.Drawing.Rectangle(offset, 20, w, 64));

                    string drawString = this.ZType.GetUnzipString();
                    Font drawFont = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                    SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#fa0ade"));
                    float x = offset + 2.4F;
                    float y = 3.8F;
                    StringFormat drawFormat = new StringFormat();
                    drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                    g.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);

                    offset += w;
                }

            }

            return mergeimg;
        }

        #endregion graphics bmp creation

    }

}
