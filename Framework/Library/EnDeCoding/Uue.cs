using Area23.At.Framework.Library;
using DBTek.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
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

        public static readonly char[] ValidChars = "!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_` \r\n".ToCharArray();
        public static List<char> ValidCharList = new List<char>(ValidChars);

        /// <summary>
        /// ToUu
        /// </summary>
        /// <param name="inBytes">binary byte array</param>
        /// <returns>uuencoded string</returns>
        public static string ToUu(byte[] inBytes, bool originalUue = true)
        {
            string bytStr = string.Empty;
            string uu = "";

            
            if (originalUue)
            {
                bytStr = Encoding.ASCII.GetString(inBytes);
                uu = (new UUEncoder()).EncodeString(bytStr);
            }
            else
            {
                string hexOutFile = DateTime.Now.Area23DateTimeWithMillis() + ".hex";
                string hexOutPath = LibPaths.UuDirPath + hexOutFile;
                string uuOutFile = DateTime.Now.Area23DateTimeWithMillis() + ".uue";
                string uuOutPath = LibPaths.UuDirPath + uuOutFile;
                // inBytes.ToFile(uuOutPath);
                System.IO.File.WriteAllBytes(hexOutPath, inBytes);
                ProcessCmd.Execute("uuencode", $"{hexOutPath} {uuOutFile} > {uuOutPath}", false);
                uu = System.IO.File.ReadAllText($"{uuOutPath}");
            }

            return uu;
        }


        /// <summary>
        /// FromUu
        /// </summary>
        /// <param name="uuEncStr">uuencoded string</param>
        /// <returns>binary byte array</returns>
        public static byte[] FromUu(string uuEncStr, bool originalUue = true)
        {
            string plainStr = string.Empty;
            byte[] plainBytes;
            if (originalUue)
            {
                plainStr = (new UUEncoder()).DecodeString(uuEncStr);
                plainBytes = Encoding.ASCII.GetBytes(plainStr);
            }
            else
            {
                string uuOutFile = DateTime.Now.Area23DateTimeWithMillis() + ".uue";
                string uuOutPath = LibPaths.UuDirPath + uuOutFile;
                string hexOutFile = DateTime.Now.Area23DateTimeWithMillis() + ".hex";
                string hexOutPath = LibPaths.UuDirPath + hexOutFile;

                System.IO.File.WriteAllText(uuOutPath, uuEncStr);
                ProcessCmd.Execute("uudecode", $"{uuOutPath} -o {hexOutPath}", false);
                plainBytes = System.IO.File.ReadAllBytes(hexOutPath);
            }

            return plainBytes;

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

        public static bool IsValidUue(string uuEncodedStr)
        {
            if (uuEncodedStr.StartsWith("begin"))
            {
                int firstNewLineIds = uuEncodedStr.IndexOf('\n');
                if (firstNewLineIds > -1) 
                    uuEncodedStr = uuEncodedStr.Substring(firstNewLineIds);
            }
            if (uuEncodedStr.EndsWith("\nend") || uuEncodedStr.EndsWith("\nend\n") || uuEncodedStr.EndsWith("\nend\r\n"))
            {
                uuEncodedStr = uuEncodedStr.Replace("\nend\r\n", "\n");
                uuEncodedStr = uuEncodedStr.Replace("\nend\n", "\n");
                uuEncodedStr = uuEncodedStr.Replace("\nend", "\n");
            }
                
            foreach (char ch in uuEncodedStr)
            {
                if (!ValidCharList.Contains(ch))
                    return false;
            }
            return true;
        }

        #region helper
        private static byte[] UuEncodeBytes(byte[] src, int len)
        {
            if (len == 0) return new byte[] { 96, 13, 10 };

            List<byte> bytes = new List<byte>(src);
            switch ((src.Length % 3))
            {
                case 1: bytes.Add((byte)0); bytes.Add((byte)0); src = bytes.ToArray(); break;
                case 2: bytes.Add((byte)0); src = bytes.ToArray(); break;
                case 0:
                default: break;
            }
            List<byte> cod = new List<byte>();
            // cod.Add((byte)(len + 32));

            for (int i = 0; i < src.Length; i += 3)
            {
                cod.Add((byte)(32 + src[i] / 4));
                cod.Add((byte)(32 + (src[i] % 4) * 16 + src[i + 1] / 16));
                cod.Add((byte)(32 + (src[i + 1] % 16) * 4 + src[i + 2] / 64));
                cod.Add((byte)(32 + src[i + 2] % 64));
                // cod.Add((char)((src[i] >> 2) + 33));
                // cod.Add((char)(((char)((src[i] & 0x3) << 4) | (char)(src[i + 1] >> 4)) + 33));
                // cod.Add((char)(((char)((src[i + 1] & 0xf) << 2) | (char)(src[i + 2] >> 6)) + 33));
                // cod.Add((char)((char)(src[i + 2] & 0x3f) + 33));
            }

            return cod.ToArray();
        }

        private static byte[] UuDecodeBytes(byte[] uuEnc, int len)
        {
            List<byte> bytes = new List<byte>(uuEnc);
            switch ((uuEnc.Length % 4))
            {
                case 1: bytes.Add((byte)0); bytes.Add((byte)0); bytes.Add((byte)0); uuEnc = bytes.ToArray(); break;
                case 2: bytes.Add((byte)0); bytes.Add((byte)0); uuEnc = bytes.ToArray(); break;
                case 3: bytes.Add((byte)0); uuEnc = bytes.ToArray(); break;
                case 0:
                default: break;
            }
            
            List<byte> cod = new List<byte>();
            for (int i = 0; i < uuEnc.Length; i += 4)
            {
                cod.Add((byte)((byte)((uuEnc[i] - 33) << 2) | (byte)((uuEnc[i + 1] - 33) >> 4)));
                cod.Add((byte)(((byte)((uuEnc[i] & 0x3) << 4) | (uuEnc[i + 1] >> 4)) + 33));
                cod.Add((byte)(((byte)((uuEnc[i + 1] & 0xf) << 2) | (byte)(uuEnc[i + 2] >> 6)) + 33));
                cod.Add((byte)(((byte)(uuEnc[i + 2] - 33) << 6) | (byte)(uuEnc[i + 3] - 33)));
            }

            return cod.ToArray();
        }

        #endregion helper

    }

}