using Area23.At.Framework.Core.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;


namespace Area23.At.Framework.Core.Static
{

    /// <summary>
    /// Static extension methods Area23.At.Framework.Core
    /// 
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
            return Math.Truncate(d) == d || Math.Round(d) == d;
        }

        /// <summary>
        /// <see cref="double"/>.ToLong() extension methods: converts a double to a long <see cref="long"/>
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
            string formatted = string.Format("{0:yyyyMMdd_HHmmss}_{1}", dateTime, dateTime.Millisecond);
            // return formatted;
            return dateTime.ToString("yyyyMMdd_HHmmss_") + dateTime.Millisecond;
        }

        #endregion DateTime extensions

        #region stream_byteArray_string_extensions

        #region stream_extensions

        /// <summary>
        /// <see cref="Stream"/>.ToByteArray() extension method: converts <see cref="Stream"/> to <see cref="byte[]"/> array
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> which static methods are now extended</param>
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
        public static string ToFile(this byte[] bytes, string? filePath = null, string? fileName = null, string? fext = null)
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
                Area23Log.LogOriginMsgEx("Extensions", "ToFile(this byte[] bytes", ex);
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
        /// <returns>hexadecimal string</returns>
        public static string ToHexString(this byte[] bytes)
        {
            var sb = new StringBuilder();

            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

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


        public static byte[] ToExternalBytes(this IPAddress? ip)
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


        public static byte[] ToVersionBytes(this System.Version? version)
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
            byte[] bytes = Convert.FromBase64String(base64);
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
        public static Image? Base64ToImage(this string base64)
        {
            Bitmap? bitmap = null;
            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    bitmap = new Bitmap(ms);
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("Extensions", "Base64ToImage(this string base64)", ex);
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
        /// <see cref="StringComparison">comparasionType</see> is set default to <see cref="StringComparison.CurrentCulture"/>
        /// </param>
        /// <returns><see cref="string">substring</see>, if no substring could be extracted, then <see cref="string.Empty"/></returns>
        public static string GetSubStringByPattern(this string main,
            string patternStart, bool firstIndex = true, string markStartEnd = "",
            string patternEnd = "", bool lastIndex = false, StringComparison comparasionType = StringComparison.CurrentCulture)
        {

            string substring = string.Empty;
            if (string.IsNullOrEmpty(main))
                return substring;

            substring = main;

            if (!string.IsNullOrEmpty(patternStart) && substring.Contains(patternStart, comparasionType))
            {
                if (firstIndex)
                    substring = main.Substring(substring.IndexOf(patternStart, comparasionType) + patternStart.Length);
                else
                    substring = main.Substring(substring.LastIndexOf(patternStart, comparasionType) + patternStart.Length);

                if (!string.IsNullOrEmpty(markStartEnd) && substring.Contains(markStartEnd, comparasionType))
                    substring = substring.Substring(substring.IndexOf(markStartEnd, comparasionType) + markStartEnd.Length);
            }
            if (!string.IsNullOrEmpty(patternEnd) && substring.Contains(patternEnd, comparasionType))
            {
                if (!lastIndex)
                    substring = substring.Substring(0, substring.IndexOf(patternEnd, comparasionType));
                else
                    substring = substring.Substring(0, substring.LastIndexOf(patternEnd, comparasionType));
            }

            return substring;
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
        /// <see cref="Color"/>.FromHtml(string hex) extension method: gets color from hexadecimal rgb string html standard
        /// </summary>
        /// <param name="color">System.Drawing.Color.FromHtml(string hex) extension method</param>
        /// <param name="hex">hexadecimal rgb string with starting #</param>
        /// <returns>Color, that was defined by hexadecimal html standarized #rrggbb string</returns>
        public static Color FromHtml(this Color color, string hex)
        {
            if (string.IsNullOrWhiteSpace(hex) || hex.Length != 7 || !hex.StartsWith("#"))
                throw new ArgumentException(
                    string.Format("System.Drawing.Color.FromHtml(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", hex));

            Color _color = ColorTranslator.FromHtml(hex);
            return _color;
        }

        /// <summary>
        /// <see cref="Color"/>.FromXrgb(string hex) extension method: gets color from hexadecimal rgb string
        /// </summary>
        /// <param name="color">System.Drawing.Color.FromXrgb(string hex) extension method</param>
        /// <param name="hex">hexadecimal rgb string with starting #</param>
        /// <returns>Color, that was defined by hexadecimal rgb string</returns>
        public static Color FromXrgb(this Color color, string hex)
        {
            if (string.IsNullOrWhiteSpace(hex) || hex.Length < 6 || hex.Length > 9)
                throw new ArgumentException(
                    string.Format("System.Drawing.Color.FromXrgb(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" or \"rrggbb\"", hex));

            string rgbWork = hex.TrimStart("#".ToCharArray());

            string colSeg = rgbWork.Substring(0, 2);
            colSeg = colSeg.Contains("00") ? "0" : colSeg.TrimStart("0".ToCharArray());
            int r = Convert.ToUInt16(colSeg, 16);
            colSeg = rgbWork.Substring(2, 2);
            colSeg = colSeg.Contains("00") ? "0" : colSeg.TrimStart("0".ToCharArray());
            int g = Convert.ToUInt16(colSeg, 16);
            colSeg = rgbWork.Substring(4, 2);
            colSeg = colSeg.Contains("00") ? "0" : colSeg.TrimStart("0".ToCharArray());
            int b = Convert.ToUInt16(colSeg, 16);

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// <see cref="Color"/>.FromRGB(byte r, byte g, byte b) extension method: gets color from R G B
        /// </summary>
        /// <param name="color">System.Drawing.Color.FromXrgb(string hex) extension method</param>
        /// <param name="r">red byte</param>
        /// <param name="g">green byte</param>
        /// <param name="b">blue byte</param>
        /// <returns>Color, that was defined by hexadecimal rgb string</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Color FromRGB(this Color color, byte r, byte g, byte b)
        {
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// <see cref="Color"/>.ToXrgb() extension method: converts current color to hex string 
        /// </summary>
        /// <param name="color">current color</param>
        /// <returns>hexadecimal #rrGGbb string with leading # character</returns>
        public static string ToXrgb(this Color color)
        {
            string rx = color.R.ToString("X");
            rx = rx.Length > 1 ? rx : "0" + rx;
            string gx = color.G.ToString("X");
            gx = gx.Length > 1 ? gx : "0" + gx;
            string bx = color.B.ToString("X");
            bx = bx.Length > 1 ? bx : "0" + bx;

            string hex = string.Format("#{0}{1}{2}", rx, gx, bx);
            return hex.ToLower();
        }

        /// <summary>
        /// <see cref="Color"/>.IsInLevenSteinDistance(Color colorCompare) extension method: finds out, if colorSrc and colorCompare are inside Levenstein distance
        /// </summary>
        /// <param name="colorSrc">source <see cref="Color"/></param>
        /// <param name="colorCompare"><see cref="Color"/> to compare with</param>
        /// <param name="levenSteinDelta">the absolute distance between to colors to tolerate</param>
        /// <returns>true, if both colors are inside Levenstein distance</returns>
        public static bool IsInLevenSteinDistance(this Color colorSrc, Color colorCompare, int levenSteinDelta = 6)
        {
            byte sRed = colorSrc.R;
            byte sGreen = colorSrc.G;
            byte sBlue = colorSrc.B;

            byte cRed = colorCompare.R;
            byte cGreen = colorCompare.G;
            byte cBlue = colorCompare.B;

            int deltaRed = Math.Abs(cRed - sRed);
            int deltaGreen = Math.Abs(cGreen - sGreen);
            int deltaBlue = Math.Abs(cBlue - sBlue);

            int distanceRGB = deltaRed + deltaGreen + deltaBlue;

            return distanceRGB <= levenSteinDelta;
        }

        #endregion System.Drawing.Color extensions

        #region System.Drawing.Image extensions

        #region System.Drawing.Image extensions

        /// <summary>
        /// <see cref="Image"/>.SaveRawToMemoryStream(ImageFormat imageFormat, out Guid? g) extension method: 
        /// saves an Image to <see cref="MemoryStream"/> and return <see cref="Guid"/> of <see cref="ImageFormat"/> as out parameter
        /// </summary>
        /// <param name="img"><see cref="Image"/> to be processed by Exentsion Method</param>
        /// <param name="imageFormat"><see cref="ImageFormat"/></param>
        /// <param name="g"><see cref="Guid">out Guid g</see></param>
        /// <returns><see cref="MemoryStream"/></returns>
        public static MemoryStream SaveRawToMemoryStream(this System.Drawing.Image img, ImageFormat imageFormat, out Guid? g)
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
            Guid? imgFormGuid;
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
                Area23Log.LogOriginMsgEx("Extensions", "ToByteArray(this Image img)", exImgFormat); 
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
                Area23Log.LogOriginMsgEx("Extensions", "ToByteArray(this Image img)", ex);
            }

            return bytes;

        }

        /// <summary>
        /// <see cref="Image"/>.ToBase64() extension method: converts <see cref="Image"/> to base64 string
        /// </summary>
        /// <param name="img">this <see cref="Image"/></param>
        /// <returns>base64 encoded <see cref="string?"/></returns>
        public static string? ToBase64(this Image img)
        {
            string? base64 = null;
            byte[] bytes;
            try
            {
                bytes = img.ToByteArray();
                base64 = Convert.ToBase64String(bytes, Base64FormattingOptions.None); // Crypt.EnDeCoding.Base64.Encode(bytes);
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("Extensions", "ToBase64(this Image img)", ex);
            }

            return base64;
        }

        #endregion System.Drawing.Image extensions


        #endregion System.Drawing.Image extensions

        #region Mutex extensions


        #endregion

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
            if (string.IsNullOrWhiteSpace(strInput))
            {
                return false;
            }
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

        public static bool IsSameIp(this IPAddress ip1, IPAddress ip2, AddressFamily? addrFamily = null)
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
        public static T[] SwapTPositions<T>(this T[] tarray, int posA, int posB, bool exceptionOnNullable = false)
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


        /// <summary>
        /// ArrayIndexOf<T> generic extension to find the first or last occurence of a T value in T[] tarray
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tarray">generic T array</param>
        /// <param name="tvalue">T value to find</param>
        /// <param name="fromStart">if true, begin search at start (default), if false begin searching from end to start</param>
        /// <returns>if found, first or last index postition of Tvalue, if not found -1</returns>
        public static int ArrayIndexOf<T>(this T[] tarray, T tvalue, bool fromStart = true)
        {
            if (tarray != null && tarray.Length > 0)
            {
                for (int cnt = (fromStart) ? 0 : tarray.Length - 1; (cnt < tarray.Length && fromStart) || (cnt >= 0 && !fromStart); cnt += fromStart ? 1 : -1)
                {
                    if (tarray[cnt].Equals(tvalue))
                        return cnt;
                }
            }
            return -1;
        }

        /// <summary>
        /// FirstIndexOf<T> generic search in a T array from starting at begin as usual
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tarray">generic T array</param>
        /// <param name="tvalue">generic T value to find</param>
        /// <returns>if found, first  index of occurence of T value, if not found -1</returns>
        public static int FirstIndexOf<T>(this T[] tarray, T tvalue) => tarray.ArrayIndexOf(tvalue, true);

        /// <summary>
        /// LastIndexOf<T> generic search in a T array from starting at end
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tarray">generic T array</param>
        /// <param name="tvalue">generic T value to find</param>
        /// <returns>if found, last index of occurence of T value, if not found -1</returns>
        public static int LastIndexOf<T>(this T[] tarray, T tvalue) => tarray.ArrayIndexOf(tvalue, false);

        /// <summary>
        /// IndicesOf<T> generic search that returns all indices of occurence of T value inside T array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tarray">generic T arreay</param>
        /// <param name="tvalue">T value to find</param>
        /// <returns>is tarray is null, null, if not fount an empty index array int[0], else inidices array int[]</returns>
        public static int[]? IndicesOf<T>(this T[] tarray, T tvalue)
        {
            List<int>? indices = null;
            if (tarray != null && tarray.Length > 0)
            {
                indices = new List<int>();
                for (int cnt = 0; cnt < tarray.Length; cnt++)
                {
                    if (tarray[cnt].Equals(tvalue))
                        indices.Add(cnt);
                }
                return indices.ToArray();
            }
            return null;
        }

        public static int[]? IndicesIn<T>(this T[] haystack, T[] needle)
        {
            List<int>? indices = null;
            if (haystack != null && haystack.Length > 0 && needle != null && needle.Length > 0 && needle.Length < haystack.Length)
            {
                indices = new List<int>();
                T needleStartValue = needle[0];
                int nextNeedleRef = -1;

                for (int chay = 0; chay < haystack.Length;)
                {
                    if (haystack[chay].Equals(needleStartValue))
                    {
                        nextNeedleRef = chay;
                        for (int ineedle = 1; ineedle < needle.Length; ineedle++)
                        {
                            if (haystack[chay + ineedle].Equals(needleStartValue)) // next beginning sequene of needle found
                                nextNeedleRef = chay + ineedle;

                            if (haystack[chay + ineedle].Equals(needle[ineedle]))
                            {
                                // found next part of needle
                                if (ineedle < needle.Length - 1) 
                                    continue;   // continue searching to end of needle
                                indices.Add(chay);
                            }

                            // needle really found or really not found, but NOT might complete at later positions
                            chay = (nextNeedleRef > chay) ? nextNeedleRef : chay + ineedle;
                            break;
                        }
                    }
                    else chay++; // increment in case of not found
                }                  
                return indices.ToArray();
            }
            return null;
        }

        #endregion genericsT_extensions

    }


}
