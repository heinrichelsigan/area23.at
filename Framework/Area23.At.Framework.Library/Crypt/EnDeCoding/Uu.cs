using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{


    /// <summary>
    /// Uu is unix2unix uuencode uudecode
    /// Thanks to <see href="https://github.com/n3wt0n/Crypto/blob/master/DBTek.Crypto.Shared/UUEncoder.cs" />
    /// Thanks to <see href="https://rextester.com/TGN19503" />
    /// Thanks to <see href="https://ssojet.com/binary-encoding-decoding/uuencoding-in-c/" />
    /// </summary>
    public class Uu : IDecodable
    {

        #region const or static readonly fields
        public static readonly object _lock = new object();
        public const string VALID_CHARS = "!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_` \r\n";

        static string invalidChars = "";

        static readonly byte[] UUEncMap = new byte[]
        {
          0x60, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F,
          0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
          0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x4B, 0x4C, 0x4D, 0x4E, 0x4F,
          0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x5B, 0x5C, 0x5D, 0x5E, 0x5F
        };

        static readonly byte[] UUDecMap = new byte[]
        {
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
          0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
          0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x1B, 0x1C, 0x1D, 0x1E, 0x1F,
          0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x2B, 0x2C, 0x2D, 0x2E, 0x2F,
          0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
          0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };


        #endregion const or static readonly fields

        //public static bool IsUnix { get => (Path.DirectorySeparatorChar == '/'); }
        //public static bool IsWindows { get => (Path.DirectorySeparatorChar == '\\'); }


        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;

        public HashSet<char> ValidCharList => ValidCharSet;

        public static HashSet<char> ValidCharSet => (new HashSet<char>(VALID_CHARS.ToCharArray()));

        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public string EnCode(byte[] data) => Uu.ToUu(data);

        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public byte[] DeCode(string encodedString) => Uu.FromUu(encodedString);

        public bool IsValidShowError(string encodedString, out string error) => Uu.IsValidUu(encodedString, out error);

        public bool IsValid(string encodedString) => Uu.IsValidUu(encodedString, out _);

        public bool Validate(string encodedString) => Uu.IsValidUu(encodedString, out _);

        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        public static void UUDecode(System.IO.Stream input, System.IO.Stream output)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (output == null)
                throw new ArgumentNullException("output");

            long len = input.Length;
            if (len == 0)
                return;

            long didx = 0;
            int nextByte = input.ReadByte();
            while (nextByte >= 0)
            {
                // get line length (in number of encoded octets)
                int line_len = UUDecMap[nextByte];

                // ascii printable to 0-63 and 4-byte to 3-byte conversion
                long end = didx + line_len;
                byte u0, u1, u2, u3;
                if (end > 2)
                {
                    while (didx < end - 2)
                    {
                        u0 = UUDecMap[input.ReadByte()];
                        u1 = UUDecMap[input.ReadByte()];
                        u2 = UUDecMap[input.ReadByte()];
                        u3 = UUDecMap[input.ReadByte()];

                        output.WriteByte((byte)(((u0 << 2) & 255) | ((u1 >> 4) & 3)));
                        output.WriteByte((byte)(((u1 << 4) & 255) | ((u2 >> 2) & 15)));
                        output.WriteByte((byte)(((u2 << 6) & 255) | (u3 & 63)));
                        didx += 3;
                    }
                }

                if (didx < end)
                {
                    u0 = UUDecMap[input.ReadByte()];
                    u1 = UUDecMap[input.ReadByte()];
                    output.WriteByte((byte)(((u0 << 2) & 255) | ((u1 >> 4) & 3)));
                    didx++;

                    if (didx < end)
                    {
                        u2 = UUDecMap[input.ReadByte()];
                        output.WriteByte((byte)(((u1 << 4) & 255) | ((u2 >> 2) & 15)));
                        didx++;
                    }
                }

                // skip padding
                do
                {
                    nextByte = input.ReadByte();
                }
                while (nextByte >= 0 && nextByte != '\n' && nextByte != '\r');

                // skip end of line
                do
                {
                    nextByte = input.ReadByte();
                }
                while (nextByte >= 0 && (nextByte == '\n' || nextByte == '\r'));
            }
        }

        public static void UUEncode(System.IO.Stream input, System.IO.Stream output)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (output == null)
                throw new ArgumentNullException("output");

            long len = input.Length;
            if (len == 0)
                return;

            int sidx = 0;
            int line_len = 45;
            byte[] nl = Encoding.ASCII.GetBytes(Environment.NewLine);

            byte u0, u1, u2;
            // split into lines, adding line-length and line terminator
            while (sidx + line_len < len)
            {
                // line length
                output.WriteByte(UUEncMap[line_len]);

                // 3-byte to 4-byte conversion + 0-63 to ascii printable conversion
                for (int end = sidx + line_len; sidx < end; sidx += 3)
                {
                    u0 = (byte)input.ReadByte();
                    u1 = (byte)input.ReadByte();
                    u2 = (byte)input.ReadByte();

                    output.WriteByte(UUEncMap[(u0 >> 2) & 63]);
                    output.WriteByte(UUEncMap[(u1 >> 4) & 15 | (u0 << 4) & 63]);
                    output.WriteByte(UUEncMap[(u2 >> 6) & 3 | (u1 << 2) & 63]);
                    output.WriteByte(UUEncMap[u2 & 63]);
                }

                // line terminator
                for (int idx = 0; idx < nl.Length; idx++)
                    output.WriteByte(nl[idx]);
            }

            // line length
            output.WriteByte(UUEncMap[len - sidx]);

            // 3-byte to 4-byte conversion + 0-63 to ascii printable conversion
            while (sidx + 2 < len)
            {
                u0 = (byte)input.ReadByte();
                u1 = (byte)input.ReadByte();
                u2 = (byte)input.ReadByte();

                output.WriteByte(UUEncMap[(u0 >> 2) & 63]);
                output.WriteByte(UUEncMap[(u1 >> 4) & 15 | (u0 << 4) & 63]);
                output.WriteByte(UUEncMap[(u2 >> 6) & 3 | (u1 << 2) & 63]);
                output.WriteByte(UUEncMap[u2 & 63]);
                sidx += 3;
            }

            if (sidx < len - 1)
            {
                u0 = (byte)input.ReadByte();
                u1 = (byte)input.ReadByte();

                output.WriteByte(UUEncMap[(u0 >> 2) & 63]);
                output.WriteByte(UUEncMap[(u1 >> 4) & 15 | (u0 << 4) & 63]);
                output.WriteByte(UUEncMap[(u1 << 2) & 63]);
                output.WriteByte(UUEncMap[0]);
            }
            else if (sidx < len)
            {
                u0 = (byte)input.ReadByte();

                output.WriteByte(UUEncMap[(u0 >> 2) & 63]);
                output.WriteByte(UUEncMap[(u0 << 4) & 63]);
                output.WriteByte(UUEncMap[0]);
                output.WriteByte(UUEncMap[0]);
            }

            // line terminator
            for (int idx = 0; idx < nl.Length; idx++)
                output.WriteByte(nl[idx]);
        }

        /// <summary>
        /// ToUu
        /// </summary>
        /// <param name="inBytes">binary byte array</param>
        /// <returns>uuencoded string</returns>
        public static string ToUu(byte[] inBytes)
        {
            string toUuFunCall = $"ToUu(byte[{inBytes.Length}] inBytes)";
            Area23Log.LogOriginMsg("Uu", $"{toUuFunCall} ... STARTED.");
            MemoryStream outStream = new MemoryStream();
            UUEncode(new MemoryStream(inBytes), outStream);
            outStream.Flush();

            return Encoding.UTF8.GetString(outStream.ToArray());
        }

        /// <summary>
        /// FromUu
        /// </summary>
        /// <param name="uuEncStr">uuencoded string</param>
        /// <returns>binary byte array</returns>
        public static byte[] FromUu(string uuEncStr)
        {
            string fromUuFunCall = "FromUu(string uuEncStr[.Length=" + uuEncStr.Length + "])";
            Area23Log.LogOriginMsg("Uu", fromUuFunCall + "... STARTED.");

            lock (_lock)
            {
                if (!string.IsNullOrEmpty(invalidChars))
                {
                    for (int i = 0; i < invalidChars.Length; i++)
                        uuEncStr = uuEncStr.Replace(((char)invalidChars[i]).ToString(), "");

                    invalidChars = "";
                }
                                
            }

            MemoryStream inStream = new MemoryStream(), outStream = new MemoryStream();
            var writer = new StreamWriter(inStream);
            writer.Write(uuEncStr);
            writer.Flush();
            inStream.Position = 0;

            UUDecode(inStream, outStream);
            outStream.Flush();
            byte[] plainBytes = outStream.ToArray();

            Area23Log.LogOriginMsg("Uu", $"byte[{plainBytes.Length}] plainBytes = FromUu(string uuEncStr) ... FINISHED.");
            return plainBytes;
        }

        /// <summary>
        /// UuEncode unix 2 unix encodes a string
        /// </summary>
        /// <param name="plainText">plain text string to encode</param>
        /// <returns>uuencoded string</returns>
        public static string UuEncode(string plainText)
        {
            string uuEncodedString = ToUu(Encoding.UTF8.GetBytes(plainText));
            return uuEncodedString;
        }

        /// <summary>
        /// UuDecode unix 2 unix decodes a string
        /// </summary>
        /// <param name="uuEncodedStr">uuencoded string</param>
        /// <returns>uudecoded plain text</returns>
        public static string UuDecode(string uuEncodedStr)
        {
            byte[] uuDecBytes = FromUu(uuEncodedStr);
            string uuDecString = Encoding.UTF8.GetString(uuDecBytes);
            return uuDecString;
        }

        public static bool IsValidUu(string uuEncodedStr, out string error)
        {
            string encodedBody = uuEncodedStr;
            bool isValid = true;
            error = "";


            if (ValidCharSet != null && ValidCharSet.Count > 0)
            {
                if (uuEncodedStr.StartsWith("begin"))
                {
                    encodedBody = uuEncodedStr.GetSubStringByPattern("begin", true, "", "\n", false);
                    if (encodedBody.Contains("\nend") || encodedBody.Contains("end"))
                    {
                        encodedBody = encodedBody.GetSubStringByPattern("end", false, "", "", true);
                    }
                }

                foreach (char ch in uuEncodedStr)
                {
                    if (!ValidCharSet.Contains(ch))
                    {
                        if (((int)ch) >= 256)
                        {
                            Area23Log.LogOriginMsg("Uu", $"illegal high (char){(long)ch} \'{ch}\'");
                            invalidChars += ch.ToString();
                        }
                        else
                        {
                            error += ch.ToString();
                            Area23Log.LogOriginMsg("Uu", $"illegal char \'{ch}\'");
                            isValid = false;
                            return false;
                        }
                    }
                }
                
            }

            return isValid;
        }

    }

}