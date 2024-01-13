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
        internal static List<Control> listControl = null;

        public Control[] TextControls
        {
            get
            {
                if (listControl == null)
                {
                    listControl = new List<Control>();
                    listControl.Add(this.TextBox_Birthday);
                    listControl.Add(this.TextBox_City);
                    listControl.Add(this.TextBox_Coutry);
                    listControl.Add(this.TextBox_Email);
                    listControl.Add(this.TextBox_FirstName);
                    listControl.Add(this.TextBox_LastName);
                    listControl.Add(this.TextBox_Mobile);
                    listControl.Add(this.TextBox_Note);
                    listControl.Add(this.TextBox_Org);
                    listControl.Add(this.TextBox_OrgTitle);
                    listControl.Add(this.TextBox_Phone);
                    listControl.Add(this.TextBox_Region);
                    listControl.Add(this.TextBox_Street);
                    listControl.Add(this.TextBox_StreetNr);
                    listControl.Add(this.TextBox_Web);
                    listControl.Add(this.TextBox_ZipCode);
                }

                return listControl.ToArray();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // this.TextBox_Color.Text = "#8c1157";
                // this.input_color.Value = "#8c1157";
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

        protected override void ResetChangedElements()
        {
            // base.ResetChangedElements();

            foreach (var ctrl in ((QRMaster)this.Master).MasterForm.Controls)
            {
                if (ctrl is TextBox)
                {
                    // if (((TextBox)(ctrl)).BorderColor == Color.Red)
                    // {
                    ((TextBox)(ctrl)).BorderColor = Color.Black;
                    ((TextBox)(ctrl)).BorderStyle = BorderStyle.Solid;
                    ((TextBox)(ctrl)).BorderWidth = 1;
                    // }
                }
            }

            foreach (var ctrl in TextControls)
            {
                if (((TextBox)(ctrl)).BorderColor == Color.Red)
                {
                    ((TextBox)(ctrl)).BorderColor = Color.Black;
                    ((TextBox)(ctrl)).BorderStyle = BorderStyle.Solid;
                    ((TextBox)(ctrl)).BorderWidth = 1;
                }
            }
        }


        protected void Button_QRCode_Click(object sender, EventArgs e)
        {
            ResetFormElements();
            GenerateQRImage();
        }


        protected override void GenerateQRImage(string qrString = "")
        {
            Bitmap aQrBitmap = null;

            if (string.IsNullOrEmpty(this.color1.Value))
                this.color1.Value = Constants.ColorString;
            else
                Constants.ColorString = this.color1.Value;

            if (string.IsNullOrEmpty(this.input_color.Value))
                this.input_color.Value = Constants.ColorString;
            else
                Constants.ColorString = this.input_color.Value;


            if (this.Button_QRCode.Attributes["qrcolor"] != null)
                this.Button_QRCode.Attributes["qrcolor"] = Constants.ColorString;
            else
                this.Button_QRCode.Attributes.Add("qrcode", Constants.ColorString);

            try
            {
                Constants.QrColor = Util.ColorFrom.FromHtml(this.color1.Value);
                qrString = GetQrString();

                if (!string.IsNullOrEmpty(qrString))
                {
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


        protected virtual string GetQrStringFromForm(System.Web.UI.HtmlControls.HtmlForm form)
        {
            return GetQrString();
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


        protected override void SetQRImage(Bitmap qrImage)
        {
            MemoryStream ms = new MemoryStream();
            qrImage.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            this.ImgQR.Src = "data:image/gif;base64," + base64Data;
        }

    }
}