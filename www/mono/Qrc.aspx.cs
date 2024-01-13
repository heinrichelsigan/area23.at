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
using area23.at.www.mono.Util;

namespace area23.at.www.mono
{
    public partial class Qrc : QrBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ResetFormElements();
                GenerateQRImage();
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

        public Control[] TextControls
        {
            get
            {
                List<Control> list = new List<Control>();
                list.Add(this.TextBox_City);
                list.Add(this.TextBox_Coutry);
                list.Add(this.TextBox_Email);
                list.Add(this.TextBox_FirstName);
                list.Add(this.TextBox_LastName);
                list.Add(this.TextBox_Mobile);
                list.Add(this.TextBox_Note);
                list.Add(this.TextBox_Org);
                list.Add(this.TextBox_OrgTitle);
                list.Add(this.TextBox_Phone);
                list.Add(this.TextBox_Region);
                list.Add(this.TextBox_Street);
                list.Add(this.TextBox_StreetNr);
                list.Add(this.TextBox_Web);
                list.Add(this.TextBox_ZipCode);
                return list.ToArray();
            }
        }

        protected override void ResetChangedElements()
        {
            // base.ResetChangedElements();

            foreach (var ctrl in TextControls)
            {
                if (ctrl is TextBox)
                {
                    if (((TextBox)(ctrl)).BorderColor == Color.Red)
                    {
                        ((TextBox)(ctrl)).BorderColor = Color.Black;
                        ((TextBox)(ctrl)).BorderStyle = BorderStyle.Solid;
                    }
                }
            }
        }

        protected void Button_QRCode_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            GenerateQRImage();
        }
       
        protected override string GetQrString()
        {
            DateTime? dateTime = null;
            try
            {
                if (!string.IsNullOrEmpty(TextBox_Birthday.Text))
                    dateTime = DateTime.Parse(TextBox_Birthday.Text);
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }
            ContactData qrContact = new ContactData(ContactData.ContactOutputType.VCard3,
                TextBox_FirstName.Text, TextBox_LastName.Text, null,
                TextBox_Phone.Text, TextBox_Mobile.Text,
                null, TextBox_Email.Text, dateTime, TextBox_Web.Text,
                TextBox_Street.Text, TextBox_StreetNr.Text,
                TextBox_City.Text, TextBox_ZipCode.Text,
                TextBox_Coutry.Text, TextBox_Note.Text, TextBox_Region.Text,
                ContactData.AddressOrder.Default,
                TextBox_Org.Text, TextBox_OrgTitle.Text);

            string qrString = qrContact.ToString();
            return qrString;
        }



        protected override void GenerateQRImage(string qrString = "")
        {
            Bitmap aQrBitmap = null;
            try
            {
                qrString = GetQrString();
                Color c = Util.ColorFrom.FromHtml(this.TextBox_Color.Text);
                // 8lor.FromHtml()
                if (!string.IsNullOrEmpty(qrString))
                {
                    aQrBitmap = GetQRBitmap(qrString, c);
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