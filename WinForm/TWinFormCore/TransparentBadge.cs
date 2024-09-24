using Area23.At.Framework.Library.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.TWinFormCore
{
    public partial class TransparentBadge : Form
    {

        public string TFormType
        {
            get => this.GetType().ToString();
        }

        public TransparentBadge()
        {
            InitializeComponent();
        }

        public TransparentBadge(string labelName) : this()
        {            
            string name = DateTime.Now.Area23DateTimeWithMillis();
            Program.tFormUniqueNames.Add(name);
            this.Name = name;
            this.labelBadge.Text = labelName;
            // this.Text = name;
            this.Name = name;
        }

    }
}
