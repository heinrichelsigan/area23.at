using Area23.At.Framework.Core.CqrXs.CqrMsg;
using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Net.WebHttp;
using Area23.At.Framework.Core.Util;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace Area23.At.Framework.Core.CqrXs.CqrSrv
{


    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
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



        /// <summary>
        /// CqrSrvMsg
        /// </summary>
        /// <param name="sender"><see cref="CqrContact"/></param>
        /// <param name="receipient"><see cref="CqrContact"/></param>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        /// <exception cref="InvalidDataException"></exception>
        public string CqrSrvMsg(CqrContact sender, CqrContact receipient, string msg, EncodingType encType = EncodingType.Base64)
        {
            if (sender == null || receipient == null)
                throw new InvalidDataException("CqrSender contact or CqrRecipient conact are null.");

            CqrSender = sender;
            CqrRecipient = receipient;

            FullSrvMsg<string> fullMsg = new FullSrvMsg<string>(sender, receipient, msg, PipeString);
            string allMsg = fullMsg.ToJson();
            fullMsg._message = allMsg;
            fullMsg._rawMessage = allMsg + "\n" + symmPipe.PipeString + "\0";

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
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        /// <exception cref="InvalidDataException"></exception>
        public string CqrSrvMsg<T>(CqrContact sender, CqrContact receipient, T tcontent, EncodingType encType = EncodingType.Base64) where T : class
        {
            if (sender == null || receipient == null)
                throw new InvalidDataException("CqrSender contact or CqrRecipient conact are null.");

            CqrSender = sender;
            CqrRecipient = receipient;

            FullSrvMsg<T> fullMsg = new FullSrvMsg<T>(sender, receipient, tcontent, PipeString);
            string allMsg = fullMsg.ToJson();
            fullMsg._message = allMsg;
            fullMsg._rawMessage = allMsg + "\n" + symmPipe.PipeString + "\0";

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

            string allMsg = fullServMsg.ToJson();
            fullServMsg._message = allMsg;
            fullServMsg._rawMessage = allMsg + "\n" + fullServMsg._hash + "\0";

            byte[] allBytes = EnDeCodeHelper.GetBytesFromString(fullServMsg._rawMessage);
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
            string allSrvMsg = fullServMsg.ToJson();
            fullServMsg._message = allSrvMsg;
            fullServMsg._rawMessage = allSrvMsg + "\n" + fullServMsg._hash + "\0";

            byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(fullServMsg._message);
            byte[] srvMsgBytes = msgBytes;
            if (LibPaths.CqrEncrypt)
                srvMsgBytes = symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash);

            CqrMessage = EnDeCodeHelper.EncodeBytes(srvMsgBytes, encType);

            clientMsg._hash = ClientPipeString;
            string allClientMsg = clientMsg.ToJson();
            clientMsg._message = allClientMsg;
            clientMsg._rawMessage = allClientMsg + "\n" + clientMsg._hash + "\0";

            byte[] cMsgBytes = EnDeCodeHelper.GetBytesFromString(clientMsg._message);
            byte[] clientMsgBytes = cMsgBytes;
            if (LibPaths.CqrEncrypt)
                clientMsgBytes = symmPipe.MerryGoRoundEncrpyt(cMsgBytes, cKey, cHash);

            cClientMessage = EnDeCodeHelper.EncodeBytes(clientMsgBytes, encType);

            

            string[] rets  =  { CqrMessage, cClientMessage };
            return rets;
        }




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
            FullSrvMsg<TS> fullMsg = new FullSrvMsg<TS>();
            MsgContent msgContent = base.NCqrBaseMsg(cqrMessage, encType);

            if (msgContent != null && !string.IsNullOrEmpty(msgContent.Message))
                fullMsg.FromJson(msgContent.Message);

            return fullMsg;
        }



        public FullSrvMsg<TC> NCqrClientMsgTC<TC>(string clientMessage, EncodingType encType = EncodingType.Base64)
            where TC : class
        {
            FullSrvMsg<TC> clientMsg = new FullSrvMsg<TC>();
            FullSrvMsg<TC>? clientOutMsg = new FullSrvMsg<TC>();
            CqrMessage = clientMessage.TrimEnd("\0".ToCharArray());

            byte[] cipherBytes = EnDeCodeHelper.DecodeText(CqrMessage, encType);
            byte[] unroundedMerryBytes = LibPaths.CqrEncrypt ? symmPipe.DecrpytRoundGoMerry(cipherBytes, cKey, cHash) : cipherBytes;
            string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes); //EnDeCodeHelper.GetStringFromBytesTrimNulls(unroundedMerryBytes);
            while (decrypted[decrypted.Length - 1] == '\0')
                decrypted = decrypted.Substring(0, decrypted.Length - 1);

            MsgEnum msgEnum = (decrypted.IsValidJson()) ? MsgEnum.JsonSerialized : MsgEnum.RawWithHashAtEnd;
            MsgContent msgContent = new MsgContent(decrypted, msgEnum);
            string hashVerification = msgContent.Hash;
            if (!VerifyHash(hashVerification, clientSymmPipe.PipeString))
            {
                string hashSymShow = clientSymmPipe.PipeString ?? "        ";
                throw new InvalidOperationException(
                    string.Format("SymmCiphers [{0}] in crypt pipeline doesn't match serverside key !?$* byte length={1}.",
                        hashSymShow.Substring(0, 2) + "...." + hashSymShow.Substring(6), keyBytes.Length));
            }

            if (msgContent != null && !string.IsNullOrEmpty(msgContent.Message))
                clientOutMsg = clientMsg.FromJson(msgContent.Message);

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
            FullSrvMsg<TC> fullClient= NCqrSrvMsgSC<TC>(clientEncrypted, MsgKind.Client, encType);

            ClientSrvMsg<TS, TC> retMes = new ClientSrvMsg<TS, TC>(fullServer, fullClient, serverEncrypted, clientEncrypted);
            return retMes;
        }
        
        


        /// <summary>
        /// Send_CqrSrvMsg sends registration msg to server
        /// </summary>
        /// <param name="msg">string message</param>
        /// <param name="srvIp">public availible server ip address</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <returns></returns>
        public string Send_CqrSrvMsg(string msg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {
            string encrypted = string.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                CqrBaseMsg(msg));

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }


        /// <summary>
        /// Send_CqrSrvMsgT sends a generic msg to server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullServerMsg"></param>
        /// <param name="srvIp">public availible server ip address</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <returns></returns>
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



        /// <summary>
        /// Send_CqrSrvMsgTTC sends a fullServerMsg and fulk
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TC"></typeparam>
        /// <param name="fullServerMsg"></param>
        /// <param name="fullClientMsg"></param>
        /// <param name="srvIp">public availible server ip address</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <returns></returns>
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


        //public string Send_CqrSrvMsg_Soap<T, TC>(FullSrvMsg<T> fullServerMsg, FullSrvMsg<TC> fullClientMsg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64) 
        //    where T : class
        //    where TC : class
        //{
        //    string cryptSrv = CqrSrvMsg<T>(fullServerMsg);
        //    string cryptPatner = CqrSrvMsg<TC>(fullClientMsg);
        //    string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
        //    string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();


        //    Area23.At.Framework.Core.CqrXs.SrvStub.CqrServiceSoapClient client = new CqrServiceSoapClient(CqrServiceSoapClient.EndpointConfiguration.CqrServiceSoap12);
        //    SendSrvMsgRequest req = new SendSrvMsgRequest(cryptSrv, cryptPatner);
        //    SendSrvMsgResponse resp = client.SendSrvMsg(req);

        //    return resp.SendSrvMsgResult;                        
             
        //}

    }

}
