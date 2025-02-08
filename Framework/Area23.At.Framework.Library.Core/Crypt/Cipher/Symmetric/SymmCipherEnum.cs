using System.ComponentModel;

namespace Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric
{


    /// <summary>
    /// SymmCipherEnum maps prefered provided symmetric <see cref="Org.BouncyCastle.Crypto.IBlockCipher"/> algorthims
    /// Default algorithm is <see cref="SymmCipherEnum.Aes"/ ><seealso cref="Org.BouncyCastle.Crypto.Engines.AesEngine" />
    /// </summary>
    [DefaultValue("Aes")]
    public enum SymmCipherEnum : byte
    {
        Aes = 0x0,

        BlowFish = 0x1,
        Camellia = 0x2,
        Cast6 = 0x3,
        Des3 = 0x4,
        Fish2 = 0x5,
        Fish3 = 0x6,
        Gost28147 = 0x7,
        Idea = 0x8,

        RC532 = 0x9, // Rijndael = 0x9,
        Seed = 0xa,
        SkipJack = 0xb,
        Serpent = 0xc,
        Tea = 0xd,
        XTea = 0xe,

        ZenMatrix = 0xf

    }

    /// <summary>
    /// SymmCipherEnumExtensions provides extension methods for <see cref="SymmCipherEnum"/>
    /// </summary>
    public static class SymmCipherEnumExtensions
    {

        /// <summary>
        /// Extensions method for Enum <see cref="SymmCipherEnum"/>
        /// gets a character for each Cipher Algorithm, that is used here
        /// </summary>
        /// <param name="symmCipher">this <see cref="SymmCipherEnum"/></param>
        /// <returns>a <see cref="char"/>, that is a short name for the encryption</returns>
        public static char GetSymmCipherChar(this SymmCipherEnum symmCipher)
        {
            switch (symmCipher)
            {
                case SymmCipherEnum.Aes: return 'A';

                case SymmCipherEnum.BlowFish: return 'b';
                case SymmCipherEnum.Camellia: return 'C';
                case SymmCipherEnum.Cast6: return '6';
                case SymmCipherEnum.Des3: return 'D';
                case SymmCipherEnum.Fish2: return 'f';
                case SymmCipherEnum.Fish3: return 'F';
                case SymmCipherEnum.Gost28147: return 'g';

                case SymmCipherEnum.Idea: return 'I';
                case SymmCipherEnum.RC532: return '5';
                // case SymmCipherEnum.Rijndael: return 'a';
                case SymmCipherEnum.Seed: return 's';
                case SymmCipherEnum.Serpent: return 'S';
                case SymmCipherEnum.SkipJack: return 'J';
                case SymmCipherEnum.Tea: return 't';
                case SymmCipherEnum.XTea: return 'X';

                case SymmCipherEnum.ZenMatrix: return 'z';
            }

            return ((char)('A'));
        }

    }

}
