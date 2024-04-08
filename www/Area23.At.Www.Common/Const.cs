using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Area23.At.Framework.Library;

namespace Area23.At.Www.Common
{
    /// <summary>
    /// static Const including static application settings
    /// </summary>
    public static class Const
    {
        #region private readonly static

        private static readonly string qrColorString = "#000000";
        private static readonly string backColorString = "#ffffff";
        
        #endregion private readonly static

        #region properties


        private static System.Globalization.CultureInfo locale = null;
        private static String defaultLang = null;

        /// <summary>
        /// Culture Info from HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE]
        /// </summary>
        public static System.Globalization.CultureInfo Locale
        {
            get
            {
                if (locale == null)
                {
                    defaultLang = "en";
                    try
                    {
                        if (HttpContext.Current.Request != null && HttpContext.Current.Request.Headers != null &&
                            HttpContext.Current.Request.Headers[Constants.ACCEPT_LANGUAGE] != null)
                        {
                            string firstLang = HttpContext.Current.Request.Headers[Constants.ACCEPT_LANGUAGE].
                                ToString().Split(',').FirstOrDefault();
                            defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                        }

                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                    catch (Exception)
                    {
                        defaultLang = "en";
                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                }
                return locale;
            }
        }

        public static string ISO2Lang { get => Locale.TwoLetterISOLanguageName; }

        
        public static string BackColorString
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.BACK_COLOR_STRING] != null) ?
                    (string)HttpContext.Current.Session[Constants.BACK_COLOR_STRING] : backColorString;                
            set
            {
                HttpContext.Current.Session[Constants.BACK_COLOR] = ColorFrom.FromHtml(value);
                HttpContext.Current.Session[Constants.BACK_COLOR_STRING] = value;
            }
        }

        
        public static string QrColorString
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.QR_COLOR_STRING] != null) ?
                    (string)HttpContext.Current.Session[Constants.QR_COLOR_STRING] : qrColorString;
            set
            {
                HttpContext.Current.Session[Constants.QR_COLOR] = ColorFrom.FromHtml(value);
                HttpContext.Current.Session[Constants.QR_COLOR_STRING] = value;
            }
        }

        public static System.Drawing.Color BackColor
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.BACK_COLOR] != null) ?
                    (System.Drawing.Color)HttpContext.Current.Session[Constants.BACK_COLOR] : ColorFrom.FromHtml(backColorString);
            set
            {
                if (value != null)
                {
                    HttpContext.Current.Session[Constants.BACK_COLOR] = value;
                    HttpContext.Current.Session[Constants.BACK_COLOR_STRING] = value.ToXrgb();
                }
                else
                {
                    HttpContext.Current.Session[Constants.BACK_COLOR_STRING] = backColorString;
                    HttpContext.Current.Session[Constants.BACK_COLOR] = ColorFrom.FromHtml(backColorString);
                }
            }
        }

        public static System.Drawing.Color QrColor
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.QR_COLOR] != null) ?
                    (System.Drawing.Color)HttpContext.Current.Session[Constants.QR_COLOR] : ColorFrom.FromHtml(qrColorString);           
            set
            {
                if (value != null)
                {                                        
                    HttpContext.Current.Session[Constants.QR_COLOR] = value;
                    HttpContext.Current.Session[Constants.QR_COLOR_STRING] = value.ToXrgb();
                }
                else
                {
                    HttpContext.Current.Session[Constants.QR_COLOR_STRING] = qrColorString;
                    HttpContext.Current.Session[Constants.QR_COLOR] = ColorFrom.FromHtml(qrColorString);
                }
            }                
        } 

        public static bool FortuneBool
        {
            get
            {
                if (HttpContext.Current.Session[Constants.FORTUNE_BOOL] == null)
                    HttpContext.Current.Session[Constants.FORTUNE_BOOL] = false;
                else
                    HttpContext.Current.Session[Constants.FORTUNE_BOOL] = !((bool)HttpContext.Current.Session[Constants.FORTUNE_BOOL]);
                
                return (bool)HttpContext.Current.Session[Constants.FORTUNE_BOOL];
            }
        }        

        #endregion properties
    }
}