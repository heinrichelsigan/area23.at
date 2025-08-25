using System;
using System.Collections.Generic;
using System.Linq;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{
    /// <summary>
    /// Normal hexadecimal byte encoding / decoding
    /// </summary>
    public class Hex16 : IDecodable
    {
        public const string VALID_CHARS = "0123456789abcdef";        
        #region common interface, interfaces for static members appear in C# 7.3 or later
        public IDecodable Decodable => this;

        HashSet<char> IDecodable.ValidCharList => new HashSet<char>(VALID_CHARS.ToCharArray());


        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public string EnCode(byte[] inBytes) => Hex16.ToHex16(inBytes);

        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public byte[] DeCode(string encodedString) => Hex16.FromHex16(encodedString);


        bool IDecodable.Validate(string encodedStr) => Hex16.IsValidHex16(encodedStr, out _);

        public bool IsValidShowError(string encodedString, out string error) => Hex16.IsValidHex16(encodedString, out error);

        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public static string Encode(byte[] inBytes) => ToHex16(inBytes);


        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public static byte[] Decode(string encodedString) => FromHex16(encodedString);


        /// <summary>
        /// Checks if a string is a valid encoded string
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>true, when encoding is OK, otherwise false, if encoding contains illegal characters</returns>
        public static bool IsValid(string encodedString) => IsValidHex16(encodedString, out _);


        /// <summary>
        /// Encode ToHex converts a binary byte array to hex string
        /// </summary>
        /// <param name="inBytes">byte array</param>
        /// <returns>hex string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToHex16(byte[] inBytes)
        {
            if (inBytes == null || inBytes.Length == 0)
                throw new ArgumentNullException("inBytes", "public static string ToHex(byte[] inBytes == NULL)");

            string hexString = string.Empty;
            for (int wc = 0; wc < inBytes.Length; wc++)
            {
                hexString += string.Format("{0:x2}", inBytes[wc]);
            }

            // string strUtf8 = System.Text.Encoding.UTF8.GetString(inBytes);

            return hexString;
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

            // byte[] bytesUtf8 = System.Text.Encoding.UTF8.GetBytes(hexStr);
            return bytes.ToArray();

        }


        public static bool IsValidHex16(string inString, out string error)
        {
            bool valid = true;
            error = "";
            foreach (char ch in inString)
            {
                if (!VALID_CHARS.ToCharArray().Contains(ch))
                {
                    error += ch.ToString();
                    valid = false;
                }
            }
            return valid;
        }


    }

}
