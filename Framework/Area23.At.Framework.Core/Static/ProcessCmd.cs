using Area23.At.Framework.Core.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Static
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
        /// <param name="arguments">arguments passed to executable</param>
        /// <param name="useShellExecute">set Process.StartInfo.UseShellExecute</param>
        /// <returns>standard output of process pexecec it.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string Execute(string filepath = "SystemInfo", string arguments = "", bool useShellExecute = false)
        {
            string consoleError = "", consoleOutput = "", args = !string.IsNullOrEmpty(arguments) ? arguments : "";
            Area23Log.LogOriginMsg("ProcessCmd", string.Format("ProcessCmd.Execute(filepath = ${0}, args = {1}, useShellExecute = {2}) called ...",
                filepath, args, useShellExecute));
            try
            {
                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = filepath;
                    compiler.StartInfo.CreateNoWindow = true;
                    compiler.StartInfo.Arguments = args;
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
                Area23Log.LogOriginMsgEx("ProcessCmd", $"can't execute: {filepath} {args}\n\tStdErr = {consoleError}", exi); 
                throw new InvalidOperationException($"can't execute: {filepath} {args} \tStdErr = {consoleError}", exi);
            }

            Area23Log.LogOriginMsg("ProcessCmd", 
                string.Format("ProcessCmd.Execute(filepath = ${0}, args = {1}, useShellExecute = {2}) finished successfull, output length: {3}",
                    filepath, args, useShellExecute, consoleOutput.Length));
            if (!string.IsNullOrEmpty(consoleError))
                Area23Log.LogOriginMsg("ProcessCmd", "ProcessCmd.Execute consoleError: " + consoleError);

            return consoleOutput;
        }

        /// <summary>
        /// Execute a binary or shell cmd
        /// </summary>
        /// <param name="cmdPath">full or relative filepath to executable</param>
        /// <param name="arguments"><see cref="string[]">string[] arguments</see> passed to executable</param>
        /// <param name="quoteArgs"><see cref="bool">bool quoteArgs</see> set each single argument under double quote, when passing it to cmdPath</param>
        /// <param name="useShellExecute"><see cref="bool">bool useShellExecute</see> true, when using system shell to execute cmdPath</param>
        /// <returns>Console output of executed command</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string Execute(string cmdPath = "SystemInfo", string[]? arguments = null, bool quoteArgs = false, bool useShellExecute = false)
        {
            string consoleError = string.Empty, consoleOutput = string.Empty, argStr = string.Empty;
            if (arguments != null && arguments.Length > 0)
            {
                for (int ac = 0; ac < arguments.Length; ac++)
                {
                    if (string.IsNullOrEmpty(arguments[ac]))
                    {
                        if (ac != 0)
                            argStr += " ";
                        if (quoteArgs && !arguments[ac].StartsWith("\"") && !arguments[ac].EndsWith("\""))
                            argStr += $"\"{arguments[ac]}\"";
                        else
                            argStr += arguments[ac];
                    }
                }
            }

            Area23Log.LogOriginMsg("ProcessCmd", 
                string.Format("ProcessCmd.Execute(cmdPath = ${0}, args = {1}, quoteArgs = {2}, useShellExecute = {3}) called ...",
                    cmdPath, argStr, quoteArgs, useShellExecute));
            try
            {
                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = cmdPath;
                    compiler.StartInfo.CreateNoWindow = true;
                    compiler.StartInfo.Arguments = argStr;
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
                string stdErr = string.IsNullOrEmpty(consoleError) ? string.Empty : $"\tStdErr = {consoleError}";
                Area23Log.LogOriginMsg("ProcessCmd", $"ProcessCmd.Execute {cmdPath} {argStr} throwed Exception: {exi.Message}");
                Area23Log.LogOriginMsg("ProcessCmd", $"can't execute: {cmdPath} {argStr} {stdErr}\n\tException: {exi}");
                throw new InvalidOperationException($"can't perform {cmdPath} {argStr} {stdErr}", exi);
            }


            Area23Log.LogOriginMsg("ProcessCmd",
                string.Format("ProcessCmd.Execute(cmdPath = ${0}, arguments = {1}, useShellExecute = {2}) finished successfull, output length: {3}",
                    cmdPath, argStr, useShellExecute, consoleOutput.Length));
            if (!string.IsNullOrEmpty(consoleError))
                Area23Log.LogOriginMsg("ProcessCmd", "ProcessCmd.Execute consoleError: " + consoleError);

            return consoleOutput;
        }

    }
}
