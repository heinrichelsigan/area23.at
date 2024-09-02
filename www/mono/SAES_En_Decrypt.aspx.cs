﻿using Area23.At;
using Area23.At.Framework.Library;
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
using System.Web;
using System.Web.Caching;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace Area23.At.Mono
{
    /// <summary>
    /// SAES_En_Decrypt En-/De-cryption pipeline page 
    /// Feature to encrypt and decrypt simple plain text or files
    /// </summary>
    public partial class SAES_En_Decrypt : Util.UIPage
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
                string encryptedText = string.Empty;
                byte[] inBytesText = Framework.Library.Cipher.Symmetric.CryptHelper.GetBytesFromString(this.TextBoxSource.Text, 256, false);
                byte[] inBytesHash = Framework.Library.Cipher.Symmetric.CryptHelper.GetBytesFromString("\r\n" + this.TextBox_IV.Text, 256, false);
                // byte[] inBytes = Framework.Library.Cipher.Symmetric.CryptHelper.GetBytesFromString(source, 256, true);

                List<byte> enc = new List<byte>(inBytesText);
                enc.AddRange(inBytesHash);
                byte[] inBytes = enc.ToArray();

                // System.Text.Encoding.UTF8.GetBytes(this.TextBoxSource.Text);

                string[] algos = this.TextBox_Encryption.Text.Split("⇛;,".ToCharArray());
                byte[] encryptBytes = inBytes;
                foreach (string algo in algos)
                {
                    string cryptAlgorithm = (string.IsNullOrEmpty(algo)) ? "YenMatrix" : algo;
                    encryptBytes = EncryptBytes(inBytes, cryptAlgorithm);
                    inBytes = encryptBytes;

                }

                encryptedText = Convert.ToBase64String(encryptBytes);
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
                string decryptedText = string.Empty;
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] decryptedBytes = cipherBytes;
                int ig = 0;

                string[] algos = this.TextBox_Encryption.Text.Split("⇛;,".ToCharArray());
                for (ig = (algos.Length - 1); ig >= 0; ig--)
                {
                    string decryptAlgorithm = string.IsNullOrEmpty(algos[ig]) ? "YenMatrix" : algos[ig];
                    decryptedBytes = DecryptBytes(cipherBytes, decryptAlgorithm);
                    cipherBytes = decryptedBytes;
                }

                decryptedText = Framework.Library.Cipher.Symmetric.CryptHelper.GetStringFromBytesTrimNulls(decryptedBytes);
                this.TextBoxDestionation.Text = HandleString_PrivateKey_Changed(decryptedText);
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
                DropDownList_SymChiffer.SelectedValue.ToString() == "Rijndael" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Seed" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Serpent" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Skipjack" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Tea" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Tnepres" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "XTea" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "YenMatrix" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "ZenMatrix")
            {
                addChiffre = DropDownList_SymChiffer.SelectedValue.ToString() + "⇛";
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
            this.TextBox_IV.Text = Framework.Library.Cipher.Symmetric.CryptHelper.KeyHexString(this.TextBox_Key.Text);
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
            // string strFilePath;
            lblUploadResult.Text = "";

            if (pfile != null && (pfile.ContentLength > 0 || pfile.FileName.Length > 0))
            {
                //// Save the uploaded file to the server.
                // strFilePath = LibPaths.OutDirPath + strFileName;
                //while (System.IO.File.Exists(strFilePath))
                //{
                //    string newFileName = strFilePath.Contains(Constants.DateFile) ?
                //        Constants.DateFile + Guid.NewGuid().ToString() + "_" + strFileName :
                //        Constants.DateFile + strFileName;
                //    strFilePath = Paths.OutDirPath + newFileName;
                //    lblUploadResult.Text = String.Format("{0} already exists on server, saving it to {1}.",
                //        strFileName, newFileName);
                //}

                // pfile.SaveAs(strFilePath);
                if (string.IsNullOrEmpty(lblUploadResult.Text))
                    lblUploadResult.Text = strFileName + " has been successfully uploaded.";

                byte[] fileBytes = pfile.InputStream.ToByteArray();
                byte[] outBytes = null;

                if (!string.IsNullOrEmpty(strFileName))
                {
                    string[] algos = this.TextBox_Encryption.Text.Split("⇛;,".ToCharArray());
                    if (algos.Length <= 1 || String.IsNullOrEmpty(algos[0]))
                    {
                        imgOut.Src = "res/img/file.png";
                        lblUploadResult.Text = "file keept unmodified and uploaded to ";
                    }
                    else
                    {
                        int cryptCount = 0;
                        if (crypt)
                        {
                            //byte[] inBytesHash = System.Text.Encoding.UTF8.GetBytes("\r\n" + this.TextBox_IV.Text);
                            //List<byte> byteList = new List<byte>(fileBytes);
                            //byteList.AddRange(inBytesHash);
                            //byte[] inBytes = byteList.ToArray();
                            byte[] inBytes = fileBytes;

                            imgOut.Src = "res/img/encrypted.png";

                            foreach (string algo in algos)
                            {
                                string encryptAlgorithm = !string.IsNullOrEmpty(algo) ? algo : "YenMatrix";
                                outBytes = EncryptBytes(inBytes, encryptAlgorithm);
                                inBytes = outBytes;
                                cryptCount++;
                                strFileName += "." + encryptAlgorithm.ToLower();
                            }
                            lblUploadResult.Text = string.Format("file {0} x encrypted to ", cryptCount);
                        }
                        else
                        {
                            strFileName = strFileName.EndsWith(".hex") ? strFileName.Replace(".hex", "") : strFileName;
                            strFileName = strFileName.EndsWith(".oct") ? strFileName.Replace(".oct", "") : strFileName;
                            imgOut.Src = "res/img/decrypted.png";
                            for (int ig = (algos.Length - 1); ig >= 0; ig--)
                            {
                                string decryptAlogrithm = !string.IsNullOrEmpty(algos[ig]) ? algos[ig] : "YenMatrix";
                                outBytes = DecryptBytes(fileBytes, decryptAlogrithm);
                                fileBytes = outBytes;
                                cryptCount++;
                                strFileName = strFileName.EndsWith("." + decryptAlogrithm.ToLower()) ? strFileName.Replace("." + decryptAlogrithm.ToLower(), "") : strFileName;
                            }

                            fileBytes = Framework.Library.Cipher.Symmetric.CryptHelper.GetBytesTrimNulls(outBytes);
                            outBytes = fileBytes;
                            //outBytes = HandleBytes_PrivateKey_Changed(fileBytes, out bool success);
                            //if (success)
                                lblUploadResult.Text = "file has been decrypted to ";
                            //else
                            //    lblUploadResult.Text = "decrypting file failed, byte trash saved  to ";                            
                        }
                    }
                    string outMsg;
                    string savedTransFile = this.ByteArrayToFile(outBytes, out outMsg, strFileName);
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
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : string.Empty;
            string userHostString = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : string.Empty;

            byte[] encryptBytes = inBytes;
            // byte[] outBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            if (algo == "2FISH")
            {
                Framework.Library.Cipher.Symmetric.Fish2.Fish2GenWithKey(secretKey, userHostString, true);
                encryptBytes = Framework.Library.Cipher.Symmetric.Fish2.Encrypt(inBytes);
            }
            if (algo == "3FISH")
            {
                Framework.Library.Cipher.Symmetric.Fish3.Fish3GenWithKey(secretKey, userHostString, true);
                encryptBytes = Framework.Library.Cipher.Symmetric.Fish3.Encrypt(inBytes);
            }
            if (algo == "3DES")
            {
                Framework.Library.Cipher.Symmetric.Des3.Des3FromKey(secretKey, userHostString, true);
                encryptBytes = Framework.Library.Cipher.Symmetric.Des3.Encrypt(inBytes);
            }
            if (algo == "AES")
            {
                Framework.Library.Cipher.Symmetric.Aes.AesGenWithNewKey(secretKey, userHostString, true);
                encryptBytes = Framework.Library.Cipher.Symmetric.Aes.Encrypt(inBytes);
            }
            //if (algo == "RC564")
            //{
            //    RC564.RC564GenWithKey(secretKey, true);
            //    encryptBytes = RC564.Encrypt(inBytes);
            //}
            if (algo == "Serpent")
            {
                Framework.Library.Cipher.Symmetric.Serpent.SerpentGenWithKey(secretKey, userHostString, true);
                encryptBytes = Framework.Library.Cipher.Symmetric.Serpent.Encrypt(inBytes);
            }
            if (algo == "YenMatrix")
            {
                Framework.Library.Cipher.Symmetric.YenMatrix.YenMatrixGenWithKey(secretKey, userHostString, true);
                encryptBytes = Framework.Library.Cipher.Symmetric.YenMatrix.Encrypt(inBytes);
            }
            if (algo == "ZenMatrix")
            {
                Framework.Library.Cipher.Symmetric.ZenMatrix.ZenMatrixGenWithKey(secretKey, true);
                encryptBytes = Framework.Library.Cipher.Symmetric.ZenMatrix.Encrypt(inBytes);
            }
            if (algo == "Camellia" || algo == "Cast5" || algo == "Cast6" ||
                algo == "Gost28147" || algo == "Idea" || algo == "Noekeon" ||
                algo == "RC2" || algo == "RC532" || algo == "RC6" || // || algo == "RC564"
                algo == "Rijndael" ||
                algo == "Seed" || algo == "Skipjack" || // algo == "Serpent" ||
                algo == "Tea" || algo == "Tnepres" || algo == "XTea")
            {
                IBlockCipher blockCipher = Framework.Library.Cipher.Symmetric.CryptHelper.GetBlockCipher(algo, ref mode, ref blockSize, ref keyLen);

                cryptBounceCastle = new Framework.Library.Cipher.Symmetric.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, userHostString, secretKey, true);
                encryptBytes = cryptBounceCastle.Encrypt(inBytes);

            }

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
            bool sameKey = true;
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : string.Empty;
            string userHostString = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : string.Empty;

            byte[] decryptBytes = cipherBytes;
            // byte[] plainBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            if (algorithmName == "2FISH")
            {
                sameKey = Framework.Library.Cipher.Symmetric.Fish2.Fish2GenWithKey(secretKey, userHostString, false);
                decryptBytes = Framework.Library.Cipher.Symmetric.Fish2.Decrypt(cipherBytes);
            }
            if (algorithmName == "3FISH")
            {
                sameKey = Framework.Library.Cipher.Symmetric.Fish3.Fish3GenWithKey(secretKey, userHostString, true);
                decryptBytes = Framework.Library.Cipher.Symmetric.Fish3.Decrypt(cipherBytes);
            }
            if (algorithmName == "3DES")
            {
                sameKey = Framework.Library.Cipher.Symmetric.Des3.Des3FromKey(secretKey, userHostString, true);
                decryptBytes = Framework.Library.Cipher.Symmetric.Des3.Decrypt(cipherBytes);
            }
            if (algorithmName == "AES")
            {
                sameKey = Framework.Library.Cipher.Symmetric.Aes.AesGenWithNewKey(secretKey, userHostString, false);
                decryptBytes = Framework.Library.Cipher.Symmetric.Aes.Decrypt(cipherBytes);
            }
            //if (algorithmName.ToUpper() == "RC564")
            //{
            //    RC564.RC564GenWithKey(secretKey, false);
            //    decryptBytes = RC564.Decrypt(cipherBytes);
            //}
            if (algorithmName == "Serpent")
            {
                sameKey = Framework.Library.Cipher.Symmetric.Serpent.SerpentGenWithKey(secretKey, userHostString, false);
                decryptBytes = Framework.Library.Cipher.Symmetric.Serpent.Decrypt(cipherBytes);
            }
            if (algorithmName == "YenMatrix")
            {
                sameKey = Framework.Library.Cipher.Symmetric.YenMatrix.YenMatrixGenWithKey(secretKey, userHostString, false);
                decryptBytes = Framework.Library.Cipher.Symmetric.YenMatrix.Decrypt(cipherBytes);
            }
            if (algorithmName == "ZenMatrix")
            {
                sameKey = Framework.Library.Cipher.Symmetric.ZenMatrix.ZenMatrixGenWithKey(secretKey, false);
                decryptBytes = Framework.Library.Cipher.Symmetric.ZenMatrix.Decrypt(cipherBytes);
            }
            if (algorithmName == "Camellia" || algorithmName == "Cast5" || algorithmName == "Cast6" ||
                algorithmName == "Gost28147" || algorithmName == "Idea" || algorithmName == "Noekeon" ||
                algorithmName == "RC2" || algorithmName == "RC532" || algorithmName == "RC6" || // || algorithmName == "RC564" 
                algorithmName == "Rijndael" ||
                algorithmName == "Seed" || algorithmName == "Skipjack" || // algorithmName == "Serpent" || 
                algorithmName == "Tea" || algorithmName == "Tnepres" || algorithmName == "XTea")
            {
                IBlockCipher blockCipher = Framework.Library.Cipher.Symmetric.CryptHelper.GetBlockCipher(algorithmName, ref mode, ref blockSize, ref keyLen);

                cryptBounceCastle = new Framework.Library.Cipher.Symmetric.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, userHostString, secretKey, true);
                decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
            }

            return decryptBytes;
        }

        #endregion enryption_decryption_members 

        protected void Reset_TextBox_IV(string userEmailKey = "")
        {
            if (!string.IsNullOrEmpty(userEmailKey))
                this.TextBox_Key.Text = userEmailKey;
            else if (string.IsNullOrEmpty(this.TextBox_Key.Text))
                this.TextBox_Key.Text = Constants.AUTHOR_EMAIL;

            this.TextBox_IV.Text = Framework.Library.Cipher.Symmetric.CryptHelper.KeyHexString(this.TextBox_Key.Text);

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
        /// <param name="decryptedBytes"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        protected byte[] HandleBytes_PrivateKey_Changed(byte[] decryptedBytes, out bool success)
        {
            success = false;
            byte[] outBytesSameKey = null;
            byte[] ivBytesHash = System.Text.Encoding.UTF8.GetBytes("\r\n" + this.TextBox_IV.Text);
            // Framework.Library.Cipher.Symmetric.CryptHelper.GetBytesFromString("\r\n" + this.TextBox_IV.Text, 256, false);
            if (decryptedBytes != null && decryptedBytes.Length > ivBytesHash.Length)
            {
                int j = decryptedBytes.Length - 1;
                int i = ivBytesHash.Length - 1;
                while (decryptedBytes[j] != ivBytesHash[i] && decryptedBytes[j - 1] != ivBytesHash[i - 1])
                {
                    j--;
                    if (decryptedBytes.Length > 1024 && j < decryptedBytes.Length - 1024)
                        break;
                }
                for (i = ivBytesHash.Length - 1; i >= 0; i--)
                {
                    if (decryptedBytes[j] == ivBytesHash[i])
                    {
                        if (i == 0)
                        {
                            success = true;
                            break;
                        }
                        j--;
                    }
                    else
                    {
                        success = false;
                        break;
                    }
                }
                if (success)
                {
                    outBytesSameKey = new byte[decryptedBytes.Length - ivBytesHash.Length];
                    Array.Copy(decryptedBytes, outBytesSameKey, (decryptedBytes.Length - ivBytesHash.Length));
                }                
            }

            if (!success)
            {
                this.TextBox_IV.Text = "Private Key changed!";
                this.TextBox_IV.ToolTip = "Check Enforce decrypt (without key check).";
                this.TextBox_IV.BorderColor = Color.Red;
                this.TextBox_IV.BorderWidth = 2;

                this.TextBoxDestionation.Text =
                    $"Decryption failed!\r\nKey: {this.TextBox_Key.Text} with HexHash: {this.TextBox_Key.Text} doesn't match!";

                outBytesSameKey = decryptedBytes;
            }

            return outBytesSameKey;
        }

    }
}