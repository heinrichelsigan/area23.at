using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static QRCoder.PayloadGenerator;


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
            base.ResetChangedElements();
        }

        protected void Button_QrUrl_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            GenerateQRImage();
        }

        protected void LinkButton_QrUrl_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            GenerateQRImage();
        }

        protected override string GetQrString()
        {
            QRCoder.PayloadGenerator.Url qrUrl = new QRCoder.PayloadGenerator.Url(this.TextBox_QrUrl.Text);

            string qrString = qrUrl.ToString();
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
        }

    }
}