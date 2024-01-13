using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace area23.at.www.mono.Util
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
                    $"Supu.Framework.Extensions.ColorFrom.FromHtml(string hex = {hex}), hex must be an rgb string in format \"#rdgdbd\" like \"#3F230E\"!");

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
                    $"Supu.Framework.Extensions.ColorFrom.FromXrgb(string hex = {hex}), hex must be an rgb string in format \"#rdgdbd\" like \"#3F230E\"!");

            string rgbWork = hex.TrimStart("#".ToCharArray());
            int r = Convert.ToUInt16(rgbWork.Substring(0, 2).TrimStart("0".ToCharArray()), 16);
            int g = Convert.ToUInt16(rgbWork.Substring(2, 2).TrimStart("0".ToCharArray()), 16);
            int b = Convert.ToUInt16(rgbWork.Substring(4, 2).TrimStart("0".ToCharArray()), 16);

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        #endregion Extensions.ColorFrom static methods
    }
}