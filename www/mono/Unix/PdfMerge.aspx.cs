using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Unix
{

    /// <summary>
    /// PdfMerge variant for merging multiple pdfs
    /// </summary>
    public partial class PdfMerge : System.Web.UI.Page
    {

        #region fields & properties

        private static readonly string pdfMergeCmd = "";

        private static readonly object _lock;
        
        private string _mergeFile = "";
        internal string MergeAppPath { get => LibPaths.OutAppPath + _mergeFile; }
        internal string MergeSystemPath { get => LibPaths.SystemDirOutPath + _mergeFile; }
        internal string MergeToolTip { get => "Successfully merged pdfs to " + _mergeFile; }

        internal long FileSizeLimit
        {
            get => ConfigurationManager.AppSettings["PDFMergeFileSizeLimitMB"] != null ?
                (int.Parse(ConfigurationManager.AppSettings["PDFMergeFileSizeLimitMB"]) * 1024 * 1024) : (1024 * 1024 * 10);
        }

        public string Base64Mime { get; set; }        

        string _joinFiles = "";
        internal string JoinedFiles 
        {
            get => StringValueFromListBox();
            set => ListBoxFromValue(value);
        }

        #endregion fields & properties

        #region ctors
        static PdfMerge()
        {
            _lock = new object();
            if (ConfigurationManager.AppSettings["PDFMergeCmd"] != null)
                pdfMergeCmd = (string)ConfigurationManager.AppSettings["PDFMergeCmd"];
        }

        public PdfMerge()
        {            
            Base64Mime = "";                        
        }
        #endregion ctors

        #region page and control event handlers

        #region OnInit Page_Load page event cycle hooks

        /// <summary>
        /// overriden init calls base <see cref="Page.OnInit(EventArgs)"/>
        /// </summary>
        /// <param name="e"><see cref="EventArgs">EventArgs e</see></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ButtonUploadID.Attributes["name"] = "ButtonUploadName";
            // oFile.Attributes["onchange"] = "UploadFileJS(this)";
            oFile.Attributes["onchange"] = "UploadFile(this, " + ButtonUploadID.ClientID + ")";
            // FileUploadInput.Attributes["onchange"] = "UploadFileJS(this, " + ButtonUploadID.ClientID + ")";            

            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session.Keys != null && HttpContext.Current.Session.Keys.Count > 0)
            {
                _mergeFile = (HttpContext.Current.Session[Constants.DECRYPTED_TEXT_BOX] != null) ?
                                (string)HttpContext.Current.Session[Constants.DECRYPTED_TEXT_BOX] :
                            (SpanDownload.Visible && !string.IsNullOrEmpty(aPdfMergeDownload.InnerText) ?
                                aPdfMergeDownload.InnerText : _mergeFile);
            }
        }

        /// <summary>
        /// Page_Load is fired at <see cref="System.Web.UI.Page.OnLoadComplete(EventArgs)"/> page life cycle in asp.net classic
        /// </summary>
        /// <param name="sender"><see cref="object">object sender</see></param>
        /// <param name="e"><see cref="EventArgs">EventArgs e</see></param>
        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!Page.IsPostBack)
            {
                if ((HttpContext.Current.Session != null) && (HttpContext.Current.Session[Constants.UPSAVED_FILE] != null))
                {
                    string joinFiles = (string)HttpContext.Current.Session[Constants.UPSAVED_FILE];
                    ListBoxFromValue(joinFiles);
                }
                DivObject.Visible = false;
                SpanDownload.Visible = false;
                aPdfMergeDownload.Visible = false;
                LabelUploadResult.Visible = false;
            }

            //if (Request.Files != null && Request.Files.Count > 0)
            //{
            //    var httpFile = Request.Files[0];
            //    UploadFile(httpFile);
            //}

        }

        #endregion OnInit Page_Load page event cycle hooks

        /// <summary>
        /// Event fired only from hidden <see cref="Button"/ <see cref="UploadID"/> via onchange js property of file uploader
        /// Calls simply <see cref="UploadFile(HttpPostedFile)"/>
        /// </summary>
        /// <param name="sender"><see cref="object">object sender</see></param>
        /// <param name="e"><see cref="EventArgs">EventArgs e</see></param>
        protected void ButtonUpload_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(oFile.Value))
                UploadFile(oFile.PostedFile);            
        }

        protected void ListBoxFilesUploaded_SelectedIndexChanged(object sender, EventArgs e) 
        {


        }


        protected void ImButtonUp_ArrowUp(object sender, EventArgs e)
        {
            int selected = ListBoxFilesUploaded.SelectedIndex;
            if (selected > 0)
            {
                ListItem item = ListBoxFilesUploaded.Items[selected];
                ListBoxFilesUploaded.Items.RemoveAt(selected);
                ListBoxFilesUploaded.Items.Insert(selected - 1, item);
                ListBoxFilesUploaded.SelectedIndex = (selected - 1);
            }
        }


        protected void ImButtonDown_ArrowDown(object sender, EventArgs e)
        {
            int selected = ListBoxFilesUploaded.SelectedIndex;
            if (selected > -1 && selected < (ListBoxFilesUploaded.Items.Count - 1))
            {
                ListItem item = ListBoxFilesUploaded.Items[selected];
                ListBoxFilesUploaded.Items.RemoveAt(selected);
                ListBoxFilesUploaded.Items.Insert(selected + 1, item);
                ListBoxFilesUploaded.SelectedIndex = selected + 1;
            }
        }

        protected void ImButtonDel_Delete(object sender, EventArgs e)
        {
            int selected = ListBoxFilesUploaded.SelectedIndex;
            if (selected > -1)
            {
                ListItem item = ListBoxFilesUploaded.Items[selected];
                ListBoxFilesUploaded.Items.RemoveAt(selected);
                DeleteItemFile(item);
            }
        }

        protected void ImButtonMerge_Merge(object sender, EventArgs e)
        {
            ButtonPdfMerge_Click(sender, e);
        }


        /// <summary>
        /// Clears all <see cref="ListBoxFilesUploaded"/> items and deletes all uploaded files
        /// and merged pdf file
        /// </summary>
        /// <param name="sender"><see cref="object">object sender</see></param>
        /// <param name="e"><see cref="EventArgs">EventArgs e</see></param>
        protected void ButtonClear_Click(object sender, EventArgs e)
        {
            _mergeFile = (HttpContext.Current.Session[Constants.DECRYPTED_TEXT_BOX] != null) ?
                   (string)HttpContext.Current.Session[Constants.DECRYPTED_TEXT_BOX] :
                       (!string.IsNullOrEmpty(aPdfMergeDownload.InnerText) ?
                           aPdfMergeDownload.InnerText : _mergeFile);

            if (SpanDownload.Visible && !string.IsNullOrEmpty(_mergeFile)) 
            {
                lock (_lock)
                {
                    if (File.Exists(MergeSystemPath))
                    {
                        try
                        {
                            File.Delete(MergeSystemPath);
                        }
                        catch (Exception exFileDel)
                        {
                            Area23Log.LogStatic(exFileDel);
                        }
                    }
                }
            }
            
            foreach (ListItem item in ListBoxFilesUploaded.Items)
            {
                DeleteItemFile(item);
            }

            _joinFiles = "";
            Session[Constants.UPSAVED_FILE] = _joinFiles;
            Session[Constants.DECRYPTED_TEXT_BOX] = "";
            ListBoxFilesUploaded.Items.Clear();
                     
            LabelUploadResult.Text = "Form cleared.";
            LabelUploadResult.Visible = true;

            aPdfMergeDownload.InnerText = "";
            SpanDownload.Visible = false;
            aPdfMergeDownload.Visible = false;
            DivObject.Visible = false;

            HttpContext.Current.Session.Remove(Constants.DECRYPTED_TEXT_BOX);
            HttpContext.Current.Session.Remove(Constants.UPSAVED_FILE);

        }


        /// <summary>
        /// ButtonPdfMerge_Click merges all uploaded files shown in <see cref="ListBoxFilesUploaded"/> to a merged pdf
        /// </summary>
        /// <param name="sender"><see cref="object">object sender</see></param>
        /// <param name="e"><see cref="EventArgs">EventArgs e</see></param>
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
                _mergeFile = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_merge.pdf";
                args += MergeSystemPath;

                string psCmd = ProcessCmd.ExecuteWithOutAndErr(pdfMergeCmd, args, out stdOut, out stdErr);

                if (File.Exists(MergeSystemPath))
                {
                    LabelUploadResult.Text = $"successfully merged {argCnt} pdf's to {_mergeFile}.";
                    LabelUploadResult.Visible = true;

                    aPdfMergeDownload.HRef = MergeAppPath;
                    aPdfMergeDownload.Visible = true;
                    aPdfMergeDownload.InnerText = _mergeFile;
                    SpanDownload.Visible = true;

                    // Thread.Sleep(100);
                    
                    byte[] fileBytes = File.ReadAllBytes(MergeSystemPath);
                    Base64Mime = Convert.ToBase64String(fileBytes, Base64FormattingOptions.None);

                    DivObject.InnerHtml = String.Format(
                        "\t\t<object data=\"data:application/pdf;base64,{0}\" type='application/pdf' width=\"640px\" height=\"480px\">\r\n" +
                        "\t\t\t<p>Unable to display type application/pdf</p>\r\n\t\t</object>\r\n", Base64Mime);                    
                    DivObject.Visible = true;
                }

            }

        }

        #endregion page and control event handlers


        #region ListBoxFilesUploaded helper functions
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
                        ListItem listItem = new ListItem(file, file, true);
                        ListBoxFilesUploaded.Items.Add(listItem);
                    }
                }
            }
        }

        internal void DeleteItemFile(ListItem item)
        {
            if (item == null)
                return;

            string itemString = (!string.IsNullOrEmpty(item.Value)) ? item.Value :
                (!string.IsNullOrEmpty(item.Text) ? item.Text : item.ToString());

            if (string.IsNullOrEmpty(itemString))
                return;

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
                        Area23Log.LogStatic(exFileDel);
                    }
                }
            }
        }


        #endregion ListBoxFilesUploaded helper functions


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

                if (!fileExtn.TrimEnd().EndsWith("pdf"))
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

                if (ListBoxFilesUploaded.Items.Count > 4)
                {
                    LabelUploadResult.Text = "Only 4 .pdf's are allowed. Discarded: " + fileName + "!";
                    LabelUploadResult.ToolTip = "You can merge more .pdf's by merging 4 by 4 and then merge the results!";
                    return;
                }
                if (pfile.ContentLength > FileSizeLimit)
                {
                    LabelUploadResult.Text = "Maximum 8 MB (mega bytes) are allowed for upload. Discarded: " + fileName + "!";
                    LabelUploadResult.ToolTip = "You can merge bigger .pdf's by using pdfunite under linux / unix!";
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
                    LabelUploadResult.Text = fileName + " successfully uploaded.";
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

    }

}
