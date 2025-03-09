﻿using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Crypt.Hash
{

    public static class Sha256Sum
    {

        public static string Hash(string filePath, bool showFileName = true)
        {
            if (!System.IO.File.Exists(filePath))
                return Hash(Encoding.Default.GetBytes(filePath));

            byte[] fileBytes = File.ReadAllBytes(filePath);
            string fileName = Path.GetFileName(filePath);
            string hash = (showFileName) ? Hash(fileBytes, fileName) : Hash(fileBytes);
            return hash;
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
