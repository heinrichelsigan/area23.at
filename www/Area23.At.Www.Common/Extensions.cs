﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Area23.At.Www.Common
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

        #endregion primitive types extensions

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

            Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
            return _color;
        }

        /// <summary>
        /// FromXrgb gets color from hexadecimal rgb string
        /// </summary>
        /// <param name="color">System.Drawing.Color.FromXrgb(string hex) extension method</param>
        /// <param name="hex">hexadecimal rgb string with starting #</param>
        /// <returns>Color, that was defined by hexadecimal rgb string</returns>
        public static System.Drawing.Color FromXrgb(this System.Drawing.Color color, string hex)
        {
            // return Supu.Framework.Extensions.ColorFrom.FromXrgb(hex);
            if (String.IsNullOrWhiteSpace(hex) || hex.Length != 7 || !hex.StartsWith("#"))
                throw new ArgumentException(
                    String.Format("System.Drawing.Color.FromXrgb(string hex = {0}), hex must be an rgb string in format \"#rdgdbd\" like \"#3f230e\"!", hex));

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
        /// FromRGB gets color from R G B
        /// </summary>
        /// <param name="color">System.Drawing.Color.FromXrgb(string hex) extension method</param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns>Color, that was defined by hexadecimal rgb string</returns>
        /// <exception cref="ArgumentException"></exception>
        public static System.Drawing.Color FromRGB(this System.Drawing.Color color, uint r, uint g, uint b)
        {
            return System.Drawing.Color.FromArgb((int)r, (int)g, (int)b);
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