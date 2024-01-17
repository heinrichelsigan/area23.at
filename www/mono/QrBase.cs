using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using QRCoder;
using System.Web.UI.WebControls;
using System.Xml;
using System.Security.Cryptography;
using area23.at.www.mono.Util;

namespace area23.at.www.mono
{
    /// <summary>
    /// QrBase QrPage abstract base class
    /// </summary>
    public abstract class QrBase : System.Web.UI.Page
    {

        /// <summary>
        /// QRBase_ElementChanged Eventhandler to mark changed TextBoxes & DropDownLists
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected virtual void QRBase_ElementChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (sender is TextBox || sender is DropDownList)
                {
                    ((TextBox)(sender)).BorderColor = Color.Red;
                    ((TextBox)(sender)).BorderStyle = BorderStyle.Dashed;
                }
            }
        }

        /// <summary>
        /// Resets edited controls 
        /// </summary>
        protected virtual void ResetChangedElements()
        {
            foreach (var ctrl in this.Page.Controls)
            {
                if (ctrl is TextBox || ctrl is DropDownList)
                {
                    ((TextBox)(ctrl)).BorderColor = Color.Black;
                    ((TextBox)(ctrl)).BorderStyle = BorderStyle.Solid;
                }
            }
        }


        /// <summary>
        /// you must implement this method in each derived page
        /// </summary>
        /// <returns></returns>
        protected abstract string GetQrString();

        protected virtual void GenerateQRImage(string qrString = null)
        {
            Bitmap aQrBitmap = null;
            qrString = qrString ?? GetQrString();
            try
            {                
                if (!string.IsNullOrEmpty(qrString))
                {
                    aQrBitmap = GetQRBitmap(qrString, Color.Black);
                }
                if (aQrBitmap != null)
                {
                    SetQRImage(aQrBitmap);
                }
            }
            catch (Exception)
            {
                // ErrorDiv.Visible = true;
                // ErrorDiv.InnerHtml = "<p style=\"font-size: large; color: red\">" + ex.Message + "</p>\r\n" +
                //      "<!-- " + ex.ToString() + " -->\r\n" +
                //      "<!-- " + ex.StackTrace.ToString() + " -->\r\n";
                throw;
            }
        }

        /// <summary>
        /// GetQrBitmap - gets a <see cref="Bitmap"/> from qrString
        /// </summary>
        /// <param name="qrString"></param>
        /// <param name="c"></param>
        /// <returns><see cref="Bitmap"/></returns>
        /// <exception cref="ArgumentNullException">thrown, when <paramref name="qrString"/> is null or ""</exception>
        protected virtual Bitmap GetQRBitmap(string qrString, Color c)
        {
            if (string.IsNullOrEmpty(qrString))
                throw new ArgumentNullException("qrString", "Error calling GetQRBitmap(qrString = null); qrString is null...");

            if (c == null)
                c = Color.Black;
            Color lighter = Color.White; // Color.FromArgb((Color.White.R - (byte)(c.R / 2)), (Color.White.G - (byte)(c.G / 2)), (Color.White.B - (byte)(c.B / 2)));

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrString, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(4, c, Color.White, true);
                        
            // qrCodeImage.MakeTransparent();
            for (int ix = 0; ix < qrCodeImage.Width; ix++)
            {
                for (int iy = 0; iy < qrCodeImage.Height; iy++)
                {
                    Color getCol = qrCodeImage.GetPixel(ix, iy);
                    if ((getCol.R == Color.White.R && getCol.G == Color.White.G && getCol.B == Color.White.B) ||
                        (((getCol.R + 1) == Color.White.R) && getCol.G == Color.White.G && getCol.B == Color.White.B) ||
                        ((getCol.R == Color.White.R) && (getCol.G + 1) == Color.White.G && getCol.B == Color.White.B) ||
                        ((getCol.R == Color.White.R) && getCol.G == Color.White.G && (getCol.B + 1) == Color.White.B) ||
                        (((getCol.R + 1) == Color.White.R) && (getCol.G + 1) == Color.White.G && getCol.B == Color.White.B) ||
                        (((getCol.R + 1) == Color.White.R) && getCol.G == Color.White.G && (getCol.B + 1) == Color.White.B) ||
                        ((getCol.R == Color.White.R) && (getCol.G + 1) == Color.White.G && (getCol.B + 1) == Color.White.B) ||
                        (((getCol.R + 1) == Color.White.R) && (getCol.G + 1) == Color.White.G && (getCol.B + 1) == Color.White.B))
                        qrCodeImage.SetPixel(ix, iy, Color.Transparent);
                }
            }
            qrCodeImage.MakeTransparent();            
            
            return qrCodeImage;
        }


        protected virtual void SetQRImage(HtmlImage img, Bitmap qrImage)
        {
            if (img == null)
                throw new ArgumentNullException("img", "Error calling SetQRImage(HtmlImage img = null, Bitmap qrImage); img is null...");
            if (qrImage == null)
                throw new ArgumentNullException("qrImage", "Error calling SetQRImage(HtmlImage imgl, Bitmap qrImage = null); qrImage is null...");

            MemoryStream ms = new MemoryStream();
            qrImage.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            img.Src = "data:image/gif;base64," + base64Data;
        }

        protected virtual void SetQRImage(System.Web.UI.WebControls.Image image, Bitmap qrImage)
        {
            if (image == null)
                throw new ArgumentNullException("image", "Error calling SetQRImage(System.Web.UI.WebControls.Image image = null, Bitmap qrImage); image is null...");
            if (qrImage == null)
                throw new ArgumentNullException("qrImage", "Error calling SetQRImage(HtmlImage imgl, Bitmap qrImage = null); qrImage is null...");

            MemoryStream ms = new MemoryStream();
            qrImage.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            image.ImageUrl = "data:image/gif;base64," + base64Data;
        }


        /// <summary>
        /// Please overwrite SetQRImage by taking a look to <see cref="SetQRImage(HtmlImage, Bitmap)"/> 
        /// or <seealso cref="SetQRImage(System.Web.UI.WebControls.Image, Bitmap)"/>
        /// </summary>
        /// <param name="qrImage"><see cref="Bitmap">Bitmap qrImage</see></param>
        protected virtual void SetQRImage(Bitmap qrImage)
        {
            foreach (var ctrl in this.Controls)
            {
                if (ctrl is HtmlControl && ctrl is HtmlImage)
                {
                    SetQRImage((HtmlImage)ctrl, qrImage);
                    return;
                }
                if (ctrl is System.Web.UI.WebControls.Image)
                {
                    SetQRImage((System.Web.UI.WebControls.Image)ctrl, qrImage);
                    return;
                }
            }            
        }
    }
}