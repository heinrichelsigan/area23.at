using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Static;
using Org.BouncyCastle.Crypto.Generators;
using System.Windows.Forms;

namespace Area23.At.Framework.Core.Crypt.Cipher
{

    /// <summary>
    /// static class CryptHelper provides static helper methods for encryption / decryption
    /// </summary>
    public static class CryptHelper
    {
        const ushort PASSWD_BYTE_LEN = 64;
        const ushort SALT_BYTE_LEN = 16;
        const ushort AVG_COST = 46;

        /// <summary>
        /// <see cref="Org.BouncyCastle.Crypto.Generators.BCrypt"/>
        /// Thanx to the legion of <see href="https://bouncycastle.org/"" />
        /// </summary>
        /// <param name="passwd">passwd or key to encrypt</param>
        /// <returns>bcrypted byte[]</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] BCrypt(string passwd)
        {
            if (string.IsNullOrEmpty(passwd))
                throw new ArgumentNullException("passwd");

            byte[] keyBytes = EnDeCodeHelper.GetBytes(passwd);
            
            if (keyBytes.Length > PASSWD_BYTE_LEN)
                throw new ArgumentException($"BCrypt(passwd) => GetBytes(passwd) => {Hex16.ToHex16(keyBytes)} Length {keyBytes.LongLength} > {PASSWD_BYTE_LEN} bytes", "passwd");

            byte[] salt = EnDeCodeHelper.KeyToHexBytes(passwd, SALT_BYTE_LEN);
            
            byte[] bcrypted = Org.BouncyCastle.Crypto.Generators.BCrypt.Generate(keyBytes, salt, AVG_COST);
            
            return bcrypted;
        }



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


        /// <summary>
        /// GetUserKeyBytes gets symmetric chiffre private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="key">user key, default email address</param>
        /// <param name="keyHash">user hash</param>        
        /// <param name="keyLen">length of user key bytes, maximum length <see cref="Constants.MAX_KEY_LEN"/></param>        
        /// <param name="useBcrypt">use bcrypted key <see cref="BCrypt(string)"/></param>
        /// <returns>Array of byte with length KeyLen</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] GetUserKeyBytes(string key, string keyHash, int keyLen = 32, bool useBcrypt = false)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            byte[] keyBytes = (useBcrypt) ? BCrypt(key) : EnDeCodeHelper.GetBytes(key);
            byte[] hashBytes = string.IsNullOrEmpty(keyHash) ? EnDeCodeHelper.GetBytes(Hex16.ToHex16(keyBytes)) : EnDeCodeHelper.GetBytes(keyHash);
            // keyHash = (string.IsNullOrEmpty(keyHash)) ? EnDeCodeHelper.KeyToHex(key) : keyHash;

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

        #endregion GetUserKeyBytes

    }

}
