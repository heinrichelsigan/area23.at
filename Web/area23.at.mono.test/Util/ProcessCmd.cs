using System;
using System.Diagnostics;

namespace area23.at.mono.test.Util
{
    public static class ProcessCmd
    {
        public static string Execute(string filepath = "SystemInfo", string args = "")
        {
            string consoleOutput = "";
            try
            {
                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = filepath;
                    string argTrys = (!string.IsNullOrEmpty(args)) ? args : "";
                    compiler.StartInfo.Arguments = argTrys;
                    compiler.StartInfo.UseShellExecute = false;
                    compiler.StartInfo.RedirectStandardOutput = true;
                    compiler.Start();

                    consoleOutput = compiler.StandardOutput.ReadToEnd();

                    compiler.WaitForExit();

                    return consoleOutput;
                }
            }
            catch (Exception exi)
            {
                consoleOutput = $"Exception: {exi.Message}";
            }
            
            return consoleOutput;
        }
    }
}