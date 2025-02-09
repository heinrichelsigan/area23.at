using Area23.At.Framework.Core;
using Area23.At.Framework.Core.Win32Api;
using Area23.At.WinForm.SecureChat.Entities;
using System;
using System.Reflection;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Area23.At.WinForm.SecureChat
{
    internal static class Program
    {
        internal static string progName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        private static Mutex mutex = new Mutex(true, progName);

        internal static Mutex PMutec
        {
            get => mutex;
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!mutex.WaitOne(1000, false))
            {                
                NativeWrapper.Kernel32.AttachConsole(NativeWrapper.Kernel32.ATTACH_PARENT_PROCESS);
                // Area23.At.Framework.Library.Area23Log.Logger.LogOriginMsg(roachName, $"Another instance of {roachName} is already running!");
                Console.Out.WriteLine($"Another instance of {progName} is already running!");
                MessageBox.Show($"Another instance of {progName} is already running!", $"{progName}: multiple startup!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);
            ApplicationConfiguration.Initialize();
            Form newChat = (Form)(new Gui.Forms.SecureChat());
            Application.Run(newChat);
            
            ReleaseCloseDisposeMutex(mutex);

        }



        /// <summary>
        /// Release Mutax exclusion, that not 2 chat programs could be started at same machine
        /// </summary>
        /// <param name="mutex"></param>
        internal static void ReleaseCloseDisposeMutex(Mutex? mutex)
        {
            Exception? ex = null;            
            
            if (mutex != null)
            {
                var safeWaitHandle = mutex.GetSafeWaitHandle();
                if (safeWaitHandle != null && !safeWaitHandle.IsInvalid && !safeWaitHandle.IsClosed)
                {
                    try
                    {
                        mutex.ReleaseMutex();
                    }
                    catch (Exception exRelease)
                    {
                        Area23Log.LogStatic(exRelease);
                        CqrException.LastException = exRelease;
                        ex = exRelease;
                    }
                    try
                    {
                        mutex.Close();
                    }
                    catch (Exception exClose)
                    {
                        Area23Log.LogStatic(exClose);
                        CqrException.LastException = exClose;
                        ex = exClose;
                    }
                    try
                    {
                        mutex.Dispose();
                    }
                    catch (Exception exDispose)
                    {
                        Area23Log.LogStatic(exDispose);
                        CqrException.LastException = exDispose;
                        ex = exDispose;
                    }

                }
            }
            try
            {
                mutex = null;
            }
            catch (Exception exNull)
            {
                Area23Log.LogStatic(exNull);
                CqrException.LastException = exNull;
                ex = exNull;
            }
            finally
            {                
                if (ex != null)
                {                    
                    throw ex;
                }                
            }

            return ;
        }

    }

}