using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;

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

        public static AsymmetricCipherKeyPair DsaKeyPair => GetDsaKeyPair();        

        public static DsaKeyParameters DsaPublicKey => (DsaKeyParameters)DsaKeyPair.Public;
        
        private static DsaPrivateKeyParameters DsaPrivateKey => (DsaPrivateKeyParameters)dsaKeyPair.Private;
       

        public static string PrivateKey => DsaPrivateKey.ToString();

        #endregion Properties

        #region Ctor_Gen

        static Dsa()
        {
            if (dsaKeyPair == null)
                dsaKeyPair = GetDsaKeyPair(1024);
        }


        #endregion Ctor_Gen


        public static AsymmetricCipherKeyPair GetDsaKeyPair(int size = 1024)
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

        public static byte[] DsaSign(byte[] msgBytes)
        {
            ISigner signer = SignerUtilities.GetSigner("SHA256withDSA");
            signer.Init(true, DsaPrivateKey);
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
            byte[] signatureBytes = signer.GenerateSignature();

            return signatureBytes;
        }


        public static bool DsaVerify(byte[] msgBytes, byte[] signatureBytes)
        {
            var signer = SignerUtilities.GetSigner("SHA256withDSA");
            signer.Init(false, DsaPublicKey);
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
            return signer.VerifySignature(signatureBytes);
        }
    }

}
