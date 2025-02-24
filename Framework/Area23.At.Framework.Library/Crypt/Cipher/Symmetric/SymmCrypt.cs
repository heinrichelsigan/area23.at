using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
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
        public static new byte[] EncryptBytes(byte[] inBytes,
            SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes,
            string secretKey = "heinrich.elsigan@area23.at",
            string hashIv = "6865696e726963682e656c736967616e406172656132332e6174")
        {
            byte[] encryptBytes = inBytes;

            string algo = cipherAlgo.ToString();
            switch (cipherAlgo)
            {
                //case SymmCipherEnum.Des3:
                //    Des3.Des3GenWithKeyHash(secretKey, hashIv, true);
                //    encryptBytes = Des3.Encrypt(inBytes);
                //    break;
                //case SymmCipherEnum.Fish2:
                //    Fish2.Fish2GenWithKeyHash(secretKey, hashIv, true);
                //    encryptBytes = Fish2.Encrypt(inBytes);
                //    break;
                //case SymmCipherEnum.Fish3:
                //    Fish3.Fish3GenWithKeyHash(secretKey, hashIv, true);
                //    encryptBytes = Fish3.Encrypt(inBytes);
                //    break;
                //case SymmCipherEnum.Rijndael:
                //    Rijndael.RijndaelGenWithNewKey(secretKey, hashIv, true);
                //    encryptBytes = Rijndael.Encrypt(inBytes);
                //    break;
                case SymmCipherEnum.Serpent:
                    Serpent.SerpentGenWithKey(secretKey, hashIv, true);
                    encryptBytes = Serpent.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.ZenMatrix:
                    encryptBytes = (new ZenMatrix(secretKey, hashIv, false)).Encrypt(inBytes);
                    break;
                case SymmCipherEnum.Des3:
                case SymmCipherEnum.BlowFish:
                case SymmCipherEnum.Fish2:
                case SymmCipherEnum.Fish3:
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.RC532:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                default:
                    CryptParamsPrefered cpParams = CryptHelper.GetPreferedCryptParams(cipherAlgo);
                    cpParams.Key = secretKey;
                    cpParams.Hash = hashIv;
                    CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(cpParams, true);
                    encryptBytes = cryptBounceCastle.Encrypt(inBytes);
                    break;
            }


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
        public static new byte[] DecryptBytes(byte[] cipherBytes,
            SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes,
            string secretKey = "heinrich.elsigan@area23.at",
            string hashIv = "6865696e726963682e656c736967616e406172656132332e6174")
        {
            bool sameKey = true;
            byte[] decryptBytes = cipherBytes;

            switch (cipherAlgo)
            {
                case SymmCipherEnum.Des3:
                    sameKey = Des3.Des3GenWithKeyHash(secretKey, hashIv, true);
                    decryptBytes = Des3.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Fish2:
                    sameKey = Fish2.Fish2GenWithKeyHash(secretKey, hashIv, true);
                    decryptBytes = Fish2.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Fish3:
                    sameKey = Fish3.Fish3GenWithKeyHash(secretKey, hashIv, true);
                    decryptBytes = Fish3.Decrypt(cipherBytes);
                    break;
                //case SymmCipherEnum.Rijndael:
                //    sameKey = Rijndael.RijndaelGenWithNewKey(secretKey, hashIv, true);
                //    decryptBytes = Rijndael.Decrypt(cipherBytes);
                //    break;
                case SymmCipherEnum.Serpent:
                    sameKey = Serpent.SerpentGenWithKey(secretKey, hashIv, true);
                    decryptBytes = Serpent.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.ZenMatrix:
                    decryptBytes = (new ZenMatrix(secretKey, hashIv, false)).Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.BlowFish:
                // case SymmCipherEnum.Fish2:
                // case SymmCipherEnum.Fish3:
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.RC532:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                    CryptParamsPrefered cpParams = CryptHelper.GetPreferedCryptParams(cipherAlgo);
                    cpParams.Key = secretKey;
                    cpParams.Hash = hashIv;
                    CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(cpParams, true);
                    decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Aes:
                default:
                    sameKey = Aes.AesGenWithKeyHash(secretKey, hashIv, true);
                    decryptBytes = Aes.Decrypt(cipherBytes);
                    break;
            }

            return DeEnCoder.GetBytesTrimNulls(decryptBytes);
            // return decryptBytes;
        }


        #region merry_go_rount
        /*
        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/> both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>encrypted byte Array</returns>
        public static byte[] EncryptBytesFast(byte[] inBytes, SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes, 
            string secretKey = "postmaster@kernel.org", string hashIv = "")
        {
            byte[] encryptBytes = inBytes;

            string algo = cipherAlgo.ToString();

            switch (cipherAlgo)
            {
                case SymmCipherEnum.Des3:
                    Des3.Des3FromKey(secretKey, hashIv, true);
                    encryptBytes = Des3.Encrypt(inBytes);
                    break;                
                case SymmCipherEnum.Serpent:
                    Serpent.SerpentGenWithKey(secretKey, hashIv, true);
                    encryptBytes = Serpent.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.ZenMatrix:
                    ZenMatrix.ZenMatrixGenWithKey(secretKey, hashIv, true);
                    encryptBytes = ZenMatrix.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.BlowFish:
                case SymmCipherEnum.Fish2:
                case SymmCipherEnum.Fish3:
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.RC532:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                    CryptParamsPrefered cpParams = CryptHelper.GetPreferedCryptParams(cipherAlgo);
                    cpParams.Key = secretKey;
                    cpParams.Hash = hashIv;
                    Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);                    
                    encryptBytes = cryptBounceCastle.Encrypt(inBytes);
                    break;
                case SymmCipherEnum.Aes:
                default:
                    Aes.AesGenWithNewKey(secretKey, hashIv, true);
                    encryptBytes = Aes.Encrypt(inBytes);
                    break;
            }

            return encryptBytes;
        }
        */

        /*
        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/>both symmetric and asymetric cipher algorithms</param>
        /// <param name="secretKey">secret key to decrypt</param>
        /// <param name="keyIv">key's iv</param>
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytesFast(byte[] cipherBytes, SymmCipherEnum cipherAlgo = SymmCipherEnum.Aes, 
            string secretKey = "postmaster@kernel.org", string hashIv = "")
        {
            bool sameKey = true;
            string algorithmName = cipherAlgo.ToString();
            byte[] decryptBytes = cipherBytes;

            switch (cipherAlgo)
            {

                case SymmCipherEnum.Des3:
                    sameKey = Des3.Des3FromKey(secretKey, hashIv, true);
                    decryptBytes = Des3.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Serpent:
                    sameKey = Serpent.SerpentGenWithKey(secretKey, hashIv, true);
                    decryptBytes = Serpent.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.ZenMatrix:
                    sameKey = ZenMatrix.ZenMatrixGenWithKey(secretKey, hashIv, true);
                    decryptBytes = ZenMatrix.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.BlowFish:
                case SymmCipherEnum.Fish2:
                case SymmCipherEnum.Fish3:
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.RC532:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                    CryptParamsPrefered cpParams = CryptHelper.GetPreferedCryptParams(cipherAlgo);
                    cpParams.Key = secretKey;
                    cpParams.Hash = hashIv;
                    Symmetric.CryptBounceCastle cryptBounceCastle = new Symmetric.CryptBounceCastle(cpParams, true);                    
                    decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
                    break;
                case SymmCipherEnum.Aes:
                default:
                    sameKey = Aes.AesGenWithNewKey(secretKey, hashIv, true);
                    decryptBytes = Aes.Decrypt(cipherBytes);
                    break;
            }

            // return DeEnCoder.GetBytesTrimNulls(decryptBytes);
            return decryptBytes;
        }
        */

        /*
        /// <summary>
        /// MerryGoRoundEncrpyt starts merry to go arround from left to right in clock hour cycle
        /// </summary>
        /// <param name="inBytes">plain <see cref="byte[]"/ to encrypt></param>
        /// <param name="secretKey">user secret key to use for all symmetric cipher algorithms in the pipe</param>
        /// <param name="keyIv">hash key iv relational to secret key</param>
        /// <returns>encrypted byte[]</returns>
        public static byte[] MerryGoRoundEncrpyt(byte[] inBytes, SymmCipherPipe spipe, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] keyHashBytes = CryptHelper.GetUserKeyBytes(secretKey, keyIv, 16);
            byte[] encryptedBytes = new byte[inBytes.Length * 2];

            foreach (var symmCipher in spipe.InPipe)
            {
                encryptedBytes = EncryptBytesFast(inBytes, symmCipher, secretKey, keyIv);
                inBytes = encryptedBytes;
            }

            return encryptedBytes;
        }
        */

        /*
        /// <summary>
        /// DecrpytRoundGoMerry against clock turn -
        /// starts merry to turn arround from right to left against clock hour cycle 
        /// </summary>
        /// <param name="cipherBytes">encrypted byte array</param>
        /// <param name="secretKey">user secret key, normally email address</param>
        /// <param name="keyIv">hash relational to secret kay</param>
        /// <returns><see cref="byte[]"/> plain bytes</returns>
        public static byte[] DecrpytRoundGoMerry(byte[] cipherBytes, SymmCipherPipe dspipe, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] keyHashBytes = CryptHelper.GetUserKeyBytes(secretKey, keyIv, 16);
            byte[] outBytes = new byte[cipherBytes.Length * 2];
            
            foreach (var symmCipher in dspipe.OutPipe)
            {
                outBytes = DecryptBytesFast(cipherBytes, symmCipher, secretKey, keyIv);
                cipherBytes = outBytes;
            }

            return outBytes;
        }

        */
        #endregion merry_go_rount

    }

}
