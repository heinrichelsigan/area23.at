using Area23.At;
using Area23.At.Framework.Library;
using Area23.At.Framework.Library;
using Area23.At.Framework.Library.EnDeCoding;
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
    public partial class AesImprove : Util.UIPage
    {
        internal Framework.Library.Cipher.Symmetric.CryptBounceCastle cryptBounceCastle;


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
            frmConfirmation.Visible = false;

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
                string encodingMethod = this.DropDownList_Encoding.SelectedValue.ToLowerInvariant();
                string encryptedText = DeEnCoder.EncodeEncryptedBytes(encryptBytes, encodingMethod, fromPlain, false);
                //switch (encodingMethod)
                //{
                //    case "hex16":       encryptedText = Hex16.ToHex16(encryptBytes); break;
                //    case "base16":      encryptedText = Base16.ToBase16(encryptBytes); break;
                //    case "base32":      encryptedText = Base32.ToBase32(encryptBytes); break;
                //    case "base32hex":   encryptedText = Base32Hex.ToBase32Hex(encryptBytes); break;
                //    case "uu":          encryptedText = Uu.ToUu(encryptBytes, fromPlain); break;
                //    case "base64":
                //    default:            encryptedText = Base64.ToBase64(encryptBytes); break;
                //}                
                this.TextBoxDestionation.Text = encryptedText;

            }
            else
            {
                this.TextBox_IV.Text = "TextBox source is empty!";
                this.TextBox_IV.ForeColor = Color.BlueViolet;
                this.TextBox_IV.BorderColor = Color.Blue;

                this.TextBoxSource.BorderColor = Color.BlueViolet;
                this.TextBoxSource.BorderStyle = BorderStyle.Dotted;
                this.TextBoxSource.BorderWidth = 2;
            }
        }

        /// <summary>
        /// ButtonDecrypt_Click fired when ButtonDecrypt for text decryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonDecrypt_Click(object sender, EventArgs e)
        {
            frmConfirmation.Visible = false;
            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                string cipherText = this.TextBoxSource.Text;
                bool plainUu = string.IsNullOrEmpty(this.TextBox_Encryption.Text);
                string decryptedText = string.Empty;
                string encodingMethod = this.DropDownList_Encoding.SelectedValue.ToLowerInvariant();
                byte[] cipherBytes = DeEnCoder.EncodedTextToBytes(cipherText, out string errMsg, encodingMethod, plainUu, false);
                if (cipherBytes == null && !string.IsNullOrEmpty(errMsg))
                {
                    this.TextBox_IV.Text = errMsg;
                    this.TextBox_IV.ForeColor = Color.BlueViolet;
                    this.TextBox_IV.BorderColor = Color.Blue;
                    return;
                }
                //switch (encodingMethod)
                //{
                //    case "hex16":
                //        if (Hex16.IsValidHex16(cipherText))
                //            cipherBytes = Hex16.FromHex16(cipherText);
                //        else
                //        {
                //            this.TextBox_IV.Text = "Input Text is not valid hex16 string!";
                //            this.TextBox_IV.ForeColor = Color.BlueViolet;
                //            this.TextBox_IV.BorderColor = Color.Blue;
                //            return;
                //        }
                //        break;
                //    case "base16":
                //        if (Base16.IsValidBase16(cipherText))
                //            cipherBytes = Base16.FromBase16(cipherText);
                //        else
                //        {
                //            this.TextBox_IV.Text = "Input Text is not valid base16 string!";
                //            this.TextBox_IV.ForeColor = Color.BlueViolet;
                //            this.TextBox_IV.BorderColor = Color.Blue;
                //            return;
                //        }
                //        break;
                //    case "base32":
                //        if (Base32.IsValidBase32(cipherText))
                //            cipherBytes = Base32.FromBase32(cipherText);
                //        else
                //        {
                //            this.TextBox_IV.Text = "Input Text is not valid base32 string!";
                //            this.TextBox_IV.ForeColor = Color.BlueViolet;
                //            this.TextBox_IV.BorderColor = Color.Blue;
                //            return;
                //        }
                //        break;
                //    case "base32hex":
                //        if (Base32Hex.IsValidBase32Hex(cipherText))
                //            cipherBytes = Base32Hex.FromBase32Hex(cipherText);
                //        else
                //        {
                //            this.TextBox_IV.Text = "Input Text is not valid base32 hex string!";
                //            this.TextBox_IV.ForeColor = Color.BlueViolet;
                //            this.TextBox_IV.BorderColor = Color.Blue;
                //            return;
                //        }
                //        break;
                //    case "uu":
                //        if (Uu.IsValidUue(cipherText))                        
                //            cipherBytes = Uu.FromUu(cipherText, plainUu);                        
                //        else
                //        {
                //            this.TextBox_IV.Text = "Input Text is not valid uuencoded string!";
                //            this.TextBox_IV.ForeColor = Color.BlueViolet;
                //            this.TextBox_IV.BorderColor = Color.Blue;
                //            return;
                //        }
                //        break;
                //    case "base64":
                //    default:
                //        if (Base64.IsValidBase64(cipherText))
                //            cipherBytes = Base64.FromBase64(cipherText);
                //        else
                //        {
                //            this.TextBox_IV.Text = "Input Text is not valid base64 string!";
                //            this.TextBox_IV.ForeColor = Color.BlueViolet;
                //            this.TextBox_IV.BorderColor = Color.Blue;
                //            return;
                //        }
                //        break;
                //}

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
            }
            else
            {
                this.TextBox_IV.Text = "TextBox source is empty!";
                this.TextBox_IV.ForeColor = Color.BlueViolet;
                this.TextBox_IV.BorderColor = Color.Blue;

                this.TextBoxSource.BorderColor = Color.BlueViolet;
                this.TextBoxSource.BorderStyle = BorderStyle.Dotted;
                this.TextBoxSource.BorderWidth = 2;
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
        }

        /// <summary>
        /// Add encryption alog to encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageButton_Add_Click(object sender, EventArgs e)
        {
            string addChiffre = "";
            if (DropDownList_SymChiffer.SelectedValue.ToString() == "3DES" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "2FISH" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "3FISH" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "AES" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Camellia" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Cast5" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Cast6" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "DesEde" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Gost28147" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Idea" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Noekeon" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC2" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC532" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC564" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC6" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Rsa" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Rijndael" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Seed" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Serpent" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Skipjack" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Tea" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Tnepres" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "XTea" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "ZenMatrix")
            {
                addChiffre = DropDownList_SymChiffer.SelectedValue.ToString() + ";"; 
                this.TextBox_Encryption.Text += addChiffre;
                this.TextBox_Encryption.BorderStyle = BorderStyle.Double;
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
            string encodingMethod = this.DropDownList_Encoding.SelectedValue.ToLowerInvariant();
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
                    string baseEncoding = this.DropDownList_Encoding.SelectedValue.ToLowerInvariant();

                    //if (algos.Length <= 1 || String.IsNullOrEmpty(algos[0]))
                    //{
                    //    imgOut.Src = LibPaths.ResAppPath + "img/file.png";
                    //    lblUploadResult.Text = "file keept unmodified and uploaded to ";
                    //}
                    //else
                    //{
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


                        savedTransFile = this.ByteArrayToFile(outBytes, out outMsg, strFileName);
                        if (!string.IsNullOrEmpty(savedTransFile) && !string.IsNullOrEmpty(outMsg))
                        {
                            lblUploadResult.Text = string.Format("file {0}x encrypted to {1}", cryptCount, outMsg);

                            string encryptedText = DeEnCoder.EncodeEncryptedBytes(outBytes, encodingMethod, plainUu, true);
                            strFileName += "." + encodingMethod;
                            string savedTransFileEnc = this.StringToFile(encryptedText, out outMsg, strFileName);

                            lblUploadResult.Text += string.Format(" and to encoded {0}.", savedTransFileEnc);
                        }
                        else
                            lblUploadResult.Text = "file failed to encrypt and save!";
                    }
                    else
                    {

                        string decryptedText = string.Empty;
                        byte[] cipherBytes = fileBytes;
                        Array.Copy(fileBytes, 0, cipherBytes, 0, fileBytes.Length);

                        string ext = strFileName.GetExtensionFromFileString();
                        switch (ext.ToLowerInvariant())
                        {
                            case "null":
                            case ".null":
                            case "hex16":
                            case ".hex16":
                            case "base16":
                            case ".base16":
                            case "base32":
                            case ".base32":
                            case "base32hex":
                            case ".base32hex":
                            case "uu":
                            case ".uu":
                            case "uue":
                            case ".uue":
                            case "base64":
                            case ".base64":
                            case "mime":
                            case ".mime":
                                encodingMethod = (ext.StartsWith(".")) ? ext.ToLowerInvariant().Substring(1) : ext.ToLowerInvariant();
                                string cipherText = EnCoderHelper.GetString8(fileBytes);
                                string tmpFile = ByteArrayToFile(fileBytes, out outMsg, strFileName + ".tmp");
                                // tmpFile = tmpFile.Replace(".hex", ".tmp");
                                if (System.IO.File.Exists(LibPaths.OutDirPath + tmpFile))
                                {
                                    cipherText = System.IO.File.ReadAllText(LibPaths.OutDirPath + tmpFile, Encoding.UTF8);
                                }

                                cipherBytes = DeEnCoder.EncodedTextToBytes(cipherText, out string errMsg, encodingMethod, plainUu, true);
                                strFileName = strFileName.EndsWith("." + encodingMethod) ? strFileName.Replace("." + encodingMethod, "") : strFileName;
                                break;
                            default: // assert here
                                break;
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
                        lblUploadResult.Text = string.Format("file has been decrypted to {0}!", outMsg);
                        // else
                        // lblUploadResult.Text = "decrypting file failed, byte trash saved  to ";                            
                    }

                    aTransFormed.HRef = LibPaths.OutAppPath + savedTransFile;
                    lblUploadResult.Text += outMsg;
                }
            }
            else
            {
                lblUploadResult.Text = "Click 'Browse' to select the file to upload.";
            }

            // Display the result of the upload.
            frmConfirmation.Visible = true;
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

            byte[] encryptBytes = Framework.Library.Cipher.Crypt.EncryptBytes(inBytes, algo, secretKey, keyIv);
            
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

            byte[] decryptBytes = Framework.Library.Cipher.Crypt.DecryptBytes(cipherBytes, algorithmName, secretKey, keyIv);

            return decryptBytes;
        }

        #endregion enryption_decryption_members 

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
        }
        

        /// <summary>
        /// Handles string decryption, compares if private key & hex hash match in decrypted text
        /// </summary>
        /// <param name="decryptedText">decrypted plain text</param>
        /// <returns>decrypted plain text without check hash or an error message, in case that check hash doesn't match.</returns>
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
        protected byte[] HandleBytes_PrivateKey_Changed(byte[] decryptedBytes, out bool success)
        {
            success = false;
            byte[] outBytesSameKey = null;
            byte[] ivBytesHash = EnCoderHelper.GetBytes8("\r\n" + this.TextBox_IV.Text);
            // Framework.Library.Cipher.Symmetric.CryptHelper.GetBytesFromString("\r\n" + this.TextBox_IV.Text, 256, false);
            if (decryptedBytes != null && decryptedBytes.Length > ivBytesHash.Length)
            {
                int needleFound = Framework.Library.Extensions.BytesBytes(decryptedBytes, ivBytesHash, ivBytesHash.Length - 1);
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

    }
}