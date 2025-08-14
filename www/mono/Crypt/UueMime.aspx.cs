using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using System;
using System.Text;
using System.Web;


namespace Area23.At.Mono.Crypt
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
                    case "hex16":   encodedStr = Hex16.ToHex16(bytes); break;
                    case "base16":  encodedStr = Base16.ToBase16(bytes); break;
                    case "base32":  encodedStr = Base32.ToBase32(bytes); break;                    ;
                    case "uu":      encodedStr = Uu.UuEncode(srcStr); break;
                    case "html":    encodedStr = HttpUtility.HtmlEncode(srcStr); break;
                    case "url":     encodedStr = Server.UrlEncode(srcStr); break;
                    case "base64":
                    default:        encodedStr = Base64.ToBase64(bytes); break;
                }

                if (!string.IsNullOrEmpty(encodedStr))
                {
                    this.preOut.InnerText = encodedStr;
                    this.preOut.Style.Value = "margin-top: -4px; height: 332px;max-height: 332px; overflow:scroll; word-break: break-all; word-wrap: break-word; border: 0px hidden white;";
                    this.preOut.Visible = true;
                }
            }
        }

        protected void oFile_Submit(object sender, EventArgs e)
        {
            Button_UploadFile_Encode_Click(sender, e);
        }

        protected void Button_UploadFile_Encode_Click(object sender, EventArgs e)
        {
            string encodedStr = string.Empty;
            if (!String.IsNullOrEmpty(oFile.Value) && (oFile.PostedFile != null))
            {
                byte[] fileBytes = oFile.PostedFile.InputStream.ToByteArray();
                switch (this.DropDownList_EncodeType.SelectedValue.ToLower())
                {
                    case "hex16":       encodedStr = Hex16.ToHex16(fileBytes); break;
                    case "base16":      encodedStr = Base16.ToBase16(fileBytes); break;
                    case "base32":      encodedStr = Base32.ToBase32(fileBytes); break;
                    case "hex32":       encodedStr = Hex32.ToHex32(fileBytes); break;
                    case "uu":          encodedStr = Uu.ToUu(fileBytes, true, true); break;
                    case "html":        encodedStr = "Can't html encode a binary file!"; break;
                    case "url":         encodedStr = "Can't url encode a binary file!"; break;
                    case "base64":
                    default:            encodedStr = Base64.ToBase64(fileBytes); break;
                }

                if (!string.IsNullOrEmpty(encodedStr))
                {
                    this.preOut.InnerText = encodedStr;
                    this.preOut.Style.Value = "margin-top: -4px; height: 332px;max-height: 332px; overflow:scroll; word-break: break-all; word-wrap: break-word; border: 0px hidden white;";
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
                    case "hex16":
                        if (Hex16.IsValid(this.TextBoxSource.Text))
                            byteSrc = Hex16.Decode(srcStr);
                        else
                        {
                            this.preOut.InnerText = "Input Text is not valid hex16 string!";
                            this.preOut.Style.Value = "margin-top: -4px; height: 332px;max-height: 332px; overflow:scroll; word-break: break-all; word-wrap: break-word; border: 1px dashed red;";
                            return;
                        }
                        break;
                    case "base16":
                        if (Base16.IsValid(this.TextBoxSource.Text))
                            byteSrc = Base16.Decode(srcStr);
                        else
                        {
                            this.preOut.InnerText = "Input Text is not valid base16 string!";
                            this.preOut.Style.Value = "margin-top: -4px; height: 332px;max-height: 332px; overflow:scroll; word-break: break-all; word-wrap: break-word; border: 1px dashed red;";
                            return;
                        }
                        break;
                    case "base32":
                        if (Base32.IsValid(this.TextBoxSource.Text))
                            byteSrc = Base32.Decode(srcStr);
                        else
                        {
                            this.preOut.InnerText = "Input Text is not valid base32 string!";
                            this.preOut.Style.Value = "margin-top: -4px; height: 332px;max-height: 332px; overflow:scroll; word-break: break-all; word-wrap: break-word; border: 1px dashed red;";
                            return;
                        }
                        break;
                    case "hex32":
                        if (Hex32.IsValid(this.TextBoxSource.Text))
                            byteSrc = Hex32.Decode(srcStr);
                        else
                        {
                            this.preOut.InnerText = "Input Text is not valid base32 hex string!";
                            this.preOut.Style.Value = "margin-top: -4px; height: 332px;max-height: 332px; overflow:scroll; word-break: break-all; word-wrap: break-word; border: 1px dashed red;";
                            return;
                        }
                        break;
                    case "uu":
                        if (Uu.IsValid(this.TextBoxSource.Text))
                            decodedStr = Uu.UuDecode(srcStr);
                        else
                        {
                            this.preOut.InnerText = "Input Text is not valid uuencoded string!";
                            this.preOut.Style.Value = "margin-top: -4px; height: 332px;max-height: 332px; overflow:scroll; word-break: break-all; word-wrap: break-word; border: 1px dashed red;";
                            return;
                        }
                        break;
                    case "html": decodedStr = HttpUtility.HtmlDecode(srcStr); break;
                    case "url": decodedStr = Server.UrlDecode(srcStr); break;
                    case "base64":
                    default:
                        if (Base64.IsValidBase64(this.TextBoxSource.Text, out _))
                            byteSrc = EnDeCodeHelper.Decode(srcStr, EncodingType.Base64);
                        else
                        {
                            this.preOut.InnerText = "Input Text is not valid base64 string!";
                            this.preOut.Style.Value = "margin-top: -4px; height: 332px;max-height: 332px; overflow:scroll; word-break: break-all; word-wrap: break-word; border: 1px dashed red;";
                            return;
                        }
                        break;
                }

                if (byteSrc != null && byteSrc.Length > 0)
                    decodedStr = Encoding.UTF8.GetString(byteSrc);

                if (!string.IsNullOrEmpty(decodedStr))
                {
                    this.preOut.InnerText = decodedStr;
                    this.preOut.Style.Value = "margin-top: -4px; height: 332px;max-height: 332px; overflow:scroll; word-break: break-all; word-wrap: break-word; border: 0px hidden white;";
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