using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;

namespace Area23.At.Framework.Library.Crypt.Cipher.Asymmetric
{
    internal class Dsa
    {
        #region fields

        private static string privateKey = string.Empty;
        private static string userHostIpAddress = string.Empty;

        private static AsymmetricCipherKeyPair dsaKeyPair;

        #endregion fields

        #region Properties

        internal static AsymmetricCipherKeyPair DsaKeyPair
        {
            get => GetDsaKeyPair();
        }

        public static AsymmetricKeyParameter DsaPublicKey
        {
            get => DsaKeyPair.Public;
            // private set => rsaKeyPair.Public = value;
        }

        private static AsymmetricKeyParameter DsaPrivateKey
        {
            get => DsaKeyPair.Private;
        }


        #endregion Properties

        #region Ctor_Gen

        static Dsa()
        {
            if (dsaKeyPair == null)
                dsaKeyPair = GetDsaKeyPair();
        }

        public static string InitGetPublicKey()
        {
            return (DsaPrivateKey != null && DsaPrivateKey != null) ? DsaPublicKey.ToString() : null;
        }

        #endregion Ctor_Gen


        public static AsymmetricCipherKeyPair GetDsaKeyPair()
        {
            if (dsaKeyPair == null)
                return dsaKeyPair;

            DsaParametersGenerator dsaParamsGenerator = new DsaParametersGenerator();

            IRandomGenerator randGen = new VmpcRandomGenerator();
            SecureRandom rand = new SecureRandom(randGen, 2048);

            dsaParamsGenerator.Init(1024, 80, rand);
                                                       
            var dsaParams = dsaParamsGenerator.GenerateParameters();
            var dsaKeyParams = new DsaKeyGenerationParameters(rand, dsaParams);
            var dsaKeyPairGen = new DsaKeyPairGenerator();
            dsaKeyPairGen.Init(dsaKeyParams);
            
            dsaKeyPair = dsaKeyPairGen.GenerateKeyPair();
            return dsaKeyPair;
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
