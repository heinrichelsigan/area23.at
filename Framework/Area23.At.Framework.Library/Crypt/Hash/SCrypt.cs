using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;

namespace Area23.At.Framework.Library.Crypt.Hash
{

    /// <summary>
    /// SCrypt a classic unix passwd crypt method
    /// 
    /// All members are implemented via <see cref="Org.BouncyCastle.Crypto"/ namespace.
    /// Thanx to the legion of <see href="https://bouncycastle.org/"" />
    /// <see cref="Org.BouncyCastle.Crypto.Generators.SCrypt"/>
    /// </summary>
    public static class SCrypt
    {
        const int PASSWD_BYTE_LEN = 64;
        const int SALT_BYTE_LEN = 16;
        const int AVG_COST = 4;

        /// <summary>
        /// <see cref="Org.BouncyCastle.Crypto.Generators.SCrypt"/>
        /// Thanx to the legion of <see href="https://bouncycastle.org/"" />
        /// </summary>
        /// <param name="keyBytes">keyBytes to hash encrypt</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] SCryptHash(byte[] keyBytes)
        {
            if (keyBytes == null || keyBytes.Length == 0)
            {
                string argExcMsg = "SCryptHash(keyBytes) => keyBytes";
                argExcMsg += (keyBytes == null) ? " is null." : string.Concat(".Length = ", keyBytes.Length, ".");
                throw new ArgumentException(argExcMsg, "keyBytes"); 
            }

            if (keyBytes.Length > PASSWD_BYTE_LEN)
                throw new ArgumentException($"SCryptHash(keyBytes) => {Hex16.ToHex16(keyBytes)} Length {keyBytes.LongLength} > {PASSWD_BYTE_LEN} bytes", "keyBytes");

            byte[] salt = EnDeCodeHelper.KeyBytesToHexBytesSalt(keyBytes, SALT_BYTE_LEN);

            byte[] scrypted = Org.BouncyCastle.Crypto.Generators.SCrypt.Generate(keyBytes, salt, AVG_COST, SALT_BYTE_LEN, 1, 32);

            return scrypted;
        }


        public static byte[] SCryptHash(string passwd)
        {
            if (string.IsNullOrEmpty(passwd))
                throw new ArgumentNullException("passwd string is null or string.Empty.", "passwd");

            byte[] keyBytes = EnDeCodeHelper.GetBytes(passwd);

            return SCryptHash(keyBytes);
        }

        public static string Hash(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;
            
            if (System.IO.File.Exists(filePath))
                return Hash(System.IO.File.ReadAllBytes(filePath));
                
            return HashString(filePath);            
        }


        public static string HashString(string string2Hash) => SCryptHash(string2Hash).ToHexString(false);



        public static string Hash(byte[] bytes) => SCryptHash(bytes).ToHexString(false);


        public static byte[] HashBytes(byte[] bytes) => SCryptHash(bytes);
    


    }

}
