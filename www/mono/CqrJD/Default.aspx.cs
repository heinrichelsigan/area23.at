using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Cqr.Msg;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Configuration;
using System.Text;
using System.Web.UI;

namespace Area23.At.Mono.CqrJD
{
    /// <summary>
    /// Default TesWebForm for cqrxs.eu
    /// </summary>
    public partial class Default : CqrJdBasePage
    {

        protected override void Page_Init(object sender, EventArgs e)
        {
            base.Page_Init(sender, e);
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            Response.Redirect("CqrService.asmx");
            return;

            //if (Application[Constants.JSON_CONTACTS] != null)
            //    _contacts = (HashSet<CqrContact>)(Application[Constants.JSON_CONTACTS]);
            //else
            //    _contacts = LoadJsonContacts();

            tmpStrg = string.Empty;

            if (ConfigurationManager.AppSettings["ServerIPv4"] != null)
            {
                LiteralServerIPv4.Text = (string)ConfigurationManager.AppSettings["ServerIPv4"];
                Area23Log.LogStatic("ServerIPv4: " + LiteralServerIPv4.Text);
                tmpStrg += "ServerIPv4: " + (string)ConfigurationManager.AppSettings["ServerIPv4"] + Environment.NewLine;
            }
            if (ConfigurationManager.AppSettings["ServerIPv6"] != null)
            {
                this.LiteralServerIPv6.Text = (string)ConfigurationManager.AppSettings["ServerIPv6"];
                Area23Log.LogStatic("ServerIPv6: " + LiteralServerIPv6.Text);
                tmpStrg += "ServerIPv6: " + (string)ConfigurationManager.AppSettings["ServerIPv6"] + Environment.NewLine + allStrng;
            }

            allStrng = tmpStrg + allStrng;


            this.LiteralClientIp.Text = myServerKey.Replace(Constants.APP_NAME, "").ToString();
            Area23Log.LogStatic("ClientIp: " + myServerKey.Replace(Constants.APP_NAME, "").ToString());


            if (!Page.IsPostBack)
            {

                TextBoxLastMsg.Text = string.Empty;

                byte[] bytes = Request.InputStream.ToByteArray();
                string rq = Encoding.UTF8.GetString(bytes);
                if (rq.Contains("TextBoxEncrypted="))
                {
                    // rq = rq.GetSubStringByPattern("TextBoxEncrypted=", true, "", "TextBoxDecrypted=", false);
                    rq = rq.Substring(rq.IndexOf("TextBoxEncrypted=") + "TextBoxEncrypted=".Length);
                    if (rq.Contains("TextBoxDecrypted="))
                        rq = rq.Substring(0, rq.IndexOf("TextBoxDecrypted="));
                }

                if (Application["lastmsg"] != null)
                    TextBoxLastMsg.Text = (string)Application["lastmsg"];
                if (Application["lastdecrypted"] != null)
                    this.preLast.InnerHtml = (string)Application["lastdecrypted"];

                Area23Log.LogStatic("myServerKey = " + myServerKey);
                CqrFacade cqrFacade = new CqrFacade(myServerKey);
                Application["ServerKey"] = myServerKey;
                decrypted = string.Empty;
                allStrng += "Msg: " + rq.ToString() + Environment.NewLine;
                Area23Log.LogStatic("Msg: " + rq.ToString());

                Application["lastmsg"] = rq;
                this.TextBoxEncrypted.Text = rq;
                CContact aContact = new CContact() { _hash = cqrFacade.PipeString };

                try
                {
                    if (!string.IsNullOrEmpty(rq) && rq.Length >= 8)
                    {
                        AppDomain.CurrentDomain.SetData(Constants.FISH_ON_AES_ENGINE, true);
                        myContact = aContact.DecryptFromJson(myServerKey, rq);
                        decrypted = $"<textarea name=TextBoxDecrypted>\r\nThank you Mr./Mrs. for registration {myContact.Name} [{myContact.Email}],\r\n";

                        Area23Log.LogStatic("Contact.ToJson(): " + decrypted);
                    }
                }
                catch (Exception ex)
                {
                    CqrException.SetLastException(ex);
                    decrypted = $"<textarea name=TextBoxDecrypted>\r\nThank you Mr./Mrs. for registration,\r\n";
                    this.preOut.InnerText = ex.Message + ex.ToString();
                    Area23Log.LogStatic(ex);
                }

                decrypted += "\n\r\nPlease wait until 30.April 2025 on next version\r\n and download new client then from https://srv.cqrxs.eu/v1,1/, \n\r\n";
                decrypted += "\r\nCurrently 3-fish rides on AesEngine,\r\n which is not propper and will be fixed soonly!\r\n\r\n";
                decrypted += "\r\nSincerly he,\r\n have a nice day!\r\n\r\n</textarea>\r\n";
                this.TextBoxDecrypted.Text = decrypted;

                if (!string.IsNullOrEmpty(decrypted) && myContact != null && !string.IsNullOrEmpty(myContact.NameEmail))
                {

                    CContact foundCt = FindContactByNameEmail(_contacts, myContact);
                    if (foundCt != null)
                    {
                        Area23Log.LogStatic("found contact: " + foundCt.ToString());
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

                        decrypted = foundCt.ToJson();
                    }
                    else
                    {
                        if (myContact.Cuid == null || myContact.Cuid == Guid.Empty)
                            myContact.Cuid = new Guid();
                        _contacts.Add(myContact);

                        Area23Log.LogStatic("contact added: " + myContact.ToString());
                        decrypted = myContact.ToJson();
                        foundCt = myContact;
                    }


                    allStrng += "Decrypted: " + decrypted.ToString() + Environment.NewLine;
                    Application["lastdecrypted"] = decrypted;

                    SaveJsonContacts(_contacts);
                }


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