using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Www.S.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Area23.At.Www.S.Util
{

    /// <summary>
    /// JsonHelper wrapper class for reading and writing json serialized store file.
    /// </summary>
    public static class JsonHelper
    {

        static readonly object lock0 = new object();

        private static string loadFileName = "";
        public static string JsonFileName
        {
            get
            {
                if (!string.IsNullOrEmpty(loadFileName) && File.Exists(loadFileName))
                    return loadFileName;

                loadFileName = System.IO.Path.Combine(LibPaths.SystemDirJsonPath, Constants.JSON_SAVE_FILE);
                if (File.Exists(loadFileName))
                    return loadFileName;

                if (!File.Exists(loadFileName))
                {
                    loadFileName = AppContext.BaseDirectory.
                        Replace(LibPaths.SepChar + Constants.BIN_DIR, "").Replace(LibPaths.SepChar + Constants.OBJ_DIR, "").
                        Replace(LibPaths.SepChar + Constants.RELEASE_DIR, "").Replace(LibPaths.SepChar + Constants.DEBUG_DIR, "");
                    loadFileName += (loadFileName.EndsWith(LibPaths.SepChar)) ? "" : LibPaths.SepChar;
                    loadFileName += Constants.QR_DIR + LibPaths.SepChar + Constants.JSON_SAVE_FILE;
                    if (File.Exists(loadFileName))
                        return loadFileName;
                }
                if (!File.Exists(loadFileName) && ConfigurationManager.AppSettings["JsonUrlShortMap"] != null)
                {
                    loadFileName = ConfigurationManager.AppSettings["JsonUrlShortMap"].ToString();
                    if (File.Exists(loadFileName))
                        return loadFileName;
                }
                if (!File.Exists(loadFileName))
                {
                    loadFileName = AppDomain.CurrentDomain.BaseDirectory.
                        Replace(LibPaths.SepChar + Constants.BIN_DIR, "").Replace(LibPaths.SepChar + Constants.OBJ_DIR, "").
                        Replace(LibPaths.SepChar + Constants.RELEASE_DIR, "").Replace(LibPaths.SepChar + Constants.DEBUG_DIR, "");
                    loadFileName += (loadFileName.EndsWith(LibPaths.SepChar)) ? "" : LibPaths.SepChar;
                    loadFileName += Constants.RES_DIR + LibPaths.SepChar + Constants.JSON_SAVE_FILE;
                }

                return loadFileName;
            }
        }


        public static Dictionary<string, Uri> ShortenMapJson
        {
            get
            {
                Dictionary<string, Uri> tmpDict = null;

                try
                {
                    lock (lock0)
                    {
                        string jsonText = File.ReadAllText(JsonFileName);
                        tmpDict = JsonConvert.DeserializeObject<Dictionary<string, Uri>>(jsonText);
                    }
                }
                catch (Exception getMapEx)
                {
                    Area23Log.LogOriginMsgEx("JsonHelper", "Dictionary<string, Uri> ShortenMapJson.get throwed Exception " + getMapEx.GetType(), getMapEx);
                    tmpDict = null;
                }

                if (tmpDict == null)
                {
                    tmpDict = new Dictionary<string, Uri>();
                }

                HttpContext.Current.Application[Constants.APP_NAME] = tmpDict;

                Area23Log.LogOriginMsg("JsonHelper", "urlshorter dict count: " + tmpDict.Count);
                return tmpDict;
            }
            set
            {
                JsonSerializerSettings jsets = new JsonSerializerSettings();
                jsets.Formatting = Formatting.Indented;
                string jsonString = JsonConvert.SerializeObject(value, Formatting.Indented);
                System.IO.File.WriteAllText(JsonFileName, jsonString);
                HttpContext.Current.Application[Constants.APP_NAME] = value;
            }
        }

        
        
        internal static void SaveDictionaryToJson(Dictionary<string, Uri> saveDict)
        {
            Area23.At.Framework.Library.Static.JsonHelper.SaveDictionaryToJson(saveDict);
        }

    }

}