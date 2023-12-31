﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Area23.At.Framework.ScreenCapture;

namespace Area23.At.WinForm.WinRoachCore
{
    public partial class CRoach : Form
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
                    winDeskImg = (Image)System.AppDomain.CurrentDomain.GetData("DesktopImage");

                    if (winDeskImg == null || everCapture)
                    {
                        this.Visible = false;
                        innerLock = new object();
                        lock (innerLock)
                        {
                            ScreenCapture sc = new ScreenCapture();
                            winDeskImg = sc.CaptureScreen();
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
            winDeskImg = (desktopImage != null && changed) ? desktopImage : GetDesktopImage();

            if (pt == Point.Empty || (pt.X == 0 && pt.Y == 0))
            {
                pt = new Point(((int)(winDeskImg.Width / 2) - 32),
                    ((int)(winDeskImg.Height / 2) - 32));
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
                if (roachCnt % 3 == 0)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                else if (roachCnt % 3 == 1)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach1;
                else if (roachCnt % 3 == 2)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach0;
            }
            if (roachCnt > 65536 * 16) roachCnt = 0;
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
                Form f = FindForm();
                scrX = f.DesktopBounds.Location.X - 1;
                scrY = f.DesktopBounds.Location.Y - 1;

                scrX = DesktopLocation.X - 1;
                scrY = DesktopLocation.Y - 1;

                if (scrX < 0)
                    scrX = winDeskImg.Width - 48;
                if (scrY < 0)
                    scrY = winDeskImg.Height - 48;

                Point roachPosition = new Point(scrX, scrY);

                spinLock = new object();
                if (++roachCnt % 23 == 0)
                {
                    lock (spinLock)
                    {
                        DateTime t0 = DateTime.Now;
                        winDeskImg = GetDesktopImage(true);
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
                    RotateSay();
                }
                else
                {
                    SetRoachBG(roachPosition);
                    if (roachCnt % 11 == 1)
                        Show();
                }
                if (roachCnt > 65535) {
                    AppExit("RoachMove", new EventArgs());
                    break;
                }
                System.Threading.Thread.Sleep(200);
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
            SaySpeachCore.Instance.Say(setences[speachCnt]);
        }

        private void RoachExit(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void AppExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
