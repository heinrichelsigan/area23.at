using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace area23.at.www.mono
{
    public partial class Area23 : System.Web.UI.MasterPage
    {

        public global::System.Web.UI.HtmlControls.HtmlForm MasterFrom { get => this.form1; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterHead { get => this.head; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterBody { get => this.bodydiv; }


        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}