using Area23.At.Framework.Library;
using Area23.At.Www.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Area23.At.Www.S.Util
{
    public static class JsonHelper
    {
        internal static Dictionary<string, Uri> GetShortenMapFromJson()
        {
            Dictionary<string, Uri> tmpDict = null;
            try
            {
                string loadFileName = System.IO.Path.Combine(Paths.QrDirPath, Constants.JSON_SAVE_FILE);
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
            string saveFileName = System.IO.Path.Combine(Paths.QrDirPath, Constants.JSON_SAVE_FILE);
            JsonSerializerSettings jsets = new JsonSerializerSettings();
            jsets.Formatting = Formatting.Indented;            
            string jsonString = JsonConvert.SerializeObject(saveDict, Formatting.Indented);
            System.IO.File.WriteAllText(saveFileName, jsonString);

            HttpContext.Current.Application[Constants.APP_NAME] = saveDict;
        }


        internal static string GetUserHost()
        {
            string userHost = Constants.UNKNOWN;
            try
            {
                userHost = (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostName)) ?
                    HttpContext.Current.Request.UserHostName :
                    (HttpContext.Current.Request.UserHostAddress ?? "unknown");
            }
            catch (Exception ex)
            {
                userHost = "UNKNOWN";
                Area23Log.LogStatic(ex);
            }
            return userHost;
        }

        internal static void LogRequest(object sender, EventArgs e, string preMsg = "")
        {
            object oNull = (object)(Constants.SNULL);
            string logMsg = String.Format("{0} {1} object sender={2}, EventArgs e={3}",                
                GetUserHost(),
                preMsg ?? "",
                (sender ?? oNull).ToString(),
                (e ?? oNull).ToString());
            
            Area23Log.LogStatic(logMsg);
        }

        internal static string LogRequest(object sender, EventArgs e)
        {
            object oNull = (object)(Constants.SNULL);
            string logReq = String.Format("from {0} object sender={1}, EventArgs e={2}",
                GetUserHost(),
                (sender ?? oNull).ToString(),
                (e ?? oNull).ToString());
            
            return logReq;            
        }
    }
}