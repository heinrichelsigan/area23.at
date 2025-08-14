using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Unix
{

    public partial class MergePdf : System.Web.UI.Page
    {

        private static readonly string pdfMergeCmd = "";

        private static readonly object _lock;

        private string mergeFile = "";
        public string Base64Mime { get; set; }


        string _joinFiles = "";
        internal string JoinedFiles 
        {
            get => StringValueFromListBox();
            set => ListBoxFromValue(value);
        }

        static MergePdf()
        {
            _lock = new object();
            if (ConfigurationManager.AppSettings["PDFMergeCmd"] != null)
                pdfMergeCmd = (string)ConfigurationManager.AppSettings["PDFMergeCmd"];
        }

        public MergePdf()
        {            
            Base64Mime = "";                        
        }


        internal bool ListBoxContainsItemByName(string itemName)
        {
            foreach (ListItem item in ListBoxFilesUploaded.Items)
            {
                if (item == null)
                    continue;
                if ((!string.IsNullOrEmpty(item.Text) && item.Text.Equals(itemName, StringComparison.CurrentCultureIgnoreCase)) ||
                    (!string.IsNullOrEmpty(item.Value) && item.Value.Equals(itemName, StringComparison.CurrentCultureIgnoreCase)) ||
                    (!string.IsNullOrEmpty(item.ToString()) && item.ToString().Equals(itemName, StringComparison.CurrentCultureIgnoreCase)))
                        return true;
            }
            return false;              
        }

        internal string StringValueFromListBox()
        {

            string _val = "";
            int cnt = 0;

            foreach (ListItem item in ListBoxFilesUploaded.Items)
            {
                if (item == null)
                    continue;

                string itemString = (!string.IsNullOrEmpty(item.Value)) ? item.Value :
                    (!string.IsNullOrEmpty(item.Text) ? item.Text : item.ToString());

                if (string.IsNullOrEmpty(itemString))
                    continue;

                _val += ((cnt > 0) ? ";" : "") + itemString;
                ++cnt;
            }

            _joinFiles = _val;
            return _val;
        }
         
        internal void ListBoxFromValue(string _val)
        {
            if (!string.IsNullOrEmpty(_val))
            {
                _joinFiles = _val;
                foreach (string file in _joinFiles.Split(";".ToCharArray()))
                {
                    if (!ListBoxContainsItemByName(file))
                    {
                        if (File.Exists(LibPaths.SystemDirOutPath + file))
                        {
                            ListItem listItem = new ListItem(file);
                            ListBoxFilesUploaded.Items.Add(listItem);
                        }
                    }
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            UploadID.Attributes["name"] = "UploadName";
            // oFile.Attributes["onchange"] = "UploadFile(this)";
            FileUploadInput.Attributes["onchange"] = "UploadFile(this, " + UploadID.ClientID + ")";
            
            if (!Page.IsPostBack)
            {
                if ((HttpContext.Current.Session != null) && (HttpContext.Current.Session[Constants.UPSAVED_FILE] != null))
                {
                    string joinFiles = (string)HttpContext.Current.Session[Constants.UPSAVED_FILE];
                    ListBoxFromValue(joinFiles);
                }
                DivObject.Visible = false;
                aPdfMergeDownload.Visible = false;
                LabelUploadResult.Visible = false;
            }

            //if (Request.Files != null && Request.Files.Count > 0)
            //{
            //    var httpFile = Request.Files[0];
            //    UploadFile(httpFile);
            //}

        }


        protected void Upload_Click(object sender, EventArgs e)
        {
            if (FileUploadInput.PostedFile != null)
                UploadFile(FileUploadInput.PostedFile);
        }


        /// <summary>
        /// Uploads a http posted file
        /// </summary>
        /// <param name="pfile"><see cref="HttpPostedFile"/></param>
        protected void UploadFile(HttpPostedFile pfile)
        {
            string filePath = "", fileName = "", fileExtn = "";
            LabelUploadResult.Visible = true;
            aPdfMergeDownload.Visible = false;
            DivObject.Visible = false;
            LabelUploadResult.Text = "";

            if (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0))
            {
                fileExtn = Path.GetExtension(pfile.FileName).Trim().ToLower();

                fileName = Path.GetFileName(pfile.FileName).BeautifyUploadFileNames();
                filePath = LibPaths.SystemDirOutPath + fileName;

                if (!fileExtn.TrimEnd().EndsWith("pdf", StringComparison.InvariantCultureIgnoreCase))
                {
                    LabelUploadResult.Text = fileName + " " + fileExtn + " isn't 'pdf'!";
                    LabelUploadResult.ToolTip = "Can't upload file " + fileName + ", because " + fileExtn + " isn't 'pdf'!";
                    return;
                } 

                if (ListBoxContainsItemByName(fileName) && File.Exists(filePath))
                {
                    LabelUploadResult.Text = fileName + " already exists.";
                    LabelUploadResult.ToolTip = "Can't upload file " + fileName + ", because it already exists.";
                    return;
                }

                byte[] fileBytes = pfile.InputStream.ToByteArray();
                if (!fileBytes.Take(7).SequenceEqual(MimeType.PDF))
                {
                    LabelUploadResult.ToolTip = fileName + " might be a corrupted pdf after testing MIT magic sequence.";
                    LabelUploadResult.Text = "Maybe corrupted ? ";
                }

                pfile.SaveAs(filePath);

                if (System.IO.File.Exists(filePath))
                {
                    LabelUploadResult.Text += fileName + " successfully uploaded.";
                    LabelUploadResult.ToolTip = "File " + fileName + " has been successfully uploaded.";

                    ListItem listItem = new ListItem(fileName);
                    ListBoxFilesUploaded.Items.Add(listItem);
                    Session[Constants.UPSAVED_FILE] = JoinedFiles;

                    Base64Mime = Convert.ToBase64String(fileBytes, Base64FormattingOptions.None);
                    DivObject.InnerHtml = String.Format(
                            "<object data=\"data:application/pdf;base64,{0}\" type='application/pdf' width=\"640px\" height=\"480px\">" +
                            "<p>Unable to display type application/pdf</p></object>\r\n", Base64Mime);
                    DivObject.Visible = true;
                }
            }
            else
            {
                LabelUploadResult.Text = "Upload unsuccessfully!";
                LabelUploadResult.ToolTip = "Failed to upload file!";
            }
        }


        protected void ListBoxFilesUploaded_SelectedIndexChanged(object sender, EventArgs e) 
        {


        }

        protected void ButtonClear_Click(object sender, EventArgs e)
        {
            if (aPdfMergeDownload.Visible && !string.IsNullOrEmpty(aPdfMergeDownload.InnerText))
            {
                lock (_lock)
                {
                    if (File.Exists(LibPaths.SystemDirOutPath + aPdfMergeDownload.InnerText))
                    {
                        try
                        {
                            File.Delete(LibPaths.SystemDirOutPath + aPdfMergeDownload.InnerText);
                        }
                        catch (Exception exFileDel)
                        {
                            Area23Log.Logger.Log(exFileDel);
                        }
                    }
                }

                aPdfMergeDownload.Visible = false;
            }
            
            foreach (ListItem item in ListBoxFilesUploaded.Items)
            {
                if (item == null)
                    continue;

                string itemString = (!string.IsNullOrEmpty(item.Value)) ? item.Value :
                    (!string.IsNullOrEmpty(item.Text) ? item.Text : item.ToString());

                if (string.IsNullOrEmpty(itemString))
                    continue;

                lock (_lock)
                {
                    if (File.Exists(LibPaths.SystemDirOutPath + itemString))
                    {
                        try
                        {
                            File.Delete(LibPaths.SystemDirOutPath + itemString);
                        }
                        catch (Exception exFileDel)
                        {
                            Area23Log.Logger.Log(exFileDel);
                        }
                    }
                }
            }
            _joinFiles = "";
            Session[Constants.UPSAVED_FILE] = _joinFiles;
            
            ListBoxFilesUploaded.Items.Clear();

            aPdfMergeDownload.Visible = false;
            DivObject.Visible = false;
            LabelUploadResult.Text = "Form cleared.";
            LabelUploadResult.Visible = true;
            
            HttpContext.Current.Session.Remove(Constants.UPSAVED_FILE);

        }


        protected void ButtonPdfMerge_Click(object sender, EventArgs e)
        {
            int argCnt = 0;
            string args = "", stdOut = "", stdErr = "";

            if (ListBoxFilesUploaded.Items.Count < 2)
            {
                LabelUploadResult.Text = "You neeed at least 2 files to merge.";
                LabelUploadResult.Visible = true;
                aPdfMergeDownload.Visible = false;
                DivObject.Visible = false;
                return ; 
            }

            foreach (ListItem lItem in ListBoxFilesUploaded.Items)
            {
                if (File.Exists(LibPaths.SystemDirOutPath + lItem.Text))
                {
                    args += LibPaths.SystemDirOutPath + lItem.Text + " ";
                    argCnt++; // TODO check file exists here
                }
            }

            if (argCnt > 0)
            {
                mergeFile = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_merge.pdf";
                args += LibPaths.SystemDirOutPath + mergeFile;
                string psCmd = ProcessCmd.ExecuteWithOutAndErr(pdfMergeCmd, args, out stdOut, out stdErr);

                Thread.Sleep(100);

                if (File.Exists(LibPaths.SystemDirOutPath + mergeFile))
                {
                    LabelUploadResult.Text = "succesfully merged to " + mergeFile;
                    LabelUploadResult.Visible = true;
                    
                    aPdfMergeDownload.HRef = LibPaths.OutAppPath + mergeFile;
                    aPdfMergeDownload.Visible = true;
                    aPdfMergeDownload.InnerText = mergeFile;
                    Thread.Sleep(100);

                    DivObject.Style["display"] = "block";
                    byte[] fileBytes = File.ReadAllBytes(LibPaths.SystemDirOutPath + mergeFile);
                    Base64Mime = Convert.ToBase64String(fileBytes, Base64FormattingOptions.None);

                    DivObject.InnerHtml = String.Format(
                           "<object data=\"data:application/pdf;base64,{0}\" type='application/pdf' width=\"640px\" height=\"480px\">" +
                           "</object>", Base64Mime);
                    DivObject.Visible = true;
                }
            }

        }


    } 
       
}