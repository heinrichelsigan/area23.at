using area23.at.mono.test.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace area23.at.mono.test
{
    public partial class HexDump : System.Web.UI.Page
    {
        char radix = 'n';
        int hexWidth = 8;
        int wordWidth = 32;
        long seekBytes = 0;
        long readBytes = 1024;
        string device = "urandom";
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

        protected void Perform_HexDump()
        {
            TextBox_OdCmd.Text = odCmdPath + OdArgs;
            preOut.InnerText = Process_HexDump(odSuidCmdPath, OdArgs);
        }

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

    }
}