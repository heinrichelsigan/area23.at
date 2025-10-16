using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Zfx;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Area23.At.Mono.Crypt
{

    /// <summary>
    /// CoolCrypt is En-/De-cryption pipeline page 
    /// Former hash inside crypted bytestream is removed
    /// Feature to encrypt and decrypt simple plain text or files
    /// </summary>
    public partial class CoolCrypt : UIPage
    {

        KeyHash keyHash = KeyHash.Hex;
        EncodingType encType = EncodingType.Base64;
        ZipType zipType = ZipType.None;
        CipherEnum[] pipeAlgortihms = new CipherEnum[0];
        CipherPipe cipherPipe;
        string key = "", hash = "";

        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((Session[Constants.AES_ENVIROMENT_KEY] != null) && !string.IsNullOrEmpty((string)Session[Constants.AES_ENVIROMENT_KEY]) &&
                    (((string)Session[Constants.AES_ENVIROMENT_KEY]).Length > 3))
                {
                    Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);
                }
                SetBackgroundPicture("../res/img/crypt/AesImproveBG.gif");
            }

            if ((Request.Files != null && Request.Files.Count > 0) || (!string.IsNullOrEmpty(oFile.Value)))
            {
                UploadFile(oFile.PostedFile);
            }

        }

        #region page_events

        /// <summary>
        /// TextBox_Key_TextChanged - fired on <see cref="TextBox_Key"/> TextChanged event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        [Obsolete("TextBox_Key_TextChanged is fully deprectated, because no autopostback anymore", false)]
        protected void TextBox_Key_TextChanged(object sender, EventArgs e)
        {
            Reset_TextBox_IV(this.TextBox_Key.Text);

            this.TextBox_IV.BorderColor = Color.GreenYellow;
            this.TextBox_IV.ForeColor = Color.DarkOliveGreen;
            this.TextBox_IV.BorderStyle = BorderStyle.Dotted;
            this.TextBox_IV.BorderWidth = 1;

            this.TextBox_Encryption.BorderStyle = BorderStyle.Double;
            this.TextBox_Encryption.BorderColor = Color.DarkOliveGreen;
            this.TextBox_Encryption.BorderWidth = 2;

            SetBackgroundPicture("../res/img/crypt/AesImproveBG.gif");
        }


        /// <summary>
        /// Saves current email address as crypt key inside that asp Session
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Key_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 1)
            {
                Session[Constants.AES_ENVIROMENT_KEY] = this.TextBox_Key.Text;
            }
        }

        /// <summary>
        /// Clear encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Clear_Click(object sender, EventArgs e)
        {
            this.TextBox_Encryption.Text = "";
            this.TextBox_IV.Text = "";
            this.TextBox_Key.Text = Constants.AUTHOR_EMAIL;
            this.RadioButtonList_Hash.SelectedValue = KeyHash.Hex.ToString();
            this.TextBoxSource.Text = "";
            this.TextBoxDestionation.Text = "";

            if ((Session[Constants.AES_ENVIROMENT_KEY] != null))
                Session.Remove(Constants.AES_ENVIROMENT_KEY);

            SetBackgroundPicture("../res/img/crypt/AesImproveBG.gif");
        }

        /// <summary>
        /// Button_Hash_Click sets hash from key and fills pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Hash_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
                Reset_TextBox_IV(this.TextBox_Key.Text);
            else
                this.TextBox_IV.Text = "";
        }

        /// <summary>
        /// RadioButtonList_Hash_ParameterChanged changes key hashing method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadioButtonList_Hash_ParameterChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text))
            {
                Reset_TextBox_IV(this.TextBox_Key.Text);
            }
        }


        /// <summary>
        /// Add encryption alog to encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageButton_Add_Click(object sender, EventArgs e)
        {
            CipherEnum[] cipherAlgor = CipherEnumExtensions.ParsePipeText(this.TextBox_Encryption.Text);
            if (cipherAlgor != null && cipherAlgor.Length > 8)
            {
                uploadResult.Text = "Max 8 algorithms in pipeline allowed!";
                uploadResult.Visible = true;
                return; 
            }
            uploadResult.Text = "";

            foreach (string cryptName in Enum.GetNames(typeof(CipherEnum)))
            {                
                if (!string.IsNullOrEmpty(cryptName) && cryptName != "None")
                {
                    if (DropDownList_Cipher.SelectedValue.ToString() == cryptName)
                    {
                        string addChiffre = DropDownList_Cipher.SelectedValue.ToString() + ";";
                        this.TextBox_Encryption.Text += addChiffre;
                        this.TextBox_Encryption.BorderStyle = BorderStyle.Double;

                        SetBackgroundPicture("../res/img/crypt/AesImproveBG.gif");
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Button_Reset_KeyIV_Click resets <see cref="TextBox_Key"/> and <see cref="TextBox_IV"/> to default loaded values
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_SetPipeline_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
            {
                Reset_TextBox_IV(this.TextBox_Key.Text);
                key = TextBox_Key.Text;
                hash = TextBox_IV.Text;

                CipherEnum[] cses = CipherEnumExtensions.FromString(key);
                this.TextBox_Encryption.Text = string.Empty;
                foreach (CipherEnum c in cses)
                {
                    this.TextBox_Encryption.Text += c.ToString() + ";";
                }

                this.TextBox_Encryption.BorderStyle = BorderStyle.Double;
                this.TextBox_Encryption.BorderColor = Color.DarkOliveGreen;
                this.TextBox_Encryption.BorderWidth = 2;
            }
        }

        /// <summary>
        /// Fired, when DropDownList_Encoding_SelectedIndexChanged
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void DropDownList_Encoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DropDownList_Encoding.SelectedValue.ToLowerInvariant() == "none")
            {
                this.CheckBoxEncode.Checked = false;
                this.CheckBoxEncode.Enabled = false;
            }
            else if (!this.CheckBoxEncode.Enabled)
            {
                CheckBoxEncode.Enabled = true;
                CheckBoxEncode.Checked = true;
            }
        }


        /// <summary>
        /// ButtonEncryptFile_Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonEncryptFile_Click(object sender, EventArgs e)
        {
            if (SpanLeftFile.Visible && aUploaded.HRef.Contains(Constants.OUT_DIR) && !string.IsNullOrEmpty(imgIn.Alt))
            {
                string filePath = LibPaths.SystemDirOutPath + imgIn.Alt;
                if (System.IO.File.Exists(filePath))
                {
                    EnDeCryptFile(null, true, filePath);
                    // EnDeCryptUploadFile(null, true, filePath);
                    return;
                }
            }

            if (Request.Files != null && Request.Files.Count > 0)
                EnDeCryptFile(Request.Files[0], true);
        }

        /// <summary>
        /// ButtonDecryptFile_Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonDecryptFile_Click(object sender, EventArgs e)
        {
            if (SpanLeftFile.Visible && aUploaded.HRef.Contains(Constants.OUT_DIR) && !string.IsNullOrEmpty(imgIn.Alt))
            {
                string filePath = LibPaths.SystemDirOutPath + imgIn.Alt;
                if (System.IO.File.Exists(filePath))
                {
                    EnDeCryptFile(null, false, filePath);
                    // EnDeCryptUploadFile(null, false, filePath);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(oFile.Value))
            {
                EnDeCryptFile(oFile.PostedFile, false);
                // EnDeCryptUploadFile(oFile.PostedFile, false);
            }
        }

        /// <summary>
        /// ButtonEncrypt_Click fired when ButtonEncrypt for text encryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonEncrypt_Click(object sender, EventArgs e)
        {
            Reset_TextBox_IV(this.TextBox_Key.Text);

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                ClearPostedFileSession(false);

                cipherPipe = new CipherPipe(pipeAlgortihms);

                // None Encoding not possible, because we can't display binary non ASCII data in a TextBox
                if (encType == EncodingType.None || encType == EncodingType.Null)
                {
                    DropDownList_Encoding.SelectedValue = EncodingType.Base64.ToString();
                    encType = EncodingType.Base64; ;
                    CheckBoxEncode.Checked = true;
                    CheckBoxEncode.Enabled = true;
                }

                TextBoxDestionation.Text = cipherPipe.EncrpytTextGoRounds(TextBoxSource.Text, key, hash, encType, zipType, keyHash);

                SetBackgroundPicture("../res/img/crypt/AesBGText.gif");
            }
            else
            {
                this.TextBox_IV.Text = "TextBox source is empty!";
                this.TextBox_IV.ForeColor = Color.BlueViolet;
                this.TextBox_IV.BorderColor = Color.Blue;

                this.TextBoxSource.BorderColor = Color.BlueViolet;
                this.TextBoxSource.BorderStyle = BorderStyle.Dotted;
                this.TextBoxSource.BorderWidth = 2;

                SetBackgroundPicture("../res/img/crypt/AesImproveBG.gif");
            }
        }

        /// <summary>
        /// ButtonDecrypt_Click fired when ButtonDecrypt for text decryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonDecrypt_Click(object sender, EventArgs e)
        {
            Reset_TextBox_IV(this.TextBox_Key.Text);

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                ClearPostedFileSession(false);

                if (encType == EncodingType.None || encType == EncodingType.Null)
                {
                    DropDownList_Encoding.SelectedValue = EncodingType.Base64.ToString();
                    encType = EncodingType.Base64;
                    CheckBoxEncode.Checked = true;
                    CheckBoxEncode.Enabled = true;
                }

                cipherPipe = new CipherPipe(pipeAlgortihms);
                TextBoxDestionation.Text = cipherPipe.DecryptTextRoundsGo(TextBoxSource.Text, key, hash, encType, zipType, keyHash);

                SetBackgroundPicture("../res/img/crypt/AesBGText.gif");
            }
            else
            {
                this.TextBox_IV.Text = "TextBox source is empty!";
                this.TextBox_IV.ForeColor = Color.BlueViolet;
                this.TextBox_IV.BorderColor = Color.Blue;

                this.TextBoxSource.BorderColor = Color.BlueViolet;
                this.TextBoxSource.BorderStyle = BorderStyle.Dotted;
                this.TextBoxSource.BorderWidth = 2;

                SetBackgroundPicture("../res/img/crypt/AesImproveBG.gif");  
            }
        }

        #endregion page_events


        #region file_handling_members 

        /// <summary>
        /// Uploads a http posted file
        /// </summary>
        /// <param name="pfile"><see cref="HttpPostedFile"/></param>
        protected void UploadFile(HttpPostedFile pfile)
        {

            if (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0))
            {
                string strFileName = pfile.FileName;
                strFileName = System.IO.Path.GetFileName(strFileName).BeautifyUploadFileNames();
                string strFilePath = LibPaths.SystemDirOutPath + strFileName;

                if (Utils.DenyExtensionInOut(LibPaths.OutAppPath + strFileName))
                {
                    SpanLeftFile.Visible = true;
                    imgIn.Src = "../res/img/crypt/file_warning.gif";
                    SpanLabel.Visible = true;
                    uploadResult.Visible = true;
                    uploadResult.Text = "File extension \"" + System.IO.Path.GetExtension(strFilePath) +
                        "\" denied for upload!";

                    return;
                }

                pfile.SaveAs(strFilePath);

                if (System.IO.File.Exists(strFilePath))
                {
                    if (Utils.AllowExtensionInOut(LibPaths.OutAppPath + strFileName))
                    {
                        SetBackgroundPicture("../res/img/crypt/file_working.gif");
                    }
                    else
                    {
                        imgIn.Src = "../res/img/crypt/file_warning.gif";
                        uploadResult.Visible = true;
                        uploadResult.Text = "File ext \"" + System.IO.Path.GetExtension(strFilePath) +
                            "\" might be critically!";
                    }

                    Session[Constants.UPSAVED_FILE] = strFilePath;

                    SpanLabel.Visible = true;
                    SpanLeftFile.Visible = true;
                    SpanRightFile.Visible = false;
                    aUploaded.HRef = LibPaths.OutAppPath + strFileName;
                    imgIn.Alt = strFileName;
                }

            }
        }


        /// <summary>
        /// Encrypts or Decrypts uploaded file
        /// </summary>
        /// <param name="pfile">HttpPostedFile pfile</param>
        /// <param name="crypt">true for encrypt, false for decrypt</param>
        protected void EnDeCryptFile(HttpPostedFile pfile, bool crypt = true, string fileSavedName = "")
        {
            // Get the name of the file that is posted.
            string strFileName = (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0)) ?
                pfile.FileName : fileSavedName;
            strFileName = System.IO.Path.GetFileName(strFileName).BeautifyUploadFileNames();
            string strFilePath = LibPaths.SystemDirOutPath + strFileName;

            Reset_TextBox_IV(this.TextBox_Key.Text);

            if (Utils.DenyExtensionInOut(LibPaths.OutAppPath + strFileName))
            {
                SpanLeftFile.Visible = true;
                imgIn.Src = "../res/img/crypt/file_warning.gif";
                SpanLabel.Visible = true;
                uploadResult.Visible = true;
                uploadResult.Text = "File extension \"" + System.IO.Path.GetExtension(strFilePath) +
                    "\" denied for upload!";

                try
                {
                    if (System.IO.File.Exists(strFilePath))
                        System.IO.File.Delete(strFilePath);
                }
                catch (Exception exFile)
                {
                    Area23Log.LogOriginMsgEx("CoolCrypt.aspx", "EnDeCryptUploadFile", exFile);
                    try
                    {
                        System.IO.File.Move(strFilePath, strFilePath.Replace("/out/", "/tmp/") + DateTime.Now.Ticks);
                    }
                    catch { }
                }

                return;
            }

            string savedTransFile = "", outMsg = "";
            byte[] inBytes = new byte[65536];
            int strLen = 0;

            // CFile cFile;
            if (!string.IsNullOrEmpty(strFileName) &&
                ((pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0)) ||
                (!string.IsNullOrEmpty(fileSavedName) && System.IO.File.Exists(fileSavedName))))
            {
                if (!string.IsNullOrEmpty(fileSavedName) && System.IO.File.Exists(fileSavedName))
                    inBytes = System.IO.File.ReadAllBytes(fileSavedName);
                else if (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0))
                    inBytes = pfile.InputStream.ToByteArray();

                // cFile = new CFile(inBytes, strFileName);
                cipherPipe = new CipherPipe(pipeAlgortihms);

                // write source file hash
                this.TextBoxSource.Text =
                    "File: " + strFileName + "\n" +
                    "StreamLength: " + inBytes.Length + "\n" +
                    "MD5Sum " + MD5Sum.Hash(inBytes, strFileName) + "\n" +
                    "Sha256 " + Sha256Sum.Hash(inBytes, strFileName) + "\n";
                uploadResult.Text = "";

                byte[] outBytes = new byte[inBytes.Length];
                imgOut.Src = LibPaths.ResAppPath + "img/crypt/file_warning.gif";

                if (crypt)
                {
                    strFileName += zipType.ZipFileExtension(cipherPipe.PipeString);
                    // cFile.EncryptToJson(key, hash, encType, zipType, keyHash);
                    outBytes = cipherPipe.EncrpytFileBytesGoRounds(inBytes, key, hash, encType, zipType, keyHash);

                    if (CheckBoxEncode.Checked)
                    {
                        strFileName += "." + encType.ToString().ToLowerInvariant();
                        string outString = encType.GetEnCoder().EnCode(outBytes);
                        strLen = outString.Length;
                        savedTransFile = this.StringToFile(outString, out outMsg, strFileName, LibPaths.SystemDirOutPath);
                    }
                    else
                        savedTransFile = this.ByteArrayToFile(outBytes, out outMsg, strFileName, LibPaths.SystemDirOutPath);

                    imgOut.Src = LibPaths.ResAppPath + "img/crypt/encrypted.png";

                    if (!string.IsNullOrEmpty(savedTransFile) && !string.IsNullOrEmpty(outMsg))
                        uploadResult.Text = string.Format("{0}x crypt {1}", cipherPipe.PipeString.Length, outMsg);
                    else
                        uploadResult.Text = "file failed to encrypt and save!";
                }
                else // decrypt
                {
                    string decryptedText = string.Empty;
                    strFileName = strFileName.EndsWith(".hex") ? strFileName.Replace(".hex", "") : strFileName;
                    strFileName = strFileName.EndsWith(".oct") ? strFileName.Replace(".oct", "") : strFileName;

                    string ext = strFileName.GetExtensionFromFileString();
                    EncodingType extEncType = EncodingTypesExtensions.GetEncodingTypeFromFileExt(ext);
                    if (extEncType != EncodingType.None && extEncType != EncodingType.Null)
                    {
                        string error = "";
                        string cipherText = System.Text.Encoding.UTF8.GetString(inBytes);
                        if (!extEncType.GetEnCoder().IsValidShowError(cipherText, out error))
                        {
                            uploadResult.Text = "file isn't a valid " + extEncType.ToString() + " encoding. Invalid characters: " + error;
                            SpanLabel.Visible = true;
                            return;
                        }
                        inBytes = extEncType.GetEnCoder().DeCode(cipherText);

                        strFileName = strFileName.EndsWith("." + extEncType.GetEncodingFileExtension()) ? strFileName.Replace("." + extEncType.GetEncodingFileExtension(), "") : strFileName;
                    }

                    outBytes = cipherPipe.DecryptFileBytesRoundsGo(inBytes, key, hash, encType, zipType, keyHash);
                    strFileName = strFileName.Contains(zipType.ZipFileExtension(cipherPipe.PipeString)) ?
                        strFileName.Replace(zipType.ZipFileExtension(cipherPipe.PipeString), "") :
                        strFileName;

                    imgOut.Src = LibPaths.ResAppPath + "img/crypt/decrypted.png";
                    savedTransFile = ByteArrayToFile(outBytes, out outMsg, strFileName, LibPaths.SystemDirOutPath);
                    uploadResult.Text = string.Format("decrypt to {0}", outMsg);

                }

                // set a href to saved trans file
                aTransFormed.HRef = LibPaths.OutAppPath + savedTransFile;

                // write destination file hash
                this.TextBoxDestionation.Text =
                    "File: " + savedTransFile + "\n" +
                    "StreamLength: " + outBytes.Length + "\n" +
                    "MD5Sum " + MD5Sum.Hash(LibPaths.SystemDirOutPath + savedTransFile) + "\n" +
                    "Sha256 " + Sha256Sum.Hash(LibPaths.SystemDirOutPath + savedTransFile) + "\n";

                // Display the result of the upload.
                ClearPostedFileSession(true);

                SetBackgroundPicture("../res/img/crypt/AesBGFile.gif");
            }
            else
            {
                uploadResult.Text = "Click 'Browse' to select the file to upload.";
                ClearPostedFileSession(false);
                SpanLabel.Visible = true;

                SetBackgroundPicture("../res/img/crypt/AesImproveBG.gif");
            }

        }


        #endregion file_handling_members 

        #region helper methods

        /// <summary>
        /// SetBackgroundPicture sets background picture of <see cref="DivCryptImprove"/>
        /// </summary>
        /// <param name="bgPicBackUrl">url to background picture</param>
        protected void SetBackgroundPicture(string bgPicBackUrl)
        {
            //if (!bgPictureUrl.Contains(LibPaths.AppUrlPath) && bgPictureUrl.StartsWith("../res/"))
            //    bgPictureUrl = bgPictureUrl.Replace("../res/", LibPaths.AppUrlPath + "/res/");
            DivCryptImprove.Attributes["style"] = $"padding-left: 40px; margin-left: 2px; background-image: url('{bgPicBackUrl}'); background-repeat: no-repeat; background-color: transparent;";
            DivCryptImprove.Style["background-image"] = $"url('{bgPicBackUrl}')";
        }

        /// <summary>
        /// Resets TextBox Key_IV to standard value for <see cref="Constants.AUTHOR_EMAIL"/>
        /// </summary>
        /// <param name="userEmailKey">user email key to generate key bytes iv</param>
        protected void Reset_TextBox_IV(string userEmailKey = "")
        {
            if (!string.IsNullOrEmpty(userEmailKey))
                this.TextBox_Key.Text = userEmailKey;
            else if (string.IsNullOrEmpty(this.TextBox_Key.Text))
                this.TextBox_Key.Text = Constants.AUTHOR_EMAIL;
            Session[Constants.AES_ENVIROMENT_KEY] = this.TextBox_Key.Text;

            if (!Enum.TryParse<KeyHash>(RadioButtonList_Hash.SelectedValue, out keyHash))
                keyHash = KeyHash.Hex;
            if (!Enum.TryParse<ZipType>(DropDownList_Zip.SelectedValue, out zipType))
                zipType = ZipType.None;
            if (!Enum.TryParse<EncodingType>(DropDownList_Encoding.SelectedValue, out encType))
                encType = EncodingType.Base64;

            key = this.TextBox_Key.Text;
            hash = keyHash.Hash(TextBox_Key.Text);
            TextBox_IV.Text = hash;
            pipeAlgortihms = CipherEnumExtensions.ParsePipeText(this.TextBox_Encryption.Text);
            uploadResult.Text = ""; 

            this.TextBox_IV.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBox_IV.BorderColor = Color.LightGray;
            this.TextBox_IV.BorderStyle = BorderStyle.Solid;
            this.TextBox_IV.BorderWidth = 1;

            this.TextBoxSource.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBoxSource.BorderStyle = BorderStyle.Solid;
            this.TextBoxSource.BorderColor = Color.LightGray;
            this.TextBoxSource.BorderWidth = 1;

            this.TextBoxDestionation.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBoxDestionation.BorderStyle = BorderStyle.Solid;
            this.TextBoxDestionation.BorderColor = Color.LightGray;
            this.TextBoxDestionation.BorderWidth = 1;

            this.TextBox_Encryption.BorderStyle = BorderStyle.Solid;
            this.TextBox_Encryption.BorderColor = Color.LightGray;
            this.TextBox_Encryption.BorderWidth = 1;

            // this.LiteralWarn.Text = "Hint: 7zip compression is still noty implemented, please use only gzip, bzip2 and zip.";            

            if ((Session[Constants.UPSAVED_FILE] != null) && System.IO.File.Exists((string)Session[Constants.UPSAVED_FILE]))
            {
                SpanLabel.Visible = true;
                SpanLeftFile.Visible = true;
            }
            else
            {
                ClearPostedFileSession(false);
            }
            SpanRightFile.Visible = false;

            SetBackgroundPicture("../res/img/crypt/AesImproveBG.gif");

        }

        /// <summary>
        /// removes posted file from session and file location
        /// </summary>
        protected void ClearPostedFileSession(bool spansVisible = false)
        {
            if ((Session[Constants.UPSAVED_FILE] != null))
            {
                if (System.IO.File.Exists((string)Session[Constants.UPSAVED_FILE]))
                {
                    try
                    {
                        System.IO.File.Delete((string)Session[Constants.UPSAVED_FILE]);
                        Session.Remove(Constants.UPSAVED_FILE);
                    }
                    catch (Exception exi)
                    {
                        Area23Log.LogStatic(exi);
                    }
                }

            }
            imgIn.Alt = "";
            aUploaded.HRef = "#";

            SpanRightFile.Visible = spansVisible;
            SpanLeftFile.Visible = spansVisible;
            SpanLabel.Visible = spansVisible;
        }

        #endregion helper methods

    }

}