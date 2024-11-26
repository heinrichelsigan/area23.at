using Area23.At.Framework.Library;
using System;
using System.Windows.Forms;

namespace Area23.At.WinForm.TransparentForms.TForms
{
    public partial class TBadge : Form
    {
        public TBadge()
        {
            InitializeComponent();
        }

        public TBadge(string labelName) : this()
        {
            string name = DateTime.Now.Area23DateTimeWithMillis();
            this.Name = name;
            this.labelBadge.Text = labelName;
            // this.Text = name;
            this.Name = name;
        }
    }
}
