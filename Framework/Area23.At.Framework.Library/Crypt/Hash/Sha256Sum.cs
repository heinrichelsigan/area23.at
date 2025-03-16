using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Area23.At.Framework.Library.Crypt.Hash
{

    /// <summary>
    /// Sha256Sum creates Sha256Sum of a file or stream or byte[] or string
    /// </summary>
    public static class Sha256Sum
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
            return (showFileName) ? Hash(fileBytes, fileName) : Hash(fileBytes);
        }


        public static string HashString(string string2Hash, string fileName = "")
        {
            byte[] bytes = EnDeCoding.EnDeCodeHelper.GetBytes(string2Hash);
            string hashed = Hash(bytes, fileName);
            return hashed;
        }


        public static string Hash(byte[] bytes, string fileName = "")
        {
            byte[] hashed = HashBytes(bytes);
            string hasha = Encoding.UTF8.GetString(hashed);
            string hashb = hashed.ToHexString();
            if (!string.IsNullOrEmpty(fileName))
            {
                hasha += "  " + fileName;
                hashb += "  " + fileName;
            }
            return hashb.ToLower();
        }

        public static string Hash(Stream s, string fileName = "")
        {
            byte[] hashed = HashBytes(s);
            string hasha = BitConverter.ToString(hashed).Replace("-", string.Empty);
            string hashb = hashed.ToHexString();
            if (!string.IsNullOrEmpty(fileName))
            {
                hasha += "  " + fileName;
                hashb += "  " + fileName;
            }
            return hashb.ToLower();
        }

        public static byte[] HashBytes(byte[] bytes)
        {
            return SHA256.Create().ComputeHash(bytes);
        }

        public static byte[] HashBytes(Stream s)
        {
            return SHA512.Create().ComputeHash(s);
        }


        public static Stream HashStream(byte[] bytes)
        {
            byte[] hashed = SHA256.Create().ComputeHash(bytes);
            return new MemoryStream(hashed);
        }


        public static Stream HashStream(Stream s)
        {
            byte[] hashed = SHA256.Create().ComputeHash(s);
            return new MemoryStream(hashed);
        }

    }

}
