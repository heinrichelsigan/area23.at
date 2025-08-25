using System;
using System.Collections.Generic;
using System.Linq;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{
    /// <summary>
    /// Base16 hexadecimal byte encoding / decoding
    /// </summary>
    public class Base16 : IDecodable
    {
        public const string VALID_CHARS = "0123456789ABCDEF";       

        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;

        HashSet<char> IDecodable.ValidCharList => new HashSet<char>(VALID_CHARS.ToCharArray());

        /// <summary>
        /// Encodes a byte[] 
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>hex16 encoded string</returns>
        public string EnCode(byte[] inBytes) => Base16.ToBase16(inBytes);

        /// <summary>
        /// Decodes a hex string to byte[]
        /// </summary>
        /// <param name="hexString">hex16 encoded string</param>
        /// <returns></returns>
        public byte[] DeCode(string encodedString) => Base16.FromBase16(encodedString);


        public bool Validate(string encodedString) => Base16.IsValidBase16(encodedString, out _);


        public bool IsValidShowError(string encodedString, out string error) => Base16.IsValidBase16(encodedString, out error);

        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        /// <summary>
        /// Encodes a byte[] 
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>hex16 encoded string</returns>
        public static string Encode(byte[] inBytes)
        {
            return ToBase16(inBytes);
        }

        /// <summary>
        /// Decodes a hex string to byte[]
        /// </summary>
        /// <param name="hexString">hex16 encoded string</param>
        /// <returns></returns>
        public static byte[] Decode(string encodedString)
        {
            return FromBase16(encodedString);
        }

        /// <summary>
        /// Checks if a string is a valid encoded string
        /// </summary>
        /// <param name="encString">encoded string</param>
        /// <returns>true, when encoding is OK, otherwise false, if encoding contains illegal characters</returns>
        public static bool IsValid(string encodedString)
        {
            return Base16.IsValidBase16(encodedString, out _);
        }


        /// <summary>
        /// ToBase16 converts a binary byte array to hex string
        /// </summary>
        /// <param name="inBytes">byte array</param>
        /// <returns>hex string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string ToBase16(byte[] inBytes)
        {
            if (inBytes == null || inBytes.Length < 1)
                throw new ArgumentNullException("inBytes", "public static string ToHex(byte[] inBytes == NULL)");

            string hexString = string.Empty;            
            for (int wc = 0; wc < inBytes.Length; wc++)
            {
                hexString += string.Format("{0:X2}", inBytes[wc]);                
            }

            return hexString; // to slow for very large files .ToUpper();     
        }


        /// <summary>
        /// FromBase16 transforms a hex string to binary byte array
        /// </summary>
        /// <param name="hexStr">a hex string</param>
        /// <returns>binary byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] FromBase16(string hexStr)
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
                    lsb = Char.ToUpper((char)hexStr[wb]);
                }
                else
                {
                    msb = Char.ToUpper((char)hexStr[wb]);
                    lsb = Char.ToUpper((char)hexStr[wb + 1]);
                }
                string sb = msb.ToString() + lsb.ToString();
                byte b = Convert.ToByte(sb, 16);
                bytes.Add(b);
            }

            // byte[] bytesUtf8 = EnDeCodeHelper.GetBytes(hexStr);
            // return bytesUtf8;
            return bytes.ToArray();
            
        }


        public static bool IsValidBase16(string inString, out string error)
        {
            bool valid = true;
            error = "";
            foreach (char ch in inString)
            {
                if (!VALID_CHARS.ToArray().Contains(ch))
                {
                    error += ch;
                    valid = false;
                }
            }
            return valid;
        }

    }

}
