using Area23.At.Framework.Library;
using Area23.At.Mono.Qr;
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
using System.Text;
// using System.Xml.Linq;
using System.Data.SqlTypes;
using System.Windows.Media.Imaging;
using Area23.At.Framework.Library.Util;

namespace Area23.At.Mono.Qr
{
    /// <summary>
    /// QrBase QrPage abstract base class
    /// </summary>
    public abstract class QrBase : System.Web.UI.Page
    {

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
            short qrmode = 2, QRCodeGenerator.ECCLevel ecclvl = QRCodeGenerator.ECCLevel.Q)
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
            string qrModStr = Enum.GetNames(typeof(QRCoder.QRCodeGenerator.ECCLevel))[qrmode % 4];
            // QRCodeGenerator.ECCLevel ecclvl = QRCodeGenerator.ECCLevel.Q;
            // Enum.TryParse(qrModStr, out ecclvl);
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
            string qrOutPath = LibPaths.SystemDirOutPath + qrfn + ".gif";
            QrImgPath = LibPaths.OutAppPath + qrfn + ".gif";

            // normal operation => save qrCodeImage to qrOutToPath
            string qrOutToPath = LibPaths.SystemDirOutPath + qrfn + "_11.gif";
            qrCodeImage.Save(qrOutToPath);

            MemoryStream gifStrm = new MemoryStream();
            qrCodeImage.Save(gifStrm, ImageFormat.Gif);
            string qrStringGif = qrString.Replace("\r", "").Replace("\n", " ").Replace("\t", " ");

            byte[] gifBytes = gifStrm.ToArray();
            List<byte> toByteList = new List<byte>();
            bool flagOnce = false;

            for (int bc = 0; bc < gifBytes.Length; bc++)
            {
                if ((gifBytes[bc] == (byte)0x21) &&         // (byte)((char)'!')
                    (gifBytes[bc + 1] == (byte)0xf9 ||        // 0xf9 ||
                        gifBytes[bc + 1] == (byte)0xfe) &&  // 0xfe
                    (gifBytes[bc + 2] == (byte)0x04) &&     // EOT
                    (gifBytes[bc + 3] == (byte)0x01) &&     // SOH                        
                    (gifBytes[bc + 4] == (byte)0x00) &&     // NUL
                    (0x00 == (byte)0x00))
                {
                    if (!flagOnce)
                    {
                        foreach (byte b in GifCommentBytes(qrStringGif))
                        {
                            toByteList.Add(b);
                        }
                        flagOnce = true;
                    }
                }

                if ((bc == gifBytes.Length - 1) &&      // only short check at end of gifBytes, 
                    ((gifBytes[bc - 1] == (byte)0x0) &&   // that penultimate byte is NUL
                        gifBytes[bc] == (byte)0x3b))    // and last byte of GIF is 0x3b
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
            // GifMetadataAdapter gifAdapter = new GifMetadataAdapter(qrOutPath);
            // var meta = gifAdapter.Metadata;
            return QrImgPath;
        }

