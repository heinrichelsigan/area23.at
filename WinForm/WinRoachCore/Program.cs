using Area23.At.Framework.Library.Core.Win32Api;
using System.Reflection;

namespace Area23.At.WinForm.WinRoachCore
{
    internal static class Program
    {
        internal static string progName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        internal static Mutex? mutex;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string roachName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            int roachNum = 0;
            if (args != null && args.Length > 0)
            {
                if (!Int32.TryParse(args[0], out roachNum))
                    roachNum = 0;                 
            }
            roachName += roachNum.ToString();

            mutex = new Mutex(false, roachName);
            if (roachNum > 3 || !mutex.WaitOne(1200, false))
            {
                NativeWrapper.Kernel32.AttachConsole(NativeWrapper.Kernel32.ATTACH_PARENT_PROCESS);
                Area23.At.Framework.Library.Core.Area23Log.Logger.LogOriginMsg(roachName, $"Another instance of {roachName} is already running!");
                Console.Out.WriteLine($"Another instance of {roachName} is already running!");
                MessageBox.Show($"Another instance of {roachName} is already running!", $"{roachName} multiple startup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new RoachBase(roachNum));

            ReleaseCloseDisposeMutex();
        }


        internal static void ReleaseCloseDisposeMutex()
        {
            Exception? ex = null;
            if (Program.mutex != null)
            {
                var safeWaitHandle = Program.mutex.GetSafeWaitHandle();
                if (safeWaitHandle != null && !safeWaitHandle.IsInvalid && !safeWaitHandle.IsClosed)
                {
                    try
                    {
                        Program.mutex.ReleaseMutex();
                    }
                    catch (Exception exRelease)
                    {
                        ex = exRelease;
                        NativeWrapper.Kernel32.AttachConsole(NativeWrapper.Kernel32.ATTACH_PARENT_PROCESS);
                        Console.Out.WriteLine($"{progName} exception when releasing mutex: {exRelease.Message}\r\n{exRelease.ToString()}\r\n{exRelease.StackTrace}\r\n");
                    }
                    try
                    {
                        Program.mutex.Close();
                    }
                    catch (Exception exClose)
                    {
                        if (ex == null)
                            NativeWrapper.Kernel32.AttachConsole(NativeWrapper.Kernel32.ATTACH_PARENT_PROCESS);
                        Console.Out.WriteLine($"{progName} exception when closing mutex: {exClose.Message}\r\n{exClose.ToString()}\r\n{exClose.StackTrace}\r\n");
                        ex = exClose;
                    }
                    try
                    {
                        Program.mutex.Dispose();
                    }
                    catch (Exception exDispose)
                    {
                        if (ex == null)
                            NativeWrapper.Kernel32.AttachConsole(NativeWrapper.Kernel32.ATTACH_PARENT_PROCESS);
                        Console.Out.WriteLine($"{progName} exception when disposing mutex: {exDispose.Message}\r\n{exDispose.ToString()}\r\n{exDispose.StackTrace}\r\n");
                        ex = exDispose;
                    }

                }
            }
            try
            {
                Program.mutex = null;
            }
            catch (Exception exNull)
            {
                if (ex == null)
                    NativeWrapper.Kernel32.AttachConsole(NativeWrapper.Kernel32.ATTACH_PARENT_PROCESS);
                Console.Out.WriteLine($"{progName} exception when setting mutex to NULL: {exNull.Message}\r\n{exNull.ToString()}\r\n{exNull.StackTrace}\r\n");
                ex = exNull;
            }
            finally
            {
                if (ex != null)
                    throw ex;
            }
        }
    }
}