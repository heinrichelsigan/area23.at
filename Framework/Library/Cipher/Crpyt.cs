using Area23.At.Framework.Library.Cipher.Symmetric;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto;

namespace Area23.At.Framework.Library.Cipher
{

    /// <summary>
    /// Basic class for symm and asym cipher encryption
    /// </summary>
    public static class Crypt
    {

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytes(byte[] inBytes, CipherEnum cipherAlgo = CipherEnum.ZenMatrix, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] encryptBytes = inBytes;
            // byte[] outBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            string algo = cipherAlgo.ToString();
            if (cipherAlgo == CipherEnum.FISH2 || algo == "2FISH" || algo == "FISH2")
            {
                Symmetric.Algo.Fish2.Fish2GenWithKey(secretKey, keyIv, true);
                encryptBytes = Symmetric.Algo.Fish2.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.FISH3 || algo == "3FISH" || algo == "3FISH")
            {
                Symmetric.Algo.Fish3.Fish3GenWithKey(secretKey, keyIv, true);
                encryptBytes = Symmetric.Algo.Fish3.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.DES3 || algo == "3DES" || algo == "DES3")
            {
                Symmetric.Algo.Des3.Des3FromKey(secretKey, keyIv, true);
                encryptBytes = Symmetric.Algo.Des3.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.AES || algo == "AES")
            {
                Symmetric.Algo.Aes.AesGenWithNewKey(secretKey, keyIv, true);
                encryptBytes = Symmetric.Algo.Aes.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.Rijndael || algo == "Rijndael")
            {
                Symmetric.Algo.Rijndael.RijndaelGenWithKey(secretKey, keyIv, true);
                encryptBytes = Symmetric.Algo.Rijndael.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.Rsa || algo == "Rsa")
            {
                var keyPair = Asymmetric.Algo.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                string privKey = keyPair.Private.ToString();
                encryptBytes = Asymmetric.Algo.Rsa.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.Serpent || algo == "Serpent")
            {
                Symmetric.Algo.Serpent.SerpentGenWithKey(secretKey, keyIv, true);
                encryptBytes = Symmetric.Algo.Serpent.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.ZenMatrix || algo == "ZenMatrix")
            {
                Symmetric.Algo.ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, true);
                encryptBytes = Symmetric.Algo.ZenMatrix.Encrypt(inBytes);
            }
            if (algo == "Camellia" || algo == "Cast5" || algo == "Cast6" ||
                algo == "Gost28147" || algo == "Idea" || algo == "Noekeon" ||
                algo == "RC2" || algo == "RC532" || algo == "RC6" || // || algo == "RC564"
                                                                     // algo == "Rijndael" ||
                algo == "Seed" || algo == "SkipJack" || // algo == "Serpent" ||
                algo == "Tea" || algo == "Tnepres" || algo == "XTea")
            {
                IBlockCipher blockCipher = Symmetric.CryptHelper.GetBlockCipher(algo, ref mode, ref blockSize, ref keyLen);

                Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, keyIv, secretKey, true);
                encryptBytes = cryptBounceCastle.Encrypt(inBytes);

            }

            return encryptBytes;
        }


        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="cipherAlgo">both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytes(byte[] cipherBytes, CipherEnum cipherAlgo = CipherEnum.ZenMatrix, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            bool sameKey = true;
            string algorithmName = cipherAlgo.ToString();
            byte[] decryptBytes = cipherBytes;
            // byte[] plainBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            if (cipherAlgo == CipherEnum.FISH2 || algorithmName == "2FISH" || algorithmName == "FISH2")
            {
                sameKey = Symmetric.Algo.Fish2.Fish2GenWithKey(secretKey, keyIv, false);
                decryptBytes = Symmetric.Algo.Fish2.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.FISH3 || algorithmName == "3FISH" || algorithmName == "FISH3")
            {
                sameKey = Symmetric.Algo.Fish3.Fish3GenWithKey(secretKey, keyIv, true);
                decryptBytes = Symmetric.Algo.Fish3.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.DES3 || algorithmName == "3DES" || algorithmName == "DES3")
            {
                sameKey = Symmetric.Algo.Des3.Des3FromKey(secretKey, keyIv, true);
                decryptBytes = Symmetric.Algo.Des3.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.AES || algorithmName == "AES")
            {
                sameKey = Symmetric.Algo.Aes.AesGenWithNewKey(secretKey, keyIv, false);
                decryptBytes = Symmetric.Algo.Aes.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Rijndael || algorithmName == "Rijndael")
            {
                Symmetric.Algo.Rijndael.RijndaelGenWithKey(secretKey, keyIv, false);
                decryptBytes = Symmetric.Algo.Rijndael.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Rsa || algorithmName == "Rsa")
            {
                var keyPair = Asymmetric.Algo.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                string privKey = keyPair.Private.ToString();
                decryptBytes = Asymmetric.Algo.Rsa.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Serpent || algorithmName == "Serpent")
            {
                sameKey = Symmetric.Algo.Serpent.SerpentGenWithKey(secretKey, keyIv, false);
                decryptBytes = Symmetric.Algo.Serpent.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.ZenMatrix || algorithmName == "ZenMatrix")
            {
                sameKey = Symmetric.Algo.ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, false);
                decryptBytes = Symmetric.Algo.ZenMatrix.Decrypt(cipherBytes);
            }
            if (algorithmName == "Camellia" || algorithmName == "Cast5" || algorithmName == "Cast6" ||
                algorithmName == "Gost28147" || algorithmName == "Idea" || algorithmName == "Noekeon" ||
                algorithmName == "RC2" || algorithmName == "RC532" || algorithmName == "RC6" || // || algorithmName == "RC564" 
                                                                                                // algorithmName == "Rijndael" ||
                algorithmName == "Seed" || algorithmName == "SkipJack" || // algorithmName == "Serpent" || 
                algorithmName == "Tea" || algorithmName == "Tnepres" || algorithmName == "XTea")
            {
                IBlockCipher blockCipher = Symmetric.CryptHelper.GetBlockCipher(algorithmName, ref mode, ref blockSize, ref keyLen);

                Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, keyIv, secretKey, true);
                decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
            }

            return decryptBytes;
        }

    }

}
