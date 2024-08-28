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
        /// <summary>
        /// main entry point for classic .NET 4.8.x
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RoachBase());
        }
    }
}
