using Area23.At.Framework.Library.Cqr.Msg;
using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Area23.At.Mono.CqrJD;
using System.IO;
using Area23.At.Framework.Library.Cache;

namespace Area23.At.Mono.Util
{

    /// <summary>
    /// JsonChatRoom 
    /// </summary>
    public static class JsonChatRoom
    {

        #region static fields

        private static object _lock = new object();
        private static HashSet<string> _chatRooms;
        private static string _jsonChatRoomNumber;
        #endregion static fields

        #region static ctor

        /// <summary>
        /// static parameterless connstructor
        /// </summary>
        static JsonChatRoom()
        {
            List<string> jsonChatRooms = new List<string>();
            try
            {
                jsonChatRooms = ChatRoomNumbersFromFs();
            }
            catch (Exception exCtor)
            {
                Area23Log.LogStatic("static JsonChatRoom()", exCtor, "");
            }
            _chatRooms = new HashSet<string>(jsonChatRooms);
            _jsonChatRoomNumber = System.DateTime.Now.ToString();
        }

        #endregion static ctor

        #region static Load Save Delete

        /// <summary>
        /// Static LoadChatRoom
        /// </summary>
        /// <param name="cSrvMsgIn"><see cref="CSrvMsg{TC}"/></param>
        /// <param name="chatRoomNr">chatRoomNr</param>
        /// <returns>><see cref="CSrvMsg{TC}"/></returns>
        public static CSrvMsg<string> LoadChatRoom(CSrvMsg<string> cSrvMsgIn, string chatRoomNr)
        {
            string jsonCRoomFileName = GetJsonChatRoomFullPath(chatRoomNr);

            CSrvMsg<string> cServerMessage = null;
            string jsonText = null;
            if (!File.Exists(jsonCRoomFileName)) // we need to a create chatroom
            {
                CChatRoom chatRoom = cSrvMsgIn.CRoom ?? new CChatRoom(chatRoomNr, Guid.NewGuid(), DateTime.MinValue, DateTime.MinValue);
                chatRoom.ChatRoomNr = chatRoomNr;
                SaveChatRoom(cSrvMsgIn, chatRoom);
            }

            lock (_lock)
            {
                jsonText = File.ReadAllText(jsonCRoomFileName);
                cServerMessage = JsonConvert.DeserializeObject<CSrvMsg<string>>(jsonText);
            }

            return SerializeCSrvMsg(cServerMessage, out string serJsonString);
        }

        /// <summary>
        /// static SaveChatRoom
        /// </summary>
        /// <param name="cSrvMsg"><see cref="CSrvMsg<string>"/></param>
        /// <param name="chatRoom"><see cref="CChatRoom"/></param>
        /// <returns></returns>
        public static CSrvMsg<string> SaveChatRoom(CSrvMsg<string> cSrvMsg, CChatRoom chatRoom)
        {
            string jsonString = "";
            string chatRoomNumber = chatRoom.ChatRoomNr;
            lock (_lock)
            {
                if (!chatRoomNumber.EndsWith(".json"))
                    chatRoomNumber += ".json";

                cSrvMsg.CRoom = new CChatRoom(chatRoomNumber, chatRoom.ChatRuid, chatRoom.LastPushed, chatRoom.LastPolled)
                {
                    MsgType = chatRoom.MsgType,
                    TicksLong = chatRoom.TicksLong,
                    _message = chatRoomNumber,
                    _hash = chatRoom.Hash,
                    Md5Hash = chatRoom.Md5Hash,
                    CBytes = chatRoom.CBytes
                };

                if (cSrvMsg.CRoom != null && !string.IsNullOrEmpty(cSrvMsg.CRoom.ChatRoomNr))
                    cSrvMsg.Sender._message = cSrvMsg.CRoom.ChatRoomNr;

                string jsonCRoomFileName = GetJsonChatRoomFullPath(chatRoomNumber);
                SerializeCSrvMsg(cSrvMsg, out jsonString, true);
                // System.IO.File.WriteAllText(jsonCRoomFileName, jsonString);
            }

            cSrvMsg.SerializedMsg = jsonString;

            return cSrvMsg;
        }

