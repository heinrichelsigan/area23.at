using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Drawing;

namespace Area23.At.Framework.Library.Util
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
            string formatted = String.Format("{0:yyyyMMdd_HHmmss}_{1}", dateTime, dateTime.Millisecond);
            // return formatted;
            return dateTime.ToString("yyyyMMdd_HHmmss_") + dateTime.Millisecond;
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
                using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
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
                filePath = LibPaths.AppDirPath;
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
                Area23Log.LogStatic(ex);
            }

            if (System.IO.File.Exists(fullFileName))
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

                        if (needleIt >= (needle.Length - 1))
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
            byte[] bytes = EnDeCoding.Base64.Decode(base64);
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
            byte[] bytes = EnDeCoding.Base64.Decode(base64);
            using (MemoryStream ms = new MemoryStream(bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Flush();
                bitmap = new Bitmap(ms);
            }
            return (Image)bitmap;
        }

        /// <summary>
        /// <see cref="string"/>.FromHtmlToColor() extension methods: transforms hex #rrggbb string into <see cref="System.Drawing.Color"/>
        /// </summary>
        /// <param name="htmlRGBString"><see cref="string"/> to transform</param>
        /// <returns><see cref="System.Drawing.Color"/></returns>
        /// <exception cref="ArgumentException">invalid argument exception, in case of malformatted string</exception>
        public static System.Drawing.Color FromHtmlToColor(this string htmlRGBString)
        {
            if (String.IsNullOrWhiteSpace(htmlRGBString) || htmlRGBString.Length != 7 || !htmlRGBString.StartsWith("#"))
                throw new ArgumentException(
                    String.Format("System.Drawing.Color.FromHtml(string htmlRGBString = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", htmlRGBString));

            System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml(htmlRGBString);
            return _color;
        }


        public static string GetExtensionFromFileString(this string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !fileName.Contains("."))
                return null;
            int lastIdx = fileName.LastIndexOf('.');
            string ext = fileName.Substring(lastIdx);
            
            return ext;
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
        /// <see cref="System.Drawing.Image"/>.ToBase64() extension method: converts <see cref="System.Drawing.Image"/> to base64 string
        /// </summary>
        /// <param name="img">this <see cref="System.Drawing.Image"/></param>
        /// <returns>base64 encoded <see cref="string"/></returns>
        public static string ToBase64(this System.Drawing.Image img)
        {
            string base64 = null;
            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                ms.Flush();
                byte[] bytes = ms.ToArray();
                base64 = EnDeCoding.Base64.Encode(bytes);
            }

            return base64;
        }

        /// <summary>
        /// <see cref="System.Drawing.Image"/>.ToByteArray() extension method: converts <see cref="System.Drawing.Image"/> to byte array
        /// </summary>
        /// <param name="img">this <see cref="System.Drawing.Image"/></param>
        /// <returns><see cref="byte[]"/> array</returns>
        public static byte[] ToByteArray(this System.Drawing.Image img)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    img.Save(ms, img.RawFormat);
                    // ms.Flush();
                    byte[] bytes = ms.ToArray();
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }
            return (byte[])null;
        }

        #endregion System.Drawing.Image extensions

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


        #endregion genericsT_extensions

        #region serializer_xml_json

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

    }

    /// <summary>
    /// Static class alternative for System.Drawing.Color Extension Methods
    /// </summary>
    public static class ColorFrom
    {
        #region ColorFrom static methods

        /// <summary>
        /// FromHtml gets color from hexadecimal rgb string html standard
        /// static method Supu.Framework.Extensions.ColorFrom.FromHtml(string hex) 
        /// is an alternative to System.Drawing.Color.FromHtml(string hex) extension method
        /// </summary>
        /// <param name="hex">hexadecimal rgb string with starting #</param>
        /// <returns>Color, that was defined by hexadecimal html standarized #rrggbb string</returns>
        public static System.Drawing.Color FromHtml(string hex)
        {

            if (String.IsNullOrWhiteSpace(hex) || hex.Length != 7 || !hex.StartsWith("#"))
                throw new ArgumentException(
                    String.Format("Area23.At.Framework.Library.ColorForm.FromHtml(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", hex));

            Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
            return _color;
        }

        /// <summary>
        /// FromXrgb gets color from hexadecimal rgb string
        /// static method Supu.Framework.Extensions.ColorFrom.FromXrgb(string hex) 
        /// is an alternative to System.Drawing.Color.FromXrgb(string hex) extension method
        /// </summary>
        /// <param name="hex">hexadecimal rgb string with starting #</param>
        /// <returns>Color, that was defined by hexadecimal rgb string</returns>
        public static System.Drawing.Color FromXrgb(string hex)
        {
            if (String.IsNullOrWhiteSpace(hex) || hex.Length != 7 || !hex.StartsWith("#"))
                throw new ArgumentException(
                    String.Format("Area23.At.Framework.Library.ColorForm.FromXrgb(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", hex));

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

        #endregion ColorFrom static methods
    }

}