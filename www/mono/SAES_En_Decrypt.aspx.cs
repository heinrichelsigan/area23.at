using Area23.At;
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
using static System.Net.WebRequestMethods;

namespace Area23.At.Mono
{
    /// <summary>
    /// SAES_En_Decrypt En-/De-cryption pipeline page 
    /// Feature to encrypt and decrypt simple plain text or files
    /// </summary>
    public partial class SAES_En_Decrypt : Util.UIPage
    {
        internal Framework.Library.Symchiffer.CryptBounceCastle cryptBounceCastle;


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

                string source = this.TextBoxSource.Text;
                string encryptedText = string.Empty;
                byte[] inBytes = Framework.Library.Symchiffer.CryptHelper.GetBytesFromString(this.TextBoxSource.Text, true);
                // System.Text.Encoding.UTF8.GetBytes(this.TextBoxSource.Text);

                string[] algos = this.TextBox_Encryption.Text.Split("⇛;,".ToCharArray());
                byte[] encryptBytes = inBytes;
                foreach (string algo in algos)
                {
                    if (!string.IsNullOrEmpty(algo))
                    {
                        encryptBytes = EncryptBytes(inBytes, algo);
                        inBytes = encryptBytes;
                    }
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
                    if (!string.IsNullOrEmpty(algos[ig]))
                    {
                        decryptedBytes = DecryptBytes(cipherBytes, algos[ig]);
                        cipherBytes = decryptedBytes;
                    }
                }

                decryptedText = Framework.Library.Symchiffer.CryptHelper.GetStringFromBytesTrimNulls(decryptedBytes);
                this.TextBoxDestionation.Text = decryptedText;
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
                // DropDownList_SymChiffer.SelectedValue.ToString() == "Noekeon" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC2" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC532" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC6" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Rijndael" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Seed" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Serpent" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Skipjack" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Tea" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Tnepres" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "XTea" ||
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
            this.TextBox_IV.Text = Framework.Library.Symchiffer.CryptHelper.KeyHexString(this.TextBox_Key.Text);
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
            string strFilePath;
            // Get the name of the file that is posted.
            string strFileName = pfile.FileName;
            strFileName = Path.GetFileName(strFileName);
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
                            imgOut.Src = "res/img/encrypted.png";

