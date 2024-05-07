using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.Util
{
    public class UIPage : System.Web.UI.Page
    {
        private Uri gitUrl;
        private Uri backUrl;
        private System.Globalization.CultureInfo locale;

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

        public string SepChar { get => Paths.SepChar.ToString(); }

        public string LogFile { get => Paths.LogPathFile; }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            locale = Locale;
            InitURLBase();
        }


        public virtual void InitURLBase()
        {
            gitUrl = new Uri(Constants.GIT_URL);
            backUrl = new Uri(Request.Url.ToString());
        }

        public virtual void Log(string msg)
        {
            Area23Log.LogStatic(msg);
        }


        /// <summary>
        /// Get Image mime type for image bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetMimeTypeForImageBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
            {
                return ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == img.RawFormat.Guid).MimeType;
            }
        }


        /// <summary>
        /// Saves a byte[] to a fileName
        /// </summary>
        /// <param name="bytes">byte[] of raw data</param>
        /// <param name="outMsg"></param>
        /// <param name="fileName">filename to save</param>
        /// <returns>fileName under which it was saved really</returns>
        protected virtual string ByteArrayToFile(byte[] bytes, out string outMsg, string fileName = null)
        {
            string strPath = Paths.OutDirPath;
            outMsg = String.Empty;
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

            string newFileName = fileName;
            
            strPath = Paths.OutDirPath + fileName;            
            try
            {
                while (System.IO.File.Exists(strPath))
                {
                    newFileName = fileName.Contains(Constants.DateFile) ?
                        Constants.DateFile + Guid.NewGuid().ToString() + "_" + fileName :
                        Constants.DateFile + fileName;
                    strPath = Paths.OutDirPath + newFileName;
                    outMsg = String.Format("{0} already exists on server, saving it to ", fileName);
                    fileName = newFileName;
                }
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
                    newFileName = fileName.Replace("tmp", extR);
                    System.IO.File.Move(strPath, Paths.OutDirPath + newFileName);
                    outMsg += newFileName;
                    return newFileName;
                }
                outMsg += fileName;
                return fileName;
            }
            outMsg = null;
            return null;
        }

        protected virtual byte[] GetFileByteArray(string filename)
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