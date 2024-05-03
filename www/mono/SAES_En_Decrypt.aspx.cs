using Area23.At.Mono.Properties;
using Area23.At.Mono.Util;
using Area23.At.Mono.Util.Enum;
using Area23.At.Mono.Util.SymChiffer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
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
                ; // handled by Event members
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

        /// <summary>
        /// Decrypt File
        /// </summary>
        /// <param name="data">byte[] of encrypted file</param>
        /// <param name="imgFileName"></param>
        /// <param name="symChiffre">encryption SymChiffre</param>
        /// <returns></returns>
        protected byte[] DecryptFile(byte[] data, string imgFileName = null, SymChiffre symChiffre = SymChiffre.NONE)
        {
            string savedFile = "res/fortune.u8";
            byte[] plainData = data;

            aTransFormed.HRef = savedFile;
            imgOut.Src = "res/img/decrypted.png";
            return plainData;
        }


        /// <summary>
        /// Encrypt File
        /// </summary>
        /// <param name="data">byte[] of plain text file</param>
        /// <param name="imgFileName"></param>
        /// <param name="symChiffre">decryption SymChiffre</param>
        /// <returns></returns>
        protected byte[] EncryptFile(byte[] data, string imgFileName = null, SymChiffre symChiffre = SymChiffre.NONE)
        {
            string savedFile = "res/fortune.u8";
            byte[] encryptedData = data;
            aTransFormed.HRef = savedFile;
            imgOut.Src = "res/img/encrypted.png";
            return encryptedData;
        }


        protected void ButtonEncryptFile_Click(object sender, EventArgs e)
        {
            if (Request.Files != null && Request.Files.Count > 0)
                EnDeCryptUploadFile(Request.Files[0], true);
        }


        protected void ButtonDecryptFile_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(oFile.Value))
            {
                EnDeCryptUploadFile(oFile.PostedFile, false);
            }
        }

        protected void EnDeCryptUploadFile(HttpPostedFile pfile, bool crypt = true)
        {
            string strFilePath;
            // Get the name of the file that is posted.
            string strFileName = pfile.FileName;
            strFileName = Path.GetFileName(strFileName);
            lblUploadResult.Text = "";

            if (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0))
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
                
                if (!string.IsNullOrEmpty(strFilePath) && System.IO.File.Exists(strFilePath))
                {
                    if (crypt)
                    {
                        foreach (ListItem item in this.CheckBoxListEncDeCryption.Items)
                        {
                            if (item.Value == "3DES" && item.Selected)
                            {
                                EncryptFile(fileBytes, strFilePath, SymChiffre.DES3);
                            }
                            if (item.Value == "AES" && item.Selected)
                            {
                                EncryptFile(fileBytes, strFilePath, SymChiffre.AES);
                            }
                            if (item.Value == "Serpent" && item.Selected)
                            {
                                EncryptFile(fileBytes, strFilePath, SymChiffre.SERPENT);
                            }
                        }
                    }                        
                    else
                    {
                        bool tripleDesDecrypt = false;
                        bool aesDecrypt = false;
                        bool serpentDecrypt = false;

                        foreach (ListItem item in this.CheckBoxListEncDeCryption.Items)
                        {
                            tripleDesDecrypt = (item.Value == "3DES" && item.Selected);
                            aesDecrypt = (item.Value == "AES" && item.Selected);
                            serpentDecrypt = (item.Value == "Serpent" && item.Selected);                            
                        }
                        if (serpentDecrypt)
                        {
                            DecryptFile(fileBytes, strFilePath, SymChiffre.SERPENT);
                        }
                        if (aesDecrypt)
                        {
                            DecryptFile(fileBytes, strFilePath, SymChiffre.AES);
                        }
                        if (tripleDesDecrypt)
                        {
                            DecryptFile(fileBytes, strFilePath, SymChiffre.DES3);
                        }
                    }
                        
                }                
            }
            else
            {
                lblUploadResult.Text = "Click 'Browse' to select the file to upload.";
            }


            // Display the result of the upload.
            frmConfirmation.Visible = true;
        }

        protected void ButtonEncrypt_Click(object sender, EventArgs e)
        {
            frmConfirmation.Visible = false;
            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                string source = this.TextBoxSource.Text;
                string encrypted = string.Empty;

                foreach (ListItem item in this.CheckBoxListEncDeCryption.Items)
                {
                    if (item.Value == "3DES" && item.Selected)
                    {
                        encrypted = TripleDes.EncryptString(source);
                        source = encrypted;
                    }
                    if (item.Value == "AES" && item.Selected)
                    {
                        encrypted = Aes.EncryptString(source, Aes.AesKey, Aes.AesIv);
                        source = encrypted;
                    }
                    if (item.Value == "Serpent" && item.Selected)
                    {
                        encrypted = Serpent.EncryptString(source);
                        source = encrypted;
                    }
                }
                this.TextBoxDestionation.Text = encrypted;
            }
        }

        protected void ButtonDecrypt_Click(object sender, EventArgs e)
        {
            frmConfirmation.Visible = false;
            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                string source = this.TextBoxSource.Text;
                string decrypted = string.Empty;
                bool tripleDesDecrypt = false;
                bool aesDecrypt = false;
                bool serpentDecrypt = false;

                foreach (ListItem item in this.CheckBoxListEncDeCryption.Items)
                {
                    tripleDesDecrypt = (item.Value == "3DES" && item.Selected);
                    aesDecrypt = (item.Value == "AES" && item.Selected);
                    serpentDecrypt = (item.Value == "Serpent" && item.Selected);
                }
                if (serpentDecrypt)
                {
                    decrypted = Serpent.DecryptString(source);
                    source = decrypted;
                }
                if (aesDecrypt)
                {
                    decrypted = Aes.DecryptString(source, Aes.AesKey, Aes.AesIv);
                    source = decrypted;
                }
                if (tripleDesDecrypt)
                {
                    decrypted = TripleDes.DecryptString(source);
                    source = decrypted;
                }
                this.TextBoxDestionation.Text = decrypted;
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