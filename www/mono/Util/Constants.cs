using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace area23.at.www.mono.Util
{
    public static class Constants
    {
        public const string APPNAME = "area23.at.www.mono";
        public const string APPDIR = "mono";
        public const string VERSION = "v2.11.33";
        public const string APPPATH = "https://area23.at/mono/test/";
        public const string GITURL = "https://github.com/heinrichelsigan/area23.at";
        public const string URLPIC = "https://area23.at/mono/test/res/img/";
        public const string URLPREFIX = "https://area23.at/mono/test/res/";
        public const string LOGDIR = "log";
        public const string RESFOLDER = "res";
        public const string ACCEPT_LANGUAGE = "Accept-Language";
        public const string FORTUNE_BOOL = "FORTUNE_BOOL";
        public const string DEFAULTMIMETYPE = "application/octet-stream";

        public const char DATEDELIM = '-';
        public const char WHITESPACE = ' ';
        public const char UNDERSCORE = '_';
        public const char ANNOUNCE = ':';

        private static System.Globalization.CultureInfo locale = null;
        private static String defaultLang = null;
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


        public static string DateArea23
        {
            get => DateTime.UtcNow.ToString("yyyy") + Constants.DATEDELIM +
                DateTime.UtcNow.ToString("MM") + Constants.DATEDELIM +
                DateTime.UtcNow.ToString("dd") + Constants.WHITESPACE +
                DateTime.UtcNow.ToString("HH") + Constants.ANNOUNCE +
                DateTime.UtcNow.ToString("mm") + Constants.ANNOUNCE + Constants.WHITESPACE;
        }

        public static string DateFile { get => DateArea23.Replace(WHITESPACE, UNDERSCORE).Replace(ANNOUNCE, UNDERSCORE); }

        private static string colorString = "#8c1157";
        public static string ColorString
        {
            get => colorString;
            set
            {
                qrColor = ColorFrom.FromXrgb(value);
                colorString = qrColor.ToXrgb();
            }
        }
         
        private static System.Drawing.Color qrColor;
        public static System.Drawing.Color QrColor
        {
            get
            {       
                try
                {
                    qrColor = ColorFrom.FromXrgb(colorString);
                }
                catch (Exception)
                {
                    colorString = "#8c1157";
                    qrColor = ColorFrom.FromXrgb(colorString);
                }
                return qrColor;
            }
            set
            {
                if (value != null)
                {
                    qrColor = value;
                    colorString = value.ToXrgb();
                }
                else
                {
                    colorString = "#8c1157";
                    qrColor = ColorFrom.FromHtml(colorString);
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

        public const string JsonSample = "{ \n " +
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
    }
}