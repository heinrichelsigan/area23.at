using Area23.At.Framework.Library.Cache;
using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Cqr.Msg;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;

namespace Area23.At.Mono.CqrJD
{


    /// <summary>
    /// CqrBaseService
    /// </summary>
    [WebService(Namespace = "https://area23.at/net/CqrJD/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CqrBaseService : WebService
    {

        protected internal static HashSet<CContact> _contacts;
        protected internal CContact _contact;
        protected internal CqrFacade cqrFacade;
        // protected internal string _literalServerIPv4, _literalServerIPv6;
        protected internal string _serverKey = string.Empty;
        protected internal string _decrypted = string.Empty, _encrypted = string.Empty;
        protected internal string _responseString = string.Empty;
        protected internal string _chatRoomNumber = string.Empty;
        // protected internal ConnectionMultiplexer redis;
        // protected internal ConfigurationOptions options;
        protected internal static bool useAWSCache = false, useAppState = true;
        // protected internal string endpoint = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        // protected internal StackExchange.Redis.IDatabase db;


        /// <summary>
        /// CqrBaseService
        /// </summary>
        public CqrBaseService()
        {
            InitMethod();
        }

        public virtual void InitMethod()
        {
            _contacts = JsonContacts.GetContacts();
            GetServerKey();
            cqrFacade = new CqrFacade(_serverKey);
            _decrypted = string.Empty;
            _responseString = string.Empty;
            _contact = null;

            // if (PersistInCache.CacheType == PersistType.Redis)
            // {
            //     string status = RedisCache.ConnMux.GetStatus();            
            //    memClient = new MemcachedClient(config);
            // }
        }

        [WebMethod]
        public virtual string TestService()
        {

            string ret = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().GetName().FullName) +
                Assembly.GetExecutingAssembly().GetName().Version.ToString();

            object[] sattributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (sattributes.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)sattributes[0];
                if (titleAttribute.Title != "")
                    ret = titleAttribute.Title;
            }
            ret += "\n" + Assembly.GetExecutingAssembly().GetName().Version.ToString();

            object[] oattributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (oattributes.Length > 0)
                ret += "\n" + ((AssemblyDescriptionAttribute)oattributes[0]).Description;

            object[] pattributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (pattributes.Length > 0)
                ret += "\n" + ((AssemblyProductAttribute)pattributes[0]).Product;

            object[] crattributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (crattributes.Length > 0)
                ret += "\n" + ((AssemblyCopyrightAttribute)crattributes[0]).Copyright;

            object[] cpattributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (cpattributes.Length > 0)
                ret += "\n" + ((AssemblyCompanyAttribute)cpattributes[0]).Company;


            return ret;

        }

        [WebMethod]
        public virtual string GetIPAddress()
        {
            string userHostAddr = HttpContext.Current.Request.UserHostAddress;
            return userHostAddr;
        }

        [WebMethod]
        public virtual string TestCache()
        {
            string testReport = $"{DateTime.Now.Area23DateTimeMilliseconds()}:TestCache() started.\n";
            try
            {
                InitMethod();
            }
            catch (Exception ex1)
            {
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Exception {ex1.GetType()}: {ex1.Message}\n\t{ex1}\n";
            }

            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: InitMethod() completed.\n";

            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Persistence in {PersistInCache.CacheType.ToString()}\n";


            Dictionary<Guid, CContact> dictCacheTest = new Dictionary<Guid, CContact>();
            foreach (CContact c in _contacts)
            {
                if (c != null && c.Cuid != null && c.Cuid != Guid.Empty &&
                    !dictCacheTest.Keys.Contains(c.Cuid))
                    dictCacheTest.Add(c.Cuid, c);
            }
            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Added {dictCacheTest.Count} count contacts to Dictionary<Guid, CqrContact>...\n";

            try
            {
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: PersistInCache.CacheType = PersistType.{PersistInCache.CacheType} \n";

                //if (PersistInCache.CacheType == PersistType.Redis)
                //{
                //    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Ready to connect to {ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY]}\n";
                //    string status = RedisCache.ConnMux.GetStatus();
                //    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: ConnectionMulitplexer.Status = {status}" + Environment.NewLine;
                //}

                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Getting MemoryCache.CacheDict.AllKeys" + Environment.NewLine;

                string[] allKeys = MemoryCache.CacheDict.AllKeys;
                if (allKeys == null)
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got null (NULL) keys" + Environment.NewLine;
                else
                {
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got null {allKeys.Length} keys" + Environment.NewLine;
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: AllKeys = [ {string.Join(" ,", allKeys)} ]" + Environment.NewLine;
                }
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Preparing to set Dictionary<Guid, CqrContact> in cache." + "\r\n";
                MemoryCache.CacheDict.SetValue<Dictionary<Guid, CContact>>("TestCache", dictCacheTest);
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Added serialized json string to cache." + Environment.NewLine;

                Dictionary<Guid, CContact> outdict = (Dictionary<Guid, CContact>)MemoryCache.CacheDict.GetValue<Dictionary<Guid, CContact>>("TestCache");
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got Dictionary<Guid, CqrContact> from cache with {outdict.Keys.Count} keys." + "\r\n";
                foreach (CContact contact in outdict.Values)
                {
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Contact Cuid={contact.Cuid} NameEmail={contact.NameEmail} Mobile={contact.Mobile}." + "\r\n";
                }
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Preparing to delete key \"TestCache\":" + "\r\n";
                MemoryCache.CacheDict.RemoveKey("TestCache");
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Deleted key \"TestCache\"." + "\r\n";
            }
            catch (Exception ex2)
            {
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Exception {ex2.GetType()}: {ex2.Message}\n\t{ex2}\n";
            }
            try
            {
                List<string> chatRooms = JsonChatRoom.GetJsonChatRoomsFromCache();
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}:Found {chatRooms.Count} chat room keys in cache." + Environment.NewLine;
                foreach (string room in chatRooms)
                {
                    try
                    {
                        Dictionary<long, string> dicTest = GetCachedMessageDict(room);
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: chat room {room} with keys {dicTest.Keys.Count} messages." + Environment.NewLine;
                    }
                    catch (Exception exChatRoom)
                    {
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: loading chat room {room} failed. Exception: {exChatRoom.Message}." + Environment.NewLine;
                        Area23Log.LogStatic($"Loading chat room {room} failed. ", exChatRoom, "");
                    }
                }
            }
            catch (Exception ex2)
            {
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Exception {ex2.GetType()}: {ex2.Message}\n\t{ex2}\n";
            }

            return testReport;
        }


