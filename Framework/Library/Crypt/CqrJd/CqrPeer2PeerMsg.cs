using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Net.IpSocket;
using Area23.At.Framework.Library.Net.WebHttp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Area23.At.Framework.Library.Crypt.CqrJd
{


    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class CqrPeer2PeerMsg : CqrBaseMsg
    {

        /// <summary>
        /// CqrServerMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public CqrPeer2PeerMsg(string srvKey = "") : base(srvKey) { }


        /// <summary>
        /// CqrPeerMsg encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrPeerMsg(string msg, EncodingType encType = EncodingType.Base64)
        {
            return CqrMsg(msg, encType);
        }

        /// <summary>
        /// CqrPeerAttachment encrypts a file attchment message
        /// </summary>
        /// <param name="fileName">file name of attached file</param>
        /// <param name="mimeType"><see cref="Util.MimeType"/></param>
        /// <param name="base64Mime">base64 encoded mime block</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted attachment msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrPeerAttachment(string fileName, string mimeType, string base64Mime, out MimeAttachment attachment,
            EncodingType encType = EncodingType.Base64, string sMd5 = "", string sSha256 = "")
        {
            return CqrMsgAttachment(fileName, mimeType, base64Mime, out attachment, encType, sMd5, sSha256);
        }

        /// <summary>
        /// NCqrPeerMsg decryptes an secure encrypted msg 
        /// </summary>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns><see cref="MsgContent"/> Message plain text decrypted string</returns>
        /// <exception cref="InvalidOperationException">will be thrown, 
        /// if server and client or both side use a different secret key 4 encryption</exception>
        public MsgContent NCqrPeerMsg(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            MsgContent msgContent = NCqrMsg(cqrMessage, encType);
            return msgContent;
        }


        public MsgContent NCqrSrvMsg(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            MsgContent msgContent = NCqrMsg(cqrMessage, encType);
            return msgContent;

        }

        /// <summary>
        /// SendCqrPeerMsg
        /// </summary>
        /// <param name="msg">message to send</param>
        /// <param name="peerIp">peer partner ip address</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <param name="serverPort">tcp server port</param>
        /// <returns>response string</returns>
        public string SendCqrPeerMsg(string msg, IPAddress peerIp, EncodingType encodingType = EncodingType.Base64, int serverPort = 7777)
        {
            string encrypted = CqrPeerMsg(msg, encodingType);
            string response = Sender.Send(peerIp, encrypted, Constants.CHAT_PORT);
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
        public string SendCqrPeerAttachment(string fileName, string mimeType, string base64Mime, IPAddress peerIp, out MimeAttachment attachment,
            EncodingType encodingType = EncodingType.Base64, int serverPort = 7777, string sMd5 = "", string sSha256 = "")
        {
            string encrypted = CqrPeerAttachment(fileName, mimeType, base64Mime, out attachment, encodingType, sMd5, sSha256);
            string response = Sender.Send(peerIp, encrypted, Constants.CHAT_PORT);
            return response;
        }


    }

}
