using Area23.At.Framework.Library.Core.EnDeCoding;
using Area23.At.Framework.Library.Core.Util;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Core.Cipher.Symm
{

    /// <summary>
    /// static class CryptHelper provides static helper methods for encryption / decryption
    /// </summary>
    public static class CryptHelper
    {


        public static IBlockCipher GetBlockCipher(SymmCipherEnum cipherAlgo, ref string mode, ref int blockSize, ref int keyLen)
        {
            IBlockCipher blockCipher = null;
            if (string.IsNullOrEmpty(mode))
                mode = "ECB";
            if (blockSize < 64)
                blockSize = 256;
            if (keyLen < 8)
                keyLen = 32;

            string requestedAlgorithm = cipherAlgo.ToString();
            switch (cipherAlgo)
            {
                case SymmCipherEnum.BlowFish:
                    blockSize = 64;
                    keyLen = 8;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
                    break;
                case SymmCipherEnum.Fish2:
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.TwofishEngine();
                    break;
                case SymmCipherEnum.Fish3:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.ThreefishEngine(blockSize);
                    break;
                case SymmCipherEnum.Camellia:
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaLightEngine();
                    break;
                case SymmCipherEnum.RC532:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                    break;
                case SymmCipherEnum.Cast6:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case SymmCipherEnum.Gost28147:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case SymmCipherEnum.Idea:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                //case "RC564":
                //    blockSize = 256;
                //    keyLen = 32;
                //    mode = "ECB";
                //    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                //    break;
                case SymmCipherEnum.Seed:
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    break;
                case SymmCipherEnum.Serpent:
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    break;
                case SymmCipherEnum.SkipJack:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case SymmCipherEnum.Tea:
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case SymmCipherEnum.XTea:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                case SymmCipherEnum.Aes:
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
        /// KeyUserHashBytes, helper to double private secret key with hash
        /// </summary>
        /// <param name="secretKey">users private secret key</param>
        /// <param name="userHash">users private secret key hash</param>
        /// <returns>doubled concatendated string of (secretKey + hash)</returns>
        internal static byte[] KeyUserHashBytes(string secretKey, string userHash, bool merge = false)
        {
            // TODO: throw Exception, when secret key is null or empty instead of using Constants.AUTHOR_EMAIL & Constants.AREA23_EMAIL
            string secKey = string.IsNullOrEmpty(secretKey) ? Constants.AUTHOR_EMAIL : secretKey;
            string usrHash = string.IsNullOrEmpty(userHash) ? Constants.AREA23_EMAIL : userHash;
            byte[] secBytes = EnDeCoder.GetBytes(secKey);
            byte[] hashBytes = EnDeCoder.GetBytes(usrHash);

            List<Byte> outBytes = new List<byte>();
            if (!merge)
                outBytes.AddRange(secBytes.TarBytes(hashBytes));
            else
            {
                int hb = 0;
                for (int sb = 0; (sb < secBytes.Length || hb < hashBytes.Length); sb++)
                {
                    if (sb < secBytes.Length)
                        outBytes.Add(secBytes[sb]);
                    if (hb < hashBytes.Length)
                        outBytes.Add(hashBytes[hb]);
                    hb++;

                }
            }

            return outBytes.ToArray();
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

            byte[] keyHashBytes = KeyUserHashBytes(secretKey, usrHash);
            keyByteCnt = keyHashBytes.Length;
            byte[] keyHashTarBytes = new byte[keyByteCnt * 2 + 1];

            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(KeyUserHashBytes(usrHash, secretKey));
                keyByteCnt = keyHashTarBytes.Length;

                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
                randomNumGen.GetBytes(tmpKey, 0, keyLen);

                int tinyLength = keyHashBytes.Length;

                for (int bytCnt = 0; bytCnt < keyLen; bytCnt++)
                {
                    tmpKey[bytCnt] = keyHashBytes[bytCnt % tinyLength];
                }
            }
            else
            {
                for (int bytIdx = 0; bytIdx < keyLen; bytIdx++)
                {
                    tmpKey[bytIdx] = keyHashBytes[bytIdx];
                }
            }

            return tmpKey;

        }

        #endregion GetUserKeyBytes


    }

}
