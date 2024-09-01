using Area23.At.Framework.Library.Core;
using Area23.At.Framework.Library.Core.Win32Api;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.WinRoachCore
{
    public class CRoach : BRoach
    {
        private System.ComponentModel.IContainer components = null;

        public CRoach(int roachNumber)
        {
            roachNum = roachNumber;
            InitializeComponent();
            Name = "CRoach" + roachNum;
        }


        internal override void SetRoachBG(Point pt)
        {
            Screen screen = Screen.FromControl(this);

            if (pt == Point.Empty || (pt.X <= 0 && pt.Y <= 0))
            {
                pt = new Point(((int)(screen.Bounds.Width) - 64),
                    ((int)(screen.Bounds.Height) - 64));
            }
            this.Location = pt;
            this.SetDesktopLocation(pt.X, pt.Y);

            Image bgImg = CaptureForm(this);            
            // this.BackgroundImage = bgImg;

            if (roachCnt % 7 == 0)
            {
                if (roachCnt % 4 == 0)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                else if (roachCnt % 4 == 1)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach1;
                if (roachCnt % 4 == 2)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                else if (roachCnt % 4 == 3)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach0;
            }

        }


        protected internal override void OnLoad(object sender, EventArgs e)
        {
            SelfMoveRoach(72);
        }


        protected internal override void SelfMoveRoach(int interval = 0)
        {
            SetRoachBG(this.Location);

            System.Timers.Timer tRoachMove = new System.Timers.Timer { Interval = interval + (interval * roachNum) };
            tRoachMove.Elapsed += (s, en) =>
            {
                this.Invoke(new Action(() =>
                {
                    RoachMove();
                }));
                tRoachMove.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            tRoachMove.Start();
        }
       
    }

}
