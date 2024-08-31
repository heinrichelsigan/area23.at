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
    public partial class DRoach : Form
    {
        internal string[] setences = {"Twenty", "Atou Marriage Fourty", "close down", "last beat winner", "Thank you and enough",
            "I change with Jack", "Last but not least", "Hey Mister", "Hey misses"};

        internal string[] schnapserlm = { "Und Zwanzig", "Vierzig", "Danke und genug hab I", "I drah zua", "Habeas tibi",
            "Tausch gegen den Buam aus", "Letzter fertzter", "Na oida" };

        protected Image winDeskImg = null;
        volatile int speachCnt = 0;
        volatile bool locChangedOff = false;
        volatile int roachCnt = 0;
        long maxTicks = 0;
        object spinLock = new object(), innerLock = new object();
        DateTime lastCapture = DateTime.Now;
        DateTime lastSay = DateTime.Now;
        int scrX = -1;
        int scrY = -1;

        public DRoach(int roachNum)
        {
            InitializeComponent();
            Name = "DRoach" + roachNum;
            panelDRoach.Name = "panelDRoach" + roachNum;            
        }

        public Image GetDesktopImage(bool everCapture = false)
        {
            if (winDeskImg == null || everCapture)
            {
                spinLock = new object();
                lock (spinLock)
                {
                    winDeskImg = (Image)System.AppDomain.CurrentDomain.GetData(Constants.ROACH_DESKTOP_WINDOW);

                    if (winDeskImg == null || everCapture)
                    {
                        this.Visible = false;
                        innerLock = new object();
                        lock (innerLock)
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

        public Image Crop(Image image, int width, int height, int x, int y)
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

        public void SetRoachBG(Point pt, Image desktopImage = null, bool changed = false)
        {
            System.Timers.Timer t0D = new System.Timers.Timer { Interval = 20 };
            t0D.Elapsed += (s, en) =>
            {
                this.Invoke(new Action(() =>
                {
                    winDeskImg = (desktopImage != null && changed) ? desktopImage : GetDesktopImage();

                    if (pt == Point.Empty || (pt.X <= 0 && pt.Y <= 0))
                    {
                        pt = new Point(((int)(winDeskImg.Width) - 64),
                            ((int)(winDeskImg.Height) - 64));
                    }
                    this.Location = pt;
                    this.SetDesktopLocation(pt.X, pt.Y);

                    Graphics g = Graphics.FromImage(winDeskImg);
                    // Form f = this.FindForm();
                    Image bgImg = Crop(winDeskImg, 64, 64, pt.X - 1, pt.Y - 1);
                    // Image bgImg = Crop(winDeskImg, DesktopBounds.Size.Width, DesktopBounds.Size.Height, f.DesktopBounds.Location.X, f.DesktopBounds.Location.Y);
                    this.BackgroundImage = bgImg;
                    roachCnt++;
                    if (roachCnt % 7 == 0)
                    {
                        if (roachCnt % 4 == 0)
                            this.panelDRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                        else if (roachCnt % 4 == 1)
                            this.panelDRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach1;
                        if (roachCnt % 4 == 2)
                            this.panelDRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                        else if (roachCnt % 4 == 3)
                            this.panelDRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach0;
                    }
                    if (roachCnt > 65536 * 16) roachCnt = 0;
                }));
                t0D.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            t0D.Start();            
        }


        private void OnLoad(object sender, EventArgs e)
        {
            SetRoachBG(this.Location);
            SelfMoveRoach(10);
        }

        public void SelfMoveRoach(int interval = 0)
        {
            SetRoachBG(this.Location);
            winDeskImg = GetDesktopImage(true);

            System.Timers.Timer tD1 = new System.Timers.Timer { Interval = 40 + interval };
            tD1.Elapsed += (s, en) =>
            {
                this.Invoke(new Action(() =>
                {
                    RoachMove();
                }));
                tD1.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            tD1.Start();
        }

        private void RoachMove()
        {
            while (true)
            {
                System.Timers.Timer tD2 = new System.Timers.Timer { Interval = 80 };
                tD2.Elapsed += (s, en) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        Form f = FindInternal();
                        scrX = f.DesktopBounds.Location.X - 1;
                        scrY = f.DesktopBounds.Location.Y - 1;

                        scrX = DesktopLocation.X - 1;
                        scrY = DesktopLocation.Y - 1;

                        if (scrX <= 0)
                            scrX = winDeskImg.Width - 64;
                        if (scrY <= 0)
                            scrY = winDeskImg.Height - 64;

                        Point roachPosition = new Point(scrX, scrY);

                        // spinLock = new object();
                        if (++roachCnt % 23 == 0)
                        {
                            // lock (spinLock)
                            // {
                            DateTime t0 = DateTime.Now;
                                winDeskImg = GetDesktopImage(true);
                                DateTime t1 = DateTime.Now;
                                TimeSpan ts = t1.Subtract(t0);
                                if (ts.Ticks > maxTicks)
                                {
                                    maxTicks = ts.Ticks;
                                    Console.Error.WriteLine($"MaxTicks={maxTicks} \t ts: {ts.ToString()}");
                                }
                            // }
                            SetRoachBG(roachPosition, winDeskImg, true);
                            BringToFront();
                            // RotateSay();
                        }
                        else
                        {
                            SetRoachBG(roachPosition);
                            if (roachCnt % 11 == 1)
                                Show();
                        }
                        if (roachCnt > 65535)
                        {
                            AppExit("RoachMove", new EventArgs());
                            // break;
                        }
                        System.Threading.Thread.Sleep(144);
                    }));
                    tD2.Stop(); // Stop the timer(otherwise keeps on calling)
                };
                tD2.Start();
            }
        }

        public virtual void RotateSay()
        {
            // spinLock = new object();
            // lock (spinLock)
            // {
            if (++speachCnt >= setences.Length)
                    speachCnt = 0;
                lastSay = DateTime.Now;
            // }
            SaySpeachCore.Instance.Say(setences[speachCnt]);
        }

        internal Form FindInternal()
        {
            Control control = this;
            while (control != null && !(control is Form))
            {
                control = control.Parent;
            }
            if (control is Form) 
            { 
                if (((Form)control).Name.StartsWith("DRoach"))
                {
                    return (Form)control;
                }
            }
            return (Form)Form.ActiveForm;            
        }

        private void RoachExit(object sender, MouseEventArgs e)
        {
            AppExit(sender, e);
        }

        private void AppExit(object sender, EventArgs e)
        {
            string roachName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            MessageBox.Show($"Roach {Application.ProductName} is exiting now!", $"{roachName} roach exit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

    }
}
