using Area23.At.Framework.Library.Cache;
using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Area23.At.Mono.Crypt
{

    /// <summary>
    /// CoolCrypt is En-/De-cryption pipeline page 
    /// Former hash inside crypted bytestream is removed
    /// Feature to encrypt and decrypt simple plain text or files
    /// </summary>
    public partial class ZenMatrixVisualize : UIPage
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
                    string aesKey = MemoryCache.CacheDict.ContainsKey(Constants.AES_ENVIROMENT_KEY) ?
                        MemoryCache.CacheDict.GetValue<string>(Constants.AES_ENVIROMENT_KEY) : TextBox_Key.Text;
                    if (!string.IsNullOrEmpty(aesKey) && aesKey.Length > 0)
                        Reset_TextBox_IV(aesKey);
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
            this.RadioButtonList_Hash.SelectedValue = KeyHash.Hex.ToString();
            this.TextBox_Key.Text = Constants.AUTHOR_EMAIL;
            this.TextBox_IV.Text = "";
            this.TextBox_IV.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBox_IV.BorderStyle = BorderStyle.Solid;
            this.TextBox_IV.BorderColor = Color.LightGray;
            this.TextBox_IV.BorderWidth = 1;

            ClearMatrix();

            if (MemoryCache.CacheDict.ContainsKey(Constants.AES_ENVIROMENT_KEY))
                MemoryCache.CacheDict.RemoveKey(Constants.AES_ENVIROMENT_KEY);
        }

        /// <summary>
        /// Change mode from key keyHash to bcrypted key and keyHash from bcrypted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckBox_FullSymmetric_OnCheckedChanged(object sender, EventArgs e)
        {
            Button_Hash_Click(sender, e);
        }

        /// <summary>
        /// TextBox_Key_TextChanged - fired on <see cref="TextBox_Key"/> TextChanged event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        [Obsolete("TextBox_Key_TextChanged is fully deprectated, because no autopostback anymore", true)]
        protected void TextBox_Key_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
                Reset_TextBox_IV(this.TextBox_Key.Text);
        }

        /// <summary>
        /// Button_Reset_KeyIV_Click resets <see cref="TextBox_Key"/> and <see cref="TextBox_IV"/> to default loaded values
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>

        protected void Button_Reset_KeyIV_Click(object sender, EventArgs e)
        {
            Button_Clear_Click(sender, e);
        }


        /// <summary>
        /// Saves current email address as crypt key inside that asp Session
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Key_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
                Reset_TextBox_IV(this.TextBox_Key.Text);
        }


        /// <summary>
        /// RadioButtonList_Hash_ParameterChanged set key hash algorithm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RadioButtonList_Hash_ParameterChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
            {
                Reset_TextBox_IV(this.TextBox_Key.Text);
                
                ClearMatrix();
                string key = TextBox_Key.Text;
                string keyHash = TextBox_IV.Text;
                DrawZenMatrix(key, keyHash);

            }
        }


        /// <summary>
        /// Button_Hash_Click sets hash from key and fills pipeline
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Button_Hash_Click(object sender, EventArgs e)
        {            
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
            {
                ClearMatrix();
                Reset_TextBox_IV(this.TextBox_Key.Text);

                string key = TextBox_Key.Text;
                string keyHash = TextBox_IV.Text;

                DrawZenMatrix(key, keyHash);
            }
        }


        #endregion page_events

        /// <summary>
        /// Clears the drawn matrix
        /// </summary>
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
        /// Draws a ZenMatrix
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyHash"></param>
        protected void DrawZenMatrix(string key, string keyHash)
        {
            byte[] kb = Framework.Library.Crypt.Cipher.CryptHelper.GetUserKeyBytes(key, keyHash, 16);
            SymmCipherEnum[] cses = new Framework.Library.Crypt.Cipher.Symmetric.SymmCipherPipe(kb).InPipe;

            bool fullSymmetric = false;
            try
            {
                fullSymmetric = this.CheckBox_FullSymmetric.Checked;
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
            }

            ZenMatrix z = new ZenMatrix(key, keyHash, fullSymmetric);
            // string zenMt = "|zen|=>\t| ";

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
            MemoryCache.CacheDict.SetValue<string>(Constants.AES_ENVIROMENT_KEY, TextBox_Key.Text);

            KeyHash keyHash = KeyHash.Hex;
            if (!Enum.TryParse<KeyHash>(RadioButtonList_Hash.SelectedValue, out keyHash))
                keyHash = KeyHash.Hex;

            string hashed = keyHash.Hash(TextBox_Key.Text);

            TextBox_IV.Text = hashed;
            TextBox_IV.ForeColor = this.TextBox_Key.ForeColor;
            TextBox_IV.BorderColor = Color.LightGray;
            TextBox_IV.BorderStyle = BorderStyle.Solid;
            TextBox_IV.BorderWidth = 1;

        }

    }

}