using Area23.At.Framework.Core.CqrXs.CqrMsg;
using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Net.WebHttp;
using Area23.At.Framework.Core.Util;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.Security.Policy;
using System.Text.Json.Serialization;

namespace Area23.At.Framework.Core.CqrXs.CqrSrv
{


    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class SrvMsg : BaseMsg
    {

        protected internal CqrContact CqrSender { get; private set; }
        protected internal CqrContact CqrRecipient { get; private set; }


        /// <summary>
        /// CqrServerMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public SrvMsg(string srvKey = "") : base(srvKey) { }


        /// <summary>
        /// ctor with sender & recipient
        /// </summary>
        /// <param name="sender"><see cref="CqrContact">CqrContact sender</see></param>
        /// <param name="receipient"><see cref="CqrContact">CqrContact receipient</see></param>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        public SrvMsg(CqrContact sender, CqrContact receipient, string srvKey = "") : base(srvKey)
        {
            CqrSender = sender;
            CqrRecipient = receipient;
        }



        /// <summary>
        /// CqrSrvMsg encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        /// <exception cref="InvalidDataException"></exception>
        public string CqrSrvMsg(string msg, EncodingType encType = EncodingType.Base64)
        {
            if (CqrSender == null || CqrRecipient == null)
                throw new InvalidDataException("CqrSender contact or CqrRecipient conact are null.");

            return CqrBaseMsg(msg, encType);
        }

        /// <summary>
        /// CqrServerAttachment encrypts a file attchment message
        /// </summary>
        /// <param name="fileName">file name of attached file</param>
        /// <param name="mimeType"><see cref="MimeType"/></param>
        /// <param name="base64Mime">base64 encoded mime block</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted attachment msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrSrvAttachment(string fileName, string mimeType, string base64Mime, out MimeAttachment attachment,
            EncodingType encType = EncodingType.Base64, string sMd5 = "", string sSha256 = "")
        {
            if (CqrSender == null || CqrRecipient == null)
                throw new InvalidDataException("CqrSender contact or CqrRecipient conact are null.");

            return base.CqrBaseAttachment(fileName, mimeType, base64Mime, out attachment, encType, sMd5, sSha256);
        }


        /// <summary>
        /// CqrSrvMsg
        /// </summary>
        /// <param name="sender"><see cref="CqrContact"/></param>
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
            string allMsg = CqrSender.ToJson() + "\r\n" + CqrRecipient.ToJson() + "\r\n";

            msg = msg + "\n" + symmPipe.PipeString + "\0";
            byte[] headerBytes = DeEnCoder.GetBytesFromString(allMsg);
            byte[] msgBytes = DeEnCoder.GetBytesFromString(msg);
            byte[] cqrMsgBytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;
            byte[] allMsgBytes = headerBytes.TarBytes(cqrMsgBytes);
            CqrMessage = DeEnCoder.EncodeBytes(allMsgBytes, encType);

            return CqrMessage;
        }

        /// <summary>
        /// CqrServerAttachment encrypts a file attchment message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receipient"></param>
        ///  <param name="fileName">file name of attached file</param>
        /// <param name="mimeType"><see cref="MimeType"/></param>
        /// <param name="base64Mime">base64 encoded mime block</param>
        /// <param name="attachment">out <see cref="MimeAttachment"/></param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <param name="sMd5"><see cref="Hash.MD5Sum"/></param>
        /// <param name="sSha256"><see cref="Hash.Sha256Sum"/></param>
        /// <returns>encrypted attachment msg via <see cref="SymmCipherPipe"/></returns>
        /// <exception cref="InvalidDataException"></exception>
        public string CqrSrvAttachment(CqrContact sender, CqrContact receipient, string fileName, string mimeType, string base64Mime,
            out MimeAttachment attachment, EncodingType encType = EncodingType.Base64, string sMd5 = "", string sSha256 = "")
        {
            if (sender == null || receipient == null)
                throw new InvalidDataException("CqrSender contact or CqrRecipient conact are null.");

            CqrSender = sender;
            CqrRecipient = receipient;
            string allMsg = CqrSender.ToJson() + "\r\n" + CqrRecipient.ToJson() + "\r\n";

            attachment = new MimeAttachment(fileName, mimeType, base64Mime, symmPipe.PipeString, sMd5, sSha256);
            string mimeMsg = attachment.MimeMsg;
            mimeMsg += "\n" + symmPipe.PipeString + "\0";
            byte[] headerBytes = DeEnCoder.GetBytesFromString(allMsg);
            byte[] msgBytes = DeEnCoder.GetBytesFromString(mimeMsg);
            byte[] cqrMsgBytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;

            byte[] allMsgBytes = headerBytes.TarBytes(cqrMsgBytes);
            CqrMessage = DeEnCoder.EncodeBytes(allMsgBytes, encType);

            return CqrMessage;
        }


        /// <summary>
        /// NCqrSrvMsg decryptes an secure encrypted msg 
        /// </summary>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns><see cref="MsgContent"/> Message plain text decrypted string</returns>
        /// <exception cref="InvalidOperationException">will be thrown, 
        /// if server and client or both side use a different secret key 4 encryption</exception>
        public MsgContent NCqrSrvMsg(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            MsgContent msgContent = base.NCqrBaseMsg(cqrMessage, encType);
            return msgContent;
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
                CqrSrvMsg(msg));

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }


        /// <summary>
        /// Send_CqrSrvAttachment sends an attached base64 encoded file
        /// </summary>
        /// <param name="fileName">file name of attached file</param>
        /// <param name="mimeType"><see cref="MimeType"/></param>
        /// <param name="base64Mime">base64 encoded mime block</param>
        /// <param name="peerIp">peer partner ip address</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <param name="serverPort">tcp server port</param>
        /// <returns>response string</returns>
        public string Send_CqrSrvAttachment(string fileName, string mimeType, string base64Mime, IPAddress srvIp, out MimeAttachment mimeAttachment,
            EncodingType encodingType = EncodingType.Base64, int serverPort = 7777, string sMd5 = "", string sSha256 = "")
        {
            string encrypted = string.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                CqrSrvAttachment(fileName, mimeType, base64Mime, out mimeAttachment, encodingType, sMd5, sSha256));

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }

    }

}
