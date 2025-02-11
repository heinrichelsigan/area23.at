using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.CqrJd
{
    public partial class Index : System.Web.UI.Page
    {
        string hashKey = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
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
                
                if (Application["lastmsg"] != null)
                {
                    TextBoxLastMsg.Text = (string)Application["lastmsg"].ToString();
                }

                Application["lastmsg"] = rq;
                this.TextBoxSubmit.Text = rq;
                
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            ViewState["Submit"] = this.TextBoxSubmit.Text;
        }
    }
}