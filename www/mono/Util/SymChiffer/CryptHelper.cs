using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.Util.SymChiffer
{
    public static class CryptHelper
    {

        public static string KeyHexString(string key)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
            string ivStr = keyBytes.ToHexString();
            return ivStr;
        }

        public static byte[] GetBytesFromString(string inString, bool upStretchToCorrectBlockSize = false)
        {
            string sourceString = (string.IsNullOrEmpty(inString)) ? string.Empty : inString;
            byte[] sourceBytes = System.Text.Encoding.UTF8.GetBytes(sourceString);
            int inBytesLen = sourceBytes.Length;

            if (upStretchToCorrectBlockSize)
            {
                int mul = ((int)(sourceBytes.Length / 256));
                double dDiv = (double)(sourceBytes.Length / 256);
                int iFactor = (int)Math.Min(Math.Truncate(dDiv), Math.Round(dDiv));
                inBytesLen = (iFactor + 1) * 256;
            }

            byte[] inBytes = new byte[inBytesLen];
            for (int bytCnt = 0; bytCnt < inBytesLen; bytCnt++)
            {
                inBytes[bytCnt] = (bytCnt < sourceBytes.Length) ? sourceBytes[bytCnt] : (byte)0;
            }

            return inBytes;
        }

        public static string GetStringFromBytesTrimNulls(byte[] decryptedBytes)
        {
            int ig = -1;
            string decryptedText = string.Empty;

            if ((ig = decryptedBytes.ArrayIndexOf((byte)0)) > 0)
            {
                byte[] decryptedNonNullBytes = new byte[ig];
                Array.Copy(decryptedBytes, decryptedNonNullBytes, ig);
                decryptedText = System.Text.Encoding.UTF8.GetString(decryptedNonNullBytes);
            }
            else
                decryptedText = System.Text.Encoding.UTF8.GetString(decryptedBytes);

            return decryptedText;
        }

        public static string Trim_Decrypted_Text(string decryptedText)
        {
            int ig = 0;
            List<char> charList = new List<char>();
            for (int i = 1; i < 32; i++)
            {
                char ch = (char)i;
                if (ch != '\v' && ch != '\f' && ch != '\t' && ch != '\r' && ch != '\n')
                    charList.Add(ch);
            }
            char[] chars = charList.ToArray();
            decryptedText = decryptedText.TrimEnd(chars);
            decryptedText = decryptedText.TrimStart(chars);
            decryptedText = decryptedText.Replace("\0", "");
            foreach (char ch in chars)
            {
                while ((ig = decryptedText.IndexOf(ch)) > 0)
                {
                    decryptedText = decryptedText.Substring(0, ig) + decryptedText.Substring(ig + 1);
                }
            }

            return decryptedText;
        }
    }
}