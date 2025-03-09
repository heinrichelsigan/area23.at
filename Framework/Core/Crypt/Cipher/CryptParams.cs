using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Core.Crypt.Cipher
{

    /// <summary>
    /// CryptParams parameters for encryption algorithm engine
    /// </summary>
    public class CryptParams
    {
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

        public int BlockSize { get; set; }

        public int KeyLen { get; set; }

        public IBlockCipher BlockCipher { get; set; }

        /// <summary>
        /// standard ctor with <see cref="CipherEnum.Aes"/> default
        /// </summary>
        public CryptParams()
        {
            Cipher = CipherEnum.Aes;
            BlockSize = 256;
            KeyLen = 32;
            Mode = "ECB";
            BlockCipher = new AesEngine();
        }

        /// <summary>
        /// constructs a object with correct <see cref="Mode"/>, <see cref="BlockSize"/>, <see cref="KeyLen"/>
        /// for parameter <see cref="Cipher"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/></param>
        public CryptParams(CipherEnum cipherAlgo)
        {
            Cipher = cipherAlgo;

            switch (Cipher)
            {
                case CipherEnum.Aes:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
                case CipherEnum.BlowFish:
                    BlockSize = 64;
                    KeyLen = 8;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case CipherEnum.Fish2:
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TwofishEngine();
                    break;
                case CipherEnum.Fish3:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.ThreefishEngine(BlockSize);
                    break;
                case CipherEnum.Camellia:
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case CipherEnum.Cast5:
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Cast5Engine();
                    break;
                case CipherEnum.Cast6:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case CipherEnum.Des:
                    BlockSize = 64;
                    KeyLen = 8;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEngine();
                    break;
                case CipherEnum.Des3:
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEdeEngine();
                    break;
                case CipherEnum.Gost28147:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case CipherEnum.Idea:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                case CipherEnum.Noekeon:
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.NoekeonEngine();
                    break;
                case CipherEnum.RC2:
                    BlockSize = 128;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                    break;
                case CipherEnum.RC532:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                    break;
                case CipherEnum.RC564:
                    BlockSize = 64;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                    break;
                case CipherEnum.RC6:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                    break;
                case CipherEnum.Seed:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    break;
                case CipherEnum.Serpent:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    break;
                case CipherEnum.SkipJack:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case CipherEnum.Tea:
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case CipherEnum.Tnepres:
                    BlockSize = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TnepresEngine();
                    break;
                case CipherEnum.XTea:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                default:
                    BlockSize = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
            }

        }

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
            Hash = hash;
        }

        /// <summary>
        /// Constructs instance via another object instance
        /// </summary>
        /// <param name="cryptParams">another instance</param>
        public CryptParams(CryptParams cryptParams) : this(cryptParams.Cipher, cryptParams.Key, cryptParams.Hash) { }

        /// <summary>
        /// static way to get valid <see cref="CryptParams"/> for a requested <see cref="CipherEnum"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/></param>
        /// <returns><see cref="CryptParams"/></returns>
        public static CryptParams RequestAlgorithm(CipherEnum cipherAlgo)
        {
            return new CryptParams(cipherAlgo);
        }

        public static CryptParams GetCryptParams(CryptParams cParams)
        {
            return new CryptParams(cParams);
        }

        public static IBlockCipher GetBlockCipher(CipherEnum cipherAlgo)
        {
            return CryptHelper.GetCryptParams(cipherAlgo).BlockCipher;
        }

    }

}
