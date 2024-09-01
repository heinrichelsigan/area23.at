using Area23.At.Framework.Library.Core;
using Area23.At.Framework.Library.Core.Win32Api;
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
    public partial class DRoach : BRoach
    {

        internal string[] setences = {"Twenty", "Atou Marriage Fourty", "close down", "last beat winner", "Thank you and enough",
            "I change with Jack", "Last but not least", "Hey Mister", "Hey misses"};
        volatile int speachCnt = 0;
        DateTime lastSay = DateTime.Now;

        public DRoach(int roachNumber)
        {
            roachNum = roachNumber;
            InitializeComponent();
            Name = "DRoach" + roachNum;
        }

        internal override void SetRoachBG(Point pt, Image desktopImage = null, bool changed = false)
        {
            System.Timers.Timer t0D = new System.Timers.Timer { Interval = 20 };
            t0D.Elapsed += (s, en) =>
            {
                this.Invoke(new Action(() =>
                {
                    winDeskImg = (desktopImage != null) ? desktopImage : GetDesktopImage(changed);

                    if (pt == Point.Empty || (pt.X <= 0 && pt.Y <= 0))
                    {
                        pt = new Point(((int)(winDeskImg.Width) - 64),
                            ((int)(winDeskImg.Height) - 64));
                    }
                    this.Location = pt;
                    this.SetDesktopLocation(pt.X, pt.Y);

                    Image bgImg = Crop(winDeskImg, 64, 64, pt.X - 1, pt.Y - 1);                    
                    this.BackgroundImage = bgImg;

                    if (roachCnt % 43 == 0)
                    {                        
                        this.panelDRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.DRoach;
                        // RotateSay();
                    }

                }));
                t0D.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            t0D.Start();            
        }


        protected internal override void OnLoad(object sender, EventArgs e)
        {
            SelfMoveRoach(18);
        }



        internal virtual void RotateSay()
        {
            lock0 = new object();
            lock (lock0)
            {
                if (++speachCnt >= setences.Length)
                        speachCnt = 0;
                    lastSay = DateTime.Now;
            }
            SaySpeachCore.Instance.Say(setences[speachCnt]);
        }

    }
}
