using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Area23.At.Framework.Screen;

namespace Area23.At.Framework.Screen
{
    
    /// <summary>
    /// Heinrich Elsigan
    /// GNU General Public License v3
    /// </summary>
    public class ScreenCapture
    {
        /// <summary>
        /// CaptureScreen creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public Image CaptureScreen()
        {
            return CaptureWindow(NativeMethods.User32.GetDesktopWindow());
        }

        /// <summary>
        /// CaptureDesktopScreen creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public Image CaptureDesktopScreen()
        {
            // capture desktop window            
            IntPtr ptrDesk = NativeMethods.User32.GetDesktopWindow();
            Image imageDesk = CaptureWindow(ptrDesk);

            return imageDesk;
        }

        /// <summary>
        /// CaptureAllDesktops capture all existing windows desktop
        /// </summary>
        /// <returns>Image, that contains all windows desktop capture</returns>
        public Image CaptureAllDesktops()
        {
            Screen[] screens;
            screens = Screen.AllScreens;
            int noofscreens = screens.Length, maxwidth = 0, maxheight = 0;
            for (int i = 0; i < noofscreens; i++)
            {
                if (maxwidth < (screens[i].Bounds.X + screens[i].Bounds.Width)) maxwidth = screens[i].Bounds.X + screens[i].Bounds.Width;
                if (maxheight < (screens[i].Bounds.Y + screens[i].Bounds.Height)) maxheight = screens[i].Bounds.Y + screens[i].Bounds.Height;
            }
            Image desktopImage = CaptureAllScreen(0, 0, maxwidth, maxheight);
            return desktopImage;
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop and child windows
        /// </summary>
        /// <returns>Array of Images</returns>
        public Image[] CaptureAllWindows()
        {
            List<Image> windowImages = new List<Image>();
            IntPtr deskPtr = NativeMethods.User32.GetDesktopWindow();
            Image imageDesk = CaptureWindow(deskPtr);
            windowImages.Add(imageDesk);
            IntPtr topPtr = NativeMethods.User32.GetTopWindow(deskPtr);
            Image imageTop = CaptureWindow(topPtr);
            windowImages.Add(imageTop);
            IntPtr nextPtr = topPtr;
            for (int i = 0; i < 16384; i++)
            {
                try
                {
                    nextPtr = NativeMethods.User32.GetWindow(nextPtr, NativeMethods.User32.GW_HWNDNEXT);
                    Image nextImage = CaptureWindow(nextPtr);
                    if (nextImage.Height > 1 && nextImage.Width > 1)
                        windowImages.Add(nextImage);
                }
                catch (Exception) { }
            }

            return windowImages.ToArray();
        }


        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns>Image</returns>
        public Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = NativeMethods.User32.GetWindowDC(handle);
            // get the size
            NativeMethods.User32.RECT windowRect = new NativeMethods.User32.RECT();
            NativeMethods.User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = NativeMethods.GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = NativeMethods.GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = NativeMethods.GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            NativeMethods.GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, NativeMethods.GDI32.SRCCOPY);
            // restore selection
            NativeMethods.GDI32.SelectObject(hdcDest, hOld);
            // clean up
            NativeMethods.GDI32.DeleteDC(hdcDest);
            NativeMethods.User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            NativeMethods.GDI32.DeleteObject(hBitmap);
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
        public Image CaptureAllScreen(int x, int y, int width, int height)
        {
            //create DC for the entire virtual screen
            int hdcSrc = NativeMethods.GDI32.CreateDC("DISPLAY", null, null, IntPtr.Zero);
            int hdcDest = NativeMethods.GDI32.CreateCompatibleDC(hdcSrc);
            int hBitmap = NativeMethods.GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            NativeMethods.GDI32.SelectObject(hdcDest, hBitmap);

            // set the destination area White - a little complicated
            Bitmap bmp = new Bitmap(width, height);
            Image ii = (Image)bmp;
            Graphics gf = Graphics.FromImage(ii);
            IntPtr hdc = gf.GetHdc();
            //use whiteness flag to make destination screen white
            NativeMethods.GDI32.BitBlt(hdcDest, 0, 0, width, height, (int)hdc, 0, 0, 0x00FF0062);
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
                    NativeMethods.GDI32.BitBlt(hdcDest, X - x, Y - y, X1 - X, Y1 - Y, hdcSrc, X, Y,
                             0x40000000 | 0x00CC0020); //SRCCOPY AND CAPTUREBLT
                }
            }

            // send image to clipboard
            Image imgHBmp = Image.FromHbitmap(new IntPtr(hBitmap));

            NativeMethods.GDI32.DeleteDC(hdcSrc);
            NativeMethods.GDI32.DeleteDC(hdcDest);
            NativeMethods.GDI32.DeleteObject(hBitmap);

            return imgHBmp;
        }

        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }

        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        /// <summary>
        /// Captures a screen shot of the entire desktop and all child windows and saves it tó a directory
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="format"></param>
        public void CaptureScreenAndAllWindowsToDirectory(string directory, ImageFormat format)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            Image[] imgs = CaptureAllWindows();
            int ix = 0;
            foreach (Image img in imgs)
            {
                string filename = Path.Combine(directory, DateTime.Now.Ticks.ToString() + ix++ + ".png");
                img.Save(filename, format);
            }

            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Length <= 2048)
                    fi.Delete();
            }
        }

    }

}
