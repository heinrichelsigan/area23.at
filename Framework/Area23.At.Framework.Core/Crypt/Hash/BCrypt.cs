using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;

namespace Area23.At.Framework.Core.Crypt.Hash
{

    /// <summary>
    /// BCrypt a classic unix passwd crypt method
    /// 
    /// All members are implemented via <see cref="Org.BouncyCastle.Crypto"/ namespace.
    /// Thanx to the legion of <see href="https://bouncycastle.org/"" />
    /// <see cref="Org.BouncyCastle.Crypto.Generators.BCrypt"/>
    /// </summary>
    public static class BCrypt
    {
        const int PASSWD_BYTE_LEN = 64;
        const int SALT_BYTE_LEN = 16;
        const int AVG_COST = 4;

        /// <summary>
        /// <see cref="Org.BouncyCastle.Crypto.Generators.BCrypt"/>
        /// Thanx to the legion of <see href="https://bouncycastle.org/"" />
        /// </summary>
        /// <param name="keyBytes">keyBytes to hash encrypt</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] BCryptHash(byte[] keyBytes)
        {
            if (keyBytes == null || keyBytes.Length == 0)
            {
                string argExcMsg = "BCryptHash(keyBytes) => keyBytes";
                argExcMsg += (keyBytes == null) ? " is null." : string.Concat(".Length = ", keyBytes.Length, ".");
                throw new ArgumentException(argExcMsg, "keyBytes");
            }

            if (keyBytes.Length > PASSWD_BYTE_LEN)
                throw new ArgumentException($"BCryptHash(keyBytes) => {Hex16.ToHex16(keyBytes)} Length {keyBytes.LongLength} > {PASSWD_BYTE_LEN} bytes", "keyBytes");

            byte[] salt = EnDeCodeHelper.KeyBytesToHexBytesSalt(keyBytes, SALT_BYTE_LEN);

            byte[] bcrypted = Org.BouncyCastle.Crypto.Generators.BCrypt.Generate(keyBytes, salt, AVG_COST);

            return bcrypted;
        }

        public static byte[] BCryptHash(string passwd)
        {
            if (string.IsNullOrEmpty(passwd))
                throw new ArgumentNullException("passwd string is null or string.Empty.", "passwd");
            
            byte[] keyBytes = EnDeCodeHelper.GetBytes(passwd);
            
            return BCryptHash(keyBytes);
        }

        public static string Hash(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;
            
            if (System.IO.File.Exists(filePath))
                return Hash(File.ReadAllBytes(filePath));
                
            return HashString(filePath);            
        }


        public static string HashString(string string2Hash) => BCryptHash(string2Hash).ToHexString(false);
            

        public static string Hash(byte[] bytes) => BCryptHash(bytes).ToHexString(false);


        public static byte[] HashBytes(byte[] bytes) => BCryptHash(bytes);
    


    }

}
