using Area23.At.Framework.Core.Crypt.EnDeCoding;
using System.Security.Cryptography;
using System.Text;

namespace Area23.At.Framework.Core.Crypt.Hash
{


    /// <summary>
    /// Sha256Sum creates Sha1Sum of a file or stream or byte[] or string
    /// </summary>
    public static class Sha1
    {

        /// <summary>
        /// Hashes a file
        /// </summary>
        /// <param name="filePath">full(unc) path to file</param>
        /// <param name="fileName">optional filename to add after hash</param>
        /// <returns>Sha512 hash of file with optional fileName at end</returns>
        /// <exception cref="ArgumentNullException">thrown, when filePath == null | filePath == "" | !File.Exists(filePath)</exception>        
        public static string Hash(string filePath, bool showFileName = true)
        {
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                throw new ArgumentNullException($"Sha256Sum.Hash(filePath, showFileName = {showFileName}) filePath is null or empty or file at filePath doesn't exist.");

            byte[] fileBytes = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            return Hash(fileBytes) + ((showFileName) ? $"  {fileName}" : "");
        }


        public static string HashString(string string2Hash)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(string2Hash);
            string hashed = Hash(bytes);
            return hashed;
        }


        public static string Hash(byte[] bytes, string fileName = "")
        {
            byte[] hashed = HashBytes(bytes);
            string hashHex = Hex16.ToHex16(hashed);

            return hashHex.ToLower();
        }

        public static string Hash(Stream s, string fileName = "")
        {
            byte[] hashed = HashBytes(s);
            string hashHex = Hex16.ToHex16(hashed);
            return hashHex;
        }

        public static byte[] HashBytes(byte[] bytes)
        {
            return SHA1.Create().ComputeHash(bytes);
        }

        public static byte[] HashBytes(Stream s)
        {
            return SHA1.Create().ComputeHash(s);
        }


        public static Stream HashStream(byte[] bytes)
        {
            byte[] hashed = SHA1.Create().ComputeHash(bytes);
            return new MemoryStream(hashed);
        }


        public static Stream HashStream(Stream s)
        {
            byte[] hashed = SHA256.Create().ComputeHash(s);
            return new MemoryStream(hashed);
        }

    }


}
