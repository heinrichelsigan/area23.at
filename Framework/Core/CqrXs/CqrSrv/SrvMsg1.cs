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
using System.Runtime.Serialization;
using System.Windows.Interop;
using static QRCoder.Core.PayloadGenerator;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Area23.At.Framework.Core.CqrXs.CqrSrv
{

    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class SrvMsg1 : BaseMsg
    {
        internal CqrContact? MsgContact { get; set; }

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
            myContact._hash = PipeString;
            MsgContact = myContact;
            MsgContact._hash = PipeString;
            MsgContact._message = Newtonsoft.Json.JsonConvert.SerializeObject(myContact, Formatting.None);
            MsgContact._rawMessage = MsgContact.Message + "\n" + PipeString + "\0";            
            return CqrBaseMsg(MsgContact, encType);
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
            MsgContact = myContact;
            CqrImage myImage = myContact.ContactImage;
            string md5 = Area23.At.Framework.Core.Crypt.Hash.MD5Sum.Hash(myImage.ImageData, myImage.ImageFileName);
            string sha256 = Area23.At.Framework.Core.Crypt.Hash.Sha256Sum.Hash(myImage.ImageData, myImage.ImageFileName);
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
            MsgContent msgContent = base.NCqrBaseMsg(cqrMessage, encType);
            if (msgContent != null && !string.IsNullOrEmpty(msgContent.Message))
                MsgContact = JsonConvert.DeserializeObject<CqrContact>(msgContent.Message);

            return MsgContact;
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
            myContact._hash = PipeString;
            
            // string msg = Newtonsoft.Json.JsonConvert.SerializeObject(myContact);
            // string encMsg = CqrBaseMsg(msg, encodingType);
            string encMsg = CqrSrvMsg1(myContact, encodingType);
            string encrypted = String.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                encMsg);

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }

        public string Send1st_CqrSrvMsg1(string serializedContact, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {            
            string encMsg = CqrBaseMsg(serializedContact, encodingType);
            string encrypted = String.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                encMsg);

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }


    }

}
