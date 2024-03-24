using Area23.At.Web.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static QRCoder.PayloadGenerator;
using Area23.At.Web.S.Util;
using Newtonsoft.Json;
using QRCoder;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;

namespace Area23.At.Web.S
{
    public partial class Default : Area23BasePage
    {
        internal Dictionary<string, Uri> shortenMap = null;
        internal Uri redirectUri = null;
        internal string hashKey = string.Empty;

        internal String ShortUrl { get => Constants.URL_SHORT + hashKey; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (this.input_color != null && string.IsNullOrEmpty(input_color.Value))
                    this.input_color.Value = Constants.QrColorString;
                if (this.input_backcolor != null && string.IsNullOrEmpty(input_backcolor.Value))
                    this.input_backcolor.Value = Constants.BackColorString;
            }

            if (shortenMap == null)
            {
                shortenMap = (Application[Constants.APP_NAME] != null) ? (Dictionary<string, Uri>)Application[Constants.APP_NAME] : JsonHelper.GetShortenMapFromJson();
            }
        }

        protected void TextBox_UrlLong_TextChanged(object sender, EventArgs e)
        {
            QRBase_ElementChanged(sender, e);
        }
        

        protected virtual void ResetFormElements()
        {
            ResetChangedElements();
        }

        protected override void ResetChangedElements()
        {
            // base.ResetChangedElements();

            this.TextBox_UrlShort.BorderColor = Color.Black;
            this.TextBox_UrlShort.BorderStyle = BorderStyle.Solid;

            this.TextBox_UrlLong.BorderColor = Color.Black;
            this.TextBox_UrlLong.BorderStyle = BorderStyle.Solid;

            this.ErrorDiv.InnerHtml = string.Empty;
            this.ErrorDiv.Visible = false;
        }

        protected void LinkButton_UrlShorten_Click(object sender, EventArgs e)
        {
            if ((redirectUri = VerifyUri(this.TextBox_UrlLong.Text)) != null)
            {
                hashKey = ShortenUri(redirectUri);
                this.TextBox_UrlShort.Text = ShortUrl;
                ResetFormElements();
                QRCoder.PayloadGenerator.Url qrUrl = new QRCoder.PayloadGenerator.Url(this.TextBox_UrlLong.Text);
                GenerateQRImage(qrUrl.ToString());
            }            
        }

