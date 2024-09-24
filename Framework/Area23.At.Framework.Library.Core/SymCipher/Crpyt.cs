using Org.BouncyCastle.Crypto;

namespace Area23.At.Framework.Library.Core.SymCipher
{
    public static class Crypt
    {

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="algo">Symetric chiffre algorithm</param>        
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytes(byte[] inBytes, string algo = "ZenMatrix", string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] encryptBytes = inBytes;
            // byte[] outBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            if (algo == "2FISH")
            {
                Algo.Fish2.Fish2GenWithKey(secretKey, keyIv, true);
                encryptBytes = Algo.Fish2.Encrypt(inBytes);
            }
            if (algo == "3FISH")
            {
                Algo.Fish3.Fish3GenWithKey(secretKey, keyIv, true);
                encryptBytes = Algo.Fish3.Encrypt(inBytes);
            }
            if (algo == "3DES")
            {
                Algo.Des3.Des3FromKey(secretKey, keyIv, true);
                encryptBytes = Algo.Des3.Encrypt(inBytes);
            }
            if (algo == "AES")
            {
                Algo.Aes.AesGenWithNewKey(secretKey, keyIv, true);
                encryptBytes = Algo.Aes.Encrypt(inBytes);
            }
            if (algo == "Rijndael")
            {
                Algo.Rijndael.RijndaelGenWithKey(secretKey, keyIv, true);
                encryptBytes = Algo.Rijndael.Encrypt(inBytes);
            }
            if (algo == "Serpent")
            {
                Algo.Serpent.SerpentGenWithKey(secretKey, keyIv, true);
                encryptBytes = Algo.Serpent.Encrypt(inBytes);
            }
            if (algo == "ZenMatrix")
            {
                Algo.ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, true);
                encryptBytes = Algo.ZenMatrix.Encrypt(inBytes);
            }
            if (algo == "Camellia" || algo == "Cast5" || algo == "Cast6" ||
                algo == "Gost28147" || algo == "Idea" || algo == "Noekeon" ||
                algo == "RC2" || algo == "RC532" || algo == "RC6" || // || algo == "RC564"
                                                                     // algo == "Rijndael" ||
                algo == "Seed" || algo == "Skipjack" || // algo == "Serpent" ||
                algo == "Tea" || algo == "Tnepres" || algo == "XTea")
            {
                IBlockCipher blockCipher = CryptHelper.GetBlockCipher(algo, ref mode, ref blockSize, ref keyLen);

                CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(blockCipher, blockSize, keyLen, mode, keyIv, secretKey, true);
                encryptBytes = cryptBounceCastle.Encrypt(inBytes);

            }

            return encryptBytes;
        }


        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="algorithmName">Symetric chiffre algorithm</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytes(byte[] cipherBytes, string algorithmName = "ZenMatrix", string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            bool sameKey = true;

            byte[] decryptBytes = cipherBytes;
            // byte[] plainBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            if (algorithmName == "2FISH")
            {
                sameKey = Algo.Fish2.Fish2GenWithKey(secretKey, keyIv, false);
                decryptBytes = Algo.Fish2.Decrypt(cipherBytes);
            }
            if (algorithmName == "3FISH")
            {
                sameKey = Algo.Fish3.Fish3GenWithKey(secretKey, keyIv, true);
                decryptBytes = Algo.Fish3.Decrypt(cipherBytes);
            }
            if (algorithmName == "3DES")
            {
                sameKey = Algo.Des3.Des3FromKey(secretKey, keyIv, true);
                decryptBytes = Algo.Des3.Decrypt(cipherBytes);
            }
            if (algorithmName == "AES")
            {
                sameKey = Algo.Aes.AesGenWithNewKey(secretKey, keyIv, false);
                decryptBytes = Algo.Aes.Decrypt(cipherBytes);
            }
            if (algorithmName == "Rijndael")
            {
                Algo.Rijndael.RijndaelGenWithKey(secretKey, keyIv, false);
                decryptBytes = Algo.Rijndael.Decrypt(cipherBytes);
            }
            if (algorithmName == "Serpent")
            {
                sameKey = Algo.Serpent.SerpentGenWithKey(secretKey, keyIv, false);
                decryptBytes = Algo.Serpent.Decrypt(cipherBytes);
            }
            if (algorithmName == "ZenMatrix")
            {
                sameKey = Algo.ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, false);
                decryptBytes = Algo.ZenMatrix.Decrypt(cipherBytes);
            }
            if (algorithmName == "Camellia" || algorithmName == "Cast5" || algorithmName == "Cast6" ||
                algorithmName == "Gost28147" || algorithmName == "Idea" || algorithmName == "Noekeon" ||
                algorithmName == "RC2" || algorithmName == "RC532" || algorithmName == "RC6" || // || algorithmName == "RC564" 
                                                                                                // algorithmName == "Rijndael" ||
                algorithmName == "Seed" || algorithmName == "Skipjack" || // algorithmName == "Serpent" || 
                algorithmName == "Tea" || algorithmName == "Tnepres" || algorithmName == "XTea")
            {
                IBlockCipher blockCipher = CryptHelper.GetBlockCipher(algorithmName, ref mode, ref blockSize, ref keyLen);

                CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(blockCipher, blockSize, keyLen, mode, keyIv, secretKey, true);
                decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
            }

            return decryptBytes;
        }

    }
}
