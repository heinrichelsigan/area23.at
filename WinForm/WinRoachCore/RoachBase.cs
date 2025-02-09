using Area23.At.Framework.Core;
using Area23.At.Framework.Core.Win32Api;
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
        volatile int roachCnt = 0;
        volatile bool locChangedOff = false, startedRoach = false;
        object spinLockImage = new object(), spinLock = new object();
        DateTime lastCapture = DateTime.Now;
        int scrX = -1;
        int scrY = -1;
        volatile int roachNum = 0;
        CRoach cRoach;
        DRoach dRoach;
        ERoach eRoach;
        MRoach mRoach;
        Process? procStarted;

        public RoachBase(int num)
        {
            roachNum = num;
            cRoach = null;
            dRoach = null;
            eRoach = null;
            mRoach = null;
            InitializeComponent();
        }


        public Image PersistDesktopImage()
        {
            if (winDeskImg == null)
            {
                spinLockImage = new object();
                lock (spinLockImage)
                {
                    winDeskImg = (Image)System.AppDomain.CurrentDomain.GetData(Constants.ROACH_DESKTOP_WINDOW);
                    if (winDeskImg == null)
                    {
                        spinLock = new object();
                        lock (spinLock)
                        {
                            this.WindowState = FormWindowState.Minimized;
                            winDeskImg = ScreenCapture.CaptureAllDesktops();
                            if (winDeskImg != null)
                                System.AppDomain.CurrentDomain.SetData(Constants.ROACH_DESKTOP_WINDOW, winDeskImg);
                            lastCapture = DateTime.Now;
                        }
                    }
                }
            }

            return winDeskImg;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            if (winDeskImg == null)
            {
                PersistDesktopImage();
            }

            if ((roachNum == 0 && cRoach == null) || (roachNum == 1 && dRoach == null) || (roachNum == 2 && eRoach == null) || (roachNum == 3 && mRoach == null))
            {
                StartRoach(roachNum);
            }

        }

        private void StartRoach(int numRoach = 0)
        {
            Point dPt = Point.Empty;

            if (procStarted != null && !procStarted.HasExited)
            {
                ;
            }
            else if ((numRoach < 4) && (procStarted == null || procStarted.HasExited))
            {
                procStarted = Process.Start(new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = System.Windows.Forms.Application.ExecutablePath,
                    Arguments = (numRoach + 1).ToString()
                });
            }

            System.Timers.Timer tLoad = new System.Timers.Timer { Interval = (250 + (numRoach * 250)) };
            tLoad.Elapsed += (s, en) =>
            {
                this.Invoke(new Action(() =>
                {
                    if (numRoach == 0 && cRoach == null)
                    {
                        dPt = new Point((int)(winDeskImg.Width / 3), (int)((winDeskImg.Height) / 3));
                        cRoach = new CRoach(numRoach);
                        // cRoach.TopMost = true;
                        cRoach.SetRoachBG(dPt);
                        cRoach.Show();
                    }
                    if (numRoach == 1 && dRoach == null)
                    {
                        dPt = new Point(((int)(winDeskImg.Width / 2) + 96), ((int)((winDeskImg.Height) / 2) + 48));
                        dRoach = new DRoach(numRoach);
                        dRoach.SetRoachBG(dPt);
                        dRoach.Show();
                    }
                    if (numRoach == 2 && eRoach == null)
                    {
                        dPt = new Point(((int)(winDeskImg.Width - 192)), ((int)(winDeskImg.Height - 128)));
                        eRoach = new ERoach(numRoach);
                        eRoach.SetRoachBG(dPt);
                        eRoach.Show();
                    }
                    if (numRoach == 3 && mRoach == null)
                    {
                        dPt = new Point(((int)(winDeskImg.Width - 96)), ((int)(winDeskImg.Height - 48)));
                        mRoach = new MRoach(numRoach);
                        mRoach.SetRoachBG(dPt);
                        mRoach.Show();
                    }
                    // PollDesktopImage();
                }));
                tLoad.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            tLoad.Start();
        }

    }

}