        protected void LinkButton_UrlQr_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBox_UrlShort.Text) || (!this.TextBox_UrlShort.Text.Contains(Constants.URL_SHORT)))
            {
                if ((redirectUri = VerifyUri(this.TextBox_UrlLong.Text)) != null)
                {
                    hashKey = ShortenUri(redirectUri);
                    this.TextBox_UrlShort.Text = ShortUrl;                    
                }
            }
                
            if (!string.IsNullOrEmpty(this.TextBox_UrlShort.Text) && (this.TextBox_UrlShort.Text.Contains(Constants.URL_SHORT)))
            {                
                ResetFormElements();
                QRCoder.PayloadGenerator.Url qrUrl = new QRCoder.PayloadGenerator.Url(this.TextBox_UrlShort.Text);
                GenerateQRImage(qrUrl.ToString());
            }
        }

        protected void Button_QRCode_Click(object sender, EventArgs e)
        {
            if ((redirectUri = VerifyUri(this.TextBox_UrlLong.Text)) != null)
            {
                hashKey = ShortenUri(redirectUri);
                this.TextBox_UrlShort.Text = ShortUrl;
                ResetFormElements();
                GenerateQRImage();
            }
        }

        #region qrmembers

        protected override string GetQrString()
        {
            string qrUrlStr = "";
            QRCoder.PayloadGenerator.Url qrUrl = null;
            if (!string.IsNullOrEmpty(this.TextBox_UrlLong.Text))
            {
                qrUrl = new QRCoder.PayloadGenerator.Url(this.TextBox_UrlLong.Text);
                qrUrlStr = qrUrl.ToString();
            }
            if (!string.IsNullOrEmpty(this.TextBox_UrlShort.Text))
            {
                qrUrl = new QRCoder.PayloadGenerator.Url(this.TextBox_UrlShort.Text);
                qrUrlStr = qrUrl.ToString();
            }

            return qrUrlStr;
        }

        protected virtual string GetQrStringFromForm(System.Web.UI.HtmlControls.HtmlForm form)
        {
            return GetQrString();
        }

        protected override void GenerateQRImage(string qrString = "")
        {
            int qrWidth = -1;
            string qrImgPath = string.Empty;
            // Bitmap aQrBitmap = null;

            if (string.IsNullOrEmpty(this.input_color.Value))
                this.input_color.Value = Constants.QrColorString;
            else
                Constants.QrColorString = this.input_color.Value;

            if (string.IsNullOrEmpty(this.input_backcolor.Value))
                this.input_backcolor.Value = Constants.BackColorString;
            else
                Constants.BackColorString = this.input_backcolor.Value;

            if (this.Button_QRCode.Attributes["qrcolor"] != null)
                this.Button_QRCode.Attributes["qrcolor"] = Constants.QrColorString;
            else
                this.Button_QRCode.Attributes.Add("qrcode", Constants.QrColorString);

            try
            {
                Constants.QrColor = ColorFrom.FromHtml(this.input_color.Value);
                Constants.BackColor = ColorFrom.FromHtml(this.input_backcolor.Value);
                qrString = (string.IsNullOrEmpty(qrString)) ? GetQrString() : qrString;

                if (!string.IsNullOrEmpty(qrString))
                {
                    // aQrBitmap = GetQRBitmap(qrString, Constants.QrColor, Color.Transparent);
                    qrImgPath = GetQRImgPath(qrString, out qrWidth, this.input_color.Value, this.input_backcolor.Value);
                }
                if (!string.IsNullOrEmpty(qrImgPath))
                {
                    SetQrImageUrl(qrImgPath, qrWidth);
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                ErrorDiv.Visible = true;
                ErrorDiv.InnerHtml = "<p style=\"font-size: large; color: red\">" + ex.Message + "</p>\r\n" +
                    "<pre>" + ex.ToString() + "</pre>\r\n" +
                    "<!-- " + ex.StackTrace.ToString() + " -->\r\n";
                ErrorDiv.Visible = true;
                
                    
            }
        }

        protected override void SetQRImage(Bitmap qrImage)
        {
            MemoryStream ms = new MemoryStream();
            qrImage.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            // this.ImgQR.Src = "data:image/gif;base64," + base64Data;
            this.ImageQr.Visible = true;
            this.ImageQr.BackColor = ColorFrom.FromHtml(this.input_backcolor.Value);
            this.ImageQr.ImageUrl = "data:image/gif;base64," + base64Data;
            ResetFormElements();
        }

        protected override void SetQrImageUrl(string imgPth, int qrWidth = -1)
        {
            this.ImageQr.Visible = true;
            this.ImageQr.BackColor = ColorFrom.FromHtml(this.input_backcolor.Value);
            this.ImageQr.ImageUrl = imgPth;
            if (qrWidth > 0)
                this.ImageQr.Width = qrWidth;
        }

        #endregion qrmembers


        /// <summary>
        /// VerifyUri
        /// </summary>
        /// <param name="redirUrl">url from which qr code is generated and where page redirects afer 8 sec</param>
        /// <returns>Uri</returns>
        protected virtual Uri VerifyUri(string redirUrl)
        {
            if (string.IsNullOrEmpty(redirUrl))
            {
                ErrorDiv.InnerHtml = "<p style=\"font-size: large; color: red\">Url to shorten doesn't exist!</p>\r\n";
                ErrorDiv.Visible = true;
                this.TextBox_UrlLong.BackColor = Color.Red;
                this.TextBox_UrlLong.BorderStyle = BorderStyle.Dashed;
                this.TextBox_UrlLong.BorderWidth = 1;
                return null;
            }                

            redirectUri = new Uri(redirUrl);
            if (!redirectUri.IsAbsoluteUri)
            {
                ErrorDiv.InnerHtml = "<p><span style=\"font-size: large; color: red\">" + redirUrl + "</span><br />isn't an AbsoluteUri!</p>\r\n";
                ErrorDiv.Visible = true;
                this.TextBox_UrlLong.BackColor = Color.Red;
                this.TextBox_UrlLong.BorderStyle = BorderStyle.Dotted;
                this.TextBox_UrlLong.BorderWidth = 1;
                return null;
            }               
            
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(redirectUri.ToString());
                request.Method = "GET";
                request.Timeout = 1500;
                request.Headers.Add("accept-encoding", "gzip, deflate, br");
                request.Headers.Add("cache-control", "max-age=0");
                request.Headers.Add("accept-language", "en-US,en;q=0.9");
                request.UserAgent = "Apache2 mod_mono Amazon aws by https://area23.at/s/ to verify shortend url";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.Accepted)
                {
                    response.Close();
                    return redirectUri;
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);                    
                ErrorDiv.InnerHtml = "<p style=\"font-size: large; color: red\">" + ex.Message + "</p>\r\n" +
                    "<pre>" + ex.ToString() + "</pre>\r\n" +
                    "<!-- " + ex.StackTrace.ToString() + " -->\r\n";
                ErrorDiv.Visible = true;
                if ((ex.Message.ToString().Contains("403")) ||
                    (ex.Message.ToString().Contains("The operation has timed out.")))
                    return redirectUri;

                redirectUri = null;
            }

            return redirectUri;
        }


        /// <summary>
        /// ShortenUri - shortens a Uri to hash
        /// </summary>
        /// <param name="longUri">long Uri</param>
        /// <returns><see cref="string" />hashvalue from</returns>
        protected virtual string ShortenUri(Uri longUri)
        {            
            string shortHash = string.Empty;

            if (longUri != null)
            {
                // if already uri exists in Dictionary => return hash
                if (shortenMap.ContainsValue(longUri))
                {
                    foreach (var mapEntry in shortenMap)
                        if (mapEntry.Value == longUri)
                            return mapEntry.Key;
                }

                Random rand = new Random(DateTime.Now.Millisecond);
                Byte[] bytes = new Byte[2];

                shortHash += String.Format("{0:x}", longUri.ToString().GetHashCode());
                while (shortenMap.ContainsKey(shortHash))
                {
                    rand.NextBytes(bytes);
                    shortHash += String.Format("{0:x2}{1:x2}", bytes[0], bytes[1]);                    
                }

                shortenMap.Add(shortHash, longUri);
                JsonHelper.SaveDictionaryToJson(shortenMap);                
            }
            
            return shortHash;
        }

    }
}