        /// <summary>
        /// Deletes chatroom from cache and skeleton without messages from filesystem 
        /// </summary>
        /// <param name="chatRoomNr">chat room number</param>
        /// <returns></returns>
        public static bool DeleteChatRoom(string chatRoomNr)
        {
            string jsonChatRoomFileName = GetJsonChatRoomFullPath(chatRoomNr);

            DeleteJsonChatRoomFromCache(chatRoomNr);

            lock (_lock)
            {
                if (System.IO.File.Exists(jsonChatRoomFileName)) // we need to create chatroom
                {
                    try
                    {
                        System.IO.File.Delete(jsonChatRoomFileName);
                    }
                    catch (Exception exDelChatRoomFromFs)
                    {
                        Area23Log.LogStatic($"DeleteChatRoom(string chatRoomNr = {chatRoomNr}): Error deleting chat room ", exDelChatRoomFromFs, "");
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion static Load Save Delete

        #region basic static members
        /// <summary>
        /// SerializeCSrvMsg serializes a <see cref="CSrvMsg{string}"/> to file system 
        /// TODO: we need only <see cref="CChatRoom"/> to merialize => FIX-IT
        /// </summary>
        /// <param name="cSrvMsg">a full CSrvMsg</param>
        /// <param name="serializedJsonString">out parameter of serialized string</param>
        /// <param name="wrtieJsonToFs">if true, serialize it to fs, default false</param>
        /// <returns>CSrvMsg with actual serialized string</returns>
        public static CSrvMsg<string> SerializeCSrvMsg(CSrvMsg<string> cSrvMsg, out string serializedJsonString, bool wrtieJsonToFs = false)
        {
            // TODO: we need only<see cref = "CChatRoom" /> to merialize => FIX  IT!!!
            serializedJsonString = string.Empty;
            if (cSrvMsg != null)
            {
                cSrvMsg.SerializedMsg = string.Empty;
                JsonSerializerSettings jsets = new JsonSerializerSettings();
                jsets.Formatting = Formatting.Indented;
                jsets.MaxDepth = 16;
                serializedJsonString = JsonConvert.SerializeObject(cSrvMsg, Formatting.Indented);

                if (wrtieJsonToFs && cSrvMsg.CRoom != null && !string.IsNullOrEmpty(cSrvMsg.CRoom.ChatRoomNr))
                {
                    string fullPath = JsonChatRoom.GetJsonChatRoomFullPath(cSrvMsg.CRoom.ChatRoomNr);
                    File.WriteAllText(fullPath, serializedJsonString);
                }
            }

            cSrvMsg.SerializedMsg = serializedJsonString;
            return cSrvMsg;
        }

        /// <summary>
        /// GetJsonChatRoomFullPath gets full path in filesystem for json chat room skeleton file
        /// </summary>
        /// <param name="jsonChatRoomNr">name or number of chat room</param>
        /// <returns></returns>
        public static string GetJsonChatRoomFullPath(string jsonChatRoomNr)
        {
            string fullPath = Path.Combine(LibPaths.SystemDirJsonPath, jsonChatRoomNr);

            return fullPath;
        }

        /// <summary>
        /// ChatRoomCheckPermission
        /// Validates, if a user has permission to poll or to push messages in the chat room by the following steps:
        /// 1. chat room number in encrypted, now decrypted msg from webservice must be ident with chat room number from json file
        /// 2. sender email from  encrypted, now decrypted msg from webservice must much
        /// 2.a. either creator invitor of chat room
        /// 2.b. on of the invited persons in invitation
        /// </summary>
        /// <param name="cSrvMsg"><see cref="CSrvMsg{string}"/> decoded from <see cref="CqrService.CqrService"/> Webservice</param>
        /// <param name="chatRoomMsg"><see cref="CSrvMsg{string}"/> generated from chat room json</param>
        /// <param name="chatRoomNr"><see cref="string"/> chat room number of chat room</param>
        /// <param name="isValid"><see cref="bool"/>true, if person is allowed to push or receive msg from / to chat room</param>
        /// <param name="isClosingRequest"><see cref="bool"/>default false, true when closing and deleting chat room</param>
        /// <returns>modified chatRoomMsg <see cref="CSrvMsg{string}"/></returns>        
        public static CSrvMsg<string> CheckPermission(CSrvMsg<string> cSrvMsg, CSrvMsg<string> chatRoomMsg, string chatRoomNr, out bool isValid, bool isClosingRequest = false)
        {
            isValid = false;
            if (chatRoomNr.Equals(chatRoomMsg.CRoom.ChatRoomNr, StringComparison.CurrentCultureIgnoreCase)) // validate chat number
            {
                chatRoomMsg.TContent = string.Empty;
                chatRoomMsg._message = chatRoomNr;

                if ((!string.IsNullOrEmpty(cSrvMsg.Sender.Email) && cSrvMsg.Sender.Email.Equals(chatRoomMsg.Sender.Email, StringComparison.CurrentCultureIgnoreCase)) ||
                    (!string.IsNullOrEmpty(cSrvMsg.Sender.NameEmail) && cSrvMsg.Sender.NameEmail.Equals(chatRoomMsg.Sender.NameEmail, StringComparison.InvariantCultureIgnoreCase)))
                {
                    isValid = true;
                    return cSrvMsg;
                }
                if (!isClosingRequest)
                {
                    foreach (CContact c in chatRoomMsg.Recipients)
                    {
                        if (cSrvMsg.Sender.NameEmail.Equals(c.NameEmail, StringComparison.CurrentCultureIgnoreCase) ||
                            cSrvMsg.Sender.Email.Equals(c.Email, StringComparison.CurrentCultureIgnoreCase) ||
                            (cSrvMsg.Sender.Name.Equals(c.Name, StringComparison.CurrentCultureIgnoreCase) && cSrvMsg.Sender.Cuid == c.Cuid))
                        {
                            isValid = true;
                            return cSrvMsg;
                        }
                    }
                }
            }

            return cSrvMsg;
        }
        #endregion basic static members

        #region static cache operations

        /// <summary>
        /// Loads a list of json chat rooms from fs
        /// </summary>
        /// <returns><see cref="List{string}"/></returns>
        public static List<string> ChatRoomNumbersFromFs()
        {

            List<string> chatRooms = new List<string>();
            string[] csr = new string[0];

            try
            {
                csr = Directory.GetFiles(LibPaths.SystemDirJsonPath, "room*.json");
                string file = "";
                foreach (string filedir in csr)
                {
                    file = Path.GetFileName(filedir);
                    chatRooms.Add(file);
                }
            }
            catch (Exception exChatRoomFs)
            {
                Area23Log.LogStatic("ChatRoomNumbersFromFs()", exChatRoomFs, "");
            }

            SetJsonChatRoomsToCache(chatRooms);


            return chatRooms;
        }

        /// <summary>
        /// GetJsonChatRoomsFromCache loads chatroom list from ApplicationState in proc or redis cache
        /// if no entries are found there, GetJsonChatRoomsFromCache() loads chatroom list from filesystem
        /// and stores it either in ApplicationState in proc or in redis elastic cache
        /// </summary>
        /// <returns><see cref="List{string}<">List of chat room names / keys</see></returns>
        public static List<string> GetJsonChatRoomsFromCache()
        {
            List<string> chatRooms = new List<string>();

            try
            {
                chatRooms = (List<string>)MemoryCache.CacheDict.GetValue<List<string>>(Constants.CHATROOMS);
            }
            catch (Exception exLoadFromCache)
            {
                Area23Log.LogStatic("GetJsonChatRoomsFromCache(): Failed to load chatrooms from cache", exLoadFromCache, "");
            }


            if (chatRooms == null || chatRooms.Count < 1)
            {
                chatRooms = ChatRoomNumbersFromFs();
                if (chatRooms != null && chatRooms.Count > 0)
                    SetJsonChatRoomsToCache(chatRooms);
            }

            return chatRooms;

        }

        /// <summary>
        /// SetJsonChatRoomsToCache persist list of chat rooms of 
        /// </summary>
        /// <param name="chatRooms"><see cref="List{string}">list of chat rooms</see></param>
        public static void SetJsonChatRoomsToCache(List<string> chatRooms)
        {
            MemoryCache.CacheDict.SetValue<List<string>>(Constants.CHATROOMS, chatRooms);
        }

        /// <summary>
        /// AddJsonChatRoomToCache adds chatRoomName to cache variable ChatRooms, which contains all chat room names 
        /// </summary>
        /// <param name="chatRoomNr">name of chat room to add to ChatRooms array in cache</param>
        public static void AddJsonChatRoomToCache(string chatRoomNr)
        {
            List<string> chatRooms = GetJsonChatRoomsFromCache();
            if (!chatRooms.Contains(chatRoomNr))
                chatRooms.Add(chatRoomNr);

            SetJsonChatRoomsToCache(chatRooms);
        }

        /// <summary>
        /// Delete Json ChatRoom from cache
        /// - gets current cache key ChatRooms, which contains a array of all chat room names form cache
        /// - removes key with chat room name from cahce (with entire chatroom messages)
        /// - removes chatroom name from cache key Chatrooms
        /// - sets cache key ChatRooms without the deleted chatroom name back to cachg
        /// </summary>
        /// <param name="chatRoomNr">chatroom name</param>
        public static void DeleteJsonChatRoomFromCache(string chatRoomNr)
        {
            List<string> chatRooms = GetJsonChatRoomsFromCache();

            if (MemoryCache.CacheDict.ContainsKey(chatRoomNr))
                MemoryCache.CacheDict.RemoveKey(chatRoomNr);

            if (chatRooms.Contains(chatRoomNr))
                chatRooms.Remove(chatRoomNr);

            SetJsonChatRoomsToCache(chatRooms);
        }

        #endregion static cache operations


    }

}