using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Zfx;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Controls;

namespace Area23.At.Mono.Crypt
{


    /// <summary>
    /// AesImprove is En-/De-cryption pipeline page 
    /// Former hash inside crypted bytestream is removed
    /// Feature to encrypt and decrypt simple plain text or files
    /// </summary>
    public partial class AesImprove : UIPage
    {
        CipherMode2 cmode2 = CipherMode2.ECB;
        EncodingType encType = EncodingType.Base64;
        ZipType zipType = ZipType.GZip;
        CipherEnum[] pipeAlgortihms = new CipherEnum[0];
        SecureCipherPipe cipherPipe;
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
            this.TextBox_Key.Text = Constants.AUTHOR_EMAIL;
            this.TextBoxSource.Text = "";
            this.TextBoxDestionation.Text = "";
            ResetVisibleDivs();

            if ((Session[Constants.AES_ENVIROMENT_KEY] != null))
                Session.Remove(Constants.AES_ENVIROMENT_KEY);

            SetBackgroundPicture("../res/img/crypt/AesImproveBG.gif");
        }

       

        /// <summary>
        /// Add encryption alog to encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageButton_Add_Click(object sender, EventArgs e)
        {
            pipeAlgortihms = CipherEnumExtensions.ParsePipeText(this.TextBox_Encryption.Text);
            if (pipeAlgortihms != null && pipeAlgortihms.Length > 8)
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

            if (!Enum.TryParse<CipherMode2>(this.DropDownList_CipherMode.SelectedValue, out cmode2))
                cmode2 = CipherMode2.ECB;

            pipeAlgortihms = CipherEnumExtensions.ParsePipeText(this.TextBox_Encryption.Text);
            if (pipeAlgortihms != null && pipeAlgortihms.Length > 0)
            {
                cipherPipe = new SecureCipherPipe(pipeAlgortihms, 8, encType, zipType, cmode2);
                SetCipherPipeImage(cipherPipe, false);
                return;
            }
        }

