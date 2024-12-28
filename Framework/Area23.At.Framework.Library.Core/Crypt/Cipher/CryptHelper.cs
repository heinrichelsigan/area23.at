using Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Core.Util;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Core.Crypt.Cipher
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
        /// Gets a prefered <see cref="IBlockCipher"/> 
        /// for a symmetric only engine from <see cref="Org.BouncyCastle.Crypto.Engines"/> 
        /// </summary>
        /// <param name="symmCipherAlgo">symmetric prefered alogorithm to chipher</param>
        /// <returns><see cref="IBlockCipher"/></returns>
        public static IBlockCipher GetPreferedBlockCipher(SymmCipherEnum symmCipherAlgo)
        {
            CryptParamsPrefered cryptParamsPrefered = new CryptParamsPrefered(symmCipherAlgo);
            return cryptParamsPrefered.BlockCipher;
        }

        /// <summary>
        /// Gets <see cref="CryptParamsPrefered"/> with 
        /// a symmetric only engine from <see cref="Org.BouncyCastle.Crypto.Engines"/> 
        /// in <see cref="Org.BouncyCastle.Crypto.IBlockCipher"/> 
        /// </summary>
        /// <param name="symmCipherAlgo">alogorithm to chipher</param>
        /// <returns>CryptParamsPrefered</returns>
        public static CryptParamsPrefered GetPreferedCryptParams(SymmCipherEnum symmCipherAlgo)
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
            return string.IsNullOrEmpty(secretKey) ? Constants.AUTHOR_EMAIL : secretKey;
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
            string usrHash = string.IsNullOrEmpty(userHash) ? Constants.AUTHOR_IV : userHash;

            return string.Concat(secKey, usrHash);
        }

        /// PrivateKeyWithUserHash, helper to hash merge private user key with hash
        /// </summary>
        /// <param name="key">users private key</param>
        /// <param name="hash">users private hash</param>
        /// <returns>doubled concatendated string of (secretKey + hash)</returns>
        internal static byte[] KeyUserHashBytes(string key, string hash, bool merge = true)
        {
            // TODO: throw Exception, when secret key is null or empty,
            // instead of using Constants.AUTHOR_EMAIL & Constants.AUTHOR_IV
            key = (string.IsNullOrEmpty(key)) ? Constants.AUTHOR_EMAIL : key;
            hash = (string.IsNullOrEmpty(hash)) ? Constants.AUTHOR_IV : hash;

            byte[] keyBytes = EnDeCoder.GetBytes(key);
            byte[] hashBytes = EnDeCoder.GetBytes(hash);

            List<Byte> outBytes = new List<byte>();
            if (!merge)
                outBytes.AddRange(keyBytes.TarBytes(hashBytes));
            else
            {
                int kb = 0, hb = 0;
                for (int ob = 0; (ob < (keyBytes.Length + hashBytes.Length)); ob++)
                {
                    if (kb < keyBytes.Length)
                        outBytes.Add(keyBytes[kb++]);
                    if (hb < hashBytes.Length)
                        outBytes.Add(hashBytes[hb++]);
                    if (hb < hashBytes.Length)
                        outBytes.Add(hashBytes[hashBytes.Length - hb]);
                    hb++;
                    if (kb < keyBytes.Length)
                        outBytes.Add(keyBytes[keyBytes.Length - kb]);
                    kb++;

                    ob = outBytes.Count;
                }
            }

            return outBytes.ToArray();
        }

        /// <summary>
        /// GetUserKeyBytes gets symetric chiffer private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="key">user key, default email address</param>
        /// <param name="hash">user hash</param>        
        /// <param name="keyLen">length of user key bytes, maximum length <see cref="Constants.MAX_KEY_LEN"/></param>
        /// <returns>Array of byte with length KeyLen</returns>
        public static byte[] GetUserKeyBytes(string key, string hash, int keyLen = 32)
        {
            // TODO: throw Exception, when secret key is null or empty,
            // instead of using Constants.AUTHOR_EMAIL & Constants.AUTHOR_IV
            key = (string.IsNullOrEmpty(key)) ? Constants.AUTHOR_EMAIL : key;
            hash = (string.IsNullOrEmpty(hash)) ? Constants.AUTHOR_IV : hash;

            int keyByteCnt = -1;
            keyLen = (keyLen > Constants.MAX_KEY_LEN) ? Constants.MAX_KEY_LEN : keyLen;
            string keyByteHashString = key;
            byte[] tmpKey = new byte[keyLen];

            byte[] keyHashBytes = KeyUserHashBytes(key, hash);
            keyByteCnt = keyHashBytes.Length;
            byte[] keyHashTarBytes = new byte[keyByteCnt * 2 + 1];

            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(KeyUserHashBytes(hash, key));
                keyByteCnt = keyHashTarBytes.Length;

                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(
                    KeyUserHashBytes(hash, key),
                    KeyUserHashBytes(key, hash)
                );
                keyByteCnt = keyHashTarBytes.Length;

                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(
                    KeyUserHashBytes(hash + hash, key + key, false),
                    KeyUserHashBytes(hash + key + hash, key + hash + key, false),
                    KeyUserHashBytes(hash + key + hash, key + hash + key, true),
                    KeyUserHashBytes(hash + key + key + hash, key + hash + hash + key, false),
                    KeyUserHashBytes(hash + key + key + hash, key + hash + hash + key, true)
                );

                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            while (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(keyHashBytes);
                keyByteCnt = keyHashTarBytes.Length;
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);

                //RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
                //randomNumGen.GetBytes(tmpKey, 0, keyLen);

                //int tinyLength = keyHashBytes.Length;

                //for (int bytCnt = 0; bytCnt < keyLen; bytCnt++)
                //{
                //    tmpKey[bytCnt] = keyHashBytes[bytCnt % tinyLength];
                //}
            }

            if (keyLen <= keyByteCnt)
            {
                // Array.Copy(keyHashBytes, 0, tmpKey, 0, keyLen);
                for (int bytIdx = 0; bytIdx < keyLen; bytIdx++)
                    tmpKey[bytIdx] = keyHashBytes[bytIdx];
            }

            return tmpKey;

        }

        #endregion GetUserKeyBytes

    }

}
