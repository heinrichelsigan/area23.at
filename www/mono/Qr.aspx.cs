using area23.at.www.mono.Util;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Speech.Synthesis.TtsEngine;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static QRCoder.PayloadGenerator;
using System.Security.Policy;


namespace area23.at.www.mono
{
    public partial class Qr : QrBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (this.input_color != null && string.IsNullOrEmpty(input_color.Value))
                    this.input_color.Value = Constants.ColorString;
                if (this.input_backcolor != null && string.IsNullOrEmpty(input_backcolor.Value))
                    this.input_backcolor.Value = Constants.BackColorString;
            }
        }

        protected void QRCode_ParameterChanged(object sender, EventArgs e)
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

            this.TextBox_QrString.BorderColor = Color.Black;
            this.TextBox_QrString.BorderStyle = BorderStyle.Solid;

            this.TextBox_QrPhone.BorderColor = Color.Black;
            this.TextBox_QrPhone.BorderStyle = BorderStyle.Solid;

            this.TextBox_QrUrl.BorderColor = Color.Black;
            this.TextBox_QrUrl.BorderStyle = BorderStyle.Solid;

            this.TextBox_IBAN.BorderColor = Color.Black;
            this.TextBox_IBAN.BorderStyle = BorderStyle.Solid;

            this.TextBox_BIC.BorderColor = Color.Black;
            this.TextBox_BIC.BorderStyle = BorderStyle.Solid;

            this.TextBox_AccountName.BorderColor = Color.Black;
            this.TextBox_AccountName.BorderStyle = BorderStyle.Solid;

            this.TextBox_Reason.BorderColor = Color.Black;
            this.TextBox_Reason.BorderStyle = BorderStyle.Solid;
        }

        protected void LinkButton_QrString_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            QRGenericString qrString = new QRGenericString(this.TextBox_QrString.Text);
            GenerateQRImage(qrString.ToString());
        }

        protected void LinkButton_QrUrl_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            QRCoder.PayloadGenerator.Url qrUrl = new QRCoder.PayloadGenerator.Url(this.TextBox_QrUrl.Text);
            GenerateQRImage(qrUrl.ToString());
        }

        protected void LinkButton_QrPhone_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            QRCoder.PayloadGenerator.PhoneNumber qrPhone = new PhoneNumber(this.TextBox_QrPhone.Text);
            
            GenerateQRImage(qrPhone.ToString());
        }

        protected void LinkButton_QrIBAN_Click(object sender, EventArgs e)
        {
            ResetFormElements();

            QRCoder.PayloadGenerator.BezahlCode qrBank = new BezahlCode(BezahlCode.AuthorityType.contact_v2,
                TextBox_AccountName.Text, TextBox_AccountName.Text, "", TextBox_IBAN.Text, TextBox_BIC.Text, TextBox_Reason.Text);
            GenerateQRImage(qrBank.ToString());
        }

        protected void Button_QRCode_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            GenerateQRImage();
        }


        protected override string GetQrString()
        {
            QRCoder.PayloadGenerator.Url qrUrl = new QRCoder.PayloadGenerator.Url(this.TextBox_QrUrl.Text);
            QRCoder.PayloadGenerator.BezahlCode qrBank = new BezahlCode(BezahlCode.AuthorityType.contact_v2,
                TextBox_AccountName.Text, TextBox_AccountName.Text, "", TextBox_IBAN.Text, TextBox_BIC.Text, TextBox_Reason.Text);
            QRCoder.PayloadGenerator.PhoneNumber qrPhone = new PhoneNumber(this.TextBox_QrPhone.Text);
            string qrString = String.Concat(qrUrl.ToString(), qrPhone.ToString(), qrBank.ToString());
            return qrString;
        }

        protected virtual string GetQrStringFromForm(System.Web.UI.HtmlControls.HtmlForm form)
        {
            return GetQrString();
        }

        protected override void GenerateQRImage(string qrString = "")
        {
            Bitmap aQrBitmap = null;

            if (string.IsNullOrEmpty(this.input_color.Value))
                this.input_color.Value = Constants.ColorString;
            else
                Constants.ColorString = this.input_color.Value;

            if (string.IsNullOrEmpty(this.input_backcolor.Value))
                this.input_backcolor.Value = Constants.BackColorString;
            else
                Constants.BackColorString = this.input_backcolor.Value;

            if (this.Button_QRCode.Attributes["qrcolor"] != null)
                this.Button_QRCode.Attributes["qrcolor"] = Constants.ColorString;
            else
                this.Button_QRCode.Attributes.Add("qrcode", Constants.ColorString);

            try
            {
                Constants.QrColor = Util.ColorFrom.FromHtml(this.input_color.Value);
                Constants.BackColor = Util.ColorFrom.FromHtml(this.input_backcolor.Value);
                qrString = (string.IsNullOrEmpty(qrString)) ? GetQrString() : qrString;

                if (!string.IsNullOrEmpty(qrString))
                {
                    // aQrBitmap = GetQRBitmap(qrString, Constants.QrColor, Color.Transparent);
                    aQrBitmap = GetQRBitmap(qrString, Constants.QrColor);
                }
                if (aQrBitmap != null)
                {
                    SetQRImage(aQrBitmap);
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
            this.ImageQr.BackColor = Util.ColorFrom.FromHtml(this.input_backcolor.Value);
            this.ImageQr.ImageUrl = "data:image/gif;base64," + base64Data;
            ResetFormElements();
        }

    }
}