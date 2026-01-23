using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{

    /*
 * Table 2: The "URL and Filename safe" Base 64 Alphabet
 *   Value Encoding  Value Encoding  Value Encoding  Value Encoding
 *       0 A            17 R            34 i            51 z
 *       1 B            18 S            35 j            52 0
 *       2 C            19 T            36 k            53 1
 *       3 D            20 U            37 l            54 2
 *       4 E            21 V            38 m            55 3
 *       5 F            22 W            39 n            56 4
 *       6 G            23 X            40 o            57 5
 *       7 H            24 Y            41 p            58 6
 *       8 I            25 Z            42 q            59 7
 *       9 J            26 a            43 r            60 8
 *      10 K            27 b            44 s            61 9
 *      11 L            28 c            45 t            62 - (minus)
 *      12 M            29 d            46 u            63 _ (underscore)
 *      13 N            30 e            47 v           
 *      14 O            31 f            48 w
 *      15 P            32 g            49 x
 *      16 Q            33 h            50 y         (pad) =
 *
 */

    /// <summary>
    /// Hex64 mime standard encoding
    /// Must handle 0xfeff
    /// <see href="https://www.fileformat.info/info/unicode/char/feff/index.htm" />
    /// Unicode Character 'ZERO WIDTH NO-BREAK SPACE' (U+FEFF)
    /// </summary>
    public class Hex64 : IDecodable
    {

        public const char ZERO_WIDTH_NO_BREAK_SPACE = (char)0xfeff;
        public static readonly char[] SPECIAL_CHAR_ARRAY = { ZERO_WIDTH_NO_BREAK_SPACE, ' ', '\t', '\r', '\n' };
        public static readonly string SPECIAL_CHARS = new string(SPECIAL_CHAR_ARRAY);
        public static readonly string VALID_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz+/=" + SPECIAL_CHARS;

        private static readonly object _lock = new object();
        static string invalidChars = "";

        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;

        HashSet<char> IDecodable.ValidCharList => new HashSet<char>(VALID_CHARS.ToCharArray());
       
        public string EnCode(byte[] inBytes) => Hex64.Encode(inBytes);

        public byte[] DeCode(string encodedString) => Hex64.Decode(encodedString);
        
        public bool Validate(string encodedStr) => Hex64.IsValidHex64(encodedStr, out _);

        public bool IsValidShowError(string encodedString, out string error) => Base64.IsValidBase64(encodedString, out error);


        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public static string Encode(byte[] inBytes)
        {
            return ToHex64(inBytes);
        }

        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public static byte[] Decode(string encodedString)
        {
            return FromHex64(encodedString);
        }

        /// <summary>
        /// Checks if a string is a valid encoded string
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>true, when encoding is OK, otherwise false, if encoding contains illegal characters</returns>
        public static bool IsValid(string encodedString) => IsValidHex64(encodedString, out _);


        public static string ToHex64(byte[] inBytes)
        {
            string os = Convert.ToBase64String(
                inBytes,
                0,
                inBytes.Length,
                Base64FormattingOptions.InsertLineBreaks
            // Base64FormattingOptions.None                
            );
            return os.Replace('+', '-').Replace('/', '_');
        }

        public static byte[] FromHex64(string inString)
        {
            bool valid = true;
            string error = "", parsedString = "";


            foreach (char ch in parsedString)
            {
                // if (!ValidCharList.Contains(ch))
                if (!VALID_CHARS.ToCharArray().ToList().Contains(ch))               
                {
                    error += ch;
                    valid = false;
                }
            }
            byte[] outBytes = new byte[0];

            parsedString = (string.IsNullOrEmpty(error)) ?
                inString.Replace('-', '+').Replace('_', '/') :
                inString.Trim(error.ToCharArray()).Replace('-', '+').Replace('_', '/');
            try
            {
                outBytes = Convert.FromBase64String(inString.Replace('-', '+').Replace('_', '/'));
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsg($"Base64.FromBase64", "need to trim error chars \"{error}\", " +
                    $"because of Exception {ex.GetType().Name} with message: {ex.Message}", 2);
                outBytes = Convert.FromBase64String(parsedString);
            }
            return outBytes;
        }


        public static bool IsValidHex64(string inString, out string error)
        {
            bool valid = true;
            error = "";
            foreach (char ch in inString)
            {
                // if (!ValidCharList.Contains(ch))
                if (!VALID_CHARS.ToCharArray().ToList().Contains(ch))
                {
                    error += ch;
                    valid = false;
                }
            }
            return valid;
        }


    }

}