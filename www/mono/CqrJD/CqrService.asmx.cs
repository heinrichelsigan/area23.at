using Area23.At;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.CqrXs;
using Area23.At.Framework.Library.CqrXs.Msg;
using Area23.At.Framework.Library.CqrXs.CqrSrv;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Mono.Util;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;


namespace Area23.At.Mono.CqrJD
{

    /// <summary>
    /// CqrService offers a simple chat room service with strong encryption
    /// </summary>
    [WebService(Namespace = "https://cqrjd.eu/cqrsrv/cqrjd/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CqrService : BaseWebService
    {

        /// <summary>
        /// Send1StSrvMsg sends first registration message of contact
        /// </summary>
        /// <param name="cryptMsg">with sercerkey encrypted message</param>
        /// <returns>with serverkey encrypted responnse of own contact</returns>
        [WebMethod]
        public string Send1StSrvMsg(string cryptMsg)
        {
            Area23Log.LogStatic($"Send1StSrvMsg(string cryptMsg) called.  cryptMsg.Length = {cryptMsg.Length}.\n");
            InitMethod();

            if (UseApplicationState)
                HttpContext.Current.Application["lastmsg"] = cryptMsg;
            //if (UseAmazonElasticCache)
            //    RedIs.Db.StringSet("lastmsg", cryptMsg);

            SrvMsg1 srv1stMsg = new SrvMsg1(_serverKey);
            SrvMsg1 srv1stRespMsg = new SrvMsg1(_serverKey);

            try
            {
                if (!string.IsNullOrEmpty(cryptMsg) && cryptMsg.Length >= 8)
                {
                    _contact = srv1stMsg.NCqrSrvMsg1(cryptMsg);
                    _decrypted = _contact.ToJson();
                    Area23Log.LogStatic("$Contact decrypted successfully: {_decrypted}\n");
                }
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
                Area23Log.LogStatic($"Exception {ex.GetType()} when decrypting contact: {ex.Message}\n\t{ex.ToString()}\n");
            }

            _responseString = srv1stRespMsg.CqrBaseMsg("", EncodingType.Base64);

            if (!string.IsNullOrEmpty(_decrypted) && _contact != null && !string.IsNullOrEmpty(_contact.NameEmail))
            {
                if (UseApplicationState)
                    Application["lastdecrypted"] = _decrypted;
                //if (UseAmazonElasticCache)
                //    RedIs.Db.StringSet("lastdecrypted", _decrypted);

                CqrContact foundCt = AddContact(_contact);
                _responseString = srv1stRespMsg.CqrSrvMsg1(foundCt, EncodingType.Base64);
            }

            Area23Log.LogStatic($"Send1StSrvMsg(string cryptMsg) finished.  _contact.Cuid = {_contact.Cuid}.\n");
            return _responseString;
        }


        /// <summary>
        /// Invites to a chat romm 
        /// with an encrypted <see cref="FullSrvMsg<string>"/>
        /// </summary>
        /// <param name="cryptMsg">encrypted <see cref="FullSrvMsg<string>"/></param>
        /// <returns>encrypted <see cref="FullSrvMsg<string>"/> including chatroom number</returns>
        [WebMethod]

        public string ChatRoomInvite(string cryptMsg)
        {
            Area23Log.LogStatic($"ChatRoomInvite(string cryptMsg) called.  cryptMsg.Length = {cryptMsg.Length}.\n");
            InitMethod();

            _chatRoomNumber = "";
            SrvMsg srvMsg = new SrvMsg(_serverKey, _serverKey);
            FullSrvMsg<string> fullSrvMsg, chatRSrvMsg;

            _responseString = srvMsg.CqrBaseMsg(Constants.NACK);

            try
            {
                if (!string.IsNullOrEmpty(cryptMsg) && cryptMsg.Length >= 8)
                {
                    fullSrvMsg = srvMsg.NCqrSrvMsg<string>(cryptMsg);
                    _contact = AddContact(fullSrvMsg.Sender);
                    chatRSrvMsg = InviteToChatRoom(fullSrvMsg);
                    _chatRoomNumber = chatRSrvMsg.ChatRoomNr;

                    _responseString = srvMsg.CqrSrvMsg<string>(chatRSrvMsg);
                }
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
                Area23Log.LogStatic(ex);
            }

            Area23Log.LogStatic($"ChatRoomInvite(string cryptMsg) finished. chatroomId = {_chatRoomNumber}.\n");
            return _responseString;

        }

        /// <summary>
        /// Polls a chat room for new messages
        /// </summary>
        /// <param name="cryptMsg">encrypted <see cref="FullSrvMsg<string>"/> with chat room number and last polled dates</param>
        /// <returns>
        /// encrypted <see cref="FullSrvMsg<string>"/> including chatroom number 
        /// with encrypted clientmsg with clientkey.
        /// Server doesn't know client key and always delivers encrypted encrypted messages
        /// Server can only read and decrypt outer envelope message encrypted with server key
        /// </returns>
        [WebMethod]
        public string ChatRoomPoll(string cryptMsg)
        {
            Area23Log.LogStatic($"ChatRoomPoll(string cryptMsg) called.  cryptMsg.Length = {cryptMsg.Length}.\n");
            InitMethod();

            Dictionary<long, string> dict = new Dictionary<long, string>();
            bool isValid = false;
            SrvMsg srvMsg = new SrvMsg(_serverKey, _serverKey);
            FullSrvMsg<string> fullSrvMsg;

            _responseString = srvMsg.CqrBaseMsg(Constants.NACK);

            try
            {
                if (!string.IsNullOrEmpty(cryptMsg) && cryptMsg.Length >= 8)
                {
                    fullSrvMsg = srvMsg.NCqrSrvMsg<string>(cryptMsg);
                    _contact = fullSrvMsg.Sender;
                    _chatRoomNumber = (!string.IsNullOrEmpty(fullSrvMsg.ChatRoomNr)) ? fullSrvMsg.ChatRoomNr : fullSrvMsg.Sender.ChatRoomId;

                    FullSrvMsg<string> chatRoomMsg = (new JsonChatRoom(_chatRoomNumber)).LoadJsonChatRoom(fullSrvMsg, _chatRoomNumber);
                    isValid = ValidateChatRoomNr(fullSrvMsg, chatRoomMsg, _chatRoomNumber);
                    if (isValid)
                    {
                        if (UseApplicationState && (HttpContext.Current.Application[_chatRoomNumber] != null))
                            dict = (Dictionary<long, string>)HttpContext.Current.Application[_chatRoomNumber];

                        //if (UseAmazonElasticCache)
                        //{
                        //    string dictJson = RedIs.Db.StringGet(_chatRoomNumber);
                        //    dict = (Dictionary<long, string>)JsonConvert.DeserializeObject<Dictionary<long, string>>(dictJson);
                        //}

                        List<long> pollKeys = new List<long>();
                        foreach (long ticksLong in dict.Keys)
                        {
                            if (ticksLong > fullSrvMsg.Sender.LastPolled.Ticks)
                                pollKeys.Add(ticksLong);
                        }
                        long polledPtr = -1;
                        if (pollKeys.Count > 0)
                        {
                            polledPtr = pollKeys[0];
                            string firstPollClientMsg = dict[polledPtr];
                            if (string.IsNullOrEmpty(firstPollClientMsg))
                            {
                                if (pollKeys.Count > 1)
                                {
                                    polledPtr = pollKeys[1];
                                    firstPollClientMsg = dict[polledPtr];
                                }
                            }

                            chatRoomMsg.TContent = firstPollClientMsg;

                            DateTime polledMsgDate = new DateTime(polledPtr);
                            _contact = AddPollDate(_contact, polledMsgDate, false);
                            chatRoomMsg.Sender = AddPollDate(chatRoomMsg.Sender, polledMsgDate, false);

                            UpdateContact(chatRoomMsg.Sender);
                            chatRoomMsg = (new JsonChatRoom(_chatRoomNumber)).SaveJsonChatRoom(chatRoomMsg, _chatRoomNumber);
                            chatRoomMsg.Sender.LastPolled = polledMsgDate;
                        }

                    }

                    _responseString = srvMsg.CqrSrvMsg<string>(chatRoomMsg);

                }
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
                Area23Log.LogStatic(ex);
            }

            Area23Log.LogStatic($"ChatRoomPushMessage(string cryptMsg, string chatRoomMembersCrypted) finihed. chatroomId =  {_chatRoomNumber}.\n");
            return _responseString;

        }


        /// <summary>
        /// Pushes a new message for chatroom to the server
        /// </summary>
        /// <param name="cryptMsg">encrypted <see cref="FullSrvMsg<string>"/> with chat room number and last polled dates</param>
        /// <param name="chatRoomMembersCrypted">with client key encrypted message, that is stored in proc of server, but server can't decrypt</param>
        /// <returns>encrypted <see cref="FullSrvMsg<string>"/> with chat room number and last polled date changed to now</returns>
        [WebMethod]
        public string ChatRoomPushMessage(string cryptMsg, string chatRoomMembersCrypted)
        {
            Area23Log.LogStatic($"ChatRoomPushMessage(string cryptMsg, string chatRoomMembersCrypted) called. len = {chatRoomMembersCrypted.Length}.\n");
            InitMethod();
            bool isValid = false;
            Dictionary<long, string> dict;
            SrvMsg srvMsg = new SrvMsg(_serverKey, _serverKey);
            FullSrvMsg<string> fullSrvMsg;

            _responseString = srvMsg.CqrBaseMsg(Constants.NACK);

            try
            {
                if (!string.IsNullOrEmpty(cryptMsg) && cryptMsg.Length >= 8)
                {
                    fullSrvMsg = srvMsg.NCqrSrvMsg<string>(cryptMsg);
                    _contact = fullSrvMsg.Sender;
                    _chatRoomNumber = (!string.IsNullOrEmpty(fullSrvMsg.ChatRoomNr)) ? fullSrvMsg.ChatRoomNr : fullSrvMsg.Sender.ChatRoomId;

                    FullSrvMsg<string> chatRoomMsg = (new JsonChatRoom(_chatRoomNumber)).LoadJsonChatRoom(fullSrvMsg, _chatRoomNumber);
                    isValid = ValidateChatRoomNr(fullSrvMsg, chatRoomMsg, _chatRoomNumber);
                    if (isValid)
                    {
                        DateTime now = DateTime.Now;

                        dict = new Dictionary<long, string>();
                        if (BaseWebService.UseApplicationState && HttpContext.Current.Application[_chatRoomNumber] != null)
                            dict = (Dictionary<long, string>)HttpContext.Current.Application[_chatRoomNumber];
                        //if (BaseWebService.UseAmazonElasticCache)
                        //{
                        //    string dictJson = RedIs.Db.StringGet(_chatRoomNumber);
                        //    dict = (Dictionary<long, string>)JsonConvert.DeserializeObject<Dictionary<long, string>>(dictJson);
                        //}

                        dict.Add(now.Ticks, chatRoomMembersCrypted);

                        if (BaseWebService.UseApplicationState)
                            HttpContext.Current.Application[_chatRoomNumber] = dict;
                        //if (BaseWebService.UseAmazonElasticCache)
                        //{
                        //    string dictJson = JsonConvert.SerializeObject(dict);
                        //    RedIs.Db.StringSet(_chatRoomNumber, dictJson);
                        //}

                        _contact = AddPollDate(_contact, now, true);
                        chatRoomMsg.Sender = AddPollDate(chatRoomMsg.Sender, now, true);

                        UpdateContact(_contact);
                        chatRoomMsg = (new JsonChatRoom(_chatRoomNumber)).SaveJsonChatRoom(chatRoomMsg, _chatRoomNumber);
                        chatRoomMsg.Sender.LastPushed = now;

                    }

                    _responseString = srvMsg.CqrSrvMsg<string>(chatRoomMsg);

                }
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
                Area23Log.LogStatic(ex);
            }

            Area23Log.LogStatic($"ChatRoomPushMessage(string cryptMsg, string chatRoomMembersCrypted) finished. chatroomId =  {_chatRoomNumber}.\n");
            return _responseString;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="cryptMsg"></param>
        /// <returns></returns>
        [WebMethod]
        public string ChatRoomClose(string cryptMsg)
        {
            Area23Log.LogStatic($"ChatRoomClose(string cryptMsg) started. cryptMsg.Length =  {cryptMsg.Length}.\n");
            InitMethod();
            bool isValid = false;
            SrvMsg srvMsg = new SrvMsg(_serverKey, _serverKey);
            FullSrvMsg<string> fullSrvMsg;
            List<CqrContact> _invited = new List<CqrContact>();

            _responseString = srvMsg.CqrBaseMsg(Constants.NACK);

            try
            {
                if (!string.IsNullOrEmpty(cryptMsg) && cryptMsg.Length >= 8)
                {
                    fullSrvMsg = srvMsg.NCqrSrvMsg<string>(cryptMsg);
                    _contact = AddContact(fullSrvMsg.Sender);
                    _chatRoomNumber = (!string.IsNullOrEmpty(fullSrvMsg.ChatRoomNr)) ? fullSrvMsg.ChatRoomNr : fullSrvMsg.Sender.ChatRoomId;
                    JsonChatRoom jschatRoom = new JsonChatRoom(_chatRoomNumber);
                    FullSrvMsg<string> chatRoomMsg = jschatRoom.LoadJsonChatRoom(fullSrvMsg, _chatRoomNumber);
                    isValid = ValidateChatRoomNr(fullSrvMsg, chatRoomMsg, _chatRoomNumber);
                    if (isValid)
                    {
                        jschatRoom.DeleteJsonChatRoom(_chatRoomNumber);
                    }

                    _responseString = srvMsg.CqrSrvMsg<string>(chatRoomMsg);

                }
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
                Area23Log.LogStatic(ex);
            }

            Area23Log.LogStatic($"ChatRoomClose(string cryptMsg) finished. deleted chat room chatroomId =  {_chatRoomNumber}.\n");

            return _responseString;

        }



        [WebMethod]
        public override string TestService()
        {
            return base.TestService();
        }

        [WebMethod]
        public override string GetIPAddress()
        {
            return base.GetIPAddress();
        }


        [WebMethod]
        public virtual string TestCache()
        {
            return base.TestCache();
        }


    }


}
