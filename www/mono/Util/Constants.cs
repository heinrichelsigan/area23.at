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
        public const string FORTUNE_BOOL = "FORTUNE_BOOL";

        public const char DATEDELIM = '-';
        public const char WHITESPACE = ' ';
        public const string ANNOUNCE = ": ";

        private static System.Globalization.CultureInfo locale = null;
        public static System.Globalization.CultureInfo Locale
        {
            get
            {
                if (locale == null)
                {
                    try
                    {
                        string defaultLang = HttpContext.Current.Request.Headers["Accept-Language"].ToString();
                        string firstLang = defaultLang.Split(',').FirstOrDefault();
                        defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                    catch (Exception)
                    {
                        locale = new System.Globalization.CultureInfo("en");
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
                DateTime.UtcNow.ToShortTimeString() + Constants.ANNOUNCE;
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