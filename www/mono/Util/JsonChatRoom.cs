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


        public FullSrvMsg<string> LoadJsonChatRoom(FullSrvMsg<string> fullSrvMsgIn, string chatRoomId)
        {
            JsonChatRoomNumber = chatRoomId;
            FullSrvMsg<string> fullServerMessage = null;
            string jsonText = null;
            if (!System.IO.File.Exists(JsonChatRoomFileName)) // we need to create chatroom
            {
                SaveJsonChatRoom(fullSrvMsgIn, chatRoomId);
            }

            lock (_lock)
            {
                jsonText = System.IO.File.ReadAllText(JsonChatRoomFileName);
                fullServerMessage = JsonConvert.DeserializeObject<FullSrvMsg<string>>(jsonText);
            }
            fullServerMessage._message = jsonText;
            // fullSrvMsgOut.RawMessage = jsonText;

            return fullServerMessage;
        }


        public FullSrvMsg<string> SaveJsonChatRoom(FullSrvMsg<string> fullSrvMsg, string chatRoomId)
        {
            string jsonString = "";
            lock (_lock)
            {
                if (!chatRoomId.Equals(this.JsonChatRoomNumber))
                    JsonChatRoomNumber = chatRoomId;

                if (!JsonChatRoomNumber.EndsWith(".json"))
                    JsonChatRoomNumber += ".json";

                fullSrvMsg.ChatRoomNr = JsonChatRoomNumber;
                fullSrvMsg.Sender.ChatRoomId = JsonChatRoomNumber;
                fullSrvMsg.RawMessage = "";
                fullSrvMsg._message = "";

                JsonSerializerSettings jsets = new JsonSerializerSettings();
                jsets.Formatting = Formatting.Indented;
                jsonString = JsonConvert.SerializeObject(fullSrvMsg, Formatting.Indented);
                System.IO.File.WriteAllText(JsonChatRoomFileName, jsonString);
            }

            fullSrvMsg._message = jsonString;
            // fullSrvMsg.RawMessage = jsonString; 


            return fullSrvMsg;
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