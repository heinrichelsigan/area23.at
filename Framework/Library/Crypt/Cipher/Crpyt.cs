using Symm = Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using System;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using System.Linq;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using System.Security.Cryptography;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    /// <summary>
    /// Basic functionality for Crypt, <see cref="Area23.At.Framework.Library.Core.Cipher.Symm.Crypt"/>
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
        public static byte[] EncryptBytes(byte[] inBytes, CipherEnum cipherAlgo = CipherEnum.ZenMatrix, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] encryptBytes = inBytes;

            string algo = cipherAlgo.ToString();
                    
            if (cipherAlgo == CipherEnum.Des3 || algo == "3Des" || algo == "Des3")
            {
                Des3.Des3FromKey(secretKey, keyIv, true);
                encryptBytes = Des3.Encrypt(inBytes);
                return encryptBytes;
            }
            if (cipherAlgo == CipherEnum.Aes || algo == "Aes")
            {
                Symmetric.Aes.AesGenWithNewKey(secretKey, keyIv, true);
                encryptBytes = Symmetric.Aes.Encrypt(inBytes);
                return encryptBytes;
            }            
            if (cipherAlgo == CipherEnum.Rsa || algo == "Rsa")
            {
                var keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                string privKey = keyPair.Private.ToString();
                encryptBytes = Asymmetric.Rsa.Encrypt(inBytes);
                return encryptBytes;
            }
            if (cipherAlgo == CipherEnum.RC564 || algo == "RC564")
            {
                RC564.RC564GenWithKey(secretKey, keyIv, true);
                encryptBytes = RC564.Encrypt(inBytes);
                return encryptBytes;
            }
            if (cipherAlgo == CipherEnum.Serpent || algo == "Serpent")
            {
                Serpent.SerpentGenWithKey(secretKey, keyIv, true);
                encryptBytes = Serpent.Encrypt(inBytes);
                return encryptBytes;
            }
            if (cipherAlgo == CipherEnum.ZenMatrix || algo == "ZenMatrix")
            {
                ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, true);
                encryptBytes = ZenMatrix.Encrypt(inBytes);
                return encryptBytes;
            }
            if (algo == "Aes" || algo == "BlowFish" || 
                algo == "2Fish" || algo == "Fish2" || algo == "3Fish" || algo == "Fish3" ||
                algo == "Camellia" || algo == "Cast5" || algo == "Cast6" ||
                algo == "Gost28147" || algo == "Idea" || algo == "Noekeon" ||
                algo == "RC2" || algo == "RC532" || algo == "RC564" || algo == "RC6" ||
                algo == "Rijndael" ||  algo == "Serpent" ||
                algo == "Rijndael" || algo == "Seed" || algo == "SkipJack" ||
                algo == "Tea" || algo == "Tnepres" || algo == "XTea" ||

                cipherAlgo == CipherEnum.Aes || cipherAlgo == CipherEnum.BlowFish || 
                cipherAlgo == CipherEnum.Fish2 || cipherAlgo == CipherEnum.Fish3 ||
                cipherAlgo == CipherEnum.Camellia || cipherAlgo == CipherEnum.Cast5 || cipherAlgo == CipherEnum.Cast6 ||
                cipherAlgo == CipherEnum.Gost28147 || cipherAlgo == CipherEnum.Idea || cipherAlgo == CipherEnum.Noekeon ||
                cipherAlgo == CipherEnum.RC2 || cipherAlgo == CipherEnum.RC532 || cipherAlgo == CipherEnum.RC564 || cipherAlgo == CipherEnum.RC6 ||
                cipherAlgo == CipherEnum.Seed || cipherAlgo == CipherEnum.SkipJack ||
                cipherAlgo == CipherEnum.Tea || cipherAlgo == CipherEnum.Tnepres || cipherAlgo == CipherEnum.XTea)
            {
                CryptParams cparams = new CryptParams(cipherAlgo);
                cparams.Key = secretKey;
                cparams.Hash = keyIv;

                Symm.CryptBounceCastle cryptBounceCastle = new Symm.CryptBounceCastle(cparams, true);
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

            if (cipherAlgo == CipherEnum.Des3 || algorithmName == "3Des" || algorithmName == "Des3")
            {
                sameKey = Des3.Des3FromKey(secretKey, keyIv, true);
                decryptBytes = Des3.Decrypt(cipherBytes);
                return decryptBytes;
            }
            if (cipherAlgo == CipherEnum.Aes || algorithmName == "Aes")
            {
                sameKey = Symmetric.Aes.AesGenWithNewKey(secretKey, keyIv, true);
                decryptBytes = Symmetric.Aes.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.RC564 || algorithmName == "RC564")
            {
                var keyPair = Symmetric.RC564.RC564GenWithKey(secretKey, keyIv, false);
                decryptBytes = Symmetric.RC564.Decrypt(cipherBytes);
                return decryptBytes;
            }
            if (cipherAlgo == CipherEnum.Rsa || algorithmName == "Rsa")
            {
                var keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                string privKey = keyPair.Private.ToString();
                decryptBytes = Asymmetric.Rsa.Decrypt(cipherBytes);
                return decryptBytes;
            }
            if (cipherAlgo == CipherEnum.Serpent || algorithmName == "Serpent")
            {
                sameKey = Serpent.SerpentGenWithKey(secretKey, keyIv, false);
                decryptBytes = Serpent.Decrypt(cipherBytes);
                return decryptBytes;
            }
            if (cipherAlgo == CipherEnum.ZenMatrix || algorithmName == "ZenMatrix")
            {
                sameKey = ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, false);
                decryptBytes = ZenMatrix.Decrypt(cipherBytes);
            }
            if (algorithmName == "BlowFish" || algorithmName == "2Fish" || algorithmName == "Fish2" || algorithmName == "3Fish" || algorithmName == "Fish3" ||
                algorithmName == "Camellia" || algorithmName == "Cast5" || algorithmName == "Cast6" ||
                algorithmName == "Gost28147" || algorithmName == "Idea" || algorithmName == "Noekeon" ||
                algorithmName == "RC2" || algorithmName == "RC532" || algorithmName == "RC564" || algorithmName == "RC6" ||
                // || algorithmName == "RC564" || algorithmName == "Rijndael" || algorithmName == "Serpent" || 
                algorithmName == "Seed" || algorithmName == "SkipJack" ||
                algorithmName == "Tea" || algorithmName == "Tnepres" || algorithmName == "XTea" ||

                cipherAlgo == CipherEnum.BlowFish || cipherAlgo == CipherEnum.Fish2 || cipherAlgo == CipherEnum.Fish3 ||
                cipherAlgo == CipherEnum.Camellia || cipherAlgo == CipherEnum.Cast5 || cipherAlgo == CipherEnum.Cast6 ||
                cipherAlgo == CipherEnum.Gost28147 || cipherAlgo == CipherEnum.Idea || cipherAlgo == CipherEnum.Noekeon ||
                cipherAlgo == CipherEnum.RC2 || cipherAlgo == CipherEnum.RC532 || cipherAlgo == CipherEnum.RC564  || cipherAlgo == CipherEnum.RC6 ||
                cipherAlgo == CipherEnum.Seed || cipherAlgo == CipherEnum.SkipJack ||
                cipherAlgo == CipherEnum.Tea || cipherAlgo == CipherEnum.Tnepres || cipherAlgo == CipherEnum.XTea)
            {

                CryptParams cparams = CryptHelper.GetCryptParams(cipherAlgo);
                cparams.Key = secretKey;
                cparams.Hash = keyIv;

                Symm.CryptBounceCastle cryptBounceCastle = new Symm.CryptBounceCastle(cparams, true);
                decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
            }

            return DeEnCoder.GetBytesTrimNulls(decryptBytes);
        }




        public static char GetCipherChar(CipherEnum cipher)
        {
            switch (cipher)
            {
                case CipherEnum.Aes: return 'A';

                case CipherEnum.BlowFish: return 'b';
                case CipherEnum.Camellia: return 'M';
                case CipherEnum.Cast6: return 'C';
                case CipherEnum.Des3: return 'D';
                case CipherEnum.Fish2: return 'f';
                case CipherEnum.Fish3: return 'F';
                case CipherEnum.Gost28147: return 'g';

                case CipherEnum.Idea: return 'I';
                case CipherEnum.RC532: return '5';
                case CipherEnum.Seed: return 's';
                case CipherEnum.Serpent: return 'S';
                case CipherEnum.SkipJack: return 'J';
                case CipherEnum.Tea: return 't';
                case CipherEnum.XTea: return 'X';

                case CipherEnum.ZenMatrix: return 'z';

                case CipherEnum.Cast5: return 'c';
                case CipherEnum.Noekeon: return 'N';
                case CipherEnum.RC2: return 'r';
                case CipherEnum.RC6: return 'R';
                case CipherEnum.Tnepres: return 'T';

                case CipherEnum.Rsa: return 'Z';
                default: break;
            }

            return 'A';
        }

        public static string SymmCipherPipeString(CipherEnum[] ciphers)
        {
            string hashSymms = string.Empty;

            foreach (CipherEnum cipher in ciphers)
            {
                hashSymms += GetCipherChar(cipher);
            }

            return hashSymms;
        }
    }

}
