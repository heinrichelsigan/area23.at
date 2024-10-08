﻿using Area23.At.Framework.Library;
using Area23.At.Www.U;
using Area23.At.Www.U.Util;
using Newtonsoft.Json;
using QRCoder;
using static QRCoder.PayloadGenerator;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Security.Policy;
using System.Web.DynamicData;
using System.Windows.Media.Animation;

namespace Area23.At.Www.U
{
    public partial class Default : Area23BasePage
    {
        internal Dictionary<long, Utf8Symbol> symbolDict = null;
        internal Uri redirectUri = null;
        internal string hashKey = string.Empty;
        internal QRCodeGenerator.ECCLevel eCCLevel = QRCodeGenerator.ECCLevel.Q;
        internal short qrMode = 2;

        internal String ShortUrl { get => Constants.URL_SHORT + hashKey; }


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


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (symbolDict == null)
            {
                symbolDict = (Application[Constants.APP_NAME] != null) ? (Dictionary<long, Utf8Symbol>)Application[Constants.APP_NAME] : JsonHelper.GetUtf8Dictionary();
            }
            foreach (var symbol in symbolDict.Values)
            {
                ListItem item = new ListItem(symbol.HtmlCode, symbol.Utf8L.ToString());
                DropDown_Symbol.Items.Add(item);
            }
        }

        protected void TextBox_Search_TextChanged(object sender, EventArgs e)
        {
            Button_Search_Click(sender, e);
        }

        protected void Button_Search_Click(object sender, EventArgs e)
        {
            // TODO: implement it
        }

        protected void DropDown_Symbol_Changed(object sender, EventArgs e)
        {
            long lindex = -1;
            if (DropDown_Symbol.SelectedItem != null)
            {
                if (long.TryParse(DropDown_Symbol.SelectedItem.Value, out lindex))
                {
                    if (this.symbolDict != null && this.symbolDict.ContainsKey(lindex))
                    {
                        Utf8Symbol symbol = this.symbolDict[lindex];
                        if (symbol != null)
                        {
                            this.Literal_Symbol.Text = symbol.HtmlCode;
                            this.Literal_CodeSymbol.Text = symbol.HexCode;
                            this.Literal_CodeHtml.Text = symbol.HtmlEncoded;
                            this.Literal_HexCodeHtml.Text = symbol.HexHtmlEncoded;
                            this.TextBox_Number.Text = symbol.Utf8L.ToString();
                            this.TextBox_Name.Text = symbol.Name;
                        }
                    }
                }
            }
        }

        protected void DropDown_PixelPerUnit_Changed(object sender, EventArgs e) 
        {
            string qrModeStr = this.DropDown_PixelPerUnit.SelectedItem.Text;
            switch (qrModeStr)
            {
                case "1": qrMode = 1; break;
                case "2": qrMode = 2; break;
                case "3": qrMode = 3; break;
                case "4": qrMode = 4; break;
                case "6": qrMode = 6; break;
                case "8": qrMode = 8; break;
                default: qrMode = 2; break;
            }
            Button_Search_Click(sender, e);
        }

        protected void DropDown_QrMode_Changed(object sender, EventArgs e)
        {
            string eccModeStr = this.DropDown_QrMode.SelectedItem.Text;
            switch (eccModeStr)
            {
                case "L": eCCLevel = QRCodeGenerator.ECCLevel.L; break;
                case "M": eCCLevel = QRCodeGenerator.ECCLevel.M; break;
                case "Q": eCCLevel = QRCodeGenerator.ECCLevel.Q; break;
                case "H": eCCLevel = QRCodeGenerator.ECCLevel.H; break;
            }
            Button_Search_Click(sender, e);
        }


        protected virtual void ResetFormElements()
        {
            ResetChangedElements();
        }

        protected override void ResetChangedElements()
        {
            // base.ResetChangedElements();

            this.HrefShort.Visible = true;
            // this.HrefShort.Style["Border"]

            this.TextBox_Name.BorderColor = Color.Black;
            this.TextBox_Search.BorderStyle = BorderStyle.Solid;

            this.ErrorDiv.InnerHtml = string.Empty;
            this.ErrorDiv.Visible = false;
        }

        #region qrmembers

