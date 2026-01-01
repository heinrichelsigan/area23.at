using Org.BouncyCastle.Utilities.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

namespace Area23.At.Mono.MyIp
{

    public struct GeoRecord
    {
        public int Latitude { get; set; }
        public int Longitude { get; set; }

        public string HostName { get; set; }
        public IPAddress IPAddr { get; set; }

        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }

    }


    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GeoRecord geoRecord =GetIplocFrame(ipapiFrame, "https://area23.at/net/R.aspx");
        }


        protected GeoRecord GetIplocFrame(HtmlGenericControl htmlCtrl, string url)
        {
            htmlCtrl = htmlCtrl ?? ipapiFrame;
 
            url = url ?? "https://area23.at/net/R.aspx";
            string resolved = htmlCtrl.ResolveClientUrl(url);
            resolved = htmlCtrl.ResolveUrl(url);
            GeoRecord geoRecord = new GeoRecord();
            string innerHtml = htmlCtrl.InnerHtml;
            string text = htmlCtrl.InnerText;
            string outText = "";
            byte[] rbytes = new byte[8192];
            //Stream htrsps = htmlCtrl.Page.Response.OutputStream;

            //htmlCtrl.Page.Response.OutputStream.Read(rbytes, 0, 8192);
            //outText = System.Text.Encoding.UTF8.GetString(rbytes);

            foreach (Control ctl in htmlCtrl.Controls)
            {
                ctl.Page.Response.Output.ToString();
            }
            return geoRecord;
        }

    }
}