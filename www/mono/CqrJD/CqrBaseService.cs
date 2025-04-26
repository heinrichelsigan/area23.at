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
    /// BaseWebService
    /// </summary>
    [WebService(Namespace = "https://srv.cqrxs.eu/v1.3/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BaseWebService : WebService
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
        /// Persist encrypted messages in chat rooms in application state
        /// use this option only for testing, because you will you get soon an out of memory error
        /// </summary>
        public static bool PersistMsgInApplicationState
        {
            get => (PersistMsgIn.PersistMsg == PersistType.ApplicationState);
        }

        /// <summary>
        /// Use Amazon elastic cache to persist encrypted messages in chat rooms
        /// Fast option, but expensive, when we have a lot of huge size messages
        /// </summary>
        public static bool PersistMsgInAmazonElasticCache
        {
            get => (PersistMsgIn.PersistMsg == PersistType.AmazonElasticCache);
        }

        /// <summary>
        /// Use file system to encrypted messages in chat rooms
        /// Fast option, but expensive, when we have a lot of huge size messages
        /// </summary>
        public static bool PersistMsgInFileSystem
        {
            get => (PersistMsgIn.PersistMsg == PersistType.FileSystem);
        }

        /// <summary>
        /// BaseWebService
        /// </summary>
        public BaseWebService()
        {
            InitMethod();
        }

        public virtual void InitMethod()
        {
            _contacts = GetContacts();
            GetServerKey();
            cqrFacade = new CqrFacade(_serverKey);
            _decrypted = string.Empty;
            _responseString = string.Empty;
            _contact = null;


            if (PersistMsgInAmazonElasticCache)
            {
                string status = RedIS.GetStatus();

                //config = new ElastiCacheClusterConfig("cachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com", 11211);
                //// ClusterConfigSettings clusterConfig = new ClusterConfigSettings("cachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com", 11211);
                //memClient = new MemcachedClient(config);
            }
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

            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Persistence in {PersistMsgIn.PersistMsg.ToString()}\n";


            Dictionary<Guid, CContact> dictCacheTest = new Dictionary<Guid, CContact>();
            foreach (CContact c in _contacts)
            {
                if (c != null && c.Cuid != null && c.Cuid != Guid.Empty &&
                    !dictCacheTest.Keys.Contains(c.Cuid))
                    dictCacheTest.Add(c.Cuid, c);
            }
            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Added {dictCacheTest.Count} count contacts to Dictionary<Guid, CqrContact>...\n";
            if (PersistMsgInAmazonElasticCache)
            {
                try
                {
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Ready to connect to {ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY]}\n";
                    string status = RedIS.GetStatus();
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: ConnectionMulitplexer.Status = {status}" + Environment.NewLine;

                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Executing RedIS.GetAllKeys()" + Environment.NewLine;

                    HashSet<string> allKeys = RedIS.GetAllKeys();
                    if (allKeys == null)
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got null (NULL) keys" + Environment.NewLine;
                    else
                    {
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got null {allKeys.Count} keys" + Environment.NewLine;
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: AllKeys = [ {string.Join(" ,", allKeys.ToArray())} ]" + Environment.NewLine;
                    }
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Preparing to set Dictionary<Guid, CqrContact> in cache." + "\r\n";
                    RedIS.ValKey.SetKey<Dictionary<Guid, CContact>>("TestCache", dictCacheTest);
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Added serialized json string to cache." + Environment.NewLine;

                    Dictionary<Guid, CContact> outdict = (Dictionary<Guid, CContact>)RedIS.ValKey.GetKey<Dictionary<Guid, CContact>>("TestCache");
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got Dictionary<Guid, CqrContact> from cache with {outdict.Keys.Count} keys." + "\r\n";
                    foreach (CContact contact in outdict.Values)
                    {
                        testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Contact Cuid={contact.Cuid} NameEmail={contact.NameEmail} Mobile={contact.Mobile}." + "\r\n";
                    }
                    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Preparing to delete key \"TestCache\":" + "\r\n";
                    RedIS.ValKey.DeleteKey("TestCache");
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
            }

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

        internal HashSet<CContact> GetContacts()
        {
            _contacts = JsonContacts.GetContacts();
            return _contacts;
        }

        internal CContact AddContact(CContact ccontact)
        {
            _contacts = JsonContacts.GetContacts();
            CContact foundCt = JsonContacts.FindContactByNameEmail(_contacts, ccontact);
            if (foundCt != null)
            {
                foundCt.ContactId = ccontact.ContactId;
                if (foundCt.Cuid == null || foundCt.Cuid == Guid.Empty)
                    foundCt.Cuid = Guid.NewGuid();
                if (!string.IsNullOrEmpty(ccontact.Address))
                    foundCt.Address = ccontact.Address;
                if (ccontact.Mobile != null && ccontact.Mobile.Length > 1)
                    foundCt.Mobile = ccontact.Mobile;
                if (ccontact._message != null && !string.IsNullOrEmpty(ccontact._message))
                    foundCt._message = ccontact._message;

                foundCt.ContactImage = null;
            }
            else
            {
                if (ccontact.Cuid == null || ccontact.Cuid == Guid.Empty)
                    ccontact.Cuid = Guid.NewGuid();
                foundCt = new CContact(ccontact, ccontact._message, ccontact._hash);
                foundCt.ContactImage = null;
                foundCt._message = ccontact._message;

                _contacts.Add(foundCt);

            }

            UpdateContact(foundCt);

            return foundCt;
        }

        internal void UpdateContact(CContact ccontact)
        {
            CContact toAddContact = null;
            if (ccontact == null || string.IsNullOrEmpty(ccontact.Email))
                return;

            string chatRoomNr = (ccontact._message != null && !string.IsNullOrEmpty(ccontact._message)) ? ccontact._message : "";
            HashSet<CContact> contacts = new HashSet<CContact>();

            foreach (CContact ct in _contacts)
            {
                if ((ct.Cuid == ccontact.Cuid && ct.Email.Equals(ccontact.Email, StringComparison.CurrentCultureIgnoreCase) ||
                    ct.NameEmail.Equals(ccontact.NameEmail, StringComparison.CurrentCultureIgnoreCase)))
                {
                    toAddContact = new CContact(ccontact, chatRoomNr, ccontact.Hash);
                    toAddContact.Mobile = ccontact.Mobile;
                    toAddContact.ContactImage = null;
                    toAddContact.Cuid = (ccontact.Cuid != null && ccontact.Cuid != Guid.Empty) ? ccontact.Cuid : Guid.NewGuid();
                    toAddContact._message = chatRoomNr;
                    contacts.Add(toAddContact);
                }
                else
                    contacts.Add(ct);
            }
            _contacts = contacts;
            JsonContacts.SaveJsonContacts(_contacts);
        }

        internal void UpdateContacts(CContact contact, CSrvMsg<string> chatRoomMsg, string chatRoomNr)
        {
            bool foundCt = false;
            CContact toDelContact = null;
            if (contact == null || string.IsNullOrEmpty(contact.Email))
                return;

            if ((chatRoomMsg.Sender.Cuid == contact.Cuid && chatRoomMsg.Sender.Email.Equals(contact.Email, StringComparison.CurrentCultureIgnoreCase)) ||
                (chatRoomMsg.Sender.NameEmail.Equals(contact.NameEmail, StringComparison.CurrentCultureIgnoreCase)))
            {
                chatRoomMsg.Sender = new CContact(contact, chatRoomNr, contact.Hash);
            }
            for (int i = 0; i < chatRoomMsg.Recipients.Count; i++)
            {
                if ((chatRoomMsg.Recipients.ElementAt(i).Cuid == contact.Cuid &&
                        chatRoomMsg.Recipients.ElementAt(i).Name.Equals(contact.Name, StringComparison.CurrentCultureIgnoreCase)) ||
                    (chatRoomMsg.Recipients.ElementAt(i).Cuid == contact.Cuid &&
                        chatRoomMsg.Recipients.ElementAt(i).Email.Equals(contact.Email, StringComparison.CurrentCultureIgnoreCase)) ||
                    chatRoomMsg.Recipients.ElementAt(i).NameEmail.Equals(contact.NameEmail, StringComparison.CurrentCultureIgnoreCase))
                {
                    toDelContact = chatRoomMsg.Recipients.ElementAt(i);
                    foundCt = true;
                    break;
                }
            }
            if (foundCt)
            {
                if (chatRoomMsg.Recipients.Remove(toDelContact))
                {

                    CContact cToAdd = new CContact(contact, chatRoomNr, contact._hash);
                    cToAdd._message = chatRoomNr;
                    chatRoomMsg.Recipients.Add(cToAdd);
                }
            }

            JsonChatRoom.SaveChatRoom(chatRoomMsg, chatRoomMsg.CRoom);
            // (new JsonChatRoom(_chatRoomNumber)).SaveJsonChatRoom(chatRoomMsg, chatRoomNr);

            foundCt = false;
            GetContacts();
            for (int j = 0; j < _contacts.Count; j++)
            {
                if ((_contacts.ElementAt(j).Cuid == contact.Cuid) &&
                    (_contacts.ElementAt(j).Name.Equals(contact.Name) ||
                        _contacts.ElementAt(j).NameEmail.Equals(contact.NameEmail)))
                {
                    toDelContact = _contacts.ElementAt(j);
                    foundCt = true;
                    break;
                }
            }
            if (foundCt)
            {
                if (_contacts.Remove(toDelContact))
                {
                    CContact c2Add = new CContact(contact, chatRoomNr, contact.Hash);
                    c2Add._message = chatRoomNr;
                    _contacts.Add(c2Add);
                }
            }

            JsonContacts.SaveJsonContacts(_contacts);

        }

        /// <summary>
        /// Generates a chat room with a new ChatRoomNr, containing sender and recpients
        /// </summary>
        /// <param name="cSrvMsg"><see cref="CSrvMsg{string}"/></param>
        /// <returns><see cref="FullSrvMsg{string}"/></returns>
        internal CSrvMsg<string> InviteToChatRoom(CSrvMsg<string> cSrvMsg)
        {
            string chatRoomNr = string.Empty;
            DateTime now = DateTime.Now; // now1 = now.AddMilliseconds(10);
            List<CContact> _invited = new List<CContact>();

            string restMail = cSrvMsg.Sender.Email.Contains("@") ? (cSrvMsg.Sender.Email.Substring(0, cSrvMsg.Sender.Email.IndexOf("@"))) : cSrvMsg.Sender.Email.Trim();
            restMail = restMail.Replace("@", "_").Replace(".", "_");
            if (string.IsNullOrEmpty(chatRoomNr))
                chatRoomNr = String.Format("room_{0:MMdd}_{1}.json", DateTime.Now, restMail);

            if (string.IsNullOrEmpty(chatRoomNr))
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
                cSrvMsg.CRoom.TicksLong = dict.Keys.ToList();

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
        /// <param name="isClosingRequest">default false, only on close, where only creator can close chat room</param>
        /// <returns>true, if person is allowed to push or receive msg from / to chat room</returns>        
        public bool ChatRoomCheckPermission(CSrvMsg<string> cSrvMsg, CSrvMsg<string> chatRoomMsg, string chatRoomNr, bool isClosingRequest = false)
        {

            bool isValid = false;
            if (chatRoomNr.Equals(chatRoomMsg.CRoom.ChatRoomNr))
            {
                if ((cSrvMsg.Sender.NameEmail == chatRoomMsg.Sender.NameEmail) ||
                    (!string.IsNullOrEmpty(cSrvMsg.Sender.Email) && cSrvMsg.Sender.Email.Equals(chatRoomMsg.Sender.Email, StringComparison.CurrentCultureIgnoreCase)))
                {
                    _contact = chatRoomMsg.Sender;
                    isValid = true;
                    return isValid;
                }
                if (!isClosingRequest)
                {
                    foreach (CContact c in chatRoomMsg.Recipients)
                    {
                        if (cSrvMsg.Sender.NameEmail == c.NameEmail ||
                            cSrvMsg.Sender.NameEmail.Equals(c.NameEmail, StringComparison.CurrentCultureIgnoreCase) ||
                            cSrvMsg.Sender.Email.Equals(c.Email, StringComparison.CurrentCultureIgnoreCase) ||
                            (cSrvMsg.Sender.Name.Equals(c.Name, StringComparison.CurrentCultureIgnoreCase) && cSrvMsg.Sender.Cuid == c.Cuid))
                        {
                            _contact = c;
                            isValid = true;
                            return isValid;
                        }
                    }
                }
            }

            return isValid;
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

            // ApplicationState as Cache
            if (PersistMsgInApplicationState && (HttpContext.Current.Application[chatRoomNumber] != null))
                dict = (Dictionary<long, string>)HttpContext.Current.Application[chatRoomNumber];

            // Amazon Redis Valkey Cache
            if (PersistMsgInAmazonElasticCache)
            {
                dict = (Dictionary<long, string>)RedIS.ValKey.GetKey<Dictionary<long, string>>(chatRoomNumber);
            }

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

            if (BaseWebService.PersistMsgInApplicationState)
                HttpContext.Current.Application[chatRoomNumber] = dict;
            if (BaseWebService.PersistMsgInAmazonElasticCache)
            {
                RedIS.ValKey.SetKey<Dictionary<long, string>>(chatRoomNumber, dict);
            }

            return;
        }

    }

}
