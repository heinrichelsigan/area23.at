using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Cipher
{

    public class CryptParams
    {
        public CipherEnum Cipher { get; set; }

        public string AlgorithmName { get; set; }

        public string Mode { get; set; }

        public int BlockSize { get; set; }

        public int KeyLen { get; set; }

        public IBlockCipher BlockChipher { get; set; }

        public CryptParams()
        {
            Cipher = CipherEnum.Aes;
            AlgorithmName = "Aes";
            BlockSize = 256;
            KeyLen = 32;
            Mode = "ECB";
            BlockChipher = new AesEngine();
        }

        public CryptParams(CipherEnum cipherAlgo)
        {
            var c = RequestAlgorithm(cipherAlgo);
            AlgorithmName = c.AlgorithmName;
            Mode = c.Mode;
            KeyLen = c.KeyLen;
            BlockSize = c.BlockSize;
            BlockChipher = c.BlockChipher;
        }

        public static CryptParams RequestAlgorithm(CipherEnum cipherAlgo)
        {
            CryptParams cParams = new CryptParams();
            cParams.Cipher = cipherAlgo;
            cParams.AlgorithmName = cipherAlgo.ToString();

            switch (cipherAlgo)
            {
                case CipherEnum.BlowFish:
                    cParams.BlockSize = 64;
                    cParams.KeyLen = 8;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case CipherEnum.Fish2:
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case CipherEnum.Fish3:
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case CipherEnum.Camellia:
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case CipherEnum.Cast5:
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.Cast5Engine();
                    break;
                case CipherEnum.Cast6:
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case CipherEnum.Gost28147:
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case CipherEnum.Idea:
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                case CipherEnum.Noekeon:
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.NoekeonEngine();
                    break;
                case CipherEnum.RC2:
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                    break;
                case CipherEnum.RC532:
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                    break;
                //case "RC564":
                //    blockSize = 256;
                //    keyLen = 32;
                //    mode = "ECB";
                //    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                //    break;
                case CipherEnum.RC6:
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                    break;
                case CipherEnum.Seed:
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    break;
                case CipherEnum.Serpent:
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    break;
                case CipherEnum.SkipJack:
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case CipherEnum.Tea:
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case CipherEnum.Tnepres:
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.TnepresEngine();
                    break;
                case CipherEnum.XTea:
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                case CipherEnum.Rijndael:
                default:
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
            }

            return cParams;
        }
        public static CryptParams RequestAlgorithm(string requestedAlgorithm)
        {
            CryptParams cParams = new CryptParams();
            cParams.AlgorithmName = requestedAlgorithm;

            switch (requestedAlgorithm)
            {
                case "Camellia":
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new CamelliaEngine();
                    break;
                case "Cast5":
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Cast5Engine();
                    break;
                case "Cast6":
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Cast6Engine();
                    break;
                case "Gost28147":
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new Gost28147Engine();
                    break;
                case "Idea":
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new IdeaEngine();
                    break;
                case "Noekeon":
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new NoekeonEngine();
                    break;
                case "RC2":
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new RC2Engine();
                    break;
                case "RC532":
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new RC532Engine();
                    break;
                //case "RC564":
                //    cParams.BlockSize = 256;
                //    cParams.KeyLen = 32;
                //    cParams.Mode = "ECB";
                //    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                //    break;
                case "RC6":
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new RC6Engine();
                    break;
                case "Seed":
                    cParams.BlockChipher = new SeedEngine();
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    break;
                //case "Serpent":
                //    cParams.BlockChipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                //    cParams.BlockSize = 256;
                //    cParams.KeyLen = 16;
                //    cParams.Mode = "ECB";
                //    break;
                case "Skipjack":
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new SkipjackEngine();
                    break;
                case "Tea":
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new TeaEngine();
                    break;
                case "Tnepres":
                    cParams.BlockSize = 128;
                    cParams.KeyLen = 16;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new TnepresEngine();
                    break;
                case "XTea":
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.BlockChipher = new XteaEngine();
                    break;
                case "Rijndael":
                default:
                    cParams.BlockSize = 256;
                    cParams.KeyLen = 32;
                    cParams.Mode = "ECB";
                    cParams.AlgorithmName = "Aes";
                    cParams.BlockChipher = new AesEngine();
                    break;
            }

            return cParams;
        }


        public static IBlockCipher GetCryptParams(ref CryptParams cParams)
        {
            if (cParams == null)
                cParams = new CryptParams();
            else
                cParams = RequestAlgorithm(cParams.AlgorithmName);

            return cParams.BlockChipher;
        }

    }

}
