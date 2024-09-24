using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Area23.At.Framework.Library.Core.EnDeCoding
{
    /// <summary>
    /// Base64 mime standard encoding
    /// </summary>
    public static class Base64
    {
        public static string ToBase64(byte[] inBytes)
        {
            string os = Convert.ToBase64String(inBytes, 0, inBytes.Length, Base64FormattingOptions.None);
            return os;
        }

        public static byte[] FromBase64(string inString)
        {
            byte[] outBytes = Convert.FromBase64String(inString);
            return outBytes;
        }

    }
}