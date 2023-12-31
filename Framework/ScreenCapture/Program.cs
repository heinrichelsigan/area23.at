﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.Framework.ScreenCapture
{
    internal static class Program
    {
        /// <summary>
        /// main entry point for classic .NET 4.8.x
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Drawing.Image winDesktopImg;            
            object spinLock = new object();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            lock (spinLock)
            {
                // this.WindowState = FormWindowState.Minimized;
                ScreenCapture sc = new ScreenCapture();
                winDesktopImg = sc.CaptureAllDesktops();                
            }
            System.Windows.Forms.NativeWindow nativeWin = new NativeWindow();
            System.Windows.Forms.Form containerCtrl = (Form)(new ContainerControl());
            containerCtrl.Dock = DockStyle.Fill;
            System.Windows.Forms.PictureBox pictureBox1 = new System.Windows.Forms.PictureBox();
            pictureBox1.Image = winDesktopImg;
            containerCtrl.Controls.Add(pictureBox1);
            
            MessageBox.Show("ScreenCapture", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Run(containerCtrl);            
        }
         
    }
}
