using System;
using System.Diagnostics;

namespace Area23.At.Mono.Util
{
    /// <summary>
    /// static class for processing shell command
    /// </summary>
    public static class ProcessCmd
    {
        /// <summary>
        /// Execute a binary or shell cmd
        /// </summary>
        /// <param name="filepath">full or relative filepath to executable</param>
        /// <param name="args">arguments passed to executable</param>
        /// <param name="useShellExecute">set Process.StartInfo.UseShellExecute</param>
        /// <returns>standard output of process pexecec it.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string Execute(string filepath = "SystemInfo", string args = "", bool useShellExecute = false)
        {
            string consoleError = "", consoleOutput = "";
            Area23Log.LogStatic(String.Format("ProcessCmd.Execute(filepath = ${0}, args = {1}, useShellExecute = {2}) called ...",
                filepath, args, useShellExecute));
            try
            {                
                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = filepath;
                    compiler.StartInfo.CreateNoWindow = true;
                    string argTrys = (!string.IsNullOrEmpty(args)) ? args : "";
                    compiler.StartInfo.Arguments = argTrys;
                    compiler.StartInfo.UseShellExecute = useShellExecute;
                    compiler.StartInfo.RedirectStandardError = true;
                    compiler.StartInfo.RedirectStandardOutput = true;
                    compiler.Start();

                    consoleOutput = compiler.StandardOutput.ReadToEnd();
                    consoleError = compiler.StandardError.ReadToEnd();

                    compiler.WaitForExit();
                }
            }
            catch (Exception exi)
            {
                Area23Log.LogStatic("ProcessCmd.Execute Exception: " + exi.Message);
                throw new InvalidOperationException($"can't perform {filepath} {args}\nStdErr = {consoleError}", exi);
            }

            if (!string.IsNullOrEmpty(consoleError))
                Area23Log.LogStatic("ProcessCmd.Execute consoleError: " + consoleError);
            Area23Log.LogStatic("ProcessCmd.Execute consoleOutput: " + consoleOutput);

            return consoleOutput;
        }
    }
}