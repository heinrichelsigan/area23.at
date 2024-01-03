using area23.at.mono.test.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace area23.at.mono.test
{
    public partial class FortunAsp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetFortune();
        }

        protected void ButtonHidden_Click(object sender, EventArgs e)
        {
            SetFortune();
        }

        protected void SetFortune()
        {
            if (Constants.FortuneBool) LiteralFortune.Text = ExecFortune(false);
            else PreFortune.InnerText = ExecFortune(true);
        }

        protected string ExecFortune(bool longFortune = true)
        {
            return (longFortune) ?
                ProcessCmd.Execute("/usr/games/fortune", " -a -l ") :
                ProcessCmd.Execute("/usr/games/fortune", "-o -s  ");
        }
    }
}