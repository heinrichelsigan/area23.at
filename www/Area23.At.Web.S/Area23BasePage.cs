using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Area23.At.Web.S.Util;
using Area23.At.Web.Util;
using QRCoder;

namespace Area23.At.Web.S
{
    public abstract class Area23BasePage : System.Web.UI.Page
    {
        protected System.Collections.Generic.Queue<string> mqueue = new Queue<string>();
        protected Uri area23URL = new Uri("https://area23.at/");
        protected Uri darkstarURL = new Uri("https://darkstar.work/");
        protected Uri gitURL = new Uri("https://github.com/heinrichelsigan/area23.at/");

        protected System.Globalization.CultureInfo locale;


        public System.Globalization.CultureInfo Locale
        {
            get
            {
                if (locale == null)
                {
                    try
                    {
                        string defaultLang = Request.Headers["Accept-Language"].ToString();
                        string firstLang = defaultLang.Split(',').FirstOrDefault();
                        defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                    catch (Exception)
                    {
                        locale = new System.Globalization.CultureInfo("en");
                    }
                }
                return locale;
            }
        }

        public string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        public string LogFile
        {
            get
            {
                string logAppPath = MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                if (!logAppPath.Contains("MarriageRisk"))
                    logAppPath += "MarriageRisk" + SepChar;
                logAppPath += "log" + SepChar + DateTime.UtcNow.ToString("yyyyMMdd") + "_" + "marriage_risk.log";
                return logAppPath;
            }
        }


        public String QrImgPath { get; protected set; }

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

        protected virtual Bitmap GetQRBitmap(string qrString, Color c, Color bg)
        {
            return GetQRBitmap(qrString, c.ToXrgb(), bg.ToXrgb());
        }


        protected virtual Bitmap GetQRBitmap(string qrString, Color c)
        {
            return GetQRBitmap(qrString, c.ToXrgb());
        }

        /// <summary>
        /// GetQRImgPath - gets path to saved QRImage from qrString
        /// </summary>
        /// <param name="qrString"></param>
        /// <param name="c"></param>
        /// <returns><see cref="Bitmap"/></returns>
        /// <exception cref="ArgumentNullException">thrown, when <paramref name="qrString"/> is null or ""</exception>
        protected virtual string GetQRImgPath(string qrString, out int qrWidth, string qrhex, string bghex = "#ffffff", 
            short qrmode = 4, QRCodeGenerator.ECCLevel ecclvl = QRCodeGenerator.ECCLevel.Q)
        {
            if (string.IsNullOrEmpty(qrString))
                throw new ArgumentNullException("qrString", "Error calling GetQRBitmap(qrString = null); qrString is null...");

            qrWidth = -1;
            if (String.IsNullOrEmpty(qrhex))
                qrhex = "#000000";
            Color colFg = ColorFrom.FromHtml(qrhex);
            Color colWh = ColorFrom.FromHtml("#ffffff");
            Byte rB, gB, bB;
            rB = (byte)(colFg.R + (byte)((colWh.R - colFg.R) / 2));
            gB = (byte)(colFg.G + (byte)((colWh.G - colFg.G) / 2));
            bB = (byte)(colFg.B + (byte)((colWh.B - colFg.B) / 2));
            Color colLg = Color.FromArgb(rB, gB, bB);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrString, ecclvl);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(qrmode);

            // qrCodeImage.MakeTransparent();
            var px0 = qrCodeImage.GetPixel(0, 0);
            var px1 = qrCodeImage.GetPixel(1, 1);

            if (qrCodeImage.Width <= 128)
                qrWidth = qrCodeImage.Width * 2;

            Color backGr = ColorFrom.FromHtml(bghex);

            for (int ix = 0; ix < qrCodeImage.Width; ix++)
            {
                for (int iy = 0; iy < qrCodeImage.Height; iy++)
                {
                    Color getCol = qrCodeImage.GetPixel(ix, iy);
                    if ((getCol.R == px0.R && getCol.G == px0.G && getCol.B == px0.B) ||
                        (((getCol.R + 1) == px0.R) && getCol.G == px0.G && getCol.B == px0.B) ||
                        ((getCol.R == px0.R) && (getCol.G + 1) == px0.G && getCol.B == px0.B) ||
                        ((getCol.R == px0.R) && getCol.G == px0.G && (getCol.B + 1) == px0.B) ||
                        (((getCol.R + 1) == px0.R) && (getCol.G + 1) == px0.G && getCol.B == px0.B) ||
                        (((getCol.R + 1) == px0.R) && getCol.G == px0.G && (getCol.B + 1) == px0.B) ||
                        ((getCol.R == px0.R) && (getCol.G + 1) == px0.G && (getCol.B + 1) == px0.B) ||
                        (((getCol.R + 1) == px0.R) && (getCol.G + 1) == px0.G && (getCol.B + 1) == px0.B))
                    {
                        qrCodeImage.SetPixel(ix, iy, Color.Transparent);
                    }
                    else
                    {
                        qrCodeImage.SetPixel(ix, iy, colFg);
                    }
                }
            }

            string qrfn = Constants.DateFile + DateTime.Now.Millisecond + ".png";
            QrImgPath = Paths.QrAppPath + qrfn;
            qrCodeImage.Save(Paths.QrDirPath + qrfn);

            return QrImgPath;
        }