        /// <summary>
        /// GenerateGifWithComment - hackish raw method to add a GIF-Comment
        /// </summary>
        /// <param name="codecImage">existing Bitmap to modify</param>
        /// <param name="gifComment">comment, that you want to insert in CompuServe GIF</param>
        /// <param name="saveFilePath">filepath to save modified GIF file with comment on filesysten with full directory path</param>
        /// <returns></returns>
        public Bitmap GenerateGifWithComment(Bitmap codecImage, string gifComment = null, string saveFilePath = null)
        {
            string outFullPath = System.AppDomain.CurrentDomain.BaseDirectory + System.IO.Path.PathSeparator;
            string outGenerated = outFullPath + DateTime.UtcNow.ToString("{yyyy-MM-dd_HH.mm.ss.fff}.gif");

            saveFilePath = saveFilePath ?? outGenerated;
            gifComment = gifComment ?? "";
            gifComment = gifComment.Trim("\r\b\a\v".ToCharArray());

            MemoryStream gifMemStream = new MemoryStream();     // create gifMemStream - a new MemoryStream()
            codecImage.Save(gifMemStream, ImageFormat.Gif);     // save codecImage to gifMemStream in ImageFormat.Gif
            byte[] imgGifBytes = gifMemStream.ToArray();        // get all byte[] imgGifBytes from gifMemStream

            List<byte> toByteList = new List<byte>();
            bool flagOnce = false;

            for (int bydx = 0; bydx < imgGifBytes.Length; bydx++)
            {
                // this kind of MIT Magick Cookie will detect position, where to insert before GIF-Comment
                //  insert GIF-Comment before <=  0x21 0xF9 0x04 0x01 0x00
                //  insert GIF-Comment before <=  0x21 0xFE 0x04 0x01 0x00
                if ((imgGifBytes[bydx] == (byte)0x21) &&        // 0x21 (byte)((char)'!')
                    (imgGifBytes[bydx + 1] == (byte)0xf9 ||     // 0xF9 | 0xFE
                        imgGifBytes[bydx + 1] == (byte)0xfe) &&
                    (imgGifBytes[bydx + 2] == (byte)0x04) &&    // 0x04     EOT
                    (imgGifBytes[bydx + 3] == (byte)0x01) &&    // 0x01     SOH                        
                    (imgGifBytes[bydx + 4] == (byte)0x00))      // 0x00     NUL
                {
                    if (!flagOnce)
                    {
                        foreach (byte b in GifCommentBytes(gifComment))
                        {
                            toByteList.Add(b);
                        }
                        flagOnce = true;
                    }
                }

                if ((bydx == imgGifBytes.Length - 1) &&         // only short check at end of gifBytes, 
                    ((imgGifBytes[bydx - 1] == (byte)0x0) &&    // that penultimate byte is NUL 
                        imgGifBytes[bydx] == (byte)0x3b))       // and last byte of GIF is 0x3b  
                {; } /* <= only breakpoint trigger */

                toByteList.Add(imgGifBytes[bydx]);               // Add next byte from gifBytes to ToByteList
            }

            // save original codecImage as Gif to filename $"{dateTime.UtcNow:yyyy-MM-dd_HH.mm.ss.fff}_original.gif"
            outGenerated = outFullPath + DateTime.UtcNow.ToString("{yyyy-MM-dd_HH.mm.ss.fff}_original.gif");
            codecImage.Save(outGenerated, ImageFormat.Gif);

            // open a FileStream and write (byte[])toByteList.ToArray() fully out => flush
            using (Stream fs = File.Open(saveFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(toByteList.ToArray(), 0, toByteList.Count);    // writes toByteList.Count byte[] from toByteList
                fs.Flush();
            }

            // GifMetadataAdapter gifAdapter = new GifMetadataAdapter(saveFilePath);
            // var meta = gifAdapter.Metadata;
            Bitmap gifMap = new Bitmap(saveFilePath);
            return gifMap;
        }


        /// <summary>
        /// GifCommentBytes - gets a byte[] for further comment to add to GIF
        /// </summary>
        /// <param name="c">GIF-Comment as <see cref="string"/></param>
        /// <returns><see cref="byte[]">array of byte</see></returns>
        protected virtual byte[] GifCommentBytes(string c)
        {
            if (string.IsNullOrEmpty(c))
                return new byte[0];

            List<byte> byteList = new List<byte>();     // create a new generic List<byte>()                   
            byteList.Add((byte)0x21);                   // Write first 0x21 '!' detection sequence
            byteList.Add((byte)0xfe);                   // Write 0xfe ((byte)254) as snd byte

            byte[] bytes = Encoding.UTF8.GetBytes(c);  // get byte[] from string ASCII
            byte b0 = Convert.ToByte(c.Length & 0xff);  // write first content length of now following comment
            if (c.Length > (int)0xff)                   // TODO: might be still buggy, if comment length >= 256 ;(
            {
                byte b1 = Convert.ToByte((c.Length >> 8) & 0xff);
                byteList.Add(b1);                       // add most significant byte  from content length to List<byte>
            }
            byteList.Add(b0);                           // finally add the least significant byte from content length to List<byte>

            foreach (byte b in bytes)                   // loop through all comment bytes
            {
                byteList.Add(b);                        // add byte per byte from byte[] comment to List<byte>
            }

            byteList.Add((byte)0x0);                    // add 0x00 as termination symbol to finish comment header in GIF

            return byteList.ToArray();
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
            QrImgPath = LibPaths.OutAppPath + qrfn;
            qrCodeImage.Save(LibPaths.SystemDirOutPath + qrfn);

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
    }
}