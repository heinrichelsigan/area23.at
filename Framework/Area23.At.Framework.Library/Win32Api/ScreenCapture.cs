using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Area23.At.Framework.Library.Win32Api
{

    /// <summary>
    /// Thanks to <see href="https://github.com/dotnet">github.com/dotnet</see>,
    /// <see href="https://stackoverflow.com/">stackoverflow.com/</see>,
    /// <see href="https://www.pinvoke.net/">pinvoke.net</see> and
    /// <see cref="https://www.codeproject.com/Articles/546006/Screen-Capture-on-Multiple-Monitors"/>
    /// </summary>
    public static class ScreenCapture
    {

        /// <summary>
        /// CaptureScreen creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public static Image CaptureScreen()
        {
            return CaptureWindow(NativeWrapper.User32.GetDesktopWindow());
        }

        /// <summary>
        /// CaptureDesktopScreen creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static Image CaptureDesktopScreen()
        {
            // capture desktop window            
            IntPtr ptrDesk = NativeWrapper.User32.GetDesktopWindow();
            Image imageDesk = CaptureWindow(ptrDesk);

            return imageDesk;
        }

        /// <summary>
        /// CaptureAllDesktops capture all existing windows desktop
        /// </summary>
        /// <returns>Image, that contains all windows desktop capture</returns>
        public static Image CaptureAllDesktops()
        {
            Screen[] screens;
            screens = Screen.AllScreens;
            int noofscreens = screens.Length, maxwidth = 0, maxheight = 0;
            int minX = 0, minY = 0;
            //for (int j = 0; j < noofscreens; j++)
            //{
            //    if (screens[j].Bounds.X < minX)
            //        minX = screens[j].Bounds.X;
            //    if (screens[j].WorkingArea.X < minX)
            //        minX = screens[j].WorkingArea.X;
            //    if (screens[j].Bounds.Y < minY)
            //        minY = screens[j].Bounds.Y;
            //    if (screens[j].WorkingArea.Y < minY)
            //        minY = screens[j].WorkingArea.Y;
            //}

            for (int i = 0; i < noofscreens; i++)
            {
                if (maxwidth < (screens[i].Bounds.X + screens[i].Bounds.Width)) maxwidth = screens[i].Bounds.X + screens[i].Bounds.Width;
                if (maxheight < (screens[i].Bounds.Y + screens[i].Bounds.Height)) maxheight = screens[i].Bounds.Y + screens[i].Bounds.Height;
            }
            //if (minX < 0)
            //    maxwidth += Math.Abs(minX);
            //if (minY < 0)
            //    maxheight += Math.Abs(minY);

            Image desktopImage = CaptureAllScreen(minX, minY, maxwidth, maxheight);
            return desktopImage;
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop and child windows
        /// </summary>
        /// <returns>Array of Images</returns>
        public static Image[] CaptureAllWindows()
        {
            // List<Image> windowImages = new List<Image>();
            Dictionary<IntPtr, Image> windowsImages = new Dictionary<IntPtr, Image>();
            Image imageAllSreens = CaptureAllDesktops();
            windowsImages.Add(IntPtr.Zero, imageAllSreens);

            IntPtr deskPtr = NativeWrapper.User32.GetDesktopWindow();
            Image imageDesk = CaptureWindow(deskPtr);
            windowsImages.Add(deskPtr, imageDesk);

            IntPtr topPtr = NativeWrapper.User32.GetTopWindow(deskPtr);
            Image imageTop = CaptureWindow(topPtr);
            windowsImages.Add(topPtr, imageTop);

            IntPtr nextPtr = topPtr;
            for (uint winCnt = 0; winCnt < 16384; winCnt++)
            {
                try
                {
                    nextPtr = NativeWrapper.User32.GetWindow(nextPtr, NativeWrapper.User32.GW_HWNDNEXT);
                    if (!windowsImages.Keys.Contains(nextPtr))
                    {
                        Image nextImage = CaptureWindow(nextPtr);
                        if (nextImage.Height > 1 && nextImage.Width > 1)
                            windowsImages.Add(nextPtr, nextImage);
                    }                                        
                }
                catch (Exception exCapture) 
                {
                    Area23Log.LogOriginMsgEx("ScreenCapture.CaptureAllWindows()",
                        $"Exception on capturing window {winCnt} with windows handle {nextPtr.ToString()}.", exCapture);
                }
            }

            return windowsImages.Values.ToArray();
        }


        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns>Image</returns>
        public static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = NativeWrapper.User32.GetWindowDC(handle);
            // get the size
            NativeWrapper.User32.RECT windowRect = new NativeWrapper.User32.RECT();
            NativeWrapper.User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = NativeWrapper.GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = NativeWrapper.GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = NativeWrapper.GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            NativeWrapper.GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, NativeWrapper.GDI32.SRCCOPY);
            // restore selection
            NativeWrapper.GDI32.SelectObject(hdcDest, hOld);
            // clean up
            NativeWrapper.GDI32.DeleteDC(hdcDest);
            NativeWrapper.User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            NativeWrapper.GDI32.DeleteObject(hBitmap);
            return img;
        }


        /// <summary>
        /// CaptureAllScreen capture screen section    
        /// </summary>
        /// <param name="x">x start postion to capture</param>
        /// <param name="y">y start postion to capture</param>
        /// <param name="width">full with of all screens</param>
        /// <param name="height">full height of all screens</param>
        /// <returns>Image, that contains all screen capture cutting</returns>        
        public static Image CaptureAllScreen(int x, int y, int width, int height)
        {
            //create DC for the entire virtual screen
            int hdcSrc = NativeWrapper.GDI32.CreateDC("DISPLAY", null, null, IntPtr.Zero);
            int hdcDest = NativeWrapper.GDI32.CreateCompatibleDC(hdcSrc);
            int hBitmap = NativeWrapper.GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            NativeWrapper.GDI32.SelectObject(hdcDest, hBitmap);

            // set the destination area White - a little complicated
            Bitmap bmp = new Bitmap(width, height);
            Image ii = (Image)bmp;
            Graphics gf = Graphics.FromImage(ii);
            IntPtr hdc = gf.GetHdc();
            //use whiteness flag to make destination screen white
            NativeWrapper.GDI32.BitBlt(hdcDest, 0, 0, width, height, (int)hdc, 0, 0, 0x00FF0062);
            gf.Dispose();
            ii.Dispose();
            bmp.Dispose();

            //Now copy the areas from each screen on the destination hbitmap
            Screen[] screendata = Screen.AllScreens;
            int X, X1, Y, Y1;
            for (int i = 0; i < screendata.Length; i++)
            {
                if (screendata[i].Bounds.X > (x + width) || (screendata[i].Bounds.X +
                   screendata[i].Bounds.Width) < x || screendata[i].Bounds.Y > (y + height) ||
                   (screendata[i].Bounds.Y + screendata[i].Bounds.Height) < y)
                {// no common area
                }
                else
                {
                    // something  common
                    if (x < screendata[i].Bounds.X) X = screendata[i].Bounds.X; else X = x;
                    if ((x + width) > (screendata[i].Bounds.X + screendata[i].Bounds.Width))
                        X1 = screendata[i].Bounds.X + screendata[i].Bounds.Width;
                    else X1 = x + width;
                    if (y < screendata[i].Bounds.Y) Y = screendata[i].Bounds.Y; else Y = y;
                    if ((y + height) > (screendata[i].Bounds.Y + screendata[i].Bounds.Height))
                        Y1 = screendata[i].Bounds.Y + screendata[i].Bounds.Height;
                    else Y1 = y + height;
                    // Main API that does memory data transfer
                    NativeWrapper.GDI32.BitBlt(hdcDest, X - x, Y - y, X1 - X, Y1 - Y, hdcSrc, X, Y,
                             0x40000000 | 0x00CC0020); //SRCCOPY AND CAPTUREBLT
                }
            }

            // send image to clipboard
            Image imgHBmp = Image.FromHbitmap(new IntPtr(hBitmap));

            NativeWrapper.GDI32.DeleteDC(hdcSrc);
            NativeWrapper.GDI32.DeleteDC(hdcDest);
            NativeWrapper.GDI32.DeleteObject(hBitmap);

            return imgHBmp;
        }

        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }

        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        /// <summary>
        /// Captures a screen shot of the entire desktop and all child windows and saves it tó a directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="format"></param>
        public static Image[] CaptureScreenAndAllWindowsToDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string[] files = Directory.GetFiles(directory);
            foreach (string f in files)
            {
                try
                {
                    File.Delete(f);
                }
                catch { }
            }

            ImageFormat format = ImageFormat.Png;
            Image[] imgs = CaptureAllWindows();
            int ix = 0;            
            foreach (Image img in imgs)
            {
                string filename = Path.Combine(directory, $"{ix:X4} {DateTime.Now.Ticks}.png");
                img.Save(filename, format);
                ix++;
            }

            string[] filez = Directory.GetFiles(directory);
            foreach (string file in filez)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Length <= 16384)
                    fi.Delete();
            }

            return imgs;
        }

    }

}
