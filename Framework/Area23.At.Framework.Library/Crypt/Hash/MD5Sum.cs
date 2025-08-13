using Area23.At.Framework.Library.Static;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Area23.At.Framework.Library.Crypt.Hash
{

    /// <summary>
    /// MD5Sum hashes a md5 sum for a string, stream, byte[], 
    /// </summary>
    public static class MD5Sum
    {

        /// <summary>
        /// Hash
        /// </summary>
        /// <param name="filePath">filePath to file</param>
        /// <param name="showFileName">show fileName after hash sum</param>
        /// <returns></returns>
        public static string Hash(string filePath, bool showFileName = true)
        {
            if (!System.IO.File.Exists(filePath))
                return Hash(Encoding.Default.GetBytes(filePath));

            byte[] bytes = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);

            return (showFileName) ? Hash(bytes, fileName) : Hash(bytes);
        }


        public static string HashString(string string2Hash, string fileName = "")
        {
            byte[] bytes = EnDeCoding.EnDeCodeHelper.GetBytes(string2Hash);
            string hashed = Hash(bytes, fileName);
            return hashed;
        }


        public static string Hash(byte[] bytes, string fileName = "")
        {
            byte[] hashed = MD5.Create().ComputeHash(bytes);
            string hash = hashed.ToHexString();
            if (!string.IsNullOrEmpty(fileName))
                hash += "  " + fileName;

            return hash.ToLower();
        }

        public static string Hash(Stream s, string fileName = "")
        {
            byte[] bytes = MD5.Create().ComputeHash(s);
            string hash = bytes.ToHexString();

            if (!string.IsNullOrEmpty(fileName))
                hash += "  " + fileName;

            return hash.ToLower();
        }

        public static byte[] HashBytes(byte[] bytes)
        {
            return MD5.Create().ComputeHash(bytes);
        }

        public static byte[] HashBytes(Stream s)
        {
            return MD5.Create().ComputeHash(s);
        }


        public static Stream HashStream(byte[] bytes)
        {
            byte[] hashed = MD5.Create().ComputeHash(bytes);
            return new MemoryStream(bytes);
        }


        public static Stream HashStream(Stream s)
        {
            byte[] bytes = MD5.Create().ComputeHash(s);
            return new MemoryStream(bytes);
        }

    }

}
