using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace Area23.At.Framework.Core.Crypt.Cipher.Asymmetric
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

        internal static AsymmetricCipherKeyPair DHKeyPair
        {
            get => GetDHKeyPair();
        }

        public static AsymmetricKeyParameter DHPublicKey
        {
            get => DHKeyPair.Public;
            // private set => rsaKeyPair.Public = value;
        }

        private static AsymmetricKeyParameter DHPrivateKey
        {
            get => DHKeyPair.Private;
        }


        #endregion Properties

        #region Ctor_Gen

        static DH()
        {
            if (dhKeyPair == null)
                dhKeyPair = GetDHKeyPair();
        }

        public static string InitGetPublicKey()
        {
            return (DHPrivateKey != null && DHPublicKey != null) ? DHPublicKey.ToString() : null;
        }

        #endregion Ctor_Gen

        internal static AsymmetricCipherKeyPair GetDHKeyPair()
        {
            if (dhKeyPair != null)
                return dhKeyPair;

            DHKeyPairGenerator dhKeyPairGen = new DHKeyPairGenerator();
            IRandomGenerator randGen = new VmpcRandomGenerator();
            SecureRandom rand = new SecureRandom(randGen, 1024);
            KeyGenerationParameters dhKeyParams = new KeyGenerationParameters(rand, 1024);
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
