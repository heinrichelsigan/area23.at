using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

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
        /// FindChildWindow
        /// </summary>
        /// <param name="hWndChild">child window handle to find</param>
        /// <param name="hWndParent">parent window handle</param>
        /// <param name="className">Class Name</param>
        /// <param name="windowTitle">Window Title</param>
        /// <returns>child window handle to find</returns>
        public static IntPtr FindChildWindow(ref IntPtr hWndChild, IntPtr hWndParent, string className = "", string windowTitle = "Internet Explorer")
        {
            IntPtr hWndReturn = (hWndChild != IntPtr.Zero) ? hWndChild : IntPtr.Zero;

            if (hWndReturn == IntPtr.Zero)
                hWndReturn = HWndInfo.FindWindowEx(hWndParent, hWndChild, className, windowTitle);
            if (hWndReturn == IntPtr.Zero)
                hWndReturn = HWndInfo.FindWindowEx(hWndParent, hWndChild, "", windowTitle);
            if (hWndReturn == IntPtr.Zero)
                hWndReturn = HWndInfo.FindWindowEx(hWndParent, hWndChild, className, null);


            return hWndChild;
        }


        /// <summary>
        /// <returns>window handle to find</returns>
        /// </summary>
        /// <param name="className">Class Name</param>
        /// <param name="windowTitle">Window Title</param>
        /// <returns><see cref="IntPtr"/> (<seealso cref="nint"/) window handle</returns>
        public static IntPtr FindWindowHandle(string className = "", string windowTitle = "Internet Explorer")
        {
            IntPtr hWndRet = IntPtr.Zero;

            hWndRet = HWndInfo.FindWindow(className, windowTitle);
            if (hWndRet != IntPtr.Zero)
                hWndRet = HWndInfo.FindWindow(className, null);
            if (hWndRet != IntPtr.Zero)
                hWndRet = HWndInfo.FindWindow("", windowTitle);

            return hWndRet;
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
