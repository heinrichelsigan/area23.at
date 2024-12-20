using Area23.At.Framework.Library.Core.Cipher.Symm;
using Area23.At.Framework.Library.Core.Cipher.ASym;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto;

namespace Area23.At.Framework.Library.Core.Cipher
{

    /// <summary>
    /// Basic functionality for Crypt, <see cref="Area23.At.Framework.Library.Core.Cipher.Symm.Crypt"/>
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
            if (cipherAlgo == CipherEnum.Fish2 || algo == "2Fish" || algo == "Fish2")
            {
                Symm.Algo.Fish2.Fish2GenWithKey(secretKey, keyIv, true);
                encryptBytes = Symm.Algo.Fish2.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.Fish3 || algo == "3Fish" || algo == "Fish3")
            {
                Symm.Algo.Fish3.Fish3GenWithKey(secretKey, keyIv, true);
                encryptBytes = Symm.Algo.Fish3.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.Des3 || algo == "3Des" || algo == "Des3")
            {
                Symm.Algo.Des3.Des3FromKey(secretKey, keyIv, true);
                encryptBytes = Symm.Algo.Des3.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.Aes || algo == "Aes")
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
                var keyPair = ASym.Algo.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                string privKey = keyPair.Private.ToString();
                encryptBytes = ASym.Algo.Rsa.Encrypt(inBytes);
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
            if (algo == "BlowFish" || 
                algo == "Camellia" || algo == "Cast5" || algo == "Cast6" ||
                algo == "Gost28147" || algo == "Idea" || algo == "Noekeon" ||
                algo == "RC2" || algo == "RC532" || algo == "RC6" ||
                // algo == "RC564" || algo == "Rijndael" ||  algo == "Serpent" ||
                algo == "Seed" || algo == "SkipJack" ||
                algo == "Tea" || algo == "Tnepres" || algo == "XTea" ||
                
                cipherAlgo == CipherEnum.BlowFish ||
                cipherAlgo == CipherEnum.Camellia || cipherAlgo == CipherEnum.Cast5 || cipherAlgo == CipherEnum.Cast6 ||
                cipherAlgo == CipherEnum.Gost28147 || cipherAlgo == CipherEnum.Idea || cipherAlgo == CipherEnum.Noekeon ||
                cipherAlgo == CipherEnum.RC2 || cipherAlgo == CipherEnum.RC532 || cipherAlgo == CipherEnum.RC6 ||
                cipherAlgo == CipherEnum.Seed || cipherAlgo == CipherEnum.SkipJack ||
                cipherAlgo == CipherEnum.Tea || cipherAlgo == CipherEnum.Tnepres || cipherAlgo == CipherEnum.XTea)
            {
                IBlockCipher blockCipher = CryptHelper.GetBlockCipher(cipherAlgo, ref mode, ref blockSize, ref keyLen);

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

            if (cipherAlgo == CipherEnum.Fish2 || algorithmName == "2Fish" || algorithmName == "Fish2")
            {
                sameKey = Symm.Algo.Fish2.Fish2GenWithKey(secretKey, keyIv, false);
                decryptBytes = Symm.Algo.Fish2.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Fish3 || algorithmName == "3Fish" || algorithmName == "Fish3")
            {
                sameKey = Symm.Algo.Fish3.Fish3GenWithKey(secretKey, keyIv, true);
                decryptBytes = Symm.Algo.Fish3.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Des3 || algorithmName == "3Des" || algorithmName == "Des3")
            {
                sameKey = Symm.Algo.Des3.Des3FromKey(secretKey, keyIv, true);
                decryptBytes = Symm.Algo.Des3.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Aes || algorithmName == "Aes")
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
                var keyPair = ASym.Algo.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                string privKey = keyPair.Private.ToString();
                decryptBytes = ASym.Algo.Rsa.Decrypt(cipherBytes);
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
            if (algorithmName == "BlowFish" ||
                algorithmName == "Camellia" || algorithmName == "Cast5" || algorithmName == "Cast6" ||
                algorithmName == "Gost28147" || algorithmName == "Idea" || algorithmName == "Noekeon" ||
                algorithmName == "RC2" || algorithmName == "RC532" || algorithmName == "RC6" ||
                // || algorithmName == "RC564" || algorithmName == "Rijndael" || algorithmName == "Serpent" || 
                algorithmName == "Seed" || algorithmName == "SkipJack" ||
                algorithmName == "Tea" || algorithmName == "Tnepres" || algorithmName == "XTea" ||

                cipherAlgo == CipherEnum.BlowFish ||
                cipherAlgo == CipherEnum.Camellia || cipherAlgo == CipherEnum.Cast5 || cipherAlgo == CipherEnum.Cast6 ||
                cipherAlgo == CipherEnum.Gost28147 || cipherAlgo == CipherEnum.Idea || cipherAlgo == CipherEnum.Noekeon ||
                cipherAlgo == CipherEnum.RC2 || cipherAlgo == CipherEnum.RC532 || cipherAlgo == CipherEnum.RC6 ||
                cipherAlgo == CipherEnum.Seed || cipherAlgo == CipherEnum.SkipJack ||
                cipherAlgo == CipherEnum.Tea || cipherAlgo == CipherEnum.Tnepres || cipherAlgo == CipherEnum.XTea)
            {
                
                IBlockCipher blockCipher = CryptHelper.GetBlockCipher(cipherAlgo, ref mode, ref blockSize, ref keyLen);

                Symm.CryptBounceCastle cryptBounceCastle = new Symm.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, keyIv, secretKey, true);
                decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
            }

            return decryptBytes;
        }


    }

}
