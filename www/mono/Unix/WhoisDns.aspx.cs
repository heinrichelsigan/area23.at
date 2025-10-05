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

        protected internal const string whoisCmdPath = "/usr/bin/whois";
        protected internal const string hostCmdPath = "/usr/bin/host";
        protected internal const string hostCmdArgs = " -d -v -a {0}";
        protected internal static string[] linesOut = { };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) 
                this.TextBox_HostName.Text = Environment.MachineName;
        }

        /// <summary>
        /// Perform_Hexdump()
        /// </summary>
        protected string Perform_Whois(string filepath = whoisCmdPath, string args = " {0}")
        {
            preOut.InnerText = "";
            try
            {
                args = string.Format(args, this.TextBox_HostName.Text);
                TextBox_Whois.Text = filepath + args;
                string cmdOut = ProcessCmd.Execute(filepath, args);
                linesOut = cmdOut.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int ln= 0; ln < linesOut.Length; ln++)
                {
                    if (linesOut[ln].Contains("http"))
                    {
                        string href = linesOut[ln].Substring(linesOut[ln].IndexOf("http"));
                        href = href.Split(" \t\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                        linesOut[ln] = href.Replace(href, String.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", href));
                    }
                    this.preOut.InnerText += linesOut[ln] + "\r\n";
                }                
            } 
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                preOut.InnerText = ex.GetType().ToString() +": " + ex.Message + "\r\n" + ex.StackTrace;
            }

            return preOut.InnerText;
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
            try
            {
                args = string.Format(args, this.TextBox_HostName.Text);
                TextBox_Host.Text = filepath + args;
                string cmdOut = ProcessCmd.Execute(filepath, args);
                linesOut = cmdOut.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int ln = 0; ln < linesOut.Length; ln++)
                {
                    if (linesOut[ln].Contains("http"))
                    {
                        string href = linesOut[ln].Substring(linesOut[ln].IndexOf("http"));
                        href = href.Split(" \t\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                        linesOut[ln] = linesOut[ln].Replace(href, String.Format("<a href=\"{0}\" target=\"_blank\">{0}</a>", href));
                        this.preOut.InnerHtml = linesOut[ln] + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                preOut.InnerText += ex.GetType().ToString() + ": " + ex.Message + "\r\n" + ex.StackTrace;
            }

            return preOut.InnerText;
        }


        protected void Button_WhoisDns_Click(object sender, EventArgs e)
        {
            Perform_Whois(whoisCmdPath, this.TextBox_Host.Text);
            Process_DnsHost(hostCmdPath, hostCmdArgs);
        }

        protected void Button_WhoisDns_TextChanged(object sender, EventArgs e)
        {
            Perform_Whois(whoisCmdPath, this.TextBox_Host.Text);
            Process_DnsHost(hostCmdPath, hostCmdArgs);
        }

    }
}