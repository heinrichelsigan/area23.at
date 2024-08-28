using Area23.At.Framework.Library;
using Area23.At.Framework;


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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);

            // MessageBox.Show("ScreenCapture", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            Application.Run(new TWinForm());

        }
    }
}