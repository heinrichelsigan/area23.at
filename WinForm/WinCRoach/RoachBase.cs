using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Static;
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

namespace Area23.At.WinForm.WinCRoach
{
    public partial class RoachBase : Form
    {
        protected Image winDeskImg = null;
        long maxTicks = 0;
        volatile int speachCnt = 0;
        volatile int roachCnt = 0;
        volatile bool locChangedOff = false, startedRoach = false;
        object spinLockImage = new object(), spinLock = new object();
        DateTime lastCapture = DateTime.Now;
        DateTime lastSay = DateTime.Now;
        CRoach cRoach, dRoach;
        int scrX = -1;
        int scrY = -1;

        string[] setences = {"Twenty", "Atou Marriage Fourty", "close down", "last beat winner", "Thank you and enough",
            "I change with Jack", "Last but not least", "Hey Mister", "Hey misses"};

        string[] schnapserlm = { "Und Zwanzig", "Vierzig", "Danke und genug hab I", "I drah zua", "Habeas tibi",
            "Tausch gegen den Buam aus", "Letzter fertzter", "Na oida" };


        public RoachBase()
        {
            InitializeComponent();
        }

        public Image GetDesktopImage()
        {
            locChangedOff = true;
            Image winDesktopImg;

            spinLockImage = new object();            
            lock (spinLockImage)
            {
                this.WindowState = FormWindowState.Minimized;
                winDesktopImg = ScreenCapture.CaptureAllDesktops();
                lastCapture = DateTime.Now;
                locChangedOff = false;
            }
            return winDesktopImg;
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

        public void PersistDesktopImage()
        {
            winDeskImg = GetDesktopImage();
            if (winDeskImg != null) 
                System.AppDomain.CurrentDomain.SetData(Constants.ROACH_DESKTOP_WINDOW, winDeskImg);
        }

        public void RotateSay()
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

        private void OnLoad(object sender, EventArgs e)
        {
            if (this.winDeskImg == null)
            {
                PersistDesktopImage();
            }

            Point dPt = new Point((int)(winDeskImg.Width / 2), (int)(winDeskImg.Height / 2));
            if (cRoach == null)
            {
                cRoach = new CRoach();
                cRoach.SetRoachBG(dPt, this.winDeskImg, true);
                // dRoach.SelfMoveRoach(400);
                cRoach.Show();                                  
            }

            // startRoach();

            //System.Timers.Timer tLoad0 = new System.Timers.Timer { Interval = 450 };
            //tLoad0.Elapsed += (s, en) =>
            //{
            //    this.Invoke(new Action(() =>
            //    {
            //        if (dRoach == null)
            //        {
            //            dRoach = new CRoach();
            //            // dRoach.SelfMoveRoach(400);
            //            dRoach.ShowDialog();
            //            // dRoach.SetDesktopLocation(dPt.X, dPt.Y);                    
            //        }
            //        //if (eRoach == null)
            //        //{
            //        //    Point ePt = new Point(dPt.X + 128, dPt.Y + 126);
            //        //    eRoach = new CRoach() { Location = ePt };
            //        //    // eRoach.SetDesktopLocation(ePt.X, ePt.Y);
            //        //    eRoach.SelfMoveRoach(200);
            //        //    eRoach.Show();
            //        //    eRoach.SetDesktopLocation(ePt.X, ePt.Y);
            //        //}

            //    }));
            //    tLoad0.Stop(); // Stop the timer(otherwise keeps on calling)
            //};
            //tLoad0.Start();


            //System.Timers.Timer tLoad3 = new System.Timers.Timer { Interval = 750 };
            //tLoad3.Elapsed += (s, en) =>
            //{
            //    this.Invoke(new Action(() =>
            //    {
            //        startRoach();
            //    }));
            //    tLoad3.Stop(); // Stop the timer(otherwise keeps on calling)
            //};
            //tLoad3.Start();           
        }

        private void startRoach()
        {
            Point dPt = new Point(((int)(winDeskImg.Width / 2) + 32), ((int)((winDeskImg.Height) / 2) + 24));
            System.Timers.Timer tLoad0 = new System.Timers.Timer { Interval = 125 };
            tLoad0.Elapsed += (s, en) =>
            {
                this.Invoke(new Action(() =>
                {
                    if (cRoach == null)
                    {
                        cRoach = new CRoach() { Location = dPt };
                        // cRoach.SelfMoveRoach(100);
                        cRoach.Show();
                        PollDesktopImage();
                    }

                }));
                tLoad0.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            tLoad0.Start();

        }

        private void PollDesktopImage()
        {
            while(true)
            {
                // PersistDesktopImage();
                RoachMove();                
                System.Threading.Thread.Sleep(192);
            }
        }

        private void RoachMove()
        {
            Form f = cRoach.FindInternal();
            scrX = f.DesktopBounds.Location.X - 1;
            scrY = f.DesktopBounds.Location.Y - 1;

            scrX = cRoach.DesktopLocation.X - 1;
            scrY = cRoach.DesktopLocation.Y - 1;            
                       
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
                    cRoach.Visible = false;
                    winDeskImg = GetDesktopImage();
                    cRoach.Visible = true;
                    DateTime t1 = DateTime.Now;
                    TimeSpan ts = t1.Subtract(t0);
                    if (ts.Ticks > maxTicks)
                    {
                        maxTicks = ts.Ticks;
                        Console.Error.WriteLine($"MaxTicks={maxTicks} \t ts: {ts.ToString()}");
                    }
                }
                cRoach.SetRoachBG(roachPosition, winDeskImg, true);
                cRoach.BringToFront();
                RotateSay();
            }
            else
            {
                cRoach.SetRoachBG(roachPosition);
                if (roachCnt % 11 == 1)
                    cRoach.Show();
            }

          
            // System.Threading.Thread.Sleep(333);
        }

    }
}
