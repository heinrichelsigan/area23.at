using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Static;
using Area23.At.WinForm.TWinFormCore.Gui.Forms;
using Area23.At.WinForm.TWinFormCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
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
            Program.tFormUniqueNames.Add(name);
            this.Name = name;
            this.labelBadge.Text = labelName;
            // this.Text = name;
            this.Name = name;
        }


        internal void TransparentBadge_Load(object sender, EventArgs e)
        {
            Point pt = this.DesktopLocation;
            Font badgeFont = new Font("Lucida Sans Unicode", 10F, FontStyle.Regular);

            System.Timers.Timer timer = new System.Timers.Timer { Interval = 1000 };
            System.Timers.Timer timerDispose = new System.Timers.Timer { Interval = 3300 };
            timer.Elapsed += (s, en) =>
            {
                this.Invoke(new Action(() =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (i % 3 == 0)
                            badgeFont = new Font("Lucida Sans Unicode", 10F, FontStyle.Regular);
                        if (i % 3 == 1)
                            badgeFont = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
                        if (i % 3 == 2)
                            badgeFont = new Font("Lucida Sans Unicode", 10F, FontStyle.Italic);
                        this.labelBadge.Font = badgeFont;

                        this.SetDesktopLocation(pt.X, pt.Y - (i * 2));
                        Thread.Sleep(200);
                    }
                }));
                timer.Stop(); // Stop the timer(otherwise keeps on calling)
            };

            timerDispose.Elapsed += (s, en) =>
            {
                this.Invoke(new Action(() =>
                {
                    if (this != null)
                    {
                        Close();
                        Dispose();
                    }
                }));
                timerDispose.Stop(); // Stop the timer(otherwise keeps on calling)
            };

            timer.Start(); // Starts the show autosave timer after 2,5 sec
            timerDispose.Start(); // Starts the DisposePictureMessage timer after 4sec
        }

        private void TransparentBadge_Shown(object sender, EventArgs e)
        {
            
        }

    }
}
