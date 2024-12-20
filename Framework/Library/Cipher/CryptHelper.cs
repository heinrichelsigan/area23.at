using Area23.At.Framework.Library.EnDeCoding;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Cipher
{

    /// <summary>
    /// static class CryptHelper provides static helper methods for encryption / decryption
    /// </summary>
    public static class CryptHelper
    {

        public static IBlockCipher GetBlockCipher(CipherEnum cipherAlgo, ref string mode, ref int blockSize, ref int keyLen)
        {
            IBlockCipher blockCipher = null;
            if (string.IsNullOrEmpty(mode))
                mode = "ECB";
            if (blockSize < 64)
                blockSize = 64;
            if (keyLen < 8)
                keyLen = 8;

            string requestedAlgorithm = cipherAlgo.ToString();
            switch (cipherAlgo)
            {
                case CipherEnum.BlowFish:
                    blockSize = 64;
                    keyLen = 8;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case CipherEnum.Fish2:
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.TwofishEngine();
                    break;
                case CipherEnum.Fish3:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.ThreefishEngine(blockSize);
                    break;
                case CipherEnum.Camellia:
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case CipherEnum.Cast5:
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Cast5Engine();
                    break;
                case CipherEnum.Cast6:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case CipherEnum.Gost28147:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case CipherEnum.Idea:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                case CipherEnum.Noekeon:
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.NoekeonEngine();
                    break;
                case CipherEnum.RC2:
                    blockSize = 128;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                    break;
                case CipherEnum.RC532:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                    break;
                //case "RC564":
                //    blockSize = 256;
                //    keyLen = 32;
                //    mode = "ECB";
                //    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                //    break;
                case CipherEnum.RC6:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                    break;
                case CipherEnum.Seed:
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    break;
                case CipherEnum.Serpent:
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    break;
                case CipherEnum.SkipJack:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case CipherEnum.Tea:
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case CipherEnum.Tnepres:
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.TnepresEngine();
                    break;
                case CipherEnum.XTea:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                case CipherEnum.Rijndael:
                default:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
            }


            return blockCipher;
        }


        public static CryptParams GetBlockCipher(string requestAlgorithm)
        {
            CipherEnum cipher = CipherEnum.Aes;
            if (!Enum.TryParse<CipherEnum>(requestAlgorithm, out cipher))
                cipher = CipherEnum.Aes;
            
            CryptParams cryptParams = new CryptParams(cipher);
            return cryptParams;
        }


        public static IBlockCipher GetBlockCipher(CryptParams cryptParams)
        {
            CryptParams cParams = new CryptParams(cryptParams.Cipher);
            return cryptParams.BlockChipher;
        }


        #region GetUserKeyBytes

        /// <summary>
        /// PrivateUserKey, helper to double private secret key to get a longer byte[]
        /// </summary>
        /// <param name="secretKey">users private secret key</param>
        /// <returns>doubled concatendated string of secretKey</returns>
        internal static string PrivateUserKey(string secretKey)
        {
            return Symmetric.CryptHelper.PrivateUserKey(secretKey);
        }

        /// <summary>
        /// PrivateKeyWithUserHash, helper to double private secret key with hash
        /// </summary>
        /// <param name="secretKey">users private secret key</param>
        /// <param name="userHash">users private secret key hash</param>
        /// <returns>doubled concatendated string of (secretKey + hash)</returns>
        internal static string PrivateKeyWithUserHash(string secretKey, string userHash)
        {
            string secKey = string.IsNullOrEmpty(secretKey) ? Constants.AUTHOR_EMAIL : secretKey;
            string usrHash = string.IsNullOrEmpty(userHash) ? Constants.AREA23_EMAIL : userHash;

            return Symmetric.CryptHelper.PrivateKeyWithUserHash(secKey, usrHash);
        }


        /// <summary>
        /// GetUserKeyBytes gets symetric chiffer private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="secretKey">user secret key, default email address</param>
        /// <param name="usrHash">user hash</param>
        /// <returns>Array of byte with length KeyLen</returns>
        public static byte[] GetUserKeyBytes(string secretKey = "postmaster@kernel.org", string usrHash = "kernel.org", int keyLen = 32)
        {
            return Symmetric.CryptHelper.GetUserKeyBytes(secretKey, usrHash, keyLen); 

        }

        #endregion GetUserKeyBytes

    }

}
