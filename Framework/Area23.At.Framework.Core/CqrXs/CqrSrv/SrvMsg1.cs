using Area23.At.Framework.Core.Crypt;
using Area23.At.Framework.Core.CqrXs.CqrMsg;
using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Net.WebHttp;
using Area23.At.Framework.Core.Util;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.Security.Policy;
using Area23.At.Framework.Core.Crypt.Hash;

namespace Area23.At.Framework.Core.CqrXs.CqrSrv
{

    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class SrvMsg1 : BaseMsg
    {


        /// <summary>
        /// Cqr1stServerMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public SrvMsg1(string srvKey = "") : base(srvKey) { }



        /// <summary>
        /// CqrSrvMsg1 encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        /// <exception cref="InvalidDataException"></exception>
        public string CqrSrvMsg1(CqrContact myContact, EncodingType encType)
        {
            string msg = JsonConvert.SerializeObject(myContact);
            return CqrBaseMsg(msg, encType);
        }


        /// <summary>
        /// CqrAttachmentSrvMsg1 encrypts the picture of user as attchment
        /// </summary>
        /// <param name="myImage"><see cref="CqrImage"/></param>        
        /// <param name="attachment">out <see cref="MimeAttachment"/></param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted attachment msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrAttachmentSrvMsg1(CqrImage myImage, out MimeAttachment attachment, EncodingType encType = EncodingType.Base64)
        {

            string md5 = MD5Sum.Hash(myImage.ImageData, myImage.ImageFileName);
            string sha256 = Sha256Sum.Hash(myImage.ImageData, myImage.ImageFileName);
            MimeAttachment mimeAttachment = new MimeAttachment(myImage.ImageFileName, myImage.ImageMimeType, myImage.ImageBase64, "", md5, sha256);

            return CqrBaseAttachment(myImage.ImageFileName, myImage.ImageMimeType, myImage.ImageBase64, out attachment, encType, md5, sha256);
        }



        /// <summary>
        /// NCqrSrvMsg1 decryptes an secure encrypted msg 
        /// </summary>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns><see cref="CqrContact"/>CqrContact decrypted string</returns>
        /// <exception cref="InvalidOperationException">will be thrown, 
        /// if server and client or both side use a different secret key 4 encryption</exception>
        public CqrContact? NCqrSrvMsg1(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            CqrContact? myContact = null;
            MsgContent msgContent = base.NCqrBaseMsg(cqrMessage, encType);
            if (msgContent != null && !string.IsNullOrEmpty(msgContent.Message))
                myContact = JsonConvert.DeserializeObject<CqrContact>(msgContent.Message);

            return myContact;
        }



        /// <summary>
        /// Send1st_CqrSrvMsg1 sends registration msg to server
        /// </summary>
        /// <param name="myContact"><see cref="CqrContact"/></param>
        /// <param name="srvIp">public availible server ip address</param>
        /// <param name="encodingType"><+
        /// see cref="EncodingType"/></param>
        /// <returns></returns>
        public string Send1st_CqrSrvMsg1(CqrContact myContact, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {
            string msg = JsonConvert.SerializeObject(myContact);
            string encMsg = CqrBaseMsg(msg, encodingType);
            string encrypted = string.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                encMsg);

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();


            if (srvIp == null || string.IsNullOrEmpty(srvIp.ToString()))
            {
                srvIp = IPAddress.Parse(ConfigurationManager.AppSettings["ServerIPv4"].ToString());
            }

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }



    }

}
