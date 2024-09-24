using Area23.At.Framework.Library.Core;
using Area23.At.Framework.Library.Core.EnDeCoding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Area23.At.WinForm.TWinFormCore
{
    public partial class EnDeCodingForm : TransparentFormCore8
    {
        public EnDeCodingForm()
        {
            InitializeComponent();
        }

        public EnDeCodingForm(bool encodeOnly = false, bool dragNDrop = false)
        {
            InitializeComponent();
            comboBoxCryptAlgo.Visible = false;
        }

        private void comboBoxEnDeCoding_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonEncode_Click(object sender, EventArgs e)
        {
            string srcStr = textBoxIn.Text;
            string encodedStr = string.Empty;
            if (!string.IsNullOrEmpty(srcStr) && comboBoxEnDeCoding.SelectedValue != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(srcStr);
                switch (this.comboBoxEnDeCoding.SelectedItem.ToString().ToLower())
                {
                    case "base32": encodedStr = Base32.ToBase32(bytes); break;
                    case "hex16": encodedStr = Hex.ToHex(bytes); break;
                    case "uu": encodedStr = Uu.UuEncode(srcStr); break;
                    case "html": encodedStr = HttpUtility.HtmlEncode(srcStr); break;
                    case "url": encodedStr = HttpUtility.UrlEncode(srcStr); break;
                    case "base64":
                    default: encodedStr = Base64.ToBase64(bytes); break;
                }

                if (!string.IsNullOrEmpty(encodedStr))
                {
                    this.textBoxOut.Text = encodedStr;
                    this.textBoxOut.Visible = true;
                }
            }
        }

        private void buttonDecode_Click(object sender, EventArgs e)
        {
            string srcStr = textBoxIn.Text;
            string decodedStr = string.Empty;
            byte[] byteSrc = null;

            if (!string.IsNullOrEmpty(srcStr) && comboBoxEnDeCoding.SelectedValue != null)
            {
                switch (this.comboBoxEnDeCoding.SelectedItem.ToString().ToLower())
                {
                    case "base32": byteSrc = Base32.FromBase32(srcStr); break;
                    case "hex16": byteSrc = Hex.FromHex(srcStr); break;
                    case "uu": decodedStr = Uu.UuDecode(srcStr); break;
                    case "html": decodedStr = HttpUtility.HtmlDecode(srcStr); break;
                    case "url": decodedStr = HttpUtility.UrlDecode(srcStr); break;
                    case "base64":
                    default: byteSrc = Base64.FromBase64(srcStr); break;
                }

                if (byteSrc != null && byteSrc.Length > 0)
                    decodedStr = Encoding.UTF8.GetString(byteSrc);

                if (!string.IsNullOrEmpty(decodedStr))
                {
                    this.textBoxOut.Text = decodedStr;
                    this.textBoxOut.Visible = true;
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string srcStr = textBoxIn.Text;
            string decodedStr = string.Empty;
            byte[] byteSrc = null;
            string? savedFile = null;

            if (!string.IsNullOrEmpty(srcStr) && comboBoxEnDeCoding.SelectedItem.ToString() != null)
            {
                switch (this.comboBoxEnDeCoding.SelectedItem.ToString().ToLower())
                {
                    case "base32": byteSrc = Base32.FromBase32(srcStr); break;
                    case "hex16": byteSrc = Hex.FromHex(srcStr); break;
                    case "uu": decodedStr = Uu.UuDecode(srcStr); break;
                    case "html": decodedStr = HttpUtility.HtmlDecode(srcStr); break;
                    case "url": decodedStr = HttpUtility.UrlDecode(srcStr); break;
                    case "base64":
                    default: byteSrc = Base64.FromBase64(srcStr); break;
                }

                if (byteSrc != null && byteSrc.Length > 0)
                {
                    string ext = MimeType.GetFileExtForMimeTypeApache(MimeType.GetMimeType(byteSrc, ".hex"));
                    string fName = AppDomain.CurrentDomain.BaseDirectory + LibPaths.SepChar + DateTime.Now.Area23Date() + "." + ext;
                    savedFile = base.SafeFileName(fName, byteSrc);
                }
            }
        }
    }
}
