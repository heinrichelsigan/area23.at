using Area23.At.Framework.Library;
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

namespace Area23.At.Mono.Crypt
{
    public partial class ImgPngCrypt : Util.UIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                buttonUpload_Click(sender, e);
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

            string outMsg;
            string mimeType = ByteArrayToFile(bList.ToArray(), out outMsg);   
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
                strFilePath = LibPaths.OutDirPath + strFileName;
                while (System.IO.File.Exists(strFilePath))
                {
                    string newFileName = strFilePath.Contains(Constants.DateFile) ?
                        Constants.DateFile + Guid.NewGuid().ToString() + "_" + strFileName :
                        Constants.DateFile + strFileName;
                    strFilePath = LibPaths.OutDirPath + newFileName;
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

    }
}