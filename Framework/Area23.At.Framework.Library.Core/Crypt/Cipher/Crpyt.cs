using Symm = Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using System;
using Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
using System.Linq;
using Area23.At.Framework.Library.Core.Crypt.EnDeCoding;

namespace Area23.At.Framework.Library.Core.Crypt.Cipher
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
            }
            if (cipherAlgo == CipherEnum.Aes || algo == "Aes")
            {
                Aes.AesGenWithNewKey(secretKey, keyIv, true);
                encryptBytes = Aes.Encrypt(inBytes);
            }            
            if (cipherAlgo == CipherEnum.Rsa || algo == "Rsa")
            {
                var keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                string privKey = keyPair.Private.ToString();
                encryptBytes = Asymmetric.Rsa.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.Serpent || algo == "Serpent")
            {
                Serpent.SerpentGenWithKey(secretKey, keyIv, true);
                encryptBytes = Serpent.Encrypt(inBytes);
            }
            if (cipherAlgo == CipherEnum.ZenMatrix || algo == "ZenMatrix")
            {
                ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, true);
                encryptBytes = ZenMatrix.Encrypt(inBytes);
            }
            if (algo == "BlowFish" || algo == "2Fish" || algo == "Fish2" || algo == "3Fish" || algo == "Fish3" ||
                algo == "Camellia" || algo == "Cast5" || algo == "Cast6" ||
                algo == "Gost28147" || algo == "Idea" || algo == "Noekeon" ||
                algo == "RC2" || algo == "RC532" || algo == "RC6" ||
                // algo == "RC564" || algo == "Rijndael" ||  algo == "Serpent" ||
                algo == "Seed" || algo == "SkipJack" ||
                algo == "Tea" || algo == "Tnepres" || algo == "XTea" ||

                cipherAlgo == CipherEnum.BlowFish || cipherAlgo == CipherEnum.Fish2 || cipherAlgo == CipherEnum.Fish3 ||
                cipherAlgo == CipherEnum.Camellia || cipherAlgo == CipherEnum.Cast5 || cipherAlgo == CipherEnum.Cast6 ||
                cipherAlgo == CipherEnum.Gost28147 || cipherAlgo == CipherEnum.Idea || cipherAlgo == CipherEnum.Noekeon ||
                cipherAlgo == CipherEnum.RC2 || cipherAlgo == CipherEnum.RC532 || cipherAlgo == CipherEnum.RC6 || cipherAlgo == CipherEnum.Rijndael ||
                cipherAlgo == CipherEnum.Seed || cipherAlgo == CipherEnum.SkipJack ||
                cipherAlgo == CipherEnum.Tea || cipherAlgo == CipherEnum.Tnepres || cipherAlgo == CipherEnum.XTea)
            {
                CryptParams cparams = CryptHelper.GetCryptParams(cipherAlgo);
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
            }
            if (cipherAlgo == CipherEnum.Aes || algorithmName == "Aes")
            {
                sameKey = Aes.AesGenWithNewKey(secretKey, keyIv, true);
                decryptBytes = Aes.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Rsa || algorithmName == "Rsa")
            {
                AsymmetricCipherKeyPair keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                string privKey = keyPair.Private.ToString();
                decryptBytes = Asymmetric.Rsa.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.Serpent || algorithmName == "Serpent")
            {
                sameKey = Serpent.SerpentGenWithKey(secretKey, keyIv, false);
                decryptBytes = Serpent.Decrypt(cipherBytes);
            }
            if (cipherAlgo == CipherEnum.ZenMatrix || algorithmName == "ZenMatrix")
            {
                sameKey = ZenMatrix.ZenMatrixGenWithKey(secretKey, keyIv, false);
                decryptBytes = ZenMatrix.Decrypt(cipherBytes);
            }
            if (algorithmName == "BlowFish" || algorithmName == "2Fish" || algorithmName == "Fish2" || algorithmName == "3Fish" || algorithmName == "Fish3" ||
                algorithmName == "Camellia" || algorithmName == "Cast5" || algorithmName == "Cast6" ||
                algorithmName == "Gost28147" || algorithmName == "Idea" || algorithmName == "Noekeon" ||
                algorithmName == "Rijndael" || algorithmName == "RC2" || algorithmName == "RC532" || algorithmName == "RC6" ||
                // || algorithmName == "RC564" || algorithmName == "Rijndael" || algorithmName == "Serpent" || 
                algorithmName == "Seed" || algorithmName == "SkipJack" ||
                algorithmName == "Tea" || algorithmName == "Tnepres" || algorithmName == "XTea" ||

                cipherAlgo == CipherEnum.BlowFish || cipherAlgo == CipherEnum.Fish2 || cipherAlgo == CipherEnum.Fish3 ||
                cipherAlgo == CipherEnum.Camellia || cipherAlgo == CipherEnum.Cast5 || cipherAlgo == CipherEnum.Cast6 ||
                cipherAlgo == CipherEnum.Gost28147 || cipherAlgo == CipherEnum.Idea || cipherAlgo == CipherEnum.Noekeon ||
                cipherAlgo == CipherEnum.RC2 || cipherAlgo == CipherEnum.RC532 || cipherAlgo == CipherEnum.RC6 ||
                cipherAlgo == CipherEnum.Rijndael || cipherAlgo == CipherEnum.Seed || cipherAlgo == CipherEnum.SkipJack ||
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

    }

}
