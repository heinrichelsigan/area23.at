using System;
using System.Diagnostics;

namespace Area23.At.Framework.Core.Win32Api
{

    /// <summary>
    /// ProcessCmd static class for running an executable or processing shell command
    /// <see cref="https://github.com/heinrichelsigan/area23.at/blob/main/Framework/Library/ProcessCmd.cs">ProcessCmd.cs at github.com/heinrichelsigan</see>
    /// ProcessCmd class is free software; 
    /// you can redistribute it and/or modify it under the terms of the GNU Library General Public License 
    /// aspublished by the Free Software Foundation; 
    /// either <seealso cref="https://www.gnu.org/licenses/old-licenses/gpl-2.0.html">version 2</seealso> 
    /// of the License, or (at your option) any later version.
    /// See the GNU Library General Public License for more details.    
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
                Area23Log.LogStatic("ProcessCmd.Execute throwed Exception: " + exi.Message);
                Area23Log.LogStatic($"can't perform {filepath} {args}\nStdErr = {consoleError}\tException: {exi}");
                throw new InvalidOperationException($"can't perform {filepath} {args}\nStdErr = {consoleError}", exi);
            }

            if (!string.IsNullOrEmpty(consoleError))
                Area23Log.LogStatic("ProcessCmd.Execute consoleError: " + consoleError);
            Area23Log.LogStatic("ProcessCmd.Execute consoleOutput: " + consoleOutput);

            return consoleOutput;
        }

        /// <summary>
        /// Execute a binary or shell cmd
        /// </summary>
        /// <param name="cmdPath">full or relative filepath to executable</param>
        /// <param name="args"><see cref="string[]">string[] args</see> passed to executable</param>
        /// <param name="quoteArgs"><see cref="bool">bool quoteArgs</see> set each single argument under double quote, when passing it to cmdPath</param>
        /// <param name="useShellExecute"><see cref="bool">bool useShellExecute</see> true, when using system shell to execute cmdPath</param>
        /// <returns>Console output of executed command</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string Execute(string cmdPath = "SystemInfo", string[] args = null, bool quoteArgs = false, bool useShellExecute = false)
        {
            string consoleError = string.Empty, consoleOutput = string.Empty, arguments = string.Empty;
            if (args != null && args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (!string.IsNullOrEmpty(arguments))
                        arguments += " ";
                    if (quoteArgs && !arg.StartsWith("\"") && !arg.EndsWith("\""))
                        arguments += $"\"{arg}\"";
                    else
                        arguments += arg;
                }
            }

            Area23Log.LogStatic(string.Format("ProcessCmd.Execute(cmdPath = ${0}, args = {1}, quoteArgs = {2}, useShellExecute = {3}) called ...",
                    cmdPath, arguments, quoteArgs, useShellExecute));
            try
            {
                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = cmdPath;
                    compiler.StartInfo.CreateNoWindow = true;
                    compiler.StartInfo.Arguments = arguments;
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
                Area23Log.LogStatic($"ProcessCmd.Execute {cmdPath} {arguments}\tException: {exi.Message}");
                Area23Log.LogStatic($"can't perform {cmdPath} {arguments}\nStdErr = {consoleError}\tException: {exi}");
                throw new InvalidOperationException($"can't perform {cmdPath} {arguments}\nStdErr = {consoleError}\tException: {exi}");
            }

            if (!string.IsNullOrEmpty(consoleError))
                Area23Log.LogStatic($"ProcessCmd.Execute {cmdPath} {arguments} consoleError: " + consoleError);
            Area23Log.LogStatic($"ProcessCmd.Execute {cmdPath} {arguments} consoleOutput: " + consoleOutput);

            return consoleOutput;
        }

    }

}
