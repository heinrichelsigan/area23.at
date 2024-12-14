using Area23.At.Framework.Library.Core;
using Area23.At.Framework.Library.Core.EnDeCoding;
using Area23.At.WinForm.SecureChat.Gui.Forms;
using Area23.At.WinForm.SecureChat.Gui;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Area23.At.Framework.Library.Core.Cipher.Symm;

namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    public partial class EnDeCodingForm : TransparentFormCore
    {
        protected string savedFile = string.Empty;
        protected string loadDir = string.Empty;

        public EnDeCodingForm()
        {
            InitializeComponent();
        }

        public EnDeCodingForm(bool encodeOnly = false, bool dragNDrop = false)
        {
            InitializeComponent();
            if (encodeOnly)
            {
                this.ComboBox_SymChiffer.Enabled = false;
            }
            else
            {
                string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
                Reset_TextBox_IV(usrMailKey);
                ComboBox_SymChiffer.SelectedIndex = 0;
            }

            ComboBox_EnDeCoding.SelectedIndex = 3;
        }



        /// <summary>
        /// buttonEncode_Click fired when ButtonEncrypt for text encryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Button_Encode_Click(object sender, EventArgs e)
        {
            // frmConfirmation.Visible = false;

            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                string source = this.TextBoxSource.Text + "\r\n" + this.TextBox_IV.Text;
                string encryptedText = string.Empty;
                byte[] inBytesText = DeEnCoder.GetBytesFromString(this.TextBoxSource.Text, 256, false);
                byte[] inBytesHash = DeEnCoder.GetBytesFromString("\r\n" + this.TextBox_IV.Text, 256, false);

                byte[] inBytes = Extensions.TarBytes(inBytesText, inBytesHash);

                // EnDeCoder.GetBytes(this.TextBoxSource.Text);

                string[] algos = this.TextBox_CryptPipeline.Text.Split("+;,→⇛".ToCharArray());
                byte[] encryptBytes = inBytes;
                foreach (string algo in algos)
                {
                    if (!string.IsNullOrEmpty(algo))
                    {
                        encryptBytes = EncryptBytes(inBytes, algo);
                        inBytes = encryptBytes;
                    }
                }

                switch (this.ComboBox_EnDeCoding.SelectedItem.ToString().ToLower())
                {
                    case "hex16": encryptedText = Hex16.ToHex16(encryptBytes); break;
                    case "base16": encryptedText = Base16.ToBase16(encryptBytes); break;
                    case "base32": encryptedText = Base32.ToBase32(encryptBytes); break;
                    case "unix2unix": encryptedText = Uu.ToUu(encryptBytes); break;
                    case "base64":
                    default: encryptedText = Base64.ToBase64(encryptBytes); break;
                }

                this.TextBoxDestionation.BackColor = SystemColors.ControlLightLight;
                this.TextBoxDestionation.Text = encryptedText;

            }
            else
            {
                this.TextBox_IV.Text = "TextBox source is empty!";
                this.TextBox_IV.BackColor = Color.BlueViolet;
                this.TextBox_IV.BorderStyle = BorderStyle.FixedSingle;

                this.TextBox_IV.BackColor = Color.PeachPuff;
                this.TextBoxSource.BorderStyle = BorderStyle.FixedSingle;
            }
        }


        /// <summary>
        /// Button_Decode_Click fired when Button_Decode for text encryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Button_Decode_Click(object sender, EventArgs e)
        {
            // frmConfirmation.Visible = false;
            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                string cipherText = this.TextBoxSource.Text;
                string decryptedText = string.Empty;
                byte[] cipherBytes = null;
                switch (this.ComboBox_EnDeCoding.SelectedItem.ToString().ToLower())
                {
                    case "hex16": cipherBytes = Hex16.FromHex16(cipherText); break;
                    case "base16": cipherBytes = Base16.FromBase16(cipherText); break;
                    case "base32": cipherBytes = Base32.FromBase32(cipherText); break;
                    case "unix2unix": cipherBytes = Uu.FromUu(cipherText); break;
                    case "base64":
                    default: cipherBytes = Base64.FromBase64(cipherText); break;
                }

                byte[] decryptedBytes = cipherBytes;
                int ig = 0;

                string[] algos = this.TextBox_CryptPipeline.Text.Split("+;,→⇛".ToCharArray());
                for (ig = (algos.Length - 1); ig >= 0; ig--)
                {
                    if (!string.IsNullOrEmpty(algos[ig]))
                    {
                        decryptedBytes = DecryptBytes(cipherBytes, algos[ig]);
                        cipherBytes = decryptedBytes;
                    }
                }

                decryptedText = DeEnCoder.GetStringFromBytesTrimNulls(decryptedBytes);
                this.TextBoxDestionation.BackColor = SystemColors.ControlLightLight;
                this.TextBoxDestionation.Text = HandleString_PrivateKey_Changed(decryptedText);
            }
            else
            {
                this.TextBox_IV.Text = "TextBox source is empty!";
                this.TextBox_IV.BackColor = Color.BlueViolet;
                this.TextBox_IV.BorderStyle = BorderStyle.FixedSingle;

                this.TextBox_IV.BackColor = Color.PeachPuff;
                this.TextBoxSource.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private void Button_Load_Click(object sender, EventArgs e)
        {
            byte[] inFileBytes = OpenCryptFileDialog(ref loadDir);
            if (inFileBytes != null && inFileBytes.Length > 0)
            {
                string encryptedText = string.Empty;
                byte[] inBytesSeperator = EnDeCoder.GetBytes8("\r\n");
                byte[] inBytesKeyHash = EnDeCoder.GetBytes8(this.TextBox_IV.Text);
                byte[] inBytes = Framework.Library.Core.Extensions.TarBytes(inFileBytes, inBytesSeperator, inBytesKeyHash);
                byte[] encryptBytes = inBytes;

                string[] algos = this.TextBox_CryptPipeline.Text.Split("+;,→⇛".ToCharArray());

                foreach (string algo in algos)
                {
                    if (!string.IsNullOrEmpty(algo))
                    {
                        encryptBytes = EncryptBytes(inBytes, algo);
                        inBytes = encryptBytes;
                    }
                }
                switch (this.ComboBox_EnDeCoding.SelectedItem.ToString().ToLower())
                {
                    case "hex16": encryptedText = Hex16.ToHex16(encryptBytes); break;
                    case "base16": encryptedText = Base16.ToBase16(encryptBytes); break;
                    case "base32": encryptedText = Base32.ToBase32(encryptBytes); break;
                    case "unix2unix": encryptedText = Uu.ToUu(encryptBytes); break;
                    case "base64":
                    default: encryptedText = Base64.ToBase64(encryptBytes); break;
                }

                this.TextBoxDestionation.BackColor = SystemColors.ControlLightLight;
                this.TextBoxDestionation.Text = encryptedText;
            }
            else
            {
                this.TextBox_IV.Text = "In file source was empty!";
                this.TextBox_IV.BackColor = Color.AliceBlue;
                this.TextBox_IV.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        /// <summary>
        /// Button_Save_Click fired when ButtonDecrypt for text decryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Button_Save_Click(object sender, EventArgs e)
        {
            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);

            if (this.TextBoxSource.Text != null && TextBoxSource.Text.Length > 0)
            {
                string cipherText = this.TextBoxSource.Text;
                string decryptedText = string.Empty;
                byte[] cipherBytes = null, fileBytes = null, outBytes = null;
                switch (this.ComboBox_EnDeCoding.SelectedText.ToLower())
                {
                    case "hex16": cipherBytes = Hex16.FromHex16(cipherText); break;
                    case "base16": cipherBytes = Base16.FromBase16(cipherText); break;
                    case "base32": cipherBytes = Base32.FromBase32(cipherText); break;
                    case "unix2unix": cipherBytes = Uu.FromUu(cipherText); break;
                    case "base64":
                    default: cipherBytes = Base64.FromBase64(cipherText); break;
                }

                byte[] decryptedBytes = cipherBytes;
                int ig = 0;

                string[] algos = this.TextBox_CryptPipeline.Text.Split("+;,→⇛".ToCharArray());
                for (ig = (algos.Length - 1); ig >= 0; ig--)
                {
                    if (!string.IsNullOrEmpty(algos[ig]))
                    {
                        decryptedBytes = DecryptBytes(cipherBytes, algos[ig]);
                        cipherBytes = decryptedBytes;
                    }
                }

                outBytes = DeEnCoder.GetBytesTrimNulls(decryptedBytes);
                fileBytes = HandleBytes_PrivateKey_Changed(outBytes, out bool success);
                decryptedText = DeEnCoder.GetStringFromBytesTrimNulls(decryptedBytes);
                this.TextBoxDestionation.Text = HandleString_PrivateKey_Changed(decryptedText);
                if (fileBytes != null && fileBytes.Length > 0)
                {
                    string ext = MimeType.GetFileExtForMimeTypeApache(MimeType.GetMimeType(fileBytes, ".hex"));
                    string fName = AppDomain.CurrentDomain.BaseDirectory + LibPaths.SepChar + DateTime.Now.Area23Date() + "." + ext;
                    savedFile = base.SafeFileName(fName, fileBytes);
                }
            }
            else
            {
                this.TextBox_IV.Text = "TextBox source is empty!";
                this.TextBox_IV.BackColor = Color.BlueViolet;
                this.TextBox_IV.BorderStyle = BorderStyle.FixedSingle;

                this.TextBox_IV.BackColor = Color.PeachPuff;
                this.TextBoxSource.BorderStyle = BorderStyle.FixedSingle;
            }


        }

        private void Button_AddToPipeline_Click(object sender, EventArgs e)
        {
            this.TextBox_CryptPipeline.Text += this.ComboBox_SymChiffer.SelectedItem + "⇒";
            this.TextBox_CryptPipeline.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>
        /// Button_ClearPipeline_Click => Clear encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Button_ClearPipeline_Click(object sender, EventArgs e)
        {
            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);
            this.TextBox_CryptPipeline.Text = string.Empty;
            this.TextBoxDestionation.Text = string.Empty;
        }

        private void Button_SecretKey_Click(object sender, EventArgs e)
        {
            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);
        }



        /// <summary>
        /// Add encryption alog to encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageButton_Add_Click(object sender, EventArgs e)
        {
            string addChiffre = "";
            if (ComboBox_SymChiffer.SelectedItem.ToString() == "3DES" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "2FISH" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "3FISH" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "AES" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Camellia" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Cast5" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Cast6" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "DesEde" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Gost28147" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Idea" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Noekeon" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "RC2" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "RC532" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "RC564" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "RC6" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Rijndael" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Seed" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Serpent" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Skipjack" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Tea" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "Tnepres" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "XTea" ||
                ComboBox_SymChiffer.SelectedItem.ToString() == "ZenMatrix")
            {
                addChiffre = ComboBox_SymChiffer.SelectedItem.ToString() + "⇒";
                this.TextBox_CryptPipeline.Text += addChiffre;
                this.TextBox_CryptPipeline.BorderStyle = BorderStyle.FixedSingle;
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
            this.TextBox_IV.BackColor = Color.Red;
            this.TextBox_IV.BorderStyle = BorderStyle.FixedSingle;

            this.TextBox_CryptPipeline.BorderStyle = BorderStyle.FixedSingle;
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
        /// Generic encrypt bytes to bytes
        /// </summary>
        /// <param name="inBytes">Array of byte</param>
        /// <param name="algo">Symetric chiffre algorithm</param>
        /// <returns>encrypted byte Array</returns>
        /// 
        protected byte[] EncryptBytes(byte[] inBytes, string algo)
        {
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : string.Empty;
            string keyIv = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : string.Empty;

            byte[] encryptBytes = Crypt.EncryptBytes(inBytes, algo, secretKey, keyIv);

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
            string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : string.Empty;
            string keyIv = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : string.Empty;

            byte[] decryptBytes = Crypt.DecryptBytes(cipherBytes, algorithmName, secretKey, keyIv);

            return decryptBytes;
        }


        protected void Reset_TextBox_IV(string userEmailKey = "")
        {
            if (!string.IsNullOrEmpty(userEmailKey))
                this.TextBox_Key.Text = userEmailKey;
            else if (string.IsNullOrEmpty(this.TextBox_Key.Text))
                this.TextBox_Key.Text = Constants.AUTHOR_EMAIL;

            this.TextBox_Key.ForeColor = SystemColors.ControlText;
            this.TextBox_Key.BackColor = SystemColors.ControlLightLight;

            this.TextBox_IV.Text = DeEnCoder.KeyHexString(this.TextBox_Key.Text);
            this.TextBox_IV.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBox_IV.BackColor = this.TextBox_Key.BackColor;
            this.TextBox_IV.BorderStyle = BorderStyle.None;

            this.TextBoxSource.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBoxSource.BackColor = SystemColors.ControlLightLight;
            this.TextBoxSource.BorderStyle = BorderStyle.None;
            // this.TextBoxSource.BorderWidth = 1;

            this.TextBoxDestionation.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBoxDestionation.BackColor = SystemColors.Control;
            this.TextBoxDestionation.BorderStyle = BorderStyle.None;
            // this.TextBoxDestionation.BorderWidth = 1;

            this.TextBox_CryptPipeline.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBox_CryptPipeline.BackColor = this.TextBox_Key.BackColor;
            this.TextBox_CryptPipeline.BorderStyle = BorderStyle.None;
            // this.TextBox_Encryption.BorderWidth = 1;
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
                    else if ((sameKey = decryptedText.Contains(shouldEndWithIv.Substring(0, shouldEndWithIv.Length - 3))))
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
                // this.TextBox_IV. = "Check Enforce decrypt (without key check).";
                this.TextBox_IV.BorderStyle = BorderStyle.FixedSingle;
                // this.TextBox_IV.BorderWidth = 2;

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
            byte[] ivBytesHash = EnDeCoder.GetBytes8("\r\n" + this.TextBox_IV.Text);
            // Framework.Library.Cipher.Symmetric.CryptHelper.GetBytesFromString("\r\n" + this.TextBox_IV.Text, 256, false);
            if (decryptedBytes != null && decryptedBytes.Length > ivBytesHash.Length)
            {
                int needleFound = Framework.Library.Core.Extensions.BytesBytes(decryptedBytes, ivBytesHash, ivBytesHash.Length - 1);
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
                // this.TextBox_IV.ToolTip = "Check Enforce decrypt (without key check).";
                this.TextBox_IV.BorderStyle = BorderStyle.Fixed3D;
                this.TextBox_IV.BackColor = Color.Red;

                this.TextBoxDestionation.Text = errorMsg;

            }

            return decryptedBytes;
        }

        private void Button_HashIv_Click(object sender, EventArgs e)
        {
            string usrMailKey = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);
        }

        private void EnDeCodingForm_Load(object sender, EventArgs e)
        {

        }
    }
}
