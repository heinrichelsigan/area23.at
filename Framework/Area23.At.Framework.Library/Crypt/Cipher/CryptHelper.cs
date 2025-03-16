using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    /// <summary>
    /// static class CryptHelper provides static helper methods for encryption / decryption
    /// </summary>
    public static class CryptHelper
    {

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
        /// <param name="key">users private secret key</param>
        /// <param name="hash">users private secret key hash</param>
        /// <returns>doubled concatendated string of (secretKey + hash)</returns>
        internal static string PrivateKeyWithUserHash(string key, string hash)
        {
            string secKey = string.IsNullOrEmpty(key) ? Constants.AUTHOR_EMAIL : key;
            string usrHash = string.IsNullOrEmpty(hash) ? Constants.AUTHOR_IV : hash;

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

            byte[] keyBytes = EnDeCodeHelper.GetBytes(key);
            byte[] hashBytes = EnDeCodeHelper.GetBytes(hash);

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

            while (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(keyHashBytes);
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
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
