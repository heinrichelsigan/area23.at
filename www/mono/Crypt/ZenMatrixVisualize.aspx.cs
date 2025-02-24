using Area23.At;
using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Zfx;
using Area23.At.Mono.Properties;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
    /// CoolCrypt is En-/De-cryption pipeline page 
    /// Former hash inside crypted bytestream is removed
    /// Feature to encrypt and decrypt simple plain text or files
    /// </summary>
    public partial class ZenMatrixVisualize : Util.UIPage
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
                try
                {
                    if ((Session[Constants.AES_ENVIROMENT_KEY] != null) && !string.IsNullOrEmpty((string)Session[Constants.AES_ENVIROMENT_KEY]) &&
                        (((string)Session[Constants.AES_ENVIROMENT_KEY]).Length > 7))
                    {
                        Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);
                    }
                }
                catch (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                }
            }
           
        }

        #region page_events


        /// <summary>
        /// Clear encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Clear_Click(object sender, EventArgs e)
        {            
            this.TextBox_IV.Text = "";
            ClearMatrix();
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
        /// Fired, when DropDownList_Encoding_SelectedIndexChanged
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void DropDownList_Encoding_SelectedIndexChanged(object sender, EventArgs e)
        {            
        }

        /// <summary>
        /// TextBox_Key_TextChanged - fired on <see cref="TextBox_Key"/> TextChanged event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        [Obsolete("TextBox_Key_TextChanged is fully deprectated, because no autopostback anymore", true)]
        protected void TextBox_Key_TextChanged(object sender, EventArgs e)
        {
            this.TextBox_IV.Text = DeEnCoder.KeyToHex(this.TextBox_Key.Text);
            this.TextBox_IV.BorderColor = Color.GreenYellow;
            this.TextBox_IV.ForeColor = Color.DarkOliveGreen;
            this.TextBox_IV.BorderStyle = BorderStyle.Dotted;
            this.TextBox_IV.BorderWidth = 1;

        }

        /// <summary>
        /// Button_Reset_KeyIV_Click resets <see cref="TextBox_Key"/> and <see cref="TextBox_IV"/> to default loaded values
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>

        protected void Button_Reset_KeyIV_Click(object sender, EventArgs e)
        {
            this.TextBox_IV.Text = "";
        }


        /// <summary>
        /// Saves current email address as crypt key inside that asp Session
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Key_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 3)
            {
                Session[Constants.AES_ENVIROMENT_KEY] = this.TextBox_Key.Text;
                Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);
            }
        }

        /// <summary>
        /// Button_Hash_Click sets hash from key and fills pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Hash_Click(object sender, EventArgs e)
        {
            ClearMatrix();
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
            {
                Session[Constants.AES_ENVIROMENT_KEY] = this.TextBox_Key.Text; 
                Reset_TextBox_IV(this.TextBox_Key.Text);

                byte[] kb = Framework.Library.Crypt.Cipher.CryptHelper.GetUserKeyBytes(this.TextBox_Key.Text, this.TextBox_IV.Text, 16);
                SymmCipherEnum[] cses = new Framework.Library.Crypt.Cipher.Symmetric.SymmCipherPipe(kb).InPipe;
                
                
                ZenMatrix z = new ZenMatrix(this.TextBox_Key.Text, this.TextBox_IV.Text, true);
                string zenMt = "|zen|=>\t| ";
                
                int b = 0xf;
                sbyte[] myBytes = z.PermutationKeyHash.ToArray();
                string permHashString = string.Empty;
                foreach (sbyte sb in myBytes)
                {
                    Control ctrl = MatrixTable.FindControl("TextBox_" + b.ToString("x1") + "_" + sb.ToString("x1"));
                    permHashString += sb.ToString("x1");
                    if (ctrl != null && ctrl is TextBox tb)
                    {
                        tb.BorderWidth = 2;
                        tb.BorderStyle = BorderStyle.Outset;
                        tb.BorderColor = new Color().FromXrgb("#222222");
                        tb.Text = "1";
                    }

                    Control lctrl = MatrixTable.FindControl("TextBox_" + b.ToString("x1") + "_vf");
                    if (lctrl != null && lctrl is TextBox ttb)
                    {
                        ttb.BorderWidth = 2;
                        ttb.Text = sb.ToString("x1");
                        ttb.BorderStyle = BorderStyle.Outset;
                        ttb.BorderColor = new Color().FromXrgb("#222222");
                    }
                    b--;
                }
                this.TextBoxPermutation.Text = permHashString;
            }
        }


        #endregion page_events

        protected void ClearMatrix()
        {
            this.TextBoxPermutation.Text = string.Empty;
            for (int b = 0xf; b >= 0; b--)
            {
                for (int a = 0; a < 16; a++)
                {
                    Control ctrl = MatrixTable.FindControl("TextBox_" + b.ToString("x1") + "_" + a.ToString("x1"));
                    if (ctrl != null && ctrl is TextBox tb)
                    {
                        tb.Text = "0";
                        tb.BorderWidth = 1;
                        tb.BorderStyle = BorderStyle.Inset;
                        tb.BorderColor = new Color().FromXrgb("#cfcfcf");
                    }
                }
                Control lctrl = MatrixTable.FindControl("TextBox_" + b.ToString("x1") + "_vf");
                if (lctrl != null && lctrl is TextBox ttb)
                {
                    ttb.Text = "0";
                    ttb.BorderWidth = 1;
                    ttb.BorderStyle = BorderStyle.Inset;
                    ttb.BorderColor = new Color().FromXrgb("#cfcfcf");
                }
            }
        }

        
        /// <summary>
        /// Resets TextBox Key_IV to standard value for <see cref="Constants.AUTHOR_EMAIL"/>
        /// </summary>
        /// <param name="userEmailKey">user email key to generate key bytes iv</param>
        protected void Reset_TextBox_IV(string userEmailKey = "")
        {
            if (!string.IsNullOrEmpty(userEmailKey))
                this.TextBox_Key.Text = userEmailKey;
            else if (string.IsNullOrEmpty(this.TextBox_Key.Text))
                this.TextBox_Key.Text = DateTime.Now.Ticks.ToString();

            this.TextBox_IV.Text = DeEnCoder.KeyToHex(this.TextBox_Key.Text);

            this.TextBox_IV.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBox_IV.BorderColor = Color.LightGray;
            this.TextBox_IV.BorderStyle = BorderStyle.Solid;
            this.TextBox_IV.BorderWidth = 1;           
        }

        
    }

}