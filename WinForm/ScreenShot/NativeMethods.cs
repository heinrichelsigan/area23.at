using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.ScreenShot
{
    /// <summary>
    /// NativeMethods contain inner classes for User32, Kernel32 and GDI32 Windows Core API calls
    /// </summary>
    [SuppressUnmanagedCodeSecurityAttribute]
    internal static class NativeMethods
    {

        /// <summary>
        /// Helper class containing kernel32 functions
        /// </summary>
        internal class Kernel32
        {
            internal const int ATTACH_PARENT_PROCESS = -1;

            /// <summary>
            /// AttachConsole to Windows Form App
            /// </summary>
            /// <param name="dwProcessId"></param>
            /// <returns></returns>
            [DllImport("kernel32.dll")]
            internal static extern bool AttachConsole(int dwProcessId);
        }

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        internal class GDI32
        {
            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter            

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("GDI32.dll")]
            public static extern bool BitBlt(int hdcDest, int nXDest, int nYDest,
                int nWidth, int nHeight, int hdcSrc, 
                int nXSrc, int nYSrc, int dwRop);

            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
            [DllImport("GDI32.dll")]
            public static extern int CreateCompatibleBitmap(int hdc, int nWidth, int nHeight);             

            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("GDI32.dll")]
            public static extern int CreateCompatibleDC(int hdc);

            [DllImport("gdi32.dll")]
            public static extern int CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);


            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("GDI32.dll")]
            public static extern bool DeleteDC(int hdc);


            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);            
            [DllImport("GDI32.dll")]
            public static extern bool DeleteObject(int hObject);

            [DllImport("GDI32.dll")]
            public static extern int GetDeviceCaps(int hdc, int nIndex);

            [DllImport("GDI32.dll")]
            public static extern int SelectObject(int hdc, int hgdiobj);

            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        }

        /// <summary>
        /// User class containing simplified User32 API functions with int instead of IntPtr
        /// </summary>
        internal class User
        {
            [DllImport("user32.dll")]
            public static extern int GetDesktopWindow();
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        internal class User32
        {
            public const int HT_CAPTION = 0x2;

            public const uint GW_HWNDFIRST = 0x000;
            public const uint GW_HWNDLAST = 0x001;
            public const uint GW_HWNDNEXT = 0x002;
            public const uint GW_HWNDPREV = 0x003;
            public const uint GW_OWNER = 0x004;
            public const uint GW_CHILD = 0x005;
            public const uint GW_ENABLEDPOPUP = 0x006;

            public const uint WM_PRINT = 0x317;
            public const int WM_NCLBUTTONDOWN = 0xA1;
            public const int WM_APPCOMMAND = 0x319;

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [Flags]
            internal enum PRF_FLAGS : uint
            {
                CHECKVISIBLE = 0x01,
                CHILDREN = 0x02,
                CLIENT = 0x04,
                ERASEBKGND = 0x08,
                NONCLIENT = 0x10,
                OWNED = 0x20
            }


            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();


            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

            [DllImport("user32.dll")]
            public static extern IntPtr GetTopWindow(IntPtr hWnd);

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
            [DllImport("User32.dll")]
            public static extern int GetWindowDC(int hWnd);

            [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
            public static extern bool ReleaseCapture();

            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("User32.dll")]
            public static extern int ReleaseDC(int hWnd, int hDC);


            [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

            [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
            public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll")]
            internal static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr hdc, PRF_FLAGS drawingOptions);

        }

    }
}
