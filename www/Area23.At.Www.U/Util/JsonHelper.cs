using Area23.At.Framework.Library;
using Area23.At.Www.U;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Area23.At.Www.U.Util
{
    public static class JsonHelper
    {
        internal static Dictionary<long, Utf8Symbol> GetUtf8Dictionary()
        {
            Dictionary<long, Utf8Symbol> tmpDict = null;
            try
            {
                string loadFileName = System.IO.Path.Combine(LibPaths.Utf8PathDir, Constants.UTF8_JSON);
                string jsonText = File.ReadAllText(loadFileName);
                tmpDict = JsonConvert.DeserializeObject<Dictionary<long, Utf8Symbol>>(jsonText);
            }
            catch (Exception getMapEx)
            {
                Area23Log.LogStatic(getMapEx);
                tmpDict = null;
            }

            if (tmpDict == null || tmpDict.Count < 1)
            {
                tmpDict = new Dictionary<long, Utf8Symbol>(Utf8Dictionary.Uft8DictSingle);
            }

            HttpContext.Current.Application[Constants.APP_NAME] = tmpDict;
            return tmpDict;
        }


        internal static void SaveDictionaryToJson(Dictionary<long, Utf8Symbol> saveDict)
        {
            string saveFileName = System.IO.Path.Combine(LibPaths.Utf8PathDir, Constants.UTF8_JSON);
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