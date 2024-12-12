using Area23.At.Framework.Library.Core.EnDeCoding;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Core.SymCipher
{

    /// <summary>
    /// static class CryptHelper provides static helper methods for encryption / decryption
    /// </summary>
    public static class CryptHelper
    {

    
        public static IBlockCipher GetBlockCipher(string requestedAlgorithm, ref string mode, ref int blockSize, ref int keyLen)
        {
            IBlockCipher blockCipher = null;
            if (string.IsNullOrEmpty(mode))
                mode = "ECB";
            if (blockSize < 64)
                blockSize = 256;
            if (keyLen < 8) 
                keyLen = 32;

            switch (requestedAlgorithm)
            {
                case "Camellia":
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case "Cast5":
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Cast5Engine();
                    break;
                case "Cast6":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case "Gost28147":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case "Idea":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                case "Noekeon":
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.NoekeonEngine();
                    break;
                case "RC2":
                    blockSize = 128;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                    break;
                case "RC532":
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
                case "RC6":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                    break;
                case "Seed":
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    break;
                //case "Serpent":
                //    blockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                //    blockSize = 256;
                //    keyLen = 16;
                //    mode = "ECB";
                //    break;
                case "Skipjack":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case "Tea":
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case "Tnepres":
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.TnepresEngine();
                    break;
                case "XTea":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                case "Rijndael":
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
            CryptParams cryptParams = new CryptParams(requestAlgorithm);
            return cryptParams;
        }


        public static IBlockCipher GetBlockCipher(CryptParams cryptParams)
        {
            CryptParams cParams = new CryptParams(cryptParams.AlgorithmName);
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
            string secKey = string.IsNullOrEmpty(secretKey) ? Constants.AUTHOR_EMAIL : secretKey;
            return string.Concat(secKey);
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

            return string.Concat(secKey, usrHash);
        }


        /// <summary>
        /// GetUserKeyBytes gets symetric chiffer private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="secretKey">user secret key, default email address</param>
        /// <param name="usrHash">user hash</param>
        /// <returns>Array of byte with length KeyLen</returns>
        public static byte[] GetUserKeyBytes(string secretKey = "postmaster@kernel.org", string usrHash = "kernel.org", int keyLen = 32)
        {
            
            int keyByteCnt = -1;
            string keyByteHashString = secretKey;
            byte[] tmpKey = new byte[keyLen];

            if ((keyByteCnt = EnDeCoder.GetByteCount(keyByteHashString)) < keyLen)
            {
                keyByteHashString = PrivateUserKey(secretKey);
                keyByteCnt = EnDeCoder.GetByteCount(keyByteHashString);
            }
            if (keyByteCnt < keyLen)
            {
                keyByteHashString = PrivateKeyWithUserHash(secretKey, usrHash);
                keyByteCnt = EnDeCoder.GetByteCount(keyByteHashString);
            }
            if (keyByteCnt < keyLen)
            {
                RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
                randomNumGen.GetBytes(tmpKey, 0, keyLen);

                byte[] tinyKeyBytes = new byte[keyByteCnt];
                tinyKeyBytes = EnDeCoder.GetBytes(keyByteHashString);
                int tinyLength = tinyKeyBytes.Length;

                for (int bytCnt = 0; bytCnt < keyLen; bytCnt++)
                {
                    tmpKey[bytCnt] = tinyKeyBytes[bytCnt % tinyLength];
                }
            }
            else
            {
                byte[] ssSmallNotTinyKeyBytes = new byte[keyByteCnt];
                ssSmallNotTinyKeyBytes = EnDeCoder.GetBytes(keyByteHashString);
                int ssSmallByteCnt = ssSmallNotTinyKeyBytes.Length;

                for (int bytIdx = 0; bytIdx < keyLen; bytIdx++)
                {
                    tmpKey[bytIdx] = ssSmallNotTinyKeyBytes[bytIdx];
                }
            }

            return tmpKey;

        }

        #endregion GetUserKeyBytes


    }

}
