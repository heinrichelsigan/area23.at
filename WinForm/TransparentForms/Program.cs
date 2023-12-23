using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.TransparentForms
{
    internal static class Program
    {
        /// <summary>
        /// main entry point for classic .NET 4.8.x
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TForm());
        }
    }
}
