﻿using Area23.At.Framework.Library.Core;
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
    public abstract partial class BRoach : Form
    {
        protected internal Image capturedImage = null;
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


        protected internal virtual Image CaptureForm(Form f)
        {
            if (f == null)
                f = FindInternal();

            capturedImage = ScreenCapture.CaptureWindow(f.Handle);
            return capturedImage;
        }


        internal virtual void SetRoachBG(Point pt)
        {
            Screen screen = Screen.FromControl(this);

            if (pt == Point.Empty || (pt.X <= 0 && pt.Y <= 0))
            {
                pt = new Point(((int)(screen.Bounds.Width) - 64),
                    ((int)(screen.Bounds.Height) - 64));
            }
            this.Location = pt;
            this.SetDesktopLocation(pt.X, pt.Y);

            Image bgImg = CaptureForm(this);
            // this.BackgroundImage = bgImg;

            if (roachCnt % 7 == 0)
            {
                if (roachCnt % 4 == 0)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                else if (roachCnt % 4 == 1)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach1;
                if (roachCnt % 4 == 2)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach;
                else if (roachCnt % 4 == 3)
                    this.panelRoach.BackgroundImage = (System.Drawing.Bitmap)global::Area23.At.WinForm.WinRoachCore.Properties.Resource.CRoach0;
            }

        }


        protected internal abstract void SelfMoveRoach(int interval = 0);        

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
                        Screen screen = Screen.FromControl(f);
                        roachPosRect = f.DesktopBounds;
                        
                        if (cX <= 0)
                            cX = screen.Bounds.Width - 64;
                        if (cY <= 0)
                            cY = screen.Bounds.Height - 64;
                    }
                }

                roachPosition = new Point(cX, cY);

                lock2 = new object();
                lock (lock2)
                {
                    SetRoachBG(roachPosition);
                    if (roachCnt % 23 == 2)
                    {
                        lock3 = new object();
                        lock (lock3)
                        {                            
                            BringToFront();
                        }
                    }
                    else if (roachCnt % 11 == 1)
                    {
                        lock3 = new object();
                        lock (lock3)
                        {
                            Show();
                        }
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
