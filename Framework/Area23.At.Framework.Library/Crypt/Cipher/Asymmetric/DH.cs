using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;

namespace Area23.At.Framework.Library.Crypt.Cipher.Asymmetric
{

    /// <summary>
    /// Diffie Hellman key exchange
    /// </summary>
    public static class DH
    {
        #region fields

        private static string privateKey = string.Empty;
        private static string userHostIpAddress = string.Empty;

        private static AsymmetricCipherKeyPair dhKeyPair;

        #endregion fields

        #region Properties

        public static AsymmetricCipherKeyPair DHKeyPair => GetDHKeyPair();
       

        public static AsymmetricKeyParameter DHPublicKey => DHKeyPair.Public;

        private static AsymmetricKeyParameter DHPrivateKey => DHKeyPair.Private;


        #endregion Properties

        #region Ctor_Gen

        static DH()
        {
            if (dhKeyPair == null)
                dhKeyPair = GetDHKeyPair(1024);
        }

        public static string InitGetPublicKey()
        {
            return (DHPrivateKey != null && DHPublicKey != null) ? DHPublicKey.ToString() : null;
        }

        #endregion Ctor_Gen

        public static AsymmetricCipherKeyPair GetDHKeyPair(int size = 1024)
        {
            //if (dhKeyPair != null)
            //    return dhKeyPair;

            DHKeyPairGenerator dhKeyPairGen = new DHKeyPairGenerator();
            IRandomGenerator randGen = new VmpcRandomGenerator();
            SecureRandom rand = new SecureRandom(randGen, size);
            KeyGenerationParameters dhKeyParams = new KeyGenerationParameters(rand, size);
            dhKeyPairGen.Init(dhKeyParams);

            dhKeyPair = dhKeyPairGen.GenerateKeyPair();
            return dhKeyPair;
        }


        public static void AliceBobAgreement(int size = 256, int randSeed = 1024)
        {
            var aliceKey = GeneratorUtilities.GetKeyPairGenerator("DH");
            IRandomGenerator randGen = new VmpcRandomGenerator();
            SecureRandom secRand = new SecureRandom(randGen, randSeed);

            DHParametersGenerator aliceGenerator = new DHParametersGenerator();
            aliceGenerator.Init(size, 100, secRand);
            DHParameters aliceParameters = aliceGenerator.GenerateParameters();

            var aliceKGP = new DHKeyGenerationParameters(new SecureRandom(randGen, randSeed), aliceParameters);
            aliceKey.Init(aliceKGP);

            var aliceKeyPair = aliceKey.GenerateKeyPair();
            var aliceKeyAgree = AgreementUtilities.GetBasicAgreement("DH");
            aliceKeyAgree.Init(aliceKeyPair.Private);


            var bobKey = GeneratorUtilities.GetKeyPairGenerator("DH");
            DHParametersGenerator bobGenerator = new DHParametersGenerator();
            bobGenerator.Init(size, 100, new SecureRandom(randGen, randSeed));
            DHParameters bobParameters = aliceGenerator.GenerateParameters();

            var bobKGP = new DHKeyGenerationParameters(new SecureRandom(randGen, randSeed), aliceParameters);
            aliceKey.Init(bobKGP);

            var bobKeyPair = aliceKey.GenerateKeyPair();
            var bobKeyAgree = AgreementUtilities.GetBasicAgreement("DH");
            bobKeyAgree.Init(bobKeyPair.Private);


            var aliceAgree = aliceKeyAgree.CalculateAgreement(bobKeyPair.Public);
            var bobAgree = bobKeyAgree.CalculateAgreement(aliceKeyPair.Public);


        }

    }


}
