using Area23.At.Mono.Properties;
using Area23.At.Mono.Util;
using Area23.At.Mono.Util.Enum;
using Area23.At.Mono.Util.SymChiffer;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Caching;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.WebRequestMethods;

namespace Area23.At.Mono
{
    public partial class SAES_En_Decrypt : Util.UIPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                ; // handled by Event members
            }
        }

        #region page_events

        protected void ButtonEncryptFile_Click(object sender, EventArgs e)
        {
            if (Request.Files != null && Request.Files.Count > 0)
                EnDeCryptUploadFile(Request.Files[0], true);
        }


        protected void ButtonDecryptFile_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(oFile.Value))
            {
                EnDeCryptUploadFile(oFile.PostedFile, false);
            }
        }


        protected void ButtonEncrypt_Click(object sender, EventArgs e)
        {
            frmConfirmation.Visible = false;
            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                string source = this.TextBoxSource.Text;
                string encryptedText = string.Empty;
                byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(this.TextBoxSource.Text);
                string[] algos = this.TextBox_Encryption.Text.Split("⇛;,".ToCharArray());
                byte[] encryptBytes = inBytes;
                foreach (string algo in algos)
                {
                    encryptBytes = EncryptBytes(inBytes, algo);
                    inBytes = encryptBytes;
                }

                encryptedText = Convert.ToBase64String(encryptBytes);
                this.TextBoxDestionation.Text = encryptedText;
            }
        }

        protected void ButtonDecrypt_Click(object sender, EventArgs e)
        {
            frmConfirmation.Visible = false;
            string cipherText = this.TextBoxSource.Text;
            string decryptedText = string.Empty;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = cipherBytes;
            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                string[] algos = this.TextBox_Encryption.Text.Split("⇛;,".ToCharArray());
                for (int ig = (algos.Length - 1); ig >= 0; ig--)
                {
                    decryptedBytes = DecryptBytes(cipherBytes, algos[ig]);
                    cipherBytes = decryptedBytes;
                }
                List<char> charList = new List<char>();
                for (int i = 0; i < 32; i++)
                {
                    if (i != 10 && i != 13)
                    {
                        char ch = (char)i;
                        if (ch != '\v' && ch != '\t' && ch != '\r' && ch != '\n')
                            charList.Add(ch);
                    }
                }
                char[] chars = charList.ToArray();
                decryptedText = System.Text.Encoding.UTF8.GetString(decryptedBytes).TrimEnd(chars);
                foreach (char ch in chars)
                {
                    if (decryptedText.IndexOf(ch) > 0)
                        decryptedText = decryptedText.Substring(0, decryptedText.IndexOf(ch) + 1);
                }
                if (decryptedText.LastIndexOf('\0') > 0)
                    decryptedText = decryptedText.Substring(0, decryptedText.LastIndexOf('\0'));
                this.TextBoxDestionation.Text = decryptedText;
            }
        }


        protected void Button_Clear_Click(object sender, EventArgs e)
        {
            this.TextBox_Encryption.Text = "";
        }

        protected void Button_Reset_KeyIV_Click(object sender, EventArgs e)
        {
            // TODO: implement it
        }

        protected void ImageButton_Add_Click(object sender, EventArgs e)
        {
            string addChiffre = "";
            if (DropDownList_SymChiffer.SelectedValue.ToString() == "2FISH" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "3FISH" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "3DES" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "AES" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Camellia" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "DesEde" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Gost28147" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC2" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC532" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC564" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC6" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Rijndael" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Serpent" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Skipjack" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Tea" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Tnepres" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "XTea")
            {
                addChiffre = DropDownList_SymChiffer.SelectedValue.ToString() + "⇛";
                this.TextBox_Encryption.Text += addChiffre;
            }
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
                //strFilePath = Paths.OutDirPath + strFileName;
                //while (System.IO.File.Exists(strFilePath))
                //{
                //    string newFileName = strFilePath.Contains(Constants.DateFile) ?
                //        Constants.DateFile + Guid.NewGuid().ToString() + "_" + strFileName :
                //        Constants.DateFile + strFileName;
                //    strFilePath = Paths.OutDirPath + newFileName;
                //    lblUploadResult.Text = String.Format("{0} already exists on server, saving it to {1}.",
                //        strFileName, newFileName);
                //}

                //pfile.SaveAs(strFilePath);
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
                                outBytes = EncryptBytes(fileBytes, algo);
                                fileBytes = outBytes;
                                cryptCount++;
                                strFileName += "." + algo.ToLower();                                
                            }
                            lblUploadResult.Text = String.Format("file {0} x encrypted to ", cryptCount);
                        }
                        else
                        {
                            imgOut.Src = "res/img/decrypted.png";
                            for (int ig = (algos.Length - 1); ig >= 0; ig--)
                            {                                
                                outBytes = DecryptBytes(fileBytes, algos[ig]);
                                fileBytes = outBytes;
                                cryptCount++;
                                strFileName = strFileName.EndsWith("." + algos[ig].ToLower()) ? strFileName.Replace("." + algos[ig].ToLower(), "") : strFileName;                                
                            }
                            lblUploadResult.Text = "file has been decrypted to ";
                        }
                    }
                    string outMsg;
                    string savedTransFile = this.ByteArrayToFile(outBytes, out outMsg, strFileName);
                    aTransFormed.HRef = "res/" + savedTransFile;
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
        protected byte[] EncryptBytes(byte[] inBytes, string algo)
        {
            byte[] encryptBytes = inBytes;
            byte[] outBytes;
            if (algo == "2FISH")
            {
                encryptBytes = TwoFish.Encrypt(inBytes);
            }
            if (algo == "3FISH")
            {
                encryptBytes = ThreeFish.Encrypt(inBytes);
            }
            if (algo == "3DES")
            {
                encryptBytes = TripleDes.Encrypt(inBytes);
            }
            if (algo == "AES")
            {
                encryptBytes = Aes.Encrypt(inBytes);
            }
            if (algo == "DesEde")
            {
                encryptBytes = DesEde.Encrypt(inBytes);
            }
            if (algo == "Camellia")
            {
                encryptBytes = Camellia.Encrypt(inBytes);
            }
            if (algo == "RC564")
            {
                encryptBytes = RC564.Encrypt(inBytes);
            }
            //if (algo == "Serpent")
            //    encryptBytes = Serpent.Encrypt(inBytes);
            if (algo == "Gost28147" || algo == "RC2" || algo == "RC532" || algo == "RC6" ||
                algo == "Rijndael" || algo == "Skipjack" || algo == "Rfc5649" ||
                algo == "Serpent" || algo == "Tea" || algo == "Tnepres" || algo == "XTea")
            {
                IBlockCipher blockCipher;
                switch (algo)
                {
                    case "Camellia":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaEngine();
                        break;
                    case "Gost28147":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                        break;
                    case "RC2":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                        break;
                    case "RC532":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                        break;
                    case "RC6":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                        break;
                    case "Rijndael":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RijndaelEngine();
                        break;
                    case "Skipjack":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                        break;
                    case "Tea":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                        break;
                    case "Tnepres":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.TnepresEngine();
                        break;
                    case "XTea":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                        break;
                    default:
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                        break;
                }
                CryptBounceCastle cryptCastle = new CryptBounceCastle(blockCipher);
                encryptBytes = cryptCastle.Encrypt(inBytes, out outBytes);
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
            byte[] decryptBytes = cipherBytes;
            byte[] plainBytes;
            if (algorithmName == "2FISH")
            {
                decryptBytes = TwoFish.Decrypt(cipherBytes);
            }
            if (algorithmName == "3FISH")
            {
                decryptBytes = ThreeFish.Decrypt(cipherBytes);
            }
            if (algorithmName == "3DES")
            {
                decryptBytes = TripleDes.Decrypt(cipherBytes);
            }
            if (algorithmName == "AES")
            {
                decryptBytes = Aes.Decrypt(cipherBytes);
            }
            if (algorithmName == "DesEde")
            {
                decryptBytes = DesEde.Decrypt(cipherBytes);
            }
            if (algorithmName == "Camellia")
            {
                decryptBytes = Camellia.Decrypt(cipherBytes);
            }
            if (algorithmName.ToUpper() == "RC564")
            {
                decryptBytes = RC564.Decrypt(cipherBytes);
            }
            //if (algorithmName.ToUpper() == "Serpent")
            //    decryptBytes = Serpent.Decrypt(cipherBytes);
            if (algorithmName == "Gost28147" ||
                algorithmName == "RC2" || algorithmName == "RC532" || algorithmName == "RC6" ||
                algorithmName == "Rijndael" || algorithmName == "Skipjack" || algorithmName == "Rfc5649" ||
                algorithmName == "Serpent" || algorithmName == "Tea" || algorithmName == "Tnepres" || algorithmName == "XTea") 
            {
                IBlockCipher blockCipher;
                switch (algorithmName)
                {
                    case "Gost28147":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                        break;
                    case "RC2":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                        break;
                    case "RC532":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                        break;
                    case "RC6":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                        break;
                    case "Rijndael":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RijndaelEngine();
                        break;
                    case "Skipjack":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                        break;
                    case "Tea":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                        break;
                    case "Tnepres":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.TnepresEngine();
                        break;
                    case "XTea":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                        break;
                    default:
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                        break;
                }
                CryptBounceCastle cryptCastle = new CryptBounceCastle(blockCipher);
                decryptBytes = cryptCastle.Decrypt(cipherBytes, out plainBytes);
            }

            return decryptBytes;
        }

        #endregion enryption_decryption_members 
    }
}