        /// <summary>
        /// GetQrBitmap - gets a <see cref="Bitmap"/> from qrString
        /// </summary>
        /// <param name="qrString"></param>
        /// <param name="c"></param>
        /// <returns><see cref="Bitmap"/></returns>
        /// <exception cref="ArgumentNullException">thrown, when <paramref name="qrString"/> is null or ""</exception>
        protected virtual Bitmap GetQRBitmap(string qrString, string qrhex, string bghex = "#ffffff", short qrmode = 1)
        {
            if (string.IsNullOrEmpty(qrString))
                throw new ArgumentNullException("qrString", "Error calling GetQRBitmap(qrString = null); qrString is null...");

            if (String.IsNullOrEmpty(qrhex))
                qrhex = "#000000";
            Color colFg = ColorFrom.FromHtml(qrhex);
            Color colWh = ColorFrom.FromHtml("#ffffff");
            Byte rB, gB, bB;
            rB = (byte)(colFg.R + (byte)((colWh.R - colFg.R) / 2));
            gB = (byte)(colFg.G + (byte)((colWh.G - colFg.G) / 2));
            bB = (byte)(colFg.B + (byte)((colWh.B - colFg.B) / 2));
            Color colLg = Color.FromArgb(rB, gB, bB);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            string qrModStr = Enum.GetNames(typeof(QRCoder.QRCodeGenerator.ECCLevel))[qrmode % 4];
            QRCodeGenerator.ECCLevel ecclvl = QRCodeGenerator.ECCLevel.Q;
            // Enum.TryParse(qrModStr, out ecclvl);
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrString, ecclvl);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(1);

            // qrCodeImage.MakeTransparent();
            var px0 = qrCodeImage.GetPixel(0, 0);
            var px1 = qrCodeImage.GetPixel(1, 1);

            Color backGr = ColorFrom.FromHtml(bghex);

            for (int ix = 0; ix < qrCodeImage.Width; ix++)
            {
                for (int iy = 0; iy < qrCodeImage.Height; iy++)
                {
                    Color getCol = qrCodeImage.GetPixel(ix, iy);
                    if ((getCol.R == px0.R && getCol.G == px0.G && getCol.B == px0.B) ||
                        (((getCol.R + 1) == px0.R) && getCol.G == px0.G && getCol.B == px0.B) ||
                        ((getCol.R == px0.R) && (getCol.G + 1) == px0.G && getCol.B == px0.B) ||
                        ((getCol.R == px0.R) && getCol.G == px0.G && (getCol.B + 1) == px0.B) ||
                        (((getCol.R + 1) == px0.R) && (getCol.G + 1) == px0.G && getCol.B == px0.B) ||
                        (((getCol.R + 1) == px0.R) && getCol.G == px0.G && (getCol.B + 1) == px0.B) ||
                        ((getCol.R == px0.R) && (getCol.G + 1) == px0.G && (getCol.B + 1) == px0.B) ||
                        (((getCol.R + 1) == px0.R) && (getCol.G + 1) == px0.G && (getCol.B + 1) == px0.B))
                    {
                        // qrNewImg.SetPixel(ix, iy, Color.Transparent);
                        qrCodeImage.SetPixel(ix, iy, Color.Transparent);
                    }
                    else
                    {
                        qrCodeImage.SetPixel(ix, iy, colFg);
                        // qrNewImg.SetPixel(ix, iy, colFg);
                    }
                }
            }

            string qrfn = Constants.DateFile + DateTime.Now.Millisecond + ".png";
            QrImgPath = Paths.QrAppPath + qrfn;
            qrCodeImage.Save(Paths.QrDirPath + Constants.DateFile + DateTime.Now.Millisecond + ".png");

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

        protected virtual void SetQrImageUrl(string imgPth, int qrWidth = -1)
        {
            foreach (var ctrl in this.Controls)
            {
                if (ctrl is HtmlControl && ctrl is HtmlImage)
                {
                    ((HtmlImage)ctrl).Src = imgPth;
                    if (qrWidth > 0)
                        ((HtmlImage)ctrl).Width = qrWidth;
                    return;
                }
                if (ctrl is System.Web.UI.WebControls.Image)
                {
                    ((System.Web.UI.WebControls.Image)ctrl).ImageUrl = imgPth;
                    if (qrWidth > 0)
                        ((System.Web.UI.WebControls.Image)ctrl).Width = qrWidth;
                    return;
                }
            }
        }

        public virtual void InitURLBase()
        {
            area23URL = new Uri("https://area23.at/");
            darkstarURL = new Uri("https://darkstar.work/");
            gitURL = new Uri("https://github.com/heinrichelsigan/area23.at/");
        }

        public virtual void Log(string msg)
        {
            string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string fn = this.LogFile;
            File.AppendAllText(fn, preMsg + msg + "\r\n");
        }

    }

}