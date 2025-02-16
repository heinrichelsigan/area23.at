using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Util;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using JsonHelper = Area23.At.Framework.Library.Util.JsonHelper;
using System.Threading;
using Area23.At.Framework.Library.CqrXs.CqrSrv;
using Area23.At.Framework.Library.CqrXs.CqrMsg;
using Area23.At.Framework.Library.CqrXs;

namespace Area23.At.Mono.CqrJD
{
    public partial class Default : CqrJdBasePage
    {

        protected override void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            //if (Application[Constants.JSON_CONTACTS] != null)
            //    _contacts = (HashSet<CqrContact>)(Application[Constants.JSON_CONTACTS]);
            //else
            //    _contacts = LoadJsonContacts();

            tmpStrg = string.Empty;

            if (ConfigurationManager.AppSettings["ServerIPv4"] != null)
            {
                LiteralServerIPv4.Text = (string)ConfigurationManager.AppSettings["ServerIPv4"];
                tmpStrg += "ServerIPv4: " + (string)ConfigurationManager.AppSettings["ServerIPv4"] + Environment.NewLine;
            }
            if (ConfigurationManager.AppSettings["ServerIPv6"] != null)
            {
                this.LiteralServerIPv6.Text = (string)ConfigurationManager.AppSettings["ServerIPv6"];
                tmpStrg += "ServerIPv6: " + (string)ConfigurationManager.AppSettings["ServerIPv6"] + Environment.NewLine + allStrng;
            }

            allStrng = tmpStrg + allStrng;


            this.LiteralClientIp.Text = clientIp.ToString();


            if (!Page.IsPostBack)
            {

                TextBoxLastMsg.Text = string.Empty;

                byte[] bytes = Request.InputStream.ToByteArray();
                string rq = Encoding.UTF8.GetString(bytes);
                if (rq.Contains("TextBoxEncrypted="))
                {
                    rq = rq.Substring(rq.IndexOf("TextBoxEncrypted=") + "TextBoxEncrypted=".Length);
                    if (rq.Contains("TextBoxDecrypted="))
                        rq = rq.Substring(0, rq.IndexOf("TextBoxDecrypted="));
                }

                if (Application["lastmsg"] != null)
                    TextBoxLastMsg.Text = (string)Application["lastmsg"];
                if (Application["lastdecrypted"] != null)
                    this.preLast.InnerHtml = (string)Application["lastdecrypted"];


                SrvMsg1 srv1stMsg = new SrvMsg1(myServerKey);
                decrypted = string.Empty;
                allStrng += "Msg: " + rq.ToString() + Environment.NewLine;

                Application["lastmsg"] = rq;
                this.TextBoxEncrypted.Text = rq;

                try
                {
                    if (!string.IsNullOrEmpty(rq) && rq.Length >= 8)
                    {
                        myContact = srv1stMsg.NCqrSrvMsg1(rq);
                        decrypted = myContact.ToJson();
                    }
                }
                catch (Exception ex)
                {
                    CqrException.SetLastException(ex);
                    this.preOut.InnerText = ex.Message + ex.ToString();
                    Area23Log.LogStatic(ex);
                }


                if (!string.IsNullOrEmpty(decrypted) && myContact != null && !string.IsNullOrEmpty(myContact.NameEmail))
                {
                    allStrng += "Decrypted: " + decrypted.ToString() + Environment.NewLine;
                    Application["lastdecrypted"] = decrypted;

                    CqrContact foundCt = FindContactByNameEmail(_contacts, myContact);
                    if (foundCt != null)
                    {
                        foundCt.ContactId = myContact.ContactId;
                        if (foundCt.Cuid == null || foundCt.Cuid == Guid.Empty)
                            foundCt.Cuid = new Guid();
                        if (!string.IsNullOrEmpty(myContact.Address))
                            foundCt.Address = myContact.Address;
                        if (!string.IsNullOrEmpty(myContact.Mobile))
                            foundCt.Mobile = myContact.Mobile;

                        if (myContact.ContactImage != null && !string.IsNullOrEmpty(myContact.ContactImage.ImageFileName) &&
                            !string.IsNullOrEmpty(myContact.ContactImage.ImageBase64))
                            foundCt.ContactImage = myContact.ContactImage;
                    }
                    else
                    {
                        if (myContact.Cuid == null || myContact.Cuid == Guid.Empty)
                            myContact.Cuid = new Guid();
                        _contacts.Add(myContact);
                    }

                    SaveJsonContacts(_contacts);
                }

                this.TextBoxDecrypted.Text = decrypted;

                if ((string)Application["lastall"] != null)
                    this.preLast.InnerText = (string)Application["lastall"];

                this.preOut.InnerText = allStrng;
                Application["lastall"] = allStrng;
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            this.Title = "CqrJd Testform " + DateTime.Now.Ticks;
        }

    }

}