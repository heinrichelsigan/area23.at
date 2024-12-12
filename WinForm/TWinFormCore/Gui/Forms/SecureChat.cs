using Area23.At.Framework.Library.Core;
using Area23.At.Framework.Library.Core.EnDeCoding;
using Area23.At.Framework.Library.Core.SymCipher;
using Area23.At.WinForm.TWinFormCore.Gui.Forms;
using Area23.At.WinForm.TWinFormCore.Gui;
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
using Area23.At.Framework.Library.Core.Net;
using System.Net;

namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    public partial class SecureChat : TransparentFormCore
    {
        protected string savedFile = string.Empty;
        protected string loadDir = string.Empty;

        public SecureChat()
        {
            InitializeComponent();
        }

        public SecureChat(bool encodeOnly = false, bool dragNDrop = false)
        {
            InitializeComponent();
            if (encodeOnly)
            {
                this.ComboBox_RemoteEndPoint.Enabled = false;
            }
            else
            {
                string usrMailKey = (!string.IsNullOrEmpty(this.textBoxEnter.Text)) ? this.textBoxEnter.Text : Constants.AUTHOR_EMAIL;
                Reset_TextBox_IV(usrMailKey);
                ComboBox_RemoteEndPoint.SelectedIndex = 0;
            }

            ComboBox_LocalEndPoint.SelectedIndex = 3;
        }

        private void SecureChat_Load(object sender, EventArgs e)
        {
            List<IPAddress> list = NetworkAddresses.GetConnectedIpAddresses();
            
            int mchecked = 0;
            foreach (IPAddress addr in list)
            {
                if (addr != null) 
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(addr.AddressFamily + " " + addr.ToString(), null, null, addr.ToString());
                    if (mchecked++ == 0)
                        item.Checked = true;
                    else item.Checked = false;

                    this.toolStripMenuView.DropDownItems.Add(item);
                }
            }
        }


        /// <summary>
        /// buttonEncode_Click fired when ButtonEncrypt for text encryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Button_Encode_Click(object sender, EventArgs e)
        {
            // frmConfirmation.Visible = false;

            string usrMailKey = (!string.IsNullOrEmpty(this.textBoxEnter.Text)) ? this.textBoxEnter.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);
            
        }


        /// <summary>
        /// Button_Decode_Click fired when Button_Decode for text encryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Button_Decode_Click(object sender, EventArgs e)
        {
            // frmConfirmation.Visible = false;
            string usrMailKey = Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);
            
        }

        private void Button_Load_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Button_Save_Click fired when ButtonDecrypt for text decryption receives event Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Button_Save_Click(object sender, EventArgs e)
        {
            

        }

        private void Button_AddToPipeline_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Button_ClearPipeline_Click => Clear encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        private void Button_ClearPipeline_Click(object sender, EventArgs e)
        {
            string usrMailKey = (!string.IsNullOrEmpty(this.textBoxEnter.Text)) ? this.textBoxEnter.Text : Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);
            this.TextBoxSource.Text = string.Empty;
            this.TextBoxDestionation.Text = string.Empty;
        }

        private void Button_SecretKey_Click(object sender, EventArgs e)
        {
            
        }



        /// <summary>
        /// Add encryption alog to encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImageButton_Add_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// TextBox_Key_TextChanged - fired on <see cref="TextBox_Key"/> TextChanged event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>

        protected void TextBox_Key_TextChanged(object sender, EventArgs e)
        {
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
            // string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : string.Empty;
            // string keyIv = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : string.Empty;

            // byte[] encryptBytes = Crypt.EncryptBytes(inBytes, algo, secretKey, keyIv);
            // return encryptBytes;
            return null;
        }

        /// <summary>
        /// Generic decrypt bytes to bytes
        /// </summary>
        /// <param name="cipherBytes">Encrypted array of byte</param>
        /// <param name="algorithmName">Symetric chiffre algorithm</param>
        /// <returns>decrypted byte Array</returns>
        protected byte[] DecryptBytes(byte[] cipherBytes, string algorithmName)
        {
            // string secretKey = !string.IsNullOrEmpty(this.TextBox_Key.Text) ? this.TextBox_Key.Text : string.Empty;
            // string keyIv = (!string.IsNullOrEmpty(this.TextBox_Key.Text)) ? this.TextBox_Key.Text : string.Empty;

            // byte[] decryptBytes = Crypt.DecryptBytes(cipherBytes, algorithmName, secretKey, keyIv);
            // return decryptBytes;
            return null;
        }


        protected void Reset_TextBox_IV(string userEmailKey = "")
        {
            // this.TextBox_Encryption.BorderWidth = 1;
        }


        /// <summary>
        /// Handles string decryption, compares if private key & hex hash match in decrypted text
        /// </summary>
        /// <param name="decryptedText">decrypted plain text</param>
        /// <returns>decrypted plain text without check hash or an error message, in case that check hash doesn't match.</returns>
        protected string HandleString_PrivateKey_Changed(string decryptedText)
        {
            
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
            success = true;
            return decryptedBytes;
        }

        private void Button_HashIv_Click(object sender, EventArgs e)
        {
            string usrMailKey = Constants.AUTHOR_EMAIL;
            Reset_TextBox_IV(usrMailKey);
        }

        
    }
}
