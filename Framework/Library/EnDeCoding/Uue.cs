using DBTek.Crypto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Area23.At.Framework.Library.EnDeCoding
{
    /// <summary>
    /// Uu is unix2unix uuencode uudecode
    /// </summary>
    public static class Uu
    {

        /// <summary>
        /// ToUu
        /// </summary>
        /// <param name="inBytes">binary byte array</param>
        /// <returns>uuencoded string</returns>
        public static string ToUu(byte[] inBytes)
        {            
            string bytStr = Hex.ToHex(inBytes);
            string uu = (new UUEncoder()).EncodeString(bytStr);

            //for (int i = 0; i <= inBytes.Length; i += 45)
            //{
            //    int l = ((inBytes.Length - i) > 45) ? 45 : inBytes.Length - i;
            //    byte[] linea = new byte[(l % 3 == 0) ? l : l + 3 - l % 3];
            //    Array.ConstrainedCopy(inBytes, i, linea, 0, l);
            //    uu += Array.ConvertAll<byte, char>(UuEncodeBytes(linea, l), Convert.ToChar).ToString() + "\n";
            //}

            return uu;
        }


        /// <summary>
        /// FromUu
        /// </summary>
        /// <param name="uuEncStr">uuencoded string</param>
        /// <returns>binary byte array</returns>
        public static byte[] FromUu(string uuEncStr)
        {
            string plainStr = (new UUEncoder()).DecodeString(uuEncStr);
            byte[] plainBytes = Hex.FromHex(plainStr);
            return plainBytes;

            //List<byte> bytes = new List<byte>();

            //string[] input = uuEncStr.Split("\n".ToCharArray());
            //foreach (string str in input)
            //{
            //    byte[] dec = Array.ConvertAll<char, byte>(uue.DecodeString(str).ToCharArray(), Convert.ToByte);
            //    bytes.AddRange(dec);
            //}

            //return bytes.ToArray();
        }


        /// <summary>
        /// UuEncode unix 2 unix encodes a string
        /// </summary>
        /// <param name="plainText">plain text string to encode</param>
        /// <returns>uuencoded string</returns>
        public static string UuEncode(string plainText)
        {
            string uue = (new UUEncoder()).EncodeString(plainText);
            return uue;
        }


        /// <summary>
        /// UuDecode unix 2 unix decodes a string
        /// </summary>
        /// <param name="uuEncodedStr">uuencoded string</param>
        /// <returns>uudecoded plain text</returns>
        public static string UuDecode(string uuEncodedStr)
        {
            string plainStr = (new UUEncoder()).DecodeString(uuEncodedStr);
            return plainStr;
        }

        #region helper
        private static byte[] UuEncodeBytes(byte[] input, int len)
        {
            if (len == 0) return new byte[] { 96, 13, 10 };

            List<byte> cod = new List<byte>();
            cod.Add((byte)(len + 32));

            for (int i = 0; i < len; i += 3)
            {
                cod.Add((byte)(32 + input[i] / 4));
                cod.Add((byte)(32 + (input[i] % 4) * 16 + input[i + 1] / 16));
                cod.Add((byte)(32 + (input[i + 1] % 16) * 4 + input[i + 2] / 64));
                cod.Add((byte)(32 + input[i + 2] % 64));
            }

            return cod.ToArray();
        }

        #endregion helper

    }
}