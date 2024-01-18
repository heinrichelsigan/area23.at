﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.Util
{
    /// <summary>
    /// static Constants including static application settings
    /// </summary>
    public static class Constants
    {
        #region public const
        public const string APP_NAME = "Area23.At.Mono";
        public const string APP_DIR = "mono";
        public const string VERSION = "v2.11.33";
        public const string APP_PATH = "https://area23.at/mono/test/";
        public const string RPN_URL = "https://area23.at/mono/test/RpnCalc.aspx";
        public const string GIT_URL = "https://github.com/heinrichelsigan/area23.at";
        public const string URL_PIC = "https://area23.at/mono/test/res/img/";
        public const string URL_PREFIX = "https://area23.at/mono/test/res/";
        public const string LOG_DIR = "log";
        public const string RES_FOLDER = "res";
        public const string ACCEPT_LANGUAGE = "Accept-Language";
        public const string FORTUNE_BOOL = "FORTUNE_BOOL";
        public const string DEFAULT_MIMETYPE = "application/octet-stream";

        public const char ANNOUNCE = ':';
        public const char DATE_DELIM = '-';
        public const char WHITE_SPACE = ' ';
        public const char UNDER_SCORE = '_';

        public const string BACK_COLOR = "BackColor";
        public const string QR_COLOR = "QrColor";
        public const string BACK_COLOR_STRING = "BackColorString";
        public const string QR_COLOR_STRING = "QrColorString";

        public const string JSON_SAMPLE = "{ \n " +
            "\t\"quiz\": { \n " +
            "\t\t\"sport\": { \n " +
            "\t\t\t\"q1\": { \n " +
            "\t\t\t\t\"question\": \"Which one is correct team name in NBA?\", \n " +
            "\t\t\t\t\t\"options\": [ \n " +
            "\t\t\t\t\t\t\"New York Bulls\", \n " +
            "\t\t\t\t\t\t\t\"Los Angeles Kings\", \n " +
            "\t\t\t\t\t\t\t\"Golden State Warriros\", \n " +
            "\t\t\t\t\t\t\t\"Huston Rocket\" \n " +
            "\t\t\t\t\t\t], \n " +
            "\t\t\t\t\t\"answer\": \"Huston Rocket\" \n " +
            "\t\t\t\t} \n " +
            "\t\t\t}, \n " +
            "\t\t\"maths\": { \n " +
            "\t\t\t\"q1\": { \n " +
            "\t\t\t\t\"question\": \"5 + 7 = ?\", \n " +
            "\t\t\t\t\t\"options\": [ \n " +
            "\t\t\t\t\t\t\"10\", \n " +
            "\t\t\t\t\t\t\"11\", \n " +
            "\t\t\t\t\t\t\"12\", \n " +
            "\t\t\t\t\t\t\"13\" \n " +
            "\t\t\t\t\t], \n " +
            "\t\t\t\t\t\"answer\": \"12\" \n" +
            "\t\t\t\t}, \n " +
            "\t\t\t\"q2\": { \n " +
            "\t\t\t\t\"question\": \"12 - 8 = ?\", \n " +
            "\t\t\t\t\"options\": [ \n " +
            "\t\t\t\t\t\t\"1\", \n " +
            "\t\t\t\t\t\t\"2\", \n " +
            "\t\t\t\t\t\t\"3\", \n " +
            "\t\t\t\t\t\t\"4\" \n " +
            "\t\t\t\t\t\t], \n " +
            "\t\t\t\t\t\"answer\": \"4\" \n " +
            "\t\t\t\t}, \n " +
            "\t\t} \n " +
            "\t} \n " +
            "} \n ";
        
        #endregion public const

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
                            HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE] != null)
                        {
                            string firstLang = HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE].
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

        /// <summary>
        /// UTC DateTime Formated
        /// </summary>
        public static string DateArea23
        {
            get => DateTime.UtcNow.ToString("yyyy") + Constants.DATE_DELIM +
                DateTime.UtcNow.ToString("MM") + Constants.DATE_DELIM +
                DateTime.UtcNow.ToString("dd") + Constants.WHITE_SPACE +
                DateTime.UtcNow.ToString("HH") + Constants.ANNOUNCE +
                DateTime.UtcNow.ToString("mm") + Constants.ANNOUNCE + Constants.WHITE_SPACE;
        }

        /// <summary>
        /// UTC DateTime File Prefix
        /// </summary>
        public static string DateFile { get => DateArea23.Replace(WHITE_SPACE, WHITE_SPACE).Replace(ANNOUNCE, UNDER_SCORE); }

        private static readonly string backColorString = "#ffffff";
        public static string BackColorString
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[BACK_COLOR_STRING] != null) ?
                    (string)HttpContext.Current.Session[BACK_COLOR_STRING] : backColorString;                
            set
            {
                HttpContext.Current.Session[BACK_COLOR] = ColorFrom.FromXrgb(value);
                HttpContext.Current.Session[BACK_COLOR_STRING] = value;
            }
        }

        private static readonly string qrColorString = "#000000";
        public static string QrColorString
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[QR_COLOR_STRING] != null) ?
                    (string)HttpContext.Current.Session[QR_COLOR_STRING] : qrColorString;
            set
            {
                HttpContext.Current.Session[QR_COLOR] = ColorFrom.FromXrgb(value);
                HttpContext.Current.Session[QR_COLOR_STRING] = value;
            }
        }

        public static System.Drawing.Color BackColor
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[BACK_COLOR] != null) ?
                    (System.Drawing.Color)HttpContext.Current.Session[BACK_COLOR] : ColorFrom.FromXrgb(backColorString);
            set
            {
                if (value != null)
                {
                    HttpContext.Current.Session[BACK_COLOR] = value;
                    HttpContext.Current.Session[BACK_COLOR_STRING] = value.ToXrgb();
                }
                else
                {
                    HttpContext.Current.Session[BACK_COLOR_STRING] = backColorString;
                    HttpContext.Current.Session[BACK_COLOR] = ColorFrom.FromXrgb(backColorString);
                }
            }
        }

        public static System.Drawing.Color QrColor
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[QR_COLOR] != null) ?
                    (System.Drawing.Color)HttpContext.Current.Session[QR_COLOR] : ColorFrom.FromXrgb(qrColorString);           
            set
            {
                if (value != null)
                {
                    HttpContext.Current.Session[QR_COLOR_STRING] = backColorString;
                    HttpContext.Current.Session[QR_COLOR] = ColorFrom.FromXrgb(backColorString);
                }
                else
                {
                    HttpContext.Current.Session[QR_COLOR_STRING] = qrColorString;
                    HttpContext.Current.Session[QR_COLOR] = ColorFrom.FromXrgb(qrColorString);
                }
            }                
        } 

        public static bool FortuneBool
        {
            get
            {
                if (HttpContext.Current.Session[FORTUNE_BOOL] == null)
                    HttpContext.Current.Session[FORTUNE_BOOL] = false;
                else
                    HttpContext.Current.Session[FORTUNE_BOOL] = !((bool)HttpContext.Current.Session[FORTUNE_BOOL]);
                
                return (bool)HttpContext.Current.Session[FORTUNE_BOOL];
            }
        }        

        #endregion properties
    }
}