        /// <summary>
        /// Button_SetPipe_Click sets encryption pipeline only from <see cref="TextBox_Key"/>
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_SetPipe_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
            {
                Reset_TextBox_IV(this.TextBox_Key.Text);
                key = TextBox_Key.Text;

                CipherEnum[] cses = CipherEnumExtensions.FromString(key);
                this.TextBox_Encryption.Text = string.Empty;
                foreach (CipherEnum c in cses)
                {
                    this.TextBox_Encryption.Text += c.ToString() + ";";
                }

                pipeAlgortihms = CipherEnumExtensions.ParsePipeText(this.TextBox_Encryption.Text);
                if (pipeAlgortihms != null && pipeAlgortihms.Length > 0)
                {
                    cipherPipe = new SecureCipherPipe(pipeAlgortihms, 8, encType, zipType, cmode2);
                    SetCipherPipeImage(cipherPipe, false);
                }

                SetBackgroundPicture("../res/img/crypt/AesImproveBGWithPipe.gif");
                this.TextBox_Encryption.BorderStyle = BorderStyle.Double;
                this.TextBox_Encryption.BorderColor = Color.DarkOliveGreen;
                this.TextBox_Encryption.BorderWidth = 2;
            }
        }

        /// <summary>
        /// Button_HashPipe_Click hashes CipherPipe by setting <see cref="TextBox_Encryption"/> 
        /// driven primary by hash and secondary by key
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_HashPipe_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
            {
                Reset_TextBox_IV(this.TextBox_Key.Text);
                key = TextBox_Key.Text;
                hash = KeyHash.Whirlpool.Hash(KeyHash.SCrypt.Hash(key));

                CipherEnum[] cses = CipherEnumExtensions.FromString(hash);
                this.TextBox_Encryption.Text = string.Empty;
                foreach (CipherEnum cipher in cses)
                {
                    this.TextBox_Encryption.Text += cipher.ToString() + ";";
                }

                pipeAlgortihms = CipherEnumExtensions.ParsePipeText(this.TextBox_Encryption.Text);
                if (pipeAlgortihms != null && pipeAlgortihms.Length > 0)
                {
                    cipherPipe = new SecureCipherPipe(pipeAlgortihms, 8, encType, zipType, cmode2);
                    SetCipherPipeImage(cipherPipe, false);
                }

                SetBackgroundPicture("../res/img/crypt/AesImproveBGWithPipe.gif");
                this.TextBox_Encryption.BorderStyle = BorderStyle.Double;
                this.TextBox_Encryption.BorderColor = Color.Coral;
                this.TextBox_Encryption.BorderWidth = 2;
            }
        }


        protected void ImageButton_Delete_Click(object sender, EventArgs e)
        {
            TextBox_Encryption.Text = "";
            ResetVisibleDivs();
            Reset_TextBox_IV(this.TextBox_Key.Text);
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
                this.CheckBox_Encode.Checked = false;
                this.CheckBox_Encode.Enabled = false;
            }
            else if (!this.CheckBox_Encode.Enabled)
            {
                CheckBox_Encode.Enabled = true;
                CheckBox_Encode.Checked = true;
            }
        }


        protected void DropDownList_CipherMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if (this.DropDownList_CipherMode.SelectedValue.ToLowerInvariant() == "ecb")
            if (!Enum.TryParse<CipherMode2>(this.DropDownList_CipherMode.SelectedValue, out cmode2))
            {
                cmode2 = CipherMode2.ECB;
                this.DropDownList_CipherMode.SelectedIndex = 2;
            }
        }


        /// <summary>
        /// ButtonEncryptFile_Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_EncryptFile_Click(object sender, EventArgs e)
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
        protected void Button_DecryptFile_Click(object sender, EventArgs e)
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

        protected void Button_RandomText_Click(object sender, EventArgs e)
        {
            this.TextBoxSource.Text = Fortune.ExecFortune();
        }

        /// <summary>
        /// ButtonEncrypt_Click fired when ButtonEncrypt for text encryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Encrypt_Click(object sender, EventArgs e)
        {
            Reset_TextBox_IV(this.TextBox_Key.Text);

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                ClearPostedFileSession(false);

                cipherPipe = new SecureCipherPipe(pipeAlgortihms, 8, encType, zipType, cmode2);

                // None Encoding not possible, because we can't display binary non ASCII data in a TextBox
                if (encType == EncodingType.None || encType == EncodingType.Null)
                {
                    DropDownList_Encoding.SelectedValue = EncodingType.Base64.ToString();
                    encType = EncodingType.Base64; ;
                    CheckBox_Encode.Checked = true;
                    CheckBox_Encode.Enabled = true;
                }

                TextBoxDestionation.Text = cipherPipe.EncrpytTextGoRounds(TextBoxSource.Text,
                    key, encType, zipType, cmode2);

                SetCipherPipeImage(cipherPipe, false);

                SetBackgroundPicture("../res/img/crypt/AesBGTextWithPipe.gif");
            }
            else
            {
                this.TextBoxSource.BorderColor = Color.BlueViolet;
                this.TextBoxSource.BorderStyle = BorderStyle.Dotted;
                this.TextBoxSource.BorderWidth = 2;

                SetBackgroundPicture("../res/img/crypt/AesImproveBGWithPipe.gif");
            }
        }

        /// <summary>
        /// ButtonDecrypt_Click fired when ButtonDecrypt for text decryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Decrypt_Click(object sender, EventArgs e)
        {
            Reset_TextBox_IV(this.TextBox_Key.Text);

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                ClearPostedFileSession(false);

                if (encType == EncodingType.None || encType == EncodingType.Null)
                {
                    DropDownList_Encoding.SelectedValue = EncodingType.Base64.ToString();
                    encType = EncodingType.Base64;
                    CheckBox_Encode.Checked = true;
                    CheckBox_Encode.Enabled = true;
                }

                cipherPipe = new SecureCipherPipe(pipeAlgortihms, 8, encType, zipType, cmode2);
                TextBoxDestionation.Text = cipherPipe.DecryptTextRoundsGo(TextBoxSource.Text,
                    key, encType, zipType, cmode2);

                SetCipherPipeImage(cipherPipe, true);

                SetBackgroundPicture("../res/img/crypt/AesBGTextWithPipe.gif");
            }
            else
            {
                this.TextBoxSource.BorderColor = Color.BlueViolet;
                this.TextBoxSource.BorderStyle = BorderStyle.Dotted;
                this.TextBoxSource.BorderWidth = 2;

                SetBackgroundPicture("../res/img/crypt/AesImproveBGWithPipe.gif");
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
                    ResetVisibleDivs();
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
            ResetVisibleDivs();

            string savedTransFile = "", outMsg = "";
            byte[] inBytes = new byte[65536];
            int strLen = 0;

            if (!Enum.TryParse<CipherMode2>(this.DropDownList_CipherMode.SelectedValue, out cmode2))
                cmode2 = CipherMode2.ECB;

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
                cipherPipe = new SecureCipherPipe(pipeAlgortihms, 8, encType, zipType, cmode2);

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
                    // cipherPipe.EncrpytFileBytesGoRounds(inBytes, key, hash, encType, zipType, keyHash);
                    outBytes = cipherPipe.EncryptEncodeBytes(inBytes, key,
                        (CheckBox_Encode.Checked) ? encType : EncodingType.None,
                        zipType, cmode2);

                    if (CheckBox_Encode.Checked)
                    {
                        strFileName += "." + encType.ToString().ToLowerInvariant();
                    }
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
                        // inBytes = extEncType.GetEnCoder().DeCode(cipherText);

                        // strFileName = strFileName.EndsWith("." + extEncType.GetEncodingFileExtension()) ? strFileName.Replace("." + extEncType.GetEncodingFileExtension(), "") : strFileName;
                    }

                    outBytes = cipherPipe.DecodeDecrpytBytes(inBytes, key, extEncType, zipType, cmode2);

                    strFileName = strFileName.StripCiphersInFileName();

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

            zipType = ZipType.GZip;
            if (!Enum.TryParse<EncodingType>(DropDownList_Encoding.SelectedValue, out encType))
                encType = EncodingType.Base64;
            if (!Enum.TryParse<CipherMode2>(this.DropDownList_CipherMode.SelectedValue, out cmode2))
                cmode2 = CipherMode2.ECB;

            key = this.TextBox_Key.Text;            
            pipeAlgortihms = CipherEnumExtensions.ParsePipeText(this.TextBox_Encryption.Text);
            uploadResult.Text = "";

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

        protected void ResetVisibleDivs()
        {
            divHint.Visible = true;
            divFileResult.Visible = true;
            divPipeImage.Visible = false;
        }

        protected void SetCipherPipeImage(SecureCipherPipe pipe, bool inverse = false)
        {
            divHint.Visible = false;
            divPipeImage.Visible = true;
            divFileResult.Visible = false;
            Bitmap encBmp = (inverse) ? (new Bitmap(pipe.GenerateDecryptPipeImage())) : new Bitmap(pipe.GenerateEncryptPipeImage());
            MemoryStream ms = new MemoryStream();
            encBmp.Save(ms, ImageFormat.Gif);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            this.imgPipe.Src = "data:image/gif;base64," + base64Data;
        }

        #endregion helper methods

    }

}