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

namespace Area23.At.Web.S
{
    public partial class Default : Area23BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (this.input_color != null && string.IsNullOrEmpty(input_color.Value))
                    this.input_color.Value = Constants.QrColorString;
                if (this.input_backcolor != null && string.IsNullOrEmpty(input_backcolor.Value))
                    this.input_backcolor.Value = Constants.BackColorString;
            }
        }

        protected void TextBox_UrlLong_TextChanged(object sender, EventArgs e)
        {
            QRBase_ElementChanged(sender, e);
        }

        protected void TextBox_UrlShort_TextChanged(object sender, EventArgs e)
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

        }

        protected void LinkButton_UrlShorten_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            QRGenericString qrString = new QRGenericString(this.TextBox_UrlLong.Text);
            GenerateQRImage(qrString.ToString());
        }

        protected void LinkButton_UrlQr_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            QRCoder.PayloadGenerator.Url qrUrl = new QRCoder.PayloadGenerator.Url(this.TextBox_UrlLong.Text);
            GenerateQRImage(qrUrl.ToString());
        }

        protected void Button_QRCode_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            GenerateQRImage();
        }


        protected override string GetQrString()
        {
            string qrUrlStr = "", qrGenStr = "", qrBankStr = "", qrPhoneStr = "";
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
                    "<!-- " + ex.ToString() + " -->\r\n" +
                    "<!-- " + ex.StackTrace.ToString() + " -->\r\n";
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

    }
}