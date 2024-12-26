using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using System.ComponentModel;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    /// <summary>
    /// CipherEnum maps BlockCipher algorithms <see cref="Org.BouncyCastle.Crypto.IBlockCipher"/>
    /// </summary>
    [DefaultValue("Aes")]
    public enum CipherEnum : byte
    {
        Aes =       0x0,

        BlowFish =  0x1,
        Camellia =  0x2,
        Cast6 =     0x3,
        Des3 =      0x4,
        Fish2 =     0x5,
        Fish3 =     0x6,
        Gost28147 = 0x7,

        Idea =      0x8,
        RC532 =     0x9,
        Seed =      0xa,
        SkipJack =  0xb,
        Serpent =   0xc,
        Tea =       0xd,
        XTea =      0xe,

        ZenMatrix = 0xf,


        Cast5 =     0x10,
        Noekeon =   0x11,
        RC2 =       0x12,
        RC564 =     0x13,
        RC6 =       0x14,
        Tnepres =   0x15,
        
        Rsa =       0x40
    }

    /// <summary>
    /// Extensions methods for <see cref="CipherEnum"/>
    /// </summary>
    public static class CipherEnumExtensions
    {

        /// <summary>
        /// Extensions method for Enum <see cref="CipherEnum"/>
        /// gets a character for each Cipher Algorithm, that is used here
        /// </summary>
        /// <param name="cipher">this <see cref="CipherEnum"/> extension</param>
        /// <returns>a <see cref="char"/>, that is a short name for the encryption</returns>
        public static char GetCipherChar(this CipherEnum cipher)
        {
            switch (cipher)
            {
                case CipherEnum.Aes: return 'A';

                case CipherEnum.BlowFish: return 'b';
                case CipherEnum.Camellia: return 'C';
                case CipherEnum.Cast6: return '6';
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
                case CipherEnum.RC2: return '2';
                case CipherEnum.RC564: return 'R';
                case CipherEnum.RC6: return 'r';
                case CipherEnum.Tnepres: return 'T';

                case CipherEnum.Rsa: return 'Z';
                default: break;
            }

            return 'A';
        }

    }


}
