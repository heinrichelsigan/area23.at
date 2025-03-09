# namespace Area23.At.Framework.Core.Win32Api contains Win32 native api calls for user32, kernel32, gdi32, system32 

## NativeWrapper.cs
 -  user32
 -  kernel32
 -  gdi32
 -  system32

## HWndInfo.cs

IntPtr (nint) window handle handler class

 -  public static void CloseWindow(IntPtr hwnd) => SendMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

 -  IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
   _(normal close operation to send a default close message to the window via window handle)_
 -  bool DestroyWindow(IntPtr hWnd);
 -  bool CloseWindowStation(IntPtr hWinsta);
 -  IntPtr FindWindow(string lpClassName, string lpWindowName);
 -  bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);
 -  IntPtr FindChildWindow(ref IntPtr hWndChild, IntPtr hWndParent, string className = "", string windowTitle = "Internet Explorer")
 -  IntPtr FindWindowHandle(string className = "", string windowTitle = "Internet Explorer")
 -  IntPtr GetDesktopWindow()
 -  IntPtr GetTopWindow(IntPtr hWnd)
 -  bool WindowHandleBelongsTo(IntPtr bhWnd, int bhPid)
                   

## Processes.cs

native windows processes mini library with methods like:       

 -  bool ProcessRuns(int pid)
 -  Process GetProcessById(int pid)
 -  Process GetProcessByHwnd(IntPtr hwnd)
 -  Process[] GetRunningProcessesByName(string processName)
 -  Process[] GetProcessByNameAndWindowTitle(string processName, string windowTitle)

 -  int GetParentProcessId(int pid)
 -  Process GetParentProcessById(int pid)

 -  Dictionary<int, string> GetChildIdNames(int pPid, bool addParent = false)
 -  Process[] ListChildProcesses(int pPid, bool addParent = false)
 -  int[] GetChildPids(int pPid, bool addParent = false)

 -  string GetProcessOwnerById(int pid)

 -  KillProcess(int pid, bool ignoreWin32SystemProcesses = true)
 -  KillTree(int pPidToKill)
 -  int KillProcessTree(int pid, bool killParent = true, int psKilled = 0, bool ignoreWin32SystemProcesses = true)

## RegistryAccessor.cs

 -  object? GetRegistryEntry(RegistryHive regHive, string subKeyName, string regName)
 -  void SetRegistryEntry(object value, RegistryHive regHive, string subKeyName, string regName)
 -  bool DeleteSubTree(RegistryHive regHive, string subTree)
 -  async Task<object> GetRegistryEntryAsync(RegistryHive regHive, string subKeyName, string regName)
 -  async void SetRegistryEntryAsync(object value, RegistryHive regHive, string subKeyName, string regName)
  

## ScreenCapture.cs

utility to make via win32 native api screenshots
   - Image CaptureAllDesktops()
   - Image CaptureScreen() => CaptureWindow(NativeWrapper.User32.GetDesktopWindow());
   - Image CaptureDesktopScreen()
   - Image CaptureAllDesktops()
   - Image[] CaptureAllWindows()
   - Image CaptureWindow(IntPtr handle)
   - Image CaptureAllScreen(int x, int y, int width, int height)
   - CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
   - CaptureScreenToFile(string filename, ImageFormat format)
   - Image[] CaptureScreenAndAllWindowsToDirectory(string directory)

## ProcessCmd.cs

executes a windows or dos executable 

   