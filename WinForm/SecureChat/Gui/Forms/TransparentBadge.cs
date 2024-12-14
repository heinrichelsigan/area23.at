﻿using Area23.At.Framework.Library.Core;
using Area23.At.WinForm.SecureChat.Gui.Forms;
using Area23.At.WinForm.SecureChat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    public partial class TransparentBadge : System.Windows.Forms.Form
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
            this.Name = name;
            this.labelBadge.Text = labelName;
            // this.Text = name;
            this.Name = name;
        }

    }
}
