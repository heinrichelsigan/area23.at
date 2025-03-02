using Area23.At.Framework.Library.CqrXs.CqrMsg;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.Util
{

    public class JsonChatRoom
    {
        static object _lock = new object();
        static HashSet<CqrContact> _contacts;

        public string JsonChatRoomNumber { get; set; } = System.DateTime.Now.ToString();

        internal string JsonChatRoomFileName { get => LibPaths.SystemDirJsonPath + JsonChatRoomNumber; }

        public JsonChatRoom(string jsonChatRoomNumber)
        {
            JsonChatRoomNumber = jsonChatRoomNumber;
        }


        public FullSrvMsg<string> LoadJsonChatRoom(string chatRoomNumber)
        {
            FullSrvMsg<string> fullSrvMsg;

            JsonChatRoomNumber = chatRoomNumber;
            if (!System.IO.File.Exists(JsonChatRoomFileName))
                return null;

            lock (_lock)
            {
                string jsonText = System.IO.File.ReadAllText(JsonChatRoomFileName);
                FullSrvMsg<string> srvMsg1 = new FullSrvMsg<string>();
                srvMsg1.FromJson(jsonText);
                fullSrvMsg = JsonConvert.DeserializeObject<FullSrvMsg<string>>(jsonText);
                if (!string.IsNullOrEmpty(fullSrvMsg.Message) && fullSrvMsg.Message.Equals(JsonChatRoomNumber))
                    HttpContext.Current.Application["ChatRoom"] = fullSrvMsg;
            }
            return fullSrvMsg;
        }

        public void SaveJsonChatRoom(FullSrvMsg<string> fullSrvMsg)
        {
            JsonSerializerSettings jsets = new JsonSerializerSettings();
            jsets.Formatting = Formatting.Indented;
            string jsonString = JsonConvert.SerializeObject(fullSrvMsg, Formatting.Indented);
            if (string.IsNullOrEmpty(fullSrvMsg.TContent))
                fullSrvMsg.TContent = System.DateTime.Now.ToString();
            if (!fullSrvMsg.TContent.EndsWith(".json"))
                fullSrvMsg.TContent += ".json";


            System.IO.File.WriteAllText(JsonChatRoomFileName, jsonString);
            HttpContext.Current.Application["ChatRoom"] = fullSrvMsg;
        }

        public bool DeleteJsonChatRoom(string chatRoomNumber)
        {
            FullSrvMsg<string> fullSrvMsg;

            JsonChatRoomNumber = chatRoomNumber;
            if (!System.IO.File.Exists(JsonChatRoomFileName))
                return true;

            lock (_lock)
            {
                try
                {
                    System.IO.File.Delete(JsonChatRoomFileName);
                }
                catch (Exception e)
                {
                    Area23Log.LogStatic($"Error deleting chat room {e.Message}");
                    return false;
                }
            }

            return true;
        }

    }
}