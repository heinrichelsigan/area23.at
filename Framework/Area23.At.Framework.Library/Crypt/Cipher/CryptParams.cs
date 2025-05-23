﻿using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Crypt.Cipher
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

        public int Size { get; set; }

        public int BlockSize => this.BlockCipher.GetBlockSize();

        public int KeyLen { get; set; }

        public IBlockCipher BlockCipher { get; set; }

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
                case CipherEnum.Rijndael:
                    Size = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
                case CipherEnum.AesLight:
                    Size = 128;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AesLightEngine();
                    break;
                case CipherEnum.Aria:
                    Size = 128;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.AriaEngine();
                    break;
                case CipherEnum.BlowFish:
                    Size = 64;
                    KeyLen = 8;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case CipherEnum.Fish2:
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TwofishEngine();
                    break;
                case CipherEnum.Fish3:
                    Size = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.ThreefishEngine(Size);
                    break;
                case CipherEnum.ThreeFish256:
                    Size = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.ThreefishEngine(Size);
                    break;
                case CipherEnum.Camellia:
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case CipherEnum.CamelliaLight:
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case CipherEnum.Cast5:
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Cast5Engine();
                    break;
                case CipherEnum.Cast6:
                    Size = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case CipherEnum.Des:
                    Size = 64;
                    KeyLen = 8;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEngine();
                    break;
                case CipherEnum.Des3:
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.DesEdeEngine();
                    break;
                case CipherEnum.Dstu7624:
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Dstu7624Engine(Size);
                    break;
                case CipherEnum.Gost28147:
                    Size = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case CipherEnum.Idea:
                    Size = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                case CipherEnum.Noekeon:
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.NoekeonEngine();
                    break;
                case CipherEnum.RC2:
                    Size = 128;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                    break;
                case CipherEnum.RC532:
                    Size = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                    break;
                case CipherEnum.RC564:
                    Size = 64;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                    break;
                case CipherEnum.RC6:
                    Size = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                    break;
                case CipherEnum.Seed:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    break;
                case CipherEnum.Serpent:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    break;
                case CipherEnum.SM4:
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SM4Engine();
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    break;
                case CipherEnum.SkipJack:
                    Size = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case CipherEnum.Tea:
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case CipherEnum.Tnepres:
                    Size = 128;
                    KeyLen = 16;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.TnepresEngine();
                    break;
                case CipherEnum.XTea:
                    Size = 256;
                    KeyLen = 32;
                    Mode = "ECB";
                    BlockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                default:
                    Size = 256;
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
            Hash = (string.IsNullOrEmpty(hash)) ? EnDeCodeHelper.KeyToHex(key) : hash;
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
            return (new CryptParams(cipherAlgo)).BlockCipher;
        }

    }

}
