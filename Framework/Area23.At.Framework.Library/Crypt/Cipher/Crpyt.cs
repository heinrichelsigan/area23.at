using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Org.BouncyCastle.Crypto;
using Symm = Area23.At.Framework.Library.Crypt.Cipher.Symmetric;

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
        public static byte[] EncryptBytes(byte[] inBytes, CipherEnum cipherAlgo = CipherEnum.Aes, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            byte[] encryptBytes = inBytes;

            string algo = cipherAlgo.ToString();

            switch (cipherAlgo)
            {
                case CipherEnum.RC564:
                    RC564.RC564GenWithKey(secretKey, keyIv, true);
                    encryptBytes = RC564.Encrypt(inBytes);
                    break;                    
                case CipherEnum.Rsa:
                    var keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                    string privKey = keyPair.Private.ToString();
                    encryptBytes = Asymmetric.Rsa.Encrypt(inBytes);
                    break;
                case CipherEnum.Serpent:
                    Serpent.SerpentGenWithKey(secretKey, keyIv, true);
                    encryptBytes = Serpent.Encrypt(inBytes);
                    break;
                case CipherEnum.ZenMatrix:
                    encryptBytes = (new ZenMatrix(secretKey, keyIv, false)).Encrypt(inBytes);
                    break;
                case CipherEnum.ZenMatrix2:
                    encryptBytes = (new ZenMatrix2(secretKey, keyIv, false)).Encrypt(inBytes);
                    break;
                case CipherEnum.Aes:
                case CipherEnum.Aria:
                case CipherEnum.AesLight:
                case CipherEnum.Rijndael:
                
                case CipherEnum.BlowFish:
                case CipherEnum.Fish2:
                case CipherEnum.Fish3:
                case CipherEnum.ThreeFish256:
                case CipherEnum.Camellia:
                case CipherEnum.CamelliaLight:
                case CipherEnum.Cast5:
                case CipherEnum.Cast6:
                case CipherEnum.Des:
                case CipherEnum.Des3:
                case CipherEnum.Dstu7624:

                case CipherEnum.Gost28147:
                case CipherEnum.Idea:
                case CipherEnum.Noekeon:
                case CipherEnum.RC2:
                case CipherEnum.RC532:
                case CipherEnum.RC6:
                case CipherEnum.Seed:
                case CipherEnum.SM4:
                case CipherEnum.SkipJack:
                case CipherEnum.Tnepres:
                case CipherEnum.Tea:
                case CipherEnum.XTea:
                default:
                    CryptParams cparams = new CryptParams(cipherAlgo, secretKey, keyIv);
                    Symm.CryptBounceCastle cryptBounceCastle = new Symm.CryptBounceCastle(cparams, true);
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
        /// <param name="keyIv">key's iv</param>
        /// <returns>decrypted byte Array</returns>
        public static byte[] DecryptBytes(byte[] cipherBytes, CipherEnum cipherAlgo = CipherEnum.Aes, string secretKey = "postmaster@kernel.org", string keyIv = "")
        {
            bool sameKey = true;
            string algorithmName = cipherAlgo.ToString();
            byte[] decryptBytes = cipherBytes;

            switch (cipherAlgo)
            {
                case CipherEnum.RC564:
                    RC564.RC564GenWithKey(secretKey, keyIv, true);
                    decryptBytes = RC564.Decrypt(cipherBytes);
                    break;                       
                case CipherEnum.Rsa:
                    AsymmetricCipherKeyPair keyPair = Asymmetric.Rsa.RsaGenWithKey(Constants.RSA_PUB, Constants.RSA_PRV);
                    string privKey = keyPair.Private.ToString();
                    decryptBytes = Asymmetric.Rsa.Decrypt(cipherBytes);
                    break;
                case CipherEnum.Serpent:
                    sameKey = Serpent.SerpentGenWithKey(secretKey, keyIv, false);
                    decryptBytes = Serpent.Decrypt(cipherBytes);
                    break;
                case CipherEnum.ZenMatrix:
                    decryptBytes = (new ZenMatrix(secretKey, keyIv, false)).Decrypt(cipherBytes);
                    break;
                case CipherEnum.ZenMatrix2:
                    decryptBytes = (new ZenMatrix2(secretKey, keyIv, false)).Decrypt(cipherBytes);
                    break;
                case CipherEnum.Aes:
                case CipherEnum.Rijndael:
                case CipherEnum.AesLight:
                case CipherEnum.Aria:

                case CipherEnum.BlowFish:
                case CipherEnum.Fish2:
                case CipherEnum.Fish3:
                case CipherEnum.ThreeFish256:

                case CipherEnum.Camellia:
                case CipherEnum.CamelliaLight:
                case CipherEnum.Cast5:
                case CipherEnum.Cast6:
                case CipherEnum.Des:
                case CipherEnum.Des3:
                case CipherEnum.Dstu7624:

                case CipherEnum.Gost28147:
                case CipherEnum.Idea:
                case CipherEnum.Noekeon:
                case CipherEnum.RC2:
                case CipherEnum.RC532:
                case CipherEnum.RC6:
                case CipherEnum.Seed:
                case CipherEnum.SM4:
                case CipherEnum.SkipJack:
                case CipherEnum.Tnepres:
                case CipherEnum.Tea:
                case CipherEnum.XTea:
                default:
                    CryptParams cparams = new CryptParams(cipherAlgo, secretKey, keyIv);
                    Symm.CryptBounceCastle cryptBounceCastle = new Symm.CryptBounceCastle(cparams, true);
                    decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);

                    break;
            }

            return EnDeCodeHelper.GetBytesTrimNulls(decryptBytes);
        }

    }

}
