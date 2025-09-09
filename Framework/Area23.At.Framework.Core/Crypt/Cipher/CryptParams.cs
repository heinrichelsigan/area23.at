using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.Hash;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;

namespace Area23.At.Framework.Core.Crypt.Cipher
{

    /// <summary>
    /// CryptParams parameters for encryption algorithm engine
    /// </summary>
    public class CryptParams
    {

        #region Properties

        public CipherEnum Cipher { get; set; }

        public string AlgorithmName
        {
            get => Cipher.ToString();
            // don't use the setter
            private set => Cipher = (CipherEnum)Enum.Parse(typeof(CipherEnum), value);
        }

        public string Key { get; set; }

        public string Hash { get; set; }

        public string Mode { get; set; }

        public int Size { get; set; }

        public int BlockSize => this.BlockCipher.GetBlockSize();

        public int KeyLen { get; set; }

        public IBlockCipher BlockCipher { get; set; }

        public KeyHash KeyHashing { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// standard ctor with <see cref="CipherEnum.Aes"/> default
        /// </summary>
        public CryptParams()
        {
            Cipher = CipherEnum.Aes;
            Size = 256;
            KeyLen = 32;
            Mode = "ECB";
            BlockCipher = new AesEngine();
            KeyHashing = KeyHash.Hex;
        }

        /// <summary>
        /// constructs a object with correct <see cref="Mode"/>, <see cref="BlockSize"/>, <see cref="KeyLen"/>
        /// for parameter <see cref="Cipher"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/></param>
        public CryptParams(CipherEnum cipherAlgo)
        {
            Cipher = cipherAlgo;
            Size = 256;
            KeyLen = 32;
            Mode = "ECB";

            switch (Cipher)
            {
                case CipherEnum.Aes:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
                case CipherEnum.AesLight:
                    Size = 128;
                    KeyLen = 32;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesLightEngine();
                    break;
                case CipherEnum.AesNet: // TODO: Implement interface IBlockCipher in AesNet
                    KeyLen = 32;
                    Size = 256;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
                case CipherEnum.Aria:
                    Size = 128;
                    KeyLen = 32;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AriaEngine();
                    break;
                case CipherEnum.BlowFish:
                    Size = 64;
                    KeyLen = 8;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case CipherEnum.Fish2:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TwofishEngine();
                    break;
                case CipherEnum.Fish3:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.ThreefishEngine(Size);
                    break;
                case CipherEnum.ThreeFish256:
                    ;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.ThreefishEngine(Size);
                    break;
                case CipherEnum.Camellia:
                    Size = 128;
                    KeyLen = 16; ;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case CipherEnum.CamelliaLight:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case CipherEnum.Cast5:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Cast5Engine();
                    break;
                case CipherEnum.Cast6:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case CipherEnum.Des:
                    Size = 64;
                    KeyLen = 8;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEngine();
                    break;
                case CipherEnum.Des3:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEdeEngine();
                    break;
                case CipherEnum.Des3Net: // TODO: implement IBlockCipher in Des3Net
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEdeEngine();
                    break;
                case CipherEnum.Dstu7624:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Dstu7624Engine(Size);
                    break;
                case CipherEnum.Gost28147:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case CipherEnum.Idea:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                case CipherEnum.Noekeon:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.NoekeonEngine();
                    break;
                case CipherEnum.RC2:
                    Size = 128;
                    KeyLen = 32;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                    break;
                case CipherEnum.RC532:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                    break;
                case CipherEnum.RC564:
                    Size = 64;
                    KeyLen = 32;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                    break;
                case CipherEnum.RC6:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                    break;
                case CipherEnum.Rijndael:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RijndaelEngine();
                    break;
                case CipherEnum.Seed:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    Size = 128;
                    KeyLen = 16;
                    break;
                case CipherEnum.Serpent:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                    Size = 128;
                    KeyLen = 16;
                    break;
                case CipherEnum.SM4:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SM4Engine();
                    Size = 128;
                    KeyLen = 16;
                    break;
                case CipherEnum.SkipJack:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case CipherEnum.Tea:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case CipherEnum.Tnepres:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TnepresEngine();
                    break;
                case CipherEnum.XTea:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                case CipherEnum.ZenMatrix:
                    Size = 16;
                    KeyLen = 16;
                    BlockCipher = new ZenMatrix();
                    break;
                case CipherEnum.ZenMatrix2:
                    Size = 32;
                    KeyLen = 32;
                    BlockCipher = new ZenMatrix2();
                    break;
                default:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
            }

        }


        public CryptParams(CipherEnum cipherAlgo, string key, string hash, KeyHash keyHash) : this(cipherAlgo)
        {
            Key = key;
            KeyHashing = keyHash;
            Hash = (string.IsNullOrEmpty(hash)) ? keyHash.Hash(key) : hash;
        }


        public CryptParams(CipherEnum cipherAlgo, string key, KeyHash keyHash) : this(cipherAlgo, key, keyHash.Hash(key), keyHash) { }

        /// <summary>
        /// constructs a <see cref="CryptParams"/> object by <see cref="CipherEnum"/>
        /// with additional <see cref="Key"/> and <see cref="Hash"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/></param>
        /// <param name="key">secret key</param>
        /// <param name="hash">corresponding key hash</param>
        public CryptParams(CipherEnum cipherAlgo, string key, string hash) : this(cipherAlgo)
        {
            Key = key;
            Hash = (string.IsNullOrEmpty(hash)) ? KeyHashing.Hash(key) : hash;
        }

        /// <summary>
        /// Constructs instance via another object instance
        /// </summary>
        /// <param name="cryptParams">another instance</param>
        public CryptParams(CryptParams cryptParams) : this(cryptParams.Cipher, cryptParams.Key, cryptParams.Hash, cryptParams.KeyHashing) { }

        #endregion Constructors

        #region obsolete static members

        /// <summary>
        /// static way to get valid <see cref="CryptParams"/> for a requested <see cref="CipherEnum"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/></param>
        /// <returns><see cref="CryptParams"/></returns>
        [Obsolete("RequestAlgorithm no mote used", true)]
        public static CryptParams RequestAlgorithm(CipherEnum cipherAlgo)
        {
            return new CryptParams(cipherAlgo);
        }

        [Obsolete("GetCryptParams no mote used", true)]
        public static CryptParams GetCryptParams(CryptParams cParams)
        {
            return new CryptParams(cParams);
        }

        [Obsolete("GetBlockCipher no mote used", true)]
        public static IBlockCipher GetBlockCipher(CipherEnum cipherAlgo)
        {
            return (new CryptParams(cipherAlgo)).BlockCipher;
        }

        #endregion obsolete static members

    }

}
