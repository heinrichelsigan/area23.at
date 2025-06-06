﻿using Area23.At;
using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Zfx;
using Area23.At.Mono.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Area23.At.Mono.Crypt
{

    /// <summary>
    /// AesImprove former SAES_En_Decrypt En-/De-cryption pipeline page 
    /// Feature to encrypt and decrypt simple plain text or files
    /// </summary>
    public partial class AesImprove : Util.UIPage
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
                    (((string)Session[Constants.AES_ENVIROMENT_KEY]).Length > 1))
                {
                    Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);
                }
                // else
                //     Reset_TextBox_IV(Constants.AUTHOR_EMAIL);
            }

            if ((Request.Files != null && Request.Files.Count > 0) || (!String.IsNullOrEmpty(oFile.Value)))
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
        protected void TextBox_Key_TextChanged(object sender, EventArgs e)
        {
            Button_Hash_Click(sender, e);

            DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImproveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
            DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImproveBG.gif')";
            DivAesImprove.Style["background-image"] = "url('../res/img/AesImproveBG.gif')";
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
                // Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);
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
            this.TextBoxSource.Text = "";
            this.TextBoxDestionation.Text = "";
            ClearPostedFileSession(false);

            DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImprotveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
            DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImprotveBG.gif')";
            DivAesImprove.Style["background-image"] = "url('../res/img/AesImprotveBG.gif')";
        }

        /// <summary>
        /// Button_Hash_Click sets hash from key and fills pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Hash_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 1)
            {
                Session[Constants.AES_ENVIROMENT_KEY] = this.TextBox_Key.Text;
                Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);
            }
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
        /// Button_SetPipeline_Click resets <see cref="TextBox_Key"/> and <see cref="TextBox_IV"/> to default loaded values
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_SetPipeline_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 1)
            {
                Session[Constants.AES_ENVIROMENT_KEY] = this.TextBox_Key.Text;
                Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);
                
                SymmCipherEnum[] cses = new Framework.Library.Crypt.Cipher.Symmetric.SymmCipherPipe(this.TextBox_Key.Text, this.TextBox_IV.Text).InPipe;
                this.TextBox_Encryption.Text = string.Empty;
                foreach (SymmCipherEnum c in cses)
                {
                    this.TextBox_Encryption.Text += c.ToCipherEnum().ToString() + ";";
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
            if (SpanLeftFile.Visible && aUploaded.HRef.Contains(Constants.OUT_DIR) && !string.IsNullOrEmpty(imgIn.Alt))
            {
                string filePath = LibPaths.SystemDirOutPath + imgIn.Alt;
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
                byte[] encryptBytes = new byte[inBytes.Length];
                Array.Copy(inBytes, 0, encryptBytes, 0, inBytes.Length);


                ZipType ztype = ZipType.None;
                if (Enum.TryParse<ZipType>(DropDownList_Zip.SelectedValue, out ztype))
                {
                    //switch (ztype)
                    //{
                    //    case ZipType.GZip: encryptBytes = GZ.GZip(inBytes); break;
                    //    case ZipType.BZip2: encryptBytes = BZip2.BZip(inBytes); break;
                    //    case ZipType.Zip: encryptBytes = WinZip.Zip(inBytes); break;
                    //    case ZipType.None:
                    //    default: break;
                    //}

                    string outp = string.Empty;
                    string zcmd = (Constants.UNIX) ? "/usr/local/bin/zipunzip.sh" : (Constants.WIN32) ? 
                        Path.Combine(LibPaths.SystemDirBinPath, "zipunzip.bat") : "";
                    string zfile = DateTime.UtcNow.Area23DateTimeWithMillis();
                    string zPath = encryptBytes.ToFile(LibPaths.SystemDirTmpPath, zfile, ".txt");
                    string zOutPath = zPath;
                    string zopt = "";

                    switch (ztype)
                    {
                        case ZipType.GZip: zOutPath = zPath + ".gz"; zopt = "gz"; break;
                        case ZipType.BZip2: zOutPath = zPath + ".bz2"; zopt = "bz"; break;
                        case ZipType.Z7: zOutPath = zPath + ".7z"; zopt = "7z"; break;
                        case ZipType.Zip: zOutPath = zPath + ".zip"; zopt = "zip"; break;
                        case ZipType.None:
                        default: zOutPath = ""; zPath = ""; zopt = ""; break;
                    }

                    if (!string.IsNullOrEmpty(zopt) && System.IO.File.Exists(zcmd) &&
                        System.IO.File.Exists(zPath))
                    {
                        outp = ProcessCmd.Execute(zcmd, zopt + " " + zPath + " " + zOutPath, false);
                        Thread.Sleep(50);
                        if (System.IO.File.Exists(zOutPath))
                        {
                            encryptBytes = System.IO.File.ReadAllBytes(zOutPath);
                            inBytes = encryptBytes;
                        }
                    }
                }

                string[] algorithms = this.TextBox_Encryption.Text.Split(Constants.COOL_CRYPT_SPLIT.ToCharArray());                
                CipherPipe pipe = new Framework.Library.Crypt.Cipher.CipherPipe(algorithms);
                encryptBytes = pipe.MerryGoRoundEncrpyt(inBytes, this.TextBox_Key.Text, this.TextBox_IV.Text);

                //string[] algos = this.TextBox_Encryption.Text.Split(Constants.COOL_CRYPT_SPLIT.ToCharArray());
                //foreach (string algo in algos)
                //{
                //    if (!string.IsNullOrEmpty(algo))
                //    {
                //        CipherEnum cipherAlgo = CipherEnum.Aes;
                //        if (Enum.TryParse<CipherEnum>(algo, out cipherAlgo))
                //        {
                //            inBytes = Framework.Library.Crypt.Cipher.Crypt.EncryptBytes(encryptBytes, cipherAlgo, secretKey, keyIv);
                //            encryptBytes = inBytes;
                //        }
                //    }
                //}
                inBytes = encryptBytes;                

                bool fromPlain = string.IsNullOrEmpty(this.TextBox_Encryption.Text);

                encodeType = (EncodingType)Enum.Parse(typeof(EncodingType), this.DropDownList_Encoding.SelectedValue);
                string encryptedText = EnDeCodeHelper.EncodeBytes(encryptBytes, encodeType, fromPlain, false);

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
                byte[] cipherBytes;
                string encodingMethod = encodeType.ToString().ToLowerInvariant();
                try
                {
                    cipherBytes = EnDeCodeHelper.DecodeText(cipherText /*, out string errMsg */, encodeType, plainUu, false);
                }
                catch (Exception exCode)
                {
                    Area23Log.LogStatic(exCode);
                    cipherBytes = new byte[0];
                    this.TextBox_IV.Text = "0 bytes decoded, there might be an encode or crypt error!";
                }
                if (cipherBytes == null || cipherBytes.Length < 1)
                {
                    this.TextBox_IV.ForeColor = Color.BlueViolet;
                    this.TextBox_IV.BorderColor = Color.Blue;
                    return;
                }

                byte[] decryptedBytes = cipherBytes;
                int ig = 0;

                byte[] kb = Framework.Library.Crypt.Cipher.CryptHelper.GetUserKeyBytes(this.TextBox_Key.Text, this.TextBox_IV.Text, 16);
                string[] algorithms = this.TextBox_Encryption.Text.Split(Constants.COOL_CRYPT_SPLIT.ToCharArray());
                CipherPipe pipe = new Framework.Library.Crypt.Cipher.CipherPipe(algorithms);
                decryptedBytes = pipe.DecrpytRoundGoMerry(cipherBytes, this.TextBox_Key.Text, this.TextBox_IV.Text);

                //string[] algos = this.TextBox_Encryption.Text.Split(Constants.COOL_CRYPT_SPLIT.ToCharArray());
                //for (ig = (algos.Length - 1); ig >= 0; ig--)
                //{
                //    if (!string.IsNullOrEmpty(algos[ig]))
                //    {
                //        CipherEnum cipherAlgo = CipherEnum.Aes;
                //        if (Enum.TryParse<CipherEnum>(algos[ig], out cipherAlgo))
                //        {
                //            decryptedBytes = Framework.Library.Crypt.Cipher.Crypt.DecryptBytes(cipherBytes, cipherAlgo, secretKey, keyIv);
                //            cipherBytes = decryptedBytes;
                //        }
                //    }
                //}
                cipherBytes = decryptedBytes;          

                ZipType ztype = ZipType.None;
                string zcmd = (Constants.UNIX) ? "/usr/local/bin/zipunzip.sh" : (Constants.WIN32) ?
                    Path.Combine(LibPaths.SystemDirBinPath, "zipunzip.bat") : "";
                if (Enum.TryParse<ZipType>(DropDownList_Zip.SelectedValue, out ztype))
                {
                    //switch (ztype)
                    //{
                    //    case ZipType.GZip: decryptedBytes = GZ.GUnZip(cipherBytes); break;
                    //    case ZipType.BZip2: decryptedBytes = BZip2.BUnZip(cipherBytes); break;
                    //    case ZipType.Zip: decryptedBytes = WinZip.UnZip(cipherBytes); break;
                    //    case ZipType.None:
                    //    default: decryptedBytes = cipherBytes; break;
                    //}

                    string outp = string.Empty;
                    string zfile = DateTime.UtcNow.Area23DateTimeWithMillis();
                    string zPath = cipherBytes.ToFile(LibPaths.SystemDirTmpPath, zfile, ".txt.z");
                    string zOutPath = cipherBytes.ToFile(LibPaths.SystemDirTmpPath, zfile, ".txt");
                    string zopt = "";

                    switch (ztype)
                    {
                        case ZipType.GZip:
                            zPath = cipherBytes.ToFile(LibPaths.SystemDirTmpPath, zfile, ".txt.gz");
                            zOutPath = zPath.Replace(".txt.gz", ".txt");
                            zopt = "gunzip";
                            break;
                        case ZipType.BZip2:
                            zPath = cipherBytes.ToFile(LibPaths.SystemDirTmpPath, zfile, ".txt.bz2");
                            zOutPath = zPath.Replace(".txt.bz2", ".txt").Replace(".bz2", "").Replace(".bz", "");
                            zopt = "bunzip";
                            break;
                        case ZipType.Z7:
                            zPath = cipherBytes.ToFile(LibPaths.SystemDirTmpPath, zfile, ".txt.7z");
                            zOutPath = zPath.Replace(".txt.7z", ".txt").Replace(".7z", "");
                            zopt = "7unzip";
                            break;
                        case ZipType.Zip:
                            zPath = cipherBytes.ToFile(LibPaths.SystemDirTmpPath, zfile, ".txt.zip");
                            zOutPath = zPath.Replace(".txt.zip", ".txt").Replace(".zip", "");
                            zopt = "unzip";
                            break;
                        case ZipType.None:
                        default: zopt = ""; zOutPath = ""; decryptedBytes = cipherBytes; break;
                    }

                    if (!string.IsNullOrEmpty(zopt) && !string.IsNullOrEmpty(zPath) && !string.IsNullOrEmpty(zOutPath))
                    {
                        if (System.IO.File.Exists(zPath) && System.IO.File.Exists(zcmd))
                        {
                            outp = ProcessCmd.Execute(zcmd, zopt + " " + zPath + " " + zOutPath, false);
                            Thread.Sleep(50);
                            if (System.IO.File.Exists(zOutPath))
                            {
                                decryptedBytes = System.IO.File.ReadAllBytes(zOutPath);
                                cipherBytes = decryptedBytes;
                            }
                        }
                    }
                }

                decryptedText = EnDeCodeHelper.GetStringFromBytesTrimNulls(decryptedBytes);
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
                strFileName = System.IO.Path.GetFileName(strFileName);
                string strFilePath = LibPaths.SystemDirOutPath + strFileName;
                pfile.SaveAs(strFilePath);

                if (System.IO.File.Exists(strFilePath))
                {
                    uploadResult.Text = strFileName + " has been successfully uploaded.";
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
        protected void EnDeCryptUploadFile(HttpPostedFile pfile, bool crypt = true, string fileSavedName = "")
        {
            // Get the name of the file that is posted.
            string strFileName = (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0)) ?
                pfile.FileName : fileSavedName;
            strFileName = System.IO.Path.GetFileName(strFileName);

            long inFileLen = -1;
            if (!string.IsNullOrEmpty(fileSavedName) && System.IO.File.Exists(fileSavedName))
            {
                FileInfo fIn = new FileInfo(fileSavedName);
                inFileLen = fIn.Length;
            }
            
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
                byte[] inBytes = (!string.IsNullOrEmpty(fileSavedName) && System.IO.File.Exists(fileSavedName)) ?
                     System.IO.File.ReadAllBytes(fileSavedName) : (
                        (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0)) ?
                            pfile.InputStream.ToByteArray() : new byte[65536]);

                // write source file hash
                this.TextBoxSource.Text =
                    "File: " + strFileName + "\n" +
                    "FileLen: " + inFileLen + "\n" +
                    "StreamLength: " + inBytes.Length + "\n" +
                    "MD5Sum " + MD5Sum.Hash(inBytes) + "\n" +
                    "Sha256 " + Sha256Sum.Hash(inBytes) + "\n";
                uploadResult.Text = "";

                byte[] outBytes = new byte[inBytes.Length];

                if (!string.IsNullOrEmpty(strFileName))
                {
                    string[] algos = this.TextBox_Encryption.Text.Split("+;,→⇛".ToCharArray());
                    string baseEncoding = this.DropDownList_Encoding.SelectedValue.ToLowerInvariant();

                    int cryptCount = 0;
                    outBytes = inBytes;
                    Array.Copy(inBytes, 0, outBytes, 0, inBytes.Length);

                    if (crypt)
                    {
                        // byte[] inBytes = fileBytes; //.TarBytes(inBytesSeperator, inBytesKeyHash);                            

                        imgOut.Src = LibPaths.ResAppPath + "img/encrypted.png";

                        ZipType ztype = ZipType.None;
                        string zcmd = (Constants.UNIX) ? "/usr/local/bin/zipunzip.sh" : (Constants.WIN32) ? 
                            Path.Combine(LibPaths.AdditionalBinDir, "zipunzip.bat") : "";
                        string zopt = " ";
                        if (Enum.TryParse<ZipType>(DropDownList_Zip.SelectedValue, out ztype))
                        {
                            //switch (ztype)
                            //{
                            //    case ZipType.GZip: inBytes = GZ.GZip(outBytes); break;
                            //    case ZipType.BZip2: inBytes = BZip2.BZip(outBytes); break;
                            //    case ZipType.Zip: inBytes = WinZip.Zip(outBytes); break;
                            //    case ZipType.Z7:
                            //    case ZipType.None:
                            //    default: break;
                            //}
                            string outp = string.Empty;
                            string zfile = DateTime.UtcNow.Area23DateTimeWithMillis();
                            string zPath = inBytes.ToFile(LibPaths.SystemDirTmpPath, zfile, ".txt");
                            string zOutPath = zPath;
                            switch (ztype)
                            {
                                case ZipType.GZip: zOutPath += ".gz"; zopt = "gz"; break;
                                case ZipType.BZip2: zOutPath += ".bz2"; zopt = "bz2"; break;
                                case ZipType.Z7: zOutPath += ".7z"; zopt = "7z"; break;
                                case ZipType.Zip: zOutPath += ".zip"; zopt = "zip"; break;
                                case ZipType.None:
                                default: zopt = ""; break;
                            }

                            if (!string.IsNullOrEmpty(zopt) && System.IO.File.Exists(LibPaths.SystemDirBinPath + zcmd))
                            {
                                outp = ProcessCmd.Execute(zcmd, " " + zopt + " " + zPath + " " + zOutPath, false);
                                Thread.Sleep(64);
                                if (System.IO.File.Exists(zOutPath))
                                    inBytes = System.IO.File.ReadAllBytes(zOutPath);

                                strFileName += "." + zopt;
                            }
                        }

                        byte[] kb = Framework.Library.Crypt.Cipher.CryptHelper.GetUserKeyBytes(this.TextBox_Key.Text, this.TextBox_IV.Text, 16);
                        string[] algorithms = this.TextBox_Encryption.Text.Split(Constants.COOL_CRYPT_SPLIT.ToCharArray());
                        CipherPipe pipe = new Framework.Library.Crypt.Cipher.CipherPipe(algorithms);
                        outBytes = pipe.MerryGoRoundEncrpyt(inBytes, this.TextBox_Key.Text, this.TextBox_IV.Text);
                        inBytes = outBytes;
                        cryptCount += 8;

                        strFileName += "." + pipe.PipeString;

                        //foreach (string algo in algos)
                        //{
                        //    if (!string.IsNullOrEmpty(algo))
                        //    {
                        //        CipherEnum cipherAlgo = CipherEnum.Aes;
                        //        if (Enum.TryParse<CipherEnum>(algo, out cipherAlgo))
                        //        {
                        //            outBytes = Framework.Library.Crypt.Cipher.Crypt.EncryptBytes(inBytes, cipherAlgo, secretKey, keyIv);
                        //            inBytes = outBytes;
                        //            cryptCount++;
                        //            strFileName += "." + algo.ToLower();
                        //        }
                        //    }
                        //}

                        long fileLen = -1;
                        if (CheckBoxEncode.Checked)
                        {
                            strFileName += "." + encodeType.ToString().ToLowerInvariant();
                            string outString = EnDeCodeHelper.EncodeBytes(outBytes, encodeType, plainUu, true);
                            savedTransFile = this.StringToFile(outString, out outMsg, strFileName, LibPaths.SystemDirOutPath);
                        }
                        else
                        {
                            savedTransFile = this.ByteArrayToFile(outBytes, out outMsg, strFileName, LibPaths.SystemDirOutPath);
                        }

                        try
                        {
                            FileInfo fi = new FileInfo(LibPaths.SystemDirOutPath + savedTransFile);
                            fileLen = fi.Length;
                        }
                        catch (Exception exFiLen)
                        {
                            Area23Log.LogStatic(exFiLen);
                        }

                        // // write destination file hash
                        this.TextBoxDestionation.Text =
                            "File: " + savedTransFile + "\n" +
                            "FileLen: " + fileLen + "\n" +
                            "StreamLength: " + outBytes.Length + "\n" +
                            "MD5Sum " + MD5Sum.Hash(LibPaths.SystemDirOutPath + savedTransFile, false) + "\n" +
                            "Sha256 " + Sha256Sum.Hash(LibPaths.SystemDirOutPath + savedTransFile, false) + "\n";

                        if (!string.IsNullOrEmpty(savedTransFile) && !string.IsNullOrEmpty(outMsg))
                        {
                            uploadResult.Text = string.Format("{0}x crypt {1}", cryptCount, outMsg);
                            if (cryptCount > 4)
                                uploadResult.Text =
                                    string.Format("{0}x crypt {1}", cryptCount, outMsg.Substring(outMsg.IndexOf(".")));
                        }
                        else
                            uploadResult.Text = "file failed to encrypt and save!";
                    }
                    else
                    {

                        string decryptedText = string.Empty;
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
                            string cipherText = EnDeCodeHelper.GetString(inBytes);
                            string tmpFile = ByteArrayToFile(inBytes, out outMsg, strFileName + ".tmp", LibPaths.SystemDirTmpPath);
                            // tmpFile = tmpFile.Replace(".hex", ".tmp");
                            if (System.IO.File.Exists(LibPaths.SystemDirTmpPath + tmpFile))
                            {
                                cipherText = System.IO.File.ReadAllText(LibPaths.SystemDirTmpPath + tmpFile, Encoding.UTF8);
                            }

                            outBytes = EnDeCodeHelper.DecodeText(cipherText /*, out string errMsg */, extEncType, plainUu, true);
                            strFileName = strFileName.EndsWith("." + encodingMethod) ? strFileName.Replace("." + encodingMethod, "") : strFileName;
                        }
                        else // if not decode, copy inBytes => outBytes
                        {
                            Array.Copy(inBytes, 0, outBytes, 0, inBytes.Length);
                        }

                        strFileName = strFileName.EndsWith(".hex") ? strFileName.Replace(".hex", "") : strFileName;
                        strFileName = strFileName.EndsWith(".oct") ? strFileName.Replace(".oct", "") : strFileName;
                        imgOut.Src = LibPaths.ResAppPath + "img/decrypted.png";

                        byte[] kb = Framework.Library.Crypt.Cipher.CryptHelper.GetUserKeyBytes(this.TextBox_Key.Text, this.TextBox_IV.Text, 16);
                        string[] algorithms = this.TextBox_Encryption.Text.Split(Constants.COOL_CRYPT_SPLIT.ToCharArray());
                        CipherPipe pipe = new Framework.Library.Crypt.Cipher.CipherPipe(algorithms);
                        inBytes = pipe.DecrpytRoundGoMerry(outBytes, this.TextBox_Key.Text, this.TextBox_IV.Text);
                        outBytes = inBytes;
                        cryptCount += 8;

                        strFileName = (strFileName.EndsWith("." + pipe.PipeString) && strFileName.Contains("." + pipe.PipeString)) ?
                            strFileName.Replace("." + pipe.PipeString, "") : strFileName;
                        //for (int ig = (algos.Length - 1); ig >= 0; ig--)
                        //{
                        //    if (!string.IsNullOrEmpty(algos[ig]))
                        //    {
                        //        CipherEnum cipherAlgo = CipherEnum.Aes;
                        //        if (Enum.TryParse<CipherEnum>(algos[ig], out cipherAlgo))
                        //        {
                        //            inBytes = Framework.Library.Crypt.Cipher.Crypt.DecryptBytes(outBytes, cipherAlgo, secretKey, keyIv);
                        //            outBytes = inBytes;
                        //            cryptCount++;
                        //            strFileName = strFileName.EndsWith("." + algos[ig].ToLower()) ? strFileName.Replace("." + algos[ig].ToLower(), "") : strFileName;
                        //        }
                        //    }
                        //}

                        outBytes = EnDeCodeHelper.GetBytesTrimNulls(inBytes);

                        ZipType ztype = ZipType.None;
                        string zcmd = (Constants.UNIX) ? "/usr/local/bin/zipunzip.sh" : (Constants.WIN32) ?
                             Path.Combine(LibPaths.SystemDirBinPath, "zipunzip.bat") : "";
                        if (Enum.TryParse<ZipType>(DropDownList_Zip.SelectedValue, out ztype))
                        {
                            //switch (ztype)
                            //{
                            //    case ZipType.GZip:
                            //        outBytes = GZ.GUnZipViaStream(inBytes);
                            //        break;
                            //    case ZipType.BZip2:
                            //        outBytes = BZip2.BUnZip(inBytes);
                            //        break;
                            //    case ZipType.Zip:
                            //        outBytes = WinZip.UnZip(inBytes);
                            //        break;
                            //    case ZipType.Z7:
                            //    case ZipType.None:
                            //    default:
                            //        outMsg = string.Empty;
                            //        break;
                            //}

                            string zopt = "";
                            string outp = string.Empty;
                            string zfile = DateTime.UtcNow.Area23DateTimeWithMillis();
                            string zPath = outBytes.ToFile(LibPaths.SystemDirOutPath, zfile, ".txt.z");
                            string zOutPath = LibPaths.SystemDirOutPath + strFileName;

                            switch (ztype)
                            {
                                case ZipType.GZip:
                                    zPath = outBytes.ToFile(LibPaths.SystemDirOutPath, zfile, ".txt.gz");
                                    zOutPath = zOutPath.Replace(".txt.gz", ".txt").Replace(".gz", "");
                                    zopt = "gunzip";
                                    break;
                                case ZipType.BZip2:
                                    zPath = outBytes.ToFile(LibPaths.SystemDirOutPath, zfile, ".txt.bz2");
                                    zOutPath = zOutPath.Replace(".txt.bz2", ".txt").Replace(".bz2", "").Replace(".bz", "");
                                    zopt = "bunzip";
                                    break;
                                case ZipType.Z7:
                                    zPath = outBytes.ToFile(LibPaths.SystemDirOutPath, zfile, ".txt.7z");
                                    zOutPath = zOutPath.Replace(".txt.7z", ".txt").Replace(".7z", "").Replace(".z7", "");
                                    zopt = "7unzip";
                                    break;
                                case ZipType.Zip:
                                    zPath = outBytes.ToFile(LibPaths.SystemDirOutPath, zfile, ".txt.zip");
                                    zOutPath = zOutPath.Replace(".txt.zip", ".txt").Replace(".zip", "");
                                    zopt = "unzip";
                                    break;
                                case ZipType.None:
                                default:
                                    zopt = ""; zOutPath = string.Empty; outMsg = string.Empty;
                                    break;
                            }

                            if (!string.IsNullOrEmpty(zopt) && !string.IsNullOrEmpty(zPath) && !string.IsNullOrEmpty(zOutPath))
                            {
                                if (System.IO.File.Exists(zPath) && System.IO.File.Exists(zcmd))
                                {
                                    outp = ProcessCmd.Execute(zcmd, zopt + " " + zPath + " " + zOutPath, false);
                                    Thread.Sleep(64);
                                    if (System.IO.File.Exists(zOutPath))
                                        outMsg = Path.GetFileName(zOutPath);
                                        
                                    try
                                    {
                                        System.IO.File.Delete(zPath);
                                    }
                                    catch (Exception exDel)
                                    {
                                        Area23Log.LogStatic(exDel);
                                    }
                                }
                            }
                        }

                        if (string.IsNullOrEmpty(outMsg))
                        {
                            savedTransFile = this.ByteArrayToFile(outBytes, out outMsg, strFileName, LibPaths.SystemDirOutPath);
                        }
                        else
                            savedTransFile = outMsg;

                        long fileLen = -1;
                        try
                        {
                            FileInfo fi = new FileInfo(LibPaths.SystemDirOutPath + savedTransFile);
                            fileLen = fi.Length;
                        }
                        catch (Exception exFiLen)
                        {
                            Area23Log.LogStatic(exFiLen);
                        }

                        // write destination file hash
                        this.TextBoxDestionation.Text =
                            "File: " + savedTransFile + "\n" +
                            "FileLen: " + fileLen + "\n" +
                            "StreamLength: " + outBytes.Length + "\n" +
                            "MD5Sum " + MD5Sum.Hash(LibPaths.SystemDirOutPath + savedTransFile, false) + "\n" +
                            "Sha256 " + Sha256Sum.Hash(LibPaths.SystemDirOutPath + savedTransFile, false) + "\n";
                        // if (success)
                        uploadResult.Text = string.Format("decrypt to {0}", outMsg);
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
                uploadResult.Text = "Click 'Browse' to select the file to upload.";
                ClearPostedFileSession(false);
                SpanLabel.Visible = true;

                DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImproveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
                DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImproveBG.gif')";
                DivAesImprove.Style["background-image"] = "url('../res/img/AesImproveBG.gif')";
            }

        }

        #endregion file_handling_members 

        #region helper methods

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

            this.TextBox_IV.Text = EnDeCodeHelper.KeyToHex(this.TextBox_Key.Text);

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
            imgIn.Alt = "";
            aUploaded.HRef = "#";

            SpanRightFile.Visible = spansVisible;
            SpanLeftFile.Visible = spansVisible;
            SpanLabel.Visible = spansVisible;
        }

        #endregion helper methods

    }

}