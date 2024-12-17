using Org.BouncyCastle.Crypto;

namespace Area23.At.Framework.Library.Core.Cipher.Symm
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
                Symm.Algo.Fish2.Fish2GenWithKey(secretKey, keyIv, true);
                encryptBytes = Symm.Algo.Fish2.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.FISH3 || algo == "3FISH" || algo == "3FISH")
            {
                Symm.Algo.Fish3.Fish3GenWithKey(secretKey, keyIv, true);
                encryptBytes = Symm.Algo.Fish3.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.DES3 || algo == "3DES" || algo == "DES3")
            {
                Symm.Algo.Des3.Des3FromKey(secretKey, keyIv, true);
                encryptBytes = Symm.Algo.Des3.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.AES || algo == "AES")
            {
                Symm.Algo.Aes.AesGenWithNewKey(secretKey, keyIv, true);
                encryptBytes = Symm.Algo.Aes.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.Rijndael || algo == "Rijndael")
            {
                Symm.Algo.Rijndael.RijndaelGenWithKey(secretKey, keyIv, true);
                encryptBytes = Symm.Algo.Rijndael.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.Rsa || algo == "Rsa")
            {
                var keyPair = Asym.Algo.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                string privKey = keyPair.Private.ToString();
                encryptBytes = Asym.Algo.Rsa.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.Serpent || algo == "Serpent")
            {
                Symm.Algo.Serpent.SerpentGenWithKey(secretKey, keyIv, true);
                encryptBytes = Symm.Algo.Serpent.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.ZenMatrix || algo == "ZenMatrix")
            {
                Symm.Algo.ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, true);
                encryptBytes = Symm.Algo.ZenMatrix.Encrypt(inBytes);
            }
            if (algo == "Camellia" || algo == "Cast5" || algo == "Cast6" ||
                algo == "Gost28147" || algo == "Idea" || algo == "Noekeon" ||
                algo == "RC2" || algo == "RC532" || algo == "RC6" || // || algo == "RC564"
                                                                     // algo == "Rijndael" ||
                algo == "Seed" || algo == "SkipJack" || // algo == "Serpent" ||
                algo == "Tea" || algo == "Tnepres" || algo == "XTea")
            {
                IBlockCipher blockCipher = Symm.CryptHelper.GetBlockCipher(algo, ref mode, ref blockSize, ref keyLen);

                Symm.CryptBounceCastle cryptBounceCastle = new Symm.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, keyIv, secretKey, true);
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
                sameKey = Symm.Algo.Fish2.Fish2GenWithKey(secretKey, keyIv, false);
                decryptBytes = Symm.Algo.Fish2.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.FISH3 || algorithmName == "3FISH" || algorithmName == "FISH3")
            {
                sameKey = Symm.Algo.Fish3.Fish3GenWithKey(secretKey, keyIv, true);
                decryptBytes = Symm.Algo.Fish3.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.DES3 || algorithmName == "3DES" || algorithmName == "DES3")
            {
                sameKey = Symm.Algo.Des3.Des3FromKey(secretKey, keyIv, true);
                decryptBytes = Symm.Algo.Des3.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.AES || algorithmName == "AES")
            {
                sameKey = Symm.Algo.Aes.AesGenWithNewKey(secretKey, keyIv, false);
                decryptBytes = Symm.Algo.Aes.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Rijndael || algorithmName == "Rijndael")
            {
                Symm.Algo.Rijndael.RijndaelGenWithKey(secretKey, keyIv, false);
                decryptBytes = Symm.Algo.Rijndael.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Rsa || algorithmName == "Rsa")
            {
                var keyPair = Asym.Algo.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                string privKey = keyPair.Private.ToString();
                decryptBytes = Asym.Algo.Rsa.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Serpent || algorithmName == "Serpent")
            {
                sameKey = Symm.Algo.Serpent.SerpentGenWithKey(secretKey, keyIv, false);
                decryptBytes = Symm.Algo.Serpent.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.ZenMatrix || algorithmName == "ZenMatrix")
            {
                sameKey = Symm.Algo.ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, false);
                decryptBytes = Symm.Algo.ZenMatrix.Decrypt(cipherBytes);
            }
            if (algorithmName == "Camellia" || algorithmName == "Cast5" || algorithmName == "Cast6" ||
                algorithmName == "Gost28147" || algorithmName == "Idea" || algorithmName == "Noekeon" ||
                algorithmName == "RC2" || algorithmName == "RC532" || algorithmName == "RC6" || // || algorithmName == "RC564" 
                                                                                                // algorithmName == "Rijndael" ||
                algorithmName == "Seed" || algorithmName == "SkipJack" || // algorithmName == "Serpent" || 
                algorithmName == "Tea" || algorithmName == "Tnepres" || algorithmName == "XTea")
            {
                IBlockCipher blockCipher = Symm.CryptHelper.GetBlockCipher(algorithmName, ref mode, ref blockSize, ref keyLen);

                Symm.CryptBounceCastle cryptBounceCastle = new Symm.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, keyIv, secretKey, true);
                decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
            }

            return decryptBytes;
        }

    }

}
