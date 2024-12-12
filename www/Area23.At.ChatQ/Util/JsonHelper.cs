using Area23.At.Framework.Library;
using Area23.At.ChatQ.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Area23.At.ChatQ.Util
{
    /// <summary>
    /// JsonHelper class for reading and writing json serialized store file.
    /// </summary>
    public static class JsonHelper
    {
        internal static string JsonFileName
        {
            get
            {
                string loadFileName = System.IO.Path.Combine(LibPaths.QrDirPath, Constants.JSON_SAVE_FILE);
                if (!File.Exists(loadFileName))
                {
                    loadFileName = AppContext.BaseDirectory.
                        Replace(LibPaths.SepChar + Constants.BIN_DIR, "").Replace(LibPaths.SepChar + Constants.OBJ_DIR, "").
                        Replace(LibPaths.SepChar + Constants.RELEASE_DIR, "").Replace(LibPaths.SepChar + Constants.DEBUG_DIR, "");
                    loadFileName += (loadFileName.EndsWith(LibPaths.SepChar)) ? "" : LibPaths.SepChar;
                    loadFileName += Constants.QR_DIR + LibPaths.SepChar + Constants.JSON_SAVE_FILE;
                }
                if (!File.Exists(loadFileName))
                {
                    loadFileName = AppDomain.CurrentDomain.BaseDirectory.
                        Replace(LibPaths.SepChar + Constants.BIN_DIR, "").Replace(LibPaths.SepChar + Constants.OBJ_DIR, "").
                        Replace(LibPaths.SepChar + Constants.RELEASE_DIR, "").Replace(LibPaths.SepChar + Constants.DEBUG_DIR, "");
                    loadFileName += (loadFileName.EndsWith(LibPaths.SepChar)) ? "" : LibPaths.SepChar;
                    loadFileName += Constants.QR_DIR + LibPaths.SepChar + Constants.JSON_SAVE_FILE;
                }
                return loadFileName;
            }
        }

        internal static Dictionary<string, Uri> ShortenMapJson
        {
            get
            {
                Dictionary<string, Uri> tmpDict = null;
                try
                {
                    

                    string jsonText = File.ReadAllText(JsonFileName);
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

                Area23Log.LogStatic("urlshorter dict count: " + tmpDict.Count);
                HttpContext.Current.Application[Constants.APP_NAME] = tmpDict;
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
            ShortenMapJson = saveDict;

        }

        
    }
}