using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Zfx;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Input;

namespace Area23.At.Mono.Crypt
{

    /// <summary>
    /// BCrypt is an advanced crypt(3) unix hash crypt utility
    /// Great thanx to the legion of <see href="https://bouncycastle.org" />
    /// </summary>
    public partial class BCrypt : Util.UIPage
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
                    (((string)Session[Constants.AES_ENVIROMENT_KEY]).Length > 7))
                {
                    Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);
                }                            
            }

            
        }

        #region page_events

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
            this.TextBox_BCrypt_Hash.Text = "";
            this.TextBox_IV.Text = "";
            this.TextBox_Key.Text = "he@area23.at";
            this.TextBox_BCrypt_Key.Text = "";

            Session.Remove(Constants.AES_ENVIROMENT_KEY);
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

        protected void Button_BCrypt_Hash_Click(object sender, EventArgs e)
        {
            Button_BCrypt_Click(sender, e);
        }

        /// <summary>
        /// Button_BCrypt_Click sets <see cref="TextBox_BCrypt_Key" /> and <see cref="TextBox_BCrypt_Hash"/>
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_BCrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 1)
            {
                Session[Constants.AES_ENVIROMENT_KEY] = this.TextBox_Key.Text;
                Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);

                TextBox_BCrypt_Key.Text = Hex16.ToHex16(CryptHelper.BCrypt(TextBox_Key.Text));
                TextBox_BCrypt_Hash.Text = EnDeCodeHelper.KeyToHex(TextBox_BCrypt_Key.Text);

                this.TextBox_BCrypt_Key.BorderStyle = BorderStyle.Groove;
                this.TextBox_BCrypt_Key.BorderColor = Color.DarkOliveGreen;
                this.TextBox_BCrypt_Key.BorderWidth = 2;

                this.TextBox_BCrypt_Hash.BorderStyle = BorderStyle.Outset;
                this.TextBox_BCrypt_Hash.BorderColor = Color.DarkBlue;
                this.TextBox_BCrypt_Hash.BorderWidth = 2;
            }
        }

 
        #endregion page_events


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

            this.TextBox_BCrypt_Key.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBox_BCrypt_Key.BorderStyle = BorderStyle.Solid;
            this.TextBox_BCrypt_Key.BorderColor = Color.LightGray;
            this.TextBox_BCrypt_Key.BorderWidth = 1;

            this.TextBox_BCrypt_Hash.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBox_BCrypt_Hash.BorderStyle = BorderStyle.Solid;
            this.TextBox_BCrypt_Hash.BorderColor = Color.LightGray;
            this.TextBox_BCrypt_Hash.BorderWidth = 1;

        }

        #endregion helper methods
        
    }

}