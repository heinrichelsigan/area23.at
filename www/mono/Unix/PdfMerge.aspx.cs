using Area23.At.Framework.Library.Static;
using Area23.At.Mono.Unix;
using Area23.At.Mono.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Interop;

namespace Area23.At.Mono.Unix
{
    public partial class PdfMerge : System.Web.UI.Page
    {

        private static readonly string pdfMergeCmd = "";

        private static readonly object _lock;

        private string mergeFile = "";
        public string Base64Mime { get; set; }

        public List<string> FileList { get; set; }

        internal string JoinedFiles
        {
            get => String.Join(";", FileList.ToArray());
            set => FileList = new List<string>(value.Split(";".ToCharArray()));
        }

        static PdfMerge()
        {
            _lock = new object();
            if (ConfigurationManager.AppSettings["PDFMergeCmd"] != null)
                pdfMergeCmd = (string)ConfigurationManager.AppSettings["PDFMergeCmd"];
        }

        public PdfMerge()
        {            
            Base64Mime = "";             
            FileList = new List<string>();
            
            JoinedFiles = (HttpContext.Current.Session[Constants.UPSAVED_FILE] != null) ? 
                (string)HttpContext.Current.Session[Constants.UPSAVED_FILE] : "";
            foreach (string file in FileList)
                this.ListBoxFilesUploaded.Items.Add(file);
            

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if ((Request.Files != null && Request.Files.Count > 0) || (!String.IsNullOrEmpty(oFile.Value)))
            {
                UploadFile(oFile.PostedFile);
            }

        }


        /// <summary>
        /// Uploads a http posted file
        /// </summary>
        /// <param name="pfile"><see cref="HttpPostedFile"/></param>
        protected void UploadFile(HttpPostedFile pfile)
        {
            string filePath = "", fileName = "", fileExtn = "";
            if (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0))
            {
                fileName = pfile.FileName;
                fileExtn = Path.GetExtension(fileName).Trim().ToLower();

                fileName = Path.GetFileName(fileName);
                filePath = LibPaths.SystemDirOutPath + fileName;

                if (!fileExtn.TrimEnd().EndsWith("pdf"))
                {
                    LabelUploadResult.Text = fileName + " " + fileExtn + " isn't 'pdf'!";
                    LabelUploadResult.ToolTip = "Can't upload file " + fileName + ", because " + fileExtn + " isn't 'pdf'!";
                    LabelUploadResult.Visible = true;
                    return;
                }

                if (FileList.Contains(fileName) || File.Exists(filePath))
                {
                    LabelUploadResult.Text = fileName + " already exists.";
                    LabelUploadResult.ToolTip = "Can't upload file " + fileName + ", because it already exists.";
                    LabelUploadResult.Visible = true;
                    return;
                }
                
                pfile.SaveAs(filePath);

                if (System.IO.File.Exists(filePath))
                {
                    LabelUploadResult.Text = fileName + " successfully uploaded.";
                    LabelUploadResult.ToolTip = "File " + fileName + " has been successfully uploaded.";
                    LabelUploadResult.Visible = true;

                    FileList.Add(fileName);                    
                    this.ListBoxFilesUploaded.Items.Add(fileName);
                    Session[Constants.UPSAVED_FILE] = JoinedFiles;

                    this.DivObject.Visible = true;
                    
                    Base64Mime = Convert.ToBase64String(pfile.InputStream.ToByteArray(), Base64FormattingOptions.None);
                }
            }
            else
            {
                LabelUploadResult.Text = "Upload unsuccessfully!";
                LabelUploadResult.ToolTip = "Failed to upload file!";
                LabelUploadResult.Visible = true;
            }
        }


        protected void ListBoxFilesUploaded_SelectedIndexChanged(object sender, EventArgs e) 
        {


        }


        protected void ButtonPdfMerge_Click(object sender, EventArgs e)
        {
            int argCnt = 0;
            string args = "", stdOut = "", stdErr = "";

            if (ListBoxFilesUploaded.Items.Count < 2)
            {
                this.LabelUploadResult.Text = "You neeed at least 2 files to merge.";
                return ; 
            }

            foreach (var item in ListBoxFilesUploaded.Items)
            {
                if (File.Exists(LibPaths.SystemDirOutPath + item.ToString()))
                {
                    args += LibPaths.SystemDirOutPath + item.ToString() + " ";
                    argCnt++; // TODO check file exists here
                }
            }

            if (argCnt > 0)
            {
                mergeFile = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_Merge.pdf";
                args += LibPaths.SystemDirOutPath + mergeFile;
                string psCmd = ProcessCmd.ExecuteWithOutAndErr(pdfMergeCmd, args, out stdOut, out stdErr);

                Thread.Sleep(100);

                if (File.Exists(LibPaths.SystemDirOutPath + mergeFile))
                {
                    this.LabelUploadResult.Text = "succesgully merged.";
                    this.aPdfMergeDownload.HRef = LibPaths.OutAppPath + mergeFile;
                    this.aPdfMergeDownload.Visible = true;
                    this.aPdfMergeDownload.InnerText = mergeFile;
                    byte[] fileBytes = File.ReadAllBytes(LibPaths.SystemDirOutPath + mergeFile);
                    Base64Mime = Convert.ToBase64String(fileBytes, 0, fileBytes.Length, Base64FormattingOptions.None);
                }
            }

        }


    } 
       
}