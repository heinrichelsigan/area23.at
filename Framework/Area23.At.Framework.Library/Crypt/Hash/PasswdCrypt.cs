using Area23.At.Framework.Library.Crypt.EnDeCoding;
using System;

namespace Area23.At.Framework.Library.Crypt.Hash
{

    /// <summary>
    /// Classical unix passwd crypt methods or 
    /// other one directional password hashing functions
    /// 
    /// All members are implemented via <see cref="Org.BouncyCastle.Crypto"/ namespace.
    /// Thanx to the legion of <see href="https://bouncycastle.org/"" />
    /// <see cref="Org.BouncyCastle.Crypto.Generators.BCrypt"/>
    /// <see cref="Org.BouncyCastle.Crypto.Generators.SCrypt"/>
    /// <see cref="Org.BouncyCastle.Crypto.Generators.OpenBsdBCrypt" />    
    /// </summary>
    public static class PasswdCrypt
    {
        const int PASSWD_BYTE_LEN = 64;
        const int SALT_BYTE_LEN = 16;
        const int AVG_COST = 4;

        /// <summary>
        /// <see cref="Org.BouncyCastle.Crypto.Generators.BCrypt"/>
        /// Thanx to the legion of <see href="https://bouncycastle.org/"" />
        /// </summary>
        /// <param name="passwd">passwd or key to encrypt</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] BCrypt(string passwd)
        {
            if (string.IsNullOrEmpty(passwd))
                throw new ArgumentNullException("passwd");

            byte[] keyBytes = EnDeCodeHelper.GetBytes(passwd);

            if (keyBytes.Length > PASSWD_BYTE_LEN)
                throw new ArgumentException($"BCrypt(passwd) => GetBytes(passwd) => {Hex16.ToHex16(keyBytes)} Length {keyBytes.LongLength} > {PASSWD_BYTE_LEN} bytes", "passwd");

            byte[] salt = EnDeCodeHelper.KeyToHexBytesSalt(passwd, SALT_BYTE_LEN);

            byte[] bcrypted = Org.BouncyCastle.Crypto.Generators.BCrypt.Generate(keyBytes, salt, AVG_COST);

            return bcrypted;
        }

        public static byte[] SCrypt(string passwd)
        {
            if (string.IsNullOrEmpty(passwd))
                throw new ArgumentNullException("passwd");

            byte[] keyBytes = EnDeCodeHelper.GetBytes(passwd);

            if (keyBytes.Length > PASSWD_BYTE_LEN)
                throw new ArgumentException($"SCrypt(passwd) => GetBytes(passwd) => {Hex16.ToHex16(keyBytes)} Length {keyBytes.LongLength} > {PASSWD_BYTE_LEN} bytes", "passwd");

            byte[] salt = EnDeCodeHelper.KeyToHexBytesSalt(passwd, SALT_BYTE_LEN);

            byte[] scrypted = Org.BouncyCastle.Crypto.Generators.SCrypt.Generate(keyBytes, salt, AVG_COST, SALT_BYTE_LEN, 1, 32);

            return scrypted;
        }

        public static string BSDCrypt(string passwd)
        {
            if (string.IsNullOrEmpty(passwd))
                throw new ArgumentNullException("passwd");

            char[] passChars = passwd.ToCharArray();
            byte[] keyBytes = EnDeCodeHelper.GetBytes(passwd);

            if (keyBytes.Length > PASSWD_BYTE_LEN)
                throw new ArgumentException($"BSDCrypt(passwd) => GetBytes(passwd) => {Hex16.ToHex16(keyBytes)} Length {keyBytes.LongLength} > {PASSWD_BYTE_LEN} bytes", "passwd");

            byte[] salt = EnDeCodeHelper.KeyToHexBytesSalt(passwd, SALT_BYTE_LEN);

            string bcdCrypted = Org.BouncyCastle.Crypto.Generators.OpenBsdBCrypt.Generate(passChars, salt, AVG_COST);

            return bcdCrypted;
        }

    }

}
