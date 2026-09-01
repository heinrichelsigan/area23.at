using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Text;

namespace Area23.At.Framework.Library.Crypt.Cipher.Asymmetric
{
    /// <summary>
    /// DSA Asymmetric cipher
    /// </summary>
    public static class Dsa
    {

        #region fields        

        private static AsymmetricCipherKeyPair dsaKeyPair;

        #endregion fields

        #region Properties

        public static AsymmetricCipherKeyPair DsaKeyPair
        {
            get
            {
                if (dsaKeyPair == null)
                    dsaKeyPair = GenerateDsaKeyPair();
                return dsaKeyPair;
            }
        }

        public static DsaKeyParameters DsaPublicKey => (DsaKeyParameters)DsaKeyPair.Public;
        
        private static DsaPrivateKeyParameters DsaPrivateKey => (DsaPrivateKeyParameters)dsaKeyPair.Private;
       

        public static string PrivateKey => DsaPrivateKey.ToString();

        #endregion Properties

        #region Ctor_Gen

        /// <summary>
        /// static constructor to initialize the DSA key pair.
        /// </summary>
        static Dsa()
        {
            if (dsaKeyPair == null)
                dsaKeyPair = GenerateDsaKeyPair(1024);
        }

        /// <summary>
        /// Gets a DSA key pair from the provided private and public keys in PEM format.
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="publicKey"></param>
        /// <returns>The specific DSA key pair.</returns>
        public static AsymmetricCipherKeyPair GetDsaKeyPairByKeys(string privateKey, string publicKey)
        {
            dsaKeyPair = new AsymmetricCipherKeyPair(
                (DsaKeyParameters)new PemReader(new StringReader(publicKey)).ReadObject(),
                (DsaPrivateKeyParameters)new PemReader(new StringReader(privateKey)).ReadObject()
            );
            return dsaKeyPair;
        }

        /// <summary>
        /// Generates a new DSA key pair with the specified size (default is 1024 bits).
        /// </summary>
        /// <param name="size">The size of the key in bits.</param>
        /// <returns>The generated DSA key pair.</returns>
        public static AsymmetricCipherKeyPair GenerateDsaKeyPair(int size = 1024)
        {
            // if (dsaKeyPair == null)
            //     return dsaKeyPair;

            DsaParametersGenerator dsaParamsGenerator = new DsaParametersGenerator();

            IRandomGenerator randGen = new VmpcRandomGenerator();
            SecureRandom rand = new SecureRandom(randGen, size);

            dsaParamsGenerator.Init(size, 80, rand);
                                                       
            var dsaParams = dsaParamsGenerator.GenerateParameters();
            var dsaKeyParams = new DsaKeyGenerationParameters(rand, dsaParams);
            var dsaKeyPairGen = new DsaKeyPairGenerator();
            dsaKeyPairGen.Init(dsaKeyParams);
            
            dsaKeyPair = dsaKeyPairGen.GenerateKeyPair();
            return dsaKeyPair;
        }


        #endregion Ctor_Gen


        /// <summary>
        /// Gets the private and public keys from the provided DSA key pair as a tuple of strings in PEM format.
        /// </summary>
        /// <param name="dsaKeyPair">The DSA key pair.</param>
        /// <returns>A tuple containing the private and public keys in PEM format.</returns>
        public static Tuple<string, string> GetKeysTuple(AsymmetricCipherKeyPair dsaKeyPair)
        {            
            string privKey = string.Empty, pubKey = string.Empty;
            using (TextWriter textWriter1 = new StringWriter())
            {
                var pemWriter1 = new PemWriter(textWriter1);
                pemWriter1.WriteObject(dsaKeyPair.Private);
                pemWriter1.Writer.Flush();

                privKey = textWriter1.ToString();
                Console.WriteLine(privKey);
            }

            using (TextWriter textWriter2 = new StringWriter())
            {
                var pemWriter2 = new PemWriter(textWriter2);
                pemWriter2.WriteObject(dsaKeyPair.Public);
                pemWriter2.Writer.Flush();
                pubKey = textWriter2.ToString();
                Console.WriteLine(pubKey);
            }

            Tuple<string, string> keyPairTuple = new Tuple<string, string>(privKey, pubKey);
            return keyPairTuple;
        }

        #region Sign_Verify

        public static byte[] DsaSign(byte[] msgBytes)
        {
            ISigner signer = SignerUtilities.GetSigner("SHA256withDSA");
            signer.Init(true, DsaPrivateKey);
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
            byte[] signatureBytes = signer.GenerateSignature();

            return signatureBytes;
        }

        public static byte[] DsaSign(string msg) => DsaSign(Encoding.UTF8.GetBytes(msg));
        

        public static bool DsaVerify(byte[] msgBytes, byte[] signatureBytes)
        {
            var signer = SignerUtilities.GetSigner("SHA256withDSA");
            signer.Init(false, DsaPublicKey);
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
            return signer.VerifySignature(signatureBytes);
        }

        public static bool DsaVerify(string msg, byte[] signatureBytes) => DsaVerify(Encoding.UTF8.GetBytes(msg), signatureBytes);

        #endregion Sign_Verify

    }

}
