using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace area23.at.mono.rpncalc
{
    public partial class Calculator : Area23BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void checkBoxRpnCalc_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void rpnCalc_Click(object sender, EventArgs e)
        {
            object o1 = sender;
            string eName = e.GetType().Name;
                
        }

        protected void bMath_Click(object sender, EventArgs e)
        {
            string mathString = (sender is Button) ? ((Button)sender).Text : "";
            this.textBoxbResult.Text += mathString.ToString();
        }

        protected void bSin_Click(object sender, EventArgs e)
        {

        }

        protected void BClear_Click(object sender, EventArgs e)
        {
            this.textBoxbResult.Text = "";
        }

        protected void Bdel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxbResult.Text) && textBoxbResult.Text.Length > 0)
            {
                this.textBoxbResult.Text = textBoxbResult.Text.Substring(0, textBoxbResult.Text.Length - 1);
            }
        }
    }
}