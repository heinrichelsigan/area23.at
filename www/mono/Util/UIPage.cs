using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Area23.At.Mono.Util
{
    public class UIPage : System.Web.UI.Page
    {
        private Uri gitUrl;
        private Uri backUrl;
        private System.Globalization.CultureInfo locale;
        protected internal ushort _initState = 0x0;
        protected internal object _lock = new object();
        protected internal string _directoryPath = "";

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

        public string SepChar { get => LibPaths.SepChar.ToString(); }

        public string LogFile { get => LibPaths.LogFileSystemPath; }

        public string PhysicalDirectoryPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_directoryPath) && Directory.Exists(_directoryPath))
                    return _directoryPath;

                int lastPathSeperator = -1;
                string phyAppPath = Request.PhysicalPath;

                if ((phyAppPath.Contains(SepChar)) && ((lastPathSeperator = phyAppPath.LastIndexOf(SepChar)) > 0))
                    _directoryPath = phyAppPath.Substring(0, lastPathSeperator);
                if (string.IsNullOrEmpty(_directoryPath))
                {
                    var dirInfo = Directory.GetParent(phyAppPath);
                    _directoryPath = dirInfo.FullName;
                }

                return _directoryPath;
            }
        }


        public UIPage()
        {
            _initState = 0x1;
        }


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
        /// AuthHtPasswd - authenticates a user against .htpasswd in apache2
        /// </summary>
        /// <param name="user"><see cref="string"/> username</param>
        /// <param name="passwd"><see cref="string"/>password</param>
        /// <returns>true on successful authentificaton, otherwise false</returns>
        protected virtual bool AuthHtPasswd(string user, string passwd)
        {

            bool authTypeBasic = false, authBasicProviderFile = false;
            string directoryPath = "", htAccessFile = "", authFile = "", requireUser = "";


            if (!Directory.Exists(PhysicalDirectoryPath))
            {
                Area23Log.LogStatic("return false! \tdirectory " + directoryPath + " does not exist!\n");
                return false;
            }

            htAccessFile = Path.Combine(directoryPath, ".htaccess");
            if (!File.Exists(htAccessFile))
            {
                Area23Log.LogStatic("return true; \t.htaccess file " + htAccessFile + " does not exist!\n");
                return true;
            }

            List<string> lines = File.ReadLines(htAccessFile).ToList();
            foreach (string line in lines)
            {
                if (line.StartsWith("AuthType Basic", StringComparison.CurrentCultureIgnoreCase))
                    authTypeBasic = true;
                if (line.StartsWith("AuthBasicProvider file", StringComparison.CurrentCultureIgnoreCase))
                    authBasicProviderFile = true;
                if (line.StartsWith("AuthUserFile ", StringComparison.CurrentCultureIgnoreCase))
                    authFile = line.Replace("AuthUserFile ", "").Replace("\"", "");
                if (line.StartsWith("Require user ", StringComparison.CurrentCultureIgnoreCase))
                    requireUser = line.Replace("Require user ", "");
            }

            if (!authTypeBasic && !authBasicProviderFile)
            {
                Area23Log.LogStatic("return false! \tauthTypeBasic = " + authTypeBasic + "; authBasicProviderFile = " + authBasicProviderFile + ";\n");
                return false;
            }

            if (!string.IsNullOrEmpty(requireUser) && !user.Equals(requireUser, StringComparison.CurrentCultureIgnoreCase))
            {
                Area23Log.LogStatic("return false! \trequireUser = " + requireUser + " NOT EQUALS user = " + user + "!\n");
                return false;
            }


            if (!string.IsNullOrEmpty(authFile) && File.Exists(authFile))
            {
                string consoleOut = "", consoleError = "";
                string passedthrough = ProcessCmd.ExecuteWithOutAndErr(
                    "htpasswd",
                    String.Format(" -b -v {0} {1} {2} ", authFile, user, passwd),
                    out consoleOut,
                    out consoleError,
                    false);

                Area23Log.LogStatic("passedthrough = \t$(htpasswd" + String.Format(" -b -v {0} {1} {2})", authFile, user, passwd));
                Area23Log.LogStatic("passedthrough = \t" + passedthrough);
                string userMatch = string.Format("Password for user {0} correct.", user);
                if (passedthrough.EndsWith(userMatch, StringComparison.CurrentCultureIgnoreCase) ||
                    passedthrough.Contains(userMatch))
                {
                    Area23Log.LogStatic("return true; \t[" + passedthrough + "] matches {" + userMatch + "}.\n");
                    return true;
                }
                else
                {
                    Area23Log.LogStatic("return false! \t[" + passedthrough + "] not matching {" + userMatch + "}.\n");
                    return false;
                }

            }

            Area23Log.LogStatic("return true; \tfall through.\n");
            return true;
        }


        #region bytes strings files

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
        /// <param name="directoryName">directory where to save file</param>
        /// <returns>fileName under which it was saved really</returns>
        protected virtual string ByteArrayToFile(byte[] bytes, out string outMsg, string fileName = null, string directoryName = null)
        {
            outMsg = String.Empty;
            string strPath = LibPaths.SystemDirOutPath;
            if (!string.IsNullOrEmpty(directoryName) && Directory.Exists(directoryName))
            {
                strPath = directoryName;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Constants.DateFile + Guid.NewGuid().ToString();
            }
            string ext = "hex";
            fileName += (fileName.LastIndexOf(".") > -1) ? "" : "." + ext;

            string newFileName = fileName.BeautifyUploadFileNames();

            try
            {
                while (System.IO.File.Exists(strPath + fileName))
                {
                    newFileName = fileName.Contains(Constants.DateFile) ?
                        Constants.DateFile + Guid.NewGuid().ToString() + "_" + fileName :
                        Constants.DateFile + fileName;
                    outMsg = String.Format("{0} already exists on server, saving it to {1}", fileName, newFileName);
                    fileName = newFileName;
                }
                using (var fs = new FileStream(strPath + fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }

            if (System.IO.File.Exists(strPath + fileName))
            {
                string mimeType = MimeType.GetMimeType(bytes, strPath + fileName);
                if (fileName.EndsWith("tmp"))
                {
                    string extR = MimeType.GetFileExtForMimeTypeApache(mimeType);
                    if (extR.ToLowerInvariant().Equals("hex") || extR.ToLowerInvariant().Equals("oct"))
                        newFileName = fileName;
                    else
                    {
                        newFileName = fileName.Replace("tmp", extR);
                        System.IO.File.Move(strPath + fileName, LibPaths.SystemDirOutPath + newFileName);
                    }

                    outMsg = newFileName;
                    return newFileName;
                }
                else
                {
                    outMsg = fileName;
                    return fileName;
                }
            }

            outMsg = null;
            return null;
        }

        protected virtual string StringToFile(string encodedText, out string outMsg, string fileName = null, string directoryName = null)
        {
            string strPath = LibPaths.SystemDirOutPath;
            if (!String.IsNullOrEmpty(directoryName) && Directory.Exists(directoryName))
            {
                strPath = directoryName;
            }
            outMsg = String.Empty;
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = Constants.DateFile + Guid.NewGuid().ToString();
            }
            string ext = "hex";

            fileName += (fileName.LastIndexOf(".") > -1) ? "" : "." + ext;

            string newFileName = fileName.BeautifyUploadFileNames();

            // strPath = LibPaths.SystemDirOutPath + fileName;
            try
            {
                while (System.IO.File.Exists(strPath + fileName))
                {
                    newFileName = fileName.Contains(Constants.DateFile) ?
                        Constants.DateFile + Guid.NewGuid().ToString() + "_" + fileName :
                        Constants.DateFile + fileName;
                    // strPath = LibPaths.SystemDirOutPath + newFileName;
                    outMsg = String.Format("{0} already exists on server, saving it to {1}", fileName, newFileName);
                    fileName = newFileName;
                }
                File.WriteAllText(strPath + fileName, encodedText, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }

            if (System.IO.File.Exists(strPath + fileName))
            {
                outMsg = fileName;
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

        #endregion bytes strings files

    }

}