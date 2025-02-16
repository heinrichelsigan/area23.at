using Area23.At;
using Area23.At.Framework.Library;
using Area23.At.Framework.Library.CqrXs;
using Area23.At.Framework.Library.CqrXs.CqrMsg;
using Area23.At.Framework.Library.CqrXs.CqrSrv;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Mono.Util;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;


namespace Area23.At.Mono.CqrJD
{
    /// <summary>
    /// Summary description for CqrService
    /// </summary>
    [WebService(Namespace = "https://cqrjd.eu/cqrsrv/cqrjd/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class CqrService : System.Web.Services.WebService
    {
        static HashSet<CqrContact> _contacts;
        CqrContact _contact;
        string _literalServerIPv4, _literalServerIPv6, _literalClientIp;
        string myServerKey = string.Empty;
        string _decrypted = string.Empty;
        string responseMsg = string.Empty;

        [WebMethod]
        public string Send1StSrvMsg(string cryptMsg)
        {
            myServerKey = string.Empty;
            _decrypted = string.Empty;
            responseMsg = string.Empty;
            _contact = null;

            if (Application[Constants.JSON_CONTACTS] != null)
                _contacts = (HashSet<CqrContact>)(Application[Constants.JSON_CONTACTS]);
            else
                _contacts = JsonContacts.LoadJsonContacts();

            _literalClientIp = HttpContext.Current.Request.UserHostAddress;

            if (ConfigurationManager.AppSettings["ExternalClientIP"] != null)
                myServerKey = (string)ConfigurationManager.AppSettings["ExternalClientIP"];
            else
                myServerKey = HttpContext.Current.Request.UserHostAddress;
            myServerKey += Constants.APP_NAME;

            SrvMsg1 srv1stMsg = new SrvMsg1(myServerKey);
            SrvMsg1 cqrSrvResponseMsg = new SrvMsg1(myServerKey);
            SrvMsg responseSrvMsg = new SrvMsg(myServerKey);
            HttpContext.Current.Application["lastmsg"] = cryptMsg;

            try
            {
                if (!string.IsNullOrEmpty(cryptMsg) && cryptMsg.Length >= 8)
                {
                    _contact = srv1stMsg.NCqrSrvMsg1(cryptMsg);
                    _decrypted = _contact.ToJson();
                }
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
                Area23Log.LogStatic(ex);
            }

            responseMsg = responseSrvMsg.CqrSrvMsg("", EncodingType.Base64);

            if (!string.IsNullOrEmpty(_decrypted) && _contact != null && !string.IsNullOrEmpty(_contact.NameEmail))
            {
                Application["lastdecrypted"] = _decrypted;

                CqrContact foundCt = JsonContacts.FindContactByNameEmail(_contacts, _contact);
                if (foundCt != null)
                {
                    foundCt.ContactId = _contact.ContactId;
                    if (foundCt.Cuid == null || foundCt.Cuid == Guid.Empty)
                        foundCt.Cuid = new Guid();
                    if (!string.IsNullOrEmpty(_contact.Address))
                        foundCt.Address = _contact.Address;
                    if (!string.IsNullOrEmpty(_contact.Mobile))
                        foundCt.Mobile = _contact.Mobile;

                    if (_contact.ContactImage != null && !string.IsNullOrEmpty(_contact.ContactImage.ImageFileName) &&
                        !string.IsNullOrEmpty(_contact.ContactImage.ImageBase64))
                        foundCt.ContactImage = _contact.ContactImage;


                    responseMsg = cqrSrvResponseMsg.CqrSrvMsg1(foundCt, EncodingType.Base64);
                }
                else
                {
                    if (_contact.Cuid == null || _contact.Cuid == Guid.Empty)
                        _contact.Cuid = new Guid();
                    _contacts.Add(_contact);
                    foundCt = _contact;

                    responseMsg = cqrSrvResponseMsg.CqrSrvMsg1(foundCt, EncodingType.Base64);
                }

                JsonContacts.SaveJsonContacts(_contacts);
            }


            return responseMsg;

        }



        [WebMethod]
        public string SendSrvMsg(Guid from, Guid to, string cryptMsgSrv, string cryptMsgPartner)
        {
            myServerKey = string.Empty;
            _decrypted = string.Empty;
            responseMsg = string.Empty;
            _contact = null;

            if (Application[Constants.JSON_CONTACTS] != null)
                _contacts = (HashSet<CqrContact>)(Application[Constants.JSON_CONTACTS]);
            else
                _contacts = JsonContacts.LoadJsonContacts();

            _literalClientIp = HttpContext.Current.Request.UserHostAddress;

            if (ConfigurationManager.AppSettings["ExternalClientIP"] != null)
                myServerKey = (string)ConfigurationManager.AppSettings["ExternalClientIP"];
            else
                myServerKey = HttpContext.Current.Request.UserHostAddress;
            myServerKey += Constants.APP_NAME;

            SrvMsg cqrServerMsg = new SrvMsg(myServerKey);
            SrvMsg cqrResponseMsg = new SrvMsg(myServerKey);
            SrvMsg1 cqrSrvResponseMsg = new SrvMsg1(myServerKey);

            HttpContext.Current.Application["lastmsg"] = cryptMsgSrv;

            try
            {
                if (!string.IsNullOrEmpty(cryptMsgSrv) && cryptMsgSrv.Length >= 8)
                {
                    MsgContent msgCt = cqrServerMsg.NCqrSrvMsg(cryptMsgSrv, EncodingType.Base64);
                    _decrypted = msgCt.Message;
                }
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
                Area23Log.LogStatic(ex);
            }

            responseMsg = cqrResponseMsg.CqrSrvMsg(Constants.ACK, EncodingType.Base64);

            if (!string.IsNullOrEmpty(_decrypted))
            {
                Application["lastdecrypted"] = _decrypted;
                CqrContact ctret = _contacts.ToList().Where(c => c.Cuid == to || c.Cuid == from).ToList().FirstOrDefault();
                string reStr = cqrSrvResponseMsg.CqrSrvMsg1(ctret, EncodingType.Base64);

                return reStr;
            }


            return responseMsg;

        }



    }


}
