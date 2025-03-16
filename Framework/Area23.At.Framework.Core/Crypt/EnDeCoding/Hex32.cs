using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Area23.At.Framework.Core.Crypt.EnDeCoding
{
    /// <summary>
    /// Hex32 encoding is a mapping for double hex from 0-9A-V (32 chiffers per digit), padding char is =
    /// <see href="https://datatracker.ietf.org/doc/html/rfc4648#section-7" />
    /// </summary>
    public class Hex32 : IDecodable
    {

        public const string VALID_CHARS =  "0123456789ABCDEFGHIJKLMNOPQRSTUV=";                
        private const int _mask = 31;
        private const int _shift = 5;

        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;

        public static HashSet<char>? ValidCharList { get; private set; } = new HashSet<char>(VALID_CHARS.ToCharArray());


        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public string Encode(byte[] inBytes) => Hex32.ToHex32(inBytes);
        
        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public byte[] Decode(string encodedString) => Hex32.FromHex32(encodedString);


        public bool IsValid(string encodedStr) => Hex32.IsValidHex32(encodedStr, out _);

        public bool IsValidShowError(string encodedString, out string error) => Hex32.IsValidHex32(encodedString, out error);
        

        #endregion common interface, interfaces for static members appear in C# 7.3 or later

        private static int CharToInt(char c)
        {
            int iBigA = (int)'A', iLittleA = ((int)'a'), iChar = ((int)c);

            if (Char.IsUpper(c))
                return ((iChar - iBigA) + 10);
            else if (Char.IsLower(c))
                return ((iChar - iLittleA) + 10);
            else if (Char.IsDigit(c) || Char.IsNumber(c))
                switch (c)
                {
                    case '0': return 0;
                    case '1': return 1;
                    case '2': return 2;
                    case '3': return 3;
                    case '4': return 4;
                    case '5': return 5;
                    case '6': return 6;
                    case '7': return 7;
                    case '8': return 8;
                    case '9': return 9;
                    default: break;
                }

            return -1;
        }

        /// <summary>
        /// FromHex32 converts a base32 string to a binary byte array
        /// </summary>
        /// <param name="encoded">base32 encoded string</param>
        /// <returns>byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        public static byte[] FromHex32(string encoded)
        {
            if (encoded == null)
                throw new ArgumentNullException(nameof(encoded));

            // Remove whitespace and padding. Note: the padding is used as hint 
            // to determine how many bits to decode from the last incomplete chunk
            // Also, canonicalize to all upper case
            encoded = encoded.Trim().TrimEnd('=').ToUpper();
            if (encoded.Length == 0)
                return new byte[0];

            var outLength = encoded.Length * _shift / 8;
            var result = new byte[outLength];
            var buffer = 0;
            var next = 0;
            var bitsLeft = 0;
            var charValue = 0;
            foreach (var c in encoded)
            {
                charValue = CharToInt(c);
                if (charValue < 0)
                    throw new FormatException("Illegal character: `" + c + "`");

                buffer <<= _shift;
                buffer |= charValue & _mask;
                bitsLeft += _shift;
                if (bitsLeft >= 8)
                {
                    result[next++] = (byte)(buffer >> (bitsLeft - 8));
                    bitsLeft -= 8;
                }
            }

            return result;
        }

        /// <summary>
        /// ToHex32
        /// </summary>
        /// <param name="data">binary data in byte array to convert</param>
        /// <param name="padOutput">block padding with =</param>
        /// <returns>Base32 encoded string</returns>
        public static string ToHex32(byte[] data, bool padOutput = true)
        {
            return ToHex32(data, 0, data.Length, padOutput);
        }

        public static string ToHex32(byte[] data, int offset, int length, bool padOutput = true)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            if ((offset + length) > data.Length)
                throw new ArgumentOutOfRangeException();

            if (length == 0)
                return "";

            // SHIFT is the number of bits per output character, so the length of the
            // output is the length of the input multiplied by 8/SHIFT, rounded up.
            // The computation below will fail, so don't do it.
            if (length >= (1 << 28))
                throw new ArgumentOutOfRangeException(nameof(data));

            var outputLength = (length * 8 + _shift - 1) / _shift;
            var result = new StringBuilder(outputLength);

            var last = offset + length;
            int buffer = data[offset++];
            var bitsLeft = 8;
            while (bitsLeft > 0 || offset < last)
            {
                if (bitsLeft < _shift)
                {
                    if (offset < last)
                    {
                        buffer <<= 8;
                        buffer |= (data[offset++] & 0xff);
                        bitsLeft += 8;
                    }
                    else
                    {
                        int pad = _shift - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }
                int index = _mask & (buffer >> (bitsLeft - _shift));
                bitsLeft -= _shift;
                result.Append(VALID_CHARS[index]);
            }

            if (padOutput)
            {
                int padding = 8 - (result.Length % 8);
                if (padding > 0) result.Append('=', padding == 8 ? 0 : padding);
            }

            return result.ToString();
        }

        public static bool IsValidHex32(string inString, out string error)
        {
            bool valid = true;
            error = "";
            foreach (char ch in inString)
            {
                if (!VALID_CHARS.ToCharArray().Contains(ch))
                {
                    error += ch.ToString();
                    valid = false;
                }                    
            }
            return valid;
        }

    }

}