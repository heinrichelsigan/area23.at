using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;

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
            Response.Redirect(LibPaths.EncodeAppPath + "AesImprove.aspx");
            
        }

    }
}