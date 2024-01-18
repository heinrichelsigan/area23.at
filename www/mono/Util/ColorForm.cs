using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.Util
{
    /// <summary>
    /// Static class alternative for System.Drawing.Color Extension Methods
    /// </summary>
    public static class ColorFrom
    {
        #region Extensions.ColorFrom static methods

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
                    String.Format("Area23.At.Mono.Util.ColorForm.FromHtml(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", hex));

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
                    String.Format("Area23.At.Mono.Util.ColorForm.FromXrgb(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", hex));

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

        #endregion Extensions.ColorFrom static methods
    }
}