        [WebMethod]
        public virtual string ResetCache()
        {
            string testReport = $"{DateTime.Now.Area23DateTimeMilliseconds()}:ResetCache() started.\n";
            try
            {
                InitMethod();
            }
            catch (Exception ex1)
            {
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Exception {ex1.GetType()}: {ex1.Message}\n\t{ex1}\n";
            }

            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: InitMethod() completed.\n";

            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Persistence in {PersistInCache.CacheType.ToString()}\n";

            if (PersistInCache.CacheType == PersistType.Redis)
            {
                try
                {
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: PersistInCache.CacheType = PersistType.{PersistInCache.CacheType} \n";                    
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Getting MemoryCache.CacheDict.AllKeys" + Environment.NewLine;

                    string[] allKeys = MemoryCache.CacheDict.AllKeys;
                    HashSet<string> newKeys = new HashSet<string>();
                    if (allKeys == null)
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got null (NULL) keys" + Environment.NewLine;
                    else
                    {
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got null {allKeys.Length} keys" + Environment.NewLine;
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: AllKeys = [ {string.Join(" ,", allKeys)} ]" + Environment.NewLine;
                    }
                    foreach (string aKey in allKeys)
                    {
                        if (aKey.Equals("AllKeys", StringComparison.CurrentCultureIgnoreCase) || aKey.Equals("ChatRooms", StringComparison.CurrentCultureIgnoreCase) ||
                            (aKey.StartsWith("room", StringComparison.CurrentCultureIgnoreCase) && aKey.EndsWith(".json", StringComparison.CurrentCultureIgnoreCase)))
                        {
                            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Keeping key \"" + aKey + "\":" + "\r\n";
                        }
                        else
                        {
                            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Preparing to delete key \"" + aKey + "\":" + "\r\n";
                            MemoryCache.CacheDict.RemoveKey(aKey);
                            newKeys.Add(aKey);
                        }
                    }
                    allKeys = MemoryCache.CacheDict.AllKeys;
                    newKeys = new HashSet<string>();
                    if (allKeys == null)
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got null (NULL) keys" + Environment.NewLine;
                    else
                    {
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got null {allKeys.Length} keys" + Environment.NewLine;
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: AllKeys = [ {string.Join(" ,", allKeys)} ]" + Environment.NewLine;
                    }
                }
                catch (Exception ex2)
                {
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Exception {ex2.GetType()}: {ex2.Message}\n\t{ex2}\n";
                }
            }
            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}:ResetCache() finished.\n";

