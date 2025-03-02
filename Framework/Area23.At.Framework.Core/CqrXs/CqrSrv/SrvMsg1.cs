using Area23.At.Framework.Core.Crypt;
using Area23.At.Framework.Core.CqrXs.CqrMsg;
using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Net.WebHttp;
using Area23.At.Framework.Core.Static;
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
using EU.CqrXs.CqrSrv.CqrJd;

namespace Area23.At.Framework.Core.CqrXs.CqrSrv
{


    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>    
    public class SrvMsg1 : BaseMsg
    {
        protected internal CqrContact MsgContact { get; set; }

        public SrvMsg1() { }

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
            MsgContact = new CqrContact(myContact, PipeString);
            string allMsg = MsgContact.ToJson();
            MsgContact._message = allMsg;
            MsgContact.RawMessage = allMsg + "\n" + symmPipe.PipeString + "\0";

            byte[] allBytes = EnDeCodeHelper.GetBytesFromString(allMsg);
            byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(MsgContact._message);
            byte[] cqrMsgBytes = (LibPaths.CqrEncrypt) ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;
            CqrMessage = EnDeCodeHelper.EncodeBytes(cqrMsgBytes, encType);

            return CqrBaseMsg(MsgContact, encType);
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

            MsgContact = myContact;
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
        [Obsolete("Please use Send1st_CqrSrvMsg1_Soap(..)", true)]
        public string Send1st_CqrSrvMsg1(CqrContact myContact, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {
            MsgContact = myContact;
            string msg = JsonConvert.SerializeObject(myContact);
            string encMsg = CqrBaseMsg(msg, encodingType);
            string encrypted = String.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                encMsg);

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = string.Empty;
            try
            {
                response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());
            }
            catch (Exception ex)
            {
                response = "Exception: " + ex.Message + "\n" + ex.ToString();
            }

            string reducedResponse = string.Empty;
            if (response.Contains(Constants.DECRYPTED_TEXT_AREA))
                reducedResponse = response.GetSubStringByPattern(Constants.DECRYPTED_TEXT_AREA, true, "",
                    Constants.DECRYPTED_TEXT_AREA_END, false, StringComparison.InvariantCulture);
            else if (response.Contains(Constants.DECRYPTED_TEXT_BOX))
                reducedResponse = response.GetSubStringByPattern(Constants.DECRYPTED_TEXT_BOX, true, ">",
                    Constants.DECRYPTED_TEXT_AREA_END, false, StringComparison.InvariantCulture);
            return reducedResponse;
        }


        public string Send1st_CqrSrvMsg1_Soap(CqrContact myContact, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {

            myContact._hash = PipeString;
            CqrContact sendContact = new CqrContact(myContact.ContactId, myContact.Name, myContact.Email, myContact.Mobile, myContact.Address);
            sendContact._hash = PipeString;

            string msg = Newtonsoft.Json.JsonConvert.SerializeObject(sendContact);
            // string encMsg = CqrBaseMsg(msg, encodingType);
            string encMsg = CqrSrvMsg1(sendContact, encodingType);
            // string encrypted = String.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
            //     encMsg);

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            CqrServiceSoapClient client = new CqrServiceSoapClient(CqrServiceSoapClient.EndpointConfiguration.CqrServiceSoap12);

            string response = client.Send1StSrvMsg(encMsg);


            //    string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;

        }


        public CqrContact SendFirstSrvMsg(CqrContact myContact, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {

            myContact._hash = PipeString;
            CqrContact sendContact = new CqrContact(myContact.ContactId, myContact.Name, myContact.Email, myContact.Mobile, myContact.Address);
            sendContact._hash = PipeString;

            string msg = Newtonsoft.Json.JsonConvert.SerializeObject(sendContact);
            string encMsg = CqrSrvMsg1(sendContact, encodingType);

            CqrServiceSoapClient client = new CqrServiceSoapClient(CqrServiceSoapClient.EndpointConfiguration.CqrServiceSoap12);
            string response = client.Send1StSrvMsg(encMsg);

            CqrContact? returnedContact = null;
            returnedContact = NCqrSrvMsg1(response, EncodingType.Base64);

            if (returnedContact != null)
                return returnedContact;

            var content = NCqrBaseMsg(response, EncodingType.Base64);
            
            returnedContact = JsonConvert.DeserializeObject<CqrContact>(content.Message);


            return returnedContact;

        }


    }

}
