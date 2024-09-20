using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Win32Api
{

    /// <summary>
    /// HWndInfo - window handle class
    /// Thanks to <see href="https://github.com/dotnet">github.com/dotnet</see>,
    /// <see href="https://stackoverflow.com/">stackoverflow.com/</see>,
    /// <see href="https://www.pinvoke.net/">pinvoke.net</see> and
    /// <see cref="https://www.codeproject.com/">cpdeproject.com</see>
    /// </summary>
    public class HWndInfo
    {
        #region DllImport
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private const UInt32 WM_CLOSE = 0x0010;

        [return: MarshalAs(UnmanagedType.Bool)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [DllImport("user32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool CloseWindowStation(IntPtr hWinsta);

        [DllImport("user32.dll")]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        #endregion DllImport

        #region closeWindow

        /// <summary>
        /// CloseWindow sends standard close message to window by handle
        /// </summary>
        /// <param name="hwnd">window handle</param>
        public static void CloseWindow(IntPtr hwnd)
        {
            SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        #endregion closeWindow

        #region getFindWindow        

        /// <summary>
        /// FindOffice365LoginWindow
        /// </summary>
        /// <param name="hWndBlock">window handle to find</param>
        /// <param name="pWnd">parent window handle</param>
        /// <returns>window handle to find</returns>
        public static IntPtr FindOffice365LoginWindow(ref IntPtr hWndChild, IntPtr pWnd)
        {
            if (hWndChild != IntPtr.Zero)
                return hWndChild;
            if (hWndChild == IntPtr.Zero)
                hWndChild = HWndInfo.FindWindowEx(pWnd, IntPtr.Zero, "BasicEmbeddedBrowser", null);
            if (hWndChild == IntPtr.Zero)
                hWndChild = HWndInfo.FindWindowEx(pWnd, IntPtr.Zero, "BasicEmbeddedBrowser", "");
            if (hWndChild == IntPtr.Zero)
                hWndChild = HWndInfo.FindWindowEx(pWnd, IntPtr.Zero, "Internet Explorer_Server", "");
            if (hWndChild == IntPtr.Zero)
                hWndChild = HWndInfo.FindWindowEx(pWnd, IntPtr.Zero, "Internet Explorer_Server", null);
            if (hWndChild == IntPtr.Zero)
                hWndChild = HWndInfo.FindWindowEx(pWnd, IntPtr.Zero, "Shell DocObject View", "");
            if (hWndChild == IntPtr.Zero)
                hWndChild = HWndInfo.FindWindowEx(pWnd, IntPtr.Zero, "Shell DocObject View", null);

            if (hWndChild == IntPtr.Zero)
                hWndChild = FindBlockWindowOffice365(ref hWndChild, pWnd, "Internet Explorer");

            return hWndChild;
        }

        /// <summary>
        /// Find blocking Office365 live signin window
        /// </summary>
        /// <param name="hWndBlock">window handle to find</param>
        /// <param name="pWnd">parent window handle</param>
        /// <param name="windowTitle">window title of office365 live signin dialog</param>
        /// <returns>window handle to find</returns>
        public static IntPtr FindBlockWindowOffice365(ref IntPtr hWndBlock, IntPtr pWnd, string windowTitle = "Internet Explorer")
        {
            if (hWndBlock != IntPtr.Zero)
                return hWndBlock;
            if (hWndBlock == IntPtr.Zero)
                hWndBlock = HWndInfo.FindWindowEx(pWnd, IntPtr.Zero, "Internet Explorer_Server", windowTitle);
            if (hWndBlock == IntPtr.Zero)
                hWndBlock = HWndInfo.FindWindowEx(pWnd, IntPtr.Zero, "BasicEmbeddedBrowser", windowTitle);
            if (hWndBlock == IntPtr.Zero)
                hWndBlock = HWndInfo.FindWindowEx(pWnd, IntPtr.Zero, "Shell DocObject View", windowTitle);
            if (hWndBlock != IntPtr.Zero)
                hWndBlock = HWndInfo.FindWindow("BasicEmbeddedBrowser", windowTitle);
            if (hWndBlock != IntPtr.Zero)
                hWndBlock = HWndInfo.FindWindow("Internet Explorer_Server", windowTitle);
            if (hWndBlock != IntPtr.Zero)
                hWndBlock = HWndInfo.FindWindow("Shell DocObject View", windowTitle);
            if (hWndBlock != IntPtr.Zero)
                hWndBlock = HWndInfo.FindWindow("", windowTitle);

            return hWndBlock;
        }

        /// <summary>
        /// GetDesktopWindow => window handle of root desktop window
        /// </summary>
        /// <returns>window handle of root desktop window</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IntPtr GetDesktopWindow()
        {
            IntPtr deskPtr = NativeWrapper.User32.GetDesktopWindow();
            if (deskPtr == IntPtr.Zero)
                throw new InvalidOperationException("HWndInfo.GetDesktopWindow() returned IntPtr.Zero");
            return deskPtr;
        }

        /// <summary>
        /// GetTopWindow finds the top most window for a specific window
        /// </summary>
        /// <param name="hWnd">window handle</param>
        /// <returns>top most window handle</returns>
        public static IntPtr GetTopWindow(IntPtr hWnd)
        {
            IntPtr myPtr = NativeWrapper.User32.GetTopWindow(hWnd);
            return myPtr;
        }

        #endregion getFindWindow

        /// <summary>
        /// Finds out, if window handle, belongs to process identifier
        /// </summary>
        /// <param name="bhWnd">window handle</param>
        /// <param name="bhPid">process identifier</param>
        /// <returns>true, if window handle belongs to process identifier</returns>
        public static bool WindowHandleBelongsTo(IntPtr bhWnd, int bhPid)
        {
            if (bhWnd == IntPtr.Zero)
                return false;

            System.Diagnostics.Process bhProc = Processes.GetProcessByHwnd(bhWnd);
            if (bhProc != null && !bhProc.HasExited && bhProc.MainWindowHandle == bhWnd && bhProc.Id == bhPid)
                return true;

            return false;
        }

        #region Instance

        internal delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        internal IntPtr _MainHandle;

        public HWndInfo(IntPtr handle)
        {
            _MainHandle = handle;
        }

        public void Close(IntPtr hwnd)
        {
            SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        public List<IntPtr> GetAllChildHandles()
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(_MainHandle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        internal bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList != null && gcChildhandlesList.Target != null)
            {
                List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
                childHandles.Add(hWnd);

                return true;
            }

            return false;
        }

        #endregion Instance
    }

}
