using Area23.At.Framework.Core.Static;
using System.Security.Cryptography;
using System.Text;

namespace Area23.At.Framework.Core.Crypt.Hash
{

    /// <summary>
    /// Sha256Sum creates Sha512Sum of a file or stream or byte[] or string
    /// </summary>
    public static class Sha512Sum
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
                throw new ArgumentNullException($"Sha512Sum.Hash(filePath, showFileName = {showFileName}) filePath is null or empty or file at filePath doesn't exist.");
               
            byte[] bytes = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            
            return showFileName ? Hash(bytes, fileName) : Hash(bytes);
        }


        /// <summary>
        /// Hashes a string strng
        /// </summary>
        /// <param name="strng"><see cref="string">string strng</see></param>
        /// <param name="fileName">optional filename to add after hash</param>
        public static string Hash(string strng, string fileName = "") => Hash(Encoding.UTF8.GetBytes(strng), fileName);


        /// <summary>
        /// Hashes a Sha512 of byte[]
        /// </summary>
        /// <param name="bytes"><see cref="byte[]">byte[] bytes</see></param>
        /// <param name="fileName">optional fileName to end</param>
        /// <returns></returns>
        public static string Hash(byte[] bytes, string fileName = "") => HashBytes(bytes).ToHexString() + (!string.IsNullOrEmpty(fileName) ? " " + fileName : "");


        /// <summary>
        /// Hashes a stream
        /// </summary>
        /// <param name="stream">stream strm</param>
        /// <param name="fileName">optional filename to add after hash</param>
        /// <returns><Sha512 hash with optional fileName/returns>
        public static string Hash(Stream stream, string fileName = "")
        {            
            string hash = HashBytes(stream).ToHexString();
            return string.IsNullOrEmpty(fileName) ? hash : $"{hash} {fileName}";
        }

        #region helper methods

        internal static string HashString(string s) => (HashBytes(Encoding.UTF8.GetBytes(s))).ToHexString();

        public static byte[] HashBytes(byte[] bytes) => SHA512.HashData(bytes);

        public static byte[] HashBytes(Stream s) => SHA512.HashData(s);

        internal static Stream HashStream(byte[] bytes) => new MemoryStream(SHA512.HashData(bytes));

        internal static Stream HashStream(Stream s) =>new MemoryStream(SHA512.HashData(s));
        
        #endregion helper methods

    }

}
