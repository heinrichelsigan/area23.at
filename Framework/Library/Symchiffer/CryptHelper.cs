using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Symchiffer
{

    /// <summary>
    /// static class CryptHelper provides static helper methods for encryption / decryption
    /// </summary>
    public static class CryptHelper
    {

        /// <summary>
        /// KeyHexString transforms a private secret key to hex string
        /// </summary>
        /// <param name="key">private secret key</param>
        /// <returns>hex string of bytes</returns>
        public static string KeyHexString(string key)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
            string ivStr = keyBytes.ToHexString();
            return ivStr;
        }

        /// <summary>
        /// GetBytesFromString gets byte[] array representing binary transformation of a string
        /// </summary>
        /// <param name="inString">string to transfer to binary byte[] data</param>
        /// <param name="upStretchToCorrectBlockSize">fills at the end of byte[] padding zero 0 bytes</param>
        /// <returns>byte[] array of binary byte</returns>
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

        /// <summary>
        /// GetStringFromBytesTrimNulls gets a plain text string from binary byte[] data and truncate all 0 byte at the end.
        /// </summary>
        /// <param name="decryptedBytes">decrypted byte[]</param>
        /// <returns>truncated string without a lot of \0 (null) characters</returns>
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

        /// <summary>
        /// Trim_Decrypted_Text removes all special control characters from a text string
        /// </summary>
        /// <param name="decryptedText">string to trim and strip from special control characters.</param>
        /// <returns>text only string with at least text formation special characters.</returns>
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
