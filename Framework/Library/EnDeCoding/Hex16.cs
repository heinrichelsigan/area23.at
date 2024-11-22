using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.EnDeCoding
{
    /// <summary>
    /// Normal hexadecimal byte encoding / decoding
    /// </summary>
    public static class Hex16
    {
        private static readonly char[] _digits = "0123456789abcdef".ToCharArray();
        private static List<char> ValidCharList = new List<char>(_digits);

        /// <summary>
        /// ToHex converts a binary byte array to hex string
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

            string strUtf8 = hexString.ToLower();

            return strUtf8;
        }


        /// <summary>
        /// FromHex transforms a hex string to binary byte array
        /// </summary>
        /// <param name="hexStr">a hex string</param>
        /// <returns>binary byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] FromHex16(string hexStr)
        {
            if (string.IsNullOrEmpty(hexStr))
                throw new ArgumentNullException("hexStr", "public static byte[] FromHex(string hexStr), hexStr == NULL || hexStr == \"\"");

            List<byte> bytes = new List<byte>();

            if (hexStr.Length % 2 == 1)
                hexStr += "0";
            for (int wb = 0; wb < hexStr.Length; wb += 2)
            {
                char msb = (char)hexStr[wb];
                char lsb = (char)hexStr[wb + 1];
                string sb = msb.ToString() + lsb.ToString();
                byte b = Convert.ToByte(sb, 16);
                bytes.Add(b);
            }

            byte[] bytesUtf8 = EnCoderHelper.GetBytes8(hexStr);
            // return bytesUtf8;
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
