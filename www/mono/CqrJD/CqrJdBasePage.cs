using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Cqr.Msg;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Area23.At.Mono.CqrJD
{

    /// <summary>
    /// CqrJdBasePage is base page of Default
    /// </summary>
    public abstract class CqrJdBasePage : System.Web.UI.Page
    {
        protected System.Collections.Generic.Queue<string> mqueue = new Queue<string>();
        protected Uri area23AtUrl = new Uri("https://area23.at/");
        protected Uri cqrXsEuUrl = new Uri("https://srv.cqrxs.eu/v1.1/");
        protected Uri githubUrl = new Uri("https://github.com/heinrichelsigan/chat-ipv6/");
        protected internal global::System.Web.UI.WebControls.Image ImageQr;
        protected internal System.Globalization.CultureInfo locale;
        protected global::System.Web.UI.HtmlControls.HtmlAnchor HrefShort;

        protected internal ushort initState = 0x0;
        protected internal string hashKey = string.Empty;
        protected internal string tmpStrg = string.Empty;
        protected internal string allStrng = string.Empty;
        protected internal string decrypted = string.Empty;
        protected internal object _lock = new object();
        protected internal string myServerKey = string.Empty;
        protected internal HashSet<CContact> _contacts;
        protected internal CContact myContact = null;
        // protected internal IPAddress clientIp;


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

        protected CqrJdBasePage() : base()
        {
            initState = 0x1;
        }

        protected virtual void Page_Init(object sender, EventArgs e)
        {

            myServerKey = GetServerKey();
            allStrng = string.Empty;
            myContact = null;
            if (Application[Constants.JSON_CONTACTS] != null)
                _contacts = (HashSet<CContact>)(Application[Constants.JSON_CONTACTS]);
            else
                _contacts = LoadJsonContacts();

            initState = 0x2;
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (initState < 0x2 || _contacts == null)
            {
                Page_Init(sender, e);
            }


            // clientIp = GetClientExternalIp();
            myServerKey = GetServerKey();
            allStrng = "UserHostAddress: " + myServerKey.Replace(Constants.APP_NAME, "") + Environment.NewLine;
            // myServerKey += Constants.APP_NAME;

            if (Request.Headers["User-Agent"] != null)
                tmpStrg = (string)Request.Headers["User-Agent"];
            else if (Request.Headers["User-Agent:"] != null)
                tmpStrg = (string)Request.Headers["User-Agent:"];
            allStrng += "User-Agent: " + tmpStrg + Environment.NewLine;


            if (Application["lastdecrypted"] != null)
                allStrng += "LastDecrypted: " + (string)Application["lastdecrypted"] + Environment.NewLine;
            if (Application["lastmsg"] != null)
                allStrng += "LastMsg: " + (string)Application["lastmsg"] + Environment.NewLine;

            try
            {
                if (Request.Params["Authorization"] != null)
                    allStrng += "Authorization: " + Request.Params["Authorization"].ToString() + Environment.NewLine;

                if ((Request.Files != null && Request.Files.Count > 0))
                {

                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }

            initState = 0x4;

        }



        protected string GetServerKey()
        {
            // _serverKey = Constants.AUTHOR_EMAIL;            

            if (ConfigurationManager.AppSettings["ExternalClientIP"] != null)
            {
                myServerKey = (string)ConfigurationManager.AppSettings["ExternalClientIP"];
            }
            else
                myServerKey = HttpContext.Current.Request.UserHostAddress;
            myServerKey += Constants.APP_NAME;

            return myServerKey;
        }

        public virtual IPAddress GetClientExternalIp()
        {
            string externalClientIp = (ConfigurationManager.AppSettings["ExternalClientIP"] != null) ?
                (string)ConfigurationManager.AppSettings["ExternalClientIP"] : null;

            string externalClientIpv4 = (ConfigurationManager.AppSettings["ExternalClientIPv4"] != null) ?
               (string)ConfigurationManager.AppSettings["ExternalClientIPv4"] : null;

            if (string.IsNullOrEmpty(externalClientIp))
                externalClientIp = Request.UserHostAddress;

            if (!IPAddress.TryParse(externalClientIp, out IPAddress clientIpAddr))
            {
                string reducedExternal = "";
                foreach (char ch in externalClientIp)
                {
                    if ((ch >= '0' && ch <= '9') ||
                        (ch >= 'a' && ch <= 'f') ||
                        (ch >= 'A' && ch <= 'F') ||
                        (ch == '.' && ch == ':'))
                    {
                        reducedExternal += ch;
                    }
                }
                externalClientIp = reducedExternal;
                if (externalClientIp.Contains(":"))
                {
                    List<byte> bytes = new List<byte>();
                    string[] segments = externalClientIp.Split(':');
                    foreach (string segement in segments)
                    {
                        foreach (char c in segement)
                        {
                            byte b = Convert.ToByte(c);
                            bytes.Add(b);
                        }
                    }
                    clientIpAddr = new IPAddress(bytes.ToArray());
                }
                else
                    clientIpAddr = IPAddress.Parse(externalClientIpv4);
            }

            return clientIpAddr;
        }


        public virtual void InitURLBase()
        {
            area23AtUrl = new Uri("https://area23.at/net/");
            cqrXsEuUrl = new Uri("https://srv.cqrxs.eu/v1.1//");
            githubUrl = new Uri("https://github.com/heinrichelsigan/chat-ipv6/");
        }


        public virtual void Log(string msg)
        {
            Area23Log.LogStatic(msg);
        }


        protected virtual CContact FindContactByNameEmail(HashSet<CContact> cHashSet, CContact searchContact)
        {
            CContact cqrFoundContact = JsonContacts.FindContactByNameEmail(cHashSet, searchContact);
            return cqrFoundContact;
        }



        protected virtual HashSet<CContact> LoadJsonContacts()
        {
            return JsonContacts.LoadJsonContacts();
        }

        protected virtual void SaveJsonContacts(HashSet<CContact> contacts)
        {
            JsonContacts.SaveJsonContacts(contacts);
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

            if (fileName.LastIndexOf(".") < (fileName.Length - 8))
                fileName += "." + ext;

            string newFileName = fileName;


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

            if (fileName.LastIndexOf(".") < (fileName.Length - 12))
                fileName += "." + ext;

            string newFileName = fileName;

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


    }

}