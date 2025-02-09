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
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.WinRoachCore
{
    public partial class ERoach : Form
    {
        internal Image winDeskImg = null;
        protected Rectangle roachPosRect = new Rectangle();
        protected Point roachPosition = Point.Empty;
        internal volatile bool roachInit = false;
        internal volatile int cX = -1, cY = -1, roachCnt = 0, roachNum = 0;
        protected long maxTicks = 0;
        protected object lock0 = new object(), lock1 = new object(), lock2 = new object(), lock3 = new object();
        protected DateTime lastCapture = DateTime.Now;

        internal string[] setences = {"Twenty", "Atou Marriage Fourty", "close down", "last beat winner", "Thank you and enough",
            "I change with Jack", "Last but not least", "Hey Mister", "Hey misses"};
        volatile int speachCnt = 0;
        DateTime lastSay = DateTime.Now;

        public ERoach(int roachNumber)
        {
            roachNum = roachNumber;
            InitializeComponent();
            Name = "ERoach" + roachNum;
            Text = Name;
            this.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.ERoach;
        }


        internal virtual void SetRoachBG(Point pt)
        {
            //System.Timers.Timer t0D = new System.Timers.Timer { Interval = 20 };
            //t0D.Elapsed += (s, en) =>
            //{
            //    this.Invoke(new Action(() =>
            //    {

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


            if (roachCnt % 32 == 2)
            {
                this.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.DRoach;
                // this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.DRoach;           
            }

            // Image bgImg = ScreenCapture.CaptureWindow(this.Handle);
            // this.BackgroundImage = bgImg;

            //    }));
            //    t0D.Stop(); // Stop the timer(otherwise keeps on calling)
            //};
            //t0D.Start();            
        }


        internal virtual void OnLoad(object sender, EventArgs e)
        {
            SelfMoveRoach(18);
        }


        protected virtual void OnShow(object sender, EventArgs e)
        {
            SetRoachBG(this.Location);
        }

        protected virtual void SelfMoveRoach(int interval = 0)
        {
            SetRoachBG(this.Location);

            System.Timers.Timer tRoachMove = new System.Timers.Timer { Interval = interval + (interval * roachNum) };
            tRoachMove.Elapsed += (s, en) =>
            {
                Task.Run(new Action(() =>
                {
                    RoachMove();
                }));
                tRoachMove.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            tRoachMove.Start();
        }


        protected virtual void RoachMove()
        {
            lock2 = new object();
            lock (lock2)
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
                        Screen screen = Screen.FromControl(f);
                        roachPosRect = f.DesktopBounds;

                        if (cX <= 0)
                        {
                            cX = screen.Bounds.Width;
                            foreach (Screen aScreen in Screen.AllScreens)
                            {
                                if (cX < aScreen.Bounds.Width)
                                    cX = aScreen.Bounds.Width;
                            }
                            cX -= 32;
                        }

                        if (cY <= 0)
                        {
                            cY = screen.Bounds.Height;
                            foreach (Screen aScreen in Screen.AllScreens)
                            {
                                if (cY < aScreen.Bounds.Height)
                                    cY = aScreen.Bounds.Height;
                            }
                            cY -= 32;
                        }
                    }
                }

                roachPosition = new Point(cX, cY);

                lock2 = new object();
                lock (lock2)
                {
                    SetRoachBG(roachPosition);
                    if (roachCnt % 24 == 8)
                    {
                        lock3 = new object();
                        lock (lock3)
                        {
                            BringToFront();
                        }
                    }
                    else if (roachCnt % 12 == 4)
                    {
                        lock3 = new object();
                        lock (lock3)
                        {
                            Show();
                        }
                    }
                }

                System.Threading.Thread.Sleep(64);
            }

            RoachExit("RoachMove", new MouseEventArgs(MouseButtons.Left, 1, roachPosition.X + 1, roachPosition.Y + 1, 3));
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


        protected virtual Form FindInternal()
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

        protected virtual void RoachExit(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Process[] processes = Processes.GetRunningProcessesByName(Program.progName);
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

        protected virtual void AppExit(object sender, EventArgs e)
        {
            string orocRoachName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            MessageBox.Show($"Roach {orocRoachName} is exiting now!", $"{orocRoachName} roach exit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Program.ReleaseCloseDisposeMutex();
            Dispose();
            Application.ExitThread();
            Application.Exit();
            Environment.Exit(0);
        }


    }
}
