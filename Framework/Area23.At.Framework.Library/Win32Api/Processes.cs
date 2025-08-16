using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Area23.At.Framework.Library.Win32Api
{

    /// <summary>
    /// Processes - a windows process Library using Win32 Api
    /// Thanks to <see href="https://github.com/dotnet">github.com/dotnet</see>,
    /// <see href="https://stackoverflow.com/">stackoverflow.com/</see>,
    /// <see href="https://www.pinvoke.net/">pinvoke.net</see> and
    /// <see cref="https://www.codeproject.com/">cpdeproject.com</see>
    /// </summary>
    public static class Processes
    {
        #region DllImport
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

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

        #region GetProcess       

        /// <summary>
        /// Checks if a certain process is currently running  
        /// </summary>
        /// <param name="pid">process identifier</param>
        /// <returns>true, if process <see cref="Process.GetP(pid)"/>with> </returns>
        public static bool ProcessRuns(int pid)
        {
            Process process;
            try
            {
                process = Process.GetProcessById(pid);
            }
            catch (Exception exProc)
            {
                process = null;
                Area23Log.LogOriginMsgEx($"bool Processes.ProcessRuns(int pid = {pid})",
                    $"Error on fetching process for pid {pid}.", exProc);
            }

            return (process != null && !process.HasExited);
        }

        /// <summary>
        /// GetProcessById gets the <see cref="System.Diagnostics.Process"/> by pid
        /// </summary>
        /// <param name="pid">process identifier</param>
        /// <returns><see cref="System.Diagnostics.Process"/></returns>
        public static Process GetProcessById(int pid)
        {
            Process process;
            try
            {
                process = Process.GetProcessById(pid);
            }
            catch (Exception exProc)
            {
                process = null;
                Area23Log.LogOriginMsgEx($"Processes.GetProcessById(int pid = {pid})",
                    $"Error on fetching process for pid {pid}.", exProc);
            }
            return (process != null && !process.HasExited) ? process : null;
        }

        /// <summary>
        /// GetProcessByHwnd finds process belongig to a window handle
        /// </summary>
        /// <param name="hwnd">window handle</param>
        /// <returns><see cref="System.Diagnostics.Process"/></returns>
        public static Process GetProcessByHwnd(IntPtr hwnd)
        {
            uint uHwndPid;

            try
            {
                if (hwnd != IntPtr.Zero)
                {
                    GetWindowThreadProcessId(hwnd, out uHwndPid);
                    if (uHwndPid > 0)
                    {
                        return GetProcessById((int)uHwndPid);
                    }
                }
            }
            catch (Exception exProcHwnd)
            {
                Area23Log.LogOriginMsgEx($"Processes.GetProcessByHwnd(IntPtr hwnd = {hwnd})",
                    $"Error on fetching process for window handle {hwnd}.", exProcHwnd);
            }
            return null;
        }

        /// <summary>
        /// GetRunningProcessesByName gets all running processes 
        /// </summary>
        /// <param name="processName">executable name</param>
        /// <returns><see cref="Process[]">Array of Process</see></returns>
        public static Process[] GetRunningProcessesByName(string processName)
        {
            HashSet<Process> processHash = new HashSet<Process>();
            foreach (Process p in Process.GetProcessesByName(processName))
            {
                try
                {
                    if (p != null && !p.HasExited && !processHash.Contains(p))
                        processHash.Add(p);
                }
                catch (Exception exAddProcByName)
                {
                    Area23Log.LogOriginMsgEx($"Processes.GetRunningProcessesByName(string processName = {processName}, string windowTitle = {p.MainWindowTitle})",
                        $"Exception on adding processes by name = {processName} to process array.", exAddProcByName);
                }
            }

            return processHash.ToArray();
        }

        /// <summary>
        /// GetProcessByNameAndWindowTitle gets processes with name and window title
        /// </summary>
        /// <param name="processName">name of process</param>
        /// <param name="windowTitle"window title of executable></param>
        /// <returns><see cref="Process[]"/></returns>
        public static Process[] GetProcessByNameAndWindowTitle(string processName, string windowTitle)
        {
            HashSet<Process> processHash = new HashSet<Process>();
            foreach (Process p in Process.GetProcessesByName(processName))
            {
                try
                {
                    if (p != null && !p.HasExited && p.MainWindowTitle.Equals(windowTitle, StringComparison.InvariantCultureIgnoreCase) && !processHash.Contains(p))
                        processHash.Add(p);
                }
                catch (Exception exAddProcByName)
                {
                    Area23Log.LogOriginMsgEx($"Processes.GetRunningProcessesByName(string processName = {processName}, string windowTitle = {windowTitle})",
                        $"Exception on adding processes by name = {processName} window title {windowTitle}", exAddProcByName);
                }
            }

            return (processHash != null && processHash.Count > 0) ? processHash.ToArray() : null;
        }

        #region ParentProcess

        /// <summary>
        /// Returns the parent process id for the specified process.
        /// Returns zero if it cannot be gotten for some reason.
        /// </summary>
        /// <param name="pid">process identifier</param>
        /// <returns>parent process identifier</returns>
        internal static int GetParentProcessId(int pid)
        {
            return NativeWrapper.GetParentProcessId(pid);
        }

        /// <summary>
        /// GetParentProcessById gets parent process by pid
        /// </summary>
        /// <param name="pid">process identifier</param>
        /// <returns><see cref="System.Diagnostics.Process"/></returns>
        public static Process GetParentProcessById(int pid)
        {
            int pPid = NativeWrapper.GetParentProcessId(pid);
            Process pProcess = Process.GetProcessById(pPid);
            return (pProcess != null && !pProcess.HasExited) ? pProcess : null;
        }

        #endregion ParentProcess

        #region ChildProcess

        /// <summary>
        /// GetChildIdNames 
        /// </summary>
        /// <param name="pPid"><see cref="int"/> parent process identifier pPid</param>        
        /// <param name="addParent">true, if to add parent id and process name too, default false</param>
        /// <returns><see cref="Dictionary{int, string}"/> parent_child_ProcessMap</returns>
        public static Dictionary<int, string> GetChildIdNames(int pPid, bool addParent = false)
        {
            string methodSignature = $"Processes.GetChildIdNames(int pPid = {pPid}, bool addParent = {addParent})";
            Dictionary<int, string> parentChildProcessDict = new Dictionary<int, string>();
            Process parentProcess, childProcess;
            try
            {
                parentProcess = Process.GetProcessById(pPid);
            }
            catch (Exception getProcEx)
            {
                Area23Log.LogOriginMsgEx(methodSignature, $"Exception in getting process with id [{pPid}].", getProcEx);
                return parentChildProcessDict;
            }

            if (parentProcess == null || parentProcess.HasExited)
            {
                Area23Log.LogOriginMsg(methodSignature, $"Process with id [{pPid}] is null or has already exited.");
                return parentChildProcessDict;
            }

            if (addParent)
            {
                parentChildProcessDict.Add(pPid, parentProcess.ProcessName);
                Area23Log.LogOriginMsg(methodSignature, $"Added parent process <[pPid = {pPid}], [ppName = {parentProcess.ProcessName}]> to  Dictionary<int, string>().");
            }

            DateTime parentStartTime = parentProcess.StartTime;

            // processList = NativeMethods.GetChildProcessIds(pPid, parentStartTime);
            List<KeyValuePair<int, NativeWrapper.SafeProcessHandle>> processList = NativeWrapper.GetChildProcessIds(pPid, parentStartTime);

            foreach (var elem in processList)
            {
                try
                {
                    childProcess = Process.GetProcessById(elem.Key);
                    if (childProcess != null && !childProcess.HasExited)
                    {
                        parentChildProcessDict.Add(elem.Key, childProcess.ProcessName);
                        Area23Log.LogOriginMsg(methodSignature, $"Added child process <[cPid = {elem.Key}], [cpName = {childProcess.ProcessName}]> to  Dictionary<int, string>().");
                    }
                }
                catch (Exception getChildProcEx)
                {
                    Area23Log.LogOriginMsgEx(methodSignature, $"Exception in getting child process with id [{elem.Key}].", getChildProcEx);
                }
            }

            return parentChildProcessDict;
        }

        /// <summary>
        /// ListChildProcesses list all child processes belonging to a parent process identifier pPid
        /// </summary>
        /// <param name="pPid"><see cref="int"/> parent process identifier pPid</param>
        /// <param name="addParent">true, if to add parent id and process name too, default false</param>
        /// <returns><see cref="Process[]"/> array of <see cref="Process"/></returns>
        public static Process[] ListChildProcesses(int pPid, bool addParent = false)
        {
            string methodSignature = $"Processes.ListChildProcesses(int pPid = {pPid}, bool addParent = {addParent})";
            HashSet<Process> parentChildProcessHash = new HashSet<Process>();

            Process parentProcess, childProcess;
            try
            {
                parentProcess = Process.GetProcessById(pPid);
            }
            catch (Exception getProcEx)
            {
                Area23Log.LogOriginMsgEx(methodSignature, $"Exception in getting process with id [{pPid}].", getProcEx);
                return parentChildProcessHash.ToArray();
            }

            if (parentProcess == null || parentProcess.HasExited)
            {
                Area23Log.LogOriginMsg(methodSignature, $"Process with id [{pPid}] is null or has already exited.");
                return parentChildProcessHash.ToArray();
            }

            if (addParent) // add parent to HashSet  
            {
                parentChildProcessHash.Add(parentProcess);
                Area23Log.LogOriginMsg(methodSignature, $"Added parent process \"{parentProcess.ProcessName}\" with id [{pPid}] to HashSet<int> parentChildProcessHash.");
            }

            DateTime parentStartTime = parentProcess.StartTime;

            // GetChildProcessIds
            List<KeyValuePair<int, NativeWrapper.SafeProcessHandle>> processHandleList = NativeWrapper.GetChildProcessIds(pPid, parentStartTime);

            foreach (var elem in processHandleList)
            {
                try
                {
                    childProcess = Process.GetProcessById(elem.Key);
                    if (childProcess != null && !childProcess.HasExited && !parentChildProcessHash.Contains(childProcess))
                    {
                        // Add child to HashSet
                        parentChildProcessHash.Add(childProcess);
                        Area23Log.LogOriginMsg(methodSignature, $"Added child process[Id = {elem.Key}, ProcesName = {childProcess.ProcessName}, ...] to HashSet<int> parentChildProcessHash[{parentChildProcessHash.Count}].");
                    }
                }
                catch (Exception getChildProcEx)
                {
                    Area23Log.LogOriginMsgEx(methodSignature, $"Exception in getting child process with id [{elem.Key}].", getChildProcEx);
                }
            }

            return parentChildProcessHash.ToArray();
        }

        /// <summary>
        /// GetChildPids get an <see cref="int[]">int[] array</see> of all child process identifiers from a parent process indentifier
        /// </summary>
        /// <param name="pPid"><see cref="int"/> parent process identifier pPid</param>
        /// <param name="addParent">true, if to add parent id and process name too, default false</param>
        /// <returns><see cref="int[]"/> array with all child process id's</returns>
        public static int[] GetChildPids(int pPid, bool addParent = false)
        {
            Dictionary<int, string> childIdNames = GetChildIdNames(pPid, addParent);
            return childIdNames.Keys.ToArray();
        }

        #endregion ChildProcess

        #region ProcessOwner

        /// <summary>
        /// GetProcessOwnerById - gets process owning windows user, by process identifier
        /// </summary>
        /// <param name="pid">process identifier</param>
        /// <returns>user, that owns process</returns>
        public static string GetProcessOwnerById(int pid)
        {
            IntPtr processHandle = IntPtr.Zero;
            IntPtr tokenHandle = IntPtr.Zero;
            try
            {
                processHandle = OpenProcess(PROCESS_QUERY_INFORMATION, false, pid);
                if (processHandle == IntPtr.Zero)
                    return "NO ACCESS";

                OpenProcessToken(processHandle, TOKEN_QUERY, out tokenHandle);
                using (WindowsIdentity wi = new WindowsIdentity(tokenHandle))
                {
                    string user = wi.Name;
                    return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
                }
            }
            finally
            {
                if (tokenHandle != IntPtr.Zero) CloseHandle(tokenHandle);
                if (processHandle != IntPtr.Zero) CloseHandle(processHandle);
            }
        }

        #endregion ProcessOwner

        #endregion GetProcess

        #region KillProcess

        /// <summary>
        /// KillProcess - kills process with process identifier pid 
        /// </summary>
        /// <param name="pid"><see cref="int">int pid</see> process identifier</param>
        /// <param name="ignoreWin32SystemProcesses">if true, Win32 system processes and their childs will not be killed</param>
        public static void KillProcess(int pid, bool ignoreWin32SystemProcesses = true)
        {
            string methodSignature = $"Processes.KillProcess(int pid = {pid}, bool ignoreWin32SystemProcesses = {ignoreWin32SystemProcesses})";
            Process process = null;
            try
            {
                process = Process.GetProcessById(pid);
            }
            catch (Exception exGetProcess)
            {
                Area23Log.LogOriginMsgEx(methodSignature, $"Exception in getting process with id [{pid}].", exGetProcess);
                return;
            }

            if (process == null || process.HasExited)
            {
                Area23Log.LogOriginMsg(methodSignature, $"Aborting tree recursion, because process with id [{pid}] is null or has already exited.");
                return;
            }

            if (ignoreWin32SystemProcesses && string.IsNullOrEmpty(process.ProcessName))
            {
                foreach (string winSystemProcName in Constants.EXE_WIN_SYSTEM)
                {
                    if (process.ProcessName.Equals(winSystemProcName, StringComparison.InvariantCultureIgnoreCase) ||
                        process.ProcessName.StartsWith(winSystemProcName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Area23Log.LogOriginMsg(methodSignature, $"Ignoring win32 system process \"{process.ProcessName}\" with id [{pid}]  window title '{process.MainWindowTitle}'.");
                        return;
                    }
                }
            }

            try
            {
                Area23Log.LogOriginMsg(methodSignature, $"Trying to kill process \"{process.ProcessName}\" with id [{pid}].");
                process.Kill();
            }
            catch (Exception killProcEx)
            {
                Area23Log.LogOriginMsgEx(methodSignature, $"Exception in killing process \"{process.ProcessName}\" with id [{pid}].", killProcEx);
            }

            if (process != null)
            {
                try
                {
                    process.Dispose();
                }
                catch (Exception disposeProcEx)
                {
                    Area23Log.LogOriginMsgEx(methodSignature, $"Exception in diposing process with id [{pid}].", disposeProcEx);
                }
            }
        }

        /// <summary>
        /// Kills the specified process by id and all of its children recursively.
        /// </summary>
        [Obsolete("KillTree(int pPidToKill) is obsolete, use better new implementation static int KillProcessTree(int pid, bool killParent = true, int psKilled = 0, bool ignoreWin32SystemProcesses = true).", false)]
        public static void KillTree(int pPidToKill)
        {
            NativeWrapper.KillTree(pPidToKill);
        }

        /// <summary>
        /// KillProcessTree kill all child processes recursivly including parent process id
        /// </summary>
        /// <param name="pid">pid process identifier</param>        
        /// <param name="killParent">kill parent process (and not only childs) default to <see cref="true"/></param>
        /// <param name="psKilled">counter for processes already killed in current recursion tree</param>
        /// <param name="ignoreWin32SystemProcesses">if true, Win32 system processes and their childs will not be killed</param>
        /// <returns>number of processes totally killed in current recursive tree</returns>
        public static int KillProcessTree(int pid, bool killParent = true, int psKilled = 0, bool ignoreWin32SystemProcesses = true)
        {
            string methodSignature = $"Processes.KillProcessTree(int pid = {pid}, bool killParent = {killParent}, int psKilled = {psKilled}, bool ignoreWin32SystemProcesses = {ignoreWin32SystemProcesses})";
            Process process = null;
            try
            {
                process = Process.GetProcessById(pid);
            }
            catch (Exception exGetProcess)
            {
                Area23Log.LogOriginMsgEx(methodSignature, $"Exception in getting process with id [{pid}].", exGetProcess);
                return psKilled;
            }

            if (process == null || process.HasExited)
            {
                Area23Log.LogOriginMsg(methodSignature, $"Aborting tree recursion, because process with id [{pid}] is null or has already exited.");
                return psKilled;
            }

            string processName = process.ProcessName;

            if (ignoreWin32SystemProcesses && string.IsNullOrEmpty(processName))
            {
                foreach (string winSystemProcName in Constants.EXE_WIN_SYSTEM)
                {
                    if (processName.Equals(winSystemProcName, StringComparison.InvariantCultureIgnoreCase) ||
                        processName.StartsWith(winSystemProcName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Area23Log.LogOriginMsg(methodSignature, $"Ignoring win32 system process \"{processName}\" with id [{pid}]  window title '{process.MainWindowTitle}'.");
                        return psKilled;
                    }
                }
            }

            var childs = ListChildProcesses(pid);
            if (childs != null && childs.Length > 0)
            {
                Area23Log.LogOriginMsg(methodSignature, $"Found {childs.Length} child processes for process \"{processName}\" with id [{pid}] window title '{process.MainWindowTitle}'.");
                foreach (Process childProcess in childs)
                {
                    // kill child process tree, if it's a system process it will be aborted in the next recursion loop
                    psKilled += KillProcessTree(childProcess.Id, true, psKilled, ignoreWin32SystemProcesses);
                }
            }


            if (killParent)
            {
                try
                {
                    Area23Log.LogOriginMsg(methodSignature, $"Trying to kill process \"{processName}\" with id [{pid}].");
                    process.Kill();
                    psKilled++;
                }
                catch (Exception killProcEx)
                {
                    Area23Log.LogOriginMsgEx(methodSignature, $"Exception in killing process \"{processName}\" with id [{pid}].", killProcEx);
                }
            }
            else
            {
                Area23Log.LogOriginMsg(methodSignature, $"Ignoring killing parent process \"{processName}\" with id [{pid}].");
            }

            if (process != null)
            {
                try
                {
                    process.Dispose();
                }
                catch (Exception disposeProcEx)
                {
                    Area23Log.LogOriginMsgEx(methodSignature, $"Exception in diposing process with id [{pid}].", disposeProcEx);
                }
            }

            return psKilled;
        }

        #endregion KillProcess

    }

}
