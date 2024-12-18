using Area23.At.Framework.Library.Core.Win32Api;
using System;
using System.Reflection;
using System.Threading;

namespace Area23.At.WinForm.SecureChat
{
    internal static class Program
    {
        internal static string progName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
        private static Mutex mutex = new Mutex(false, progName);

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
            ApplicationConfiguration.Initialize();
            Form newF = (Form)(new Gui.Forms.SecureChat());
            Application.Run(newF);
            
            Exception[] exceptions;
            ReleaseCloseDisposeMutex(mutex, out exceptions);

        }




        internal static void ReleaseCloseDisposeMutex(Mutex? mutex, out Exception[] exceptions)
        {
            Exception? ex = null;
            List<Exception> exceptionList = new List<Exception>();
            exceptions = exceptionList.ToArray();
            
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
                        exceptionList.Add(exRelease);
                        ex = exRelease;
                    }
                    try
                    {
                        mutex.Close();
                    }
                    catch (Exception exClose)
                    {
                        exceptionList.Add(exClose);
                        ex = exClose;
                    }
                    try
                    {
                        mutex.Dispose();
                    }
                    catch (Exception exDispose)
                    {
                        exceptionList.Add(exDispose);   
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
                exceptionList.Add(exNull);
                ex = exNull;
            }
            finally
            {
                exceptions = exceptionList.ToArray();
                if (ex != null)
                {                    
                    throw ex;
                }                
            }

            return ;
        }

    }

}