using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Win32Api;
using Area23.At.WinForm.TransparentForms.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Area23.At.WinForm.TransparentForms.TForms
{
    public partial class TForm : Form
    {
        /// <summary>
        /// <see cref="bool">static bool exiting</see> true, if application is currently closing / exiting, otherwise always false
        /// </summary>
        private static bool exiting = false;
        /// <summary>
        /// string moduleName = MethodBase.GetCurrentMethod().DeclaringType.Name;
        /// </summary>
        private static string moduleName = MethodBase.GetCurrentMethod().DeclaringType.Name;
        protected Image winDeskImg = null;
        volatile int speachCnt = 0;
        volatile bool locChangedOff = false;
        object spinLock = new object();
        DateTime lastCapture = DateTime.Now;
        DateTime lastSay = DateTime.Now;

        string[] setences = {"Twenty", "Atou Marriage Fourty", "close down", "last beat winner", "Thank you and enough",
            "I change with Jack", "Last but not least", "Hey Mister", "Hey misses"};

        string[] schnapserlm = { "Und Zwanzig", "Vierzig", "Danke und genug hab I", "I drah zua", "Habeas tibi",
            "Tausch gegen den Buam aus", "Letzter fertzter", "Na oida" };

        public string TFormType
        {
            get => this.GetType().ToString();
        }


        public TForm()
        {
            InitializeComponent();
        }


        public void SetTransBG()
        {
            if (winDeskImg == null)
                winDeskImg = GetDesktopImage();

            Graphics g = Graphics.FromImage(winDeskImg);
            Form f = this.FindForm();
            Image bgImg = Crop(winDeskImg, DesktopBounds.Size.Width, DesktopBounds.Size.Height, f.DesktopBounds.Location.X + 8, f.DesktopBounds.Location.Y + 32);
            this.BackgroundImage = bgImg;
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

        public Image GetDesktopImage()
        {
            locChangedOff = true;
            spinLock = new object();
            
            Image winDesktopImg;
            lock (spinLock)
            {
                // this.WindowState = FormWindowState.Minimized;
                winDesktopImg = ScreenCapture.CaptureAllDesktops();
                this.WindowState = FormWindowState.Normal;
                lastCapture = DateTime.Now;
                locChangedOff = false;
            }
            return winDesktopImg;
        }

        private void OnLoad(object sender, EventArgs e)
        {            
            // ScreenCapture.CaptureScreenAndAllWindowsToDirectory(Application.UserAppDataPath);            
        }

        /*
        private void OnResizeEnd(object sender, EventArgs e)
        {
            if (!locChangedOff)
                SetTransBG();

            System.Timers.Timer tLoad0 = new System.Timers.Timer { Interval = 200 };
            tLoad0.Elapsed += (s, en) =>
            {
                this.Invoke(new Action(() =>
                {
                    TimeSpan tdiff = DateTime.Now.Subtract(lastCapture);
                    if (tdiff > new TimeSpan(0, 0, 0, 2))
                    {
                        winDeskImg = GetDesktopImage();
                    }

                }));
                tLoad0.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            tLoad0.Start();

            System.Timers.Timer tSay = new System.Timers.Timer { Interval = 1200 };
            tSay.Elapsed += (s, en) =>
            {
                this.Invoke(new Action(() =>
                {
                    TimeSpan tdiff = DateTime.Now.Subtract(lastSay);
                    if (tdiff > new TimeSpan(0, 0, 0, 6))
                    {
                        RotateSay();
                    }

                }));
                tSay.Stop(); // Stop the timer(otherwise keeps on calling)
            };
            tSay.Start();
        }

        private void OnLocationChanged(object sender, EventArgs e)
        {            
            if (!locChangedOff)
                SetTransBG();                        
        }
        */


        /// <summary>
        /// TClose handles form closing events
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">FormClosingEventArgs e</param>
        internal void TClose(object sender, EventArgs e)
        {
            if (exiting)
                return;
            
            DialogResult res = MessageBox.Show($"Really Close Transparent Form .Net 4.8.1 Classic?\nAbort to close and exit.\r\nRetry to restart.\nIgnore to reload.", "Question", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
            if (res == DialogResult.Retry)
            {
                exiting = false;
                menuItemRestart_Click(sender, e);
                Area23Log.LogStatic("Retry restarts transparent form as new process!");
                return;
            }
            
            exiting = true;
            TEventArgs<DialogResult> tEventArgs = new TEventArgs<DialogResult>(res);
            OnExit(sender, tEventArgs);
                        
            return;
        }

        internal void OnExit(object sender, TEventArgs<DialogResult> tEventArgs)
        {            
            if (exiting)
                return;

            exiting = true;
            lock (spinLock)
            {
                try
                {
                    List<System.Windows.Forms.Control> ctrlList = new List<Control>();
                    ctrlList.AddRange(System.Windows.Forms.Application.OpenForms.OfType<System.Windows.Forms.Control>().ToList());
                    foreach (Form formOpen in System.Windows.Forms.Application.OpenForms.OfType<System.Windows.Forms.Form>().ToList())
                        ctrlList.Add((Control)formOpen);
                    foreach (Form containerOpen in System.Windows.Forms.Application.OpenForms.OfType<System.Windows.Forms.ContainerControl>().ToList())
                        ctrlList.Add((Control)containerOpen);

                    foreach (Control ctrl in ctrlList)
                    {
                        if (ctrl is System.Windows.Forms.Form) // || ctrl is System.Windows.Forms.ContainerControl) // || ctrl is System.Windows.Forms.Control)
                            ((Form)ctrl).Close();
                    }
                    this.Close();
                }
                catch (Exception exClose)
                {
                    Area23Log.LogStatic(exClose);
                }

                try
                {
                    this.RemoveOwnedForm(this.Owner);
                }
                catch (Exception exRemoveOwner)
                {
                    Area23Log.LogStatic(exRemoveOwner);
                }

                try
                {
                    this.Dispose(true);
                }
                catch (Exception exDispose)
                {
                    Area23Log.LogStatic(exDispose);
                }

                try
                {
                    if (tEventArgs.Data == DialogResult.Abort)
                        System.Windows.Forms.Application.Exit();
                    else
                        System.Windows.Forms.Application.Restart();
                }
                catch { }
                finally
                {
                    exiting = false;
                    if (tEventArgs.Data == DialogResult.Abort)
                        System.Windows.Forms.Application.Exit();
                }
            }

            return;
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            TClose(sender, (EventArgs)e);
        }

        private void OnClosed(object sender, FormClosedEventArgs e)
        {
            TClose(sender, (EventArgs)e);
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            TAbout about = new TAbout();
            about.ShowDialog();
        }

        private void menuItemInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"{Text} type {TFormType} Information MessageBox.", $"{Text} type {TFormType}", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected internal virtual void menuItemExit_Click(object sender, EventArgs e)
        {
            TEventArgs<DialogResult> tDiaEventArgs = new TEventArgs<DialogResult>(DialogResult.Abort);
            OnExit(sender, tDiaEventArgs);
        }

        private void menuItemRestart_Click(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(System.Windows.Forms.Application.ExecutablePath);
            string reStartCall = (file.Exists) ? file.FullName.ToString() : Assembly.GetExecutingAssembly().Location.ToString();
            string sFlag = " /restart " + file.FullName.ToString() + " " + file.DirectoryName.ToString();
            ProcessStartInfo procStrtNfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                CreateNoWindow = false,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = reStartCall,
                Arguments = sFlag
            };            
            Process.Start(procStrtNfo);

            exiting = false;
            Area23Log.LogStatic("Restarrt performed!");
            return;
        }

        private void menuItemReload_Click(object sender, EventArgs e)
        {
            TEventArgs<DialogResult> tEventArgs = new TEventArgs<DialogResult>(DialogResult.Ignore);
            OnExit(sender, tEventArgs);
        }

        private void menuItemFortune_Click(object sender, EventArgs e)
        {
            TFortune fortune = new TFortune();
            fortune.Show();
        }
    }
}
