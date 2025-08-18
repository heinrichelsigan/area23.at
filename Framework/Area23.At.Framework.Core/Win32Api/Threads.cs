using Area23.At.Framework;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Win32Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Win32Api
{

    /// <summary>
    /// Threads - a windows Threads Library using Win32 Api
    /// Thanks to <see href="https://github.com/dotnet">github.com/dotnet</see>,
    /// <see href="https://stackoverflow.com/">stackoverflow.com/</see>,
    /// <see href="https://www.pinvoke.net/">pinvoke.net</see> and
    /// <see cref="https://www.codeproject.com/">cpdeproject.com</see>
    /// </summary>
    public static class Threads
    {
        
        #region DllImport

        //#region DllImport
        //[DllImport("user32.dll", SetLastError = true)]
        //internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("coredll.dll")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);


        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        private const UInt32 STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        private const UInt32 SYNCHRONIZE = 0x00100000;

        private const UInt32 PROCESS_TERMINATE = 0x0001;
        private const UInt32 PROCESS_CREATE_THREAD = 0x0002;
        private const UInt32 PROCESS_SET_SESSIONID = 0x0004;
        private const UInt32 PROCESS_VM_OPERATION = 0x0008;
        private const UInt32 PROCESS_VM_READ = 0x0010;
        private const UInt32 PROCESS_VM_WRITE = 0x0020;
        private const UInt32 PROCESS_DUP_HANDLE = 0x0040;
        private const UInt32 PROCESS_CREATE_PROCESS = 0x0080;
        private const UInt32 PROCESS_SET_QUOTA = 0x0100;
        private const UInt32 PROCESS_SET_INFORMATION = 0x0200;
        private const UInt32 PROCESS_QUERY_INFORMATION = 0x0400;
        private const UInt32 PROCESS_SUSPEND_RESUME = 0x0800;
        private const UInt32 PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF;

        private const UInt32 TOKEN_ASSIGN_PRIMARY = 0x0001;
        private const UInt32 TOKEN_DUPLICATE = 0x0002;
        private const UInt32 TOKEN_IMPERSONATE = 0x0004;
        private const UInt32 TOKEN_QUERY = 0x0008;
        private const UInt32 TOKEN_QUERY_SOURCE = 0x0010;
        private const UInt32 TOKEN_ADJUST_PRIVILEGES = 0x0020;
        private const UInt32 TOKEN_ADJUST_GROUPS = 0x0040;
        private const UInt32 TOKEN_ADJUST_DEFAULT = 0x0080;
        private const UInt32 TOKEN_ADJUST_SESSIONID = 0x0100;
        
        #endregion DllImport

        #region GetThread       

        /// <summary>
        /// Checks if a certain process is currently running  
        /// </summary>
        /// <param name="pid">process identifier</param>
        /// <returns>true, if process <see cref="Process.GetP(pid)"/>with> </returns>
        public static int GetCurrentThreadId()
        {
            int threadId = -1;

            try
            {
                threadId = (int)NativeWrapper.GetCurrentThreadId();
            }
            catch (Exception exThread)
            {
                threadId = -1;
                Area23Log.LogOriginMsgEx($"bool Processes.ProcessRuns(int threadId = {threadId})",
                    $"Error on fetching process for pid {threadId}.", exThread);
            }

            return threadId;
        }

        /// <summary>
        /// GetThreadIdProcessId gets threadId and processId by
        /// window handle
        /// </summary>
        /// <param name="hWnd"><see cref="IntPtr" /> window handle</param>
        /// <param name="upid">unsigned int process identifier</param>
        /// <returns>uint threadId</returns>
        public static uint GetThreadIdProcessId(IntPtr hWnd, out uint upid)
        {
            uint threadId = Threads.GetWindowThreadProcessId((IntPtr)hWnd, out upid);
            return threadId;
        }

        #endregion GetThread

    }

}
