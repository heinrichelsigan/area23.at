using Area23.At.Framework.Library.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Crypt.Hash
{
    public static class Sha256Sum
    {
        internal static string Hash(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return Hash(Encoding.Default.GetBytes(filePath));

            byte[] bytes = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            string hash = Hash(bytes) + "  " + fileName;
            return hash;
        }


        internal static string Hash(byte[] bytes)
        {
            byte[] hashBytes = HashBytes(bytes);
            string hash = hashBytes.ToHexString();
            return hash;
        }

        internal static string Hash(Stream s)
        {
            byte[] bytes = HashBytes(s);
            string a = BitConverter.ToString(bytes).Replace("-", string.Empty);
            string hash = bytes.ToHexString();

            return hash;
        }

        internal static byte[] HashBytes(byte[] bytes)
        {
            return SHA256.Create().ComputeHash(bytes);
        }

        internal static byte[] HashBytes(Stream s)
        {
            return SHA256.Create().ComputeHash(s);
        }


        internal static Stream HashStream(byte[] bytes)
        {
            byte[] hashBytes = SHA3_512.Create().ComputeHash(bytes);
            return new MemoryStream(hashBytes);

        }


        internal static Stream HashStream(Stream s)
        {
            byte[] bytes = SHA256.Create().ComputeHash(s);
            return new MemoryStream(bytes);
        }

    }

}
