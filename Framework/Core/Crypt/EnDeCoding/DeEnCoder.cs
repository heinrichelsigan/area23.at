using Area23.At.Framework.Core.Util;
using DBTek.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Crypt.EnDeCoding
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
        public static string KeyToHex(string key)
        {
            byte[] keyBytes = EnDeCoder.GetBytes(key);
            string ivStr = Hex16.Encode(keyBytes);
            return ivStr;
        }

        public static string HexToKey(string hexString)
        {
            byte[] keyBytes = Hex16.Decode(hexString);
            string key = EnDeCoder.GetString(keyBytes);
            return key;
        }

        /// <summary>
        /// EncodeBytes encodes encrypted byte[] by encodingMethod to an encoded text string
        /// </summary>
        /// <param name="encryptBytes">encryptedBytes to encdode</param>
        /// <param name="encodingType">EncodingTypes are "None", "Hex16", "Base16", "Base32", "Hex32", "Uu", "Base64".
        /// "Base64" is default.</param>
        /// <param name="fromPlain">Only for uu: true, if <see cref="encryptBytes"/> represent a binary without encryption</param>
        /// <param name="fromFile">Only for uu: true, if file and not textbox will be encrypted, default (false)</param>
        /// <returns>encoded encrypted string</returns>
        public static string EncodeBytes(byte[] encryptBytes, EncodingType encodingType = EncodingType.Base64, bool fromPlain = false, bool fromFile = false)
        {
            Area23Log.LogStatic(
                "EncodeEncryptedBytes(byte[] encryptBytes.[Length=" + encryptBytes.Length + "], EncodingType encodingType =  "
                + encodingType.ToString() + ", bool fromPlain = " + fromPlain + ", bool fromFile = " + fromFile + ")");

            string encryptedText = EnDeCoder.Encode(encryptBytes, encodingType, fromPlain, fromFile);

            return encryptedText;
        }

        /// <summary>
        /// EncodedTextToBytes transforms an encoded text string into a <see cref="byte[]">býte array</see>
        /// </summary>
        /// <param name="cipherText">encoded (encrypted) text string</param>
        /// <param name="encodingType"><see cref="EncodingType"/> could be 
        /// "None", "Hex16", "Base16", "Base32", "Hex32", "Uu", "Base64". "Base64" is default.</param>
        /// <param name="fromPlain">Only for uu: true, if <see cref="encryptBytes"/> represent a binary without encryption</param>
        /// <param name="fromFile">Only for uu: true, if file and not textbox will be encrypted, default (false)</param>
        /// <returns>binary byte array</returns>
        public static byte[] DecodeText(string cipherText, /* out string errMsg, */ EncodingType encodingType = EncodingType.Base64, bool fromPlain = false, bool fromFile = false)
        {
            Area23Log.LogStatic("EncodedTextToBytes(string cipherText[.Length " + cipherText.Length + "], EncodingType encodingType  = " +
                encodingType.ToString() + ", bool fromPlain = " + fromPlain + ", bool fromFile = " + fromFile + ")");

            // errMsg = string.Empty;
            if (!EnDeCoder.IsValid(cipherText, encodingType))
            {
                // errMsg = $"Input Text is not a valid {encodingType.ToString()} string!";
                throw new FormatException($"Input Text is not a valid {encodingType.ToString()} string!");
            }

            byte[] cipherBytes = cipherBytes = EnDeCoder.Decode(cipherText, encodingType, fromPlain, fromFile);

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
            byte[] sourceBytes = EnDeCoder.GetBytes(sourceString);
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
                decryptedText = EnDeCoder.GetString(decryptedNonNullBytes);
            }
            else
                decryptedText = EnDeCoder.GetString(decryptedBytes);

            if (decryptedText.Contains('\0'))
            {
                int slashNullIdx = decryptedText.IndexOf('\0');
                decryptedText = decryptedText.Substring(0, slashNullIdx);
            }

            return decryptedText;
        }

        public static byte[] GetBytesFromBytes(byte[] inBytes, int blockSize = 64, bool upStretchToCorrectBlockSize = true)
        {
            if (!upStretchToCorrectBlockSize)
                return inBytes;
            int addByteLen = blockSize - (inBytes.Length % blockSize);
            List<byte> outBytes = new List<byte>(inBytes);
            while (outBytes.Count % blockSize != 0)
            {
                outBytes.Add((byte)0);
            }
            return outBytes.ToArray();
        }

        /// <summary>
        /// GetBytesTrimNulls gets a byte[] from binary byte[] data and truncate all 0 byte at the end.
        /// </summary>
        /// <param name="inBytes">decrypted byte[]</param>
        /// <returns>truncated byte[] without a lot of \0 (null) characters</returns>
        public static byte[] GetBytesTrimNulls(byte[] inBytes)
        {
            int ig = -1;
            if ((ig = inBytes.ArrayIndexOf((byte)0)) < 1)
                return inBytes;

            byte[] nonNullBytes = new byte[ig + 1];
            Array.Copy(inBytes, 0, nonNullBytes, 0, ig + 1);

            return nonNullBytes;
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
