using Area23.At;
using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Cipher;
using Area23.At.Framework.Library.Cipher.Symmetric;
using Area23.At.Framework.Library.EnDeCoding;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Zfx;
using Area23.At.Mono.Properties;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace Area23.At.Mono.Crypt
{
    /// <summary>
    /// SAES_En_Decrypt En-/De-cryption pipeline page 
    /// Feature to encrypt and decrypt simple plain text or files
    /// </summary>
    public partial class CoolCrypt : Util.UIPage
    {        

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
                    (((string)Session[Constants.AES_ENVIROMENT_KEY]).Length > 7))
                {
                    Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);
                }            
                else
                    Reset_TextBox_IV(Constants.AUTHOR_EMAIL);
            }

            if ((Request.Files != null && Request.Files.Count > 0) || (!String.IsNullOrEmpty(oFile.Value)))
            {
                UploadFile(oFile.PostedFile); 
            }
        }

        #region page_events

        /// <summary>
        /// ButtonEncryptFile_Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonEncryptFile_Click(object sender, EventArgs e)
        {
            if (SpanLeftFile.Visible && aUploaded.HRef.Contains(Constants.OUT_DIR) && !string.IsNullOrEmpty(img1.Alt))
            {
                string filePath = LibPaths.OutDirPath + img1.Alt;
                if (System.IO.File.Exists(filePath))
                {
                    EnDeCryptUploadFile(null, true, filePath);
                    return;
                }
            }            
            
            if (Request.Files != null && Request.Files.Count > 0)
                EnDeCryptUploadFile(Request.Files[0], true);
        }

        /// <summary>
        /// ButtonDecryptFile_Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonDecryptFile_Click(object sender, EventArgs e)
        {
            if (SpanLeftFile.Visible && aUploaded.HRef.Contains(Constants.OUT_DIR) && !string.IsNullOrEmpty(img1.Alt))
            {
                string filePath = LibPaths.OutDirPath + img1.Alt;
                if (System.IO.File.Exists(filePath))
                {
                    EnDeCryptUploadFile(null, false, filePath);
                    return;
                }
            }

            if (!String.IsNullOrEmpty(oFile.Value))
            {
                EnDeCryptUploadFile(oFile.PostedFile, false);
            }
        }

        /// <summary>
        /// ButtonEncrypt_Click fired when ButtonEncrypt for text encryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonEncrypt_Click(object sender, EventArgs e)
        {
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            string keyIv = (!string.IsNullOrEmpty(this.TextBox_IV.Text)) ? this.TextBox_IV.Text : Constants.AUTHOR_IV;
            EncodingType encodeType = (EncodingType)Enum.Parse(typeof(EncodingType), this.DropDownList_Encoding.SelectedValue);
            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                ClearPostedFileSession(false);

                if (encodeType == EncodingType.None || encodeType == EncodingType.Null)
                {
                    for (int it = 0; it < DropDownList_Encoding.Items.Count; it++)
                    {
                        if (DropDownList_Encoding.Items[it].Value == EncodingType.Base64.ToString())
                            DropDownList_Encoding.Items[it].Selected = true;
                        else DropDownList_Encoding.Items[it].Selected = false;
                    }
                    encodeType = (EncodingType)Enum.Parse(typeof(EncodingType), this.DropDownList_Encoding.SelectedValue);
                    this.CheckBoxEncode.Checked = true;
                    this.CheckBoxEncode.Enabled = true;
                }

                Reset_TextBox_IV(usrMailKey);
                byte[] inBytes = Encoding.UTF8.GetBytes(this.TextBoxSource.Text);
                // string source = this.TextBoxSource.Text + "\r\n" + this.TextBox_IV.Text;
                byte[] encryptBytes = inBytes;

                ZipType ztype = ZipType.None;
                //if (Enum.TryParse<ZipType>(DropDownList_Zip.SelectedValue, out ztype))
                //{
                //    string outp = string.Empty;
                //    string zfile = DateTime.UtcNow.Area23DateTimeWithMillis();
                //    string zPath = encryptBytes.ToFile(LibPaths.OutDirPath, zfile, ".txt");
                //    string zOutPath = zPath;
                //    switch (ztype)
                //    {
                //        case ZipType.GZip:
                //            //zOutPath += ".gz";
                //            //outp = ProcessCmd.Execute(LibPaths.BinDir + "gz.bat ", " " + zPath + "  " + zOutPath, false);
                //            //if (System.IO.File.Exists(zOutPath))
                //            //    inBytes = System.IO.File.ReadAllBytes(zOutPath);
                //            //break;
                //        case ZipType.BZip2:                           
                //        case ZipType.Z7:
                //        // case ZipType.Zip: inBytes = Zip
                //        case ZipType.None:
                //        default: break;
                //    }
                //}


                string[] algos = this.TextBox_Encryption.Text.Split("+;,→⇛".ToCharArray());
                foreach (string algo in algos)
                {
                    if (!string.IsNullOrEmpty(algo))
                    {
                        CipherEnum cipherAlgo = CipherEnum.None;
                        if (Enum.TryParse<CipherEnum>(algo, out cipherAlgo))
                        {
                            encryptBytes = Framework.Library.Cipher.Crypt.EncryptBytes(inBytes, cipherAlgo, secretKey, keyIv);
                            inBytes = encryptBytes;                            
                        }
                    }
                }

                bool fromPlain = string.IsNullOrEmpty(this.TextBox_Encryption.Text);
                // string encodingMethod = this.DropDownList_Encoding.SelectedValue.ToLowerInvariant();
                encodeType = (EncodingType)Enum.Parse(typeof(EncodingType), this.DropDownList_Encoding.SelectedValue);
                string encryptedText = DeEnCoder.EncodeBytes(encryptBytes, encodeType, fromPlain, false);

                this.TextBoxDestionation.Text = encryptedText;

                DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesBGText.gif'); background-repeat: no-repeat; background-color: transparent;";
                DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesBGText.gif')";
                DivAesImprove.Style["background-image"] = "url('../res/img/AesBGText.gif')";
            }
            else
            {
                this.TextBox_IV.Text = "TextBox source is empty!";
                this.TextBox_IV.ForeColor = Color.BlueViolet;
                this.TextBox_IV.BorderColor = Color.Blue;

                this.TextBoxSource.BorderColor = Color.BlueViolet;
                this.TextBoxSource.BorderStyle = BorderStyle.Dotted;
                this.TextBoxSource.BorderWidth = 2;


                DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImprotveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
                DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImprotveBG.gif')";
                DivAesImprove.Style["background-image"] = "url('../res/img/AesImprotveBG.gif')";
            }
        }

        /// <summary>
        /// ButtonDecrypt_Click fired when ButtonDecrypt for text decryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonDecrypt_Click(object sender, EventArgs e)
        {
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            string keyIv = (!string.IsNullOrEmpty(this.TextBox_IV.Text)) ? this.TextBox_IV.Text : Constants.AUTHOR_IV;
            EncodingType encodeType = (EncodingType)Enum.Parse(typeof(EncodingType), this.DropDownList_Encoding.SelectedValue);            
            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;            

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                ClearPostedFileSession(false);

                if (encodeType == EncodingType.None || encodeType == EncodingType.Null)
                {
                    for (int it = 0; it < DropDownList_Encoding.Items.Count; it++)
                    {
                        if (DropDownList_Encoding.Items[it].Value == EncodingType.Base64.ToString())
                            DropDownList_Encoding.Items[it].Selected = true;
                        else DropDownList_Encoding.Items[it].Selected = false;
                    }
                    encodeType = EncodingType.Base64;
                    this.CheckBoxEncode.Checked = true;
                    this.CheckBoxEncode.Enabled = true;
                }
                
                Reset_TextBox_IV(usrMailKey);

                string cipherText = this.TextBoxSource.Text;
                bool plainUu = string.IsNullOrEmpty(this.TextBox_Encryption.Text);
                string decryptedText = string.Empty;
                // encodeType = (EncodingType)Enum.Parse(typeof(EncodingType), this.DropDownList_Encoding.SelectedValue);
                string encodingMethod = encodeType.ToString().ToLowerInvariant();
                byte[] cipherBytes = DeEnCoder.DecodeText(cipherText, out string errMsg, encodeType, plainUu, false);
                if (cipherBytes == null && !string.IsNullOrEmpty(errMsg))
                {
                    this.TextBox_IV.Text = errMsg;
                    this.TextBox_IV.ForeColor = Color.BlueViolet;
                    this.TextBox_IV.BorderColor = Color.Blue;
                    return;
                }

                byte[] decryptedBytes = cipherBytes;
                int ig = 0;

                string[] algos = this.TextBox_Encryption.Text.Split("+;,→⇛".ToCharArray());
                for (ig = (algos.Length - 1); ig >= 0; ig--)
                {
                    if (!string.IsNullOrEmpty(algos[ig]))
                    {
                        CipherEnum cipherAlgo = CipherEnum.None;
                        if (Enum.TryParse<CipherEnum>(algos[ig], out cipherAlgo))
                        {                            
                            decryptedBytes = Framework.Library.Cipher.Crypt.DecryptBytes(cipherBytes, cipherAlgo, secretKey, keyIv);
                            cipherBytes = decryptedBytes;
                        }
                    }
                }

                cipherBytes = DeEnCoder.GetBytesTrimNulls(decryptedBytes);
                ZipType ztype = ZipType.None;
                //if (Enum.TryParse<ZipType>(DropDownList_Zip.SelectedValue, out ztype))
                //{
                //    string outp = string.Empty;
                //    string zfile = DateTime.UtcNow.Area23DateTimeWithMillis();
                //    string zPath, zOutPath;

                //    switch (ztype)
                //    {
                //        case ZipType.GZip:
                //            //zPath = cipherBytes.ToFile(LibPaths.OutDirPath, zfile, ".txt.gz");
                //            //zOutPath = zPath.Replace(".txt.gz", ".asc.txt");
                //            //if (System.IO.File.Exists(zPath))
                //            //{
                //            //    outp = ProcessCmd.Execute(LibPaths.BinDir + "gunzip.bat", zPath + " " + zOutPath, false);
                //            //    if (System.IO.File.Exists(zOutPath))
                //            //        decryptedBytes = System.IO.File.ReadAllBytes(zOutPath);
                //            //}
                //            //break;
                //        case ZipType.BZip2:                            
                //        case ZipType.Z7:                            
                //        // case ZipType.Zip: inBytes = Zip
                //        case ZipType.None:
                //        default: decryptedBytes = cipherBytes; break;
                //    }
                //}

                decryptedText = DeEnCoder.GetStringFromBytesTrimNulls(decryptedBytes);
                this.TextBoxDestionation.Text = decryptedText; // HandleString_PrivateKey_Changed(decryptedText);

                DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesBGText.gif'); background-repeat: no-repeat; background-color: transparent;";
                DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesBGText.gif')";
                DivAesImprove.Style["background-image"] = "url('../res/img/AesBGText.gif')";
            }
            else
            {
                this.TextBox_IV.Text = "TextBox source is empty!";
                this.TextBox_IV.ForeColor = Color.BlueViolet;
                this.TextBox_IV.BorderColor = Color.Blue;

                this.TextBoxSource.BorderColor = Color.BlueViolet;
                this.TextBoxSource.BorderStyle = BorderStyle.Dotted;
                this.TextBoxSource.BorderWidth = 2;

                DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImprotveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
                DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImprotveBG.gif')";
                DivAesImprove.Style["background-image"] = "url('../res/img/AesImprotveBG.gif')";
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
            ClearPostedFileSession(false);

            DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImprotveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
            DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImprotveBG.gif')";
            DivAesImprove.Style["background-image"] = "url('../res/img/AesImprotveBG.gif')";
        }

        /// <summary>
        /// Add encryption alog to encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageButton_Add_Click(object sender, EventArgs e)
        {
            foreach (string cryptName in Enum.GetNames(typeof(CipherEnum)))
            {
                if (cryptName != "None")
                {
                    if (DropDownList_Cipher.SelectedValue.ToString() == cryptName)
                    {
                        string addChiffre = DropDownList_Cipher.SelectedValue.ToString() + ";";
                        this.TextBox_Encryption.Text += addChiffre;
                        this.TextBox_Encryption.BorderStyle = BorderStyle.Double;

                        DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImproveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
                        DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImproveBG.gif')";
                        DivAesImprove.Style["background-image"] = "url('../res/img/AesImproveBG.gif')";
                        break;
                    }
                }
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
        /// TextBox_Key_TextChanged - fired on <see cref="TextBox_Key"/> TextChanged event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void TextBox_Key_TextChanged(object sender, EventArgs e)
        {
            this.TextBox_IV.Text = DeEnCoder.KeyHexString(this.TextBox_Key.Text);
            this.TextBox_IV.BorderColor = Color.GreenYellow;
            this.TextBox_IV.ForeColor = Color.DarkOliveGreen;
            this.TextBox_IV.BorderStyle = BorderStyle.Dotted;
            this.TextBox_IV.BorderWidth = 1;

            this.TextBox_Encryption.BorderStyle = BorderStyle.Solid;


            DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImproveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
            DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImproveBG.gif')";
            DivAesImprove.Style["background-image"] = "url('../res/img/AesImproveBG.gif')";
        }

        /// <summary>
        /// Button_Reset_KeyIV_Click resets <see cref="TextBox_Key"/> and <see cref="TextBox_IV"/> to default loaded values
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>

        protected void Button_Reset_KeyIV_Click(object sender, EventArgs e)
        {
            Reset_TextBox_IV(Constants.AUTHOR_EMAIL);
        }


        /// <summary>
        /// Saves current email address as crypt key inside that asp Session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ImageButton_Key_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 7)
            {
                Session[Constants.AES_ENVIROMENT_KEY] = this.TextBox_Key.Text;
                Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);

                byte[] kb = CryptHelper.GetUserKeyBytes(this.TextBox_Key.Text, this.TextBox_IV.Text, 16);
                SymmCipherEnum[] cses = Framework.Library.Cipher.Crypt.KeyBytesToSymmCipherPipeline(kb);
                this.TextBox_Encryption.Text = string.Empty;
                foreach (SymmCipherEnum c in cses)
                {
                    switch (c)
                    {
                        case SymmCipherEnum.ThreeFish:
                            this.TextBox_Encryption.Text += "FISH3" + ";";
                            break;
                        case SymmCipherEnum.TwoFish:
                            this.TextBox_Encryption.Text += "FISH2" + ";";
                            break;
                        case SymmCipherEnum.TripleDes:
                            this.TextBox_Encryption.Text += "DES3" + ";";
                            break;
                        case SymmCipherEnum.Aes:
                            this.TextBox_Encryption.Text += "AES" + ";";
                            break;
                        default:
                            this.TextBox_Encryption.Text += c.ToString() + ";";
                            break;
                    }
                }
                
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
                strFileName = Path.GetFileName(strFileName);
                string strFilePath = LibPaths.OutDirPath + strFileName;
                pfile.SaveAs(strFilePath);

                if (System.IO.File.Exists(strFilePath))
                {
                    lblUploadResult.Text = strFileName + " has been successfully uploaded.";
                    Session[Constants.UPSAVED_FILE] = strFilePath;

                    SpanLabel.Visible = true;
                    SpanLeftFile.Visible = true;
                    SpanRightFile.Visible = false;
                    aUploaded.HRef = LibPaths.OutAppPath + strFileName;
                    img1.Alt = strFileName;
                }
            }
        }

        /// <summary>
            /// Encrypts or Decrypts uploaded file
            /// </summary>
            /// <param name="pfile">HttpPostedFile pfile</param>
            /// <param name="crypt">true for encrypt, false for decrypt</param>
        protected void EnDeCryptUploadFile(HttpPostedFile pfile, bool crypt = true, string fileSavedName = "")
        {
            // Get the name of the file that is posted.
            string strFileName = (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0)) ? 
                pfile.FileName : fileSavedName;
            strFileName = Path.GetFileName(strFileName);

            string savedTransFile = string.Empty;
            string outMsg = string.Empty;            
            EncodingType encodeType = (EncodingType)Enum.Parse(typeof(EncodingType), this.DropDownList_Encoding.SelectedValue);
            EncodingType extEncType = encodeType;
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            string keyIv = (!string.IsNullOrEmpty(this.TextBox_IV.Text)) ? this.TextBox_IV.Text : Constants.AUTHOR_IV;
            string encodingMethod = encodeType.ToString().ToLowerInvariant();
            bool plainUu = string.IsNullOrEmpty(this.TextBox_Encryption.Text);

            if ((pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0)) ||
                (!string.IsNullOrEmpty(fileSavedName) && System.IO.File.Exists(fileSavedName)))
            {
                byte[] fileBytes = (!string.IsNullOrEmpty(fileSavedName) && System.IO.File.Exists(fileSavedName)) ?
                     System.IO.File.ReadAllBytes(fileSavedName) : (
                        (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0)) ?
                            pfile.InputStream.ToByteArray() : new byte[65536]);

                lblUploadResult.Text = "";

                byte[] outBytes = new byte[fileBytes.Length];

                if (!string.IsNullOrEmpty(strFileName))
                {
                    string[] algos = this.TextBox_Encryption.Text.Split("+;,→⇛".ToCharArray());
                    string baseEncoding = this.DropDownList_Encoding.SelectedValue.ToLowerInvariant();

                    int cryptCount = 0;
                    outBytes = fileBytes;
                    Array.Copy(fileBytes, 0, outBytes, 0, fileBytes.Length);

                    if (crypt)
                    {
                        byte[] inBytes = fileBytes; //.TarBytes(inBytesSeperator, inBytesKeyHash);                            

                        imgOut.Src = LibPaths.ResAppPath + "img/encrypted.png";

                        ZipType ztype = ZipType.None;
                        //if (Enum.TryParse<ZipType>(DropDownList_Zip.SelectedValue, out ztype))
                        //{
                        //    string outp = string.Empty;
                        //    string zfile = DateTime.UtcNow.Area23DateTimeWithMillis();
                        //    string zPath = fileBytes.ToFile(LibPaths.OutDirPath, zfile, ".txt");
                        //    string zOutPath = zPath;
                        //    switch (ztype)
                        //    {
                        //        case ZipType.GZip:
                        //            //zOutPath += ".gz";
                        //            //outp = ProcessCmd.Execute(LibPaths.BinDir + "gz.bat ", " " + zPath + "  " + zOutPath, false);
                        //            //if (System.IO.File.Exists(zOutPath))
                        //            //    inBytes = System.IO.File.ReadAllBytes(zOutPath);
                        //            //break;
                        //        case ZipType.BZip2:
                        //        case ZipType.Z7:
                        //        // case ZipType.Zip: inBytes = Zip
                        //        case ZipType.None:
                        //        default: break;
                        //    }
                        //}


                        foreach (string algo in algos)
                        {
                            if (!string.IsNullOrEmpty(algo))
                            {
                                CipherEnum cipherAlgo = CipherEnum.None;
                                if (Enum.TryParse<CipherEnum>(algo, out cipherAlgo))
                                {
                                    outBytes = Framework.Library.Cipher.Crypt.EncryptBytes(inBytes, cipherAlgo, secretKey, keyIv);                                   
                                    inBytes = outBytes;
                                    cryptCount++;
                                    strFileName += "." + algo.ToLower();
                                }
                            }
                        }

                        if (CheckBoxEncode.Checked)
                        {
                            strFileName += "." + encodeType.ToString().ToLowerInvariant();
                            string outString = DeEnCoder.EncodeBytes(outBytes, encodeType, plainUu, true);
                            savedTransFile = this.StringToFile(outString, out outMsg, strFileName);
                        }
                        else
                        {
                            savedTransFile = this.ByteArrayToFile(outBytes, out outMsg, strFileName);
                        }

                        if (!string.IsNullOrEmpty(savedTransFile) && !string.IsNullOrEmpty(outMsg))
                            lblUploadResult.Text = string.Format("{0}x crypt {1}", cryptCount, outMsg);
                        else
                            lblUploadResult.Text = "file failed to encrypt and save!";
                    }
                    else
                    {

                        string decryptedText = string.Empty;
                        byte[] cipherBytes = fileBytes;
                        Array.Copy(fileBytes, 0, cipherBytes, 0, fileBytes.Length);

                        bool decode = false;
                        string ext = strFileName.GetExtensionFromFileString();
                        foreach (var encType in EncodingTypesExtensions.GetEncodingTypes())
                        {
                            if (ext.Equals(encType.ToString(), StringComparison.OrdinalIgnoreCase) ||
                                ext.Equals(encType.ToString(), StringComparison.InvariantCultureIgnoreCase) ||
                                ext.ToLowerInvariant() == encType.ToString().ToLowerInvariant() ||
                                ext.Equals("." + encType.ToString(), StringComparison.InvariantCultureIgnoreCase) ||
                                ext.ToLowerInvariant() == "." + encType.ToString().ToLowerInvariant())
                            {
                                decode = true;
                                extEncType = encType;
                                break;
                            }
                        }
                        if (decode)
                        {
                            encodingMethod = (ext.StartsWith(".")) ? ext.ToLowerInvariant().Substring(1) : ext.ToLowerInvariant();
                            string cipherText = EnDeCoder.GetString(fileBytes);
                            string tmpFile = ByteArrayToFile(fileBytes, out outMsg, strFileName + ".tmp");
                            // tmpFile = tmpFile.Replace(".hex", ".tmp");
                            if (System.IO.File.Exists(LibPaths.OutDirPath + tmpFile))
                            {
                                cipherText = System.IO.File.ReadAllText(LibPaths.OutDirPath + tmpFile, Encoding.UTF8);
                            }

                            cipherBytes = DeEnCoder.DecodeText(cipherText, out string errMsg, extEncType, plainUu, true);
                            strFileName = strFileName.EndsWith("." + encodingMethod) ? strFileName.Replace("." + encodingMethod, "") : strFileName;
                        }

                        strFileName = strFileName.EndsWith(".hex") ? strFileName.Replace(".hex", "") : strFileName;
                        strFileName = strFileName.EndsWith(".oct") ? strFileName.Replace(".oct", "") : strFileName;
                        imgOut.Src = LibPaths.ResAppPath + "img/decrypted.png";

                        for (int ig = (algos.Length - 1); ig >= 0; ig--)
                        {
                            if (!string.IsNullOrEmpty(algos[ig]))
                            {
                                CipherEnum cipherAlgo = CipherEnum.None;
                                if (Enum.TryParse<CipherEnum>(algos[ig], out cipherAlgo))
                                {
                                    outBytes = Framework.Library.Cipher.Crypt.DecryptBytes(cipherBytes, cipherAlgo, secretKey, keyIv);
                                    cipherBytes = outBytes;
                                    cryptCount++;
                                    strFileName = strFileName.EndsWith("." + algos[ig].ToLower()) ? strFileName.Replace("." + algos[ig].ToLower(), "") : strFileName;
                                }
                            }
                        }

                        cipherBytes = DeEnCoder.GetBytesTrimNulls(outBytes);
                        outBytes = cipherBytes;

                        ZipType ztype = ZipType.None;
                        //if (Enum.TryParse<ZipType>(DropDownList_Zip.SelectedValue, out ztype))
                        //{
                        //    string outp = string.Empty;
                        //    string zfile = DateTime.UtcNow.Area23DateTimeWithMillis();
                        //    string zPath, zOutPath;

                        //    switch (ztype)
                        //    {
                        //        case ZipType.GZip:
                        //            //zPath = cipherBytes.ToFile(LibPaths.OutDirPath, zfile, ".txt.gz");
                        //            //zOutPath = zPath.Replace(".txt.gz", ".asc.txt");
                        //            //if (System.IO.File.Exists(zPath))
                        //            //{
                        //            //    outp = ProcessCmd.Execute(LibPaths.BinDir + "gunzip.bat", zPath + " " + zOutPath, false);
                        //            //    if (System.IO.File.Exists(zOutPath))
                        //            //        outBytes = System.IO.File.ReadAllBytes(zOutPath);
                        //            //}
                        //            //break;
                        //        case ZipType.BZip2:
                        //        case ZipType.Z7:
                        //        // case ZipType.Zip: inBytes = Zip
                        //        case ZipType.None:
                        //        default: outBytes = cipherBytes; break;
                        //    }
                        //}

                        cipherBytes = DeEnCoder.GetBytesTrimNulls(outBytes);
                        savedTransFile = this.ByteArrayToFile(cipherBytes, out outMsg, strFileName);
                        // if (success)
                        lblUploadResult.Text = string.Format("decrypt to {0}", outMsg);
                        // else
                        // lblUploadResult.Text = "decrypting file failed, byte trash saved  to ";                            
                    }

                    aTransFormed.HRef = LibPaths.OutAppPath + savedTransFile;
                    // lblUploadResult.Text += outMsg;
                }

                // Display the result of the upload.
                ClearPostedFileSession(true);

                DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesBGFile.gif'); background-repeat: no-repeat; background-color: transparent;";
                DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesBGFile.gif')";
                DivAesImprove.Style["background-image"] = "url('../res/img/AesBGFile.gif')";
            }
            else
            {
                lblUploadResult.Text = "Click 'Browse' to select the file to upload.";
                ClearPostedFileSession(false);
                SpanLabel.Visible = true;

                DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImproveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
                DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImproveBG.gif')";
                DivAesImprove.Style["background-image"] = "url('../res/img/AesImproveBG.gif')";
            }

        }

        #endregion file_handling_members 


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

            this.TextBox_IV.Text = DeEnCoder.KeyHexString(this.TextBox_Key.Text);

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

            DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImproveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
            DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImproveBG.gif')";
            DivAesImprove.Style["background-image"] = "url('../res/img/AesImproveBG.gif')";

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
            img1.Alt = "";
            aUploaded.HRef = "#";

            SpanRightFile.Visible = spansVisible;
            SpanLeftFile.Visible = spansVisible;
            SpanLabel.Visible = spansVisible;
        }

        #region ObsoleteDeprecated

        /// <summary>
        /// Handles string decryption, compares if private key & hex hash match in decrypted text
        /// </summary>
        /// <param name="decryptedText">decrypted plain text</param>
        /// <returns>decrypted plain text without check hash or an error message, in case that check hash doesn't match.</returns>
        [Obsolete("HandleString_PrivateKey_Changed is non standard bogus implementation, don't use it!", false)]
        protected string HandleString_PrivateKey_Changed(string decryptedText)
        {
            bool sameKey = false;
            string shouldEndWithIv = "\r\n" + this.TextBox_IV.Text;
            if (decryptedText != null && decryptedText.Length > this.TextBox_IV.Text.Length)
            {
                if ((sameKey = decryptedText.EndsWith(shouldEndWithIv, StringComparison.InvariantCultureIgnoreCase)))
                    decryptedText = decryptedText.Substring(0, decryptedText.Length - shouldEndWithIv.Length);
                else
                {
                    if ((sameKey = decryptedText.Contains(shouldEndWithIv)))
                    {
                        int idxEnd = decryptedText.IndexOf(shouldEndWithIv);
                        decryptedText = decryptedText.Substring(0, idxEnd);
                    }
                    else if ((sameKey = decryptedText.Contains(shouldEndWithIv.Substring(0, shouldEndWithIv.Length -3))))
                    {
                        int idxEnd = decryptedText.IndexOf(shouldEndWithIv.Substring(0, shouldEndWithIv.Length - 3));
                        decryptedText = decryptedText.Substring(0, idxEnd);
                    }
                }
            }

            if (!sameKey)
            {
                string errorMsg = $"Decryption failed!\r\nKey: {this.TextBox_Key.Text} with HexHash: {this.TextBox_Key.Text} doesn't match!";
                this.TextBox_IV.Text = "Private Key changed!";
                this.TextBox_IV.ToolTip = "Check Enforce decrypt (without key check).";
                this.TextBox_IV.BorderColor = Color.Red;
                this.TextBox_IV.BorderWidth = 2;

                return errorMsg;
            }

            return decryptedText;
        }

        /// <summary>
        /// Handles decrypted byte[] and checks hash of private key
        /// TODO: not well implemented yet, need to rethink hash merged at end of files with huge byte stream
        /// </summary>
        /// <param name="decryptedBytes">huge file bytes[], that contains at the end the CR + LF + iv key hash</param>
        /// <param name="success">out parameter, if finding and trimming the CR + LF + iv key hash was successfully</param>
        /// <returns>an trimmed proper array of huge byte, representing the file, otherwise a huge (maybe wrong decrypted) byte trash</returns>
        [Obsolete("HandleBytes_PrivateKey_Changed is non standard bogus implementation, don't use it!", false)]
        protected byte[] HandleBytes_PrivateKey_Changed(byte[] decryptedBytes, out bool success)
        {
            success = false;
            byte[] outBytesSameKey = null;
            byte[] ivBytesHash = EnDeCoder.GetBytes("\r\n" + this.TextBox_IV.Text);
            // Framework.Library.Cipher.Symmetric.CryptHelper.GetBytesFromString("\r\n" + this.TextBox_IV.Text, 256, false);
            if (decryptedBytes != null && decryptedBytes.Length > ivBytesHash.Length)
            {
                int needleFound = Framework.Library.Util.Extensions.BytesBytes(decryptedBytes, ivBytesHash, ivBytesHash.Length - 1);
                if (needleFound > 0)
                {
                    success = true;
                    outBytesSameKey = new byte[needleFound];
                    Array.Copy(decryptedBytes, outBytesSameKey, needleFound);
                    return outBytesSameKey;
                }
            }

            if (!success)
            {
                string errorMsg = $"Decryption failed!\r\nKey: {this.TextBox_Key.Text} with HexHash: {this.TextBox_Key.Text} doesn't match!"; 

                this.TextBox_IV.Text = "Private Key changed!";
                this.TextBox_IV.ToolTip = "Check Enforce decrypt (without key check).";
                this.TextBox_IV.BorderColor = Color.Red;
                this.TextBox_IV.BorderWidth = 2;

                this.TextBoxDestionation.Text = errorMsg;

            }

            return decryptedBytes;
        }


        #endregion ObsoleteDeprecated

        
    }

}