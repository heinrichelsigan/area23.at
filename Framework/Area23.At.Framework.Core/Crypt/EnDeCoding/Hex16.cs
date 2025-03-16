using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Area23.At.Framework.Core.Crypt.EnDeCoding
{
    /// <summary>
    /// Normal hexadecimal byte encoding / decoding
    /// </summary>
    public class Hex16 : IDecodable
    {
        
        public const string VALID_CHARS = "0123456789abcdef";


        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;
        
        public static HashSet<char>? ValidCharList { get; private set; } = new HashSet<char>(VALID_CHARS.ToCharArray());

        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public string Encode(byte[] inBytes) => Hex16.ToHex16(inBytes);

        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public byte[] Decode(string encodedString) => Hex16.FromHex16(encodedString);


        public bool IsValid(string encodedStr) => Hex16.IsValidHex16(encodedStr, out _);

        public bool IsValidShowError(string encodedString, out string error) => Hex16.IsValidHex16(encodedString, out error);
                
        
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

            byte[] bytesUtf8 = EnDeCodeHelper.GetBytes(hexStr);
            // return bytesUtf8
            return bytes.ToArray();

        }
        

        public static bool IsValidHex16(string inString, out string error)
        {
            bool isValid = true;
            error = "";
            foreach (char ch in inString)
            {
                if (!VALID_CHARS.ToCharArray().Contains(ch))
                {
                    error += ch.ToString();
                    isValid = false;
                }
            }

            return isValid;
        }

    }

}
