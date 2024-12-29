using Org.BouncyCastle.Crypto;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// CryptParamsPrefered prefered params for symmetric block cipher
    /// </summary>
    public class CryptParamsPrefered : CryptParams
    {        
        public SymmCipherEnum SymmCipher { get; set; }

        /// <summary>
        /// standard ctor with <see cref="SymmCipherEnum.Aes"/> default
        /// </summary>
        public CryptParamsPrefered() : base() 
        {
            SymmCipher = SymmCipherEnum.Aes;
        }

        /// <summary>
        /// constructs a object with correct <see cref="Mode"/>, <see cref="BlockSize"/>, <see cref="KeyLen"/>
        /// for parameter <see cref="Cipher"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/></param>
        public CryptParamsPrefered(SymmCipherEnum cipherAlgo)
        {
            SymmCipher = cipherAlgo;
            switch (cipherAlgo)
            {
                case SymmCipherEnum.BlowFish:
                    BlockSize = 64;
                    KeyLen = 8;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case SymmCipherEnum.Fish2:
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TwofishEngine();
                    break;
                case SymmCipherEnum.Fish3:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.ThreefishEngine(BlockSize);
                    break;
                case SymmCipherEnum.Camellia:
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case SymmCipherEnum.RC532:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                    break;
                case SymmCipherEnum.Cast6:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case SymmCipherEnum.Gost28147:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case SymmCipherEnum.Idea:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                //case "RC564":
                //    BlockSize = 256;
                //    KeyLen = 32;
                //    Mode = "ECB";
                //    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                //    break;
                case SymmCipherEnum.Seed:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    break;
                case SymmCipherEnum.Serpent:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    break;
                case SymmCipherEnum.SkipJack:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case SymmCipherEnum.Tea:
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case SymmCipherEnum.XTea:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                case SymmCipherEnum.Aes:
                default:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
            }


        }

        /// <summary>
        /// constructs a <see cref="CryptParamsPrefered"/> object 
        /// with correct <see cref="Mode"/>, <see cref="BlockSize"/>, <see cref="KeyLen"/>
        /// with additional <see cref="Key"/> and <see cref="Hash"/>
        /// for parameter <see cref="Cipher"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/></param>
        public CryptParamsPrefered(SymmCipherEnum cipherAlgo, string key, string hash) : this(cipherAlgo)
        {
            SymmCipher = cipherAlgo;
            Key = key;
            Hash = hash;            
        }

        /// <summary>
        /// static way to get valid <see cref="CryptParamsPrefered"/> for a requested <see cref="SymmCipherEnum"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/></param>
        /// <returns><see cref="CryptParamsPrefered"/></returns>
        public static CryptParamsPrefered RequestPreferedAlgorithm(SymmCipherEnum cipherAlgo)
        {
            return new CryptParamsPrefered(cipherAlgo);
        }

        public static IBlockCipher GetCryptParams(SymmCipherEnum cipherAlgo)
        {
            return new CryptParamsPrefered(cipherAlgo).BlockCipher;
        }

    }

}
