using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using System;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// Basic class for symmetric cipher encryption
    /// </summary>
    public class SymmCrypt : Area23.At.Framework.Library.Crypt.Cipher.Crypt
    {

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="hashIv">key hash iv</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytes(byte[] inBytes,
            SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes,
            string secretKey = "heinrich.elsigan@area23.at",
            string hashIv = "6865696e726963682e656c736967616e406172656132332e6174")
        {
            byte[] encryptBytes = SymmCipherPipe.EncryptBytesFast(inBytes, cipherAlgo, secretKey, hashIv);
            return encryptBytes;
        }

        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="cipherAlgo">both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="hashIv">key hash iv</param>
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytes(byte[] cipherBytes,
            SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes,
            string secretKey = "heinrich.elsigan@area23.at",
            string hashIv = "6865696e726963682e656c736967616e406172656132332e6174")
        {
            byte[] decryptBytes = SymmCipherPipe.DecryptBytesFast(cipherBytes, cipherAlgo, secretKey, hashIv);
            return decryptBytes;
        }


        public static new byte[] EncryptBytes(byte[] inBytes, CipherEnum cipherAlgo = CipherEnum.Aes,
            string secretKey = "", string keyIv = "")
        {
            throw new NotImplementedException("Use: static byte[] EncryptBytes(byte[] cipherBytes, SymmCipherEnum cipherAlgo, string secretKey, string hashIV)");
        }

        public static new byte[] DecryptBytes(byte[] ipherBytes, CipherEnum cipherAlgo = CipherEnum.Aes,
            string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            throw new NotImplementedException("Use: static byte[] DecryptBytes(byte[] cipherBytes, SymmCipherEnum cipherAlgo, string secretKey, string hashIV)");
        }

    }

}