        protected override string GetQrString()
        {
            string qrStr = "";
            QRGenericString qrQStr = null;
            if (!string.IsNullOrEmpty(this.Literal_Symbol.Text))
            {
                qrQStr = new QRGenericString(this.Literal_Symbol.Text);
                qrStr = qrQStr.ToString();
            }
            
            return qrStr;
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

            if (this.Button_Search.Attributes["qrcolor"] != null)
                this.Button_Search.Attributes["qrcolor"] = Constants.QrColorString;
            else
                this.Button_Search.Attributes.Add("qrcode", Constants.QrColorString);

            try
            {
                Constants.QrColor = ColorFrom.FromHtml(this.input_color.Value);
                Constants.BackColor = ColorFrom.FromHtml(this.input_backcolor.Value);
                qrString = (string.IsNullOrEmpty(qrString)) ? GetQrString() : qrString;

                if (!string.IsNullOrEmpty(qrString))
                {
                    // aQrBitmap = GetQRBitmap(qrString, Constants.QrColor, Color.Transparent);
                    qrImgPath = GetQRImgPath(qrString, out qrWidth, this.input_color.Value, this.input_backcolor.Value, qrMode, eCCLevel);
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
                    "<pre>" + ex.ToString() + "</pre>\r\n" +
                    "<!-- " + ex.StackTrace.ToString() + " -->\r\n";
                ErrorDiv.Visible = true;
                
                    
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
            ResetChangedElements();
        }

        protected override void SetQrImageUrl(string imgPth, int qrWidth = -1)
        {
            this.ImageQr.Visible = true;
            this.ImageQr.BackColor = ColorFrom.FromHtml(this.input_backcolor.Value);
            this.ImageQr.ImageUrl = imgPth;
            if (qrWidth > 0)
                this.ImageQr.Width = qrWidth;
        }

        #endregion qrmembers


        ///// <summary>
        ///// VerifyUri
        ///// </summary>
        ///// <param name="redirUrl">url from which qr code is generated and where page redirects afer 8 sec</param>
        ///// <returns>Uri</returns>
        //protected virtual Uri VerifyUri(string redirUrl)
        //{
        //    if (string.IsNullOrEmpty(redirUrl))
        //    {
        //        ErrorDiv.InnerHtml = "<p style=\"font-size: large; color: red\">Url to shorten doesn't exist!</p>\r\n";
        //        ErrorDiv.Visible = true;
        //        this.TextBox_Search.BackColor = Color.Red;
        //        this.TextBox_Search.BorderStyle = BorderStyle.Dashed;
        //        this.TextBox_Search.BorderWidth = 1;
        //        return null;
        //    }                

        //    redirectUri = new Uri(redirUrl);
        //    if (!redirectUri.IsAbsoluteUri && redirUrl.Length < 4)
        //    {
        //        ErrorDiv.InnerHtml = "<p><span style=\"font-size: large; color: red\">" + redirUrl + "</span><br />isn't an AbsoluteUri!</p>\r\n";
        //        ErrorDiv.Visible = true;
        //        this.TextBox_Search.BackColor = Color.Red;
        //        this.TextBox_Search.BorderStyle = BorderStyle.Dotted;
        //        this.TextBox_Search.BorderWidth = 1;
        //        return null;
        //    }

        //    if (shortenMap.ContainsValue(redirectUri))
        //    {
        //        foreach (var mapEntry in shortenMap)
        //            if (mapEntry.Value == redirectUri)
        //                return redirectUri;
        //    }

        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(redirectUri.ToString());
        //        request.Method = "GET";
        //        request.Timeout = 2750;
        //        request.Headers.Add("accept-encoding", "gzip, deflate, br");
        //        request.Headers.Add("cache-control", "max-age=0");
        //        request.Headers.Add("accept-language", "en-US,en;q=0.9");
        //        request.UserAgent = "Apache2 mod_mono Amazon aws by https://area23.at/s/ to verify shortend url";
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Found || response.StatusCode == HttpStatusCode.Accepted)
        //        {
        //            response.Close();
        //            return redirectUri;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Area23Log.LogStatic(ex);                    
        //        ErrorDiv.InnerHtml = "<p style=\"font-size: large; color: red\">" + ex.Message + "</p>\r\n" +
        //            "<pre>" + ex.ToString() + "</pre>\r\n" +
        //            "<!-- " + ex.StackTrace.ToString() + " -->\r\n";
        //        ErrorDiv.Visible = true;
        //        if ((ex.Message.ToString().Contains("403")) ||
        //            (ex.Message.ToString().Contains("The operation has timed out.")))
        //            return redirectUri;

        //        redirectUri = null;
        //    }

        //    return redirectUri;
        //}


        /// <summary>
        /// ShortenUri - shortens a Uri to hash
        /// </summary>
        /// <param name="longUri">long Uri</param>
        /// <returns>true on successfully saved</returns>
        protected virtual bool Save(Utf8Symbol symbol)
        {            
            string shortHash = string.Empty;

            if (symbol != null)
            {
                if (!symbolDict.ContainsKey(symbol.Utf8L))
                    symbolDict.Add(symbol.Utf8L, symbol);

                if (!Utf8Dictionary.Uft8DictSingle.ContainsKey(symbol.Utf8L) || symbol.Name != Utf8Dictionary.Uft8DictSingle[symbol.Utf8L].Name)
                {
                    JsonHelper.SaveDictionaryToJson(symbolDict);
                    return true;
                }
            }
            return false;
        }

    }
}