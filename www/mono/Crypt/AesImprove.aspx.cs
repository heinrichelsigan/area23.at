using Area23.At;
using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Cipher;
using Area23.At.Framework.Library.EnDeCoding;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Properties;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace Area23.At.Mono.Crypt
{
    /// <summary>
    /// SAES_En_Decrypt En-/De-cryption pipeline page 
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
                Reset_TextBox_IV(Constants.AUTHOR_EMAIL);

                if (Request.Files != null && Request.Files.Count > 0)
                {
                    ; // handled by Event members
                    // Reset_TextBox_IV(Constants.AREA23_EMAIL);
                }
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
            SpanRightFile.Visible = false;
            SpanRightLabel.Visible = false;           

            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                string source = this.TextBoxSource.Text + "\r\n" + this.TextBox_IV.Text;                

                byte[] inBytesText = Encoding.UTF8.GetBytes(this.TextBoxSource.Text);
                // byte[] inBytesText = DeEnCoder.GetBytesFromString(this.TextBoxSource.Text, 256, false);
                // byte[] inBytesHash = DeEnCoder.GetBytesFromString("\r\n" + this.TextBox_IV.Text, 256, false);

                byte[] inBytes = inBytesText; // Extensions.TarBytes(inBytesText, inBytesHash);

                // Encoding.UTF8.GetBytes(this.TextBoxSource.Text);

                string[] algos = this.TextBox_Encryption.Text.Split("+;,→⇛".ToCharArray());
                byte[] encryptBytes = inBytes;
                foreach (string algo in algos)
                {
                    if (!string.IsNullOrEmpty(algo))
                    {
                        encryptBytes = EncryptBytes(inBytes, algo);
                        inBytes = encryptBytes;
                    }
                }

                bool fromPlain = string.IsNullOrEmpty(this.TextBox_Encryption.Text);
                EncodingType encodeType = (EncodingType)Enum.Parse(typeof(EncodingType), this.DropDownList_Encoding.SelectedValue);
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
            SpanRightFile.Visible = false;
            SpanRightLabel.Visible = false;

            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                string cipherText = this.TextBoxSource.Text;
                bool plainUu = string.IsNullOrEmpty(this.TextBox_Encryption.Text);
                string decryptedText = string.Empty;                
                EncodingType encodeType = (EncodingType)Enum.Parse(typeof(EncodingType), this.DropDownList_Encoding.SelectedValue);
                string encodingMethod = encodeType.ToString().ToLowerInvariant();
                
                byte[] cipherBytes = DeEnCoder.DecodeText(cipherText /*, out string errMsg */, encodeType, plainUu, false);
                if (cipherBytes == null || cipherBytes.Length == 0)
                {
                    this.TextBox_IV.Text = "0 bytes decoded, there might be an encode or crypt error!";
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
                        decryptedBytes = DecryptBytes(cipherBytes, algos[ig]);
                        cipherBytes = decryptedBytes;
                    }
                }                                

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
            string addChiffre = "";
            foreach (string cryptName in Enum.GetNames(typeof(CipherEnum)))
            {
                if (cryptName != "None")
                {
                    if (DropDownList_Cipher.SelectedValue.ToString() == cryptName)
                    {
                        addChiffre = DropDownList_Cipher.SelectedValue.ToString() + ";";
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
        /// TextBox_Key_TextChanged - fired on <see cref="TextBox_Key"/> TextChanged event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void TextBox_Key_TextChanged(object sender, EventArgs e)
        {
            this.TextBox_IV.Text = DeEnCoder.KeyToHex(this.TextBox_Key.Text);
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

        #endregion page_events

        #region enryption_decryption_members 

        /// <summary>
        /// Encrypts or Decrypts uploaded file
        /// </summary>
        /// <param name="pfile">HttpPostedFile pfile</param>
        /// <param name="crypt">true for encrypt, false for decrypt</param>
        protected void EnDeCryptUploadFile(HttpPostedFile pfile, bool crypt = true)
        {            
            // Get the name of the file that is posted.
            string strFileName = pfile.FileName;
            strFileName = Path.GetFileName(strFileName);
            string savedTransFile = string.Empty;
            string outMsg = string.Empty;            
            EncodingType encodeType = (EncodingType)Enum.Parse(typeof(EncodingType), this.DropDownList_Encoding.SelectedValue);
            EncodingType extEncType = encodeType;
            string encodingMethod = encodeType.ToString().ToLowerInvariant();
            bool plainUu = string.IsNullOrEmpty(this.TextBox_Encryption.Text);

            lblUploadResult.Text = "";

            if (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0))
            {
                // pfile.SaveAs(strFilePath);
                if (string.IsNullOrEmpty(lblUploadResult.Text))
                    lblUploadResult.Text = strFileName + " has been successfully uploaded.";
                
                byte[] fileBytes = pfile.InputStream.ToByteArray();
                byte[] outBytes = new byte[fileBytes.Length];

                if (!string.IsNullOrEmpty(strFileName))
                {
                    string[] algos = this.TextBox_Encryption.Text.Split("+;,→⇛".ToCharArray());
                    
                    int cryptCount = 0;
                    outBytes = fileBytes;
                    Array.Copy(fileBytes, 0, outBytes, 0, fileBytes.Length);

                    if (crypt)
                    {
                        byte[] inBytes = fileBytes; //.TarBytes(inBytesSeperator, inBytesKeyHash);                            

                        imgOut.Src = LibPaths.ResAppPath + "img/encrypted.png";

                        foreach (string algo in algos)
                        {
                            if (!string.IsNullOrEmpty(algo))
                            {
                                outBytes = EncryptBytes(inBytes, algo);
                                inBytes = outBytes;
                                cryptCount++;
                                strFileName += "." + algo.ToLower();
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

                            cipherBytes = DeEnCoder.DecodeText(cipherText /*, out string errMsg */, extEncType, plainUu, true);
                            strFileName = strFileName.EndsWith("." + encodingMethod) ? strFileName.Replace("." + encodingMethod, "") : strFileName;
                        }
                           

                        strFileName = strFileName.EndsWith(".hex") ? strFileName.Replace(".hex", "") : strFileName;
                        strFileName = strFileName.EndsWith(".oct") ? strFileName.Replace(".oct", "") : strFileName;
                        imgOut.Src = LibPaths.ResAppPath + "img/decrypted.png";

                        for (int ig = (algos.Length - 1); ig >= 0; ig--)
                        {
                            if (!string.IsNullOrEmpty(algos[ig]))
                            {
                                outBytes = DecryptBytes(cipherBytes, algos[ig]);
                                cipherBytes = outBytes;
                                cryptCount++;
                                strFileName = strFileName.EndsWith("." + algos[ig].ToLower()) ? strFileName.Replace("." + algos[ig].ToLower(), "") : strFileName;
                            }
                        }

                        cipherBytes = DeEnCoder.GetBytesTrimNulls(outBytes);
                        outBytes = cipherBytes;

                        savedTransFile = this.ByteArrayToFile(outBytes, out outMsg, strFileName);
                        // if (success)
                        lblUploadResult.Text = string.Format("decrypt to {0}", outMsg);
                        // else
                        // lblUploadResult.Text = "decrypting file failed, byte trash saved  to ";                            
                    }

                    aTransFormed.HRef = LibPaths.OutAppPath + savedTransFile;
                    // lblUploadResult.Text += outMsg;
                }

                // Display the result of the upload.
                SpanRightFile.Visible = true;
                SpanRightLabel.Visible = true;

                DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesBGFile.gif'); background-repeat: no-repeat; background-color: transparent;";
                DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesBGFile.gif')";
                DivAesImprove.Style["background-image"] = "url('../res/img/AesBGFile.gif')";
            }
            else
            {
                SpanRightLabel.Visible = true;
                SpanRightFile.Visible = false;
                lblUploadResult.Text = "Click 'Browse' to select the file to upload.";
                
                DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImproveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
                DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImproveBG.gif')";
                DivAesImprove.Style["background-image"] = "url('../res/img/AesImproveBG.gif')";
            }           

        }

        /// <summary>
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="algo">Symetric chiffre algorithm</param>
        /// <returns>encrypted byte Array</returns>
        /// 
        protected byte[] EncryptBytes(byte[] inBytes, string algo)
        {
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            string keyIv = (!string.IsNullOrEmpty(this.TextBox_IV.Text)) ? this.TextBox_IV.Text : Constants.AUTHOR_IV;
            CipherEnum cipherAlgo = (CipherEnum)Enum.Parse(typeof(CipherEnum), algo);

            byte[] encryptBytes = Framework.Library.Cipher.Crypt.EncryptBytes(inBytes, cipherAlgo, secretKey, keyIv);
            
            return encryptBytes;
        }

        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="algorithmName">Symetric chiffre algorithm</param>
        /// <returns>decrypted byte Array</returns>
        protected byte[] DecryptBytes(byte[] cipherBytes, string algorithmName)
        {
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            string keyIv = (!string.IsNullOrEmpty(this.TextBox_IV.Text)) ? this.TextBox_IV.Text : Constants.AUTHOR_IV;

            CipherEnum cipherEnum = (CipherEnum)Enum.Parse(typeof(CipherEnum), algorithmName);
            byte[] decryptBytes = Framework.Library.Cipher.Crypt.DecryptBytes(cipherBytes, cipherEnum, secretKey, keyIv);

            return decryptBytes;
        }

        #endregion enryption_decryption_members 


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

            this.TextBox_IV.Text = DeEnCoder.KeyToHex(this.TextBox_Key.Text);

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

            SpanRightLabel.Visible = false;
            SpanRightFile.Visible = false;

            DivAesImprove.Attributes["style"] = "padding-left: 40px; margin-left: 2px; background-image: url('../res/img/AesImproveBG.gif'); background-repeat: no-repeat; background-color: transparent;";
            DivAesImprove.Style["backgroundImage"] = "url('../res/img/AesImproveBG.gif')";
            DivAesImprove.Style["background-image"] = "url('../res/img/AesImproveBG.gif')";
        }
       
    }
}