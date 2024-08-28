using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Win32Api;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Area23.At.WinForm.TWinFormCore
{
    public partial class TWinForm : Form
    {
        protected Image? winDeskImg = null;
        int speachCnt = 0;
        volatile bool locChangedOff = false;
        object spinLock = new object();
        DateTime lastCapture = DateTime.Now;


        public TWinForm()
        {
            InitializeComponent();
        }


        public void SetTransBG()
        {
            if (winDeskImg == null)
                winDeskImg = GetDesktopImage();

            Graphics g = Graphics.FromImage(winDeskImg);
            Form f = this.FindForm();
            Image? bgImg = Crop(winDeskImg, DesktopBounds.Size.Width, DesktopBounds.Size.Height, f.DesktopBounds.Location.X + 8, f.DesktopBounds.Location.Y + 32);
            if (bgImg != null)
                this.BackgroundImage = bgImg;
        }

        public Image? Crop(Image image, int width, int height, int x, int y)
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
            string captureDir = Application.UserAppDataPath;
            captureDir = "d:\\source\tmp";
            // ScreenCapture.CaptureScreenAndAllWindowsToDirectory(captureDir);
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

        }

        private void OnLocationChanged(object sender, EventArgs e)
        {
            if (!locChangedOff)
                SetTransBG();
        }

    }
}