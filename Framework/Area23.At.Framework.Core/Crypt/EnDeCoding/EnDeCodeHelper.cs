using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Area23.At.Framework.Core.Crypt.EnDeCoding
{

    /// <summary>
    // static helper class for Encoding / Decoding
    /// </summary>
    public static class EnDeCodeHelper
    {

        /// <summary>
        /// KeyHexString transforms a private secret key to hex string
        /// </summary>
        /// <param name="key">private secret key</param>
        /// <returns>hex string of bytes</returns>
        public static string KeyToHex(string key)
        {
            byte[] keyBytes = EnDeCodeHelper.GetBytes(key);
            string ivStr = Hex16.ToHex16(keyBytes);
            return ivStr;
        }

        public static string HexToKey(string hexString)
        {
            byte[] keyBytes = Hex16.FromHex16(hexString);
            string key = EnDeCodeHelper.GetString(keyBytes);
            return key;
        }

        /// <summary>
        /// EncodeBytes encodes byte[] inBytes by encodingMethod to an encoded text string
        /// </summary>
        /// <param name="inBytes">inBytes to encdode</param>
        /// <param name="encodingType">EncodingTypes are "None", "Hex16", "Base16", "Base32", "Hex32", "Uu", "Base64".
        /// "Base64" is default.</param>
        /// <param name="fromPlain">Only for uu: true, if <see cref="encryptBytes"/> represent a binary without encryption</param>
        /// <param name="fromFile">Only for uu: true, if file and not textbox will be encrypted, default (false)</param>
        /// <returns>encoded string</returns>
        public static string EncodeBytes(byte[] inBytes, EncodingType encodingType = EncodingType.Base64, bool fromPlain = false, bool fromFile = false)
        {
            SLog.Log(
                "EncodeEncryptedBytes(byte[] inBytes.[Length=" + inBytes.Length + "], EncodingType encodingType =  "
                + encodingType.ToString() + ", bool fromPlain = " + fromPlain + ", bool fromFile = " + fromFile + ")");

            string encryptedText = EnDeCodeHelper.Encode(inBytes, encodingType, fromPlain, fromFile);

            return encryptedText;
        }

        /// <summary>
        /// EncodeBytes encodes byte[] inBytes by encodingMethod as plain text param enCodingString to an encoded text string
        /// </summary>
        /// <param name="inBytes">inBytes to encdode</param>
        /// <param name="enCodingString">ebcoding enum <see cref="EncodingType"/> as plain string
        /// "Base64" is default.</param>
        /// <returns>encoded string</returns>
        public static string EncodeBytesToString(byte[] inBytes, string enCodingString)
        {
            EncodingType encodingType = EncodingTypesExtensions.GetEnum(enCodingString);

            SLog.Log(
                "EncdoeBytes(byte[] inBytes.[Length=" + inBytes.Length + "], EncodingType encodingType =  "
                + encodingType.ToString() + ")");

            string encodedText = EnDeCodeHelper.Encode(inBytes, encodingType);
            return encodedText;
        }

        public static byte[] EncodeBytes(byte[] inBytes, string enCodingString) => Encoding.UTF8.GetBytes(EncodeBytesToString(inBytes, enCodingString));


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
            SLog.Log("EncodedTextToBytes(string cipherText[.Length " + cipherText.Length + "], EncodingType encodingType  = " +
                encodingType.ToString() + ", bool fromPlain = " + fromPlain + ", bool fromFile = " + fromFile + ")");

            // errMsg = string.Empty;
            if (!EnDeCodeHelper.IsValid(cipherText, encodingType))
            {
                // errMsg = $"Input Text is not a valid {encodingType.ToString()} string!";
                throw new FormatException($"Input Text is not a valid {encodingType.ToString()} string!");
            }

            byte[] cipherBytes = cipherBytes = IDecodable.DeCode(cipherText, encodingType);

            return cipherBytes;
        }

        /// <summary>
        /// DecodeText decodes an encoded text string to a <see cref="byte[]">býte array</see>
        /// </summary>
        /// <param name="inText">encoded (encrypted) text string</param>
        /// <param name="enCodingString">ebcoding enum <see cref="EncodingType"/> as plain string
        /// "Base64" is default.</param>
        /// <returns>binary byte array</returns>
        /// <exception cref="FormatException"></exception>
        public static byte[] DecodeText(string inText, string enCodingString)
        {
            EncodingType encodingType = EncodingTypesExtensions.GetEnum(enCodingString);

            SLog.Log(
                string.Format("DecodeText(string inText[.Length {0}], EncodingType encodingType = {1})",
                    inText.Length,
                    encodingType.ToString()));

            // errMsg = string.Empty;
            if (!EnDeCodeHelper.IsValidShowError(inText, out string error, encodingType))
            {
                // errMsg = $"Input Text is not a valid {encodingType.ToString()} string!";
                throw new FormatException($"Input Text is not a valid {encodingType.ToString()} string, invalid chars: {error}");
            }

            byte[] outBytes = IDecodable.DeCode(inText, encodingType);

            return outBytes;
        }

        /// <summary>
        /// DecodeText decodes an encoded text string to a <see cref="byte[]">býte array</see>
        /// </summary>
        /// <param name="inText">encoded (encrypted) text string</param>
        /// <param name="enCodingString">ebcoding enum <see cref="EncodingType"/> as plain string
        /// "Base64" is default.</param>
        /// <returns>binary byte array</returns>>
        /// <returns></returns>
        public static byte[] DecodeBytes(byte[] inBytes, string enCodingString) => DecodeText(Encoding.UTF8.GetString(inBytes), enCodingString);
        


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
            byte[] sourceBytes = EnDeCodeHelper.GetBytes(sourceString);
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
                decryptedText = EnDeCodeHelper.GetString(decryptedNonNullBytes);
            }
            else
                decryptedText = EnDeCodeHelper.GetString(decryptedBytes);

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
            if (inBytes == null || inBytes.Length == 0)
                return null;

            if ((ig = inBytes.ArrayIndexOf((byte)0)) < 1)
                return inBytes;

            byte[] nonNullBytes = new byte[ig + 1];
            Array.Copy(inBytes, 0, nonNullBytes, 0, ig + 1);

            return nonNullBytes;
        }

        /// <summary>
        /// GetBytesTrimNulls gets a byte[] from binary byte[] data and truncate all 0 byte at the end.
        /// </summary>
        /// <param name="inBytes">decrypted byte[]</param>
        /// <returns>truncated byte[] without a lot of \0 (null) characters</returns>
        public static byte[] GetBytesTrimCrLfNulls(byte[] inBytes)
        {            
            if (inBytes == null || inBytes.Length < 1)
                return new byte[0];

            int ig = inBytes.Length;
            byte[] nonNullBytes = new byte[inBytes.Length];            
            
            while (ig > 0 && ((inBytes[ig - 1] == (byte)0) || (inBytes[ig-1] == (byte)10) || (inBytes[ig-1] == (byte)13)))
            {
                ig--;
            }
            nonNullBytes = new byte[ig];
            Array.Copy(inBytes, 0, nonNullBytes, 0, ig);                            
            
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


        public static string Encode(byte[] inBytes, EncodingType encodingType = EncodingType.Base64, bool fromPlain = false, bool fromFile = false)
        {
            IDecodable enc = encodingType.GetEnCoder();
            if (encodingType == EncodingType.Uu)
                return Uu.Encode(inBytes, fromPlain, fromFile);
            return enc.Encode(inBytes);

        }

        public static byte[] Decode(string encodedString, EncodingType encodingType = EncodingType.Base64)
        {
            IDecodable dec = encodingType.GetEnCoder();
            return dec.Decode(encodedString);
        }

        public static bool IsValid(string encodedString, EncodingType encodingType = EncodingType.Base64)
        {
            IDecodable dec = encodingType.GetEnCoder();
            return dec.IsValid(encodedString);
        }

        public static bool IsValidShowError(string encodedString, out string error, EncodingType encodingType = EncodingType.Base64)
        {
            IDecodable dec = encodingType.GetEnCoder();
            return dec.IsValidShowError(encodedString, out error);
        }


        public static string GetString(byte[] data)
        {
            return System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public static byte[] GetBytes(string str2encode)
        {
            return System.Text.Encoding.UTF8.GetBytes(str2encode);
        }


        public static int GetByteCount(string str)
        {
            return System.Text.Encoding.UTF8.GetByteCount(str);
        }


        #region GetString 


        public static string GetStringDefault(byte[] data)
        {
            return Encoding.Default.GetString(data, 0, data.Length);
        }

        public static string GetStringASCII(byte[] data)
        {
            return Encoding.ASCII.GetString(data, 0, data.Length);
        }

        public static string GetString7(byte[] data)
        {
            return Encoding.UTF7.GetString(data, 0, data.Length);
        }

        public static string GetString8(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public static string GetString16(byte[] data)
        {
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }

        public static string GetString32(byte[] data)
        {
            return Encoding.UTF32.GetString(data, 0, data.Length);
        }
        #endregion GetString 

        #region GetBytes

        public static byte[] GetBytesDefault(string str2encode) => Encoding.Default.GetBytes(str2encode);

        public static byte[] GetBytesASCII(string str2encode) => Encoding.ASCII.GetBytes(str2encode);

        public static byte[] GetBytes7(string str2encode) => Encoding.UTF7.GetBytes(str2encode);

        public static byte[] GetBytes8(string str2encode)
        {
            return Encoding.UTF8.GetBytes(str2encode);
        }

        public static byte[] GetBytes16(string str2encode)
        {
            return Encoding.Unicode.GetBytes(str2encode);
        }

        public static byte[] GetBytes32(string str2encode)
        {
            return Encoding.UTF32.GetBytes(str2encode);
        }

        #endregion GetBytes

        #region GetByteCount

        public static int GetByteCountDefault(string str)
        {
            return Encoding.Default.GetByteCount(str);
        }

        public static int GetByteCountASCII(string str)
        {
            return Encoding.ASCII.GetByteCount(str);
        }

        public static int GetByteCount7(string str)
        {
            return Encoding.UTF7.GetByteCount(str);
        }

        public static int GetByteCount8(string str)
        {
            return Encoding.UTF8.GetByteCount(str);
        }

        public static int GetByteCount16(string str)
        {
            return Encoding.Unicode.GetByteCount(str);
        }

        public static int GetByteCount32(string str)
        {
            return Encoding.UTF32.GetByteCount(str);
        }

        public static string Encode(byte[] inBytes)
        {
            throw new NotImplementedException();
        }

        public static byte[] Decode(string encodedString)
        {
            throw new NotImplementedException();
        }

        #endregion GetByteCount

    }

}
