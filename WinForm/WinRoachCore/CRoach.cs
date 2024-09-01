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
    public partial class CRoach : BRoach
    {
        
        public CRoach(int roachNumber)
        {
            roachNum = roachNumber;
            InitializeComponent();
            Name = "CRoach" + roachNum;
        }


        internal override void SetRoachBG(Point pt, Image desktopImage = null, bool changed = false)
        {
            if (roachCnt % 13 == 0)
                winDeskImg = GetDesktopImage(changed);
            else
                winDeskImg = (desktopImage != null) ? desktopImage : GetDesktopImage(changed);

            if (pt == Point.Empty || (pt.X <= 0 && pt.Y <= 0))
            {
                pt = new Point(((int)(winDeskImg.Width) - 64),
                    ((int)(winDeskImg.Height) - 64));
            }
            this.Location = pt;
            this.SetDesktopLocation(pt.X, pt.Y);
            

            Image bgImg = Crop(winDeskImg, 64, 64, pt.X - 1, pt.Y - 1);
            // Image bgImg = Crop(winDeskImg, DesktopBounds.Size.Width, DesktopBounds.Size.Height, f.DesktopBounds.Location.X, f.DesktopBounds.Location.Y);
            this.BackgroundImage = bgImg;
            
            if (roachCnt % 7 == 0)
            {
                if (roachCnt % 4 == 0)
                    this.panelCRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                else if (roachCnt % 4 == 1)
                    this.panelCRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach1;
                if (roachCnt % 4 == 2) 
                    this.panelCRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                else if (roachCnt % 4 == 3)
                    this.panelCRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach0;
            }
            
        }


        protected internal new void OnLoad(object sender, EventArgs e)
        {
            SelfMoveRoach(72);            
        }

    }
}
