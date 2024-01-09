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
                this.TextBox_AccountName.Text, "", "", TextBox_IBAN.Text, TextBox_BIC.Text, "test https://area23.at/u/");
            GenerateQRImage(qrBank.ToString());
        }

        protected override string GetQrString()
        {
            QRCoder.PayloadGenerator.Url qrUrl = new QRCoder.PayloadGenerator.Url(this.TextBox_QrUrl.Text);
            QRCoder.PayloadGenerator.BezahlCode qrBank = new BezahlCode(BezahlCode.AuthorityType.contact_v2,
                this.TextBox_AccountName.Text, "", "", TextBox_IBAN.Text, TextBox_BIC.Text, "test https://area23.at/u/");
            QRCoder.PayloadGenerator.PhoneNumber qrPhone = new PhoneNumber(this.TextBox_QrPhone.Text);
            string qrString = String.Concat(qrUrl.ToString(), qrPhone.ToString(), qrBank.ToString());
            return qrString;
        }

        protected virtual string GetQrStringFromForm(System.Web.UI.HtmlControls.HtmlForm form)
        {
            return GetQrString();
        }


        protected override void SetQRImage(Bitmap qrImage)
        {
            MemoryStream ms = new MemoryStream();
            qrImage.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            this.ImgQR.Src = "data:image/gif;base64," + base64Data;
            this.ImageQr.ImageUrl = "data:image/gif;base64," + base64Data;
            ResetFormElements();
        }

    }
}