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
    public partial class BRoach : Form
    {
        protected internal Image winDeskImg = null;
        protected internal Rectangle roachPosRect = new Rectangle();
        protected internal Point roachPosition = Point.Empty;
        internal volatile bool roachInit = false;
        internal volatile int cX = -1, cY = -1, roachCnt = 0, roachNum = 0;
        protected internal long maxTicks = 0;
        protected internal object lock0 = new object(), lock1 = new object(), lock2 = new object(), lock3 = new object();
        protected internal DateTime lastCapture = DateTime.Now;


        public BRoach()
        {
            roachNum = 0;
            InitializeComponent();
            Name = "BRoach" + roachNum;            
        }

        internal virtual Image GetDesktopImage(bool everCapture = false)
        {
            if (winDeskImg == null || everCapture)
            {
                lock0 = new object();
                lock (lock0)
                {
                    winDeskImg = (Image)System.AppDomain.CurrentDomain.GetData(Constants.ROACH_DESKTOP_WINDOW);

                    if (winDeskImg == null || everCapture)
                    {
                        this.Visible = false;
                        lock1 = new object();
                        lock (lock1)
                        {
                            winDeskImg = ScreenCapture.CaptureAllDesktops();
                            if (winDeskImg != null)
                                System.AppDomain.CurrentDomain.SetData(Constants.ROACH_DESKTOP_WINDOW, winDeskImg);
                            lastCapture = DateTime.Now;
                        }
                        this.Visible = true;
                    }
                }
            }
            
            return winDeskImg;            
        }

        internal virtual Image Crop(Image image, int width, int height, int x, int y)
        {
            try
            {
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(image, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
                g.Dispose();

                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        internal virtual void SetRoachBG(Point pt, Image desktopImage = null, bool changed = false)
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
            this.BackgroundImage = bgImg;
            
            if (roachCnt % 7 == 0)
            {
                if (roachCnt % 4 == 0)
                    this.panelBRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                else if (roachCnt % 4 == 1)
                    this.panelBRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach1;
                if (roachCnt % 4 == 2)
                    this.panelBRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                else if (roachCnt % 4 == 3)
                    this.panelBRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach0;
            }            
        }


        protected internal virtual void OnLoad(object sender, EventArgs e)
        {
            if (!roachInit)
            {
                SelfMoveRoach(144);
            }
        }


        protected internal virtual void OnShow(object sender, EventArgs e)
        {
            SetRoachBG(this.Location);
        }

        protected internal virtual void SelfMoveRoach(int interval = 0)
        {
            SetRoachBG(this.Location, winDeskImg, true);

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

        protected internal virtual void RoachMove()
        {
            lock2 = new object();
            lock(lock2)
            {
                roachInit = true;
            }

            Form f = FindInternal();
            roachPosRect = f.DesktopBounds;

            for (roachCnt = 0; roachCnt < 16384; roachCnt++)
            {
                roachPosition = DesktopLocation;
                cX = roachPosition.X - 1;
                cY = roachPosition.Y - 1;                

                if (cX <= 0 || cY <= 0)
                {
                    lock3 = new object();
                    lock (lock3)
                    {
                        f = FindInternal();
                        roachPosRect = f.DesktopBounds;                        

                        if (cX <= 0)
                            cX = winDeskImg.Width - 64;
                        if (cY <= 0)
                            cY = winDeskImg.Height - 64;
                    }
                }

                roachPosition = new Point(cX, cY);

                lock2 = new object();
                lock (lock2)
                {
                    DateTime t0 = DateTime.Now;
                    if (t0.Second >= 59 && t0.Microsecond >= 250)
                    {
                        lock3 = new object();
                        lock (lock3)
                        {
                            winDeskImg = GetDesktopImage(true);
                        }
                        DateTime t1 = DateTime.Now;
                        TimeSpan ts = t1.Subtract(t0);
                        if (ts.Ticks > maxTicks)
                        {
                            maxTicks = ts.Ticks;
                            Console.Error.WriteLine($"MaxTicks={maxTicks} \t ts: {ts.ToString()}");
                        }
                        SetRoachBG(roachPosition, winDeskImg, true);
                        BringToFront();
                    }
                    else
                    {
                        SetRoachBG(roachPosition, winDeskImg, false);
                        if (roachCnt % 11 == 1)
                            Show();
                    }
                }

                System.Threading.Thread.Sleep(125);
            }

            AppExit("RoachMove", new EventArgs());
        }

        
        protected internal virtual Form FindInternal()
        {
            Control control = this;
            while (control != null && !(control is Form))
            {
                control = control.Parent;
            }
            if (control is Form) 
            { 
                if (((Form)control).Name.Contains("Roach", StringComparison.InvariantCultureIgnoreCase))
                {
                    return (Form)control;
                }
            }
            return (Form)Form.ActiveForm;            
        }

        protected internal virtual void RoachExit(object sender, MouseEventArgs e)
        {
            string procRoachName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            System.Diagnostics.Process[] processes = Area23.At.Framework.Library.Core.Win32Api.Processes.GetRunningProcessesByName(procRoachName);
            if (processes != null && processes.Length > 0)
            {
                foreach (System.Diagnostics.Process process in processes)
                {
                    System.Timers.Timer tProcKill = new System.Timers.Timer { Interval = 600 + process.Id };
                    tProcKill.Elapsed += (s, en) =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            Area23.At.Framework.Library.Core.Win32Api.Processes.KillProcessTree(process.Id, true, 0, true);
                        }));
                        tProcKill.Stop(); // Stop the timer(otherwise keeps on calling)
                    };
                    tProcKill.Start();
                }
            }
            AppExit(sender, e);
        }

        protected internal virtual void AppExit(object sender, EventArgs e)
        {
            string orocRoachName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);           
            MessageBox.Show($"Roach {orocRoachName} is exiting now!", $"{orocRoachName} roach exit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

    }
}
