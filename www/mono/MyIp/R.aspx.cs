using System;

namespace Area23.At.Mono.MyIp
{
    /// <summary>
    /// only to get my ip-address
    /// </summary>
    public partial class R : System.Web.UI.Page
    {
        double geoLat, geoLong;

        protected void Page_Load(object sender, EventArgs e)
        {
            string userHostName;
            string userHostAddr = Request.UserHostAddress;

            literalUserHost.Text = userHostAddr;

            if (!this.IsPostBack)
            {
                userHostName = Request.UserHostName;
                title.Text = userHostName;
                if (Request.QueryString["geolat"] != null && Request.QueryString["geolong"] != null)
                {
                    geoLat = Double.Parse(Request.QueryString["geolat"].Replace(".", ","));
                    geoLong = Double.Parse(Request.QueryString["geolong"].Replace(".", ","));
                    GeoLink.NavigateUrl = "https://www.google.com/maps/@" + Request.QueryString["geolat"] + "," + Request.QueryString["geolong"] + ",15z";
                    GeoLink.Text = "Geo Location: " + Request.QueryString["geolat"] + "," + Request.QueryString["geolong"];
                    GeoLink.Target = "_blank";
                }
                

                // header.InnerHtml = "<title>" + userHostName + "</title>";
            }

        }

    }

}