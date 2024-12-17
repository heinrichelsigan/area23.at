using Area23.At.Framework.Library;
using Area23.At.ChatQ.Util;
using Newtonsoft.Json;
using QRCoder;
using static QRCoder.PayloadGenerator;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using System.Windows.Shapes;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;

namespace Area23.At.ChatQ
{
    public partial class Default : Area23BasePage
    {
        internal Dictionary<string, Uri> shortenMap = null;
        internal Uri redirectUri = null;
        internal string hashKey = string.Empty;
        internal QRCodeGenerator.ECCLevel eCCLevel = QRCodeGenerator.ECCLevel.Q;
        internal short qrMode = 2;

        internal String ShortUrl { get => Constants.URL_SHORT + hashKey; }


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
            }

            if (shortenMap == null)
            {
                shortenMap = (Application[Constants.APP_NAME] != null) ? (Dictionary<string, Uri>)Application[Constants.APP_NAME] : JsonHelper.ShortenMapJson;
            }
        }




    }
}