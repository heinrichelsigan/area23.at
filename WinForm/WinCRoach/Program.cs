using Area23.At.Framework.Library.Win32Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.WinCRoach
{
    internal static class Program
    {
        public static string ProgName { get => System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location); }
        internal static Mutex mutex;

        /// <summary>
        /// main entry point for classic .NET 4.8.x
        /// </summary>
        [STAThread]
        static void Main()
        {
            string roachName = ProgName;
            mutex = new Mutex(false, roachName);

            if (!mutex.WaitOne(1000, false))
            {
                NativeWrapper.Kernel32.AttachConsole(NativeWrapper.Kernel32.ATTACH_PARENT_PROCESS);
                // Area23.At.Framework.Library.Area23Log.Logger.LogOriginMsg(roachName, $"Another instance of {roachName} is already running!");
                Console.Out.WriteLine($"Another instance of {roachName} is already running!");
                MessageBox.Show($"Another instance of {roachName} is already running!", $"{roachName}: multiple startup!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RoachBase());

            ReleaseCloseDisposeMutex();
        }

        internal static void ReleaseCloseDisposeMutex()
        {
            Exception ex = null;
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
                        Console.Out.WriteLine($"{ProgName} exception when releasing mutex: {exRelease.Message}\r\n{exRelease.ToString()}\r\n{exRelease.StackTrace}\r\n");
                    }
                    try
                    {
                        Program.mutex.Close();
                    }
                    catch (Exception exClose)
                    {
                        if (ex == null)
                            NativeWrapper.Kernel32.AttachConsole(NativeWrapper.Kernel32.ATTACH_PARENT_PROCESS);
                        Console.Out.WriteLine($"{ProgName} exception when closing mutex: {exClose.Message}\r\n{exClose.ToString()}\r\n{exClose.StackTrace}\r\n");
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
                        Console.Out.WriteLine($"{ProgName} exception when disposing mutex: {exDispose.Message}\r\n{exDispose.ToString()}\r\n{exDispose.StackTrace}\r\n");
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
                Console.Out.WriteLine($"{ProgName} exception when setting mutex to NULL: {exNull.Message}\r\n{exNull.ToString()}\r\n{exNull.StackTrace}\r\n");
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
