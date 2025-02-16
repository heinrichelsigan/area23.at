using Area23.At.Framework.Library.CqrXs.CqrMsg;
using Area23.At.Framework.Library;
using Area23.At.Mono.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net;

namespace Area23.At.Mono.CqrJD
{
    public abstract class CqrJdBasePage : System.Web.UI.Page
    {
        protected System.Collections.Generic.Queue<string> mqueue = new Queue<string>();
        protected Uri area23AtUrl = new Uri("https://area23.at/");
        protected Uri cqrXsEuUrl = new Uri("https://cqrxs.eu/cqrsrv/cqrjd/");
        protected Uri githubUrl = new Uri("https://github.com/heinrichelsigan/chat-ipv6/");
        protected internal global::System.Web.UI.WebControls.Image ImageQr;
        protected internal System.Globalization.CultureInfo locale;
        protected global::System.Web.UI.HtmlControls.HtmlAnchor HrefShort;

        protected internal ushort initState = 0x0;
        protected internal string hashKey = string.Empty;
        protected internal string tmpStrg = string.Empty;
        protected internal string allStrng = string.Empty;
        protected internal string decrypted = string.Empty;
        protected internal object _lock = new object();
        protected internal string myServerKey = string.Empty;
        protected internal HashSet<CqrContact> _contacts;
        protected internal CqrContact myContact = null;
        protected internal IPAddress clientIp;


        public System.Globalization.CultureInfo Locale
        {
            get
            {
                if (locale == null)
                {
                    try
                    {
                        string defaultLang = Request.Headers["Accept-Language"].ToString();
                        string firstLang = defaultLang.Split(',').FirstOrDefault();
                        defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                    catch (Exception)
                    {
                        locale = new System.Globalization.CultureInfo("en");
                    }
                }
                return locale;
            }
        }

        public string SepChar { get => System.IO.Path.DirectorySeparatorChar.ToString(); }

        protected CqrJdBasePage() : base()
        {
            initState = 0x1;
        }

        protected virtual void Page_Init(object sender, EventArgs e)
        {

            myServerKey = string.Empty;
            allStrng = string.Empty;
            myContact = null;
            if (Application[Constants.JSON_CONTACTS] != null)
                _contacts = (HashSet<CqrContact>)(Application[Constants.JSON_CONTACTS]);
            else
                _contacts = LoadJsonContacts();

            initState = 0x2;
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (initState < 0x2 || _contacts == null)
            {
                Page_Init(sender, e);
            }

            clientIp = GetClientExternalIp();
            myServerKey = clientIp.ToString();
            allStrng = "UserHostAddress: " + myServerKey + Environment.NewLine;
            myServerKey += Constants.APP_NAME;

            if (Request.Headers["User-Agent"] != null)
                tmpStrg = (string)Request.Headers["User-Agent"];
            else if (Request.Headers["User-Agent:"] != null)
                tmpStrg = (string)Request.Headers["User-Agent:"];
            allStrng += "User-Agent: " + tmpStrg + Environment.NewLine;


            if (Application["lastdecrypted"] != null)
                allStrng += "LastDecrypted: " + (string)Application["lastdecrypted"] + Environment.NewLine;
            if (Application["lastmsg"] != null)
                allStrng += "LastMsg: " + (string)Application["lastmsg"] + Environment.NewLine;

            if (Request.Params["Authorization"] != null)
                allStrng += "Authorization: " + Request.Params["Authorization"].ToString() + Environment.NewLine;

            if ((Request.Files != null && Request.Files.Count > 0))
            {

            }

            initState = 0x4;

        }


        public virtual IPAddress GetClientExternalIp()
        {
            string externalClientIp = Request.UserHostAddress;
            if (ConfigurationManager.AppSettings["ExternalClientIP"] != null)
                externalClientIp = (string)ConfigurationManager.AppSettings["ExternalClientIP"];
            IPAddress clientIpAddr = IPAddress.Parse(externalClientIp);

            return clientIpAddr;
        }


        public virtual void InitURLBase()
        {
            area23AtUrl = new Uri("https://area23.at/net/");
            cqrXsEuUrl = new Uri("https://cqrxs.eu/cqrsrv/cqrjd/");
            githubUrl = new Uri("https://github.com/heinrichelsigan/chat-ipv6/");
        }





        public virtual void Log(string msg)
        {
            Area23Log.LogStatic(msg);
        }




        protected virtual CqrContact FindContactByNameEmail(HashSet<CqrContact> cHashSet, CqrContact searchContact)
        {
            CqrContact cqrFoundContact = JsonContacts.FindContactByNameEmail(cHashSet, searchContact);
            return cqrFoundContact;
        }



        protected virtual HashSet<CqrContact> LoadJsonContacts()
        {
            return JsonContacts.LoadJsonContacts();
        }

        protected virtual void SaveJsonContacts(HashSet<CqrContact> contacts)
        {
            JsonContacts.SaveJsonContacts(contacts);
        }


    }

}