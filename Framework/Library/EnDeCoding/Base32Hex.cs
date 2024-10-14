using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Area23.At.Framework.Library.EnDeCoding
{
    /// <summary>
    /// Base32Hex encoding is a mapping for double hex from 0-9A-V (32 chiffers per digit), padding char is =
    /// <see href="https://datatracker.ietf.org/doc/html/rfc4648#section-7" />
    /// </summary>
    public static class Base32Hex
    {
        private static readonly char[] _digits = "0123456789ABCDEFGHIJKLMNOPQRSTUV=".ToCharArray();
        private static List<char> ValidCharList = new List<char>(_digits);
        private const int _mask = 31;
        private const int _shift = 5;

        private static int CharToInt(char c)
        {
            int iBigA = (int)'A', iLittleA = ((int)'a'), iZero = (int)'0', iChar = ((int)c);

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
        /// FromBase32Hex converts a base32 string to a binary byte array
        /// </summary>
        /// <param name="encoded">base32 encoded string</param>
        /// <returns>byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        public static byte[] FromBase32Hex(string encoded)
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
        /// ToBase32
        /// </summary>
        /// <param name="data">binary data in byte array to convert</param>
        /// <param name="padOutput">block padding with =</param>
        /// <returns>Base32 encoded string</returns>
        public static string ToBase32Hex(byte[] data, bool padOutput = true)
        {
            return ToBase32Hex(data, 0, data.Length, padOutput);
        }

        public static string ToBase32Hex(byte[] data, int offset, int length, bool padOutput = true)
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
                result.Append(_digits[index]);
            }

            if (padOutput)
            {
                int padding = 8 - (result.Length % 8);
                if (padding > 0) result.Append('=', padding == 8 ? 0 : padding);
            }

            return result.ToString();
        }

        public static bool IsValidBase32Hex(string inString)
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