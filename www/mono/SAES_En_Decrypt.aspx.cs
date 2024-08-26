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
    /// <summary>
    /// SAES_En_Decrypt En-/De-cryption pipeline page 
    /// Feature to encrypt and decrypt simple plain text or files
    /// </summary>
    public partial class SAES_En_Decrypt : Util.UIPage
    {
        /// <summary>
        /// Page_Load
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                ; // handled by Event members
                if (!Page.IsPostBack)
                {
                    Reset_TextBox_IV();
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
            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                Reset_TextBox_IV();
                string source = this.TextBoxSource.Text;
                string encryptedText = string.Empty;
                byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(this.TextBoxSource.Text);
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
        }

        /// <summary>
        /// ButtonDecrypt_Click fired when ButtonDecrypt for text decryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ButtonDecrypt_Click(object sender, EventArgs e)
        {
            frmConfirmation.Visible = false;
            string cipherText = this.TextBoxSource.Text;
            string decryptedText = string.Empty;
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = cipherBytes;
            int ig = 0;

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                Reset_TextBox_IV();
                string[] algos = this.TextBox_Encryption.Text.Split("⇛;,".ToCharArray());
                for (ig = (algos.Length - 1); ig >= 0; ig--)
                {
                    if (!string.IsNullOrEmpty(algos[ig]))
                    {
                        decryptedBytes = DecryptBytes(cipherBytes, algos[ig]);
                        cipherBytes = decryptedBytes;
                    }
                }
                
                if ((ig = decryptedBytes.ArrayIndexOf((byte)0)) > 0)
                {
                    byte[] decryptedNonNullBytes = new byte[ig];
                    Array.Copy(decryptedBytes, decryptedNonNullBytes, ig);
                    decryptedText = System.Text.Encoding.UTF8.GetString(decryptedNonNullBytes);
                }
                else
                    decryptedText = System.Text.Encoding.UTF8.GetString(decryptedBytes);

                // decryptedText = Trim_Decrypted_Text(decryptedText);

                this.TextBoxDestionation.Text = decryptedText;
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
                DropDownList_SymChiffer.SelectedValue.ToString() == "DesEde" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Gost28147" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Idea" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "Noekeon" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC2" ||
                DropDownList_SymChiffer.SelectedValue.ToString() == "RC564" ||
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
            }
        }

        /// <summary>
        /// TextBox_Key_TextChanged - fired on <see cref="TextBox_Key"/> TextChanged event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>

        protected void TextBox_Key_TextChanged(object sender, EventArgs e)
        {
            byte[] keyTextData = System.Text.Encoding.UTF8.GetBytes(this.TextBox_Key.Text);
            string ivStr = string.Empty;
            foreach (byte b in keyTextData)
            {
                ivStr += b.ToString();
            }
            this.TextBox_IV.Text = ivStr;
        }

        /// <summary>
        /// Button_Reset_KeyIV_Click resets <see cref="TextBox_Key"/> and <see cref="TextBox_IV"/> to default loaded values
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Reset_KeyIV_Click(object sender, EventArgs e)
        {
            // TODO: implement it
            this.TextBox_Key.Text = Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV();
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
                                if (!string.IsNullOrEmpty(algo))
                                {
                                    outBytes = EncryptBytes(fileBytes, algo);
                                    fileBytes = outBytes;
                                    cryptCount++;
                                    strFileName += "." + algo.ToLower();
                                }
                            }
                            lblUploadResult.Text = String.Format("file {0} x encrypted to ", cryptCount);
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
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : null;

            byte[] encryptBytes = inBytes;
            byte[] outBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            if (algo == "2FISH")
            {                
                TwoFish.TwoFishGenWithKey(secretKey, true);
                encryptBytes = TwoFish.Encrypt(inBytes);
            }
            if (algo == "3FISH")
            {
                ThreeFish.ThreeFishGenWithKey(secretKey, true);
                encryptBytes = ThreeFish.Encrypt(inBytes);                
            }
            if (algo == "3DES")
            {
                TripleDes.TripleDesFromKey(secretKey, true);
                encryptBytes = TripleDes.Encrypt(inBytes);                
            }
            if (algo == "AES")
            {
                Aes.AesGenWithNewKey(secretKey, true);
                encryptBytes = Aes.Encrypt(inBytes);
            }
            if (algo == "DesEde")
            {                
                encryptBytes = DesEde.Encrypt(inBytes);
            }
            if (algo == "RC564")
            {
                RC564.RC564GenWithKey(secretKey, true);
                encryptBytes = RC564.Encrypt(inBytes);
            }
            if (algo == "Serpent")
            {
                Serpent.SerpentGenWithKey(secretKey, true);
                encryptBytes = Serpent.Encrypt(inBytes);
            }
            if (algo == "ZenMatrix")
            {
                ZenMatrix.ZenMatrixGenWithKey(secretKey);
                encryptBytes = ZenMatrix.Encrypt(inBytes);
            }                
            if (algo == "Camellia" || algo == "Gost28147" || 
                algo == "Idea" || algo == "Noekeon" ||
                algo == "RC2" || algo == "RC6" ||
                algo == "Rijndael" || 
                algo == "Seed" || algo == "Skipjack" ||
                algo == "Tea" || algo == "Tnepres" || algo == "XTea")
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
                    case "Idea":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                        break;
                    case "Noekeon":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.NoekeonEngine();
                        break;
                    case "RC2":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                        break;
                    case "RC6":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                        break;
                    case "Rijndael":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RijndaelEngine();
                        break;
                    case "Seed":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                        blockSize = 128;
                        keyLen = 16;
                        mode = "CBC";
                        break;
                    // case "Serpent":
                        // blockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                        // blockSize = 128;
                        // keyLen = 16;
                        // mode = "CBC";
                        // break;
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
                CryptBounceCastle cryptCastle = new CryptBounceCastle(blockCipher, blockSize, keyLen, mode, secretKey, true);
                encryptBytes = cryptCastle.Encrypt(inBytes, out outBytes);
            }

            // return (outBytes != null || outBytes.Length > 16) ? outBytes : encryptBytes;
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
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : null;

            byte[] decryptBytes = cipherBytes;
            byte[] plainBytes = null;
            string mode = "ECB";
            int keyLen = 32, blockSize = 256;

            if (algorithmName == "2FISH")
            {                
                sameKey = TwoFish.TwoFishGenWithKey(secretKey, false);
                decryptBytes = TwoFish.Decrypt(cipherBytes);
            }
            if (algorithmName == "3FISH")
            {
                sameKey = ThreeFish.ThreeFishGenWithKey(secretKey, false);
                decryptBytes = ThreeFish.Decrypt(cipherBytes);                
            }
            if (algorithmName == "3DES")
            {
                sameKey = TripleDes.TripleDesFromKey(secretKey, false);                
                decryptBytes = TripleDes.Decrypt(cipherBytes);                
            }
            if (algorithmName == "AES")
            {
                sameKey = Aes.AesGenWithNewKey(secretKey, false);
                decryptBytes = Aes.Decrypt(cipherBytes);                
            }
            if (algorithmName == "DesEde")
            {
                decryptBytes = DesEde.Decrypt(cipherBytes);
            }
            if (algorithmName.ToUpper() == "RC564")
            {
                RC564.RC564GenWithKey(secretKey, false);
                decryptBytes = RC564.Decrypt(cipherBytes);
            }
            if (algorithmName == "Serpent")
            {
                sameKey = Serpent.SerpentGenWithKey(secretKey, false); 
                decryptBytes = Serpent.Decrypt(cipherBytes);
            }
            if (algorithmName == "ZenMatrix")
            {
                sameKey = ZenMatrix.ZenMatrixGenWithKey(secretKey, false);
                decryptBytes = ZenMatrix.Decrypt(cipherBytes);
            }
            if (algorithmName == "Camellia" || algorithmName == "Gost28147" ||
                algorithmName == "Idea" || algorithmName == "Noekeon" ||
                algorithmName == "RC2" || algorithmName == "RC6" ||
                algorithmName == "Rijndael" ||
                algorithmName == "Seed" || algorithmName == "Skipjack" ||
                algorithmName == "Tea" || algorithmName == "Tnepres" || algorithmName == "XTea") 
            {
                IBlockCipher blockCipher;
                switch (algorithmName)
                {
                    case "Camellia":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaEngine();
                        break;
                    case "Gost28147":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                        break;
                    case "Idea":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                        break;
                    case "Noekeon":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.NoekeonEngine();
                        break;
                    case "RC2":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                        break;
                    case "RC6":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                        break;
                    case "Rijndael":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.RijndaelEngine();
                        break;
                    case "Seed":
                        blockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                        blockSize = 128;
                        keyLen = 16;
                        mode = "CBC";
                        break;
                    //case "Serpent":
                    //    blockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                    //    blockSize = 128;
                    //    keyLen = 16;
                    //    mode = "CBC";
                    //    break;
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
                CryptBounceCastle cryptCastle = new CryptBounceCastle(blockCipher, blockSize, keyLen, mode, secretKey, false);
                decryptBytes = cryptCastle.Decrypt(cipherBytes, out plainBytes);
            }

            if (!sameKey)
            {
                this.TextBox_IV.Text = "Private Key changed!";
                this.TextBox_IV.ToolTip = "Check Enforce decrypt (without key check).";
                this.TextBox_IV.BorderColor = Color.Red;
                this.TextBox_IV.BorderWidth = 2;
            }

            // return (plainBytes != null && plainBytes.Length >= 16) ? plainBytes : decryptBytes;
            return decryptBytes;
        }

        #endregion enryption_decryption_members 

        protected void Reset_TextBox_IV()
        {
            if (string.IsNullOrEmpty(this.TextBox_Key.Text))
                this.TextBox_Key.Text = Constants.AUTHOR_EMAIL;
            byte[] keyTextData = System.Text.Encoding.UTF8.GetBytes(this.TextBox_Key.Text);
            string ivStr = string.Empty;
            foreach (byte b in keyTextData)
            {
                ivStr += b.ToString();
            }
            this.TextBox_IV.Text = ivStr;
            this.TextBox_IV.BorderColor = Color.LightGray;
            this.TextBox_IV.BorderWidth = 1;
        }

        protected string Trim_Decrypted_Text(string decryptedText)
        {
            int ig = 0;
            List<char> charList = new List<char>();
            for (int i = 1; i < 32; i++)
            {
                char ch = (char)i;
                if (ch != '\v' && ch != '\f' && ch != '\t' && ch != '\r' && ch != '\n')
                    charList.Add(ch);
            }
            char[] chars = charList.ToArray();
            decryptedText = decryptedText.TrimEnd(chars);
            decryptedText = decryptedText.TrimStart(chars);
            decryptedText = decryptedText.Replace("\0", "");
            foreach (char ch in chars)
            {
                while ((ig = decryptedText.IndexOf(ch)) > 0)
                {
                    decryptedText = decryptedText.Substring(0, ig) + decryptedText.Substring(ig + 1);
                }
            }

            return decryptedText;
        }

        
    }
}