            return testReport;
        }

        protected string GetServerKey()
        {
            // _serverKey = Constants.AUTHOR_EMAIL;            

            if (ConfigurationManager.AppSettings["ExternalClientIP"] != null)
            {
                _serverKey = (string)ConfigurationManager.AppSettings["ExternalClientIP"];
            }
            else
                _serverKey = HttpContext.Current.Request.UserHostAddress;
            _serverKey += Constants.APP_NAME;

            return _serverKey;
        }

        /// <summary>
        /// Generates a chat room with a new ChatRoomNr, containing sender and recpients
        /// </summary>
        /// <param name="cSrvMsg"><see cref="CSrvMsg{string}"/></param>
        /// <returns><see cref="CSrvMsg{string}"/></returns>
        internal CSrvMsg<string> InviteToChatRoom(CSrvMsg<string> cSrvMsg)
        {
            string chatRoomNr = string.Empty;
            DateTime now = DateTime.Now; // now1 = now.AddMilliseconds(10);
            List<CContact> _invited = new List<CContact>();

            string restMail = cSrvMsg.Sender.Email.Contains("@") ? (cSrvMsg.Sender.Email.Substring(0, cSrvMsg.Sender.Email.IndexOf("@"))) : cSrvMsg.Sender.Email.Trim();
            restMail = restMail.Replace("@", "_").Replace(".", "_");

            if (!string.IsNullOrEmpty(restMail))
                chatRoomNr = String.Format("room_{0:MMdd}_{1}.json", DateTime.Now, restMail);
            else
                chatRoomNr = $"room_{DateTime.Now:MMddHHmm}.json";

            Dictionary<long, string> dict = new Dictionary<long, string>();
            dict.Add(now.Ticks, "");

            if (cSrvMsg.CRoom == null)
                cSrvMsg.CRoom = new CChatRoom(chatRoomNr, Guid.NewGuid(), now, now) { TicksLong = dict.Keys.ToList() };
            else
            {
                cSrvMsg.CRoom.ChatRoomNr = chatRoomNr;
                cSrvMsg.CRoom.ChatRuid = (cSrvMsg.CRoom.ChatRuid == Guid.Empty) ? Guid.NewGuid() : cSrvMsg.CRoom.ChatRuid;
                cSrvMsg.CRoom.LastPolled = now;
                cSrvMsg.CRoom.LastPushed = now;
                if (cSrvMsg.CRoom.TicksLong == null || cSrvMsg.CRoom.TicksLong.Count == 0)
                    cSrvMsg.CRoom.TicksLong = dict.Keys.ToList();
                else
                    cSrvMsg.CRoom.TicksLong.Add(now.Ticks);
            }

            cSrvMsg._message = (cSrvMsg.CRoom != null && !string.IsNullOrEmpty(cSrvMsg.CRoom.ChatRoomNr)) ? cSrvMsg.CRoom.ChatRoomNr : "";


            bool addSender = true;
            foreach (CContact cr in cSrvMsg.Recipients)
            {
                cr._message = (cSrvMsg.CRoom != null && !string.IsNullOrEmpty(cSrvMsg.CRoom.ChatRoomNr)) ? cSrvMsg.CRoom.ChatRoomNr : "";

                _invited.Add(cr);
                if ((!string.IsNullOrEmpty(cr.NameEmail) && cr.NameEmail == cSrvMsg.Sender.NameEmail) ||
                    (cr.Cuid != null && cr.Cuid != Guid.Empty && cr.Cuid == cSrvMsg.Sender.Cuid))
                    addSender = false;
            }
            if (addSender)
                _invited.Add(cSrvMsg.Sender);

            SetCachedMessageDict(chatRoomNr, dict);


            CSrvMsg<string> cChatRSrvMsg = JsonChatRoom.SaveChatRoom(cSrvMsg, cSrvMsg.CRoom);
            _chatRoomNumber = cChatRSrvMsg.CRoom.ChatRoomNr;
            cChatRSrvMsg._message = _chatRoomNumber;
            JsonChatRoom.AddJsonChatRoomToCache(_chatRoomNumber);

            // serialize chat room in msg later then saving
            cChatRSrvMsg.SerializedMsg = cChatRSrvMsg.ToJson();

            return cChatRSrvMsg;
        }

        /// <summary>
        /// Add LastPolled to contact and also to <see cref="CContact.PolledMsgDates"/>
        /// reduces <see cref="CContact.PolledMsgDates"/>, if contact wears a to huge amount of polling history
        /// TODO: implement ring buffer here
        /// </summary>
        /// <param name="contact"><see cref="CContact"/> to modify</param>
        /// <param name="date"></param>
        /// <returns>modified <see cref="CContact"/></returns>
        [Obsolete("CContact has no more chat room", true)]
        public CContact AddPollDate(CContact contact, DateTime date, bool pushed = false)
        {
            if (pushed)
                contact.Md5Hash = date.ToLongDateString();

            return contact;
        }

        /// <summary>
        /// AddLastDate adds lastPolled or lastPushed date and tickIndex to TicksLong
        /// </summary>
        /// <param name="chatRoomMsg"><see cref="CSrvMsg{string}"/> chat room msg to be returned to chat client app</param>
        /// <param name="tickIndex">tick long index</param>
        /// <param name="pushed">false for poolled, true for pushed</param>
        /// <returns><see cref="CSrvMsg{string}"/></returns>
        public CSrvMsg<string> AddLastDate(CSrvMsg<string> chatRoomMsg, long tickIndex, bool pushed = false)
        {
            DateTime date = new DateTime(tickIndex);
            if (pushed)
            {
                chatRoomMsg.CRoom.LastPushed = date;
            }
            else
            {
                chatRoomMsg.CRoom.LastPolled = date;
                if (!chatRoomMsg.CRoom.TicksLong.Contains(tickIndex))
                    chatRoomMsg.CRoom.TicksLong.Add(tickIndex);
            }

            return chatRoomMsg;
        }

        /// <summary>
        /// GetCachedMessageDict returns one chat room message dictionary
        /// either from Application State in proc or from Valkey Elastic Cache on AWS
        /// </summary>
        /// <param name="chatRoomNumber">json chat room number</param>
        /// <returns>one chat room message dictionary</returns>
        public static Dictionary<long, string> GetCachedMessageDict(string chatRoomNumber)
        {
            Dictionary<long, string> dict = new Dictionary<long, string>();

            dict = (Dictionary<long, string>)MemoryCache.CacheDict.GetValue<Dictionary<long, string>>(chatRoomNumber);

            // TODO: implement filesystem 

            return dict;

        }

        /// <summary>
        /// GetNewMessageIndices get all chat room indices, 
        /// which are newer than last <see cref="CContact.LastPolled">polling date of user</see>
        /// or user hasn't read and that are not in list <see cref="CContact.TicksLong"></see>
        /// </summary>
        /// <param name="dictKeys"><see cref="DateTime.Ticks"/> as index key of chat room message dictionary</param>
        /// <param name="cSrvMsg"><see cref="CSrvMsg{string}"/></param>
        /// <returns><see cref="List{long}">key indices of messages, that are new and not already polled</see></returns>
        public static List<long> GetNewMessageIndices(List<long> dictKeys, CSrvMsg<string> cSrvMsg)
        {

            List<long> pollKeys = new List<long>();
            foreach (long tickIndex in dictKeys)
            {
                // if (tickIndex > sender.LastPolled.Ticks)
                if (!cSrvMsg.CRoom.TicksLong.Contains(tickIndex))
                    pollKeys.Add(tickIndex);
            }

            return pollKeys;
        }

        /// <summary>
        /// SetCachedMessageDict saves the mesage dictionary for chat room in 
        /// either application state in proc or Amazon Valkey Elastic cache
        /// </summary>
        /// <param name="chatRoomNumber">json chat room number</param>
        /// <param name="dict">the mesage dictionary for chat room </param>
        public static void SetCachedMessageDict(string chatRoomNumber, Dictionary<long, string> dict)
        {
            MemoryCache.CacheDict.SetValue<Dictionary<long, string>>(chatRoomNumber, dict);

            return;
        }

    }

}
