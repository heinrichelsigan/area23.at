using Area23.At.Framework.Library.Core;
using Area23.At.Framework.Library.Core.Win32Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.WinRoachCore
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
        volatile int roachNum = 0;
        CRoach cRoach;
        DRoach dRoach;
        ERoach eRoach;
        FRoach fRoach;            
        int scrX = -1;
        int scrY = -1;

        string[] setences = {"Twenty", "Atou Marriage Fourty", "close down", "last beat winner", "Thank you and enough",
            "I change with Jack", "Last but not least", "Hey Mister", "Hey misses"};

        string[] schnapserlm = { "Und Zwanzig", "Vierzig", "Danke und genug hab I", "I drah zua", "Habeas tibi",
            "Tausch gegen den Buam aus", "Letzter fertzter", "Na oida" };


        public RoachBase(int num)
        {
            roachNum = num;
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
                System.AppDomain.CurrentDomain.SetData("DesktopImage", winDeskImg);
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

        private void OnLoad(object sender, EventArgs e)
        {
            if (this.winDeskImg == null)
            {
                PersistDesktopImage();
            }

            startRoach(roachNum);
            //Point dPt = new Point((int)(winDeskImg.Width / 3), (int)((winDeskImg.Height) / 3));
            //if (cRoach == null)
            //{
            //    cRoach = new CRoach(0);
            //    cRoach.SetRoachBG(dPt, this.winDeskImg, true);
            //    cRoach.Show();
            //    // dRoach.SetDesktopLocation(dPt.X, dPt.Y);                    
            //}          
        }

        private void startRoach(int numRoach = 0)
        {
            Point dPt0 = new Point((int)(winDeskImg.Width / 3), (int)((winDeskImg.Height) / 3));
            Point dPt1 = new Point(((int)(winDeskImg.Width / 2) + 96), ((int)((winDeskImg.Height) / 2) + 48));
            Point dPt3 = new Point(((int)(winDeskImg.Width / 2) - 48), ((int)((winDeskImg.Height) / 2) - 24));
            Point dPt2 = new Point((int)((winDeskImg.Width / 3) * 2), (int)(((winDeskImg.Height) / 3) * 2));

            if (numRoach == 0)
            {
                System.Timers.Timer tLoad0 = new System.Timers.Timer { Interval = (100 + (numRoach * 400)) };
                tLoad0.Elapsed += (s, en) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        if (cRoach == null)
                        {
                            cRoach = new CRoach(numRoach);
                            cRoach.SetRoachBG(dPt0, this.winDeskImg, true);
                            cRoach.Show();
                            // PollDesktopImage();
                        }
                        Process.Start(new ProcessStartInfo
                        {
                            UseShellExecute = true,
                            WorkingDirectory = Environment.CurrentDirectory,
                            FileName = System.Windows.Forms.Application.ExecutablePath,
                            Arguments = (numRoach + 3).ToString()
                        });
                    }));
                    tLoad0.Stop(); // Stop the timer(otherwise keeps on calling)
                };
                tLoad0.Start();
            }
            //if (numRoach == 1)
            //{
            //    System.Timers.Timer tLoad1 = new System.Timers.Timer { Interval = (100 + (numRoach * 800)) };
            //    tLoad1.Elapsed += (s, en) =>
            //    {
            //        this.Invoke(new Action(() =>
            //        {
            //            if (dRoach == null)
            //            {
            //                dRoach = new DRoach(numRoach);
            //                dRoach.SetRoachBG(dPt0, this.winDeskImg, true);
            //                dRoach.Show();
            //                // PollDesktopImage();
            //            }
            //            Process.Start(new ProcessStartInfo
            //            {
            //                UseShellExecute = true,
            //                WorkingDirectory = Environment.CurrentDirectory,
            //                FileName = System.Windows.Forms.Application.ExecutablePath,
            //                Arguments = (++numRoach).ToString()
            //            });
            //        }));
            //        tLoad1.Stop(); // Stop the timer(otherwise keeps on calling)
            //    };
            //    tLoad1.Start();
            //}
            //if (numRoach == 2)
            //{
            //    System.Timers.Timer tLoad2 = new System.Timers.Timer { Interval = (100 + (numRoach * 1200)) };
            //    tLoad2.Elapsed += (s, en) =>
            //    {
            //        this.Invoke(new Action(() =>
            //        {
            //            if (eRoach == null)
            //            {
            //                eRoach = new ERoach(numRoach);
            //                eRoach.SetRoachBG(dPt0, this.winDeskImg, true);
            //                eRoach.Show();
            //                // PollDesktopImage();
            //            }
            //            Process.Start(new ProcessStartInfo
            //            {
            //                UseShellExecute = true,
            //                WorkingDirectory = Environment.CurrentDirectory,
            //                FileName = System.Windows.Forms.Application.ExecutablePath,
            //                Arguments = (++numRoach).ToString()
            //            });
            //        }));
            //        tLoad2.Stop(); // Stop the timer(otherwise keeps on calling)
            //    };
            //    tLoad2.Start();
            //}
            if (numRoach >= 3)
            {
                System.Timers.Timer tLoad3 = new System.Timers.Timer { Interval = (100 + (numRoach * 2000)) };
                tLoad3.Elapsed += (s, en) =>
                {
                    this.Invoke(new Action(() =>
                    {
                        if (fRoach == null)
                        {
                            fRoach = new FRoach(numRoach);
                            fRoach.SetRoachBG(dPt3, this.winDeskImg, true);
                            fRoach.Show();
                        }
                        // PollDesktopImage();
                    }));
                    tLoad3.Stop(); // Stop the timer(otherwise keeps on calling)
                };
                tLoad3.Start();
            }
            
        }

        //private void PollDesktopImage()
        //{
        //    while(true)
        //    {
        //        // PersistDesktopImage();
        //        RoachMove();                
        //        System.Threading.Thread.Sleep(192);
        //    }
        //}

        //private void RoachMove()
        //{
        //    Form f = cRoach.FindForm();
        //    scrX = f.DesktopBounds.Location.X - 1;
        //    scrY = f.DesktopBounds.Location.Y - 1;

        //    scrX = cRoach.DesktopLocation.X - 1;
        //    scrY = cRoach.DesktopLocation.Y - 1;            
                       
        //    if (scrX < 0)
        //        scrX = winDeskImg.Width - 64;
        //    if (scrY < 0)
        //        scrY = winDeskImg.Height - 64;

        //    Point roachPosition = new Point(scrX, scrY);

        //    spinLock = new object();
        //    if (++roachCnt % 23 == 0)
        //    {
        //        lock (spinLock)
        //        {
        //            DateTime t0 = DateTime.Now;
        //            cRoach.Visible = false;
        //            winDeskImg = GetDesktopImage();
        //            cRoach.Visible = true;
        //            DateTime t1 = DateTime.Now;
        //            TimeSpan ts = t1.Subtract(t0);
        //            if (ts.Ticks > maxTicks)
        //            {
        //                maxTicks = ts.Ticks;
        //                Console.Error.WriteLine($"MaxTicks={maxTicks} \t ts: {ts.ToString()}");
        //            }
        //        }
        //        cRoach.SetRoachBG(roachPosition, winDeskImg, true);
        //        cRoach.BringToFront();
        //        // RotateSay();
        //    }
        //    else
        //    {
        //        cRoach.SetRoachBG(roachPosition);
        //        if (roachCnt % 11 == 1)
        //            cRoach.Show();
        //    }

          
        //    // System.Threading.Thread.Sleep(333);
        //}

    }
}
