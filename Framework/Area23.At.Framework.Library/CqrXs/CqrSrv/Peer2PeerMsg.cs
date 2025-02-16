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
using Area23.At.Framework.Library.CqrXs.CqrMsg;
using Newtonsoft.Json;

namespace Area23.At.Framework.Library.CqrXs.CqrSrv
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

        public virtual string CqrPeerMsg(MsgContent msc, EncodingType encType = EncodingType.Base64)
        {
            return CqrBaseMsg(msc, encType);
        }

        public string CqrPeerMimeAttachment(MimeAttachment attachment, EncodingType encType = EncodingType.Base64)
        {
            attachment._hash = PipeString;
            attachment.Verification = PipeString;
            attachment._message = JsonConvert.SerializeObject(attachment);
            attachment._rawMessage = attachment.Message + "\n" + PipeString + "\0";
            return CqrBaseMsg(attachment, encType);
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
            string sMd5 = "", string sSha256 = "", MsgEnum msgType = MsgEnum.None, EncodingType encType = EncodingType.Base64)
        {
            attachment = new MimeAttachment(fileName, mimeType, base64Mime, symmPipe.PipeString, sMd5, sSha256);
            attachment.MsgType = msgType;
            string mimeMsg = string.Empty;
            if (msgType == MsgEnum.None || msgType == MsgEnum.RawWithHashAtEnd)
            {
                mimeMsg = attachment.MimeMsg;
                mimeMsg += "\n" + symmPipe.PipeString + "\0";
            }
            else
            {
                attachment._hash = PipeString;
                attachment.Verification = PipeString;
                mimeMsg = JsonConvert.SerializeObject(attachment);
            }
            byte[] msgBytes = DeEnCoder.GetBytesFromString(mimeMsg);

            byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;

            CqrMessage = DeEnCoder.EncodeBytes(cqrbytes, encType);

            return CqrMessage;
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
            MsgContent msgOutContent = NCqrBaseMsg(msgInContent.RawMessage, encType);
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
            int serverPort = 7777, string sMd5 = "", string sSha256 = "", MsgEnum msgType = MsgEnum.None, EncodingType encodingType = EncodingType.Base64)
        {
            string encrypted = CqrPeerAttachment(fileName, mimeType, base64Mime, out attachment, sMd5, sSha256, msgType, encodingType);
            attachment.MsgType = msgType;
            string response = Sender.Send(peerIp, encrypted, Constants.CHAT_PORT);
            return response;
        }


    }

}
