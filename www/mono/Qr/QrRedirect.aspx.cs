using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Area23.At.Mono.Qr
{
    public partial class QrRedirect : System.Web.UI.Page
    {

        string redirectUrl = string.Empty;
        Uri redirectUri = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string qrUrl = Request.QueryString["qrurl"];
                if (string.IsNullOrEmpty(qrUrl))
                    qrUrl = HttpUtility.UrlEncode("https://area23.at/", Encoding.UTF8);

                redirectUrl = HttpUtility.UrlDecode(qrUrl, Encoding.UTF8);
                string qrCodeString = GetQrStringFromUrl(ref redirectUrl);
                if (!string.IsNullOrEmpty(qrCodeString)) {
                    Bitmap qrGenBmp = GetQRBitmap(qrCodeString);
                    SetQRImage(qrGenBmp, redirectUrl);
                }
                
            }
        }


        /// <summary>
        /// Get QR Code String from Url
        /// </summary>
        /// <param name="redirUrl">reference to url from which qr code is generated and where page redirects afer 8 sec</param>
        /// <returns>QrCodeString</returns>
        protected virtual string GetQrStringFromUrl(ref string redirUrl)
        {
            string qrString = "";
        
            redirectUri = new Uri(redirUrl);
            if (redirectUri.IsAbsoluteUri)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(redirectUri.ToString());
                    request.Method = "GET";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found)
                    {
                        response.Close();
                        PayloadGenerator.Url qrGenUrl = new PayloadGenerator.Url(redirectUri.ToString());
                        qrString = qrGenUrl.ToString();
                        return qrString;
                    }                    
                }
                catch (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                    qrString = "";
                }                
            }

            return qrString;

        }

        protected virtual Bitmap GetQRBitmap(string qrString)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrString, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }

        protected string GetRedirectUrlName(string redirUrl)
        {
            string urlName = "";
            string origUrl = (string.IsNullOrEmpty(redirUrl)) ? string.Empty :
                redirUrl.Replace('.', '_').Replace('/', '-');
            foreach (char ch in origUrl)
            {
                if ((((int)ch) >= ((int)'A') && ((int)ch) <= ((int)'Z')) ||
                    (((int)ch) >= ((int)'a') && ((int)ch) <= ((int)'z')) ||
                    (((int)ch) >= ((int)'0') && ((int)ch) <= ((int)'9')) ||
                    (((int)ch) == ((int)'_')) ||
                    (((int)ch) == ((int)'-')))
                {
                    urlName += ch;
                }
            }
            return urlName + ".gif";
        }

        protected virtual void SetQRImage(Bitmap qrImage, string redirUrl) 
        {
            string fileName = LibPaths.SystemDirOutPath + GetRedirectUrlName(redirUrl);
            MemoryStream ms = new MemoryStream();
            qrImage.Save(ms, ImageFormat.Gif);
            qrImage.Save(fileName, ImageFormat.Gif);
            byte[] buffer = ms.ToArray();
            string base64Data = Convert.ToBase64String(ms.ToArray());
            
            var refreshTag = new HtmlGenericControl("meta");
            refreshTag.Attributes.Add("http-equiv", "refresh");
            refreshTag.Attributes.Add("content", "12; URL=" + redirUrl);

            // Response.ClearContent();
            // if (Response.Headers["Refresh"] != null)
            //     Response.Headers.Remove("Refresh");
            this.Header.Controls.Add(refreshTag);
            // Response.Headers.Add("Content-Type", "application/octet-stream");
            // Response.AddHeader("Content-Disposition", "attachment; filename=" + GetRedirectUrlName(redirUrl));
            // Response.AddHeader("Content-Length", buffer.Length.ToString());
            // Response.Headers.Add("Content-Type", "image/gif");
            // Response.Headers.Add("Refresh", "6; url=" + redirUrl + "");                            
            this.ImgQR.Src = "data:image/gif;base64," + base64Data;
            aHref.HRef = redirUrl;
            aHref.Title = "Redirecting to " + redirUrl + " ...";
            aHref.InnerText = "Redirecting to " + redirUrl + " ...";
        }
    }
}