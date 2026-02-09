using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    /// <summary>
    /// static class CryptHelper provides static helper methods for encryption / decryption
    /// Everything under the namespace `Area23.At.Framework.Library.Crypt.Cipher` is licensed under the MIT License.
    /// <see href="https://opensource.org/license/mit">opensource.org/license/mit</see>
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
        /// <param name="secKey">users private secret key</param>
        /// <param name="keyHash">users private secret key hash</param>
        /// <returns>doubled concatendated string of (secretKey + hash)</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static string PrivateKeyWithUserHash(string secKey, string kayHash)
        {
            if (string.IsNullOrEmpty(secKey))
                throw new ArgumentNullException("secKey");

            string usrHash = string.IsNullOrEmpty(kayHash) ? EnDeCodeHelper.KeyToHex(secKey) : kayHash;

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
        internal static byte[] KeyUserHashBytes(string key, string keyHash, bool merge = true)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            keyHash = (string.IsNullOrEmpty(keyHash)) ? EnDeCodeHelper.KeyToHex(key) : keyHash;

            byte[] keyBytes = EnDeCodeHelper.GetBytes(key);
            byte[] hashBytes = EnDeCodeHelper.GetBytes(keyHash);

            return KeyHashBytes(keyBytes, hashBytes, merge);
        }

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

        public static byte[] GetKeyBytesSingle(string keyHash, int keyLen = 16)
        {
            if (string.IsNullOrEmpty(keyHash))
                throw new ArgumentNullException("keyHash");
            byte[] keyBytes = EnDeCodeHelper.GetBytes(keyHash);
            byte[] outBytes = new byte[16];
            if (keyBytes.Length >= 16)
            {
                Array.Copy(keyBytes, 0, outBytes, 0, keyLen);
                return outBytes;
            }
            byte[] smallBytes = keyBytes.TarBytes(EnDeCodeHelper.GetBytes(keyHash));
            if (smallBytes.Length >= keyLen)
            {
                Array.Copy(smallBytes, 0, outBytes, 0, keyLen);
                return outBytes;
            }
            byte[] bigBytes = smallBytes.TarBytes(EnDeCodeHelper.GetBytes(keyHash), keyBytes);
            if (bigBytes.Length >= keyLen)
            {
                Array.Copy(bigBytes, 0, outBytes, 0, keyLen);
                return outBytes;
            }

            return GetUserKeyBytes(keyHash, keyHash, keyLen);
        }

        public static byte[] GetKeyBytesSimple(string key, string keyHash, int keyLen = 16)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            byte[] keyBytes = EnDeCodeHelper.GetBytes(key);
            byte[] outBytes = new byte[16];
            if (keyBytes.Length >= 16)
            {
                Array.Copy(keyBytes, 0, outBytes, 0, keyLen);
                return outBytes;
            }
            byte[] smallBytes = keyBytes.TarBytes(EnDeCodeHelper.GetBytes(keyHash));
            if (smallBytes.Length >= keyLen)
            {
                Array.Copy(smallBytes, 0, outBytes, 0, keyLen);
                return outBytes;
            }
            byte[] bigBytes = smallBytes.TarBytes(EnDeCodeHelper.GetBytes(keyHash), keyBytes);
            if (bigBytes.Length >= keyLen)
            {
                Array.Copy(bigBytes, 0, outBytes, 0, keyLen);
                return outBytes;
            }

            return GetUserKeyBytes(key, keyHash, keyLen);
        }

        /// <summary>
        /// GetUserKeyBytes gets symmetric chiffre private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="key">user key, default email address</param>
        /// <param name="keyHash">user hash</param>        
        /// <param name="keyLen">length of user key bytes, maximum length <see cref="Constants.MAX_KEY_LEN"/></param> 
        /// <returns>Array of byte with length KeyLen</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] GetUserKeyBytes(string key, string keyHash, int keyLen = 32)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            byte[] keyBytes = EnDeCodeHelper.GetBytes(key);
            // keyHash = (string.IsNullOrEmpty(keyHash)) ? EnDeCodeHelper.KeyToHex(key) : keyHash;
            byte[] hashBytes = string.IsNullOrEmpty(keyHash) ? EnDeCodeHelper.GetBytes(Hex16.ToHex16(keyBytes)) : EnDeCodeHelper.GetBytes(keyHash);            

            int keyByteCnt = -1;
            keyLen = (keyLen > Constants.MAX_KEY_LEN) ? Constants.MAX_KEY_LEN : keyLen;
            string keyByteHashString = key;
            byte[] tmpKey = new byte[keyLen];

            byte[] keyHashBytes = KeyHashBytes(keyBytes, hashBytes);
            keyByteCnt = keyHashBytes.Length;
            byte[] keyHashTarBytes = new byte[keyByteCnt * 2 + 1];

            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(KeyHashBytes(hashBytes, keyBytes));
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(
                    KeyHashBytes(hashBytes, keyBytes),
                    KeyHashBytes(keyBytes, hashBytes)
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


        public static byte[] GetKeyBytesFromBytes(byte[] keyBytes, int keyLen = 32)
        {
            if (keyBytes == null || keyBytes.Length == 0)
                throw new ArgumentNullException("keyBytes");

            byte[] hashBytes = EnDeCodeHelper.GetBytes(Hex16.ToHex16(keyBytes));

            int keyByteCnt = -1;
            keyLen = (keyLen > Constants.MAX_KEY_LEN) ? Constants.MAX_KEY_LEN : keyLen;
            byte[] tmpKey = new byte[keyLen];

            byte[] keyHashBytes = KeyHashBytes(keyBytes, hashBytes);
            keyByteCnt = keyHashBytes.Length;
            byte[] keyHashTarBytes = new byte[keyByteCnt * 2 + 1];

            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(KeyHashBytes(hashBytes, keyBytes));
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(
                    KeyHashBytes(hashBytes, keyBytes),
                    KeyHashBytes(keyBytes, hashBytes)
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
