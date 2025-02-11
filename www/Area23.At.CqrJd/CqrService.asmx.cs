using Area23.At.Framework.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;

using Area23.At;
using Area23.At.Framework.Library.Net.CqrJd;
using Area23.At.Framework.Library.Crypt.CqrJd;
using Area23.At.CqrJd.Util;
namespace Area23.At.CqrJd
{
    /// <summary>
    /// Summary description for CqrService
    /// </summary>
    [WebService(Namespace = "http://cqrjd.eu/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CqrService : System.Web.Services.WebService
    {
        static HashSet<CqrContact> _contacts;
        string _literalServerIPv4, _literalServerIPv6, _literalClientIp;
        string _decrypted;

        [WebMethod]
        public string SendMsg(string cryptMsg)
        {
            string hexall = string.Empty;
            string myServerKey = string.Empty;
            if (Application[Constants.JSON_CONTACTS] != null)
                _contacts = (HashSet<CqrContact>)(Application[Constants.JSON_CONTACTS]);
            else
                _contacts = JsonContacts.LoadJsonContacts();

            if (ConfigurationManager.AppSettings["ServerIPv4"] != null)
                _literalServerIPv4= (string)ConfigurationManager.AppSettings["ServerIPv4"];
            if (ConfigurationManager.AppSettings["ServerIPv6"] != null)
                _literalServerIPv6 = (string)ConfigurationManager.AppSettings["ServerIPv6"];
            _literalClientIp = HttpContext.Current.Request.UserHostAddress;
            
            if (ConfigurationManager.AppSettings["ExternalClientIP"] != null)
                myServerKey = (string)ConfigurationManager.AppSettings["ExternalClientIP"];
            else
                myServerKey = HttpContext.Current.Request.UserHostAddress;                
            myServerKey += Constants.APP_NAME;

            CqrServerMsg serverMessage = new CqrServerMsg(myServerKey);
            _decrypted = string.Empty;
            MsgContent msgContent;

            try
            {
                if (!string.IsNullOrEmpty(cryptMsg) && cryptMsg.Length >= 8)
                {
                    msgContent = serverMessage.NCqrSrvMsg(cryptMsg);
                    _decrypted = msgContent.Message;
                }
            }
            catch (Exception ex)
            {
                // hexall = serverMessage.symmPipe.HexStages;
                // this.preOut.InnerText = ex.Message + ex.ToString();
                Area23Log.LogStatic(ex);
                // _decrypted =  
            }
;

            if (!string.IsNullOrEmpty(_decrypted))
            {
                string[] props = _decrypted.Split(Environment.NewLine.ToCharArray());
                CqrContact c = new CqrContact();
                c.Name = props[0];
                c.Email = props[1];
                //c.Mobile = props[2];
                //c.Address = props[3];
                _contacts.Add(c);
                JsonContacts.SaveJsonContacts(_contacts);

                HttpContext.Current.Application["decrypted"] = _decrypted;
                return Constants.ACK;
            }

            return Constants.NACK;
        }
    }
}
