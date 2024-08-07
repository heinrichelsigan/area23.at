﻿using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Area23.At.Framework.Library
{
    public static class Extensions
    {
        #region primitive types extensions

        /// <summary>
        /// Checks, if a double is a round number
        /// </summary>
        /// <param name="d"></param>
        /// <returns>true, if it's integer number</returns>
        public static bool IsRoundNumber(this double d)
        {
            return (Math.Truncate(d) == d || Math.Round(d) == d);
        }

        public static long ToLong(this double d)
        {
            return Convert.ToInt64(d);
        }

        public static bool IsNan(this double d)
        {
            return double.IsNaN(d);
        }

        public static System.Drawing.Color ToColor(this byte[] rgb)
        {
            if (rgb == null || rgb.Length < 1)
                return System.Drawing.Color.Transparent;
            if (rgb.Length == 1)
                return System.Drawing.Color.FromArgb(rgb[0], 0xff, 0xff);
            else if (rgb.Length == 2)
                return System.Drawing.Color.FromArgb(rgb[0], rgb[1], 0xff);
            return System.Drawing.Color.FromArgb(rgb[0], rgb[1], rgb[2]);
        }

        /// <summary>
        /// FromHtmlToColor gets color from hexadecimal rgb string
        /// </summary>        
        /// <param name="htmlHex">hexadecimal rgb string rrggbb or with starting #, like #rrggbb</param>
        /// <returns>Color, that was defined by hexadecimal #rrggbb string</returns>
        public static System.Drawing.Color FromHtmlToColor(this string htmlHex)
        {
            if (String.IsNullOrWhiteSpace(htmlHex) || htmlHex.Length < 6 || htmlHex.Length > 7)
                throw new ArgumentException(
                    String.Format("System.Drawing.Color String.FromHtmlToColor(this string htmlHex = {0}), hex must be an rgb string in format \"#rrggbb\" or \"rrggbb\"", htmlHex));

            string rgbWork = htmlHex.TrimStart("#".ToCharArray());

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

        #endregion primitive types extensions

        #region DateTime extensions

        /// <summary>
        /// Area23Date extension method for DateTime
        /// </summary>
        /// <param name="dateTime"><see cref="DateTime"/></param>
        /// <returns>formatted date <see cref="string"/></returns>
        public static string Area23Date(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Area23DateTime extension method for DateTime
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
        /// Area23DateTimeWithSeconds extension method for DateTime
        /// </summary>
        /// <param name="dateTime">d</param>
        /// <returns><see cref="string"/> formatted date time including seconds</returns>
        public static string Area23DateTimeWithSeconds(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd_HH:mm:ss");
        }

        public static string Area23DateTimeWithMillis(this DateTime dateTime)
        {
            string formatted = String.Format("{0:yyyyMMdd_HHmmss}_{1}", dateTime, dateTime.Millisecond);
            // return formatted;
            return dateTime.ToString("yyyyMMdd_HHmmss_") + dateTime.Millisecond;
        }

        #endregion DateTime extensions

        #region byte[] and stream extensions

        /// <summary>
        /// Extension method for <see cref="System.IO.Stream"/>
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

        /// <summary>
        /// GetImageMimeType - auto detect mime type of an image inside an binary byte[] array
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
        /// byte[} extension ToFile - writes a byte array to a file
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

        #endregion byte[] and stream extensions

        #region System.Exception extensions

        /// <summary>
        /// ToLogMsg - extension method to format an exception to a well formatted logging message
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
        /// FromHtml gets color from hexadecimal rgb string html standard
        /// </summary>
        /// <param name="color">System.Drawing.Color.FromHtml(string hex) extension method</param>
        /// <param name="hex">hexadecimal rgb string with starting #</param>
        /// <returns>Color, that was defined by hexadecimal html standarized #rrggbb string</returns>
        public static System.Drawing.Color FromHtml(this System.Drawing.Color color, string hex)
        {
            if (String.IsNullOrWhiteSpace(hex) || hex.Length != 7 || !hex.StartsWith("#"))
                throw new ArgumentException(
                    String.Format("System.Drawing.Color.FromHtml(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", hex));

            color = System.Drawing.ColorTranslator.FromHtml(hex);
            return color;
        }

        /// <summary>
        /// FromRGB gets color from R G B
        /// </summary>
        /// <param name="color">System.Drawing.Color.FromXrgb(string hex) extension method</param>
        /// <param name="r">red byte</param>
        /// <param name="g">green byte</param>
        /// <param name="b">blue byte</param>
        /// <returns>Color, that was defined by hexadecimal rgb string</returns>
        /// <exception cref="ArgumentException"></exception>
        public static System.Drawing.Color FromRGB(this System.Drawing.Color color, byte r, byte g, byte b)
        {
            System.Drawing.Color _color = System.Drawing.Color.FromArgb((int)r, (int)g, (int)b);
            return _color;
        }


        /// <summary>
        /// <see cref="System.Drawing.Color">System.Drawing.Color extension method</see> IsInLevenSteinDistance
        /// </summary>
        /// <param name="px0"></param>
        /// <param name="disCol"></param>
        /// <returns>true, if this baseColor is inside LevenStein distance to Color disColor</returns>
        public static bool IsInLevenSteinDistance(this System.Drawing.Color baseCol, System.Drawing.Color disCol)
        {
            if (disCol.R == baseCol.R && disCol.G == baseCol.G && disCol.B == baseCol.B) // exact match => return true;
                return true;

            System.Drawing.Color lvstCol = disCol.FromRGB(disCol.R, disCol.G, disCol.B);

            for (int rls = 0; Math.Abs(rls) < 4; rls = (rls >= 0) ? (0 - (++rls)) : Math.Abs(rls))
            {
                for (int gls = 0; Math.Abs(gls) < 4; gls = (gls >= 0) ? (0 - (++gls)) : Math.Abs(gls))
                {
                    for (int bls = 0; Math.Abs(bls) < 4; bls = (bls >= 0) ? (0 - (++bls)) : Math.Abs(bls))
                    {
                        byte r = ((byte)(disCol.R + rls) >= 0xff) ? (byte)(0xff) : (byte)(disCol.R + rls);
                        byte g = ((byte)(disCol.G + gls) >= 0xff) ? (byte)(0xff) : (byte)(disCol.G + gls);
                        byte b = ((byte)(disCol.B + bls) >= 0xff) ? (byte)(0xff) : (byte)(disCol.B + bls);
                        lvstCol = lvstCol.FromRGB(r, g, b);

                        if ((((lvstCol.R + rls) == baseCol.R) && lvstCol.G == baseCol.G && lvstCol.B == baseCol.B) ||
                            ((lvstCol.R == baseCol.R) && (lvstCol.G + gls) == baseCol.G && lvstCol.B == baseCol.B) ||
                            ((lvstCol.R == baseCol.R) && lvstCol.G == baseCol.G && (lvstCol.B + bls) == baseCol.B) ||
                            (((lvstCol.R + rls) == baseCol.R) && (lvstCol.G + gls) == baseCol.G && lvstCol.B == baseCol.B) ||
                            (((lvstCol.R + rls) == baseCol.R) && lvstCol.G == baseCol.G && (lvstCol.B + bls) == baseCol.B) ||
                            ((lvstCol.R == baseCol.R) && (lvstCol.G + gls) == baseCol.G && (lvstCol.B + bls) == baseCol.B) ||
                            (((lvstCol.R + rls) == baseCol.R) && (lvstCol.G + gls) == baseCol.G && (lvstCol.B + bls) == baseCol.B))
                            return true;
                    }
                }
            }
            // not in LevenStein disctance
            return false;
        }


        /// <summary>
        /// Extension method Color.ToXrgb() converts current color to hex string 
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

        #endregion System.Drawing.Color extensions
    }

}