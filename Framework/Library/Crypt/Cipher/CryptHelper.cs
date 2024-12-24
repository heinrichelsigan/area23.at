using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Util;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    /// <summary>
    /// static class CryptHelper provides static helper methods for encryption / decryption
    /// </summary>
    public static class CryptHelper
    {

        #region GetBlockCipher

        /// <summary>
        /// Gets <see cref="CryptParams.BlockChipher"/> symmetric or asymmetric engine from <see cref="Org.BouncyCastle.Crypto.Engines"/> 
        /// </summary>
        /// <param name="c"><see cref="CryptParams"/></param>
        /// <returns><see cref="CryptParams"/></returns>
        public static CryptParams GetCryptParams(CryptParams c)
        {
            CryptParams cout = new CryptParams(c);
            return cout;
        }

        /// <summary>
        /// Gets an <see cref="IBlockCipher"/> symmetric or asymmetric engine from <see cref="Org.BouncyCastle.Crypto.Engines"/> 
        /// </summary>
        /// <param name="c">CryptParams</param>
        /// <returns><see cref="IBlockCipher"/></returns>
        public static IBlockCipher GetBlockCipher(CipherEnum cipherAlgo)
        {
            return new CryptParams(cipherAlgo).BlockCipher;
        }

        /// <summary>
        /// Gets an <see cref="IBlockCipher"/> via <see cref="CryptParams.BlockChipher"/> and a 
        /// symmetric or asymmetric engine from <see cref="Org.BouncyCastle.Crypto.Engines"/> 
        /// </summary>
        /// <param name="cipherAlgo">alogorithm to chipher</param>
        /// <returns>CryptParams</returns>
        public static CryptParams GetCryptParams(CipherEnum cipherAlgo)
        {
            return new CryptParams(cipherAlgo);           
        }



        #endregion GetBlockCipher

        #region GetPreferedBlockCipher

        /// <summary>
        /// Gets a prefered <see cref="IBlockCipher"/> symmetric only engine from <see cref="Org.BouncyCastle.Crypto.Engines"/> 
        /// </summary>
        /// <param name="cipherAlgo">alogorithm to chipher</param>
        /// <param name="mode">mode</param>
        /// <param name="blockSize">size of block to en-/decrypt</param>
        /// <param name="keyLen">secret key/iv length</param>
        /// <returns><see cref="IBlockCipher"/></returns>
        public static IBlockCipher GetPreferedBlockCipher(SymmCipherEnum cipherAlgo, ref string mode, ref int blockSize, ref int keyLen)
        {
            IBlockCipher blockCipher = null;
            if (string.IsNullOrEmpty(mode))
                mode = "ECB";
            blockSize = (blockSize < 64) ? 64 : (blockSize > 4096) ? 4096 : blockSize;
            keyLen = (keyLen < 8) ? 8 : (keyLen > 256) ? 256 : keyLen;

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

        /// <summary>
        /// Gets a prefered <see cref="IBlockCipher"/> symmetric only engine from <see cref="Org.BouncyCastle.Crypto.Engines"/> 
        /// </summary>
        /// <param name="symmCipherAlgo">alogorithm to chipher</param>
        /// <returns>CryptParamsPrefered</returns>
        public static CryptParamsPrefered GetPreferedBlockCipher(SymmCipherEnum symmCipherAlgo)
        {
            return new CryptParamsPrefered(symmCipherAlgo);         
        }


        #endregion GetPreferedBlockCipher

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
        /// PrivateKeyWithUserHash, helper to double private secret key with hash
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
                keyHashTarBytes = keyHashBytes.TarBytes(
                    KeyUserHashBytes(usrHash, secretKey)
                );
                keyByteCnt = keyHashTarBytes.Length;

                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(
                    KeyUserHashBytes(usrHash, secretKey, true),
                    KeyUserHashBytes(secretKey, usrHash, true)
                );
                keyByteCnt = keyHashTarBytes.Length;

                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(
                    KeyUserHashBytes(usrHash + usrHash, secretKey + secretKey, false),
                    KeyUserHashBytes(usrHash + secretKey + usrHash, secretKey + usrHash + secretKey, false),
                    KeyUserHashBytes(usrHash + secretKey + usrHash, secretKey + usrHash + secretKey, true),
                    KeyUserHashBytes(usrHash + secretKey + secretKey + usrHash, secretKey + usrHash + usrHash + secretKey, false),
                    KeyUserHashBytes(usrHash + secretKey + secretKey + usrHash, secretKey + usrHash + usrHash + secretKey, true)
                );

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
