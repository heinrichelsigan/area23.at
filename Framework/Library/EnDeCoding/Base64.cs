using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Area23.At.Framework.Library.EnDeCoding
{
    /// <summary>
    /// Base64 mime standard encoding
    /// </summary>
    public static class Base64
    {

        public const string VALID_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/=";        
        private static readonly HashSet<char> ValidCharList = new HashSet<char>(VALID_CHARS.ToCharArray());

        #region common interface, interfaces for static members appear in C# 7.3 or later

        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public static string Encode(byte[] inBytes)
        {
            return ToBase64(inBytes);
        }

        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public static byte[] Decode(string encodedString)
        {
            return FromBase64(encodedString);
        }

        /// <summary>
        /// Checks if a string is a valid encoded string
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>true, when encoding is OK, otherwise false, if encoding contains illegal characters</returns>
        public static bool IsValid(string encodedString)
        {
            return IsValidBase64(encodedString);
        }

        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        public static string ToBase64(byte[] inBytes)
        {
            string os = Convert.ToBase64String(inBytes, 0, inBytes.Length, Base64FormattingOptions.None);
            return os;
        }

        public static byte[] FromBase64(string inString)
        {
            byte[] outBytes = Convert.FromBase64String(inString);
            return outBytes;
        }

        public static bool IsValidBase64(string inString)
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