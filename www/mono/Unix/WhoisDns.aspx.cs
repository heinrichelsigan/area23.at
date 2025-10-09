using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Web.UI.WebControls;
using System.Windows.Input;


namespace Area23.At.Mono.Unix
{

    /// <summary>
    /// WhoisDns query page
    /// </summary>
    public partial class WhoisDns : System.Web.UI.Page
    {
        
        protected internal const string whoisCmdPath =      "C:\\Users\\heinrich.elsigan\\bin\\whois.exe";
        protected internal const string whoisCmdPathUnix =  "/usr/bin/whois";
        protected internal const string whoisCmdArgs =      " {0} ";
        protected internal const string hostCmdPath =       "nslookup";
        protected internal const string hostCmdPathUnix =   "/usr/bin/host";        
        protected internal const string hostCmdArgs =       " -d -v -a {0}";
        protected internal static string[] linesOut = { };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) 
                this.TextBox_HostName.Text = "kernel.org";
        }

        /// <summary>
        /// Perform_Hexdump()
        /// </summary>
        protected string Perform_Whois(string filepath = whoisCmdPath, string args = " {0}")        
        {
            if (Constants.UNIX)
                filepath = whoisCmdPathUnix;

            TableCellLeft.Visible = true;
            TableCellLeft.Text = "";
            try
            {
                args = string.Format(args, this.TextBox_HostName.Text);
                TableHeaderCellLeft.Text = filepath + " " + args;
                string cmdOut = ProcessCmd.ExecuteCreateWindow(filepath, args);
                linesOut = cmdOut.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int ln= 0; ln < linesOut.Length; ln++)
                {
                    if (linesOut[ln].Contains("http"))
                    {
                        string href = linesOut[ln].Substring(linesOut[ln].IndexOf("http"));
                        href = href.Split(" \t\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                        linesOut[ln] = href.Replace(href, String.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", href));
                    }
                    this.TableCellLeft.Text += linesOut[ln] + "<br />\r\n";
                }                
            } 
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                TableCellLeft.Text += ex.GetType().ToString() + ": " + ex.Message + "<br />\r\n" + ex.StackTrace + "<br />\r\n";
            }

            return TableCellLeft.Text;
        }


        /// <summary>
        /// Process classical od cmd
        /// </summary>
        /// <param name="filepath">od cmd filepath</param>
        /// <param name="args">od arguments passed to od</param>
        /// <returns>output of od cmd</returns>
        protected string Process_DnsHost(
            string filepath = hostCmdPath, 
            string args = hostCmdArgs)
        {
            if (Constants.UNIX)
                filepath = hostCmdPathUnix;

            TableCellRight.Visible = true;
            TableCellRight.Text = "";
            try
            {
                args = string.Format(args, this.TextBox_HostName.Text);
                TableHeaderCellRight.Text = filepath + " " + args;
                string cmdOut = ProcessCmd.ExecuteCreateWindow(filepath, args);
                linesOut = cmdOut.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int ln = 0; ln < linesOut.Length; ln++)
                {
                    if (linesOut[ln].Contains("http"))
                    {
                        string href = linesOut[ln].Substring(linesOut[ln].IndexOf("http"));
                        href = href.Split(" \t\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                        linesOut[ln] = linesOut[ln].Replace(href, String.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", href));
                    }
                    this.TableCellRight.Text += linesOut[ln] + "<br />\r\n";
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                TableCellRight.Text += ex.GetType().ToString() + ": " + ex.Message + "\r\n" + ex.StackTrace + "\r\n";
            }

            return TableCellRight.Text;
        }


        protected void Button_WhoisDns_Click(object sender, EventArgs e)
        {
            Perform_Whois((Constants.UNIX) ? whoisCmdPathUnix : whoisCmdPath, this.TextBox_HostName.Text);
            Process_DnsHost((Constants.UNIX) ? hostCmdPathUnix : hostCmdPath, hostCmdArgs);
        }

        protected void Button_WhoisDns_TextChanged(object sender, EventArgs e)
        {
            Perform_Whois((Constants.UNIX) ? whoisCmdPathUnix : whoisCmdPath, this.TextBox_HostName.Text);
            Process_DnsHost((Constants.UNIX) ? hostCmdPathUnix : hostCmdPath, hostCmdArgs);
        }

    }
}