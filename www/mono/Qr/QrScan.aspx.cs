using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using OnBarcode.Barcode.BarcodeScanner;
using QRCoder;
using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using static QRCoder.PayloadGenerator;

namespace Area23.At.Mono.Qr
{
    public partial class QrScan : QrBase
    {

        string base64Mime = string.Empty;

        internal long FileSizeLimit
        {
            get => ConfigurationManager.AppSettings["PDFMergeFileSizeLimitMB"] != null ?
                (int.Parse(ConfigurationManager.AppSettings["PDFMergeFileSizeLimitMB"]) * 1024 * 1024) : (1024 * 1024 * 10);
        }

        /// <summary>
        /// overriden init calls base <see cref="Page.OnInit(EventArgs)"/>
        /// </summary>
        /// <param name="e"><see cref="EventArgs">EventArgs e</see></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.ButtonUploadID.Attributes["name"] = "ButtonUploadName";
            oFile.Attributes["onchange"] = "UploadFile(this, " + ButtonUploadID.ClientID + ")";           
        }

        /// <summary>
        /// Page_Load page cycle in asp.net classic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                ButtonUpload_Click(sender, e);
            }
        }

        /// <summary>
        /// Event fired on ButtonUpload click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                if (Session[Constants.UPSAVED_FILE] != null && !string.IsNullOrEmpty(Session[Constants.UPSAVED_FILE].ToString()))
                    Session.Remove(Constants.UPSAVED_FILE);
                else
                    UploadFile(Request.Files[0]);
            }
        }


        /// <summary>
        /// Uploads a http posted file
        /// </summary>
        /// <param name="pfile"><see cref="HttpPostedFile"/></param>
        protected void UploadFile(HttpPostedFile pfile)
        {
            string filePath = "", fileName = "", fileExtn = "";
            LabelUpload.Visible = true;
            PanelUploadImage.Visible = true;
            LabelUpload.Text = "";
            aOrigImg.HRef = "#";
            aOrigImg.InnerText = "original qr image";
            aNewQr.HRef = "#";            
            aNewQr.InnerText = "new generated qr-code";
            
            
            ImgQrIn.Visible = false;
            ImgQrOut.Visible = false;

            if (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0))
            {
                fileExtn = Path.GetExtension(pfile.FileName).Trim().ToLower().TrimEnd();

                fileName = Path.GetFileName(pfile.FileName).BeautifyUploadFileNames();
                filePath = LibPaths.SystemDirOutPath + fileName;

                switch (fileExtn)
                {
                    case "gif":
                    case ".gif":
                    case ".bmp":
                    case "bmp":
                    case ".png":
                    case "png":
                    case ".jpg":
                    case "jpg":
                    case ".jpeg":
                    case "jepg":
                    case ".exif":
                    case "exif":
                    case ".tif":
                    case "tif": break;
                    default:
                        LabelUpload.Text = fileName + " " + fileExtn + " isn't an image file (.jpg, .gif, .png).";
                        LabelUpload.ToolTip = "Can't upload file " + fileName + ", because " + fileExtn + " isn't 'pdf'!";
                        return;
                }

                if (File.Exists(filePath))
                {
                    int j = 10;
                    while (File.Exists(filePath) & j < 500)
                    {
                        fileName = j + "_" + fileName;
                        filePath = LibPaths.SystemDirOutPath + fileName;
                        j += (j < 100) ? 1 : 3;
                    }
                }

                if (pfile.ContentLength > FileSizeLimit)
                {
                    LabelUpload.Text = "Maximum 8 MB (mega bytes) are allowed for upload. Discarded: " + fileName + "!";
                    LabelUpload.ToolTip = "You can merge bigger .pdf's by using pdfunite under linux / unix!";
                    return;
                }
                
                try
                {
                    pfile.SaveAs(filePath);
                } 
                catch (Exception exi)
                {
                    Area23Log.LogOriginMsgEx("QrScan.aspx.cs", "Exception when uploading and saving scanned Qr image", exi);
                }

                if (System.IO.File.Exists(filePath))
                {
                    LabelUpload.Text = fileName + " successfully uploaded.";
                    LabelUpload.ToolTip = "File " + fileName + " has been successfully uploaded.";

                    aOrigImg.HRef = $"../res/out/{fileName}";
                    aOrigImg.InnerText = fileName;

                    Session[Constants.UPSAVED_FILE] = filePath;
                    ImgQrIn.Src = $"../res/out/{fileName}";
                    ImgQrIn.Alt = fileName;
                    ImgQrIn.Border = 0;
                    ImgQrIn.Visible = true;
                    
                    
                    Bitmap qrBmp = GetQRBitmap(filePath);
                    SetQRImage(qrBmp, fileName);
                    return;
                }
            }            
            LabelUpload.Text = "Upload unsuccessfully!";
            LabelUpload.ToolTip = "Failed to upload file!";
        }



        protected override string GetQrString()
        {            
            string qrCodeString = "";            
            return qrCodeString;
        }

        protected virtual Bitmap GetQRBitmap(string ms)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            string[] qrCodeStrings = BarcodeScanner.Scan(ms, BarcodeType.QRCode);
            string qrCodeString = qrCodeStrings[0];
            for (int i = 0; i < 10; i++)
            {
                if (qrCodeString.Contains(i + "ttp://") || qrCodeString.Contains(i + "ttps://"))
                    qrCodeString = qrCodeString.Replace(i + "ttp://", "http://").Replace(i + "ttps://", "https://");
                if (qrCodeString.Contains(i + "EGIN"))
                    qrCodeString = qrCodeString.Replace(i + "EGIN", "BEGIN");
                if (qrCodeString.Contains(i + "ank"))
                    qrCodeString = qrCodeString.Replace(i + "ank://", "bank://");
                if (qrCodeString.Contains(i + "el:"))
                    qrCodeString = qrCodeString.Replace(i +"el:", "tel:");
            }
            

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeString, QRCodeGenerator.ECCLevel.Default);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            TextBoxQrDecoded.Text = qrCodeString;
            return qrCodeImage;
        }

        protected virtual void SetQRImage(Bitmap qrImage, string fileNameQr)
        {
            fileNameQr = "Qr-" + fileNameQr;
            aNewQr.InnerText = fileNameQr;
            aNewQr.HRef = $"../res/out/{fileNameQr}";
            string fileName = LibPaths.SystemDirOutPath + fileNameQr;
            MemoryStream ms = new MemoryStream();
            qrImage.Save(ms, ImageFormat.Gif);
            qrImage.Save(fileName, ImageFormat.Gif);
            byte[] buffer = ms.ToArray();
            base64Mime = Convert.ToBase64String(ms.ToArray());

            this.ImgQrOut.Visible = true;
            this.ImgQrOut.Alt = fileNameQr;
            this.ImgQrOut.Src = "data:image/gif;base64," + base64Mime;
        }


    }
}