using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Core.Cipher.Symm
{
    public class CryptParams
    {
        public string AlgorithmName { get; set; }

        public string Mode { get; set; }

        public int BlockSize { get; set; }

        public int KeyLen { get; set; }

        public IBlockCipher BlockChipher { get; set; }

        public CryptParams()
        {
            AlgorithmName = "Aes";
            BlockSize = 256;
            KeyLen = 32;
            Mode = "ECB";
            BlockChipher = new AesEngine();
        }

        public CryptParams(string requestedAlgorithm)
        {
            var c = RequestAlgorithm(requestedAlgorithm);
            AlgorithmName = c.AlgorithmName;
            Mode = c.Mode;
            KeyLen = c.KeyLen;
            BlockSize = c.BlockSize;
            BlockChipher = c.BlockChipher;
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
