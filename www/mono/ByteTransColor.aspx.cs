using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace area23.at.www.mono
{
    public partial class ByteTransColor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void TransformImage(byte[] data, string imgFileName = null)
        {
            System.Drawing.Bitmap x1Image = null;
            System.Drawing.Bitmap fromBytesTransImage = null;

            if (!string.IsNullOrEmpty(imgFileName) && File.Exists(imgFileName))
                x1Image = new Bitmap(imgFileName);

            fromBytesTransImage = new Bitmap(1024, 768);
            int x = 0, y = 0;
            int i = 0;
            byte b0, b1, b2;
            for (int c = 0; c < data.Length; c += 3)
            {
                b0 = 0;
                b1 = 0;
                b2 = 0;
                try
                {
                    b0 = data[c];
                    b1 = data[c + 1];
                    b2 = data[c + 2];
                }
                catch (Exception e)
                {

                }
                y = i % 768;
                if (i > 0 && ((i % 768) == 0))
                    x++;
                Color col = Color.FromArgb(Convert.ToInt16(b0), Convert.ToInt16(b1), Convert.ToInt16(b2));
                fromBytesTransImage.SetPixel(x, y, col);
                i++;
            }

            MemoryStream ms = new MemoryStream();
            fromBytesTransImage.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            this.imgOut.Src = "data:image/gif;base64," + base64Data;

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strFileName;
            string strFilePath;
            string strFolder;
            strFolder = Server.MapPath("./res/");
            // Get the name of the file that is posted.
            strFileName = oFile.PostedFile.FileName;
            strFileName = Path.GetFileName(strFileName);
            if (oFile.Value != "")
            {
                // Create the directory if it does not exist.
                if (!Directory.Exists(strFolder))
                {
                    Directory.CreateDirectory(strFolder);
                }
                // Save the uploaded file to the server.
                strFilePath = strFolder + strFileName;
                if (File.Exists(strFilePath))
                {
                    lblUploadResult.Text = strFileName + " already exists on the server!";
                }
                else
                {
                    oFile.PostedFile.SaveAs(strFilePath);
                    lblUploadResult.Text = strFileName + " has been successfully uploaded.";
                    byte[] fileBytes = GetFileByteArray(strFilePath);
                    var base64Data = Convert.ToBase64String(fileBytes);
                    this.imgIn.Src = "data:image;base64," + base64Data;
                    this.TransformImage(fileBytes, strFilePath);
                }
            }
            else
            {
                lblUploadResult.Text = "Click 'Browse' to select the file to upload.";
            }


            // Display the result of the upload.
            frmConfirmation.Visible = true;
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