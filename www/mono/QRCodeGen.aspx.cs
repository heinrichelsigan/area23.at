using QRCoder;
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
using System.Runtime.CompilerServices;

namespace area23.at.www.mono
{
    public partial class QRCodeGen : QrBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ResetFormElements();
                // GenerateQRImage();
            }
        }

        protected void QRCode_ParameterChanged(object sender, EventArgs e)
        {
            QRBase_ElementChanged(sender, e);
        }

        protected virtual void ResetFormElements()
        {
            base.ResetChangedElements();
        }

        protected void Button_QRCode_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            GenerateQRImage();
        }




        protected override void GenerateQRImage()
        {
            string qrStr = string.Empty;
            Bitmap aQrBitmap = null;
            try
            {
                qrStr = GetQrStringFromForm(((QRMaster)this.Master).MasterForm);
                if (!string.IsNullOrEmpty(qrStr))
                {
                    aQrBitmap = GetQRBitmap(qrStr);
                }
                if (aQrBitmap != null)
                {
                    SetQRImage(aQrBitmap);
                }
            }
            catch (Exception ex)
            {
                ErrorDiv.Visible = true;
                ErrorDiv.InnerHtml = "<p style=\"font-size: large; color: red\">" + ex.Message + "</p>\r\n" +
                    "<!-- " + ex.ToString() + " -->\r\n" +
                    "<!-- " + ex.StackTrace.ToString() + " -->\r\n";
            }
        }

        protected override string GetQrString()
        {
            ContactData qrContact = new ContactData(ContactData.ContactOutputType.VCard3,
                TextBox_FirstName.Text, TextBox_LastName.Text, null, TextBox_Phone.Text, TextBox_Mobile.Text,
                null, TextBox_Email.Text, null, TextBox_Web.Text, TextBox_Street.Text, TextBox_StreetNr.Text,
                TextBox_City.Text, TextBox_ZipCode.Text, TextBox_Coutry.Text, null,
                DropDown_Country.SelectedValue, ContactData.AddressOrder.Default, TextBox_Org.Text, null);

            string qrString = qrContact.ToString();
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
        }

    }
}