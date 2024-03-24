using Area23.At.Web.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Area23.At.Web.S.Util
{
    public static class JsonHelper
    {
        internal static Dictionary<string, Uri> GetShortenMapFromJson()
        {
            Dictionary<string, Uri> tmpDict = null;
            try
            {
                string loadFileName = System.IO.Path.Combine(Paths.AppDirPath, Constants.JSON_SAVE_FILE);
                string jsonText = File.ReadAllText(loadFileName);
                tmpDict = JsonConvert.DeserializeObject<Dictionary<string, Uri>>(jsonText);
            }
            catch (Exception getMapEx)
            {
                Area23Log.LogStatic(getMapEx);
                tmpDict = null;
            }

            if (tmpDict == null)
            {
                tmpDict = new Dictionary<string, Uri>();
            }

            HttpContext.Current.Application[Constants.APP_NAME] = tmpDict;
            return tmpDict;
        }


        internal static void SaveDictionaryToJson(Dictionary<string, Uri> saveDict)
        {
            string saveFileName = System.IO.Path.Combine(Paths.AppDirPath, Constants.JSON_SAVE_FILE);
            string jsonString = JsonConvert.SerializeObject(saveDict);
            System.IO.File.WriteAllText(saveFileName, jsonString);

            HttpContext.Current.Application[Constants.APP_NAME] = saveDict;
        }
    }
}