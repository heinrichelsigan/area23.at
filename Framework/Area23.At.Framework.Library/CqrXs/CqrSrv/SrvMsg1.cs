using Area23.At.Framework.Library.CqrXs.CqrMsg;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Net.WebHttp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.CqrXs.CqrSrv
{


    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    [DataContract(Name = "SrvMsg1")]
    public class SrvMsg1 : BaseMsg
    {


        /// <summary>
        /// SrvMsg1 constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public SrvMsg1(string srvKey = "") : base(srvKey) { }



        /// <summary>
        /// CqrSrvMsg1 encrypts a contact
        /// </summary>
        /// <param name="myContact">my contact in plain text string</param>
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
        /// <param name="myContact"><see cref="CqrContact"/></param>        
        /// <param name="attachment">out <see cref="MimeAttachment"/></param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted attachment msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrAttachmentSrvMsg1(CqrContact myContact, out MimeAttachment attachment, EncodingType encType = EncodingType.Base64)
        {
            CqrImage myImage = myContact.ContactImage;
            string md5 = Area23.At.Framework.Library.Crypt.Hash.MD5Sum.Hash(myImage.ImageData, myImage.ImageFileName);
            string sha256 = Area23.At.Framework.Library.Crypt.Hash.Sha256Sum.Hash(myImage.ImageData, myImage.ImageFileName);
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
        public CqrContact NCqrSrvMsg1(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            CqrContact myContact = null;
            MsgContent msgContent = base.NCqrBaseMsg(cqrMessage, encType);
            if (msgContent != null && !string.IsNullOrEmpty(msgContent.Message))
                myContact = JsonConvert.DeserializeObject<CqrContact>(msgContent.Message);
            
            return myContact;
        }


        /// <summary>
        /// Send1stCqrSrvMsg sends registration msg to server
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
            string encrypted = String.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                encMsg);

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }


    }

}
