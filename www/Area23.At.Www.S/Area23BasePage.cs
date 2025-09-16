using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Static;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Area23.At.Www.S
{
    public abstract class Area23BasePage : Area23.At.Framework.Library.Util.UIPage
    {
        protected System.Collections.Generic.Queue<string> mqueue = new Queue<string>();
        protected Uri area23URL = new Uri("https://area23.at/");
        protected Uri darkstarURL = new Uri("https://darkstar.work/");
        protected Uri gitURL = new Uri("https://github.com/heinrichelsigan/area23.at/");

        protected System.Globalization.CultureInfo locale;




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
            return DrawQrBitmap(qrString, c, bg, 2, 128);
        }


        protected virtual Bitmap GetQRBitmap(string qrString, Color c)
        {
            return GetQRBitmap(qrString, c.ToXrgb(), 1);
        }

        /// <summary>
        /// IsInLevenSteinDistance
        /// TODO: should be <see cref="Color">Color extension method</see> 
        /// </summary>
        /// <param name="px0"></param>
        /// <param name="disCol"></param>
        /// <returns>true, if this baseColor is inside LevenStein distance to Color disColor</returns>
        public static bool IsInLevenSteinDistance(Color baseCol, Color disCol)
        {
            if (disCol.R == baseCol.R && disCol.G == baseCol.G && disCol.B == baseCol.B) // exact match => return true;
                return true;
            
            Color lvstCol = disCol.FromRGB(disCol.R, disCol.G, disCol.B);

            for (int rls = 0; Math.Abs(rls) < 4; rls = (rls >= 0) ? (0 - (++rls)) : Math.Abs(rls))
            {
                for (int gls = 0; Math.Abs(gls) < 4; gls = (rls >= 0) ? (0 - (++gls)) : Math.Abs(gls))
                {
                    for (int bls = 0; Math.Abs(bls) < 4; bls = (bls >= 0) ? (0 - (++bls)) : Math.Abs(bls))
                    {
                        byte r = ((byte)(disCol.R + rls) >= 0xff) ? (byte)(0xff) : (byte)(disCol.R + rls);
                        byte g = ((byte)(disCol.G + gls) >= 0xff) ? (byte)(0xff) : (byte)(disCol.G + gls);
                        byte b = ((byte)(disCol.B + bls) >= 0xff) ? (byte)(0xff) : (byte)(disCol.B + bls);
                        lvstCol = lvstCol.FromRGB(r, g, b);

                        if ((((lvstCol.R + rls) == baseCol.R) && lvstCol.G == baseCol.G && lvstCol.B == baseCol.B) ||
                            ((lvstCol.R == baseCol.R) && (lvstCol.G + gls) == baseCol.G && lvstCol.B == baseCol.B) ||
                            ((lvstCol.R == baseCol.R) && lvstCol.G == baseCol.G && (lvstCol.B + bls) == baseCol.B) ||
                            (((lvstCol.R + rls) == baseCol.R) && (lvstCol.G + gls) == baseCol.G && lvstCol.B == baseCol.B) ||
                            (((lvstCol.R + rls) == baseCol.R) && lvstCol.G == baseCol.G && (lvstCol.B + bls) == baseCol.B) ||
                            ((lvstCol.R == baseCol.R) && (lvstCol.G + gls) == baseCol.G && (lvstCol.B + bls) == baseCol.B) ||
                            (((lvstCol.R + rls) == baseCol.R) && (lvstCol.G + gls) == baseCol.G && (lvstCol.B + bls) == baseCol.B))
                            return true;
                    }
                }
            }
            // not in LevenStein disctance
            return false; 
        }

        /// <summary>
        /// DrawQrBitmap - draws a <see cref="System.Drawing.Bitmap"/> containing a QrCode
        /// </summary>
        /// <param name="qrString">qrString, that should be generated as Bitmap</param>
        /// <param name="colorQr"><see cref="Color" />Color of QR pixels</param>
        /// <param name="transparent">true, to render background color transparent, false for white background</param>
        /// <returns><see cref="System.Drawing.Bitmap"/> containing Qr Code</returns>
        public virtual Bitmap DrawQrBitmap(string qrString, Color colorQr, bool transparent = true)
        {
            Color colorBg = (transparent) ? Color.Transparent : Color.White;
            return DrawQrBitmap(qrString, colorQr, colorBg, 2, 128);
        }

        internal virtual Bitmap DrawQrBitmap(string qrString, string qrColorRGBHex, short qrMode = 2, int minWidth = 128, bool transparent = true)
        {
            if (String.IsNullOrEmpty(qrColorRGBHex))
                qrColorRGBHex = "#000000";
            Color colorQr = ColorFrom.FromHtml(qrColorRGBHex);
            Color colorBg = (transparent) ? Color.Transparent : Color.White;
            return DrawQrBitmap(qrString, colorQr, colorBg, qrMode, minWidth);
        }

        /// <summary>
        /// DrawQrBitmap - draws a <see cref="System.Drawing.Bitmap"/> containing a QrCode
        /// </summary>
        /// <param name="qrString">qrString, that should be generated as Bitmap</param>
        /// <param name="qrColorRGBHex">qr color in form of #rrggbb hex string</param>
        /// <param name="backgroundColorRGBHex">background color in form of #rrggbb hex string</param>
        /// <param name="qrMode">pixels per Qr Point, default 2, usually 1 up to 8</param>
        /// <param name="minWidth">minimal width of generated bitmap</param>
        /// <returns><see cref="System.Drawing.Bitmap"/> containing Qr Code</returns>
        public virtual Bitmap DrawQrBitmap(string qrString, ref int minWidth, string qrColorRGBHex, string backgroundColorRGBHex = "#ffffff", 
            short qrMode = 2, QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.M)
        {
            if (String.IsNullOrEmpty(qrColorRGBHex) || qrColorRGBHex.Length != 7)
                qrColorRGBHex = "#000000";
            Color colorQr = ColorFrom.FromHtml(qrColorRGBHex);
            Color colorBg = Color.White;
            colorBg = (string.IsNullOrEmpty(backgroundColorRGBHex) || backgroundColorRGBHex.Length != 7 || backgroundColorRGBHex == "#ffffff") ? 
                Color.White : 
                ColorFrom.FromHtml(backgroundColorRGBHex);
            
            Bitmap qrBitmap = DrawQrBitmap(qrString, colorQr, colorBg, minWidth, qrMode, eccLevel);
            minWidth = qrBitmap.Width;
            return qrBitmap;
        }

        internal virtual Bitmap DrawQrBitmap(string qrString, Color colorQr, Color colorBg, short qrMode = 2, int minWidth = 128)
        {
            QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q;
            switch (qrMode)
            {
                case 0: eccLevel = QRCodeGenerator.ECCLevel.L; break;
                case 1: eccLevel = QRCodeGenerator.ECCLevel.M; break;
                case 2: eccLevel = QRCodeGenerator.ECCLevel.Q; break;
                default: eccLevel = QRCodeGenerator.ECCLevel.H; break;
            }

            return DrawQrBitmap(qrString, colorQr, colorBg, minWidth, qrMode, eccLevel);
        }

        internal virtual Bitmap DrawQrBitmap(string qrString, Color colorQr, Color colorBg, int minWidth = 128,
            short qrMode = 2, QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q)
        { 
          
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrString, eccLevel, true, true, QRCodeGenerator.EciMode.Utf8, -1);
            QRCode qrCode = new QRCode(qrCodeData);            
            Bitmap qrBitmap = qrCode.GetGraphic(qrMode);            

            while(qrBitmap.Width < minWidth)
            {
                if ((int)(eccLevel) < 3)
                {
                    eccLevel = (((int)eccLevel) < 2) ? QRCodeGenerator.ECCLevel.Q : QRCodeGenerator.ECCLevel.H;
                    qrCodeData = qrGenerator.CreateQrCode(qrString, eccLevel, true, true, QRCodeGenerator.EciMode.Utf8, -1);
                    qrCode = new QRCode(qrCodeData);
                }
                qrBitmap = qrCode.GetGraphic(++qrMode);
            }
            
            Color px0 = qrBitmap.GetPixel(0, 0);
            // int c1x = qrMode, c1y = qrMode, looped = 0;
            //Color c1 = qrBitmap.GetPixel(c1x, c1y);
            //while (px0.ToXrgb() != c1.ToXrgb() && looped < 4)
            //{
            //    if (c1x >= (qrBitmap.Width - qrMode) || c1y >= (qrBitmap.Height - qrMode))
            //    {
            //        c1x = (qrBitmap.Width % c1x);
            //        c1y = (qrBitmap.Height & c1y);
            //        ++looped;
            //    }                                
            //    c1 = qrBitmap.GetPixel(++c1x, ++c1y);
            //}

            for (int ix = 0; ix < qrBitmap.Width; ix++)
            {
                for (int iy = 0; iy < qrBitmap.Height; iy++)
                {
                    Color bmpPixelCol = qrBitmap.GetPixel(ix, iy);
                    if (px0.IsInLevenSteinDistance(bmpPixelCol))
                    {
                        qrBitmap.SetPixel(ix, iy, colorBg);
                        // qrBitmap.SetPixel(ix, iy, Color.Transparent);
                    }
                    else
                    {
                        qrBitmap.SetPixel(ix, iy, colorQr);                        
                    }
                }
            }

            return qrBitmap;
        }

        /// <summary>
        /// GetQRImgPath - returns an relative application path to a new generated qr gif format image
        /// </summary>
        /// <param name="qrString">qrString, that should be generated as Bitmap</param>
        /// <param name="minWidth">minimal width of generated bitmap</param>
        /// <param name="qrColorRGBHex">qr color in form of #rrggbb hex string</param>
        /// <param name="qrMode">pixels per Qr Point, default 2, usually 1 up to 8</param>
        /// <returns>relative image path to generated Qr code gif</returns>
        protected virtual string GetQRImgPath(string qrString, ref int minWidth, string qrColorRGBHex, string backgroundColorRGBHex = "#ffffff",
            short qrMode = 2, QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.Q)
        {
            if (string.IsNullOrEmpty(qrString))
                throw new ArgumentNullException("qrString", "Error calling GetQRBitmap(qrString = null); qrString is null...");

            if (string.IsNullOrEmpty(qrColorRGBHex) || qrColorRGBHex.Length < 6 || qrColorRGBHex.Length > 7)
                qrColorRGBHex = "#000000";

            Bitmap qrCodeImage = DrawQrBitmap(qrString, ref minWidth, qrColorRGBHex, backgroundColorRGBHex, qrMode, eccLevel);

            string datePrefix = DateTime.UtcNow.Area23DateTimeWithMillis();
            string gifFileName = datePrefix + ".gif";
            string gifInverseFileName = datePrefix + "_i.gif";

            Color qrc0 = qrColorRGBHex.FromHtmlToColor();
            Color back0 = backgroundColorRGBHex.FromHtmlToColor();
            Color qrc1 = qrc0.FromRGB((byte)((byte)0xff - qrc0.R), (byte)((byte)0xff - qrc0.G), (byte)((byte)0xff - qrc0.B));
            Color back1 = back0.FromRGB((byte)((byte)0xff - back0.R), (byte)((byte)0xff - back0.G), (byte)((byte)0xff - back0.B));
            Bitmap qrCodeImageInverse = DrawQrBitmap(qrString, ref minWidth, qrc1.ToXrgb(), back1.ToXrgb(), qrMode, eccLevel);


            //TimeSpan twait1 = new TimeSpan(0, 0, 2);
            //TimeSpan twait = new TimeSpan(0, 0, 2);
            //GifEncoder enc = new GifEncoder(qrCodeImageInverse, 2, twait1);
            //enc.AddFrame(qrCodeImage, twait);
            //enc.Finish();
            //byte[] gifEncBytes = enc.GifData;
            //using (Stream fs1 = File.Open(Paths.QrDirPath + gifInverseFileName, FileMode.Create, FileAccess.ReadWrite))
            //{
            //    fs1.Write(gifEncBytes, 0, gifEncBytes.Length);
            //    fs1.Flush();
            //}

            // normal operation => save qrCodeImage to qrOutToPath
            // qrCodeImage.Save(Paths.QrDirPath + qrfn + "_11.gif");

            QrImgPath = LibPaths.QrAppPath + gifFileName;
            // BytesToWithGifComment(LibPaths.QrDirPath + gifFileName, gifEncBytes, qrString);
            SaveWithGifComment(LibPaths.SystemDirQrPath + gifInverseFileName, qrCodeImageInverse, qrString);
            SaveWithGifComment(LibPaths.SystemDirQrPath + gifFileName, qrCodeImage, qrString);            

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
        /// GetQRBitmap - generated a a <see cref="System.Drawing.Bitmap"/> containing a QR Code
        /// </summary>
        /// <param name="qrString">qrString, that should be generated as Bitmap</param>
        /// <param name="qrColorRGBHex">qr color in form of #rrggbb hex string</param>
        /// <param name="qrMode">pixels per Qr Point, default 2, usually 1 up to 8</param>
        /// <returns>a <see cref="System.Drawing.Bitmap"/></returns>
        public virtual Bitmap GetQRBitmap(string qrString, string qrColorRGBHex, short qrMode = 1)
        {
            if (string.IsNullOrEmpty(qrString))
                throw new ArgumentNullException("qrString", "Error calling GetQRBitmap(qrString = null); qrString is null...");

            if (string.IsNullOrEmpty(qrColorRGBHex) || qrColorRGBHex.Length < 6 || qrColorRGBHex.Length > 7)
                qrColorRGBHex = "#000000";
            
            return DrawQrBitmap(qrString, qrColorRGBHex, qrMode, 128, true);            
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
            Color colFg = ColorFrom.FromHtml(qrhex);
            Color colWh = ColorFrom.FromHtml("#ffffff");
            Byte rB, gB, bB;
            rB = (byte)(colFg.R + (byte)((colWh.R - colFg.R) / 2));
            gB = (byte)(colFg.G + (byte)((colWh.G - colFg.G) / 2));
            bB = (byte)(colFg.B + (byte)((colWh.B - colFg.B) / 2));
            Color colLg = Color.FromArgb(rB, gB, bB);

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

            Color backGr = ColorFrom.FromHtml(bghex);

            for (int ix = 0; ix < qrCodeThumbNail.Width; ix++)
            {
                for (int iy = 0; iy < qrCodeThumbNail.Height; iy++)
                {
                    Color getCol = qrCodeThumbNail.GetPixel(ix, iy);
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
                        qrCodeThumbNail.SetPixel(ix, iy, Color.Transparent);
                    }
                    else
                    {
                        qrCodeThumbNail.SetPixel(ix, iy, colFg);
                        // qrNewImg.SetPixel(ix, iy, colFg);
                    }
                }
            }

            string qrfn = DateTime.UtcNow.Area23DateTimeWithMillis();
            QrImgPath = LibPaths.QrAppPath + qrfn + ".gif";

            return qrCodeThumbNail;
        }


        protected virtual void SaveWithGifComment(string fullOutFilePath, Bitmap bitmap, string comment)
        {
            MemoryStream gifStrm = new MemoryStream();
            bitmap.Save(gifStrm, ImageFormat.Gif);
            byte[] gifBytes = gifStrm.ToArray();
            BytesToWithGifComment(fullOutFilePath, gifBytes, comment);
        }

        protected virtual void BytesToWithGifComment(string fullOutFilePath, byte[] gifBytes, string comment)
        {

            string gifComment = comment.Replace("\r", "").Replace("\n", " ").Replace("\t", " ");
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
                    if (!flagOnce && !string.IsNullOrEmpty(gifComment))
                    {
                        byte[] bytesComment = Encoding.ASCII.GetBytes(gifComment);
                        toByteList.Add((byte)0x21);
                        toByteList.Add((byte)0xfe);

                        byte b0 = Convert.ToByte(gifComment.Length & 0xff);
                        byte b1 = Convert.ToByte((gifComment.Length >> 8) & 0xff);

                        if (gifComment.Length > (int)0xff)
                            toByteList.Add(b1);
                        toByteList.Add(b0);

                        foreach (byte b in bytesComment)
                        {
                            toByteList.Add(b);
                        }
                        toByteList.Add((byte)0x0);
                    }
                }

                if ((bc == gifBytes.Length - 1) && (gifBytes[bc] != (byte)0x0) && (gifBytes[bc - 1] == (byte)0x0))
                {
                    flagOnce = true;
                }
                toByteList.Add(gifBytes[bc]);
            }

            byte[] outBytes = toByteList.ToArray();
            using (Stream fs = File.Open(fullOutFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(outBytes, 0, outBytes.Length);
                fs.Flush();
            }

            return ;
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



        public override void Log(string msg)
        {
            base.Log(msg);
        }

    }

}