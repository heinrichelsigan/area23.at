using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{
    /// <summary>
    /// Normal hexadecimal byte encoding / decoding
    /// </summary>
    public static class Hex16
    {
        
        public const string VALID_CHARS = "0123456789abcdef";        
        private static readonly HashSet<char> ValidCharList = new HashSet<char>(VALID_CHARS.ToCharArray());


        #region common interface, interfaces for static members appear in C# 7.3 or later

        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public static string Encode(byte[] inBytes)
        {
            return ToHex16(inBytes);
        }

        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public static byte[] Decode(string encodedString)
        {
            return FromHex16(encodedString);
        }

        /// <summary>
        /// Checks if a string is a valid encoded string
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>true, when encoding is OK, otherwise false, if encoding contains illegal characters</returns>
        public static bool IsValid(string encodedString)
        {
            return IsValidHex16(encodedString);
        }

        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        /// <summary>
        /// Encode ToHex converts a binary byte array to hex string
        /// </summary>
        /// <param name="inBytes">byte array</param>
        /// <returns>hex string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToHex16(byte[] inBytes)
        {
            if (inBytes == null || inBytes.Length < 1)
                throw new ArgumentNullException("inBytes", "public static string ToHex(byte[] inBytes == NULL)");

            string hexString = string.Empty;
            for (int wc = 0; wc < inBytes.Length; wc++)
            {
                hexString += string.Format("{0:x2}", inBytes[wc]);
            }

            string strUtf8 = hexString; // to slow for very large files.ToLower();

            return strUtf8;
        }

        /// <summary>
        /// Decode FromHex transforms a hex string to binary byte array
        /// </summary>
        /// <param name="hexStr">a hex string</param>
        /// <returns>binary byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] FromHex16(string hexStr)
        {
            if (string.IsNullOrEmpty(hexStr))
                throw new ArgumentNullException("hexStr", "public static byte[] FromHex(string hexStr), hexStr == NULL || hexStr == \"\"");

            List<byte> bytes = new List<byte>();

            for (int wb = 0; wb < hexStr.Length; wb += 2)
            {
                char msb, lsb;
                if (wb == hexStr.Length - 1)
                {
                    msb = '0';
                    lsb = hexStr[wb];
                }
                else
                {
                    msb = (char)hexStr[wb];
                    lsb = (char)hexStr[wb + 1];
                }
                string sb = msb.ToString() + lsb.ToString();
                byte b = Convert.ToByte(sb, 16);
                bytes.Add(b);
            }

            byte[] bytesUtf8 = EnDeCoder.GetBytes(hexStr);
            // return bytesUtf8
            return bytes.ToArray();

        }

        public static bool IsValidHex16(string inString)
        {
            foreach (char ch in inString)
            {
                if (!ValidCharList.Contains(ch))
                    return false;
            }
            return true;
        }

    }

}
