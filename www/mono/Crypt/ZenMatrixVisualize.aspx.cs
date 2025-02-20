﻿using Area23.At;
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
            this.TextBox_Encryption.Text = "";
            this.TextBox_IV.Text = "";
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

            this.TextBox_Encryption.BorderStyle = BorderStyle.Double;
            this.TextBox_Encryption.BorderColor = Color.DarkOliveGreen;
            this.TextBox_Encryption.BorderWidth = 2;

        }

        /// <summary>
        /// Button_Reset_KeyIV_Click resets <see cref="TextBox_Key"/> and <see cref="TextBox_IV"/> to default loaded values
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>

        protected void Button_Reset_KeyIV_Click(object sender, EventArgs e)
        {
            this.TextBox_Encryption.Text = "";
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
            if (!string.IsNullOrEmpty(this.TextBox_Key.Text) && this.TextBox_Key.Text.Length > 0)
            {
                Session[Constants.AES_ENVIROMENT_KEY] = this.TextBox_Key.Text; 
                Reset_TextBox_IV((string)Session[Constants.AES_ENVIROMENT_KEY]);

                byte[] kb = Framework.Library.Crypt.Cipher.CryptHelper.GetUserKeyBytes(this.TextBox_Key.Text, this.TextBox_IV.Text, 16);
                SymmCipherEnum[] cses = new Framework.Library.Crypt.Cipher.Symmetric.SymmCipherPipe(kb).InPipe;
                this.TextBox_Encryption.Text = string.Empty;
                foreach (SymmCipherEnum c in cses)
                {
                    switch (c)
                    {
                        case SymmCipherEnum.Fish2:
                            this.TextBox_Encryption.Text += "Fish2" + ";";
                            break;
                        //case SymmCipherEnum.Fish3:
                        //    this.TextBox_Encryption.Text += "Fish3" + ";";
                        //    break;
                        case SymmCipherEnum.Des3:
                            this.TextBox_Encryption.Text += "Des3" + ";";
                            break;
                        default:
                            this.TextBox_Encryption.Text += c.ToString() + ";";
                            break;
                    }
                }

                ZenMatrix.ZenMatrixGenWithKey(this.TextBox_Key.Text, this.TextBox_IV.Text);
                string zenMt = "|zen|=>\t| ";
                foreach (sbyte sb in ZenMatrix.PermKeyHash)
                {
                    zenMt += sb.ToString("x1") + " ";
                }
                zenMt += "| \n";
                if (Session["ZenMtrx"] != null)
                    zenMt += (string)Session["ZenMtrx"];

                this.zenmatrix.InnerHtml = zenMt;
                Session["ZenMtrx"] = zenMt;                    
                
                for (int zeni = 0; zeni < ZenMatrix.PermKeyHash.Count; zeni++) 
                {
                    sbyte sb = (sbyte)ZenMatrix.PermKeyHash.ElementAt(zeni);
                    this.zenmatrix.InnerHtml += "| " + zeni.ToString("x1") + " |=>\t| " + sb.ToString("x1") + " | \n";
                }

                this.TextBox_Encryption.BorderStyle = BorderStyle.Double;
                this.TextBox_Encryption.BorderColor = Color.DarkOliveGreen;
                this.TextBox_Encryption.BorderWidth = 2;
            }
        }


        #endregion page_events


        
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

            this.TextBox_IV.Text = DeEnCoder.KeyToHex(this.TextBox_Key.Text);

            this.TextBox_IV.ForeColor = this.TextBox_Key.ForeColor;
            this.TextBox_IV.BorderColor = Color.LightGray;
            this.TextBox_IV.BorderStyle = BorderStyle.Solid;
            this.TextBox_IV.BorderWidth = 1;

            this.TextBox_Encryption.BorderStyle = BorderStyle.Solid;
            this.TextBox_Encryption.BorderColor = Color.LightGray;
            this.TextBox_Encryption.BorderWidth = 1;

            
        }

        
    }

}