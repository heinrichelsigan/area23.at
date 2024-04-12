using Area23.At.Www.Common;
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
using Area23.At.Www.S.Util;
using Newtonsoft.Json;
using QRCoder;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;
using System.Web.DynamicData;
using Area23.At.Framework.Library;

namespace Area23.At.Www.S
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
                if (this.input_color != null && string.IsNullOrEmpty(input_color.Value))
                    this.input_color.Value = Const.QrColorString;
                if (this.input_backcolor != null && string.IsNullOrEmpty(input_backcolor.Value))
                    this.input_backcolor.Value = Const.BackColorString;
            }

            if (shortenMap == null)
            {
                shortenMap = (Application[Constants.APP_NAME] != null) ? (Dictionary<string, Uri>)Application[Constants.APP_NAME] : JsonHelper.GetShortenMapFromJson();
            }
        }

        protected void TextBox_UrlLong_TextChanged(object sender, EventArgs e)
        {
            Button_QRCode_Click(sender, e);
        }

        protected void Button_QRCode_Click(object sender, EventArgs e)
        {
            if ((redirectUri = VerifyUri(this.TextBox_UrlLong.Text)) != null)
            {
                hashKey = ShortenUri(redirectUri);
                this.HrefShort.HRef = ShortUrl;
                this.HrefShort.InnerHtml = ShortUrl;
                this.HrefShort.Visible = true;
                ResetChangedElements();
                GenerateQRImage();
            }
            else
            {
                this.HrefShort.Visible = false;
                this.ImageQr.Visible = false;
                QRBase_ElementChanged(sender, e);
            }
        }

        protected void DropDown_PixelPerUnit_Changed(object sender, EventArgs e)
        {
            string qrModeStr = this.DropDown_PixelPerUnit.SelectedItem.Text;
            switch (qrModeStr)
            {
                case "1": qrMode = 1; break;
                case "2": qrMode = 2; break;
                case "3": qrMode = 3; break;
                case "4": qrMode = 4; break;
                case "6": qrMode = 6; break;
                case "8": qrMode = 8; break;
                default: qrMode = 2; break;
            }
            Button_QRCode_Click(sender, e);
        }

        protected void DropDown_QrMode_Changed(object sender, EventArgs e)
        {
            string eccModeStr = this.DropDown_QrMode.SelectedItem.Text;
            switch (eccModeStr)
            {
                case "L": eCCLevel = QRCodeGenerator.ECCLevel.L; break;
                case "M": eCCLevel = QRCodeGenerator.ECCLevel.M; break;
                case "Q": eCCLevel = QRCodeGenerator.ECCLevel.Q; break;
                case "H": eCCLevel = QRCodeGenerator.ECCLevel.H; break;
            }
            Button_QRCode_Click(sender, e);
        }


        protected virtual void ResetFormElements()
        {
            ResetChangedElements();
        }

        protected override void ResetChangedElements()
        {
            // base.ResetChangedElements();

            this.HrefShort.Visible = true;
            // this.HrefShort.Style["Border"]

            this.TextBox_UrlLong.BorderColor = Color.Black;
            this.TextBox_UrlLong.BorderStyle = BorderStyle.Solid;

            this.ErrorDiv.InnerHtml = string.Empty;
            this.ErrorDiv.Visible = false;
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
            if (!string.IsNullOrEmpty(this.HrefShort.HRef))
            {
                qrUrl = new QRCoder.PayloadGenerator.Url(this.HrefShort.HRef);
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
            int qrWidth = 128;
            string qrImgPath = string.Empty;
            // Bitmap aQrBitmap = null;

            if (string.IsNullOrEmpty(this.input_color.Value))
                this.input_color.Value = Const.QrColorString;
            else
                Const.QrColorString = this.input_color.Value;

            if (string.IsNullOrEmpty(this.input_backcolor.Value))
                this.input_backcolor.Value = Const.BackColorString;
            else
                Const.BackColorString = this.input_backcolor.Value;

            if (this.Button_QRCode.Attributes["qrcolor"] != null)
                this.Button_QRCode.Attributes["qrcolor"] = Const.QrColorString;
            else
                this.Button_QRCode.Attributes.Add("qrcode", Const.QrColorString);

            try
            {
                Const.QrColor = ColorFrom.FromHtml(this.input_color.Value);
                Const.BackColor = ColorFrom.FromHtml(this.input_backcolor.Value);
                qrString = (string.IsNullOrEmpty(qrString)) ? GetQrString() : qrString;

                if (!string.IsNullOrEmpty(qrString))
                {
                    // aQrBitmap = GetQRBitmap(qrString, Constants.QrColor, Color.Transparent);
                    qrImgPath = GetQRImgPath(qrString, ref qrWidth, this.input_color.Value, this.input_backcolor.Value, qrMode, eCCLevel);
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
            ResetChangedElements();
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

            try
            {
                redirectUri = new Uri(redirUrl);
                if (!redirectUri.IsAbsoluteUri && redirUrl.Length < 4)
                {
                    ErrorDiv.InnerHtml = "<p><span style=\"font-size: large; color: red\">" + redirUrl + "</span><br />isn't an AbsoluteUri!</p>\r\n";
                    ErrorDiv.Visible = true;
                    this.TextBox_UrlLong.BackColor = Color.Red;
                    this.TextBox_UrlLong.BorderStyle = BorderStyle.Dotted;
                    this.TextBox_UrlLong.BorderWidth = 1;
                    return null;
                }

                if (shortenMap.ContainsValue(redirectUri))
                {
                    foreach (var mapEntry in shortenMap)
                        if (mapEntry.Value == redirectUri)
                            return redirectUri;
                }
            }
            catch (Exception exRedir)
            {
                Area23Log.LogStatic(exRedir);
                ErrorDiv.InnerHtml = "<p style=\"font-size: large; color: red\">" + exRedir.Message + "</p>\r\n" +
                    "<pre>" + exRedir.ToString() + "</pre>\r\n" +
                    "<!-- " + exRedir.StackTrace.ToString() + " -->\r\n";
                ErrorDiv.Visible = true;

                redirectUri = null;
                return redirectUri;
            }

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(redirectUri.ToString());
                request.Method = "GET";
                request.Timeout = 2750;
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
                if (shortenMap == null || shortenMap.Count == 0)
                    shortenMap = (Dictionary<string, Uri>)(Application[Constants.APP_NAME] ?? JsonHelper.GetShortenMapFromJson());
                
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