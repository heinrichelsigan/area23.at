using Area23.At.Framework.Library.Crypt.Hash;
using Org.BouncyCastle.Crypto;
using System;
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
        /// constructs a object with correct <see cref="Mode"/>, <see cref="Size"/>, <see cref="KeyLen"/>
        /// for parameter <see cref="Cipher"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/></param>
        public CryptParamsPrefered(SymmCipherEnum cipherAlgo, bool fishOnAesEngine = false) : this()
        {
            SymmCipher = cipherAlgo;
            Size = 256;
            KeyLen = 32;
            Mode = "ECB";

            switch (cipherAlgo)
            {
                case SymmCipherEnum.Aes:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
                case SymmCipherEnum.BlowFish:
                    Size = 64;
                    KeyLen = 8;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case SymmCipherEnum.Fish2:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TwofishEngine();
                    break;
                case SymmCipherEnum.Fish3:
                    // TODO: ugly hack because of 1st version bug
                    if (fishOnAesEngine)
                        BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    else
                        BlockCipher = new Org.BouncyCastle.Crypto.Engines.ThreefishEngine(Size);
                    break;
                case SymmCipherEnum.Camellia:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case SymmCipherEnum.Cast6:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case SymmCipherEnum.Des3:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEdeEngine();
                    break;
                case SymmCipherEnum.Gost28147:                    
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case SymmCipherEnum.Idea:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                case SymmCipherEnum.RC532:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                    break;
                //case "RC564":
                //    Size = 256;
                //    KeyLen = 32;
                //    Mode = "ECB";
                //    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                //    break;
                case SymmCipherEnum.Seed:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    Size = 128;
                    KeyLen = 16;
                    break;
                case SymmCipherEnum.Serpent:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                    Size = 128;
                    KeyLen = 16;
                    break;
                case SymmCipherEnum.SkipJack:                    
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case SymmCipherEnum.Tea:
                    Size = 128;
                    KeyLen = 16;
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case SymmCipherEnum.XTea:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                case SymmCipherEnum.ZenMatrix:
                    Size = 16;
                    KeyLen = 16;
                    BlockCipher = new ZenMatrix();
                    break;
                default:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
            }


        }

        /// <summary>
        /// constructs a <see cref="CryptParamsPrefered"/> object 
        /// with correct <see cref="Mode"/>, <see cref="Size"/>, <see cref="KeyLen"/>
        /// with additional <see cref="Key"/> and <see cref="Hash"/>
        /// for parameter <see cref="Cipher"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/></param>
        public CryptParamsPrefered(SymmCipherEnum cipherAlgo, string key, string hash, bool fishOnAesEngine = false)
            : this(cipherAlgo, fishOnAesEngine)
        {
            SymmCipher = cipherAlgo;
            Key = key;
            Hash = string.IsNullOrEmpty(hash) ? KeyHashing.Hash(key) : hash;
        }

        /// <summary>
        /// static way to get valid <see cref="CryptParamsPrefered"/> for a requested <see cref="SymmCipherEnum"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/></param>
        /// <returns><see cref="CryptParamsPrefered"/></returns>
        [Obsolete("RequestPreferedAlgorithm no mote used", true)]
        public static CryptParamsPrefered RequestPreferedAlgorithm(SymmCipherEnum cipherAlgo, bool fishOnAesEngine = false)
        {
            return new CryptParamsPrefered(cipherAlgo, fishOnAesEngine);
        }

        [Obsolete("RequestPreferedAlgorithm is not used anymore.", true)]
        public static IBlockCipher GetCryptParams(SymmCipherEnum cipherAlgo, bool fishOnAesEngine = false)
        {
            return new CryptParamsPrefered(cipherAlgo, fishOnAesEngine).BlockCipher;
        }

    }


}
