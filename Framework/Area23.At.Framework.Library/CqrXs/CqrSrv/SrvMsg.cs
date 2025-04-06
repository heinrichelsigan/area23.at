using Area23.At.Framework.Library.CqrXs.CqrJd;
using Area23.At.Framework.Library.CqrXs.CqrMsg;
using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Net.IpSocket;
using Area23.At.Framework.Library.Net.WebHttp;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using Org.BouncyCastle.Cms;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace Area23.At.Framework.Library.CqrXs.CqrSrv
{

    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    [DataContract(Name = "SrvMsg")]
    public class SrvMsg : BaseMsg
    {

        protected internal readonly string cKey;
        protected internal readonly string cHash;
        protected internal readonly byte[] cKeyBytes;

        protected internal readonly SymmCipherPipe clientSymmPipe;

        public string ClientPipeString { get; set; }
        public string cClientMessage { get; protected internal set; }

        public CqrContact CqrSender { get; private set; }
        public CqrContact CqrRecipient { get; private set; }


        /// <summary>
        /// CqrServerMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public SrvMsg(string srvKey = "", string clientKey = "") : base(srvKey)
        {
            if (string.IsNullOrEmpty(clientKey))
            {
                throw new ArgumentNullException("public BaseMsg(string srvKey = \"\")");
            }
            cKey = clientKey;
            cHash = EnDeCodeHelper.KeyToHex(clientKey);
            cKeyBytes = CryptHelper.GetUserKeyBytes(cKey, cHash, 16);
            clientSymmPipe = new SymmCipherPipe(cKeyBytes, 8);
            ClientPipeString = clientSymmPipe.PipeString;
        }

        /// <summary>
        /// ctor with sender & recipient
        /// </summary>
        /// <param name="sender"><see cref="CqrContact">CqrContact sender</see></param>
        /// <param name="receipient"><see cref="CqrContact">CqrContact receipient</see></param>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        public SrvMsg(CqrContact sender, CqrContact receipient, string srvKey = "", string clientKey = "") : this(srvKey, clientKey)
        {
            CqrSender = sender;
            CqrRecipient = receipient;
        }


        #region CqrSrvMsg securing server message

        /// <summary>
        /// CqrSrvMsg
        /// </summary>
        /// <param name="sender"><see cref="CqrContact"/></param>
        /// <param name="receipient"><see cref="CqrContact"/></param>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="CipherPipe"/></returns>
        /// <exception cref="InvalidDataException"></exception>
        public string CqrSrvMsg(CqrContact sender, CqrContact receipient, string msg, EncodingType encType = EncodingType.Base64)
        {
            if (sender == null || receipient == null)
                throw new InvalidDataException("CqrSender contact or CqrRecipient conact are null.");

            CqrSender = sender;
            CqrRecipient = receipient;

            FullSrvMsg<string> fullMsg = new FullSrvMsg<string>(sender, receipient, msg, PipeString);
            fullMsg.Md5Hash = Crypt.Hash.MD5Sum.HashString(msg);
            string allMsg = fullMsg.ToJson();
            fullMsg._message = allMsg;
            fullMsg.RawMessage = allMsg + "\n" + symmPipe.PipeString + "\0";

            byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(allMsg);
            byte[] cqrMsgBytes = (LibPaths.CqrEncrypt) ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;
            CqrMessage = EnDeCodeHelper.EncodeBytes(cqrMsgBytes, encType);

            return CqrMessage;
        }

        /// <summary>
        /// CqrSrvMsg generic
        /// </summary>
        /// <typeparam name="T">generic content of message</typeparam>
        /// <param name="sender"><see cref="CqrContact"/></param>
        /// <param name="receipient"><see cref="CqrContact"/></param>
        /// <param name="tcontent">generic content will be mapped to msg</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="CipherPipe"/></returns>
        /// <exception cref="InvalidDataException"></exception>
        public string CqrSrvMsg<T>(CqrContact sender, CqrContact receipient, T tcontent, EncodingType encType = EncodingType.Base64) where T : class
        {
            if (sender == null || receipient == null)
                throw new InvalidDataException("CqrSender contact or CqrRecipient conact are null.");

            CqrSender = sender;
            CqrRecipient = receipient;

            FullSrvMsg<T> fullMsg = new FullSrvMsg<T>(sender, receipient, tcontent, PipeString);
            if (fullMsg.TContent != null)
            {
                try
                {
                    fullMsg.Md5Hash = Crypt.Hash.MD5Sum.HashString((string)fullMsg.TContent.ToString());
                }
                catch (Exception exCqrSrvMsg)
                {
                    Area23Log.LogStatic(exCqrSrvMsg);
                }
            }
            string allMsg = fullMsg.ToJson();
            fullMsg._message = allMsg;
            fullMsg.RawMessage = allMsg + "\n" + symmPipe.PipeString + "\0";

            byte[] allBytes = EnDeCodeHelper.GetBytesFromString(allMsg);
            byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(fullMsg._message);
            byte[] cqrMsgBytes = (LibPaths.CqrEncrypt) ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;
            CqrMessage = EnDeCodeHelper.EncodeBytes(cqrMsgBytes, encType);

            return CqrMessage;
        }

        public string CqrSrvMsg<TC>(FullSrvMsg<TC> fullServMsg, MsgKind msgKind = MsgKind.Server, EncodingType encType = EncodingType.Base64) where TC : class
        {
            if (fullServMsg.Sender == null || fullServMsg.Recipient == null)
                throw new InvalidDataException("CqrSender contact or CqrRecipient conact are null.");

            CqrSender = fullServMsg.Sender;
            CqrRecipient = fullServMsg.Recipient;
            fullServMsg._hash = (msgKind == MsgKind.Server) ? PipeString : ClientPipeString;
            if (fullServMsg.TContent != null)
            {
                try
                {
                    fullServMsg.Md5Hash = Crypt.Hash.MD5Sum.HashString((string)fullServMsg.TContent.ToString());
                }
                catch
                (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                }
            }
            if (string.IsNullOrEmpty(fullServMsg.ChatRoomNr))
                fullServMsg.ChatRoomNr = (!string.IsNullOrEmpty(fullServMsg.Sender.ChatRoomNr)) ? fullServMsg.Sender.ChatRoomNr : fullServMsg.ChatRoomNr;

            string allMsg = fullServMsg.ToJson();
            fullServMsg._message = allMsg;
            fullServMsg.RawMessage = allMsg + "\n" + symmPipe.PipeString + "\0";

            byte[] allBytes = EnDeCodeHelper.GetBytesFromString(fullServMsg.RawMessage);
            byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(fullServMsg._message);
            byte[] cqrMsgBytes = msgBytes;
            if (LibPaths.CqrEncrypt)
                cqrMsgBytes = (msgKind == MsgKind.Server) ?
                    symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) :
                    symmPipe.MerryGoRoundEncrpyt(msgBytes, cKey, cHash);

            CqrMessage = EnDeCodeHelper.EncodeBytes(cqrMsgBytes, encType);

            return CqrMessage;
        }

        public string[] CqrSrvMsg<TS, TC>(FullSrvMsg<TS> fullServMsg, FullSrvMsg<TC> clientMsg, EncodingType encType = EncodingType.Base64)
            where TS : class
            where TC : class

        {
            if (fullServMsg.Sender == null || fullServMsg.Recipient == null)
                throw new InvalidDataException("CqrSender contact or CqrRecipient conact are null.");

            CqrSender = fullServMsg.Sender;
            CqrRecipient = fullServMsg.Recipient;
            fullServMsg._hash = PipeString;
            if (fullServMsg.TContent != null)
            {
                try
                {
                    fullServMsg.Md5Hash = Crypt.Hash.MD5Sum.HashString((string)fullServMsg.TContent.ToString());
                }
                catch
                (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                }
            }
            string allSrvMsg = fullServMsg.ToJson();
            fullServMsg._message = allSrvMsg;
            fullServMsg.RawMessage = allSrvMsg + "\n" + fullServMsg._hash + "\0";

            byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(fullServMsg._message);
            byte[] srvMsgBytes = msgBytes;
            if (LibPaths.CqrEncrypt)
                srvMsgBytes = symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash);

            CqrMessage = EnDeCodeHelper.EncodeBytes(srvMsgBytes, encType);

            clientMsg._hash = ClientPipeString;
            if (clientMsg.TContent != null)
            {
                try
                {
                    clientMsg.Md5Hash = Crypt.Hash.MD5Sum.HashString((string)clientMsg.TContent.ToString());
                }
                catch
                (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                }
            }
            string allClientMsg = clientMsg.ToJson();
            clientMsg._message = allClientMsg;
            clientMsg.RawMessage = allClientMsg + "\n" + clientMsg._hash + "\0";

            byte[] cMsgBytes = EnDeCodeHelper.GetBytesFromString(clientMsg._message);
            byte[] clientMsgBytes = cMsgBytes;
            if (LibPaths.CqrEncrypt)
                clientMsgBytes = symmPipe.MerryGoRoundEncrpyt(cMsgBytes, cKey, cHash);

            cClientMessage = EnDeCodeHelper.EncodeBytes(clientMsgBytes, encType);



            string[] rets = { CqrMessage, cClientMessage };
            return rets;
        }

        #endregion CqrSrvMsg securing server message


        #region NCqrSrvMsg decrypting server message

        /// <summary>
        /// NCqrSrvMsg decryptes an secure encrypted msg 
        /// </summary>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns><see cref="FullSrvMsg{T}"/></returns>
        /// <exception cref="InvalidOperationException">will be thrown, 
        /// if server and client or both side use a different secret key 4 encryption</exception>
        public FullSrvMsg<TS> NCqrSrvMsg<TS>(string cqrMessage, EncodingType encType = EncodingType.Base64)
             where TS : class
        {
            FullSrvMsg<TS> fullMsg = null;
            MsgContent msgContent = base.NCqrBaseMsg(cqrMessage, encType);

            if (msgContent != null && !string.IsNullOrEmpty(msgContent.Message))
            {
                if (msgContent.Message.IsValidJson())
                    fullMsg = JsonConvert.DeserializeObject<FullSrvMsg<TS>>(msgContent.Message);
                else if (msgContent.Message.StartsWith("{\"") && msgContent.Message.Contains("\"_hash\":") && msgContent.Message.Contains("\"_message\":"))
                {
                    if (Char.IsLetter(msgContent.Message[msgContent.Message.Length - 1]) || Char.IsDigit(msgContent.Message[msgContent.Message.Length - 1]))
                    {
                        msgContent._message += "\" }";
                    }
                    fullMsg = JsonConvert.DeserializeObject<FullSrvMsg<TS>>(msgContent.Message);
                }
                else if (msgContent.Message.IsValidXml())
                    fullMsg = Static.Utils.DeserializeFromXml<FullSrvMsg<TS>>(msgContent.Message);
                try
                {
                    if (fullMsg != null && fullMsg is FullSrvMsg<TS> fullSrvMsg && fullSrvMsg != null && !string.IsNullOrEmpty(fullSrvMsg.Sender?.Email))
                    {
                        fullMsg.Sender = fullSrvMsg.Sender;
                        fullMsg._hash = fullSrvMsg._hash;
                        fullMsg.Recipients = fullSrvMsg.Recipients;
                        fullMsg.TContent = fullSrvMsg.TContent;
                        fullMsg.ChatRoomNr = fullSrvMsg.ChatRoomNr;
                        fullMsg.ChatRuid = fullSrvMsg.ChatRuid;
                        fullMsg.TicksLong = fullSrvMsg.TicksLong;
                        fullMsg.LastPolled = fullSrvMsg.LastPolled;
                        fullMsg.LastPushed = fullSrvMsg.LastPushed;
                        fullMsg.Md5Hash = fullSrvMsg.Md5Hash;
                    }
                    return fullMsg;
                }
                catch (Exception exJson)
                {
                    SLog.Log(exJson);
                }
            }

            return fullMsg;
        }

        public FullSrvMsg<TC> NCqrClientMsgTC<TC>(string clientMessage, EncodingType encType = EncodingType.Base64)
            where TC : class
        {
            FullSrvMsg<TC> clientMsg = new FullSrvMsg<TC>();
            FullSrvMsg<TC> clientOutMsg = new FullSrvMsg<TC>();
            CqrMessage = clientMessage.TrimEnd("\0".ToCharArray());

            byte[] cipherBytes = EnDeCodeHelper.DecodeText(CqrMessage, encType);
            byte[] unroundedMerryBytes = LibPaths.CqrEncrypt ? symmPipe.DecrpytRoundGoMerry(cipherBytes, cKey, cHash) : cipherBytes;
            string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes); //DeEnCoder.GetStringFromBytesTrimNulls(unroundedMerryBytes);
            while (decrypted[decrypted.Length - 1] == '\0')
                decrypted = decrypted.Substring(0, decrypted.Length - 1);

            MsgEnum msgEnum = MsgEnum.RawWithHashAtEnd;
            if (decrypted.IsValidJson())
                msgEnum = MsgEnum.Json;
            else if (decrypted.StartsWith("{\"") && decrypted.Contains("\"_hash\":") && decrypted.Contains("\"_message\":"))
            {
                if (Char.IsLetter(decrypted[decrypted.Length - 1]) || Char.IsDigit(decrypted[decrypted.Length - 1]))
                {
                    decrypted += "\" }";
                }
                msgEnum = MsgEnum.Json;
            }
            else if (decrypted.IsValidXml())
                msgEnum = MsgEnum.Xml;

            MsgContent msgContent = new MsgContent(decrypted, msgEnum);
            string hashVerification = msgContent.Hash;
            if (!VerifyHash(hashVerification, clientSymmPipe.PipeString))
            {
                string hashSymShow = clientSymmPipe.PipeString ?? "        ";
                throw new InvalidOperationException(
                    string.Format("SymmCiphers [{0}] in crypt pipeline doesn't match serverside key !?$* byte length={1}.",
                        hashSymShow.Substring(0, 2) + "...." + hashSymShow.Substring(6), keyBytes.Length));
            }

            if (msgContent != null && !string.IsNullOrEmpty(msgContent._message))
            {
                if (msgContent.MsgType == MsgEnum.Json || msgContent._message.IsValidJson())
                    clientOutMsg = JsonConvert.DeserializeObject<FullSrvMsg<TC>>(msgContent._message);
                else if (msgContent.Message.StartsWith("{\"") && msgContent.Message.Contains("\"_hash\":") && msgContent.Message.Contains("\"_message\":"))
                {
                    if (Char.IsLetter(msgContent.Message[msgContent.Message.Length - 1]) || Char.IsDigit(msgContent.Message[msgContent.Message.Length - 1]))
                    {
                        msgContent._message += "\" }";
                    }
                    clientOutMsg = JsonConvert.DeserializeObject<FullSrvMsg<TC>>(msgContent.Message);
                }
                else if (msgContent.MsgType == MsgEnum.Xml || msgContent._message.IsValidXml())
                    clientOutMsg = Static.Utils.DeserializeFromXml<FullSrvMsg<TC>>(msgContent._message);

                try
                {
                    if (clientOutMsg != null && clientOutMsg is FullSrvMsg<TC> fullSrvMsg && fullSrvMsg != null && !string.IsNullOrEmpty(clientOutMsg._message))
                    {
                        clientOutMsg.Sender = fullSrvMsg.Sender;
                        clientOutMsg._hash = fullSrvMsg._hash;
                        clientOutMsg.Recipients = fullSrvMsg.Recipients;
                        clientOutMsg.Md5Hash = fullSrvMsg.Md5Hash;
                        clientOutMsg.ChatRoomNr = fullSrvMsg.ChatRoomNr;
                        clientOutMsg.ChatRuid = fullSrvMsg.ChatRuid;
                        clientOutMsg.TicksLong = fullSrvMsg.TicksLong;
                        clientOutMsg.LastPolled = fullSrvMsg.LastPolled;
                        clientOutMsg.LastPushed = fullSrvMsg.LastPushed;
                        clientOutMsg.TContent = fullSrvMsg.TContent;

                        return clientOutMsg;
                    }
                }
                catch (Exception exJson)
                {
                    SLog.Log(exJson);
                }
            }

            clientOutMsg.RawMessage = msgContent.RawMessage;
            clientOutMsg._hash = msgContent._hash;
            clientOutMsg._message = msgContent._message;
            clientOutMsg.Md5Hash = msgContent.Md5Hash;

            return clientMsg;
        }


        /// <summary>
        /// NCqrSrvMsgT decryptes an secure encrypted generic msg 
        /// </summary>
        /// <typeparam name="TC"></typeparam>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns><see cref="FullSrvMsg{TC}"/></returns>
        public FullSrvMsg<TC> NCqrSrvMsgSC<TC>(string cqrMessage, MsgKind msgKind = MsgKind.Server, EncodingType encType = EncodingType.Base64)
            where TC : class
        {
            FullSrvMsg<TC> fullMsg = new FullSrvMsg<TC>();
            fullMsg = (msgKind == MsgKind.Server) ? NCqrSrvMsg<TC>(cqrMessage, encType) : NCqrClientMsgTC<TC>(cqrMessage, encType);

            return fullMsg;
        }


        public ClientSrvMsg<TS, TC> NCqrClientSrvMsgTSC<TS, TC>(string serverEncrypted, string clientEncrypted, EncodingType encType = EncodingType.Base64)
            where TS : class
            where TC : class
        {
            FullSrvMsg<TS> fullServer = NCqrSrvMsgSC<TS>(serverEncrypted, MsgKind.Server, encType);
            FullSrvMsg<TC> fullClient = NCqrSrvMsgSC<TC>(clientEncrypted, MsgKind.Client, encType);

            ClientSrvMsg<TS, TC> retMes = new ClientSrvMsg<TS, TC>(fullServer, fullClient, serverEncrypted, clientEncrypted);
            return retMes;
        }

        #endregion NCqrSrvMsg decrypting server message


        #region Response<T> response = webServiceSoapClient.WebMethod_To_Invoke(Request<T> request)

        /// <summary>
        /// Send_InitChatRoom_Soap{<typeparamref name="T"/>} Sends an chat roomm invitation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullServerMsg"><see cref="FullSrvMsg{T}"/>, containing char room number, sender and recipients</param>
        /// <param name="srvIp"></param>
        /// <param name="encodingType"></param>
        /// <returns><see cref="FullSrvMsg{string}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public FullSrvMsg<string> Send_InitChatRoom_Soap<T>(FullSrvMsg<T> fullServerMsg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
            where T : class
        {
            string cryptSrv = CqrSrvMsg<T>(fullServerMsg);

            CqrService webService = new CqrService();

            string response = string.Empty;
            try
            {
                response = webService.ChatRoomInvite(cryptSrv);
            }
            catch (Exception exSoap)
            {
                Area23Log.LogStatic($"Exception {exSoap.GetType()}: {exSoap.Message}\n\t{exSoap}\n");
                throw;
            }

            FullSrvMsg<string> rfmsg = NCqrSrvMsg<string>(response, EncodingType.Base64);


            return rfmsg;
        }


        /// <summary>
        /// SendChatMsg_Soap{<typeparamref name="T"/>, <typeparamref name="TC"/>} 
        /// </summary>
        /// <param name="fullServerMsg"><see cref="FullSrvMsg{T}"/>, containing char room number, sender and recipients</param>
        /// <param name="fullClientMsg">client encrypted messagem, that server can't decrypt, <see cref="FullSrvMsg{TC}"/></param>fullClientMsgfullClientMsg
        /// <param name="srvIp"></param>
        /// <param name="encodingType"></param>
        /// <returns><see cref="FullSrvMsg{string}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public FullSrvMsg<string> SendChatMsg_Soap<T, TC>(FullSrvMsg<T> fullServerMsg, FullSrvMsg<TC> fullClientMsg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
            where T : class
            where TC : class
        {
            string cryptSrv = CqrSrvMsg<T>(fullServerMsg, MsgKind.Server);
            string cryptPatner = CqrSrvMsg<TC>(fullClientMsg, MsgKind.Client);

            CqrService webService = new CqrService();
            string response = webService.ChatRoomPushMessage(cryptSrv, cryptPatner);
            FullSrvMsg<string> rfmsg = NCqrSrvMsg<string>(response, EncodingType.Base64);

            return rfmsg;
        }


        /// <summary>
        /// SendChatMsg_Soap_Simple{<typeparamref name="TS"/>} send a simple push message to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullServerMsg"><see cref="FullSrvMsg{T}"/>, containing char room number, sender and recipients</param>
        /// <param name="encryptedClientMsg">already encrypted client msg, that server can't read</param>
        /// <param name="srvIp"></param>
        /// <param name="encodingType"></param>
        /// <returns><see cref="FullSrvMsg{string}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public FullSrvMsg<string> SendChatMsg_Soap_Simple<TS>(FullSrvMsg<TS> fullServerMsg, string encryptedClientMsg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
           where TS : class
        {
            string cryptSrv = CqrSrvMsg<TS>(fullServerMsg, MsgKind.Server);

            CqrService webService = new CqrService();
            string response = webService.ChatRoomPushMessage(cryptSrv, encryptedClientMsg);

            FullSrvMsg<string> rfmsg = NCqrSrvMsg<string>(response, EncodingType.Base64);

            return rfmsg;
        }


        /// <summary>
        /// ReceiveChatMsg_Soap{<typeparamref name="T"/>} is a polling chat server request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullServerMsg"><see cref="FullSrvMsg{T}"/>, containing char room number, sender and recipients</param>
        /// <param name="srvIp"></param>
        /// <param name="encodingType"></param>
        /// <returns><see cref="FullSrvMsg{string}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public FullSrvMsg<string> ReceiveChatMsg_Soap<T>(FullSrvMsg<T> fullServerMsg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        where T : class
        {
            string cryptSrv = CqrSrvMsg<T>(fullServerMsg, MsgKind.Server);

            CqrService webService = new CqrService();
            string response = webService.ChatRoomPoll(cryptSrv);
            FullSrvMsg<string> rfmsg = NCqrSrvMsg<string>(response, EncodingType.Base64);

            return rfmsg;

        }


        #endregion Response<T> response = webServiceSoapClient.WebMethod_To_Invoke(Request<T> request)


        #region obsolete methods

        /// <summary>
        /// Send_CqrSrvMsg obsolete, please use <see cref="SrvMsg1.SendFirstSrvMsg_Soap(CqrContact, IPAddress, EncodingType)"/> instead
        /// </summary>
        [Obsolete("Send_CqrSrvMsg is obsolete, please use SrvMsg1.SendFirstSrvMsg_Soap(CqrContact, IPAddress, EncodingType)", true)]
        public string Send_CqrSrvMsg(string msg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {
            string encrypted = string.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                CqrBaseMsg(msg));

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }


        [Obsolete("Please use SendChatMsg_Soap<T, TC> or better even endChatMsg_Soap_Simple<TS>(FullSrvMsg<TS> fullServerMsg, string encryptedClientMsg, ...)", true)]
        public string Send_CqrSrvMsgT<T>(FullSrvMsg<T> fullServerMsg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
            where T : class
        {
            string encrypted = string.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                CqrSrvMsg<T>(fullServerMsg));

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }


        [Obsolete("Please use SendChatMsg_Soap<T, TC> or better even endChatMsg_Soap_Simple<TS>(FullSrvMsg<TS> fullServerMsg, string encryptedClientMsg, ...)", true)]
        public string Send_CqrSrvMsgTTC<T, TC>(FullSrvMsg<T> fullServerMsg, FullSrvMsg<TC> fullClientMsg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
            where T : class
            where TC : class
        {
            string encrypted = string.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg={1}\r\nButtonSubmit=Submit",
                CqrSrvMsg<T>(fullServerMsg, MsgKind.Server, encodingType),
                CqrSrvMsg<TC>(fullClientMsg, MsgKind.Client, encodingType));

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }

        #endregion obsolete methods

    }

}
