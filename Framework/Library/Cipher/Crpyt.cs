using Symm = Area23.At.Framework.Library.Cipher.Symmetric;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using System;
using Area23.At.Framework.Library.Cipher.Symmetric;
using System.Linq;

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


        #region merry_go_rount

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytesFast(byte[] inBytes, SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] encryptBytes = inBytes;
            // byte[] outBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            string algo = cipherAlgo.ToString();

            switch (cipherAlgo)
            {
                case SymmCipherEnum.TripleDes:
                    Symm.Algo.Des3.Des3FromKey(secretKey, keyIv, true);
                    encryptBytes = Symm.Algo.Des3.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.TwoFish:
                    Symm.Algo.Fish2.Fish2GenWithKey(secretKey, keyIv, true);
                    encryptBytes = Symm.Algo.Fish2.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.ThreeFish:
                    Symm.Algo.Fish3.Fish3GenWithKey(secretKey, keyIv, true);
                    encryptBytes = Symm.Algo.Fish3.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.Serpent:
                    Symm.Algo.Serpent.SerpentGenWithKey(secretKey, keyIv, true);
                    encryptBytes = Symm.Algo.Serpent.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.ZenMatrix:
                    Symm.Algo.ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, true);
                    encryptBytes = Symm.Algo.ZenMatrix.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.Noekeon:
                case SymmCipherEnum.RC6:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                    IBlockCipher blockCipher = Symm.CryptHelper.GetBlockCipher(algo, ref mode, ref blockSize, ref keyLen);
                    Symm.CryptBounceCastle cryptBounceCastle = new Symm.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, keyIv, secretKey, true);
                    encryptBytes = cryptBounceCastle.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.Aes:
                default:
                    Symm.Algo.Aes.AesGenWithNewKey(secretKey, keyIv, true);
                    encryptBytes = Symm.Algo.Aes.Encrypt(inBytes);
                    break;
            }

            return encryptBytes;
        }

        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/>both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytesFast(byte[] cipherBytes, SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            bool sameKey = true;
            string algorithmName = cipherAlgo.ToString();
            byte[] decryptBytes = cipherBytes;
            // byte[] plainBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            switch (cipherAlgo)
            {
                case SymmCipherEnum.TwoFish:
                    sameKey = Symm.Algo.Fish2.Fish2GenWithKey(secretKey, keyIv, false);
                    decryptBytes = Symm.Algo.Fish2.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.ThreeFish:
                    sameKey = Symm.Algo.Fish3.Fish3GenWithKey(secretKey, keyIv, true);
                    decryptBytes = Symm.Algo.Fish3.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.TripleDes:
                    sameKey = Symm.Algo.Des3.Des3FromKey(secretKey, keyIv, true);
                    decryptBytes = Symm.Algo.Des3.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Serpent:
                    sameKey = Symm.Algo.Serpent.SerpentGenWithKey(secretKey, keyIv, false);
                    decryptBytes = Symm.Algo.Serpent.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.ZenMatrix:
                    sameKey = Symm.Algo.ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, false);
                    decryptBytes = Symm.Algo.ZenMatrix.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.Noekeon:
                case SymmCipherEnum.RC6:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                    IBlockCipher blockCipher = Symm.CryptHelper.GetBlockCipher(algorithmName, ref mode, ref blockSize, ref keyLen);
                    Symm.CryptBounceCastle cryptBounceCastle = new Symm.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, keyIv, secretKey, true);
                    decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Aes:
                default:
                    sameKey = Symm.Algo.Aes.AesGenWithNewKey(secretKey, keyIv, false);
                    decryptBytes = Symm.Algo.Aes.Decrypt(cipherBytes);
                    break;
            }

            return decryptBytes;
        }


        /// <summary>
        /// Gets a encrypted matrix pipeline for a byte arry
        /// </summary>
        /// <param name="keyBytes">private key or hased private key with iv</param>
        /// <param name="MAXPIPE">maximal numbers of encryption cycles</param>
        /// <returns>Array of <see cref="SymmCipherEnum"/>, that is used to perform <see cref="MerryGoRoundEncrpyt(byte[], string, string)"/> encryption cycles</returns>
        public static SymmCipherEnum[] KeyBytesToSymmCipherPipeline(byte[] keyBytes, int MAXPIPE = 8)
        {
            Dictionary<char, SymmCipherEnum> symDict = new Dictionary<char, SymmCipherEnum>();
            List<SymmCipherEnum> symmMatrixPipe = new List<SymmCipherEnum>();

            ushort scnt = 0;
            foreach (SymmCipherEnum symmC in Enum.GetValues(typeof(SymmCipherEnum)))
            {
                string hex = $"{((ushort)symmC):x1}";
                scnt++;
                symDict.Add(hex[0], symmC);
            }

            HashSet<byte> hashBytes = new HashSet<byte>();
            foreach (byte bb in keyBytes)
            {
                if (!hashBytes.Contains(bb))
                    hashBytes.Add(bb);
            }

            string hexString = string.Empty;
            for (int kcnt = 0; kcnt < hashBytes.Count && symmMatrixPipe.Count < MAXPIPE; kcnt++)
            {
                hexString = string.Format("{0:x2}", hashBytes.ElementAt(kcnt));
                if (hexString != null && hexString.Length > 1)
                {
                    SymmCipherEnum sym0 = symDict[hexString[0]];
                    symmMatrixPipe.Add(sym0);
                    SymmCipherEnum sym1 = symDict[hexString[1]];
                    symmMatrixPipe.Add(sym1);
                }
            }

            return symmMatrixPipe.ToArray();
        }


        /// <summary>
        /// MerryGoRoundEncrpyt starts merry to go arround from left to right in clock hour cycle
        /// </summary>
        /// <param name="inBytes">plain <see cref="byte[]"/ to encrypt></param>
        /// <param name="secretKey">user secret key to use for all symmetric cipher algorithms in the pipe</param>
        /// <param name="keyIv">hash key iv relational to secret key</param>
        /// <returns>encrypted byte[]</returns>
        public static byte[] MerryGoRoundEncrpyt(byte[] inBytes, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] keyHashBytes = CryptHelper.GetUserKeyBytes(secretKey, keyIv, 16);
            byte[] encryptedBytes = new byte[inBytes.Length * 2];
            // Array.Copy(inBytes, 0, encryptedBytes, 0, inBytes.Length);

            foreach (var symmCipher in KeyBytesToSymmCipherPipeline(keyHashBytes))
            {
                encryptedBytes = EncryptBytesFast(inBytes, symmCipher, secretKey, keyIv);
                inBytes = encryptedBytes;
            }

            return encryptedBytes;
        }

        /// <summary>
        /// DecrpytRoundGoMerry against clock turn -
        /// starts merry to turn arround from right to left against clock hour cycle 
        /// </summary>
        /// <param name="cipherBytes">encrypted byte array</param>
        /// <param name="secretKey">user secret key, normally email address</param>
        /// <param name="keyIv">hash relational to secret kay</param>
        /// <returns><see cref="byte[]"/> plain bytes</returns>
        public static byte[] DecrpytRoundGoMerry(byte[] cipherBytes, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] keyHashBytes = CryptHelper.GetUserKeyBytes(secretKey, keyIv, 16);
            byte[] outBytes = new byte[cipherBytes.Length * 2];
            // Array.Copy(inBytes, 0, encryptedBytes, 0, inBytes.Length);

            SymmCipherEnum[] symCiphers = KeyBytesToSymmCipherPipeline(keyHashBytes);
            for (int scnt = symCiphers.Length - 1; scnt >= 0; scnt--)
            {
                outBytes = DecryptBytesFast(cipherBytes, symCiphers[scnt], secretKey, keyIv);
                cipherBytes = outBytes;
            }

            return outBytes;
        }


        #endregion merry_go_rount

    }

}
