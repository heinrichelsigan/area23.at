using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{
    /// <summary>
    /// Ascii85 encoding is a mapping to characters <see cref="VALID_CHARS"/>
    /// <see href="https://en.wikipedia.org/wiki/Ascii85" />
    /// <see href="https://github.com/coding-horror/ascii85"/>
    /// <seealso href="https://github.com/coding-horror/ascii85/blob/master/Ascii85ConsoleSolution/Ascii85.cs" />
    /// </summary>
    public class Ascii85 : IDecodable
    {

        public const string VALID_CHARS = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuz";
        public string PrefixMark = "<~";
        public string SuffixMark = "~>";
        public int LineLength = 75;
        public bool EnforceMarks = false;
        private const int _asciiOffset = 33;
        private byte[] _encodedBlock = new byte[5];
        private byte[] _decodedBlock = new byte[4];
        private uint _tuple = 0;
        private int _linePos = 0;
        private uint[] pow85 = { 85 * 85 * 85 * 85, 85 * 85 * 85, 85 * 85, 85, 1 };

      
        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;

        HashSet<char> IDecodable.ValidCharList { get => new HashSet<char>(VALID_CHARS.ToCharArray()); }


        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public string Encode(byte[] inBytes) => ToAscii85(inBytes);


        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public byte[] Decode(string encodedString) => FromAscii85(encodedString);


        /// <summary>
        /// Checks if a string is a valid encoded string
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>true, when encoding is OK, otherwise false, if encoding contains illegal characters</returns>
        public bool Validate(string encodedString) => Ascii85.IsValidAscii85(encodedString, out _);

        public bool IsValidShowError(string encodedString, out string error) => Ascii85.IsValidAscii85(encodedString, out error);


        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        public static string EnCode(byte[] inBytes) => (new Ascii85()).ToAscii85(inBytes);


        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public static byte[] DeCode(string encodedString) => (new Ascii85()).FromAscii85(encodedString);


        public static bool IsValid(string encodedString) => Ascii85.IsValidAscii85(encodedString, out _);



        /// <summary>
        /// FromAscii85 converts a ascii85 string to a binary byte array
        /// </summary>
        /// <param name="s">ascii85 encoded string</param>
        /// <returns>byte array</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        public byte[] FromAscii85(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));

            if (EnforceMarks)
            {
                if (!s.StartsWith(PrefixMark) | !s.EndsWith(SuffixMark))
                {
                    throw new Exception("ASCII85 encoded data should begin with '" + PrefixMark +
                        "' and end with '" + SuffixMark + "'");
                }
            }

            // strip prefix and suffix if present
            if (s.StartsWith(PrefixMark))
            {
                s = s.Substring(PrefixMark.Length);
            }
            if (s.EndsWith(SuffixMark))
            {
                s = s.Substring(0, s.Length - SuffixMark.Length);
            }

            MemoryStream ms = new MemoryStream();
            int count = 0;
            bool processChar = false;

            foreach (char c in s)
            {
                switch (c)
                {
                    case 'z':
                        if (count != 0)
                        {
                            throw new Exception("The character 'z' is invalid inside an ASCII85 block.");
                        }
                        _decodedBlock[0] = 0;
                        _decodedBlock[1] = 0;
                        _decodedBlock[2] = 0;
                        _decodedBlock[3] = 0;
                        ms.Write(_decodedBlock, 0, _decodedBlock.Length);
                        processChar = false;
                        break;
                    case '\n':
                    case '\r':
                    case '\t':
                    case '\0':
                    case '\f':
                    case '\b':
                        processChar = false;
                        break;
                    default:
                        if (c < '!' || c > 'u')
                        {
                            throw new Exception("Bad character '" + c + "' found. ASCII85 only allows characters '!' to 'u'.");
                        }
                        processChar = true;
                        break;
                }

                if (processChar)
                {
                    _tuple += ((uint)(c - _asciiOffset) * pow85[count]);
                    count++;
                    if (count == _encodedBlock.Length)
                    {
                        DecodeBlock();
                        ms.Write(_decodedBlock, 0, _decodedBlock.Length);
                        _tuple = 0;
                        count = 0;
                    }
                }
            }

            // if we have some bytes left over at the end..
            if (count != 0)
            {
                if (count == 1)
                {
                    throw new Exception("The last block of ASCII85 data cannot be a single byte.");
                }
                count--;
                _tuple += pow85[count];
                DecodeBlock(count);
                for (int i = 0; i < count; i++)
                {
                    ms.WriteByte(_decodedBlock[i]);
                }
            }

            return ms.ToArray();

        }

        /// <summary>
        /// ToAscii85
        /// </summary>
        /// <param name="data">binary data in byte array to convert</param>
        /// <param name="padOutput">block padding with =</param>
        /// <returns>Ascii85 encoded string</returns>
        public string ToAscii85(byte[] data, bool padOutput = true)
        {
            StringBuilder sb = new StringBuilder((int)(data.Length * (_encodedBlock.Length / _decodedBlock.Length)));
            _linePos = 0;

            if (EnforceMarks)
            {
                AppendString(sb, PrefixMark);
            }

            int count = 0;
            _tuple = 0;
            foreach (byte b in data)
            {
                if (count >= _decodedBlock.Length - 1)
                {
                    _tuple |= b;
                    if (_tuple == 0)
                    {
                        AppendChar(sb, 'z');
                    }
                    else
                    {
                        EncodeBlock(sb);
                    }
                    _tuple = 0;
                    count = 0;
                }
                else
                {
                    _tuple |= (uint)(b << (24 - (count * 8)));
                    count++;
                }
            }

            // if we have some bytes left over at the end..
            if (count > 0)
            {
                EncodeBlock(count + 1, sb);
            }

            if (EnforceMarks)
            {
                AppendString(sb, SuffixMark);
            }
            return sb.ToString();
        }

        private void EncodeBlock(StringBuilder sb) => EncodeBlock(_encodedBlock.Length, sb);

        private void EncodeBlock(int count, StringBuilder sb)
        {
            for (int i = _encodedBlock.Length - 1; i >= 0; i--)
            {
                _encodedBlock[i] = (byte)((_tuple % 85) + _asciiOffset);
                _tuple /= 85;
            }

            for (int i = 0; i < count; i++)
            {
                char c = (char)_encodedBlock[i];
                AppendChar(sb, c);
            }

        }

        private void DecodeBlock() => DecodeBlock(_decodedBlock.Length);

        private void DecodeBlock(int bytes)
        {
            for (int i = 0; i < bytes; i++)
            {
                _decodedBlock[i] = (byte)(_tuple >> 24 - (i * 8));
            }
        }

        private void AppendString(StringBuilder sb, string s)
        {
            if (LineLength > 0 && (_linePos + s.Length > LineLength))
            {
                _linePos = 0;
                sb.Append('\n');
            }
            else
            {
                _linePos += s.Length;
            }
            sb.Append(s);
        }

        private void AppendChar(StringBuilder sb, char c)
        {
            sb.Append(c);
            _linePos++;
            if (LineLength > 0 && (_linePos >= LineLength))
            {
                _linePos = 0;
                sb.Append('\n');
            }
        }

        public static bool IsValidAscii85(string inString, out string error)
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