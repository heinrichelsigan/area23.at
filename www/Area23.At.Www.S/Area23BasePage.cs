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
using Area23.At.Www.S.Util;
using Area23.At.Www.Common;
using QRCoder;
using Area23.At.Framework.Library;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Shapes;

namespace Area23.At.Www.S
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

        public string SepChar { get => System.IO.Path.DirectorySeparatorChar.ToString(); }

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
                    ((TextBox)(sender)).BorderColor = System.Drawing.Color.Red;
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
                    ((TextBox)(ctrl)).BorderColor = System.Drawing.Color.Black;
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
                    aQrBitmap = GetQRBitmap(qrString, System.Drawing.Color.Black);
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

        protected virtual Bitmap GetQRBitmap(string qrString, System.Drawing.Color c, System.Drawing.Color bg)
        {
            return GetQRBitmap(qrString, c.ToXrgb(), bg.ToXrgb());
        }


        protected virtual Bitmap GetQRBitmap(string qrString, System.Drawing.Color c)
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
            System.Drawing.Color colFg = ColorFrom.FromHtml(qrhex);
            System.Drawing.Color colWh = ColorFrom.FromHtml("#ffffff");
            Byte rB, gB, bB;
            rB = (byte)(colFg.R + (byte)((colWh.R - colFg.R) / 2));
            gB = (byte)(colFg.G + (byte)((colWh.G - colFg.G) / 2));
            bB = (byte)(colFg.B + (byte)((colWh.B - colFg.B) / 2));
            System.Drawing.Color colLg = System.Drawing.Color.FromArgb(rB, gB, bB);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrString, ecclvl);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(qrmode);

            // qrCodeImage.MakeTransparent();
            var px0 = qrCodeImage.GetPixel(0, 0);
            var px1 = qrCodeImage.GetPixel(1, 1);

            if (qrCodeImage.Width <= 128)
                qrWidth = qrCodeImage.Width * 2;

            System.Drawing.Color backGr = ColorFrom.FromHtml(bghex);

            for (int ix = 0; ix < qrCodeImage.Width; ix++)
            {
                for (int iy = 0; iy < qrCodeImage.Height; iy++)
                {
                    System.Drawing.Color getCol = qrCodeImage.GetPixel(ix, iy);
                    if ((getCol.R == px0.R && getCol.G == px0.G && getCol.B == px0.B) ||
                        (((getCol.R + 1) == px0.R) && getCol.G == px0.G && getCol.B == px0.B) ||
                        ((getCol.R == px0.R) && (getCol.G + 1) == px0.G && getCol.B == px0.B) ||
                        ((getCol.R == px0.R) && getCol.G == px0.G && (getCol.B + 1) == px0.B) ||
                        (((getCol.R + 1) == px0.R) && (getCol.G + 1) == px0.G && getCol.B == px0.B) ||
                        (((getCol.R + 1) == px0.R) && getCol.G == px0.G && (getCol.B + 1) == px0.B) ||
                        ((getCol.R == px0.R) && (getCol.G + 1) == px0.G && (getCol.B + 1) == px0.B) ||
                        (((getCol.R + 1) == px0.R) && (getCol.G + 1) == px0.G && (getCol.B + 1) == px0.B))
                    {
                        // qrCodeImage.SetPixel(ix, iy, System.Drawing.Color.Transparent);
                        qrCodeImage.SetPixel(ix, iy, backGr);
                    }
                    else
                    {
                        qrCodeImage.SetPixel(ix, iy, colFg);
                    }
                }
            }


            string qrfn = DateTime.UtcNow.Area23DateTimeWithMillis();
            string qrOutPath = Paths.QrDirPath + qrfn + ".gif";
            QrImgPath = Paths.QrAppPath + qrfn + ".gif";

            // normal operation => save qrCodeImage to qrOutToPath
            string qrOutToPath = Paths.QrDirPath + qrfn + "_11.gif";
            qrCodeImage.Save(qrOutToPath);

            MemoryStream gifStrm = new MemoryStream();
            qrCodeImage.Save(gifStrm, ImageFormat.Gif);
            string qrStringGif = qrString.Replace("\r", "").Replace("\n", " ").Replace("\t", " ");

            byte[] gifBytes = gifStrm.ToArray();
            List<byte> toByteList = new List<byte>();
            bool flagOnce = false;

            for (int bc = 0; bc < gifBytes.Length; bc++)
            {
                if ((gifBytes[bc] == (byte)0x21) && // ! 
                    (gifBytes[bc + 1] == (byte)0xf9 || gifBytes[bc + 1] == (byte)0xfe) &&
                    (gifBytes[bc + 2] == (byte)0x04) && // EOT
                    (gifBytes[bc + 3] == (byte)0x01) && // SOH                        
                    (gifBytes[bc + 4] == (byte)0x00) && // NUL
                    (0x00 == (byte)0x00))
                {
                    if (!flagOnce)
                    {
                        foreach (byte b in WriteGifComment(qrStringGif))
                        {
                            toByteList.Add(b);
                        }
                        flagOnce = true;
                    }
                }

                if ((bc == gifBytes.Length - 1) && (gifBytes[bc] != (byte)0x0) && (gifBytes[bc - 1] == (byte)0x0))
                {
                    flagOnce = true;
                }
                toByteList.Add(gifBytes[bc]);
            }

            using (Stream fs = File.Open(qrOutPath, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(toByteList.ToArray(), 0, toByteList.Count);
                fs.Flush();
            }

            // GifMetadataAdapter gifAdapter = new GifMetadataAdapter(qrOutPath, gifStrm);
            // gifAdapter.Metadata.Comment = qrString;
            // gifAdapter.Metadata.Title = qrfn;
            // gifAdapter.SaveAs(qrOutPath);

            // MemoryStream jpegMs = new MemoryStream();
            // qrCodeImage.Save(jpegMs, ImageFormat.Jpeg);
            // JpegMetadataAdapter jpegAdapter = new JpegMetadataAdapter(qrOutPath, jpegMs);
            // jpegAdapter.Metadata.Comment = qrString;
            // jpegAdapter.Metadata.Title = qrfn;
            // jpegAdapter.SaveAs(qrOutPath);

            //GifBitmapEncoder gifBitmapEncoder = new System.Windows.Media.Imaging.GifBitmapEncoder();
            //BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //    qrCodeImage.GetHbitmap(),
            //    IntPtr.Zero,
            //    Int32Rect.Empty,
            //    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            //Bitmap thumbNail = GetQRThumbNail(qrString, qrhex, bghex.ToLower(), qrmode);
            //BitmapSource thumbNailSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //    thumbNail.GetHbitmap(),
            //    IntPtr.Zero,
            //    Int32Rect.Empty,
            //    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            //BitmapMetadata meta = new 
            //meta.Comment = qrString;

            //List<ColorContext> contextLists = new List<ColorContext>();
            //contextLists.Add(new System.Windows.Media.ColorContext(ConvertPixelFormat(qrCodeImage.PixelFormat)));
            //ReadOnlyCollection<ColorContext> colorContexts = new ReadOnlyCollection<ColorContext>(contextLists);

            //gifBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource, thumbNailSource, meta, colorContexts));
            //using (Stream stream = File.Open(qrOutPath, FileMode.Create, FileAccess.ReadWrite))
            //{
            //    gifBitmapEncoder.Save(stream);
            //}


            // try
            // {
            //    MemoryStream gifStream = new MemoryStream();
            //    GifEncoder gifEncoder = new GifEncoder(gifStream, qrCodeImage.Width, qrCodeImage.Height, null);
            //    gifEncoder.AddFrame(qrCodeImage, 0, 0, null, qrString);
            //    gifEncoder.Flush();
            //    gifStream.ToByteArray().ToFile(Paths.QrDirPath, qrfn, ".gif");
            //    QrImgPath = Paths.QrAppPath + qrfn + ".gif";
            // } 
            // catch (Exception encodeGifEx)
            // {
            //    Area23Log.LogStatic(encodeGifEx);
            //    qrfn = DateTime.UtcNow.Area23DateTimeWithMillis();
            //    qrCodeImage.Save(Paths.QrDirPath + qrfn + ".gif", ImageFormat.Gif);
            //    QrImgPath = Paths.QrAppPath + qrfn + ".gif";
            // }

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
            System.Drawing.Color colFg = ColorFrom.FromHtml(qrhex);
            System.Drawing.Color colWh = ColorFrom.FromHtml("#ffffff");
            Byte rB, gB, bB;
            rB = (byte)(colFg.R + (byte)((colWh.R - colFg.R) / 2));
            gB = (byte)(colFg.G + (byte)((colWh.G - colFg.G) / 2));
            bB = (byte)(colFg.B + (byte)((colWh.B - colFg.B) / 2));
            System.Drawing.Color colLg = System.Drawing.Color.FromArgb(rB, gB, bB);

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

            System.Drawing.Color backGr = ColorFrom.FromHtml(bghex);

            for (int ix = 0; ix < qrCodeImage.Width; ix++)
            {
                for (int iy = 0; iy < qrCodeImage.Height; iy++)
                {
                    System.Drawing.Color getCol = qrCodeImage.GetPixel(ix, iy);
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
                        qrCodeImage.SetPixel(ix, iy, System.Drawing.Color.Transparent);
                    }
                    else
                    {
                        qrCodeImage.SetPixel(ix, iy, colFg);
                        // qrNewImg.SetPixel(ix, iy, colFg);
                    }
                }
            }

            string qrfn = DateTime.UtcNow.Area23DateTimeWithMillis();
            QrImgPath = Paths.QrAppPath + qrfn + ".gif";


            return qrCodeImage;
        }



        /// <summary>
        /// GetQrBitmap - gets a <see cref="Bitmap"/> from qrString
        /// </summary>
        /// <param name="qrString"></param>
        /// <param name="c"></param>
        /// <returns><see cref="Bitmap"/></returns>
        /// <exception cref="ArgumentNullException">thrown, when <paramref name="qrString"/> is null or ""</exception>
        protected virtual Bitmap GetQRThumbNail(string qrString, string qrhex, string bghex = "#ffffff", short qrmode = 1)
        {
            if (string.IsNullOrEmpty(qrString))
                throw new ArgumentNullException("qrString", "Error calling GetQRBitmap(qrString = null); qrString is null...");

            if (String.IsNullOrEmpty(qrhex))
                qrhex = "#000000";
            System.Drawing.Color colFg = ColorFrom.FromHtml(qrhex);
            System.Drawing.Color colWh = ColorFrom.FromHtml("#ffffff");
            Byte rB, gB, bB;
            rB = (byte)(colFg.R + (byte)((colWh.R - colFg.R) / 2));
            gB = (byte)(colFg.G + (byte)((colWh.G - colFg.G) / 2));
            bB = (byte)(colFg.B + (byte)((colWh.B - colFg.B) / 2));
            System.Drawing.Color colLg = System.Drawing.Color.FromArgb(rB, gB, bB);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            string qrModStr = Enum.GetNames(typeof(QRCoder.QRCodeGenerator.ECCLevel))[qrmode % 4];
            QRCodeGenerator.ECCLevel ecclvl = QRCodeGenerator.ECCLevel.M;
            // Enum.TryParse(qrModStr, out ecclvl);
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrString, ecclvl);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeThumbNail = qrCode.GetGraphic(1);

            // qrCodeImage.MakeTransparent();
            var px0 = qrCodeThumbNail.GetPixel(0, 0);
            var px1 = qrCodeThumbNail.GetPixel(1, 1);

            System.Drawing.Color backGr = ColorFrom.FromHtml(bghex);

            for (int ix = 0; ix < qrCodeThumbNail.Width; ix++)
            {
                for (int iy = 0; iy < qrCodeThumbNail.Height; iy++)
                {
                    System.Drawing.Color getCol = qrCodeThumbNail.GetPixel(ix, iy);
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
                        qrCodeThumbNail.SetPixel(ix, iy, System.Drawing.Color.Transparent);
                    }
                    else
                    {
                        qrCodeThumbNail.SetPixel(ix, iy, colFg);
                        // qrNewImg.SetPixel(ix, iy, colFg);
                    }
                }
            }

            string qrfn = DateTime.UtcNow.Area23DateTimeWithMillis();
            QrImgPath = Paths.QrAppPath + qrfn + ".gif";

            return qrCodeThumbNail;
        }


        protected virtual byte[] WriteGifComment(string comment)
        {
            if (string.IsNullOrEmpty(comment))
                return new byte[0];

            List<byte> byteList = new List<byte>();
            byte[] bytesComment = Encoding.ASCII.GetBytes(comment);
            byte b21 = (byte)0x21;
            byte bfe = (byte)0xfe;

            byteList.Add((byte)b21);
            byteList.Add((byte)bfe);

            byte b0 = Convert.ToByte(comment.Length & 0xff);
            byte b1 = Convert.ToByte((comment.Length >> 8) & 0xff);

            if (comment.Length > (int)0xff)
            {
                byteList.Add(b1);
                byteList.Add(b0);
            }
            else
                byteList.Add(b0);

            foreach (byte b in bytesComment)
            {
                byteList.Add(b);
            }
            byteList.Add((byte)0x0);

            return byteList.ToArray();
        }


        private static System.Windows.Media.PixelFormat ConvertPixelFormat(System.Drawing.Imaging.PixelFormat sourceFormat)
        {
            switch (sourceFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb: return PixelFormats.Bgr24;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb: return PixelFormats.Bgra32;
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb: return PixelFormats.Bgr32;
                case System.Drawing.Imaging.PixelFormat.Indexed: return PixelFormats.Indexed1;
                case System.Drawing.Imaging.PixelFormat.Format1bppIndexed: return PixelFormats.Indexed1;
                case System.Drawing.Imaging.PixelFormat.Format4bppIndexed: return PixelFormats.Indexed4;
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed: return PixelFormats.Indexed8;
                case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale: return PixelFormats.Gray16;
                case System.Drawing.Imaging.PixelFormat.Format16bppRgb555: return PixelFormats.Bgr555;
                case System.Drawing.Imaging.PixelFormat.Format16bppRgb565: return PixelFormats.Bgr565;
                case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555: return PixelFormats.Bgr101010;
                case System.Drawing.Imaging.PixelFormat.Format32bppPArgb: return PixelFormats.Pbgra32;
                case System.Drawing.Imaging.PixelFormat.Format48bppRgb: return PixelFormats.Rgb48;
                case System.Drawing.Imaging.PixelFormat.Format64bppArgb: return PixelFormats.Rgba64;
                case System.Drawing.Imaging.PixelFormat.Format64bppPArgb: return PixelFormats.Prgba64;
                case System.Drawing.Imaging.PixelFormat.Gdi:
                case System.Drawing.Imaging.PixelFormat.Alpha:
                case System.Drawing.Imaging.PixelFormat.PAlpha:
                case System.Drawing.Imaging.PixelFormat.Extended:
                case System.Drawing.Imaging.PixelFormat.Canonical:
                case System.Drawing.Imaging.PixelFormat.Undefined:
                case System.Drawing.Imaging.PixelFormat.Max:
                default:
                    return new System.Windows.Media.PixelFormat();
            }
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