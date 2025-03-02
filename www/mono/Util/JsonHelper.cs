using Area23.At;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

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