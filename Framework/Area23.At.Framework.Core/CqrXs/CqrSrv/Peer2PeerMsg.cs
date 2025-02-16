using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Net.WebHttp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Area23.At.Framework.Core.Net.IpSocket;
using System.Net;
using Area23.At.Framework.Core.CqrXs.CqrMsg;

namespace Area23.At.Framework.Core.CqrXs.CqrSrv
{


    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class Peer2PeerMsg : BaseMsg
    {

        /// <summary>
        /// CqrServerMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public Peer2PeerMsg(string srvKey = "") : base(srvKey) { }


        /// <summary>
        /// CqrPeerMsg encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrPeerMsg(string msg, EncodingType encType = EncodingType.Base64)
        {
            return CqrBaseMsg(msg, encType);
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
            return CqrBaseAttachment(fileName, mimeType, base64Mime, out attachment, encType, sMd5, sSha256);
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
            MsgContent msgContent = base.NCqrBaseMsg(cqrMessage, encType);
            return msgContent;
        }


        public MsgContent NCqrSrvMsg(MsgContent msgInContent, EncodingType encType = EncodingType.Base64)
        {
            MsgContent msgOutContent = NCqrBaseMsg(msgInContent.Message, encType);
            return msgOutContent;
        }


        /// <summary>
        /// Send_CqrPeerMsg
        /// </summary>
        /// <param name="msg">message to send</param>
        /// <param name="peerIp">peer partner ip address</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <param name="serverPort">tcp server port</param>
        /// <returns>response string</returns>
        public string Send_CqrPeerMsg(string msg, IPAddress peerIp, EncodingType encodingType = EncodingType.Base64, int serverPort = 7777)
        {
            string encrypted = CqrPeerMsg(msg, encodingType);
            string response = Sender.Send(peerIp, encrypted, Constants.CHAT_PORT);
            return response;
        }

        /// <summary>
        /// Send_CqrPeerAttachment sends an attached base64 encoded file
        /// </summary>
        /// <param name="fileName">file name of attached file</param>
        /// <param name="mimeType"><see cref="Util.MimeType"/></param>
        /// <param name="base64Mime">base64 encoded mime block</param>
        /// <param name="peerIp">peer partner ip address</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <param name="serverPort">tcp server port</param>
        /// <returns>response string</returns>
        public string Send_CqrPeerAttachment(string fileName, string mimeType, string base64Mime, IPAddress peerIp, out MimeAttachment attachment,
            EncodingType encodingType = EncodingType.Base64, int serverPort = 7777, string sMd5 = "", string sSha256 = "")
        {
            string encrypted = CqrPeerAttachment(fileName, mimeType, base64Mime, out attachment, encodingType, sMd5, sSha256);
            string response = Sender.Send(peerIp, encrypted, Constants.CHAT_PORT);
            return response;
        }


    }

}
