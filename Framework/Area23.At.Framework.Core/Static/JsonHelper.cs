using Area23.At.Framework.Core.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Area23.At.Framework.Core.Static
{

    /// <summary>
    /// JsonHelper class for reading and writing json serialized store file.
    /// </summary>
    public static class JsonHelper
    {
        public static string JsonFileName
        {
            get
            {
                string loadFileName = Path.Combine(LibPaths.SystemDirQrPath, Constants.JSON_SAVE_FILE);
                if (!File.Exists(loadFileName))
                {
                    loadFileName = AppContext.BaseDirectory.
                        Replace(LibPaths.SepChar + Constants.BIN_DIR, "").Replace(LibPaths.SepChar + Constants.OBJ_DIR, "").
                        Replace(LibPaths.SepChar + Constants.RELEASE_DIR, "").Replace(LibPaths.SepChar + Constants.DEBUG_DIR, "");
                    loadFileName += loadFileName.EndsWith(LibPaths.SepChar) ? "" : LibPaths.SepChar;
                    loadFileName += Constants.QR_DIR + LibPaths.SepChar + Constants.JSON_SAVE_FILE;
                }
                if (!File.Exists(loadFileName))
                {
                    loadFileName = AppDomain.CurrentDomain.BaseDirectory.
                        Replace(LibPaths.SepChar + Constants.BIN_DIR, "").Replace(LibPaths.SepChar + Constants.OBJ_DIR, "").
                        Replace(LibPaths.SepChar + Constants.RELEASE_DIR, "").Replace(LibPaths.SepChar + Constants.DEBUG_DIR, "");
                    loadFileName += loadFileName.EndsWith(LibPaths.SepChar) ? "" : LibPaths.SepChar;
                    loadFileName += Constants.QR_DIR + LibPaths.SepChar + Constants.JSON_SAVE_FILE;
                }
                return loadFileName;
            }
        }


        public static string JsonContactsFile
        {
            get
            {
                string loadFileName = System.IO.Path.Combine(LibPaths.SystemDirJsonPath, Constants.JSON_CONTACTS_FILE);
                if (!File.Exists(loadFileName))
                {
                    loadFileName = AppContext.BaseDirectory.
                        Replace(LibPaths.SepChar + Constants.BIN_DIR, "").Replace(LibPaths.SepChar + Constants.OBJ_DIR, "").
                        Replace(LibPaths.SepChar + Constants.RELEASE_DIR, "").Replace(LibPaths.SepChar + Constants.DEBUG_DIR, "");
                    loadFileName += (loadFileName.EndsWith(LibPaths.SepChar)) ? "" : LibPaths.SepChar;
                    loadFileName += Constants.RES_DIR + LibPaths.SepChar + Constants.JSON_CONTACTS_FILE;
                }
                if (!File.Exists(loadFileName))
                {
                    loadFileName = AppDomain.CurrentDomain.BaseDirectory.
                        Replace(LibPaths.SepChar + Constants.BIN_DIR, "").Replace(LibPaths.SepChar + Constants.OBJ_DIR, "").
                        Replace(LibPaths.SepChar + Constants.RELEASE_DIR, "").Replace(LibPaths.SepChar + Constants.DEBUG_DIR, "");
                    loadFileName += (loadFileName.EndsWith(LibPaths.SepChar)) ? "" : LibPaths.SepChar;
                    loadFileName += Constants.RES_DIR + LibPaths.SepChar + Constants.JSON_CONTACTS_FILE;
                }
                return loadFileName;
            }
        }


        public static Dictionary<string, Uri> ShortenMapJson
        {
            get
            {
                Dictionary<string, Uri>? tmpDict = null;
                try
                {


                    string jsonText = File.ReadAllText(JsonFileName);
                    tmpDict = JsonConvert.DeserializeObject<Dictionary<string, Uri>>(jsonText);
                }
                catch (Exception getMapEx)
                {
                    Area23Log.LogOriginMsgEx("JsonHelper", "ShortenMapJson.get", getMapEx);
                    tmpDict = null;
                }

                if (tmpDict == null)
                {
                    tmpDict = new Dictionary<string, Uri>();
                }

                Area23Log.LogOriginMsg("JsonHelper", "urlshorter dict count: " + tmpDict.Count);
                AppDomain.CurrentDomain.SetData(Constants.UTF8_JSON, tmpDict);
                return tmpDict;
            }
            set
            {
                JsonSerializerSettings jsets = new JsonSerializerSettings();
                jsets.Formatting = Formatting.Indented;
                string jsonString = JsonConvert.SerializeObject(value, Formatting.Indented);
                File.WriteAllText(JsonFileName, jsonString);
                AppDomain.CurrentDomain.SetData(Constants.UTF8_JSON, value);
            }
        }


        public static void SaveDictionaryToJson(Dictionary<string, Uri> saveDict)
        {
            ShortenMapJson = saveDict;

        }


        public static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if (strInput.StartsWith("{") && strInput.EndsWith("}") || //For object
                strInput.StartsWith("[") && strInput.EndsWith("]")) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }

}