using Area23.At.Framework.Library.Crypt.Hash;
using Newtonsoft.Json.Linq;
using System;

namespace Area23.At.Mono.Controls
{

    public partial class HashKeyRadioButtonList : System.Web.UI.UserControl
    {

        public string SelectedKeyHashValue { get => RadioButtonList_Hash.SelectedValue; set => RadioButtonList_Hash.SelectedValue = value; }

        public event EventHandler ParameterChanged_FireUp;            

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.RadioButtonList_Hash.SelectedValue = KeyHash.Hex.ToString();
            }
        }
        

        protected void RadioButtonList_Hash_ParameterChanged(object sender, EventArgs e)
        {
            if (ParameterChanged_FireUp != null) 
                ParameterChanged_FireUp.Invoke(sender, e);
            // base.Events.AddHandler(ParameterChangedFireUp, value);            
        }         

    }

}