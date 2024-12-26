using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Net.CqrJd;
using Area23.At.Framework.Library.Util;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.CqrJD
{
    public partial class Default : System.Web.UI.Page
    {

        string hashKey = string.Empty;
        string decrypted = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["ServerIPv4"] != null)
                this.LiteralServerIPv4.Text = (string)ConfigurationManager.AppSettings["ServerIPv4"];
            if (ConfigurationManager.AppSettings["ServerIPv6"] != null)
                this.LiteralServerIPv6.Text = (string)ConfigurationManager.AppSettings["ServerIPv6"];
            this.LiteralClientIp.Text = Request.UserHostAddress;

            if (Request.Headers["User-Agent"] != null)
                this.LiteralFromClient.Text = (string)Request.Headers["User-Agent"];
            if (Request.Headers["User-Agent:"] != null)
                this.LiteralFromClient.Text = (string)Request.Headers["User-Agent:"];


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
                }
                if (rq.Contains("TextBoxDecrypted="))
                    rq = rq.Substring(0, rq.IndexOf("TextBoxDecrypted="));
                

                if (Application["lastmsg"] != null)
                {
                    TextBoxLastMsg.Text = (string)Application["lastmsg"].ToString();
                }

                string myServerKey = Request.UserHostAddress + Constants.BC_START_MSG;
                CqrServerMsg serverMessage = new CqrServerMsg(myServerKey);
                try
                {
                    decrypted = serverMessage.NCqrMessage(rq);
                }
                catch (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                    decrypted = ex.Message + ex.ToString();
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

    }

}