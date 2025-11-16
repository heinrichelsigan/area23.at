using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Org.BouncyCastle.Crypto.Digests;
using System.ComponentModel;
using System.Text;

namespace Area23.At.Framework.Core.Crypt.Hash
{

    /// <summary>
    /// KeyHash 
    /// </summary>
    [Serializable]
    [DefaultValue(KeyHash.Hex)]
    public enum KeyHash : short
    {
        Hex = 0x0,
        OpenBSDCrypt = 0x1,
        BCrypt = 0x2,
        SCrypt = 0x3,
        MD5 = 0x4,
        Sha1 = 0x5,
        Sha256 = 0x6,
        Sha384 = 0x7,
        Sha512 = 0x8,
        Whirlpool = 0x9,
        Ascon256 = 0xa,
        Blake2xs = 0xb,
        CShake = 0xc,
        Dstu7564 = 0xd,
        RipeMD256 = 0xe,
        Xoodyak = 0xf
    }

    public static class KeyHash_Extensions
    {

        private static readonly KeyHash[] keyHashes = { KeyHash.Hex, KeyHash.OpenBSDCrypt, KeyHash.BCrypt, KeyHash.SCrypt,
            KeyHash.MD5, KeyHash.Sha1, KeyHash.Sha256, KeyHash.Sha384, KeyHash.Sha512,
            KeyHash.Whirlpool, KeyHash.Blake2xs, KeyHash.CShake, KeyHash.Dstu7564, KeyHash.RipeMD256, KeyHash.Xoodyak };

        public static KeyHash[] GetHashTypes()
        {
            List<KeyHash> list = new List<KeyHash>();
            foreach (string hashName in Enum.GetNames(typeof(KeyHash)))
            {
                list.Add((KeyHash)Enum.Parse(typeof(KeyHash), hashName));
            }

            return list.ToArray();
        }

        public static KeyHash GetHashType(string typeString)
        {
            return (KeyHash)Enum.Parse(typeof(KeyHash), typeString);
        }

        public static KeyHash GetKeyHashFromValue(short kValue)
        {
            kValue = (short)((kValue % 0x10));
            foreach (KeyHash kHash in GetHashTypes())
            {
                if ((short)kHash == kValue)
                    return kHash;
            }
            return KeyHash.Hex;
        }

        public static KeyHash GetKeyHashFromString(string stringToHash)
        {
            if (!string.IsNullOrEmpty(stringToHash))
            {
                switch (stringToHash.ToLower())
                {
                    case "scrypt": return KeyHash.SCrypt;
                    case "bcrypt": return KeyHash.BCrypt;
                    case "openbsd": 
                    case "bsdcrypt": 
                    case "openbsdcrypt": return KeyHash.OpenBSDCrypt;
                    case "md5": return KeyHash.MD5;
                    case "sha1": return KeyHash.Sha1;
                    case "sha256": return KeyHash.Sha256;
                    case "sha384": return KeyHash.Sha384;
                    case "sha512": return KeyHash.Sha512;
                    case "whirlwind":
                    case "whirlpool": return KeyHash.Whirlpool;
                    case "ascon":
                    case "ascon256":
                    case "asconhash":
                    case "asconhash256": return KeyHash.Ascon256;
                    case "blake2":
                    case "blake2xs": return KeyHash.Blake2xs;
                    case "shake":
                    case "cshake": return KeyHash.CShake;
                    case "dstu7564": return KeyHash.Dstu7564;
                    case "ripe":
                    case "ripe256":
                    case "ripemd256": return KeyHash.RipeMD256;
                    case "zodiak":
                    case "xoodyac":
                    case "xoodyak": return KeyHash.Xoodyak;
                    case "hex16":
                    case "hex": return KeyHash.Hex;
                    default:
                        if (Enum.TryParse<KeyHash>(stringToHash, out KeyHash khash))
                            return khash;
                        break;
                }
            }
            return KeyHash.Hex;
        }

        public static string Hash(this KeyHash hash, string stringToHash)
        {
            switch (hash)
            {
                case KeyHash.SCrypt:
                    return SCrypt.HashString(stringToHash);
                case KeyHash.BCrypt:
                    return BCrypt.HashString(stringToHash);
                case KeyHash.OpenBSDCrypt:
                    return OpenBSDCrypt.HashString(stringToHash);
                case KeyHash.MD5:
                    return MD5Sum.HashString(stringToHash, "");
                case KeyHash.Sha1:
                    return Sha1.HashString(stringToHash);
                case KeyHash.Sha256:
                    return Sha256Sum.HashString(stringToHash, "");
                case KeyHash.Sha384:
                    return Sha384.HashString(stringToHash);
                case KeyHash.Sha512:
                    return Sha512Sum.HashString(stringToHash);
                case KeyHash.Whirlpool: 
                    return Whirlpool.HashString(stringToHash);
                case KeyHash.Ascon256: 
                    return Ascon256.HashString(stringToHash);
                case KeyHash.Blake2xs:
                    return Blake2xs.HashString(stringToHash);
                case KeyHash.CShake:
                    return CShake.HashString(stringToHash);
                case KeyHash.Dstu7564:
                    return Dstu7564.HashString(stringToHash);
                case KeyHash.RipeMD256:
                    return RipeMD256.HashString(stringToHash);
                case KeyHash.Xoodyak:                    
                    return Zodiac.HashString(stringToHash);
                case KeyHash.Hex:
                default:
                    return Hex16.ToHex16(Encoding.UTF8.GetBytes(stringToHash));
            }
        }

    }

}