                            foreach (string algo in algos)
                            {
                                if (!string.IsNullOrEmpty(algo))
                                {
                                    outBytes = EncryptBytes(fileBytes, algo);
                                    fileBytes = outBytes;
                                    cryptCount++;
                                    strFileName += "." + algo.ToLower();
                                }
                            }
                            lblUploadResult.Text = string.Format("file {0} x encrypted to ", cryptCount);
                        }
                        else
                        {
                            imgOut.Src = "res/img/decrypted.png";
                            for (int ig = (algos.Length - 1); ig >= 0; ig--)
                            {
                                if (!string.IsNullOrEmpty(algos[ig]))
                                {
                                    outBytes = DecryptBytes(fileBytes, algos[ig]);
                                    fileBytes = outBytes;
                                    cryptCount++;
                                    strFileName = strFileName.EndsWith("." + algos[ig].ToLower()) ? strFileName.Replace("." + algos[ig].ToLower(), "") : strFileName;
                                }
                                else
                                {
                                    strFileName = strFileName.EndsWith(".hex") ? strFileName.Replace(".hex", "") : strFileName;
                                    strFileName = strFileName.EndsWith(".oct") ? strFileName.Replace(".oct", "") : strFileName;
                                }
                            }
                            lblUploadResult.Text = "file has been decrypted to ";
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
            string userHostString = (!string.IsNullOrEmpty(Page.Request.UserHostName)) ?
                Page.Request.UserHostName :
                ((!string.IsNullOrEmpty(Page.Request.UserHostAddress)) ? Page.Request.UserHostAddress : "2600:1f18:7a3f:a700::6291");

            byte[] encryptBytes = inBytes;
            byte[] outBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            if (algo == "2FISH")
            {
                Framework.Library.Symchiffer.Fish2.Fish2GenWithKey(secretKey, true);
                encryptBytes = Framework.Library.Symchiffer.Fish2.Encrypt(inBytes);
            }
            if (algo == "3FISH")
            {
                Framework.Library.Symchiffer.Fish3.Fish3GenWithKey(secretKey, true);
                encryptBytes = Framework.Library.Symchiffer.Fish3.Encrypt(inBytes);                
            }
            if (algo == "3DES")
            {
                Framework.Library.Symchiffer.Des3.Des3FromKey(secretKey, true);
                encryptBytes = Framework.Library.Symchiffer.Des3.Encrypt(inBytes);                
            }
            if (algo == "AES")
            {
                Framework.Library.Symchiffer.Aes.AesGenWithNewKey(secretKey, true);
                encryptBytes = Framework.Library.Symchiffer.Aes.Encrypt(inBytes);
            }
            //if (algo == "DesEde")
            //{                
            //    encryptBytes = DesEde.Encrypt(inBytes);
            //}
            //if (algo == "RC564")
            //{
            //    RC564.RC564GenWithKey(secretKey, true);
            //    encryptBytes = RC564.Encrypt(inBytes);
            //}
            if (algo == "Serpent")
            {
                Framework.Library.Symchiffer.Serpent.SerpentGenWithKey(secretKey, true);
                encryptBytes = Framework.Library.Symchiffer.Serpent.Encrypt(inBytes);
            }
            if (algo == "ZenMatrix")
            {
                Framework.Library.Symchiffer.ZenMatrix.ZenMatrixGenWithKey(secretKey);
                encryptBytes = Framework.Library.Symchiffer.ZenMatrix.Encrypt(inBytes);
            }                
            if (algo == "Camellia" || algo == "Cast5" || algo == "Cast6" ||
                algo == "Gost28147" || algo == "Idea" || // algo == "Noekeon" ||
                algo == "RC2" || algo == "RC532" || algo == "RC6" ||
                algo == "Rijndael" || 
                algo == "Seed" || algo == "Skipjack" ||
                algo == "Tea" || algo == "Tnepres" || algo == "XTea")
            {
                IBlockCipher blockCipher = Framework.Library.Symchiffer.CryptHelper.GetBlockCipher(algo, ref mode, ref blockSize, ref keyLen);
                
                cryptBounceCastle = new Framework.Library.Symchiffer.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, userHostString, secretKey, true);
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
            string userHostString = (!string.IsNullOrEmpty(Page.Request.UserHostName)) ?
                Page.Request.UserHostName :
                ((!string.IsNullOrEmpty(Page.Request.UserHostAddress)) ? Page.Request.UserHostAddress : "2600:1f18:7a3f:a700::6291");


            byte[] decryptBytes = cipherBytes;
            byte[] plainBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            if (algorithmName == "2FISH")
            {                
                sameKey = Framework.Library.Symchiffer.Fish2.Fish2GenWithKey(secretKey, false);
                decryptBytes = Framework.Library.Symchiffer.Fish2.Decrypt(cipherBytes);
            }
            if (algorithmName == "3FISH")
            {
                sameKey = Framework.Library.Symchiffer.Fish3.Fish3GenWithKey(secretKey, false);
                decryptBytes = Framework.Library.Symchiffer.Fish3.Decrypt(cipherBytes);                
            }
            if (algorithmName == "3DES")
            {
                sameKey = Framework.Library.Symchiffer.Des3.Des3FromKey(secretKey, false);                
                decryptBytes = Framework.Library.Symchiffer.Des3.Decrypt(cipherBytes);                
            }
            if (algorithmName == "AES")
            {
                sameKey = Framework.Library.Symchiffer.Aes.AesGenWithNewKey(secretKey, false);
                decryptBytes = Framework.Library.Symchiffer.Aes.Decrypt(cipherBytes);                
            }
            //if (algorithmName == "DesEde")
            //{
            //    decryptBytes = DesEde.Decrypt(cipherBytes);
            //}
            //if (algorithmName.ToUpper() == "RC564")
            //{
            //    RC564.RC564GenWithKey(secretKey, false);
            //    decryptBytes = RC564.Decrypt(cipherBytes);
            //}
            if (algorithmName == "Serpent")
            {
                sameKey = Framework.Library.Symchiffer.Serpent.SerpentGenWithKey(secretKey, false); 
                decryptBytes = Framework.Library.Symchiffer.Serpent.Decrypt(cipherBytes);
            }
            if (algorithmName == "ZenMatrix")
            {
                sameKey = Framework.Library.Symchiffer.ZenMatrix.ZenMatrixGenWithKey(secretKey, false);
                decryptBytes = Framework.Library.Symchiffer.ZenMatrix.Decrypt(cipherBytes);
            }
            if (algorithmName == "Camellia" || algorithmName == "Cast5" || algorithmName == "Cast6" ||
                algorithmName == "Gost28147" || algorithmName == "Idea" || // algorithmName == "Noekeon" ||
                algorithmName == "RC2" || algorithmName == "RC532" || algorithmName == "RC6" ||
                algorithmName == "Rijndael" ||
                algorithmName == "Seed" || algorithmName == "Skipjack" ||
                algorithmName == "Tea" || algorithmName == "Tnepres" || algorithmName == "XTea") 
            {
                IBlockCipher blockCipher = Framework.Library.Symchiffer.CryptHelper.GetBlockCipher(algorithmName, ref mode, ref blockSize, ref keyLen);

                cryptBounceCastle = new Framework.Library.Symchiffer.CryptBounceCastle(blockCipher, blockSize, keyLen, mode, userHostString, secretKey, true);
                decryptBytes = cryptBounceCastle.Decrypt(cipherBytes);
            }

            if (!sameKey)
            {
                this.TextBox_IV.Text = "Private Key changed!";
                this.TextBox_IV.ToolTip = "Check Enforce decrypt (without key check).";
                this.TextBox_IV.BorderColor = Color.Red;
                this.TextBox_IV.BorderWidth = 2;
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

            this.TextBox_IV.Text = Framework.Library.Symchiffer.CryptHelper.KeyHexString(this.TextBox_Key.Text);

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

        protected string Trim_Decrypted_Text(string decryptedText)
        {
            return Framework.Library.Symchiffer.CryptHelper.Trim_Decrypted_Text(decryptedText);
        }


    }
}