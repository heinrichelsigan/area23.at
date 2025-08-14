using Area23.At.Framework.Library.Crypt.EnDeCoding;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    /// <summary>
    /// Crypt encapsulates <see cref="CipherPipe.EncryptBytesFast(byte[], CipherEnum, string, string)
    /// and <see cref="CipherPipe.DecryptBytesFast(byte[], CipherEnum, string, string, bool)"/> 
    /// </summary>
    public class Crypt
    {

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytes(byte[] inBytes, CipherEnum cipherAlgo = CipherEnum.Aes,
            string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            string hashIv = (string.IsNullOrEmpty(keyIv)) ? EnDeCodeHelper.KeyToHex(secretKey) : keyIv;
            return CipherPipe.EncryptBytesFast(inBytes, cipherAlgo, secretKey, hashIv);
        }


        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="cipherAlgo">both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytes(byte[] cipherBytes, CipherEnum cipherAlgo = CipherEnum.Aes,
            string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            string hashIv = (string.IsNullOrEmpty(keyIv)) ? EnDeCodeHelper.KeyToHex(secretKey) : keyIv;
            return CipherPipe.DecryptBytesFast(cipherBytes, cipherAlgo, secretKey, hashIv);
        }

    }

}
