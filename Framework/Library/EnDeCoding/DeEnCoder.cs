using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.EnDeCoding
{
    /// <summary>
    /// DeEnCoder provides static members for formating
    /// </summary>
    public static class DeEnCoder
    {

        static DeEnCoder()
        {
        }


        /// <summary>
        /// KeyHexString transforms a private secret key to hex string
        /// </summary>
        /// <param name="key">private secret key</param>
        /// <returns>hex string of bytes</returns>
        public static string KeyHexString(string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            string ivStr = keyBytes.ToHexString();
            return ivStr;
        }

        /// <summary>
        /// EncodeEncryptedBytes encodes encrypted byte[] by encodingMethod to an encoded text string
        /// </summary>
        /// <param name="encryptBytes">encryptedBytes to encdode</param>
        /// <param name="encodingMethod">Encoding methods could be 
        /// "null", "hex16", "base16", "base32", "base32hex", "uu", "base64".
        /// "base64" is default.</param>
        /// <param name="fromPlain">Only for uu: true, if <see cref="encryptBytes"/> represent a binary without encryption</param>
        /// <param name="fromFile">Only for uu: true, if file and not textbox will be encrypted, default (false)</param>
        /// <returns>encoded encrypted string</returns>
        public static string EncodeEncryptedBytes(byte[] encryptBytes, string encodingMethod = "base64", bool fromPlain = false, bool fromFile = false)
        {
            string encryptedText = string.Empty;
            switch (encodingMethod.ToLowerInvariant())
            {
                case "null":        encryptedText = Encoding.UTF8.GetString(encryptBytes); break;
                case "hex16":       encryptedText = Hex16.ToHex16(encryptBytes); break;
                case "base16":      encryptedText = Base16.ToBase16(encryptBytes); break;
                case "base32":      encryptedText = Base32.ToBase32(encryptBytes); break;
                case "base32hex":   encryptedText = Base32Hex.ToBase32Hex(encryptBytes); break;
                case "uu":          encryptedText = Uu.ToUu(encryptBytes, fromPlain, fromFile); break;
                case "base64":
                default:            encryptedText = Base64.ToBase64(encryptBytes); break;
            }

            return encryptedText;
        }

        /// <summary>
        /// EncodedTextToBytes transforms an encoded text string into a <see cref="byte[]">býte array</see>
        /// </summary>
        /// <param name="cipherText">encoded (encrypted) text string</param>
        /// <param name="errMsg">out parameter to set an error message</param>
        /// <param name="encodingMethod">Encoding methods could be 
        /// "null", "hex16", "base16", "base32", "base32hex", "uu", "base64".
        /// "base64" is default.</param>
        /// <param name="fromPlain">Only for uu: true, if <see cref="encryptBytes"/> represent a binary without encryption</param>
        /// <param name="fromFile">Only for uu: true, if file and not textbox will be encrypted, default (false)</param>
        /// <returns>binary byte array</returns>
        public static byte[] EncodedTextToBytes(string cipherText, out string errMsg, string encodingMethod = "base64", bool fromPlain = false, bool fromFile = false) 
        {
            byte[] cipherBytes = null;
            errMsg = string.Empty;
            switch (encodingMethod.ToLowerInvariant())
            {
                case "null":
                    cipherBytes = Encoding.UTF8.GetBytes(cipherText);
                    break;
                case "hex16":
                    if (Hex16.IsValidHex16(cipherText))
                        cipherBytes = Hex16.FromHex16(cipherText);
                    else
                        errMsg = "Input Text is not a valid hex16 string!";
                    break;
                case "base16":
                    if (Base16.IsValidBase16(cipherText))
                        cipherBytes = Base16.FromBase16(cipherText);
                    else
                        errMsg = "Input Text is not a valid base16 string!";
                    break;
                case "base32":
                    if (Base32.IsValidBase32(cipherText))
                        cipherBytes = Base32.FromBase32(cipherText);
                    else
                        errMsg = "Input Text isn't a valid base32 string!";
                    break;
                case "base32hex":
                    if (Base32Hex.IsValidBase32Hex(cipherText))
                        cipherBytes = Base32Hex.FromBase32Hex(cipherText);
                    else
                        errMsg = "Input Text isn't a valid base32 hex string!";
                    break;
                case "uu":
                    if (Uu.IsValidUue(cipherText))
                        cipherBytes = Uu.FromUu(cipherText, fromPlain, fromFile);
                    else
                        errMsg = "Input Text isn't a valid uuencoded string!";
                    break;
                case "base64":
                default:
                    if (Base64.IsValidBase64(cipherText))
                        cipherBytes = Base64.FromBase64(cipherText);
                    else
                        errMsg = "Input Text isn't a valid base64 string!";
                    break;
            }

            return cipherBytes;
        }


        /// <summary>
        /// GetBytesFromString gets byte[] array representing binary transformation of a string
        /// </summary>
        /// <param name="inString">string to transfer to binary byte[] data</param>
        /// <param name="blockSize">current block size, default: 256</param>
        /// <param name="upStretchToCorrectBlockSize">fills at the end of byte[] padding zero 0 bytes, default: false</param>
        /// <returns>byte[] array of binary byte</returns>
        public static byte[] GetBytesFromString(string inString, int blockSize = 256, bool upStretchToCorrectBlockSize = false)
        {
            string sourceString = (string.IsNullOrEmpty(inString)) ? string.Empty : inString;
            byte[] sourceBytes = Encoding.UTF8.GetBytes(sourceString);
            int inBytesLen = sourceBytes.Length;
            if (blockSize == 0)
                blockSize = 256;
            else if (blockSize < 0)
                blockSize = Math.Abs(blockSize);

            if (upStretchToCorrectBlockSize)
            {
                int mul = ((int)(sourceBytes.Length / blockSize));
                double dDiv = (double)(sourceBytes.Length / blockSize);
                int iFactor = (int)Math.Min(Math.Truncate(dDiv), Math.Round(dDiv));
                inBytesLen = (iFactor + 1) * blockSize;
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

            ig = decryptedBytes.ArrayIndexOf((byte)0);
            if (ig > 0)
            {
                byte[] decryptedNonNullBytes = new byte[ig + 1];
                Array.Copy(decryptedBytes, decryptedNonNullBytes, ig + 1);
                decryptedText = Encoding.UTF8.GetString(decryptedNonNullBytes);
            }
            else
                decryptedText = Encoding.UTF8.GetString(decryptedBytes);

            if (decryptedText.Contains('\0'))
            {
                int slashNullIdx = decryptedText.IndexOf('\0');
                decryptedText = decryptedText.Substring(0, slashNullIdx);
            }

            return decryptedText;
        }

        /// <summary>
        /// GetBytesTrimNulls gets a byte[] from binary byte[] data and truncate all 0 byte at the end.
        /// </summary>
        /// <param name="decryptedBytes">decrypted byte[]</param>
        /// <returns>truncated byte[] without a lot of \0 (null) characters</returns>
        public static byte[] GetBytesTrimNulls(byte[] decryptedBytes)
        {
            int ig = -1;
            byte[] decryptedNonNullBytes = null;

            if ((ig = decryptedBytes.ArrayIndexOf((byte)0)) > 0)
            {
                decryptedNonNullBytes = new byte[ig];
                Array.Copy(decryptedBytes, decryptedNonNullBytes, ig);
            }
            else
                decryptedNonNullBytes = decryptedBytes;

            return decryptedNonNullBytes;
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
