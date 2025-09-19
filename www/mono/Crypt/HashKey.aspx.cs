using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Crypt
{

    /// <summary>
    /// BCrypt is an advanced crypt(3) unix hash crypt utility
    /// Great thanx to the legion of <see href="https://bouncycastle.org" />
    /// </summary>
    public partial class HashKey : UIPage
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
                this.hashKeyRadioButtonList.ParameterChanged_FireUp += new EventHandler(Button_Hash_Click);
                string aesKey = (Session[Constants.AES_ENVIROMENT_KEY] != null) ?
                    (string)Session[Constants.AES_ENVIROMENT_KEY] : TextBox_Key.Text;

                if (!string.IsNullOrEmpty(aesKey) && aesKey.Length > 0)
                    Button_Hash_Click(sender, e);
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
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
            {
                Button_Hash_Click(sender, e);
            }
        }

        /// <summary>
        /// Clear encryption pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Clear_Click(object sender, EventArgs e)
        {
            this.hashKeyRadioButtonList.SelectedKeyHashValue = KeyHash.Hex.ToString();
            this.TextBox_Key.Text = Constants.AUTHOR_EMAIL;
            this.TextBox_BCrypt_Key.Text = "";
            this.TextBox_BCrypt_Key.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBox_BCrypt_Key.BorderStyle = BorderStyle.Solid;
            this.TextBox_BCrypt_Key.BorderColor = Color.LightGray;
            this.TextBox_BCrypt_Key.BorderWidth = 1;

            if (Session[Constants.AES_ENVIROMENT_KEY] != null)
                Session.Remove(Constants.AES_ENVIROMENT_KEY);
        }


        /// <summary>
        /// Button_BCrypt_Click sets <see cref="TextBox_BCrypt_Key" /> and <see cref="TextBox_BCrypt_Hash"/>
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Hash_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 1)
            {
                Reset_TextBox_IV(this.TextBox_Key.Text);
                
                KeyHash keyHash = KeyHash.Hex;
                if (!Enum.TryParse<KeyHash>(this.hashKeyRadioButtonList.SelectedKeyHashValue, out keyHash))
                    keyHash = KeyHash.Hex;

                string hashed = keyHash.Hash(TextBox_Key.Text);

                TextBox_BCrypt_Key.Text = hashed;
                TextBox_BCrypt_Key.BorderStyle = BorderStyle.Groove;
                TextBox_BCrypt_Key.BorderColor = Color.DarkOliveGreen;
                TextBox_BCrypt_Key.BorderWidth = 2;
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
            Session[Constants.AES_ENVIROMENT_KEY] = TextBox_Key.Text;

            this.TextBox_BCrypt_Key.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBox_BCrypt_Key.BorderStyle = BorderStyle.Solid;
            this.TextBox_BCrypt_Key.BorderColor = Color.LightGray;
            this.TextBox_BCrypt_Key.BorderWidth = 1;

        }

        #endregion helper methods
        
    }

}