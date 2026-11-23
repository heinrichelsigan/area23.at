using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    /// <summary>
    /// CipherEnum maps BlockCipher algorithms <see cref="Org.BouncyCastle.Crypto.IBlockCipher"/>
    /// </summary>
    [Serializable]
    [DefaultValue("Aes")]
    public enum CipherEnum : byte
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
        RC532 = 0x9,
        Seed = 0xa,
        SkipJack = 0xb,
        Serpent = 0xc,
        Tea = 0xd,
        XTea = 0xe,
        SM4 = 0xf,

        Cast5 = 0x10,
        Rijndael = 0x11,
        Noekeon = 0x12,
        RC2 = 0x13,
        RC564 = 0x14,
        RC6 = 0x15,
        Tnepres = 0x16,
        Des = 0x17,
        Aria = 0x18,
        CamelliaLight = 0x19,
        Dstu7624 = 0x1a,
        AesLight = 0x1b,
        ZenMatrix = 0x1c,

        Salsa20 = 0x1d,
        ChaCha = 0x1e,
        ChaCha7539 = 0x1f,
        RC4 = 0x20,
        Isaac = 0x21,
        HC128 = 0x22,
        HC256 = 0x23,
        Vmpc = 0x24,
        VmpcKsa3 = 0x25,
        XChaCha20 = 0x26,
        XSalsa20 = 0x27,


        Des3Net = 0x30,
        AesNet = 0x31,
        ZenMatrix2 = 0x32,

        Rsa = 0x40
        // Dsa = 0x41,
        // DH = 0x42,
        // Ecdsa = 0x43,
        // Ed25519 = 0x44,
        // Ed448 = 0x45,
        // X25519 = 0x46,
        // X448 = 0x47,
        // GPG = 0x48


        // Ascon = 0x1d

        // None = 0xff

    }


    /// <summary>
    /// Extensions methods for <see cref="CipherEnum"/>
    /// </summary>
    public static class CipherEnumExtensions
    {
        public static readonly string CipherExtensionString = "l23456AabCcDdEeFfgIJjlNRSsTtXrZz$%";

        public static CipherEnum[] GetCipherTypes(bool symmetricOnly = true)
        {
            List<CipherEnum> list = new List<CipherEnum>();
            foreach (string encName in Enum.GetNames(typeof(CipherEnum)))
            {
                CipherEnum cipherAlgo = (CipherEnum)Enum.Parse(typeof(CipherEnum), encName);
                if (symmetricOnly && cipherAlgo != CipherEnum.Rsa)
                    list.Add(cipherAlgo);
            }

            return list.ToArray();
        }

        public static Dictionary<byte, CipherEnum> ByteCipherDict
        {
            get
            {
                Dictionary<byte, CipherEnum> hexCipherDict = new Dictionary<byte, CipherEnum>();
                foreach (CipherEnum cipher in CipherEnumExtensions.GetCipherTypes())
                    hexCipherDict.Add((byte)((int)cipher), cipher);
                return hexCipherDict;

            }

        }

        /// <summary>
        /// Extensions method for Enum <see cref="CipherEnum"/>
        /// gets a character for each Cipher Algorithm, that is used here
        /// "$02456789AaBbCcDdEefFgHhIiJjLlNRrSsTtVvXx"
        /// </summary>
        /// <param name="cipher">this <see cref="CipherEnum"/> extension</param>
        /// <returns>a <see cref="char"/>, that is a short name for the encryption</returns>
        public static char GetCipherChar(this CipherEnum cipher)
        {
            switch (cipher)
            {
                case CipherEnum.Aes: return 'A';
                case CipherEnum.AesLight: return 'L';
                case CipherEnum.AesNet: return 'E';
                case CipherEnum.Aria: return 'a';

                case CipherEnum.BlowFish: return 'b';
                case CipherEnum.Fish2: return 'f';
                case CipherEnum.Fish3: return 'F';
                // case CipherEnum.ThreeFish256: return '3';

                case CipherEnum.Camellia: return 'C';
                case CipherEnum.CamelliaLight: return 'l';
                case CipherEnum.Cast5: return 'c';
                case CipherEnum.Cast6: return '6';

                case CipherEnum.Des: return '$';
                case CipherEnum.Des3: return 'D';
                case CipherEnum.Des3Net: return 'e';
                case CipherEnum.Dstu7624: return 'd';

                case CipherEnum.Gost28147: return 'g';
                case CipherEnum.Idea: return 'I';
                case CipherEnum.Noekeon: return 'N';

                case CipherEnum.RC2: return '2';
                case CipherEnum.RC532: return '5';
                case CipherEnum.RC564: return 'R';
                case CipherEnum.RC6: return 'r';

                case CipherEnum.Seed: return 's';
                case CipherEnum.Serpent: return 'S';
                case CipherEnum.SM4: return '4';
                case CipherEnum.SkipJack: return 'J';

                case CipherEnum.Tea: return 't';
                case CipherEnum.Tnepres: return 'T';
                case CipherEnum.Rijndael: return 'j';
                case CipherEnum.XTea: return 'X';

                case CipherEnum.ZenMatrix: return 'z';
                case CipherEnum.ZenMatrix2: return 'Z';

                case CipherEnum.ChaCha: return '0';
                case CipherEnum.ChaCha7539: return '7';
                case CipherEnum.Salsa20: return '8';
                case CipherEnum.XChaCha20: return '9';
                case CipherEnum.XSalsa20: return 'x';
                case CipherEnum.HC128: return 'h';
                case CipherEnum.HC256: return 'H';
                case CipherEnum.Isaac: return 'i';
                case CipherEnum.RC4: return 'B';
                case CipherEnum.Vmpc: return 'v';
                case CipherEnum.VmpcKsa3: return 'V';

                case CipherEnum.Rsa: return '%';
                // case CipherEnum.DH: return '!';

                default: break;
            }

            return 'A';
        }

        public static string PrintCipherType(this CipherEnum cipher)
        {
            string s = cipher.ToString("x:2") + " " + cipher.GetCipherChar() + "\t" + cipher.ToString();
            return s;
        }

        /// <summary>
        /// parses pipe semicolon separated pipe string to CipherList
        /// </summary>
        /// <param name="pipeText">semicolon separated pipe string to CipherList </param>
        /// <returns><see cref="T:CipherEnum[]"/> array of ciphers for the pipe</returns>
        public static CipherEnum[] ParsePipeText(string pipeText)
        {
            CipherEnum cipher = CipherEnum.Aes;
            List<CipherEnum> cipherList = new List<CipherEnum>();
            pipeText = pipeText ?? "";

            int pipeCnt = 0;
            string[] algos = pipeText.Split(Constants.COOL_CRYPT_SPLIT.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string algo in algos)
            {
                if (Enum.TryParse<CipherEnum>(algo, out cipher))
                {
                    cipherList.Add(cipher);
                    if ((++pipeCnt) >= Constants.PIPE_MAX_LEN)
                        break;
                }

            }

            return cipherList.ToArray();
        }



        public static CipherEnum[] FromString(string pipeText)
        {
            CipherPipe cp = new CipherPipe(pipeText);
            return cp.InPipe;
        }


    }

}
