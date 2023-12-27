using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace area23.at.mono.test.Util
{
    public static class Constants
    {
        public const string APPNAME = "area23.at.mono.test";
        public const string APPDIR = "json";
        public const string VERSION = "v2.11.33";
        public const string APPPATH = "https://area23.at/mono/json";
        public const string GITURL = "https://github.com/heinrichelsigan/MonoJsonDeserializer";

        public const string FORTUNE_BOOL = "FORTUNE_BOOL";

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