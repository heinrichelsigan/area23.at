using Area23.At.Framework.Library.Cqr.Msg;
using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Net.IpSocket;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Cqr
{
   
    /// <summary>
    /// WebService Soap and Peer-2-Peer tcp socket send facade 
    /// </summary>
    public class CqrFacade
    {

        private readonly string _key;
        private readonly string _hash;
        private readonly byte[] _keyBytes;
        private readonly SymmCipherPipe _symmPipe;
        public string PipeString { get => _symmPipe.PipeString; }


        /// <summary>
        /// CqrFacade ctor only with server key
        /// </summary>
        /// <param name="key"><see cref="string"/> server key</param>
        /// <exception cref="ArgumentNullException">thrown when key is null or string.Empty</exception>
        public CqrFacade(string key = "")
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("public CqrFacade(string key = \"\")");
            
            _key = key;
            _hash = EnDeCodeHelper.KeyToHex(_key);
            _keyBytes = CryptHelper.GetUserKeyBytes(_key, _hash, 16);
            _symmPipe = new SymmCipherPipe(_keyBytes, 8);
        }

        /// <summary>
        /// Send_CqrPeerMsg, sends a plain-text message to peer 2 peer partner
        /// </summary>
        /// <param name="msg">message to send</param>
        /// <param name="peerIp">peer partner ip address</param>
        /// <param name="serverPort">tcp server port</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <returns>response string</returns>
        public string Send_CContent_Peer(string msg, IPAddress peerIp, int serverPort = 7777, EncodingType encodingType = EncodingType.Base64)
        {
            CContent content = new CContent(msg, _symmPipe.PipeString, Msg.SerType.Json, MD5Sum.HashString(msg, ""));
            string encrypted = content.EncryptToJson(_key);

            string response = Sender.Send(peerIp, encrypted, Constants.CHAT_PORT);
            return response;
        }

        /// <summary>
        /// Send_Peer_CFile, sends an attached file to peer 2 peer partner
        /// </summary>
        /// <param name="cFile">Entity, that represents the file, data are stored as byte[]</param>
        /// <param name="peerIp"><see cref="IPAddress"/ for the partner></param>
        /// <param name="serverPort">server port, default <see cref="Constants.CHAT_PORT"/></param>
        /// <param name="msgType"><see cref="MsgEnum">msgType</see> default with <see cref="MsgEnum.Json"/></param>
        /// <param name="encType"><see cref="EncodingType"/> default to <see cref="EncodingType.Base64"/></param>
        /// <returns></returns>
        public string Send_CFile_Peer(CFile cFile, IPAddress peerIp, int serverPort = 7777, SerType msgType = SerType.Json, EncodingType encType = EncodingType.Base64)
        {
            cFile.Hash = PipeString;
            cFile.MsgType = SerType.Json;
            string encrypted = cFile.EncryptToJson(_key);

            string response = Sender.Send(peerIp, encrypted, Constants.CHAT_PORT);
            return response;
        }

        #region CqrContact SendFirstSrvMsg_Soap(CqrContact myContact, IPAddress srvIp, ..)



        /// <summary>
        /// Test_Send1st_CqrSrvMsg1_Soap test method only, please use <see cref="SendFirstSrvMsg_Soap(CqrContact, IPAddress, EncodingType)"/>
        /// </summary>
        /// <param name="myContact"></param>
        /// <param name="encodingType"></param>
        /// <returns>CContact</returns>
        public CContact Test_Send1st_CqrSrvMsg1_Soap(CContact myContact, EncodingType encodingType = EncodingType.Base64)
        {

            myContact.Hash = PipeString;
            CContact sendContact = new CContact(myContact.ContactId, myContact.Name, myContact.Email, myContact.Mobile, myContact.Address);
            sendContact.Hash = PipeString;

            string encMsg = sendContact.EncryptToJson(_key);
            
            CqrService webService = new CqrService();
            string response = webService.Send1StSrvMsg(encMsg);


            CContact tmpContact = new CContact();
            CContact responseContact = tmpContact.DecryptFromJson(_key, response);

            return responseContact;
        }


        /// <summary>
        /// SendFirstSrvMsg_Soap, real soap method for 1st registration
        /// </summary>
        /// <param name="myContact">my contact</param>
        /// <param name="encodingType"></param>
        /// <returns>my Contact with Guid Cuid</returns>
        public CContact SendFirstSrvMsg_Soap(CContact myContact, EncodingType encodingType = EncodingType.Base64)
        {

            myContact.Hash = PipeString;
            CContact sendContact = new CContact(myContact.ContactId, myContact.Name, myContact.Email, myContact.Mobile, myContact.Address);
            sendContact.Hash = PipeString;

            string encMsg = sendContact.EncryptToJson(_key);

            CqrService webService = new CqrService();
            string response = webService.Send1StSrvMsg(encMsg);

            CContact tmpContact = new CContact();
            CContact responseContact = tmpContact.DecryptFromJson(_key, response);
            
            return responseContact;
        }

        #endregion CqrContact SendFirstSrvMsg_Soap(CqrContact myContact, IPAddress srvIp, ..)

        #region Response<T> response = webServiceSoapClient.WebMethod_To_Invoke(Request<T> request)

        /// <summary>
        /// Send_InitChatRoom_Soap{<typeparamref name="T"/>} Sends an chat roomm invitation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cServerMsg"><see cref="CSrvMsg{T}"/>, containing char room number, sender and recipients</param>
        /// <param name="encodingType"><see cref="EncodingType"/> default to <see cref="EncodingType.Base64"/></param>
        /// <returns><see cref="CSrvMsg{string}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public CSrvMsg<string> Send_InitChatRoom_Soap<T>(CSrvMsg<T> cServerMsg, EncodingType encodingType = EncodingType.Base64)
            where T : class
        {
            cServerMsg.Hash = _symmPipe.PipeString;
            string cryptSrvString = cServerMsg.EncryptToJson(_key);

            CqrService webService = new CqrService();

            string response = string.Empty;
            try
            {
                response = webService.ChatRoomInvite(cryptSrvString);
            }
            catch (Exception exSoap)
            {
                Area23Log.LogOriginMsgEx("CqrFacade", $"Exception {exSoap.GetType()}: {exSoap.Message}", exSoap);
                throw;
            }

            CSrvMsg<string> respTmpMsg = new CSrvMsg<string>();
            CSrvMsg<string> responseMsg = respTmpMsg.DecryptFromJson(_key, response);


            return responseMsg;
        }


        /// <summary>
        /// SendChatMsg_Soap
        /// </summary>
        /// <param name="cServerMsg"><see cref="CSrvMsg{string}"/>, containing char room number, sender and recipients</param>
        /// <param name="cClientMsg">client encrypted messagem, that server can't decrypt, <see cref="CSrvMsg{string}"/></param>fullClientMsgfullClientMsg
        /// <param name="clientKey">clientKey for partner msg encryption</param>
        /// <param name="encodingType"><see cref="EncodingType"/> default to <see cref="EncodingType.Base64"/></param>
        /// <returns><see cref="CSrvMsg{string}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public CSrvMsg<string> SendChatMsg_Soap(CSrvMsg<string> cServerMsg, CSrvMsg<string> cClientMsg, string clientKey = "", EncodingType encodingType = EncodingType.Base64)
        {
            SymmCipherPipe clientPipe = new SymmCipherPipe(clientKey);
            cClientMsg.Hash = clientPipe.PipeString;
            string cryptClientMsg = cClientMsg.EncryptToJson(clientKey);

            cServerMsg.TContent = cryptClientMsg;
            cServerMsg.Message = cryptClientMsg;

            cServerMsg.Hash = _symmPipe.PipeString;
            string cryptSrvMsg = cServerMsg.EncryptToJson(_key);
                        
            CqrService webService = new CqrService();
            string response = webService.ChatRoomPush(cryptSrvMsg);
            
            CSrvMsg<string> responseMsg = CSrvMsg<string>.Json2Decrypt(_key, response);

            return responseMsg;
        }


        /// <summary>
        /// SendChatMsg_Soap_Simple sends a simple push message to the server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullServerMsg"><see cref="CSrvMsg{string}"/>, containing char room number, sender and recipients</param>
        /// <param name="encryptedClientMsg">already encrypted client msg, that server can't read</param>
        /// <param name="srvIp"></param>
        /// <param name="encodingType"></param>
        /// <returns><see cref="CSrvMsg{string}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public CSrvMsg<string> SendChatMsg_Soap_Simple(CSrvMsg<string> cServerMsg, string encryptedClientMsg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {

            cServerMsg.Hash = _symmPipe.PipeString;
            cServerMsg.Message = encryptedClientMsg;
            cServerMsg.TContent = encryptedClientMsg;

            string cryptSrvMsg = cServerMsg.EncryptToJson(_key);

            CqrService webService = new CqrService();
            string response = webService.ChatRoomPush(cryptSrvMsg);

            CSrvMsg<string> responseMsg = CSrvMsg<string>.Json2Decrypt(_key, response);
            
            return responseMsg;
        }


        /// <summary>
        /// ReceiveChatMsg_Soap{<typeparamref name="T"/>} is a polling chat server request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cServerMsg"><see cref="CSrvMsg{T}"/>, containing char room number, sender and recipients</param>
        /// <param name="srvIp"></param>
        /// <param name="encodingType"></param>
        /// <returns><see cref="CSrvMsg{string}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public CSrvMsg<string> ReceiveChatMsg_Soap<T>(CSrvMsg<T> cServerMsg, EncodingType encodingType = EncodingType.Base64)
        where T : class
        {
            cServerMsg.Hash = _symmPipe.PipeString;
            string cryptSrvMsg = cServerMsg.EncryptToJson(_key);

            CqrService webService = new CqrService();
            string response = webService.ChatRoomPoll(cryptSrvMsg);

            CSrvMsg<string> responseTmpMsg = new CSrvMsg<string>();
            CSrvMsg<string> responseMsg = responseTmpMsg.DecryptFromJson(_key, response);

            return responseMsg;

        }


        #endregion Response<T> response = webServiceSoapClient.WebMethod_To_Invoke(Request<T> request)


        #region async calls

        /// <summary>
        /// Send_InitChatRoom_SoapAsync{<typeparamref name="T"/>} Sends async an chat roomm invitation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cServerMsg"><see cref="CSrvMsg{T}"/>, containing char room number, sender and recipients</param>
        /// <param name="encodingType"></param>
        /// <returns><see cref="Task{CSrvMsg{string}}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public async Task<CSrvMsg<string>> Send_InitChatRoom_SoapAsync<T>(CSrvMsg<T> cServerMsg, EncodingType encodingType = EncodingType.Base64)
            where T : class
        {
            cServerMsg.Hash = _symmPipe.PipeString;
            string cryptSrvString = cServerMsg.EncryptToJson(_key);

            CqrService webService = new CqrService();

            string response = string.Empty;
            try
            {
                response = await Task.Run(() => webService.ChatRoomInvite(cryptSrvString));
            }
            catch (Exception exSoap)
            {
                Area23Log.LogOriginMsgEx("CqrFacade", $"Exception {exSoap.GetType()}: {exSoap.Message}", exSoap);
                throw;
            }

            CSrvMsg<string> respTmpMsg = new CSrvMsg<string>();
            CSrvMsg<string> responseMsg = respTmpMsg.DecryptFromJson(_key, response);


            return responseMsg;
        }


        public async Task<CSrvMsg<string>> Send_CloseChatRoom_SoapAsync<T>(CSrvMsg<T> cServerMsg, string chatRoomNr, EncodingType encodingType = EncodingType.Base64) where T : class
        {
            cServerMsg.Hash = _symmPipe.PipeString;
            cServerMsg.CRoom = new CChatRoom(chatRoomNr, Guid.NewGuid(), DateTime.MinValue, DateTime.MinValue);

            string cryptSrvString = cServerMsg.EncryptToJson(_key);

            CqrService webService = new CqrService();

            string response = string.Empty;
            try
            {
                response = await Task.Run(() => webService.ChatRoomClose(cryptSrvString));
            }
            catch (Exception exSoap)
            {
                Area23Log.LogOriginMsgEx("CqrFacade", $"Exception {exSoap.GetType()}: {exSoap.Message}", exSoap);
                throw;
            }

            CSrvMsg<string> respTmpMsg = new CSrvMsg<string>();
            CSrvMsg<string> responseMsg = respTmpMsg.DecryptFromJson(_key, response);


            return responseMsg;
        }


        /// <summary>
        /// SendChatMsg_SoapAsync{<typeparamref name="T"/>, <typeparamref name="TC"/>} 
        /// </summary>
        /// <param name="cServerMsg"><see cref="CSrvMsg{T}"/>, containing char room number, sender and recipients</param>
        /// <param name="cClientMsg">client encrypted messagem, that server can't decrypt, <see cref="CSrvMsg{TC}"/></param>
        /// <param name="clientKey"></param>
        /// <param name="encodingType"></param>
        /// <returns><see cref="Task{CSrvMsg{string}?}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public async Task<CSrvMsg<string>> SendChatMsg_SoapAsync<T, TC>(CSrvMsg<T> cServerMsg, CSrvMsg<TC> cClientMsg, string clientKey = "", EncodingType encodingType = EncodingType.Base64)
            where T : class
            where TC : class
        {
            T t = default(T);
            // TC tc = default(TC);

            cServerMsg.Hash = _symmPipe.PipeString;
            SymmCipherPipe clientPipe = new SymmCipherPipe(clientKey);
            cClientMsg.Hash = clientPipe.PipeString;
            string cryptClientMsg = cClientMsg.EncryptToJson(clientKey);
            
            cServerMsg.Message = cryptClientMsg;
            if ((t is string ts) || typeof(T) == typeof(string))
            {
                ts = cryptClientMsg;
                cServerMsg.TContent = t;
            }
            
            string cryptSrvMsg = cServerMsg.EncryptToJson(_key);
            

            CqrService webService = new CqrService();

            string response = await Task.Run(() => webService.ChatRoomPush(cryptSrvMsg));

            CSrvMsg<string> responseTmpMsg = new CSrvMsg<string>();
            CSrvMsg<string> responseMsg = responseTmpMsg.DecryptFromJson(_key, response);

            return responseMsg;
        }


        /// <summary>
        /// SendChatMsg_Soap_Simple
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullServerMsg"><see cref="CSrvMsg{string}"/>, containing char room number, sender and recipients</param>
        /// <param name="encryptedClientMsg">already encrypted client msg, that server can't read</param>
        /// <param name="encodingType"></param>
        /// <returns><see cref="Task{CSrvMsg{string}}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public async Task<CSrvMsg<string>> SendChatMsg_Soap_SimpleAsync(CSrvMsg<string> cServerMsg, string encryptedClientMsg, EncodingType encodingType = EncodingType.Base64)
        {
            cServerMsg.Hash = _symmPipe.PipeString;
            cServerMsg.TContent = encryptedClientMsg;
            cServerMsg.Message = encryptedClientMsg; 

            string cryptSrvMsg = cServerMsg.EncryptToJson(_key);

            CqrService webService = new CqrService();

            string response = await Task.Run(() => webService.ChatRoomPush(cryptSrvMsg));

            
            CSrvMsg<string> responseMsg = CSrvMsg<string>.Json2Decrypt(_key, response);

            return responseMsg;
        }



        /// <summary>
        /// ReceiveChatMsg_SoapAsync{<typeparamref name="T"/>} async polling chat server request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullServerMsg"><see cref="CSrvMsg{T}"/>, containing char room number, sender and recipients</param>
        /// <param name="encodingType"></param>
        /// <returns><see cref="Task{CSrvMsg{string}}"/>, containing char room number, last polled date, updated sender and recipients</returns>
        public async Task<CSrvMsg<string>> ReceiveChatMsg_SoapAsync<T>(CSrvMsg<T> cServerMsg, EncodingType encodingType = EncodingType.Base64)
            where T : class
        {
            cServerMsg.Hash = _symmPipe.PipeString;
            string cryptSrvMsg = cServerMsg.EncryptToJson(_key);

            CqrService webService = new CqrService();
            string response = await Task.Run(() => webService.ChatRoomPoll(cryptSrvMsg));

            CSrvMsg<string> responseTmpMsg = new CSrvMsg<string>();
            CSrvMsg<string> responseMsg = responseTmpMsg.DecryptFromJson(_key, response);

            return responseMsg;

        }


        #endregion async calls

    }

}
