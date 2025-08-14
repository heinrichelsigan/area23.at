using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
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
        /// GetSymmCipherTypes 
        /// </summary>
        /// <returns>an array of all SymmCipherEnum values</returns>
        public static SymmCipherEnum[] GetSymmCipherTypes()
        {
            List<SymmCipherEnum> list = new List<SymmCipherEnum>();
            foreach (SymmCipherEnum symmCipher in Enum.GetValues(typeof(SymmCipherEnum))) 
                list.Add(symmCipher);
            return list.ToArray();
        }

        /// <summary>
        /// GetCharSymmCipherDict gets <see cref="Dictionary{char, SymmCipherEnum}"/>,
        /// where hexbyte as char is mapped to <see cref="SymmCipherEnum" />
        /// </summary>
        /// <returns><see cref="Dictionary{char, SymmCipherEnum}"/></returns>        
        public static Dictionary<char, SymmCipherEnum> GetCharSymmCipherDict()
        {
            Dictionary<char, SymmCipherEnum> charSymmCipherDict = new Dictionary<char, SymmCipherEnum>();
            foreach (SymmCipherEnum symmCipher in Enum.GetValues(typeof(SymmCipherEnum)))
            {
                char hexChar = $"{((ushort)symmCipher):x1}".ToCharArray()[0];
                charSymmCipherDict.Add(hexChar, symmCipher);
            }
            return charSymmCipherDict;
        }

        /// <summary>
        /// GetByteSymmCipherDict gets <see cref="Dictionary{byte, SymmCipherEnum}"/>,
        /// where hex byte value is mapped to  <see cref="SymmCipherEnum" />
        /// </summary>
        /// <returns><see cref="Dictionary{byte, SymmCipherEnum}"/></returns>
        public static Dictionary<byte, SymmCipherEnum> GetByteSymmCipherDict()
        {
            Dictionary<byte, SymmCipherEnum> byteSymmCipherDict = new Dictionary<byte, SymmCipherEnum>();
            foreach (SymmCipherEnum symmCipher in Enum.GetValues(typeof(SymmCipherEnum)))
            {
                byte hexByte = ((byte)symmCipher);
                byteSymmCipherDict.Add(hexByte, symmCipher);
            }
            return byteSymmCipherDict;
        }


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

        /// <summary>
        /// ExtensionMethod ToCipherEnum converts <see cref="SymmCipherEnum"/> to <see cref="CipherEnum"/>
        /// </summary>
        /// <param name="symmCipher"><see cref="SymmCipherEnum"/> to convert</param>
        /// <returns></returns>
        public static CipherEnum ToCipherEnum(this SymmCipherEnum symmCipher)
        {
            switch (symmCipher)
            {

                case SymmCipherEnum.BlowFish: return CipherEnum.BlowFish;
                case SymmCipherEnum.Fish2: return CipherEnum.Fish2;
                case SymmCipherEnum.Fish3: return CipherEnum.Fish3;

                case SymmCipherEnum.Camellia: return CipherEnum.Camellia;
                case SymmCipherEnum.Cast6: return CipherEnum.Cast6;
                case SymmCipherEnum.Des3: return CipherEnum.Des3;

                case SymmCipherEnum.Gost28147: return CipherEnum.Gost28147;
                case SymmCipherEnum.Idea: return CipherEnum.Idea;
                case SymmCipherEnum.RC532: return CipherEnum.RC532;

                case SymmCipherEnum.Seed: return CipherEnum.Seed;
                case SymmCipherEnum.Serpent: return CipherEnum.Serpent;
                case SymmCipherEnum.SkipJack: return CipherEnum.SkipJack;

                case SymmCipherEnum.Tea: return CipherEnum.Tea;
                case SymmCipherEnum.XTea: return CipherEnum.XTea;
                case SymmCipherEnum.ZenMatrix: return CipherEnum.ZenMatrix;

                case SymmCipherEnum.Aes:
                default: return CipherEnum.Aes;
            }

        }

    }

}