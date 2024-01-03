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

namespace area23.at.mono.test
{
    public partial class WebForm1 : System.Web.UI.Page
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
            if (sender != null)
            {
                if (sender is TextBox)
                {
                    ((TextBox)(sender)).BorderColor = Color.Red;
                    ((TextBox)(sender)).BorderStyle = BorderStyle.Dotted;
                }
                if (sender is DropDownList)
                {
                    ((DropDownList)(sender)).BorderColor = Color.Red;
                    ((DropDownList)(sender)).BorderStyle = BorderStyle.Dashed;
                }
            }
        }

        protected void Button_QRCode_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            GenerateQRImage();
        }


        protected virtual void ResetFormElements()
        {
            foreach (var ctrl in ((Area23)this.Master).MasterFrom.Controls)
            {
                if (ctrl is TextBox)
                {
                    ((TextBox)(ctrl)).BorderColor = Color.Black;
                    ((TextBox)(ctrl)).BorderStyle = BorderStyle.Solid;
                }
                if (ctrl is DropDownList)
                {
                    ((DropDownList)(ctrl)).BorderColor = Color.Black;
                    ((DropDownList)(ctrl)).BorderStyle = BorderStyle.Solid;
                }
            }
        }

        protected virtual void GenerateQRImage()
        {
            string qrStr = string.Empty;
            Bitmap aQrBitmap = null;
            try
            {
                qrStr = GetQrStringFromForm(((Area23)this.Master).MasterFrom);
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

        protected virtual string GetQrStringFromControl(System.Web.UI.Control control)
        {
            if (control == null)
                control = ((Area23)this.Master).MasterFrom as System.Web.UI.Control;

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
            return GetQrStringFromControl(((Area23)this.Master).MasterFrom);
        }

        protected virtual Bitmap GetQRBitmap(string qrString)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrString, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            return qrCodeImage;
        }

        protected virtual void SetQRImage(Bitmap qrImage)
        {
            MemoryStream ms = new MemoryStream();
            qrImage.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            this.ImgQR.Src = "data:image/gif;base64," + base64Data;
        }


    }
}