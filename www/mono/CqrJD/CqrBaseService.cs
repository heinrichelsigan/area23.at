using Area23.At.Framework.Library.CqrXs.Msg;
using Area23.At.Framework.Library.Static;
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
    
    [WebService(Namespace = "https://cqrjd.eu/cqrsrv/cqrjd/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BaseWebService : WebService
    {
        protected internal static HashSet<CqrContact> _contacts;
        protected internal CqrContact _contact;
        protected internal string _serverKey = string.Empty;
        protected internal string _decrypted = string.Empty, _encrypted = string.Empty;
        protected internal string _responseString = string.Empty;
        protected internal string _chatRoomNumber = string.Empty;
        // protected internal ConnectionMultiplexer redis;
        // protected internal ConfigurationOptions options;
        protected internal static bool useAWSCache = false, useAppState = true;

        public static bool UseApplicationState
        {
            get
            {
                if (ConfigurationManager.AppSettings["UseHttpApplicationState"] != null)
                {
                    if (Boolean.TryParse((string)ConfigurationManager.AppSettings["UseHttpApplicationState"].ToString(),
                        out useAppState))
                    {
                        return useAppState;
                    }
                    useAppState = true;
                }
                return useAppState;
            }
        }

        public static bool UseAmazonElasticCache
        {
            get
            {
                if (ConfigurationManager.AppSettings["UseAmazonElasticCache"] != null)
                {
                    if (Boolean.TryParse((string)ConfigurationManager.AppSettings["UseAmazonElasticCache"].ToString(),
                        out useAWSCache))
                    {
                        return useAWSCache;
                    }
                    useAWSCache = false;
                }
                return useAWSCache;
            }
        }



        public BaseWebService()
        {
            InitMethod();
        }


        public virtual void InitMethod()
        {
            _contacts = GetContacts();
            GetServerKey();
            _decrypted = string.Empty;
            _responseString = string.Empty;
            _contact = null;


            if (UseAmazonElasticCache)
            {
                // string status = RedIs.ConnMux.GetStatus();

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
            string testReport = $"{DateTime.Now.Area23DateTimeMilliseconds()}: TestCache() started.\n";
            try
            {
                InitMethod();
            }
            catch (Exception ex1)
            {
                testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Exception {ex1.GetType()}: {ex1.Message}\n\t{ex1}\n";
            }

            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: InitMethod() completed.\n";

            Dictionary<Guid, CqrContact> dictCacheTest = new Dictionary<Guid, CqrContact>();
            foreach (CqrContact c in _contacts)
            {
                if (c != null && c.Cuid != null && c.Cuid != Guid.Empty &&
                    !dictCacheTest.Keys.Contains(c.Cuid))
                    dictCacheTest.Add(c.Cuid, c);
            }
            testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Added {dictCacheTest.Count} count contacts to Dictionary<Guid, CqrContact>...\n";
            if (UseAmazonElasticCache)
            {
                try
                {
                    //if (UseAmazonElasticCache)
                    //{
                    //    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Ready to connect to {ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT]}\n";
                    //    string status = RedIs.ConnMux.GetStatus();
                    //    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: ConnectionMulitplexer.Status = {status}\n";

                    //    string dictJson = JsonConvert.SerializeObject(dictCacheTest);
                    //    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Serialized Dictionary<Guid, CqrContact> to json string.\n";
                    //    RedIs.Db.StringSet("TestCache", dictJson);
                    //    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Added serialized json string to cache.\n";

                    //    string jsonOut = RedIs.Db.StringGet("TestCache");
                    //    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Got json serialized string from cache: {jsonOut}.\n";
                    //    Dictionary<Guid, CqrContact> outdict = (Dictionary<Guid, CqrContact>)JsonConvert.DeserializeObject<Dictionary<Guid, CqrContact>>(jsonOut);
                    //    testReport += $"{DateTime.Now.Area23DateTimeMilliseconds()}: Deserialized json sring to (Dictionary<Guid, CqrContact> with {outdict.Keys.Count} keys.";
                    //}
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
                _serverKey = (string)ConfigurationManager.AppSettings["ExternalClientIP"];
            else
                _serverKey = HttpContext.Current.Request.UserHostAddress;
            _serverKey += Constants.APP_NAME;

            return _serverKey;
        }

        internal HashSet<CqrContact> GetContacts()
        {
            _contacts = JsonContacts.GetContacts();
            return _contacts;
        }

        internal CqrContact AddContact(CqrContact cqrContact)
        {
            _contacts = JsonContacts.GetContacts();
            CqrContact foundCt = JsonContacts.FindContactByNameEmail(_contacts, cqrContact);
            if (foundCt != null)
            {
                foundCt.ContactId = cqrContact.ContactId;
                if (foundCt.Cuid == null || foundCt.Cuid == Guid.Empty)
                    foundCt.Cuid = Guid.NewGuid();
                if (!string.IsNullOrEmpty(cqrContact.Address))
                    foundCt.Address = cqrContact.Address;
                if (cqrContact.Mobile != null && cqrContact.Mobile.Length > 1)
                    foundCt.Mobile = cqrContact.Mobile;
                if (!string.IsNullOrEmpty(cqrContact.ChatRoomId))
                    foundCt.ChatRoomId = cqrContact.ChatRoomId;
                if (cqrContact.LastPolled != null && cqrContact.LastPolled > DateTime.MinValue)
                    foundCt.LastPolled = cqrContact.LastPolled;

                //if (cqrContact != null && cqrContact.ContactImage != null &&
                //    !string.IsNullOrEmpty(cqrContact.ContactImage.ImageFileName) &&
                //    cqrContact.ContactImage.ImageBase64 != null &&
                //    !string.IsNullOrEmpty(cqrContact.ContactImage.ImageBase64))
                foundCt.ContactImage = null;
            }
            else
            {
                if (cqrContact.Cuid == null || cqrContact.Cuid == Guid.Empty)
                    cqrContact.Cuid = Guid.NewGuid();
                _contacts.Add(cqrContact);
                foundCt = cqrContact;
                foundCt.ContactImage = null;

            }

            UpdateContact(foundCt);

            return foundCt;
        }


        internal void UpdateContact(CqrContact cqrContact)
        {
            CqrContact toAddContact = null;
            if (cqrContact == null || string.IsNullOrEmpty(cqrContact.Email))
                return;
            HashSet<CqrContact> contacts = new HashSet<CqrContact>();

            foreach (CqrContact ct in _contacts)
            {
                if ((ct.Cuid == cqrContact.Cuid && ct.Email == cqrContact.Email) ||
                    (ct.NameEmail == cqrContact.NameEmail))
                {
                    toAddContact = new CqrContact(cqrContact, cqrContact.ChatRoomId, cqrContact.LastPolled, cqrContact.Hash);
                    toAddContact.Mobile = cqrContact.Mobile;
                    toAddContact.ContactImage = null;
                    toAddContact.Cuid = (cqrContact.Cuid != null && cqrContact.Cuid != Guid.Empty) ? cqrContact.Cuid : Guid.NewGuid();
                    contacts.Add(toAddContact);
                }
                else
                    contacts.Add(ct);
            }
            _contacts = contacts;
            JsonContacts.SaveJsonContacts(_contacts);
        }

        internal void UpdateContacts(CqrContact contact, FullSrvMsg<string> chatRoomMsg, string chatRoomNr)
        {
            bool foundCt = false;
            CqrContact toDelContact = null;
            if (contact == null || string.IsNullOrEmpty(contact.Email))
                return;

            if ((chatRoomMsg.Sender.Cuid == contact.Cuid && chatRoomMsg.Sender.Email == contact.Email) ||
                (chatRoomMsg.Sender.NameEmail == contact.NameEmail))
            {
                chatRoomMsg.Sender = new CqrContact(contact, chatRoomNr, contact.LastPolled, contact.Hash);
            }
            for (int i = 0; i < chatRoomMsg.Recipients.Count; i++)
            {
                if ((chatRoomMsg.Recipients.ElementAt(i).Cuid == contact.Cuid) &&
                    (chatRoomMsg.Recipients.ElementAt(i).Name.Equals(contact.Name) ||
                    chatRoomMsg.Recipients.ElementAt(i).NameEmail.Equals(contact.NameEmail)))
                {
                    toDelContact = chatRoomMsg.Recipients.ElementAt(i);
                    foundCt = true;
                    break;
                }
            }
            if (foundCt)
                if (chatRoomMsg.Recipients.Remove(toDelContact))
                    chatRoomMsg.Recipients.Add(new CqrContact(contact, chatRoomNr, contact.LastPolled, contact._hash));

            (new JsonChatRoom(_chatRoomNumber)).SaveJsonChatRoom(chatRoomMsg, chatRoomNr);

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
                if (_contacts.Remove(toDelContact))
                    _contacts.Add(new CqrContact(contact, chatRoomNr, contact.LastPolled, contact.Hash));

            JsonContacts.SaveJsonContacts(_contacts);

        }


        /// <summary>
        /// Generates a chat room with a new chatroomid, containing sender and recpients
        /// </summary>
        /// <param name="fullSrvMsg"><see cref="FullSrvMsg{string}"/></param>
        /// <returns><see cref="FullSrvMsg{string}"/></returns>
        internal FullSrvMsg<string> InviteToChatRoom(FullSrvMsg<string> fullSrvMsg)
        {
            string chatRoomId = string.Empty;
            List<CqrContact> _invited = new List<CqrContact>();
                       
            if (string.IsNullOrEmpty(chatRoomId))
                chatRoomId = String.Format("room_{0:yyMMddHH}_{1}.json", DateTime.Now,
                    fullSrvMsg.Sender.Email.Replace("@", "_").Replace(".", "_"));

            if (string.IsNullOrEmpty(chatRoomId))
                chatRoomId = $"room_{DateTime.Now:yyMMddHHmmss}_0.json";

            fullSrvMsg.ChatRoomNr = chatRoomId;
            fullSrvMsg.Sender.ChatRoomId = chatRoomId;

            DateTime now = DateTime.Now;
            Dictionary<long, string> dict = new Dictionary<long, string>();
            dict.Add(now.Ticks, "");

            if (UseApplicationState)
                HttpContext.Current.Application[chatRoomId] = dict;
            //if (UseAmazonElasticCache)
            //{
            //    string dictJson = JsonConvert.SerializeObject(dict);
            //    RedIs.Db.StringSet(chatRoomId, dictJson);
            //}
                      
            fullSrvMsg.Sender.LastPolled = now;
            fullSrvMsg.Sender.LastPushed = now;            

            bool addSender = true;
            foreach (CqrContact c in fullSrvMsg.Recipients)
            {
                c.ChatRoomId = chatRoomId;
               
                _invited.Add(c);
                if ((!string.IsNullOrEmpty(c.NameEmail) && c.NameEmail == fullSrvMsg.Sender.NameEmail) ||
                    (c.Cuid != null && c.Cuid != Guid.Empty && c.Cuid == fullSrvMsg.Sender.Cuid))
                    addSender = false;
            }
            if (addSender)
                _invited.Add(fullSrvMsg.Sender);

 
            FullSrvMsg<string> chatRSrvMsg = new FullSrvMsg<string>();
            chatRSrvMsg.Sender = fullSrvMsg.Sender;
            chatRSrvMsg.Sender.ChatRoomId = chatRoomId;
            chatRSrvMsg.Sender.LastPolled = DateTime.Now;
            chatRSrvMsg.Sender.LastPushed = DateTime.Now;
            chatRSrvMsg.Recipients = new HashSet<CqrContact>(_invited);
            chatRSrvMsg._hash = fullSrvMsg._hash;
            chatRSrvMsg.ChatRoomNr = chatRoomId;
            chatRSrvMsg.MsgType = MsgEnum.Json;
            chatRSrvMsg = (new JsonChatRoom(chatRoomId)).SaveJsonChatRoom(chatRSrvMsg, chatRoomId);
            
            // serialize chat room in msg later then saving
            chatRSrvMsg.RawMessage = chatRSrvMsg.ToJson();
            chatRSrvMsg._message = chatRSrvMsg.ToJson();

            return chatRSrvMsg;
        }


        public bool ValidateChatRoomNr(FullSrvMsg<string> fullSrvMsg, FullSrvMsg<string> chatRoomMsg, string chatRoomId)
        {

            bool isValid = false;
            if (chatRoomId.Equals(chatRoomMsg.ChatRoomNr))
            {
                if ((fullSrvMsg.Sender.NameEmail == chatRoomMsg.Sender.NameEmail) ||
                    (!string.IsNullOrEmpty(fullSrvMsg.Sender.Email) && fullSrvMsg.Sender.Email == chatRoomMsg.Sender.Email))
                {
                    _contact = chatRoomMsg.Sender;
                    isValid = true;
                    return isValid;
                }
                foreach (CqrContact c in chatRoomMsg.Recipients)
                {
                    if (fullSrvMsg.Sender.NameEmail == c.NameEmail ||
                        fullSrvMsg.Sender.Email == c.Email)
                    {
                        _contact = c;
                        isValid = true;
                        return isValid;
                    }
                }

            }

            return isValid;
        }


        /// <summary>
        /// Add LastPolled to contact and also to <see cref="CqrContact.PolledMsgDates"/>
        /// reduces <see cref="CqrContact.PolledMsgDates"/>, if contact wears a to huge amount of polling history
        /// TODO: implement ring buffer here
        /// </summary>
        /// <param name="contact"><see cref="CqrContact"/> to modify</param>
        /// <param name="date"></param>
        /// <returns>modified <see cref="CqrContact"/></returns>
        public CqrContact AddPollDate(CqrContact contact, DateTime date, bool pushed = false)
        {
            if (pushed)
                contact.LastPushed = date;
            else
                contact.LastPolled = date.AddMilliseconds(100);
            
            return contact;       
        }
            

    }
}
