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
using Area23.At.Framework.ScreenCapture;

namespace Area23.At.WinForm.TransparentForms
{
    public partial class TForm : Form
    {
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
            if (this.winDeskImg == null)
                SetTransBG();
            ScreenCapture.CaptureScreenAndAllWindowsToDirectory(Application.UserAppDataPath);            
        }

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

        private void OnLeave(object sender, EventArgs e)
        {
            
        }

    }
}
