﻿using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Net.CqrJd;
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
using Area23.At.Framework.Library.Crypt.CqrJd;

namespace Area23.At.Mono.CqrJD
{
    public partial class Default : System.Web.UI.Page
    {

        string hashKey = string.Empty;
        string decrypted = string.Empty;
        object _lock = new object();
        HashSet<CqrContact> _contacts;
       

        protected void Page_Load(object sender, EventArgs e)
        {
            string hexall = string.Empty;
            string myServerKey = string.Empty;
            if (Application[Constants.JSON_CONTACTS] != null)
                _contacts = (HashSet<CqrContact>)(Application[Constants.JSON_CONTACTS]);
            else
                _contacts = LoadJsonContacts();

            if (ConfigurationManager.AppSettings["ServerIPv4"] != null)
                this.LiteralServerIPv4.Text = (string)ConfigurationManager.AppSettings["ServerIPv4"];
            if (ConfigurationManager.AppSettings["ServerIPv6"] != null)
                this.LiteralServerIPv6.Text = (string)ConfigurationManager.AppSettings["ServerIPv6"];
            if (ConfigurationManager.AppSettings["ExternalClientIP"] != null)
                myServerKey = (string)ConfigurationManager.AppSettings["ExternalClientIP"];
            if (string.IsNullOrEmpty(myServerKey))
                myServerKey = Request.UserHostAddress;

            this.LiteralClientIp.Text = myServerKey;

            if (Request.Headers["User-Agent"] != null)
                this.LiteralFromClient.Text = (string)Request.Headers["User-Agent"];
            if (Request.Headers["User-Agent:"] != null)
                this.LiteralFromClient.Text = (string)Request.Headers["User-Agent:"];
            
            myServerKey += Constants.APP_NAME;

            if (!Page.IsPostBack)
            {
                if (Request.Params["Authorization"] != null)
                {
                    hashKey = Request.Params["Authorization"].ToString();
                }
                if ((Request.Files != null && Request.Files.Count > 0))
                {

                }
                TextBoxLastMsg.Text = string.Empty;

                byte[] bytes = Request.InputStream.ToByteArray();
                string rq = Encoding.UTF8.GetString(bytes);
                if (rq.Contains("TextBoxEncrypted="))
                {
                    rq = rq.Substring(rq.IndexOf("TextBoxEncrypted="));
                    rq = rq.Substring("TextBoxEncrypted=".Length);
                    if (rq.Contains("TextBoxDecrypted="))
                        rq = rq.Substring(0, rq.IndexOf("TextBoxDecrypted="));
                    // rq = rq.TrimEnd("\r\n".ToArray());
                }

                
                if (Application["lastmsg"] != null)                
                    TextBoxLastMsg.Text = (string)Application["lastmsg"];
                if (Application["decrypted"] != null)
                    this.preLast.InnerHtml = (string)Application["decrypted"];
                                               
                CqrServerMsg serverMessage = new CqrServerMsg(myServerKey);
                MsgContent msgContent;
                decrypted = string.Empty;

                try
                {
                    if (!string.IsNullOrEmpty(rq) && rq.Length >= 8)
                    {
                        msgContent = serverMessage.NCqrSrvMsg(rq);
                        decrypted = msgContent.Message;
                    }
                }
                catch (Exception ex)
                {
                    // hexall = serverMessage.symmPipe.HexStages;
                    this.preOut.InnerText = ex.Message + ex.ToString();
                    Area23Log.LogStatic(ex);
                }


                if (!string.IsNullOrEmpty(decrypted))
                {
                    Application["decrypted"] = decrypted;
                }
                Application["lastmsg"] = rq;
                this.TextBoxEncrypted.Text = rq;
                this.TextBoxDecrypted.Text = decrypted;

            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            this.Title = "CqrJd Testform " + DateTime.Now.Ticks;
        }


        protected HashSet<CqrContact> LoadJsonContacts()
        {
            return JsonContacts.LoadJsonContacts();
        }

        protected void SaveJsonContacts(HashSet<CqrContact> contacts)
        {
            JsonContacts.SaveJsonContacts(contacts);
        }

    }

}