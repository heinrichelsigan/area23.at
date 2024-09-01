using Area23.At.Framework.Library.Core;
using Area23.At.Framework;
using Area23.At.Framework.Library.Core.Win32Api;
using System.Reflection;


namespace Area23.At.WinForm.TWinFormCore
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            string tFormName = System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);
            Mutex mutex = new Mutex(false, tFormName);

            if (!mutex.WaitOne(1000, false))
            {
                NativeMethods.Kernel32.AttachConsole(NativeMethods.Kernel32.ATTACH_PARENT_PROCESS);
                // Area23.At.Framework.Library.Area23Log.Logger.LogOriginMsg(roachName, $"Another instance of {roachName} is already running!");
                Console.Out.WriteLine($"Another instance of {tFormName} is already running!");
                MessageBox.Show($"Another instance of {tFormName} is already running!", $"{tFormName}: multiple startup!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            // MessageBox.Show("ScreenCapture", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            Application.Run(new TransparentFormCore8());

        }
    }
}