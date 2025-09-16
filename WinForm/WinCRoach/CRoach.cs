using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Win32Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Area23.At.WinForm.WinCRoach;
using System.Reflection;
using System.Diagnostics;
using Area23.At.Framework.Library.Static;

namespace Area23.At.WinForm.WinCRoach
{
    public partial class CRoach : Form
    {
        protected Image winDeskImg = null;
        volatile int speachCnt = 0;
        volatile bool locChangedOff = false;
        volatile int roachCnt = 0;
        long maxTicks = 0;
        object spinLock = new object(), innerLock = new object();
        DateTime roachStartDate = DateTime.Now;
        DateTime lastCapture = DateTime.Now;
        DateTime lastSay = DateTime.Now;
        int scrX = -1;
        int scrY = -1;

        internal string[] setences = {"Twenty", "Atou Marriage Fourty", "close down", "last beat winner", "Thank you and enough",
            "I change with Jack", "Last but not least", "Hey Mister", "Hey misses"};
        string[] schnapserlm = { "Und Zwanzig", "Vierzig", "Danke und genug hab I", "I drah zua", "Habeas tibi",
            "Tausch gegen den Buam aus", "Letzter fertzter", "Na oida" };

        public CRoach()
        {
            InitializeComponent();
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
            Screen screen = Screen.FromControl(this);

            if (pt == Point.Empty || (pt.X <= 0 && pt.Y <= 0))
            {
                int pX = screen.Bounds.Width;
                int pY = screen.Bounds.Height;
                foreach (Screen aScreen in Screen.AllScreens)
                    if (pX < aScreen.Bounds.Width)
                        pX = aScreen.Bounds.Width;
                pX -= 32;
                foreach (Screen aScreen in Screen.AllScreens)
                    if (pY < aScreen.Bounds.Height)
                        pY = aScreen.Bounds.Height;
                pY -= 32;
                pt = new Point(pX, pY);
            }
            this.Location = pt;
            this.SetDesktopLocation(pt.X, pt.Y);

            // Image bgImg = CaptureForm(this);
            // this.BackgroundImage = bgImg;

            if (roachCnt > 0)
            {
                if (roachCnt % 4 == 0)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinCRoach.Properties.Resources.CRoach;
                else if (roachCnt % 4 == 1)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinCRoach.Properties.Resources.CRoach1;
                if (roachCnt % 4 == 2)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinCRoach.Properties.Resources.CRoach;
                else if (roachCnt % 4 == 3)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinCRoach.Properties.Resources.CRoach0;
            }
        }


        private void OnLoad(object sender, EventArgs e)
        {
            SetRoachBG(this.Location);
            SelfMoveRoach(10);            
        }

        public void SelfMoveRoach(int interval = 0)
        {
            SetRoachBG(this.Location);
            // winDeskImg = GetDesktopImage(true);

            System.Timers.Timer tRoachMove = new System.Timers.Timer { Interval = 200 + interval };
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

        private void RoachMove()
        {
            while (true)
            {
                Form f = FindInternal();
                scrX = f.DesktopBounds.Location.X - 1;
                scrY = f.DesktopBounds.Location.Y - 1;

                scrX = DesktopLocation.X - 1;
                scrY = DesktopLocation.Y - 1;

                if (scrX <= 0)
                    scrX = winDeskImg.Width - 48;
                if (scrY <= 0)
                    scrY = winDeskImg.Height - 48;

                Point roachPosition = new Point(scrX, scrY);

                spinLock = new object();
                if (++roachCnt % 23 == 0)
                {
                    lock (spinLock)
                    {
                        DateTime t0 = DateTime.Now;
                        // if ((winDeskImg = GetDesktopImage(true)) != null)
                        //      System.AppDomain.CurrentDomain.SetData(Constants.ROACH_DESKTOP_WINDOW, winDeskImg);
                        DateTime t1 = DateTime.Now;
                        TimeSpan ts = t1.Subtract(t0);
                        if (ts.Ticks > maxTicks)
                        {
                            maxTicks = ts.Ticks;
                            Console.Error.WriteLine($"MaxTicks={maxTicks} \t ts: {ts.ToString()}");
                        }
                    }
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
                if (roachCnt >= 65535) {
                    RoachExit("RoachMove", new MouseEventArgs(MouseButtons.Left, 2, roachPosition.X + 2, roachPosition.Y + 2, 2));
                    break;
                }
                System.Threading.Thread.Sleep(128);
            }
        }

        public virtual void RotateSay()
        {
            spinLock = new object();
            lock (spinLock)
            {
                if (++speachCnt >= setences.Length)
                    speachCnt = 0;
                lastSay = DateTime.Now;
            }
            SaySpeach.Instance.Say(setences[speachCnt]);
        }

        internal Form FindInternal()
        {
            Control control = this;
            while (control != null && !(control is Form))
            {
                control = control.Parent;
            }

            return (Form)control;
        }


        internal virtual void RoachExit(object sender, MouseEventArgs e)
        {
            Process[] processes = Processes.GetRunningProcessesByName(Program.ProgName);
            if (processes != null && processes.Length > 0)
            {
                foreach (Process process in processes)
                {
                    System.Timers.Timer tProcKill = new System.Timers.Timer { Interval = 600 + process.Id };
                    tProcKill.Elapsed += (s, en) =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            Processes.KillProcessTree(process.Id, true, 0, true);
                        }));
                        tProcKill.Stop(); // Stop the timer(otherwise keeps on calling)
                    };
                    tProcKill.Start();
                }
            }
            AppExit(sender, e);
        }

        internal virtual void AppExit(object sender, EventArgs e)
        {
            string roachName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            MessageBox.Show($"Roach {Application.ProductName} is exiting now!", $"{roachName} roach exit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Program.ReleaseCloseDisposeMutex();
            Application.ExitThread();
            Application.Exit();
            Environment.Exit(0);            
        }

    }
}
