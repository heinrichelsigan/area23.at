using System;
using System.Collections.Generic;

namespace Area23.At.Mono.Util
{
    public static class JsonHelper
    {
        internal static string JsonFileName { get => Framework.Library.Static.JsonHelper.JsonFileName; }
        

        internal static Dictionary<string, Uri> ShortenMapJson
        {
            get => Framework.Library.Static.JsonHelper.ShortenMapJson;
            set => Framework.Library.Static.JsonHelper.ShortenMapJson = value;
        }


        internal static void SaveDictionaryToJson(Dictionary<string, Uri> saveDict)
        {
            Framework.Library.Static.JsonHelper.SaveDictionaryToJson(saveDict);
            return;
        }

    }

}