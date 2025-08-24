using Area23.At.Framework.Core.Crypt.EnDeCoding;
using System.ComponentModel;
using System.Text;

namespace Area23.At.Framework.Core.Crypt.Hash
{

    /// <summary>
    /// KeyHash 
    /// </summary>
    [DefaultValue(KeyHash.Hex)]
    public enum KeyHash
    {
        Hex = 0x16,
        OpenBSDCrypt = 0xb5dc,
        BCrypt = 0xbc,
        SCrypt = 0x5c,
        MD5 = 0x1d5,
        Sha1 = 0x5a1,
        Sha256 = 0x5a256,
        Sha384 = 0x5a384,
        Sha512 = 0x5a512
    }

    public static class KeyHash_Extensions
    {
        public static string Hash(this KeyHash hash, string stringToHash)
        {
            switch (hash)
            {
                case KeyHash.SCrypt:
                    return Hex16.ToHex16(PasswdCrypt.SCrypt(stringToHash));
                case KeyHash.BCrypt:
                    return Hex16.ToHex16(PasswdCrypt.BCrypt(stringToHash));
                case KeyHash.OpenBSDCrypt:
                    return PasswdCrypt.BSDCrypt(stringToHash);
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
                case KeyHash.Hex:
                default:
                    return Hex16.ToHex16(Encoding.UTF8.GetBytes(stringToHash));
            }
        }

    }

}
