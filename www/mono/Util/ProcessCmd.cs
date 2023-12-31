﻿using System;
using System.Diagnostics;

namespace area23.at.www.mono.Util
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
        /// <returns>standard output of process pexecec it.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static string Execute(string filepath = "SystemInfo", string args = "")
        {
            string consoleError = "", consoleOutput = "";
            try
            {
                using (Process compiler = new Process())
                {
                    compiler.StartInfo.FileName = filepath;
                    string argTrys = (!string.IsNullOrEmpty(args)) ? args : "";
                    compiler.StartInfo.Arguments = argTrys;
                    compiler.StartInfo.UseShellExecute = false;
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
                // consoleOutput = $"Exception: {exi.Message}";
                throw new InvalidOperationException($"can't perform {filepath} {args}\nStdErr = {consoleError}", exi);
            }

            return consoleOutput;
        }
    }
}