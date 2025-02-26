using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{
    /// <summary>
    /// Base64 mime standard encoding
    /// </summary>
    public class Base64 : IDecodable
    {

        public const string VALID_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/=";

        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;

        HashSet<char> IDecodable.ValidCharList => new HashSet<char>(VALID_CHARS.ToCharArray());
       
        public string EnCode(byte[] inBytes)
        {
            return Base64.Encode(inBytes);
        }

        public byte[] DeCode(string encodedString)
        {
            return Base64.Decode(encodedString);
        }

        public bool Validate(string encodedString)
        {
            return Base64.IsValid(encodedString);
        }

        #endregion common interface, interfaces for static members appear in C# 7.3 or later


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


        public static string ToBase64(byte[] inBytes)
        {            
            string os = Convert.ToBase64String(inBytes, 0, inBytes.Length, Base64FormattingOptions.InsertLineBreaks);
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
                if (!VALID_CHARS.ToCharArray().Contains(ch))
                    return false;
            }

            return true;
        }

    }

}