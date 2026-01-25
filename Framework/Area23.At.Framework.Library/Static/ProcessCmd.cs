using Area23.At.Framework.Library.Util;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace Area23.At.Framework.Library.Static
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
        internal static readonly string CMD_REGULAR_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-+*/^@%:~,_ \t\v" + Path.DirectorySeparatorChar;
        internal static readonly string ARGS_STRICT_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-+*/^@%:~,_ \t\v" + Path.DirectorySeparatorChar;

        /// <summary>
        /// Sanitize cmd or arguments to prevent injection attacks
        /// </summary>
        /// <param name="cmdArgs">cmd or arguments</param>
        /// <param name="isCmd">true, if cmdArgs is executing cmd, false if cmdArgs are arguments of command</param>
        /// <returns>sanitized string without illegal characters or escape sequences for standard cmd.exe or shell like bash, csh, ...</returns>
        public static string Sanitize(string cmdArgs, bool isCmd = true)
        { 
            string sanitezed = "";
            foreach (char ch in cmdArgs.ToCharArray()) 
            { 
                if (isCmd && CMD_REGULAR_CHARS.Contains(ch))
                    sanitezed += ch;
                else if (!isCmd && ARGS_STRICT_CHARS.Contains(ch))
                    sanitezed += ch;
            }

            return sanitezed;
        } 


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
            string consoleError = "", consoleOutput = "", processCmd = Sanitize(filepath);
            string workingDir = "", args = (!string.IsNullOrEmpty(arguments)) ? Sanitize(arguments, false) : "";
            Area23Log.LogOriginMsg("ProcessCmd", String.Format("ProcessCmd.Execute(filepath = ${0}, args = {1}, useShellExecute = {2}) called ...",
                processCmd, args, useShellExecute));
            try
            {
                // if (!File.Exists(processCmd))
                //     throw new FileNotFoundException("executable file not found: " + processCmd);

                workingDir = (processCmd.Contains(LibPaths.SepCh)) ? processCmd.Substring(0, filepath.LastIndexOf(LibPaths.SepCh)) : "";
                // workingDir = (processCmd.Contains(LibPaths.SepCh)) ? Path.GetDirectoryName(processCmd) : ""; // System.IO.Directory.GetCurrentDirectory();


                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = processCmd;
                    compiler.StartInfo.CreateNoWindow = true;
                    compiler.StartInfo.Arguments = args;
                    compiler.StartInfo.UseShellExecute = useShellExecute;
                    compiler.StartInfo.RedirectStandardError = true;
                    compiler.StartInfo.RedirectStandardOutput = true;
                    if (!string.IsNullOrEmpty(workingDir) && Directory.Exists(workingDir))
                        compiler.StartInfo.WorkingDirectory = workingDir;
                    compiler.Start();

                    consoleOutput = compiler.StandardOutput.ReadToEnd();
                    consoleError = compiler.StandardError.ReadToEnd();

                    compiler.WaitForExit(1000);
                }
            }
            catch (Exception exi)
            {
                string stdErr = (string.IsNullOrEmpty(consoleError)) ? string.Empty : $"\tStdErr = {consoleError}";
                Area23Log.LogOriginMsgEx("ProcessCmd", "ProcessCmd.Execute(string filepath = " + processCmd + ", string arguments = " +
                    arguments + ") throwed Exception: " + exi.GetType(), exi);                                
                throw new InvalidOperationException($"can't execute: {processCmd} {args} {stdErr}", exi);
            }

            Area23Log.Log(String.Format("ProcessCmd.Execute(filepath = ${0}, args = {1}, useShellExecute = {2}) finished successfull, output length: {3}",
                processCmd, args, useShellExecute, consoleOutput.Length));
            if (!string.IsNullOrEmpty(consoleError))
                Area23Log.Log("ProcessCmd.Execute consoleError: " + consoleError);            

            return consoleOutput;
        }

        public static string ExecuteCreateWindow(string filepath = "SystemInfo", string arguments = "", bool useShellExecute = false, bool createWindow = false)
        {
            string consoleError = "", consoleOutput = "", processCmd = Sanitize(filepath);
            string workingDir = "", args = (!string.IsNullOrEmpty(arguments)) ? Sanitize(arguments, false) : "";

            Area23Log.LogOriginMsg("ProcessCmd", String.Format("ProcessCmd.Execute(filepath = ${0}, args = {1}, useShellExecute = {2}) called ...",
                processCmd, args, useShellExecute));
            try
            {
                workingDir = (processCmd.Contains(LibPaths.SepCh)) ? processCmd.Substring(0, processCmd.LastIndexOf(LibPaths.SepCh)) : ""; // System.IO.Directory.GetCurrentDirectory();

                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = processCmd;
                    compiler.StartInfo.CreateNoWindow = createWindow;
                    compiler.StartInfo.Arguments = args;
                    compiler.StartInfo.UseShellExecute = useShellExecute;
                    compiler.StartInfo.RedirectStandardError = true;
                    compiler.StartInfo.RedirectStandardOutput = true;
                    if (!string.IsNullOrEmpty(workingDir) && Directory.Exists(workingDir))
                        compiler.StartInfo.WorkingDirectory = workingDir;
                    compiler.Start();

                    consoleOutput = compiler.StandardOutput.ReadToEnd();
                    consoleError = compiler.StandardError.ReadToEnd();

                    compiler.WaitForExit();
                }
            }
            catch (Exception exi)
            {
                string stdErr = (string.IsNullOrEmpty(consoleError)) ? string.Empty : $"\tStdErr = {consoleError}";
                Area23Log.LogOriginMsgEx("ProcessCmd", "ProcessCmd.Execute(string filepath = " + processCmd + ", string arguments = " +
                    arguments + ") throwed Exception: " + exi.GetType(), exi);
                throw new InvalidOperationException($"can't execute: {processCmd} {args} {stdErr}", exi);
            }

            Area23Log.Log(String.Format("ProcessCmd.Execute(filepath = ${0}, args = {1}, useShellExecute = {2}) finished successfull, output length: {3}",
                processCmd, args, useShellExecute, consoleOutput.Length));
            if (!string.IsNullOrEmpty(consoleError))
                Area23Log.Log("ProcessCmd.Execute consoleError: " + consoleError);

            return consoleOutput;
        }


        public static string ExecuteWithOutAndErr(string filepath, string arguments, out string consoleOutput, out string consoleError, bool useShellExecute = false)
        {
            string processCmd = Sanitize(filepath);
            string args = (!string.IsNullOrEmpty(arguments)) ? Sanitize(arguments, false) : "";
            string workingDir = "";
            consoleError = "";
            consoleOutput = "";
            Area23Log.Log(String.Format("ProcessCmd.Execute(filepath = ${0}, args = {1}, useShellExecute = {2}) called ...\n", processCmd, args, useShellExecute));
            try
            {
                workingDir = (processCmd.Contains(LibPaths.SepCh)) ? processCmd.Substring(0, processCmd.LastIndexOf(LibPaths.SepCh)) : ""; // System.IO.Directory.GetCurrentDirectory();

                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = processCmd;
                    compiler.StartInfo.CreateNoWindow = true;
                    compiler.StartInfo.Arguments = args;
                    compiler.StartInfo.UseShellExecute = useShellExecute;
                    compiler.StartInfo.RedirectStandardError = true;
                    compiler.StartInfo.RedirectStandardOutput = true;
                    if (!string.IsNullOrEmpty(workingDir) && Directory.Exists(workingDir))
                        compiler.StartInfo.WorkingDirectory = workingDir;
                    compiler.Start();

                    consoleOutput = compiler.StandardOutput.ReadToEnd();
                    consoleError = compiler.StandardError.ReadToEnd();

                    compiler.WaitForExit();
                }
            }
            catch (Exception exi)
            {
                string stdErr = (string.IsNullOrEmpty(consoleError)) ? string.Empty : $"\tStdErr = {consoleError}";
                Area23Log.LogOriginMsgEx("ProcessCmd", "ProcessCmd.ExecuteWithOutAndErr(string filepath = " + processCmd + ", string arguments = " +
                   arguments + ") throwed Exception: " + exi.GetType(), exi);               
                throw new InvalidOperationException($"can't execute: {processCmd} {args} {stdErr}", exi);
            }

            string consoleOutErr = !string.IsNullOrEmpty(consoleOutput) ?
                consoleOutput :
                (!string.IsNullOrEmpty(consoleError) ? consoleError : "");

            Area23Log.Log(String.Format("ProcessCmd.Execute(filepath = {0}, args = {1}, useShellExecute = {2}) finished successfull, console msg: {3}\n",
                processCmd, args, useShellExecute, consoleOutErr));

            return consoleOutErr;
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
        public static string Execute(string cmdPath = "SystemInfo", string[] arguments = null, bool quoteArgs = false, bool useShellExecute = false)
        {
            string consoleError = string.Empty, consoleOutput = string.Empty, argStr = string.Empty;
            string processCmd = Sanitize(cmdPath);
            string workingDir = "";
            consoleError = "";
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
                            argStr += Sanitize(arguments[ac], false);
                    }
                }
            }

            Area23Log.Log(string.Format("ProcessCmd.Execute(cmdPath = ${0}, args = {1}, quoteArgs = {2}, useShellExecute = {3}) called ...",
                    processCmd, argStr, quoteArgs, useShellExecute));
            try
            {
                workingDir = (processCmd.Contains(LibPaths.SepCh)) ? processCmd.Substring(0, processCmd.LastIndexOf(LibPaths.SepCh)) : ""; // System.IO.Directory.GetCurrentDirectory();

                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = processCmd;
                    compiler.StartInfo.CreateNoWindow = true;
                    compiler.StartInfo.Arguments = argStr;
                    compiler.StartInfo.UseShellExecute = useShellExecute;
                    compiler.StartInfo.RedirectStandardError = true;
                    compiler.StartInfo.RedirectStandardOutput = true;
                    if (!string.IsNullOrEmpty(workingDir) && Directory.Exists(workingDir))
                        compiler.StartInfo.WorkingDirectory = workingDir;
                    compiler.Start();

                    consoleOutput = compiler.StandardOutput.ReadToEnd();
                    consoleError = compiler.StandardError.ReadToEnd();

                    compiler.WaitForExit();
                }
            }
            catch (Exception exi)
            {
                string stdErr = (string.IsNullOrEmpty(consoleError)) ? string.Empty : $"\tStdErr = {consoleError}";
                Area23Log.LogOriginMsgEx("ProcessCmd", "ProcessCmd.Execute(string filepath = " + processCmd + ", string arguments = " +
                   String.Join(", ", arguments) + ") throwed Exception: " + exi.GetType(), exi);                
                throw new InvalidOperationException($"can't perform {processCmd} {argStr} {stdErr}", exi);
            }


            Area23Log.Log(String.Format("ProcessCmd.Execute(cmdPath = ${0}, arguments = {1}, useShellExecute = {2}) finished successfull, output length: {3}",
                processCmd, argStr, useShellExecute, consoleOutput.Length));
            if (!string.IsNullOrEmpty(consoleError))
                Area23Log.Log("ProcessCmd.Execute consoleError: " + consoleError);

            return consoleOutput;
        }

    }

}
