using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Text;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    /// <summary>
    /// CryptParams parameters for encryption algorithm engine
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

        public byte[] keyed = new byte[32];
        public byte[] hashed = new byte[32];


        public string Mode { get => CMode2.ToString(); set => CMode2 = CipherModeExtensions.ParseText(value); }

        public System.Security.Cryptography.CipherMode CMode
        {
            get => CMode2.ToCipherMode();
            set => CMode2 = value.FromCipherMode();
        }

        public CipherMode2 CMode2 { get; set; }

        public int Size { get; set; }

        public int BlockSize => this.BlockCipher.GetBlockSize();

        public int KeyLen { get; set; }

        public int IvLen { get; set; }

        public IBlockCipher BlockCipher { get; set; }

        public IStreamCipher StreamCipher { get; set; }

        public ICipherParameters CipherParameters { get; set; }

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
            IvLen = 32;
            Mode = "ECB";
            CMode2 = CipherMode2.ECB;
            BlockCipher = new AesEngine();
            KeyHashing = KeyHash.Hex;
        }

        /// <summary>
        /// constructs a object with correct <see cref="Mode"/>, <see cref="BlockSize"/>, <see cref="KeyLen"/>
        /// for parameter <see cref="Cipher"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/></param>
        public CryptParams(CipherEnum cipherAlgo) :
            this()
        {
            Cipher = cipherAlgo;
            SetCioherEngine(cipherAlgo);
            // if (IvLen < 1) IvLen = KeyLen;
        }


        public CryptParams(CipherEnum cipherAlgo, string key, string hash, KeyHash keyHash, CipherMode2 cmode = CipherMode2.ECB) :
            this()
        {
            Cipher = cipherAlgo;
            Key = key;
            KeyHashing = keyHash;
            Hash = (string.IsNullOrEmpty(hash)) ? keyHash.Hash(key) : hash;
            keyed = Encoding.UTF8.GetBytes(Key);
            hashed = Encoding.UTF8.GetBytes(Hash);
            CMode2 = cmode;
            Mode = cmode.ToString();

            SetCioherEngine(cipherAlgo);
            SetCipherParameters(keyed, hashed);
        }


        public CryptParams(CipherEnum cipherAlgo, string key, KeyHash keyHash, CipherMode2 cmode = CipherMode2.ECB) :
            this(cipherAlgo, key, keyHash.Hash(key), keyHash, cmode)
        { }

        /// <summary>
        /// constructs a <see cref="CryptParams"/> object by <see cref="CipherEnum"/>
        /// with additional <see cref="Key"/> and <see cref="Hash"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/></param>
        /// <param name="key">secret key</param>
        /// <param name="hash">corresponding key hash</param>
        public CryptParams(CipherEnum cipherAlgo, string key, string hash) :
            this(cipherAlgo, key, hash, KeyHash.Hex)
        { }


        /// <summary>
        /// Constructs instance via another object instance
        /// </summary>
        /// <param name="cryptParams">another instance</param>
        public CryptParams(CryptParams cryptParams) :
            this(cryptParams.Cipher, cryptParams.Key, cryptParams.Hash, cryptParams.KeyHashing)
        { }

        #endregion Constructors

        #region helper members

        /// <summary>
        /// SetCioherEngine sets bouncycastle SteeamCipher or BlockCipher-Engne
        /// </summary>
        /// <param name="cipher"><see cref="CipherEnum"/></param>
        public void SetCioherEngine(CipherEnum cipher)
        {

            SetBlockSizeKeyIvLen(256, 32, 32);

            switch (cipher)
            {
                case CipherEnum.Aes:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
                case CipherEnum.AesLight:
                    SetBlockSizeKeyIvLen(128, 32, 32);
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesLightEngine();
                    break;
                case CipherEnum.AesNet: // TODO: Implement interface IBlockCipher in AesNet                    
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
                case CipherEnum.Aria:
                    SetBlockSizeKeyIvLen(128, 32, 32);
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AriaEngine();
                    break;
                case CipherEnum.BlowFish:
                    SetBlockSizeKeyIvLen(64, 8, 8);
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case CipherEnum.Fish2:
                    SetBlockSizeKeyIvLen();
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TwofishEngine();
                    break;
                case CipherEnum.Fish3:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.ThreefishEngine(Size);
                    break;
                case CipherEnum.Camellia:
                    SetBlockSizeKeyIvLen();
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaEngine();
                    break;
                case CipherEnum.CamelliaLight:
                    SetBlockSizeKeyIvLen();
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case CipherEnum.Cast5:
                    SetBlockSizeKeyIvLen();
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Cast5Engine();
                    break;
                case CipherEnum.Cast6:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case CipherEnum.Des:
                    SetBlockSizeKeyIvLen(64, 8, 8);
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEngine();
                    break;
                case CipherEnum.Des3:
                    SetBlockSizeKeyIvLen(128, 16, 8);
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEdeEngine();
                    break;
                case CipherEnum.Des3Net: // TODO: implement IBlockCipher in Des3Net
                    SetBlockSizeKeyIvLen(128, 16, 8);
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEdeEngine();
                    break;
                case CipherEnum.Dstu7624:
                    SetBlockSizeKeyIvLen();
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Dstu7624Engine(Size);
                    break;
                case CipherEnum.Gost28147:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case CipherEnum.Idea:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                case CipherEnum.Noekeon:
                    SetBlockSizeKeyIvLen();
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.NoekeonEngine();
                    break;
                case CipherEnum.RC2:
                    SetBlockSizeKeyIvLen(128, 32, 32);
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                    break;
                case CipherEnum.RC532:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                    break;
                case CipherEnum.RC564:
                    SetBlockSizeKeyIvLen(54, 32, 32);
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                    break;
                case CipherEnum.RC6:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                    break;
                case CipherEnum.Rijndael:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RijndaelEngine();
                    break;
                case CipherEnum.Seed:
                    SetBlockSizeKeyIvLen();
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    break;
                case CipherEnum.Serpent:
                    SetBlockSizeKeyIvLen();
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                    break;
                case CipherEnum.SM4:
                    SetBlockSizeKeyIvLen();
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SM4Engine();
                    break;
                case CipherEnum.SkipJack:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case CipherEnum.Tea:
                    SetBlockSizeKeyIvLen();
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case CipherEnum.Tnepres:
                    SetBlockSizeKeyIvLen(128, 32, 32);
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TnepresEngine();
                    break;
                case CipherEnum.XTea:
                    SetBlockSizeKeyIvLen(128, 16, 16);
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                case CipherEnum.ZenMatrix:
                    SetBlockSizeKeyIvLen(16, 16, 16);
                    BlockCipher = new ZenMatrix(Size);
                    break;
                case CipherEnum.ZenMatrix2:
                    // throw new NotImplementedException("ZenMatrix2 IBlockCipher interface not implemented");)
                    SetBlockSizeKeyIvLen(256, 256, 256);
                    BlockCipher = new ZenMatrix2();
                    break;

                case CipherEnum.ChaCha:
                    SetKeyIvLen(32, 8);
                    StreamCipher = new ChaChaEngine();
                    break;
                case CipherEnum.ChaCha7539:
                    SetKeyIvLen(32, 12);
                    StreamCipher = new ChaCha7539Engine();
                    break;
                case CipherEnum.XChaCha20:
                    SetKeyIvLen(32, 24);
                    StreamCipher = new XChaCha20Engine();
                    break;
                case CipherEnum.HC128:
                    SetKeyIvLen(16, 16);
                    StreamCipher = new HC128Engine();
                    break;
                case CipherEnum.HC256:
                    SetKeyIvLen(32, 16);
                    StreamCipher = new HC256Engine();
                    break;
                case CipherEnum.RC4:
                    StreamCipher = new RC4Engine();
                    break;
                case CipherEnum.Isaac:
                    StreamCipher = new IsaacEngine();
                    break;
                case CipherEnum.Salsa20:
                    SetKeyIvLen(32, 8);
                    StreamCipher = new Salsa20Engine();
                    break;
                case CipherEnum.XSalsa20:
                    SetKeyIvLen(32, 24);
                    StreamCipher = new XSalsa20Engine();
                    break;
                case CipherEnum.Vmpc: // VmpcK 
                    StreamCipher = new VmpcEngine();
                    break;
                case CipherEnum.VmpcKsa3: // VmpcK 
                    StreamCipher = new VmpcKsa3Engine();
                    break;

                default:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
            }

            //if (StreamCipher == null && Size != BlockSize)
            //    throw new Exception("");
        }

        public void SetCipherParameters(byte[] keyBytes, byte[] hashBytes)
        {
            if (StreamCipher != null)
            {
                if (KeyLen > 0)
                    keyed = keyBytes.StretchCutToLength(KeyLen);
                if (IvLen > 0)
                    hashed = hashBytes.StretchCutToLength(IvLen);
                KeyParameter keyParamStreamC = new KeyParameter(keyed);
                if (Cipher == CipherEnum.Isaac || Cipher == CipherEnum.RC4)
                    CipherParameters = keyParamStreamC;
                else
                    CipherParameters = new ParametersWithIV(keyParamStreamC, hashed);
            }
            else
            {
                if (KeyLen > 0)
                    keyed = keyBytes.StretchCutToLength(KeyLen);
                if (IvLen > 0)
                    hashed = hashBytes.StretchCutToLength(IvLen);
                KeyParameter keyParamStreamC = new KeyParameter(keyed);
                CipherParameters = new ParametersWithIV(keyParamStreamC, hashed);
            }
        }

        void SetBlockSizeKeyIvLen(int blockSize = 128, int keyLen = 16, int ivLen = 16)
        {
            Size = (blockSize < 1) ? 256 : blockSize;
            KeyLen = (keyLen < 1) ? 32 : keyLen;
            IvLen = (ivLen < 0) ? 32 : ivLen;
        }

        void SetKeyIvLen(int keyLen = 32, int ivLen = 32) => SetBlockSizeKeyIvLen(256, keyLen, ivLen);

        #endregion helper members

    }


}
