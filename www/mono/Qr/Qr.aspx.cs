﻿using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using static QRCoder.PayloadGenerator;

namespace Area23.At.Mono.Qr
{

    public partial class Qr : QrBase
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
            QRGenericString qrString = new Framework.Library.Util.QRGenericString(this.TextBox_QrString.Text);
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
            string qrUrlStr = "", qrGenStr = "", qrBankStr = "", qrPhoneStr = "";
            if (!string.IsNullOrEmpty(this.TextBox_QrUrl.Text))
            {
                QRCoder.PayloadGenerator.Url qrUrl = new QRCoder.PayloadGenerator.Url(this.TextBox_QrUrl.Text);
                qrUrlStr = qrUrl.ToString() + "\r\n";                
            }
            if (!string.IsNullOrEmpty(this.TextBox_QrString.Text))
            {
                QRGenericString qrGen = new QRGenericString(this.TextBox_QrString.Text);
                qrGenStr = qrGen.ToString() + "\r\n";
            }
            if (!string.IsNullOrEmpty(TextBox_AccountName.Text))
            {
                QRCoder.PayloadGenerator.BezahlCode qrBank = new BezahlCode(BezahlCode.AuthorityType.contact_v2,
                    TextBox_AccountName.Text, TextBox_AccountName.Text, "", TextBox_IBAN.Text, TextBox_BIC.Text, TextBox_Reason.Text);
                qrBankStr = qrBank.ToString() + "\r\n";
            }
            if (!string.IsNullOrEmpty(this.TextBox_QrPhone.Text))
            {
                QRCoder.PayloadGenerator.PhoneNumber qrPhone = new PhoneNumber(this.TextBox_QrPhone.Text);
                qrPhoneStr = qrPhone.ToString() + "\r\n";
                if (!qrPhoneStr.ToLower().StartsWith("tel") && !qrPhoneStr.ToLower().Contains("tel:"))
                { 
                    qrPhoneStr = "tel:" + qrPhoneStr;
                }
            }
            string qrString = String.Concat(qrGenStr, qrUrlStr, qrPhoneStr, qrBankStr);
            return qrString;
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
                short qrMode = Convert.ToInt16(this.DropDownListQrMode.SelectedValue);
                QRCoder.QRCodeGenerator.ECCLevel eccLevel = QRCoder.QRCodeGenerator.ECCLevel.Q;
                Enum.TryParse<QRCoder.QRCodeGenerator.ECCLevel>(DropDownListQrLevel.SelectedValue, out eccLevel);

                if (!string.IsNullOrEmpty(qrString))
                {
                    // aQrBitmap = GetQRBitmap(qrString, Constants.QrColor, Color.Transparent);
                    qrImgPath = GetQRImgPath(qrString, 
                        out qrWidth, 
                        this.input_color.Value, 
                        this.input_backcolor.Value, 
                        qrMode, 
                        eccLevel);
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