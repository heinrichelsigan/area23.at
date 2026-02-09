using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Zfx;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace Area23.At.Framework.Library.Static
{

    /// <summary>
    /// Extension methods for Area23.At.Mono
    /// </summary>
    public static class Extensions
    {

        #region primitive types extensions

        /// <summary>
        /// <see cref="double"/>.IsRoundNumber() extension methods: checks, if a double is a round number
        /// </summary>
        /// <param name="d">double to check</param>
        /// <returns>true, if it's integer number</returns>
        public static bool IsRoundNumber(this double d)
        {
            return (Math.Truncate(d) == d || Math.Round(d) == d);
        }

        /// <summary>
        /// <see cref="double"/>.ToLong() extension methods: converts a double to a long <see cref="Int64"/>
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static long ToLong(this double d)
        {
            return Convert.ToInt64(d);
        }

        /// <summary>
        /// <see cref="double"/>.IsNan() extension methods: checks, if a double is not a number
        /// </summary>
        /// <param name="d">double to check</param>
        /// <returns>true, if dounble is not a number, otherwise false</returns>
        public static bool IsNan(this double d)
        {
            return double.IsNaN(d);
        }

        #endregion primitive types extensions

        #region DateTime extensions

        /// <summary>
        /// <see cref="DateTime"/>.Area23Date() extension method: formats <see cref="DateTime"/>.ToString("yyyy-MM-dd")
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/></param>
        /// <returns>formatted date <see cref="string"/></returns>
        public static string Area23Date(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// <see cref="DateTime"/>.Area23DateTime() extension method: formats <see cref="DateTime"/>.ToString("yyyy-MM-dd HH:mm")
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/></param>
        /// <returns>formatted date time <see cref="string"/> </returns>
        public static string Area23DateTime(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy") + Constants.DATE_DELIM +
                DateTime.UtcNow.ToString("MM") + Constants.DATE_DELIM +
                DateTime.UtcNow.ToString("dd") + Constants.WHITE_SPACE +
                DateTime.UtcNow.ToString("HH") + Constants.ANNOUNCE +
                DateTime.UtcNow.ToString("mm") + Constants.ANNOUNCE + Constants.WHITE_SPACE;
        }

        /// <summary>
        /// <see cref="DateTime"/>.Area23DateTimeWithSeconds() extension method: formats <see cref="DateTime"/>.ToString("yyyy-MM-dd_HH:mm:ss")
        /// </summary>
        /// <param name="dateTime">d</param>
        /// <returns><see cref="string"/> formatted date time including seconds</returns>
        public static string Area23DateTimeWithSeconds(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd_HH:mm:ss");
        }

        /// <summary>
        /// <see cref="DateTime"/>.Area23DateTimeWithMillis() extension method: formats <see cref="DateTime"/>.ToString("yyyyMMdd_HHmmss_milis")
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/></param>
        /// <returns>formatted date time <see cref="string"/> </returns>
        public static string Area23DateTimeWithMillis(this DateTime dateTime)
        {
            string formatted = String.Format("{0:yyyy-MM-dd_HH.mm.ss.fff}", dateTime);
            // return formatted;
            return formatted;
        }

        public static string Area23DateTimeMilliseconds(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        #endregion DateTime extensions

        #region stream_byteArray_string_extensions

        #region stream_extensions

        /// <summary>
        /// <see cref="System.IO.Stream"/>.ToByteArray() extension method: converts <see cref="System.IO.Stream"/> to <see cref="byte[]"/> array
        /// </summary>
        /// <param name="stream"><see cref="System.IO.Stream"/> which static methods are now extended</param>
        /// <returns>binary <see cref="byte[]">byte[] array</see></returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            if (stream is MemoryStream)
                return ((MemoryStream)stream).ToArray();
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        #endregion stream_extensions

        #region byteArray_extensions

        /// <summary>
        /// <see cref="byte[]"/>.GetImageMimeType() extension method: auto detect mime type of an image inside an binary byte[] array
        /// via <see cref="ImageCodecInfo.GetImageEncoders()"/> <seealso cref="ImageCodecInfo.GetImageDecoders()"/>
        /// </summary>
        /// <param name="bytes">binary <see cref="byte[]">byte[] array</see></param>
        /// <returns></returns>
        public static string GetImageMimeType(this byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (Image img = Image.FromStream(ms))
                {
                    return ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == img.RawFormat.Guid).MimeType;
                }
            }
        }

        /// <summary>
        /// <see cref="byte[]"/>.ArrayIndexOf(byte value) extension method: gets the first index of specified byte value
        /// </summary>
        /// <param name="bytes">byte[] to search</param>
        /// <param name="value">byte to find</param>
        /// <returns>index in array if found, otherwise -1</returns>
        public static int ArrayIndexOf(this byte[] bytes, byte value)
        {
            for (int bCnt = bytes.Length - 1; bCnt >= 0; bCnt--)
            {
                if (bytes[bCnt] != value)
                {
                    return bCnt;
                }
            }
            return -1;
        }

        /// <summary>
        /// <see cref="byte[]"/>.ToFile(string filePath, string fileName, string fext) extension method: writes a byte array to a file
        /// </summary>
        /// <param name="bytes"><see cref="byte[]"/></param>
        /// <param name="filePath">filesystem path</param>
        /// <param name="fileName">filename</param>
        /// <param name="fext">file extension</param>
        /// <returns>full file system path to new written file in case of success, on error simply null</returns>
        public static string ToFile(this byte[] bytes, string filePath = null, string fileName = null, string fext = null)
        {
            if (string.IsNullOrEmpty(filePath) || !Directory.Exists(filePath))
                filePath = LibPaths.SystemDirPath;
            if (!filePath.EndsWith(LibPaths.SepChar))
                filePath += LibPaths.SepChar;

            if (string.IsNullOrEmpty(fileName))
                fileName = DateTime.UtcNow.Area23DateTimeWithMillis();

            if (string.IsNullOrEmpty(fext) || fext.Length < 2)
            {
                fileName += "_" + Guid.NewGuid().ToString();
                fext = ".tmp"; //                
            }
            else if (!fext.StartsWith("."))
                fext = "." + fext;

            string fullFileName = filePath + fileName + fext;

            try
            {
                using (var fs = new FileStream(fullFileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("Extension", "ToFile(...) throwed Exception " + ex.GetType(),  ex);
            }       

            if (File.Exists(fullFileName))
            {
                return fullFileName;
            }

            return null;
        }
        
        
        /// <summary>
        /// <see cref="byte[]"/>.ToHexString() extension method: converts byte[] to HexString
        /// </summary>
        /// <param name="bytes">Array of <see cref="byte"/></param>
        /// <param name="UPPERCASE">bool default true for UPPERCASE hex letters ABCDEF, false for lower case letters abcdef</param>
        /// <returns>hexadecimal string</returns>
        public static string ToHexString(this byte[] bytes, bool UPPERCASE = true)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(((UPPERCASE) ? b.ToString("X2") : b.ToString("x2")));
            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }


        /// <summary>
        /// <see cref="byte[]"/>.FindBytes extension method: searches hayStack for the first occurence of needle, 
        /// FindBytes uses static equivalent <see cref="BytesBytes(byte[], byte[], int)"/> 
        /// </summary>
        /// <param name="hayStack">byte[] of haystack to search through</param>
        /// <param name="needle">byte[] of needle to find</param>        
        /// <param name="matchBytes">match the only first matchBytes of needle, -1 for all bytes</param>
        /// <returns>index of first byte of matching needle in haystack</returns>
        public static int FindBytes(this byte[] hayStack, byte[] needle, int matchBytes = -1)
        {
            return BytesBytes(hayStack, needle, matchBytes);
        }

        /// <summary>
        /// BytesBytes static method: searches hayStack for the first occurence of needle, 
        /// BytesBytes was inspired by unix posix c function strstr 
        /// </summary>
        /// <param name="hayStack">byte[] of haystack to search through</param>
        /// <param name="needle">byte[] of needle to find</param>        
        /// <param name="matchBytes">match the only first matchBytes of needle, -1 for all bytes</param>
        /// <returns>index of first byte of matching needle in haystack</returns>
        public static int BytesBytes(byte[] hayStack, byte[] needle, int matchBytes = -1)
        {
            if (needle == null || needle.Length == 0 || hayStack == null || hayStack.Length == 0 || needle.Length > hayStack.Length)
                return -1;

            int needleIt = 0;
            for (int fFwdIt = 0; fFwdIt < hayStack.Length - needle.Length; fFwdIt++)
            {
                if (hayStack[fFwdIt] == needle[needleIt])
                {
                    if (needle.Length == 1)
                        return fFwdIt;

                    for (needleIt = 1; needleIt < needle.Length; needleIt++)
                    {
                        if (hayStack[fFwdIt + needleIt] != needle[needleIt])
                        {
                            needleIt = 0;
                            break;
                        }
                        if (matchBytes > 0 && needleIt == matchBytes)
                            return fFwdIt;

                        if (needleIt >= needle.Length - 1)
                            return fFwdIt;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// <see cref="byte[]"/>.TarBytes extension method: tars 
        /// </summary>
        /// <param name="baseBytes">base byte array</param>
        /// <param name="bytesToAdd">more byte arrays</param>
        /// <returns>large tared byte array</returns>
        public static byte[] TarBytes(this byte[] baseBytes, params byte[][] bytesToAdd)
        {
            List<byte> largeBytesList = new List<byte>(baseBytes);

            foreach (byte[] bs in bytesToAdd)
            {
                largeBytesList.AddRange(bs);
            }

            return largeBytesList.ToArray();
        }

        /// <summary>
        /// TarBytes static method: tars all parameters of bytes array to one large byte array
        /// </summary>
        /// <param name="bytesToAdd">one up to many byte arrays</param>
        /// <returns>large tared byte array</returns>
        public static byte[] TarBytes(params byte[][] bytesToAdd)
        {
            List<byte> largeBytesList = new List<byte>();

            foreach (byte[] bs in bytesToAdd)
            {
                largeBytesList.AddRange(bs);
            }

            return largeBytesList.ToArray();
        }


        public static long CompareBytes(this byte[] baseBytes, byte[] bytesToCompare)
        {
            long difference = 0;
            if ((baseBytes == null && bytesToCompare == null) ||
                (baseBytes.Length == 0 && bytesToCompare.Length == 0))
                return difference;

            if (baseBytes.Length != bytesToCompare.Length)
                difference = Math.Abs((long)(baseBytes.Length - bytesToCompare.Length)) * 256;
            else // if (baseBytes.Length == bytesToCompare.Length)
            {
                for (int ib = 0; ib < baseBytes.Length; ib++)
                {
                    if (baseBytes[ib] != bytesToCompare[ib])
                        difference += Math.Abs((long)(baseBytes[ib] - bytesToCompare[ib]));
                }
            }

            return difference;
        }

        public static long BytesCompare(byte[] baseBytes, byte[] bytesToCompare)
        {
            long difference = 0;
            if ((baseBytes == null && bytesToCompare == null) ||
                (baseBytes.Length == 0 && bytesToCompare.Length == 0))
                return difference;

            if (baseBytes.Length != bytesToCompare.Length)
                difference = Math.Abs((long)(baseBytes.Length - bytesToCompare.Length)) * 256;
            else // if (baseBytes.Length == bytesToCompare.Length)
            {
                for (int ib = 0; ib < baseBytes.Length; ib++)
                {
                    if (baseBytes[ib] != bytesToCompare[ib])
                        difference += Math.Abs((long)(baseBytes[ib] - bytesToCompare[ib]));
                }
            }

            return difference;
        }


        public static byte[] ToExternalBytes(this IPAddress ip)
        {
            List<byte> bytes = new List<byte>();
            if (ip == null)
                return bytes.ToArray();

            string tmps = ip.ToString();

            if (!string.IsNullOrEmpty(tmps))
            {
                switch (ip.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        foreach (string ipv4Segment in tmps.Trim("{}".ToCharArray()).Split('.'))
                            bytes.Add(Convert.ToByte(ipv4Segment));
                        break;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        foreach (string ipv6Segment in tmps.Trim("[{}]".ToCharArray()).Split(':'))
                            bytes.Add(Convert.ToByte(ipv6Segment));
                        break;
                    default:
                        bytes.AddRange(ip.GetAddressBytes());
                        break;
                }
            }

            return bytes.ToArray();
        }


        public static byte[] ToVersionBytes(this System.Version version)
        {
            List<byte> bytes = new List<byte>();
            if (version == null)
                return bytes.ToArray();

            bytes.Add(Convert.ToByte(version.Major));
            bytes.Add(Convert.ToByte(version.Minor));
            bytes.Add(Convert.ToByte(version.Build % 256));
            bytes.Add(Convert.ToByte(version.Revision));

            return bytes.ToArray();
        }

        #endregion byte_array_extensions

        #region string_extensions

        /// <summary>
        /// <see cref="string"/>.FromHexString() extension method: converts hexadecimal string to byte[]
        /// </summary>
        /// <param name="hexString">hexadecimal string</param>
        /// <returns><see cref="byte[]">byte[]</see> Array of <see cref="byte"/></returns>
        public static byte[] FromHexString(this string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return bytes; // returns: "Hello world" for "48656C6C6F20776F726C64"
        }

        /// <summary>
        /// <see cref="string"/>.Base64ToStream() extension method: converts base64 string to stream
        /// </summary>
        /// <param name="base64">base64 encoded string</param>
        /// <returns>MemoryStream</returns>
        public static MemoryStream Base64ToStream(this string base64)
        {
            byte[] bytes = Crypt.EnDeCoding.Base64.Decode(base64);
            MemoryStream ms = new MemoryStream(bytes.Length);
            ms.Write(bytes, 0, bytes.Length);
            ms.Flush();
            return ms;
        }

        /// <summary>
        /// <see cref="string"/>.Base64ToImage() extension method: converts base64 string to an image
        /// </summary>
        /// <param name="base64">base64 encoded string</param>
        /// <returns>Image?</returns>
        public static Image Base64ToImage(this string base64)
        {
            Bitmap bitmap = null;
            try
            {
                byte[] bytes = Crypt.EnDeCoding.Base64.Decode(base64);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    bitmap = new Bitmap(ms);
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("Extension", "Base64ToImage(...) throwed Exception " + ex.GetType(), ex);
                bitmap = null;
            }
            return bitmap;
        }

        /// <summary>
        /// <see cref="string"/>.FromHtmlToColor() extension methods: transforms hex #rrggbb string into <see cref="Color"/>
        /// </summary>
        /// <param name="htmlRGBString"><see cref="string"/> to transform</param>
        /// <returns><see cref="Color"/></returns>
        /// <exception cref="ArgumentException">invalid argument exception, in case of malformatted string</exception>
        public static Color FromHtmlToColor(this string htmlRGBString)
        {
            if (string.IsNullOrWhiteSpace(htmlRGBString) || htmlRGBString.Length != 7 || !htmlRGBString.StartsWith("#"))
                throw new ArgumentException(
                    string.Format("System.Drawing.Color.FromHtml(string htmlRGBString = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", htmlRGBString));

            Color _color = ColorTranslator.FromHtml(htmlRGBString);
            return _color;
        }


        /// <summary>
        /// <see cref="string"/>.<see cref="GetExtensionFromFileString(string)"/> Extension method
        /// </summary>
        /// <param name="fileName">fileName to process</param>
        /// <returns>extension of fileName</returns>
        public static string GetExtensionFromFileString(this string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !fileName.Contains("."))
                return null;
            int lastIdx = fileName.LastIndexOf('.');
            string ext = fileName.Substring(lastIdx);

            return ext;
        }

        /// <summary>
        /// HasString extension method examines, if a <see cref="T:string"/> contains a sub string
        /// <see cref="StringComparison.CurrentCultureIgnoreCase"/> on substring is implemented so:
        /// 
        /// if (haystack.Contains(needle)) return true;
        /// if (haystack.ToLower().Contains(needle.ToLower())) return true;
        /// if (haystack.ToUpper().Contains(needle.ToUpper())) return true;
        /// if ((haystack.IndexOf(needle, StringComparison.CurrentCultureIgnoreCase)) > -1) return true;
        /// 
        /// </summary>
        /// <param name="haystack"><see cref="T:string">string haystack to search</see></param>
        /// <param name="needle"><see cref="T:string">substring needle to find</see> in haystack</param>
        /// <returns>true, if a very similiar needle <see cref="StringComparison.CurrentCultureIgnoreCase"/> was found, oterwise false.</returns>
        public static bool HasString(this string haystack, string needle)
        {

            if (haystack != null && needle != null)
            {
                if (haystack.Contains(needle)) return true;
                if (haystack.ToLower().Contains(needle.ToLower())) return true;
                if (haystack.ToUpper().Contains(needle.ToUpper())) return true;
                if ((haystack.IndexOf(needle)) > -1) return true;
            }
            
            return false;
        }

        /// <summary>
        /// <see cref="string"/>.<see cref="GetSubStringByPattern(string, string, bool, string, string, bool, StringComparison)"/> Extension method 
        /// gets a substring beginning with patternStart and ending with patternEnd
        /// </summary>
        /// <param name="main">string to transform</param>
        /// <param name="patternStart">start pattern for substring, 
        /// if <see cref="string.Contains(string, StringComparison)">main.Contains(patternStart, comparasionType)</see>,
        /// so that int firstIndex = <see cref="string.IndexOf(string)">main.IndexOf(patternStart)</see> >= 0,
        /// then substring will start at <see cref="string.Substring(int)">main.Substring(firstIndex)</see>
        /// </param>
        /// <param name="firstIndex">default <see cref="true"/>, 
        /// if <see cref="true"/>, then first occurence in main <see cref="string.IndexOf(string)">main.IndexOf(patternStart)</see> will be executed, 
        /// otherwise if <see cref="false"/>, then <see cref="string.LastIndexOf(string)">main.LastIndexOf(patternStart)</see> will be executed.
        /// </param>
        /// <param name="markStartEnd">if <see cref="!string.IsNullOrEmpty(string?)">!string.IsNullOrEmpty(markStartEnd)</see> 
        /// then start position of substring will be set to <see cref="string.IndexOf(string)>">string.IndexOf(markStartEnd)</see>
        /// </param>
        /// <param name="patternEnd">end pattern for substring, <see cref="string.LastIndexOf(string)">main.IndexOf(patternEnd)</see></param>
        /// <param name="lastIndex">default <see cref="false"/>
        /// if <see cref="true"/>, then last occurence <see cref="string.LastIndexOf(string)">main.LastIndexOf(patternEnd)</see> will be executed,
        /// otherwise if <see cref="false"/>, then first occurence in main <see cref="string.IndexOf(string)">main.IndexOf(patternEnd)</see> will be executed. 
        /// </param>
        /// <param name="comparasionType">
        /// <returns><see cref="string">substring</see>, if no substring could be extracted, then <see cref="string.Empty"/></returns>
        public static string GetSubStringByPattern(this string main,
            string patternStart, bool firstIndex = true, string markStartEnd = "",
            string patternEnd = "", bool lastIndex = false
            // , StringComparison comparasionType = StringComparison.CurrentCulture
        ) {

            string substring = string.Empty;
            if (string.IsNullOrEmpty(main))
                return substring;

            substring = main;
            
            if (!string.IsNullOrEmpty(patternStart) && substring.Contains(patternStart))
            {
                if (firstIndex)
                    substring = main.Substring(substring.IndexOf(patternStart) + patternStart.Length);
                else
                    substring = main.Substring(substring.LastIndexOf(patternStart) + patternStart.Length);

                if (!string.IsNullOrEmpty(markStartEnd) && substring.Contains(markStartEnd))
                    substring = substring.Substring(substring.IndexOf(markStartEnd) + markStartEnd.Length);
            }
            if (!string.IsNullOrEmpty(patternEnd) && substring.Contains(patternEnd))
            {
                if (!lastIndex)
                    substring = substring.Substring(0, substring.IndexOf(patternEnd));
                else
                    substring = substring.Substring(0, substring.LastIndexOf(patternEnd));
            }

            return substring;
        }

        /// <summary>
        /// BeautifyUploadFileNames extension method for string
        /// </summary>
        /// <param name="uploadFileName">this string to beautify</param>
        /// <returns>beautified string</returns>
        public static string BeautifyUploadFileNames(this string uploadFileName)
        {

            string bfName = uploadFileName;
            if (!string.IsNullOrEmpty(bfName) && bfName.Length > 2)
            {
                bfName = bfName.Replace(' ', '_');
                bfName = bfName.Replace('\\', '_');
                bfName = bfName.Replace('\"', '_');
                bfName = bfName.Replace('\'', '_');
                bfName = bfName.Replace('!', '_');
                bfName = bfName.Replace('?', '_');
                bfName = bfName.Replace('´', '_');
                bfName = bfName.Replace('$', '_');
                bfName = bfName.Replace('%', '_');
                bfName = bfName.Replace('&', '_');
                bfName = bfName.Replace('´', '_');
                bfName = bfName.Replace('´', '_');
                bfName = bfName.Replace('#', '_');
                bfName = bfName.Replace(':', '_');
                bfName = bfName.Replace(';', '_');
                bfName = bfName.Replace(',', '_');
                bfName = bfName.Replace('<', '_');
                bfName = bfName.Replace('|', '_');
                bfName = bfName.Replace('>', '_');
                bfName = bfName.Replace('=', '_');
                bfName = bfName.Replace('^', '_');
                bfName = bfName.Replace('°', '_');
                bfName = bfName.Replace('\t', '_');

                bfName = bfName.Replace('*', '-');
                bfName = bfName.Replace('+', '-');
                bfName = bfName.Replace('/', '-');
                bfName = bfName.Replace('~', '-');

                bfName = bfName.Replace('²', '2');
                bfName = bfName.Replace('³', '3');
                bfName = bfName.Replace('{', '[');
                bfName = bfName.Replace('}', ']');
                bfName = bfName.Replace('(', '[');
                bfName = bfName.Replace(')', ']');
                
                bfName = bfName.Replace("â", "a");
                bfName = bfName.Replace("à", "a");
                bfName = bfName.Replace("á", "a");

                bfName = bfName.Replace("è", "e");
                bfName = bfName.Replace("é", "e");
                bfName = bfName.Replace("ê", "e");

                bfName = bfName.Replace("ß", "sz");
                bfName = bfName.Replace("ä", "ae");
                bfName = bfName.Replace("ö", "oe");
                bfName = bfName.Replace("ü", "ue");
                bfName = bfName.Replace("Ä", "Ae");
                bfName = bfName.Replace("Ö", "Oe");
                bfName = bfName.Replace("Ü", "Ue");

            }

            return bfName;

        }

        #endregion string_extensions

        #endregion stream_byteArray_string_extensions

        #region System.Exception extensions

        /// <summary>
        /// <see cref="Exception"/>.ToLogMsg() extension method: formats an exception to a well formatted logging message
        /// </summary>
        /// <param name="exc">the <see cref="Exception">exception</see></param>
        /// <returns><see cref="string">logMsg</see></returns>
        public static string ToLogMsg(this Exception exc)
        {
            return string.Format("Exception {0} ⇒ {1}\t{2}\t{3}",
                    exc.GetType(),
                    exc.Message,
                    exc.ToString().Replace("\r", "").Replace("\n", " "),
                    exc.StackTrace.Replace("\r", "").Replace("\n", " "));
        }

        #endregion System.Exception extensions

        #region System.Drawing.Color extensions

        /// <summary>
        /// <see cref="System.Drawing.Color"/>.FromHtml(string hex) extension method: gets color from hexadecimal rgb string html standard
        /// </summary>
        /// <param name="color">System.Drawing.Color.FromHtml(string hex) extension method</param>
        /// <param name="hex">hexadecimal rgb string with starting #</param>
        /// <returns>Color, that was defined by hexadecimal html standarized #rrggbb string</returns>
        public static System.Drawing.Color FromHtml(this System.Drawing.Color color, string hex)
        {
            if (String.IsNullOrWhiteSpace(hex) || hex.Length != 7 || !hex.StartsWith("#"))
                throw new ArgumentException(
                    String.Format("System.Drawing.Color.FromHtml(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", hex));

            System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
            return _color;
        }

        /// <summary>
        /// <see cref="System.Drawing.Color"/>.FromXrgb(string hex) extension method: gets color from hexadecimal rgb string
        /// </summary>
        /// <param name="color">System.Drawing.Color.FromXrgb(string hex) extension method</param>
        /// <param name="hex">hexadecimal rgb string with starting #</param>
        /// <returns>Color, that was defined by hexadecimal rgb string</returns>
        public static System.Drawing.Color FromXrgb(this System.Drawing.Color color, string hex)
        {
            if (String.IsNullOrWhiteSpace(hex) || hex.Length < 6 || hex.Length > 9)
                throw new ArgumentException(
                    String.Format("System.Drawing.Color.FromXrgb(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" or \"rrggbb\"", hex));

            string rgbWork = hex.TrimStart("#".ToCharArray());

            string colSeg = rgbWork.Substring(0, 2);
            colSeg = (colSeg.Contains("00")) ? "0" : colSeg.TrimStart("0".ToCharArray());
            int r = Convert.ToUInt16(colSeg, 16);
            colSeg = rgbWork.Substring(2, 2);
            colSeg = (colSeg.Contains("00")) ? "0" : colSeg.TrimStart("0".ToCharArray());
            int g = Convert.ToUInt16(colSeg, 16);
            colSeg = rgbWork.Substring(4, 2);
            colSeg = (colSeg.Contains("00")) ? "0" : colSeg.TrimStart("0".ToCharArray());
            int b = Convert.ToUInt16(colSeg, 16);

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// <see cref="System.Drawing.Color"/>.FromRGB(byte r, byte g, byte b) extension method: gets color from R G B
        /// </summary>
        /// <param name="color">System.Drawing.Color.FromXrgb(string hex) extension method</param>
        /// <param name="r">red byte</param>
        /// <param name="g">green byte</param>
        /// <param name="b">blue byte</param>
        /// <returns>Color, that was defined by hexadecimal rgb string</returns>
        /// <exception cref="ArgumentException"></exception>
        public static System.Drawing.Color FromRGB(this System.Drawing.Color color, byte r, byte g, byte b)
        {
            return System.Drawing.Color.FromArgb((int)r, (int)g, (int)b);
        }

        /// <summary>
        /// <see cref="System.Drawing.Color"/>.ToXrgb() extension method: converts current color to hex string 
        /// </summary>
        /// <param name="color">current color</param>
        /// <returns>hexadecimal #rrGGbb string with leading # character</returns>
        public static string ToXrgb(this System.Drawing.Color color)
        {
            string rx = color.R.ToString("X");
            rx = (rx.Length > 1) ? rx : "0" + rx;
            string gx = color.G.ToString("X");
            gx = (gx.Length > 1) ? gx : "0" + gx;
            string bx = color.B.ToString("X");
            bx = (bx.Length > 1) ? bx : "0" + bx;

            string hex = String.Format("#{0}{1}{2}", rx, gx, bx);
            return hex.ToLower();
        }

        /// <summary>
        /// <see cref="System.Drawing.Color"/>.IsInLevenSteinDistance(Color colorCompare) extension method: finds out, if colorSrc and colorCompare are inside Levenstein distance
        /// </summary>
        /// <param name="colorSrc">source <see cref="System.Drawing.Color"/></param>
        /// <param name="colorCompare"><see cref="System.Drawing.Color"/> to compare with</param>
        /// <param name="levenSteinDelta">the absolute distance between to colors to tolerate</param>
        /// <returns>true, if both colors are inside Levenstein distance</returns>
        public static bool IsInLevenSteinDistance(this System.Drawing.Color colorSrc, System.Drawing.Color colorCompare, int levenSteinDelta = 6)
        {
            byte sRed = colorSrc.R;
            byte sGreen = colorSrc.G;
            byte sBlue = colorSrc.B;

            byte cRed = colorCompare.R;
            byte cGreen = colorCompare.G;
            byte cBlue = colorCompare.B;

            int deltaRed = Math.Abs((int)((int)cRed - (int)sRed));
            int deltaGreen = Math.Abs((int)((int)cGreen - (int)sGreen));
            int deltaBlue = Math.Abs((int)((int)cBlue - (int)sBlue));

            int distanceRGB = deltaRed + deltaGreen + deltaBlue;

            return (distanceRGB <= levenSteinDelta);
        }

        #endregion System.Drawing.Color extensions


        #region System.Drawing.Image extensions

        /// <summary>
        /// <see cref="Image"/>.SaveRawToMemoryStream(ImageFormat imageFormat, out Nullable&lt;Guid&gt; g) extension method: 
        /// saves an Image to <see cref="MemoryStream"/> and return <see cref="Guid"/> of <see cref="ImageFormat"/> as out parameter
        /// </summary>
        /// <param name="img"><see cref="Image"/> to be processed by Exentsion Method</param>
        /// <param name="imageFormat"><see cref="ImageFormat"/></param>
        /// <param name="g"><see cref="Guid">out Guid g</see></param>
        /// <returns><see cref="MemoryStream"/></returns>
        public static MemoryStream SaveRawToMemoryStream(this System.Drawing.Image img, ImageFormat imageFormat, out Nullable<Guid> g)
        {
            imageFormat = imageFormat ?? img.RawFormat;
            g = imageFormat.Guid;
            
            MemoryStream ms = new MemoryStream();
            img.Save(ms, imageFormat);
            
            return ms;
        }

        /// <summary>
        /// <see cref="Image"/>.ToByteArray() extension method: converts <see cref="Image"/> to byte array
        /// </summary>
        /// <param name="img">this <see cref="Image"/></param>
        /// <returns><see cref="byte[]?"/> array</returns>
        public static byte[] ToByteArray(this Image img)
        {
            byte[] bytes;
            MemoryStream ms = new MemoryStream();
            Nullable<Guid> imgFormGuid;
            try
            {
                if (img.RawFormat == ImageFormat.Png)
                    ms = img.SaveRawToMemoryStream(ImageFormat.Png, out imgFormGuid);
                else if (img.RawFormat == ImageFormat.Jpeg)
                    ms = img.SaveRawToMemoryStream(ImageFormat.Jpeg, out imgFormGuid);
                else if (img.RawFormat == ImageFormat.Gif)
                    ms = img.SaveRawToMemoryStream(ImageFormat.Gif, out imgFormGuid);
                else if (img.RawFormat == ImageFormat.Bmp)
                    ms = img.SaveRawToMemoryStream(ImageFormat.Bmp, out imgFormGuid);
                else if (img.RawFormat == ImageFormat.Emf)
                    ms = img.SaveRawToMemoryStream(ImageFormat.Emf, out imgFormGuid);
                else if (img.RawFormat == ImageFormat.Exif)
                    ms = img.SaveRawToMemoryStream(ImageFormat.Exif, out imgFormGuid);
                else if (img.RawFormat == ImageFormat.Wmf)
                    ms = img.SaveRawToMemoryStream(ImageFormat.Wmf, out imgFormGuid);
                else if (img.RawFormat == ImageFormat.Tiff)
                    ms = img.SaveRawToMemoryStream(ImageFormat.Tiff, out imgFormGuid);
                else if (img.RawFormat == ImageFormat.Icon)
                    ms = img.SaveRawToMemoryStream(ImageFormat.Icon, out imgFormGuid);
                else
                {
                    img.Save(ms, img.RawFormat);
                    imgFormGuid = img.RawFormat.Guid;
                }
            }
            catch (Exception exImgFormat)
            {
                imgFormGuid = Guid.Empty;
                Area23Log.LogOriginMsgEx("Extension", "ToByteArray(this Image img) throwed Exception " + exImgFormat.GetType(), exImgFormat);
            }

            if (imgFormGuid != null && imgFormGuid.HasValue && imgFormGuid.Value != Guid.Empty)
            {
                ms.Position = 0;
                bytes = ms.ToArray();
            }
            else
                bytes = new byte[0];

            try
            {
                ms.Close();
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("Extension", "ToByteArray(this Image img) throwed Exception " + ex.GetType(), ex);
            }

            return bytes;

        }

        /// <summary>
        /// <see cref="Image"/>.ToBase64() extension method: converts <see cref="Image"/> to base64 string
        /// </summary>
        /// <param name="img">this <see cref="Image"/></param>
        /// <returns>base64 encoded <see cref="string?"/></returns>
        public static string ToBase64(this Image img)
        {
            string base64 = null;
            byte[] bytes;
            try
            {                
                bytes = img.ToByteArray();
                base64 = Crypt.EnDeCoding.Base64.Encode(bytes);                
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("Extension", "ToBase64(this Image img) throwed Exception " + ex.GetType(), ex);
            }

            return base64;
        }

        #endregion System.Drawing.Image extensions

        #region serializer_xml_json

        /// <summary>
        /// IsValidXml extension method, that verifies if a string is a valid xml serialization
        /// </summary>
        /// <param name="xml">this <see cref="string">string xml</see> to xml validate</param>
        /// <returns>true, if it's a valid serialized xml string, otherwise false</returns>
        public static bool IsValidXml(this string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Extension Method, that verifies if a string is a valid json serialization
        /// </summary>
        /// <param name="strInput">this <see cref="string">string input</see> to json validate</param>
        /// <returns>true, if it's a valid serialized json string, otherwise false</returns>
        public static bool IsValidJson(this string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion serializer_xml_json

        #region System.Net extension methods

        public static bool IsSameIp(this IPAddress ip1, IPAddress ip2, Nullable<AddressFamily> addrFamily = null)
        {
            if (addrFamily == null || !addrFamily.HasValue)
                return ((Extensions.BytesCompare(ip1.GetAddressBytes(), ip2.GetAddressBytes()) == 0) &&
                    (ip1.AddressFamily == ip2.AddressFamily));

            return ((Extensions.BytesCompare(ip1.GetAddressBytes(), ip2.GetAddressBytes()) == 0) &&
                    (ip1.AddressFamily == ip2.AddressFamily) &&
                    (ip1.AddressFamily == addrFamily.Value));
        }


        public static string ShortInfo(this AddressFamily addrFamily)
        {
            switch (addrFamily)
            {
                case AddressFamily.InterNetwork: return " IPv4 ";
                case AddressFamily.InterNetworkV6: return " IPv6 ";
                case AddressFamily.Unix: return " Unix ";
                case AddressFamily.Irda: return " Irda ";
                case AddressFamily.Atm: return "  Atm ";
                case AddressFamily.Ccitt: return "Ccitt ";
                case AddressFamily.Ecma: return " Ecma ";
                case AddressFamily.Ipx: return "  Ipx ";
                // case AddressFamily.Osi:
                case AddressFamily.Iso: return "  Iso ";
                default: return "      ";
            }
        }

        #endregion System.Net extension methods

        #region genericsT_extensions

        /// <summary>
        /// <see cref="Stack{T}"/>.ReverseToString<typeparamref name="T"/> extension method: reverses a objects in a stack to a string
        /// </summary>      
        /// <typeparam name="T">type parameter for generic <see cref="Stack{T}"/></typeparam>
        /// <param name="stack">a generic  <see cref="Stack{T}">Stack</see></param>  
        /// <returns>a string concatenation of reversed (fifoed) stack</returns>
        public static string ReverseToString<T>(this Stack<T> stack)
        {
            string reverse = string.Empty;
            foreach (object s in stack.Reverse().ToArray())
            {
                reverse += s.ToString();
            }
            return reverse;
        }


        /// <summary>
        /// <see cref="T"/>.SwapTPositions&lt;<typeparamref name="T"/>&gt;(this <typeparamref name="T"/>[] tarray, .. extensions method
        /// Swaps values of two positions inside a generic Array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tarray">generic array</param>
        /// <param name="posA">position A of array</param>
        /// <param name="posB">position B of array</param>
        /// <param name="exceptionOnNullable">if indices points to null value field, throw exception</param>
        /// <returns>swapped T generic array</returns>
        /// <exception cref="ArgumentException"></exception>
        internal static T[] SwapTPositions<T>(this T[] tarray, int posA, int posB, bool exceptionOnNullable = false)
        {
            if (tarray == null || tarray.Length <= 1)
                throw new ArgumentException($"T ... {tarray.GetType()} is null or an empty or single entry array");

            if (posA < 0 || posB < 0)
                throw new ArgumentException($"Either posA {posA} or posB {posB} is a negative / minus number and less than 0. Can't access array indices < 0");

            if (posA >= tarray.Length || posB >= tarray.Length)
                throw new ArgumentException($"Either posA {posA} or posB {posB} is outside of array size of {tarray.Length}. Can't access array indices greater then array size {tarray.Length}.");

            if ((exceptionOnNullable) && (tarray[posA] == null || tarray[posB] == null))
                throw new ArgumentException($"Either T[posA= {posA}] == null or T[posB = {posB}] contains a null value; exceptionOnNullable = {exceptionOnNullable}.");


            // if (posA.Equals(posB))  // return tarray;

            T t0 = tarray[posA];
            T t1 = tarray[posB];
            tarray[posA] = t1;
            tarray[posB] = t0;

            return tarray;
        }

        #endregion genericsT_extensions


        #region cqrxs extensions

        /// <summary>
        /// IsPermAgainCryptFile 
        /// </summary>
        /// <param name="fileName">file name to parse</param>
        /// <returns>
        ///     true, when looking like perm again crypt file name,
        ///     false, when normal standard extension
        /// </returns>
        public static bool IsPermAgainCryptFile(this string fileName)
        {
            // all chars, that a CipherPipe could have from CipherEnum[] segments
            string pipeChars = "ALEabFfClc6$DedgIN25RrsS4JtTjX%!";
            if (string.IsNullOrEmpty(fileName)) return false;

            string ext = Path.GetExtension(fileName).StartsWith(".")
                ? Path.GetExtension(fileName)
                : "." + Path.GetExtension(fileName);
            
            ext = ext.ToLower();

            if (ext.EndsWith(".uu") ||
                ext.EndsWith(".xx") ||
                ext.EndsWith(".base16") ||
                ext.EndsWith(".base32") ||
                ext.EndsWith(".hex64") ||
                ext.EndsWith(".base64") ||
                ext.EndsWith(".hex16") ||
                ext.EndsWith(".hex32") ||
                ext.Contains(".gz") ||
                ext.Contains(".bz"))
                return true;

            if (ext.EndsWith(".png") ||
                ext.EndsWith(".jpg") ||
                ext.EndsWith(".jpeg") ||
                ext.EndsWith(".gif") ||
                ext.EndsWith(".tif") ||
                ext.EndsWith(".bmp") ||
                ext.EndsWith(".exif") ||
                ext.EndsWith(".ico") ||
                ext.EndsWith(".doc") ||
                ext.EndsWith(".docx") ||
                ext.EndsWith(".docm") ||
                ext.EndsWith(".dot") ||
                ext.EndsWith(".dotx") ||
                ext.EndsWith(".dotm") ||
                ext.EndsWith(".odm") ||
                ext.EndsWith(".xlsx") ||
                ext.EndsWith(".xl") ||
                ext.EndsWith(".xls") ||
                ext.EndsWith(".xlsx") ||
                ext.EndsWith(".xla") ||
                ext.EndsWith(".xlb") ||
                ext.EndsWith(".xlc") ||
                ext.EndsWith(".xld") ||
                ext.EndsWith(".xlk") ||
                ext.EndsWith(".xll") ||
                ext.EndsWith(".xlm") ||
                ext.EndsWith(".xlsb") ||
                ext.EndsWith(".xlsm") ||
                ext.EndsWith(".xlt") ||
                ext.EndsWith(".xltm") ||
                ext.EndsWith(".xltx") ||
                ext.EndsWith(".xlv") ||
                ext.EndsWith(".xlw") ||
                ext.EndsWith(".odx") ||
                ext.EndsWith(".csv") ||
                ext.EndsWith(".pptx") ||
                ext.EndsWith(".ppt") ||
                ext.EndsWith(".odp") ||
                ext.EndsWith(".vsd") ||
                ext.EndsWith(".vsw") ||
                ext.EndsWith(".vsx") ||
                ext.EndsWith(".vtx") ||
                ext.EndsWith(".vds") ||
                ext.EndsWith(".vdx") ||
                ext.EndsWith(".vss") ||
                ext.EndsWith(".vst") ||
                ext.EndsWith(".vsdx") ||
                ext.EndsWith(".vsto") ||
                ext.EndsWith(".txt") ||
                ext.EndsWith(".text") ||
                ext.EndsWith(".asc") ||
                ext.EndsWith(".md") ||
                ext.EndsWith(".cs") ||
                ext.EndsWith(".cshtml") ||
                ext.EndsWith(".c") ||
                ext.EndsWith(".h") ||
                ext.EndsWith(".java") ||
                ext.EndsWith(".jar") ||
                ext.EndsWith(".html") ||
                ext.EndsWith(".htm") ||
                ext.EndsWith(".xhtml") ||
                ext.EndsWith(".aspx") ||
                ext.EndsWith(".ascx") ||
                ext.EndsWith(".asax") ||
                ext.EndsWith(".ashx") ||
                ext.EndsWith(".xml") ||
                ext.EndsWith(".mp3") ||
                ext.EndsWith(".mp4") ||
                ext.EndsWith(".mpeg") ||
                ext.EndsWith(".webm") ||
                ext.EndsWith(".wav") ||
                ext.EndsWith(".mpg") ||
                ext.EndsWith(".wmv") ||
                ext.EndsWith(".exe") ||
                ext.EndsWith(".dll") ||
                ext.EndsWith(".pdb") ||
                ext.EndsWith(".json") ||
                ext.EndsWith(".bat") ||
                ext.EndsWith(".com") ||
                ext.EndsWith(".sh") ||
                ext.EndsWith(".ps") ||
                ext.EndsWith(".pdf") ||
                ext.EndsWith(".rtf"))
                return false;

            string[] filesegments = fileName.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int lastIdx = filesegments.Length - 1;
            // TODO: Better check with pipe
            if (lastIdx > 2)
            {
                for (int j = lastIdx; j >= 1; j--)
                {
                    bool err = false;
                    if (filesegments[j].Length > 0 && filesegments[j].Length != 3)
                    {
                        foreach (char c4 in filesegments[j])
                        {
                            if (!pipeChars.Contains(c4))
                            {
                                err = true; break;
                            }
                        }
                    }
                    if (!err)
                        return true;
                }
            }

            return false;
        }

        public static bool IsCompressedFile(this string fileExtension)
        {
            switch (fileExtension.Replace(".", "").ToLower())
            {
                case "gz":
                case "tar":
                case "tar.gz":
                case "tgz":
                case "bz":
                case "bz2":
                case "tar.bz":
                case "tar.bz2":
                case "tbz":
                case "7z":
                case "7zip":
                case "zip":
                case "rar":
                case "jar":
                case "mp4":
                case "mp3":
                case "arj":
                case "z":
                case "exe":
                case "dll":
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Extension <see cref="string"/>.StripCipherPipeFromFileName(out CipherPipe? cipherPipe)
        /// looks if a filename is <see cref="IsPermAgainCryptFile(string)"/> and if true,
        /// extracts PipeString out of filename and creates the corresponding pipe.
        /// </summary>
        /// <param name="fileName">file name to examine</param>
        /// <param name="cipherPipe">out parameter for <see cref="CipherPipe"/></param>
        /// <returns>stripped filename by CipherPipe PermAgainCrypt characters</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string StripCipherPipeFromFileName(this string fileName, out CipherPipe cipherPipe)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            cipherPipe = null;
            string strippedFileName = fileName;

            if (!fileName.IsPermAgainCryptFile())
                return strippedFileName;

            KeyHash kHash = KeyHash.Hex;

            EncodingType eType = EncodingType.None;
            foreach (EncodingType encTyp in EncodingTypesExtensions.GetEncodingTypes())
            {
                if (fileName.EndsWith("." + encTyp.ToString()) || fileName.EndsWith("." + encTyp.ToString().ToLower()))
                {
                    eType = encTyp;
                    strippedFileName = fileName.Replace("." + encTyp.ToString(), "").Replace("." + encTyp.ToString().ToLower(), "");
                    break;
                }
            }

            ZipType[] zipTypes = { ZipType.BZip2, ZipType.GZip, ZipType.Zip };
            ZipType zipTyp = ZipType.None;
            foreach (ZipType zType in zipTypes)
            {
                if (strippedFileName.EndsWith(zType.GetZipTypeExtension()))
                {
                    zipTyp = zType;
                    strippedFileName = strippedFileName.Replace(zipTyp.GetZipTypeExtension(), "");
                }
                else if (strippedFileName.Contains(zType.GetZipTypeExtension() + "."))
                {
                    zipTyp = zType;
                    strippedFileName = strippedFileName.Replace(zipTyp.GetZipTypeExtension() + ".", ".");
                }
            }


            foreach (KeyHash kh in KeyHash_Extensions.GetHashTypes())
            {
                if (strippedFileName.Contains("." + kh.ToString()))
                {
                    kHash = kh;
                    strippedFileName = strippedFileName.Replace("." + kh.ToString(), "").Replace("." + kh.ToString().ToLower(), "");

                    break;
                }
            }

            List<CipherEnum> cipherEnums = new List<CipherEnum>();
            string pipeRestString = strippedFileName.Substring(strippedFileName.LastIndexOf("."));
            foreach (char ch in pipeRestString)
            {
                foreach (CipherEnum cipher in CipherEnumExtensions.GetCipherTypes())
                {
                    if (cipher.GetCipherChar() == ch)
                        cipherEnums.Add(cipher);
                }
            }

            if (cipherEnums.Count > 0)
            {
                cipherPipe = new CipherPipe(cipherEnums.ToArray(), 8, eType, zipTyp, kHash);
                if (strippedFileName.Contains("." + cipherPipe.PipeString))
                {
                    strippedFileName = strippedFileName.Replace("." + cipherPipe.PipeString, "");
                }
            }


            return strippedFileName;
        }

        public static string StripSymmCipherPipeFromFileName(this string fileName, out CipherPipe symmCipherPipe)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            symmCipherPipe = null;
            string strippedFileName = fileName;

            if (!fileName.IsPermAgainCryptFile())
                return strippedFileName;

            KeyHash kHash = KeyHash.Hex;

            EncodingType eType = EncodingType.None;
            foreach (EncodingType encTyp in EncodingTypesExtensions.GetEncodingTypes())
            {
                if (fileName.EndsWith("." + encTyp.ToString()))
                {
                    eType = encTyp;
                    strippedFileName = fileName.Replace("." + encTyp.ToString(), "").Replace("." + encTyp.ToString().ToLower(), "");
                    break;
                }
            }

            bool cipherAfterZip = false;
            ZipType zipTyp = ZipType.None;
            foreach (ZipType zType in ZipTypeExtensions.GetZipTypes())
            {
                if (zType != ZipType.None)
                {
                    if (strippedFileName.EndsWith(zType.GetZipTypeExtension()))
                        zipTyp = zType;
                    else if (strippedFileName.Contains(zType.GetZipTypeExtension() + "."))
                    {
                        zipTyp = zType;
                        cipherAfterZip = true;
                    }
                }
                if (zipTyp != ZipType.None)
                {
                    strippedFileName = strippedFileName.Replace(zipTyp.GetZipTypeExtension(), "").Replace(zipTyp.GetZipTypeExtension().ToLower(), "");
                    break;
                }
            }


            foreach (KeyHash kh in KeyHash_Extensions.GetHashTypes())
            {
                if (strippedFileName.Contains("." + kh.ToString()))
                {
                    kHash = kh;
                    strippedFileName = strippedFileName.Replace("." + kh.ToString(), "").Replace("." + kh.ToString().ToLower(), "");

                    break;
                }
            }

            List<CipherEnum> symmCipherEnums = new List<CipherEnum>();
            if (cipherAfterZip)
            {
                string pipeRestString = strippedFileName.Substring(strippedFileName.LastIndexOf("."));
                foreach (char ch in pipeRestString)
                {
                    foreach (CipherEnum symmCipher in CipherEnumExtensions.GetCipherTypes())
                    {
                        if (symmCipher.GetCipherChar() == ch)
                            symmCipherEnums.Add(symmCipher);
                    }
                }

                if (symmCipherEnums.Count > 0)
                {
                    CipherPipe cPipe = new CipherPipe(symmCipherEnums.ToArray(), 8, eType, zipTyp, kHash);
                    if (strippedFileName.Contains("." + cPipe.PipeString))
                    {
                        symmCipherPipe = cPipe;
                        strippedFileName = strippedFileName.Replace("." + cPipe.PipeString, "");
                    }
                }
            }


            if (symmCipherPipe == null)
                symmCipherPipe = new CipherPipe(symmCipherEnums.ToArray(), 8, eType, zipTyp, kHash);


            return strippedFileName;
        }

        /// <summary>
        /// Extension <see cref="string"/>.StripCiphersInFileName() 
        /// strips all <see cref="CipherEnum"/> and 
        /// <seealso cref="SymmCipherEnum"/> characters
        /// from current string.       
        /// </summary>
        /// <param name="fileName"><see cref="string"/> with extension</param>
        /// <returns>stripped string</returns>
        public static string StripCiphersInFileName(this string fileName)
        {
            // Count dots
            int dotCnt = 0, dotIdx = -1;
            string fname = fileName, strippedFileName = fileName;
            do
            {
                if ((dotIdx = fname.IndexOf(".")) >= 0)
                {
                    dotCnt++;
                    fname = fname.Substring(dotIdx + 1);
                }

            } while (dotIdx >= 0);

            ZipType zipTyp = ZipType.None;
            KeyHash kHash = KeyHash.Hex;
            EncodingType eType = EncodingType.None;

            foreach (EncodingType encTyp in EncodingTypesExtensions.GetEncodingTypes())
            {
                if (fileName.EndsWith("." + encTyp.ToString()) || fileName.EndsWith("."  + encTyp.ToString().ToLower()))
                {
                    eType = encTyp;
                    strippedFileName = fileName.Replace("." + encTyp.ToString(), "").Replace("." + encTyp.ToString().ToLower(), "");
                    break;
                }
            }

            List<CipherEnum> cipherEnums = new List<CipherEnum>();
            string pipeRestString = strippedFileName.Substring(strippedFileName.LastIndexOf("."));
            foreach (char ch in pipeRestString)
            {
                foreach (CipherEnum cipher in CipherEnumExtensions.GetCipherTypes())
                {
                    if (cipher.GetCipherChar() == ch)
                        cipherEnums.Add(cipher);
                }
            }

            if (cipherEnums.Count > 0)
            {
                CipherPipe cPipe = new CipherPipe(cipherEnums.ToArray(), 8, eType, zipTyp, kHash);
                if (strippedFileName.Contains("." + cPipe.PipeString))
                {
                    strippedFileName = strippedFileName.Replace("." + cPipe.PipeString, "");
                }
            }

            ZipType[] zipTypes = new ZipType[] { ZipType.GZip, ZipType.BZip2, ZipType.Zip, ZipType.None };
            foreach (ZipType zType in zipTypes)
            {
                if (zType != ZipType.None)
                {
                    if (strippedFileName.EndsWith(zType.GetZipTypeExtension()))
                    {
                        zipTyp = zType;
                        strippedFileName = strippedFileName.Replace(zipTyp.GetZipTypeExtension(), "");
                        break;
                    }
                    if (strippedFileName.EndsWith(zType.GetZipTypeExtension()))
                    {
                        zipTyp = zType;
                        strippedFileName = strippedFileName.Replace(zipTyp.GetZipTypeExtension(), "");
                        break;
                    }
                    if (strippedFileName.Contains(zType.GetZipTypeExtension()))
                    {
                        zipTyp = zType;
                        int idx = strippedFileName.IndexOf(zipTyp.GetZipTypeExtension());
                        string first = strippedFileName.Substring(0, idx);
                        string rest = strippedFileName.Substring(idx + zipTyp.GetZipTypeExtension().Length + 1);
                        strippedFileName = first + rest;
                        break;
                    }

                }
            }


            foreach (KeyHash kh in KeyHash_Extensions.GetHashTypes())
            {
                if (strippedFileName.EndsWith("." + kh.ToString()) || strippedFileName.EndsWith("." + kh.ToString().ToLower()))
                {
                    kHash = kh;
                    strippedFileName = strippedFileName.Replace("." + kh.ToString(), "").Replace("." + kh.ToString().ToLower(), "");

                    break;
                }
            }

            return strippedFileName;
        }

        #endregion cqrxs extensions


    }

}
