using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Net.WebHttp;
using Area23.At.Framework.Core.Util;
using System.Configuration;
using System.Net;
using System.Security.Policy;

namespace Area23.At.Framework.Core.Crypt.CqrJd
{


    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class CqrServerMsg : CqrBaseMsg
    {

        public CqrContact CqrSender { get; private set; }
        public CqrContact CqrRecipient { get; private set; }

        /// <summary>
        /// CqrServerMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public CqrServerMsg(string srvKey = "") : base(srvKey) { }


        public CqrServerMsg(CqrContact sender, CqrContact receipient, string srvKey = "") : base(srvKey)
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

            return CqrSrvMsg(CqrSender, CqrRecipient, msg, encType);
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
            byte[] cqrMsgBytes = (LibPaths.CqrEncrypt) ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;
            byte[] allMsgBytes = headerBytes.TarBytes(cqrMsgBytes);
            CqrMessage = DeEnCoder.EncodeBytes(allMsgBytes, encType);

            return CqrMessage;
        }

        /// <summary>
        /// CqrServerAttachment encrypts a file attchment message
        /// </summary>
        /// <param name="fileName">file name of attached file</param>
        /// <param name="mimeType"><see cref="Util.MimeType"/></param>
        /// <param name="attachment">out <see cref="MimeAttachment"/></param>
        /// <param name="base64Mime">base64 encoded mime block</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted attachment msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrSrvAttachment(string fileName, string mimeType, string base64Mime, out MimeAttachment attachment,
            EncodingType encType = EncodingType.Base64, string sMd5 = "", string sSha256 = "")
        {
            if (CqrSender == null || CqrRecipient == null)
                throw new InvalidDataException("CqrSender contact or CqrRecipient conact are null.");

            return CqrSrvAttachment(CqrSender, CqrRecipient, fileName, mimeType, base64Mime, out attachment, encType, sMd5, sSha256);
        }


        /// <summary>
        /// CqrServerAttachment encrypts a file attchment message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="receipient"></param>
        //  <param name="fileName">file name of attached file</param>
        /// <param name="mimeType"><see cref="Util.MimeType"/></param>
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
            byte[] cqrMsgBytes = (LibPaths.CqrEncrypt) ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;

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
            CqrMessage = cqrMessage.TrimEnd("\0".ToCharArray());
            byte[] allBytes = DeEnCoder.DecodeText(CqrMessage, encType);

            //byte[] cipherBytes = new byte[allBytes.Length];
            // Array.Copy(allBytes, cipherBytes, allBytes.Length);

            //byte[] unroundedMerryBytes = (LibPaths.CqrEncrypt) ? symmPipe.DecrpytRoundGoMerry(cipherBytes, key, hash) : cipherBytes;
            //string decrypted = EnDeCoder.GetString(unroundedMerryBytes); //DeEnCoder.GetStringFromBytesTrimNulls(unroundedMerryBytes);
            //while (decrypted[decrypted.Length - 1] == '\0')
            //    decrypted = decrypted.Substring(0, decrypted.Length - 1);

            MsgContent msgContent = new MsgContent(EnDeCoder.GetString(allBytes));
            // string hashVerification = msgContent.VerificationHash();
            //if (!VerifyHash(hashVerification))
            //{
            //    string hashSymShow = symmPipe.PipeString ?? "        ";
            //    throw new InvalidOperationException(
            //        string.Format("SymmCiphers [{0}] in crypt pipeline doesn't match serverside key !?$* byte length={1}.",
            //            hashSymShow.Substring(0, 2) + "...." + hashSymShow.Substring(6), keyBytes.Length));
            //}

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


        public string SendCqrSrvMsg(CqrContact sender, CqrContact recipient, string msg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {
            string encrypted = String.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                CqrSrvMsg(sender, recipient, msg));

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
