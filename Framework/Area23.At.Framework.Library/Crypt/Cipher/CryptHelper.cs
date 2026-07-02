using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.Text;

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
        [Obsolete("PrivateUserKey(string secretKey) is obsolete.", false)]
        internal static string PrivateUserKey(string secretKey)
        {
            return string.IsNullOrEmpty(secretKey) ? Constants.AUTHOR_EMAIL : secretKey;
        }

        /// <summary>
        /// PrivateKeyWithUserHash, helper to double private secret key with hash
        /// </summary>
        /// <param name="secKey">users private secret key</param>
        /// <param name="hashedKey">users private secret key hash</param>
        /// <returns>doubled concatendated string of (secretKey + hash)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("PrivateKeyWithUserHash((string secKey, string hashedKey) is obsolete.", false)]
        internal static string PrivateKeyWithUserHash(string secKey, string hashedKey)
        {
            if (string.IsNullOrEmpty(secKey))
                throw new ArgumentNullException("secKey");

            string usrHash = string.IsNullOrEmpty(hashedKey) ? EnDeCodeHelper.KeyToHex(secKey) : hashedKey;

            return string.Concat(secKey, usrHash);
        }


        /// <summary>
        /// PrivateKeyWithUserHash, helper to hash merge private user key with hash
        /// </summary>
        /// <param name="key">users private key</param>
        /// <param name="keyHash">key hash</param>
        /// <param name="merge">do merge</param>
        /// <returns>doubled concatendated string of (secretKey + hash)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("Use KeyHashBytes(byte[] keyBytes, byte[] hashBytes, bool mergeKeyHash = true) instead.", false)]
        internal static byte[] KeyUserHashBytes(string key, string keyHash, bool merge = true) =>
                                KeyHashBytes(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(keyHash), true);

        /// <summary>
        /// KeyHashBytes
        /// </summary>
        /// <param name="keyBytes">private key bytes</param>
        /// <param name="hashBytes">key hash bytes</param>
        /// <param name="merge">do merge</param>
        /// <returns>doubled concatendated string of (secretKey + hash)</returns>
        internal static byte[] KeyHashBytes(byte[] keyBytes, byte[] hashBytes, bool merge = true)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw new ArgumentNullException("keyBytes");

            if (hashBytes == null || hashBytes.Length == 0)
                throw new ArgumentNullException("hashBytes");

            if (!merge)
                return keyBytes.TarBytes(hashBytes);

            List<Byte> outBytes = new List<byte>();
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

            return outBytes.ToArray();
        }

        public static byte[] GetKeyBytesSingle(string keyHash, int keyLen = 16) => GetKeyBytesSingle(Encoding.UTF8.GetBytes(keyHash), keyLen);


        public static byte[] GetKeyBytesSingle(byte[] keyBytes, int keyLen = 16)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw new ArgumentNullException("keyBytes");

            byte[] outBytes = new byte[keyLen];
            for (int kb = 0; kb < keyLen; kb++)
                outBytes[kb] = (byte)0;

            if (keyBytes.Length >= keyLen)
                Array.Copy(keyBytes, 0, outBytes, 0, keyLen);
            else
                Array.Copy(keyBytes, 0, outBytes, 0, keyBytes.Length);

            return outBytes;
        }


        public static byte[] GetKeyBytesSimple(string key, string keyHash, int keyLen = 16)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            byte[] outBytes = new byte[keyLen];
            for (int kb = 0; kb < keyLen; kb++)
                outBytes[kb] = (byte)0;

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] hashBytes = Encoding.UTF8.GetBytes(keyHash);
            byte[] keyHashBytes = keyBytes.TarBytes(hashBytes);

            if (keyHashBytes.Length >= keyLen)
                Array.Copy(keyHashBytes, 0, outBytes, 0, keyLen);
            else
                Array.Copy(keyHashBytes, 0, outBytes, 0, keyHashBytes.Length);

            return outBytes;
        }



        /// <summary>
        /// GetUserKeyBytes gets symmetric chiffre private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="key">user key, default email address</param>
        /// <param name="keyHash">user hash</param>        
        /// <param name="keyLen">length of user key bytes, maximum length <see cref="Constants.MAX_KEY_LEN"/></param> 
        /// <returns>Array of byte with length KeyLen</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] GetUserKeyBytes(string key, string keyHash, int keyLen = 32) =>
                                GetKeyHashBytes(
                                    Encoding.UTF8.GetBytes(key),
                                    string.IsNullOrEmpty(keyHash) ? new byte[0] : Encoding.UTF8.GetBytes(keyHash),
                                    keyLen);


        public static byte[] GetKeyHashBytes(byte[] keyBytes, byte[] hashBytes, int keyLen = 32)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw new ArgumentNullException("keyBytes");

            byte[] outBytes = new byte[keyLen];
            for (int kb = 0; kb < keyLen; kb++)
                outBytes[kb] = (byte)0;

            byte[] keyHashBytes = keyBytes.TarBytes(hashBytes);

            if (keyHashBytes.Length >= keyLen)
                Array.Copy(keyHashBytes, 0, outBytes, 0, keyLen);
            else
                Array.Copy(keyHashBytes, 0, outBytes, 0, keyHashBytes.Length);

            return outBytes;
        }


        #endregion GetUserKeyBytes

    }


}

