using Area23.At.Framework.Library.Win32Api;
using System.Reflection;

namespace Area23.At.WinForm.WinRoachCore
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string roachName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            Mutex mutex = new Mutex(false, roachName);

            if (!mutex.WaitOne(1000, false))
            {
                NativeMethods.Kernel32.AttachConsole(NativeMethods.Kernel32.ATTACH_PARENT_PROCESS);
                // Area23.At.Framework.Library.Area23Log.Logger.LogOriginMsg(roachName, $"Another instance of {roachName} is already running!");
                Console.Out.WriteLine($"Another instance of {roachName} is already running!");
                MessageBox.Show($"Another instance of {roachName} is already running!", $"{roachName}: multiple startup!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new RoachBase());
        }
    }
}