using Area23.At.Mono.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.WebRequestMethods;

namespace Area23.At.Mono
{
    public partial class SAES_En_Decrypt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                buttonUpload_Click(sender, e);
            }
        }

        protected string ResFoler
        {
            get
            {
                string strFolder = Server.MapPath("./res/");
                // Create the directory if it does not exist.
                if (!Directory.Exists(strFolder))
                    Directory.CreateDirectory(strFolder);
                return strFolder;
            }
        }

        protected void ReTransformImage(string imgFileName = null)
        {
            System.Drawing.Bitmap x1Image = null;

            byte b0 = Color.Transparent.R;
            byte b1 = Color.Transparent.G;
            byte b2 = Color.Transparent.B;

            List<Byte> bList = new List<byte>();

            if (!string.IsNullOrEmpty(imgFileName) && System.IO.File.Exists(imgFileName))
                x1Image = new Bitmap(imgFileName, false);

            List<long> transList = new List<long>();
            bool shouldBreak = false;
            int h = x1Image.Height;
            int w = x1Image.Width;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    Color c = x1Image.GetPixel(x, y);
                    if ((y > (h - 2)) || (x * y > ((h * w) - w)))
                    {
                        if (c.R == Color.Transparent.R && c.G == Color.Transparent.G && c.B == Color.Transparent.B)
                        {
                            transList.Add((x1Image.Width * y) + x);

                            int trCnt = transList.Count;
                            if (trCnt > 3 &&
                                transList[trCnt - 4] == (transList[trCnt - 3] - 1) &&
                                transList[trCnt - 3] == (transList[trCnt - 2] - 1) &&
                                transList[trCnt - 2] == (transList[trCnt - 1] - 1))
                            {
                                while (bList[bList.Count - 1] == Color.Transparent.R)
                                {
                                    bList.RemoveAt(bList.Count - 1);
                                }
                                shouldBreak = true; break;
                            }
                        }
                    }

                    if (!shouldBreak)
                    {
                        bList.Add(c.R);
                        bList.Add(c.G);
                        bList.Add(c.B);
                    }
                }
                if (shouldBreak) break;
            }

            string mimeType = ByteArrayToFile(bList.ToArray());   
            string base64Data = Convert.ToBase64String(bList.ToArray());
            imgOut.Src = "data:" + mimeType + ";base64," + base64Data; 
        }


        protected void TransformImage(byte[] data, string imgFileName = null)
        {
            System.Drawing.Bitmap x1Image = null;
            System.Drawing.Bitmap fromBytesTransImage = null;

            if (!string.IsNullOrEmpty(imgFileName) && System.IO.File.Exists(imgFileName))
                x1Image = new Bitmap(imgFileName);

            double dlen = (int)(data.Length / 3) + 1;
            int imgLen = (int)Math.Round(Math.Sqrt(dlen), MidpointRounding.AwayFromZero) + 1;

            fromBytesTransImage = new Bitmap(imgLen, imgLen);

            int x = 0, y = 0, c = 0;
            byte b0, b1, b2;
            for (int xy = 0; xy < imgLen * imgLen; xy++)
            {
                b0 = Color.Transparent.R;
                b1 = Color.Transparent.G;
                b2 = Color.Transparent.B;
                if (c < data.Length)
                {
                    try
                    {
                        b0 = data[c];
                        b1 = data[c + 1];
                        b2 = data[c + 2];
                    }
                    catch (Exception e)
                    {
                        Area23Log.LogStatic(e);
                    }
                }
                c += 3;
                x = xy % imgLen;
                if (xy > 0 && ((xy % imgLen) == 0))
                    y++;
                Color col = Color.FromArgb(Convert.ToInt16(b0), Convert.ToInt16(b1), Convert.ToInt16(b2));
                fromBytesTransImage.SetPixel(x, y, col);                
            }


            MemoryStream ms = new MemoryStream();
            fromBytesTransImage.Save(ms, ImageFormat.Png);
            string base64Data = Convert.ToBase64String(ms.ToArray());
            imgOut.Src = "data:image/png;base64," + base64Data;

        }


        protected void buttonTransUpload_Click(object sender, EventArgs e)
        {
            if (Request.Files != null && Request.Files.Count > 0)
                TransformUploadFile(Request.Files[0]);
        }


        protected void btnUploadTrans_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(oFile.Value))
            {
                TransformUploadFile(oFile.PostedFile);
            }
        }

        protected void TransformUploadFile(HttpPostedFile pfile)
        {
            string strFilePath;
            // Get the name of the file that is posted.
            string strFileName = pfile.FileName;
            strFileName = Path.GetFileName(strFileName);
            lblUploadResult.Text = "";

            if (pfile != null)
            {
                // Save the uploaded file to the server.
                strFilePath = ResFoler + strFileName;
                while (System.IO.File.Exists(strFilePath))
                {
                    string newFileName = strFilePath.Contains(Constants.DateFile) ?
                        Constants.DateFile + Guid.NewGuid().ToString() + "_" + strFileName :
                        Constants.DateFile + strFileName;
                    strFilePath = ResFoler + newFileName;
                    lblUploadResult.Text = String.Format("{0} already exists on server, saving it to {1}.",
                        strFileName, newFileName);
                }

                pfile.SaveAs(strFilePath);
                if (string.IsNullOrEmpty(lblUploadResult.Text))
                    lblUploadResult.Text = strFileName + " has been successfully uploaded.";

                byte[] fileBytes = pfile.InputStream.ToByteArray();
                string base64Data = Convert.ToBase64String(fileBytes);
                imgIn.Src = "data:image;base64," + base64Data;
                
                System.Drawing.Bitmap x1Image = null;
                if (!string.IsNullOrEmpty(strFilePath) && System.IO.File.Exists(strFilePath))
                {
                    x1Image = new Bitmap(strFilePath);
                    if (x1Image.Width == x1Image.Height)
                        ReTransformImage(strFilePath);
                    else
                        TransformImage(fileBytes, strFilePath);
                }                
            }
            else
            {
                lblUploadResult.Text = "Click 'Browse' to select the file to upload.";
            }


            // Display the result of the upload.
            frmConfirmation.Visible = true;
        }


        protected void buttonUpload_Click(object sender, EventArgs e)
        {
            if (Request.Files != null && Request.Files.Count > 0)
                TransformUploadFile(Request.Files[0]);
        }

        protected void btnUploadRe_Click(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(oFile.Value))
            {
                TransformUploadFile(oFile.PostedFile);
            }            
        }

        public static string GetMimeTypeForImageBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
            {
                return ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == img.RawFormat.Guid).MimeType;
            }
        }

        public string ByteArrayToFile(byte[] bytes, string fileName = null)
        {
            string strPath = ResFoler;

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Constants.DateFile + Guid.NewGuid().ToString();
            }
            string ext = "tmp";
            try
            {
                string mimeTypeExt = MimeType.GetMimeType(bytes, strPath + fileName);
                ext = MimeType.GetFileExtForMimeTypeApache(mimeTypeExt);
                // GetMimeTypeForImageBytes(bytes);
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                ext = "tmp";
            }

            if (fileName.LastIndexOf(".") < (fileName.Length - 5))
                fileName += "." + ext;

            strPath = ResFoler + fileName;
            try
            {
                using (var fs = new FileStream(strPath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }

            if (System.IO.File.Exists(strPath))
            {
                string mimeType = MimeType.GetMimeType(bytes, strPath);
                if (fileName.EndsWith("tmp"))
                {
                    string extR = MimeType.GetFileExtForMimeTypeApache(mimeType);
                    string newFileName = fileName.Replace("tmp", extR);
                    System.IO.File.Move(strPath, ResFoler + newFileName);
                }
                return mimeType;
            }
            return null;
        }

        private byte[] GetFileByteArray(string filename)
        {
            FileStream oFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // Create a byte array of file size.
            byte[] FileByteArrayData = new byte[oFileStream.Length];

            //Read file in bytes from stream into the byte array
            oFileStream.Read(FileByteArrayData, 0, System.Convert.ToInt32(oFileStream.Length));

            //Close the File Stream
            oFileStream.Close();

            return FileByteArrayData; //return the byte data
        }
    }
}