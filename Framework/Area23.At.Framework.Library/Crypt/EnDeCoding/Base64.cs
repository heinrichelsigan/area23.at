using System;
using System.Collections.Generic;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{

    /// <summary>
    /// Base64 mime standard encoding
    /// Must handle 0xfeff
    /// <see href="https://www.fileformat.info/info/unicode/char/feff/index.htm" />
    /// Unicode Character 'ZERO WIDTH NO-BREAK SPACE' (U+FEFF)
    /// </summary>
    public class Base64 : IDecodable
    {

        public const char ZERO_WIDTH_NO_BREAK_SPACE = (char)0xfeff;
        public static readonly char[] SPECIAL_CHAR_ARRAY = { ZERO_WIDTH_NO_BREAK_SPACE, ' ', '\t', '\r', '\n' };
        public static readonly string SPECIAL_CHARS = new string(SPECIAL_CHAR_ARRAY);
        public static readonly string VALID_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/=" + SPECIAL_CHARS;

        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;

        HashSet<char> IDecodable.ValidCharList => new HashSet<char>(VALID_CHARS.ToCharArray());
       
        public string EnCode(byte[] inBytes) => Base64.Encode(inBytes);

        public byte[] DeCode(string encodedString) => Base64.Decode(encodedString);
        
        public bool Validate(string encodedStr) => Base64.IsValidBase64(encodedStr, out _);

        public bool IsValidShowError(string encodedString, out string error) => Base64.IsValidBase64(encodedString, out error);


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
        public static bool IsValid(string encodedString) => IsValidBase64(encodedString, out _);


        public static string ToBase64(byte[] inBytes)
        {            
            string os = Convert.ToBase64String(inBytes, 0, inBytes.Length, Base64FormattingOptions.None);
            return os;
        }

        public static byte[] FromBase64(string inString)
        {
            inString = inString.Replace(Base64.ZERO_WIDTH_NO_BREAK_SPACE.ToString(), "");
            byte[] outBytes = Convert.FromBase64String(inString);
            return outBytes;
        }

        public static bool IsValidBase64(string inString, out string error)
        {
            bool valid = true;
            error = "";
            foreach (char ch in inString)
            {
                if (!(new HashSet<char>(VALID_CHARS.ToCharArray()).Contains(ch)))
                {
                    error += ch;
                    valid = false;
                }
            }
            return valid;
        }

    }

}