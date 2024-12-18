using Area23.At.Framework.Library;
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
        internal static string JsonFileName { get => Framework.Library.Util.JsonHelper.JsonFileName; }
        

        internal static Dictionary<string, Uri> ShortenMapJson
        {
            get => Framework.Library.Util.JsonHelper.ShortenMapJson;
            set => Framework.Library.Util.JsonHelper.ShortenMapJson = value;
        }


        internal static void SaveDictionaryToJson(Dictionary<string, Uri> saveDict)
        {
            Framework.Library.Util.JsonHelper.SaveDictionaryToJson(saveDict);
            return;
        }

    }

}