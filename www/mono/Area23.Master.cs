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

        public global::System.Web.UI.HtmlControls.HtmlForm MasterForm { get => this.Area23MasterForm; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterHead { get => this.HeadContentPlaceHolder; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterBody { get => this.BodyContentPlaceHolder; }


        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}