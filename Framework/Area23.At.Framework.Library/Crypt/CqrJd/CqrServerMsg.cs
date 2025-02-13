using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.CqrJd;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Net.WebHttp;
using Area23.At.Framework.Library.Util;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Net;
using System.Runtime.Serialization;

namespace Area23.At.Framework.Library.Net.CqrJd
{


    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    [DataContract(Name = "CqrServerMsg")]
    [Description("cqrxs.eu cqrservermsg")]
    public class CqrServerMsg : CqrBaseMsg
    {

        /// <summary>
        /// CqrServerMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public CqrServerMsg(string srvKey = "") : base(srvKey) { }


        /// <summary>
        /// CqrSrvMsg encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrSrvMsg(string msg, EncodingType encType = EncodingType.Base64)
        {
            return CqrMsg(msg, encType);
        }

        /// <summary>
        /// CqrServerAttachment encrypts a file attchment message
        /// </summary>
        /// <param name="fileName">file name of attached file</param>
        /// <param name="mimeType"><see cref="Util.MimeType"/></param>
        /// <param name="base64Mime">base64 encoded mime block</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted attachment msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrSrvAttachment(string fileName, string mimeType, string base64Mime, out MimeAttachment attachment,
            EncodingType encType = EncodingType.Base64, string sMd5 = "", string sSha256 = "")
        {
            return CqrMsgAttachment(fileName, mimeType, base64Mime, out attachment, encType, sMd5, sSha256);
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
            MsgContent msgContent = NCqrMsg(cqrMessage, encType);
            return msgContent;
        }


        /// <summary>
        /// SendCqrServerMsg sends registration msg to server
        /// </summary>
        /// <param name="msg">string message</param>
        /// <param name="srvIp">public availible server ip address</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <returns></returns>
        public string SendCqrSrvMsg(string msg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {
            string encrypted = String.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                CqrSrvMsg(msg));

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }

        /// <summary>
        /// SendCqrPeerAttachment sends an attached base64 encoded file
        /// </summary>
        /// <param name="fileName">file name of attached file</param>
        /// <param name="mimeType"><see cref="Util.MimeType"/></param>
        /// <param name="base64Mime">base64 encoded mime block</param>
        /// <param name="peerIp">peer partner ip address</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <param name="serverPort">tcp server port</param>
        /// <returns>response string</returns>
        public string SendCqrServerAttachment(string fileName, string mimeType, string base64Mime, IPAddress srvIp, out MimeAttachment mimeAttachment,
            EncodingType encodingType = EncodingType.Base64, int serverPort = 7777, string sMd5 = "", string sSha256 = "")
        {

            string encrypted = String.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                CqrSrvAttachment(fileName, mimeType, base64Mime, out mimeAttachment, encodingType, sMd5, sSha256));

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }

    }

}
