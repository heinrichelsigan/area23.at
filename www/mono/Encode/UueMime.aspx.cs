using Area23.At.Framework.Library.EnDeCoding;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace Area23.At.Mono.Encode
{
    public partial class UueMime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LinkButton_Encode_Click(object sender, EventArgs e)
        {
            string srcStr = TextBoxSource.Text;
            string encodedStr = string.Empty;
            if (!string.IsNullOrEmpty(srcStr))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(srcStr);
                switch (this.DropDownList_EncodeType.SelectedValue.ToLower())
                {
                    case "base32":  encodedStr = Base32.ToBase32(bytes); break;
                    case "hex16":   encodedStr = Hex.ToHex(bytes); break;
                    case "uu":      encodedStr = Uu.UuEncode(srcStr); break;
                    case "html":    encodedStr = HttpUtility.HtmlEncode(srcStr); break;
                    case "url":     encodedStr = Server.UrlEncode(srcStr); break;
                    case "base64":
                    default:        encodedStr = Base64.ToBase64(bytes); break;
                }

                if (!string.IsNullOrEmpty(encodedStr))
                {
                    this.preOut.InnerText = encodedStr;
                    this.preOut.Visible = true;
                }
            }
        }

        protected void LinkButton_Decode_Click(object sender, EventArgs e)
        {
            string srcStr = TextBoxSource.Text;
            string decodedStr = string.Empty;
            byte[] byteSrc = null;

            if (!string.IsNullOrEmpty(srcStr))
            {                
                switch (this.DropDownList_EncodeType.SelectedValue.ToLower())
                {
                    case "base32":  byteSrc = Base32.FromBase32(srcStr); break;
                    case "hex16":   byteSrc = Hex.FromHex(srcStr); break;
                    case "uu":      decodedStr = Uu.UuDecode(srcStr); break;
                    case "html":    decodedStr = HttpUtility.HtmlDecode(srcStr); break;
                    case "url":     decodedStr = Server.UrlDecode(srcStr); break;
                    case "base64":
                    default:        byteSrc = Base64.FromBase64(srcStr); break;
                }

                if (byteSrc != null && byteSrc.Length > 0)
                    decodedStr = Encoding.UTF8.GetString(byteSrc);

                if (!string.IsNullOrEmpty(decodedStr))
                {
                    this.preOut.InnerText = decodedStr;
                    this.preOut.Visible = true;
                }
            }
        }

        protected void Button_Clear_Click(object sender, EventArgs e)
        {
            this.preOut.InnerText = string.Empty;
            this.preOut.Visible = false;
            this.TextBoxSource.Text = string.Empty;
        }
    }
}