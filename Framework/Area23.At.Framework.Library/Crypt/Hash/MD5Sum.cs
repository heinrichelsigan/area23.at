using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Crypt.Hash
{

    public static class MD5Sum
    {

        public static string Hash(string filePath, bool showFileName = true)
        {
            if (!System.IO.File.Exists(filePath))
                return Hash(Encoding.Default.GetBytes(filePath));

            byte[] bytes = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            string hash = (showFileName) ? Hash(bytes, fileName) : Hash(bytes);

            return hash;
        }


        public static string Hash(byte[] bytes, string fileName = null)
        {
            byte[] hashed = MD5.Create().ComputeHash(bytes);
            string hasha = Encoding.UTF8.GetString(hashed);
            string hashb = hashed.ToHexString();
            if (!string.IsNullOrEmpty(fileName))
            {
                hasha += "  " + fileName;
                hashb += "  " + fileName;
            }

            return hashb.ToLower();
        }

        public static string Hash(Stream s, string fileName = null)
        {
            byte[] bytes = MD5.Create().ComputeHash(s);
            string hasha = BitConverter.ToString(bytes).Replace("-", string.Empty);
            string hashb = bytes.ToHexString();

            if (!string.IsNullOrEmpty(fileName))
            {
                hasha += "  " + fileName;
                hashb += "  " + fileName;
            }

            return hashb.ToLower();
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
