using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using System;
using System.Web.UI.HtmlControls;

namespace Area23.At.Mono.Controls
{

    public partial class HashKeyRadioButtonList : System.Web.UI.UserControl
    {
        public EventHandler RadioButtonList_Hash_ParameterChanged_FireUp { protected get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.RadioButtonList_Hash.SelectedValue = KeyHash.Hex.ToString();
            }
        }
        

        protected void RadioButtonList_Hash_ParameterChanged(object sender, EventArgs e)
        {
            if (RadioButtonList_Hash_ParameterChanged_FireUp != null)
            {
                RadioButtonList_Hash_ParameterChanged_FireUp.Invoke(this, e);
            }
        }

    }

}