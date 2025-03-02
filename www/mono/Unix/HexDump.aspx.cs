using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Win32Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace Area23.At.Mono.Unix
{
    public partial class HexDump : System.Web.UI.Page
    {
        char radix = 'n';
        int hexWidth = 8;
        int wordWidth = 32;
        long seekBytes = 0;
        long readBytes = 1024;
        string device = "urandom";
        static Random random;
        const string odCmdPath = "od";
        const string odSuidCmdPath = "/usr/bin/od";
        const string odArgsFormat = " -A {0} -t x{1}z -w{2} -v -j {3} -N {4} /dev/{5}";
        const string odDefaultArgs = "-A n -t x8z -w32 -v -j 0 -N 1024 /dev/urandom";
        private readonly string[] allowDevices = { "random", "urandom", "zero" };

        protected string OdArgs
        {
            get
            {
                if (RBL_Radix != null && RBL_Radix.SelectedItem != null && RBL_Radix.SelectedItem.Value != null && RBL_Radix.SelectedItem.Value.Length > 0)
                    radix = RBL_Radix.SelectedItem.Value.ToString()[0];
                if (!Int32.TryParse(DropDown_HexWidth.SelectedItem.Value.ToString(), out hexWidth))
                    hexWidth = 8;
                if (!Int32.TryParse(DropDown_WordWidth.SelectedItem.Value.ToString(), out wordWidth))
                    wordWidth = 32;
                if (!Int64.TryParse(TextBox_Seek.Text, out seekBytes))
                    seekBytes = 0;
                seekBytes = (seekBytes > int.MaxValue) ? int.MaxValue : seekBytes;
                if (!Int64.TryParse(DropDown_ReadBytes.SelectedItem.Value.ToString(), out readBytes))
                    readBytes = 1024;
                readBytes = (readBytes > 4194304) ? 4194304 : readBytes;
                if (DropDown_Device != null && DropDown_Device.SelectedItem != null && DropDown_Device.SelectedItem.Value != null)
                {
                    foreach (string devStr in allowDevices)
                    {
                        if (devStr == DropDown_Device.SelectedItem.Value)
                        {
                            device = devStr;
                            break;
                        }
                    }
                }

                string odArgs = String.Format(" -A {0} -t x{1}z -w{2} -v -j {3} -N {4} /dev/{5}",
                    radix, hexWidth, wordWidth, seekBytes, readBytes, device);

                return odArgs;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // if (!Page.IsPostBack)
            Perform_HexDump();
        }

        /// <summary>
        /// Perform_Hexdump()
        /// </summary>
        protected void Perform_HexDump()
        {
            try
            {
                TextBox_OdCmd.Text = odCmdPath + OdArgs;
                preOut.InnerText = Process_HexDump(odSuidCmdPath, OdArgs);
            } 
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                preOut.InnerText = GetLinesFromRandom(readBytes, wordWidth, hexWidth);
            }
        }

        /// <summary>
        /// Process classical od cmd
        /// </summary>
        /// <param name="filepath">od cmd filepath</param>
        /// <param name="args">od arguments passed to od</param>
        /// <returns>output of od cmd</returns>
        protected string Process_HexDump(
            string filepath = odSuidCmdPath,
            string args = "-A n -t x8z -w32 -v -j 0 -N 1024 /dev/urandom")
        {
            return ProcessCmd.Execute(filepath, args);
        }

        protected void Button_HexDump_Click(object sender, EventArgs e)
        {
            Perform_HexDump();
        }

        protected void HexDump_ParameterChanged(object sender, EventArgs e)
        {
            Perform_HexDump();
        }

        /// <summary>
        /// GetLinesFromRandom 
        /// prints out <see cref="readBytes"/> / <see cref="wordWidth"/> lines in Hex Dump (old od unix format)
        /// each line starts with either an hexadecimal, decimal, octal address or an empty string
        /// then all byte words will be printed by <see cref="GetWordFromRandom(int, out string)"/>
        /// at least all ASCII7 characters without special char will be printed in dump
        /// </summary>
        /// <param name="readBufferLen"><see cref="readBytes"/></param>
        /// <param name="wordLen"><see cref="wordWidth"/></param>
        /// <returns>entire (all lines) hex dump</returns>
        internal string GetLinesFromRandom(long readBufferLen, int wordLen, int byteLen)
        {
            string outPut = String.Empty;
            string ascii7Formated = String.Empty;
            for (long j = 0; j < (long)(readBufferLen / wordLen); j++)
            {
                // TODO: implement ASCII point
                switch (radix)
                {
                    case 'd': outPut += String.Format("{0:d7}\t", (wordLen * j)); break;
                    case 'o':
                        string octString; long octOut;
                        try
                        {
                            octString = Convert.ToString((int)(wordLen * j), 8);
                            octOut = Convert.ToInt64(octString);
                            outPut += String.Format("{0:d7}\t", octOut);
                        }
                        catch (Exception ex)
                        {
                            Area23Log.LogStatic(ex);
                        }
                        break;
                    case 'x': outPut += String.Format("{0:x7}\t", (wordLen * j)); break;
                    case 'n':
                    default: outPut += ""; break;
                }

                outPut += GetWordsFromRandom(wordLen, byteLen, out ascii7Formated);
                outPut += "\t>";
                outPut += ascii7Formated;
                outPut += "< \r\n";
            }
            
            return outPut;
        }

        /// <summary>
        /// GetWordsFromRandom
        /// gets <see cref="wordWidth"/> / <see cref="hexWidth"/> words
        /// </summary>
        /// <param name="wordLen"><see cref="wordWidth"/></param>
        /// <param name="byteLen"><see cref="hexWidth"/></param>
        /// <param name="ascii7Formated">out ASCII7 formated characters without special char, representing random bytes</param>
        /// <returns>byte words of hex dump as string</returns>
        internal string GetWordsFromRandom(int wordLen, int byteLen, out string ascii7Formated)
        {
            string words = string.Empty;
            ascii7Formated = string.Empty;
            // hexBytes = new byte[byteLen];
            wordLen = (byteLen > wordLen) ? byteLen : wordLen;
            int execBytes = Math.Max((int)(wordLen / byteLen), 1);
            byte[] hexBytes = new byte[execBytes * byteLen];
            for (int wc = 0; wc < execBytes; wc++)
            {
                hexBytes = GetHexBytesFromRandom(byteLen);
                foreach (byte b in hexBytes)
                {
                    words += String.Format("{0:x2}", b);
                }
                foreach (byte b in hexBytes)
                {
                    if ((int)b >= 32 && (int)b < 127)
                        ascii7Formated += (char)((int)b);
                    else
                        ascii7Formated += ".";
                }

                words += " ";
            }
            return words;
        }


        /// <summary>
        /// GetHexBytesFromRandom 
        /// </summary>
        /// <param name="byteLen">number of bytes, that will be filled with random bytes or 0 in case of /dev/zero</param>
        /// <returns>an array of bytes <see cref="byte[]" /> filled with random or 0 bytes</returns>
        internal byte[] GetHexBytesFromRandom(int byteLen)
        {
            if (HexDump.random == null)
                HexDump.random = new Random((int)(DateTime.Now.Ticks % Int32.MaxValue));
            byte[] buffer = new byte[byteLen];
            if (device == "zero") 
                for (int bc = 0; bc < buffer.Length; buffer[bc++] = (byte)0) ;
            else                     
                random.NextBytes(buffer);
            return buffer;
        }
    }
}