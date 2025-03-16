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
                case SymmCipherEnum.Aes:
                case SymmCipherEnum.BlowFish:
                case SymmCipherEnum.Fish2:
                case SymmCipherEnum.Fish3:
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.Des3:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.RC532:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                    default:
                    CryptParamsPrefered cpParams = new CryptParamsPrefered(cipherAlgo, secretKey, hashIv);
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
                case SymmCipherEnum.Aes:
                case SymmCipherEnum.BlowFish:
                case SymmCipherEnum.Fish2:
                case SymmCipherEnum.Fish3:
                case SymmCipherEnum.Camellia:
                case SymmCipherEnum.Cast6:
                case SymmCipherEnum.Des3:
                case SymmCipherEnum.Gost28147:
                case SymmCipherEnum.Idea:
                case SymmCipherEnum.RC532:
                case SymmCipherEnum.Seed:
                case SymmCipherEnum.SkipJack:
                case SymmCipherEnum.Tea:
                case SymmCipherEnum.XTea:
                default:
                    CryptParamsPrefered cpParams = new CryptParamsPrefered(cipherAlgo, secretKey, hashIv);
                    CryptBounceCastle cryptBounceCastle = new CryptBounceCastle(cpParams, true);
                    decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
                    break;                
            }

            return EnDeCodeHelper.GetBytesTrimNulls(decryptBytes);
            // return decryptBytes;
        }

    }


}
