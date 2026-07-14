using Area23.At.Framework.Library.Net.IpSocket;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows.Input;


namespace Area23.At.Mono.Unix
{

    /*
     * openssl genpkey -algorithm rsa -pkeyopt rsa_keygen_bits:$KLEN > /tmp/rsa_$KLEN.pk8 2>/dev/null
     * cat /tmp/rsa_$KLEN.pk8
     * openssl rsa -in /tmp/rsa_$KLEN.pk8 -pubout | tee rsa_$KLEN.spki
     * openssl dsaparam -verbose -out out_des.params 2048
     * openssl gendsa -aes256 -verbose -out out_des.pem  out_des.params
     */


    /// <summary>
    /// RsaDsa Generation
    /// </summary>
    public partial class RsaDsaGen : System.Web.UI.Page
    {

        protected internal readonly string whoisCmdPath = System.IO.Path.Combine(LibPaths.SystemDirBinPath, "whois.exe");
        protected internal const string opensslCmd = "openssl";
        protected internal const string opensslRsaArgs = " genpkey -algorithm rsa  -pass pass:{0} -pkeyopt rsa_keygen_bits:{1} 2>/dev/null";
        protected internal const string opensslRsaPrivCmd = " rsa -in /tmp/rsa_{0}.pk8 -pubout | tee rsa_{1}.spki";
        protected internal const string opensslDsaParams = " dsaparam -verbose -out /tmp/dsa_{0}.params {1} ";
        protected internal const string opensslDsaArgs = " gendsa -verbose -out /tmp/dsa_{0}.pem -passout pass:{1} /tmp/dsa_{2}.params ";
        protected internal static string[] linesOut = { };
        public readonly string ALLOWED_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-_";

        protected enum Algorithm { RSA, DSA };
        protected Algorithm algorithm = Algorithm.RSA;
        protected int keySize = 1024;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.TextBox_PassKey.Text = "kernel.org";
            }

            if (this.RadioButtonList_Algorithm.SelectedValue == "Dsa")
                this.algorithm = Algorithm.DSA;
            else
                this.algorithm = Algorithm.RSA;
            if (int.TryParse(this.DropDown_KeySize.SelectedValue, out int size))
                this.keySize = size;
            else
                this.keySize = 1024;

        }

        /// <summary>
        /// Perform_Hexdump()
        /// </summary>
        protected string GenerateRsaKey(int keySize)
        {
            string filepath = opensslCmd;
            TableCellLeft.Visible = true;
            TableCellLeft.Text = "";
            TableCellRight.Text = "";
            string cmdOut = "", allRsaText = "", sessionId = this.Session.SessionID;
            try
            {
                string argsPub = string.Format(opensslRsaArgs, this.TextBox_PassKey.Text, keySize, sessionId);

                TableHeaderCellLeft.Text = filepath + " " + argsPub.Replace(">", "&#62;");
                cmdOut = ProcessCmd.ExecuteCreateWindow(filepath, argsPub);
                
                System.Threading.Thread.Sleep(100);
                
                System.IO.File.WriteAllText("/tmp/rsa_" + sessionId + ".pk8", cmdOut);
                allRsaText = cmdOut;
                TableCellLeft.Text = allRsaText.Replace("\n", "<br />\r\n");
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                TableCellLeft.Text += "\n<br>\r\n" + ex.GetType().ToString() + ": " + ex.Message + "<br />\r\n" + ex.StackTrace + "<br />\r\n";
                return allRsaText; 
            }

            try
            {
                TableCellRight.Visible = true;
                string argsPriv = String.Format(opensslRsaPrivCmd, sessionId, sessionId);
                TableCellRight.Text = filepath + " " + argsPriv;
                cmdOut = ProcessCmd.ExecuteCreateWindow(filepath, argsPriv);
                allRsaText += "\n" + cmdOut;
                TableCellRight.Text = cmdOut.Replace("\n", "<br />\r\n");
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                TableCellRight.Text += "\n<br>\r\n" + ex.GetType().ToString() + ": " + ex.Message + "<br />\r\n" + ex.StackTrace + "<br />\r\n";
                return allRsaText;
            }

            System.Threading.Thread.Sleep(200);
            
            if (System.IO.File.Exists("/tmp/rsa_" + sessionId+ ".pk8"))
                System.IO.File.Delete("/tmp/rsa_" + sessionId + ".pk8");

            return allRsaText;
        }


        /// <summary>
        /// GenerateDsaKey
        /// </summary>        
        /// <returns>output of od cmd</returns>
        protected string GenerateDsaKey(int keySize)
        {
            string filepath = opensslCmd;
            TableCellLeft.Visible = true;
            TableCellLeft.Text = "";
            string cmdOut = "", allDsaText = "", sessionId = this.Session.SessionID;
            try
            {
                string argsParams = string.Format(opensslDsaParams, sessionId, keySize);

                TableHeaderCellLeft.Text = filepath + " " + argsParams;
                cmdOut = ProcessCmd.ExecuteCreateWindow(filepath, argsParams);
                
                System.Threading.Thread.Sleep(250);

                allDsaText = System.IO.File.ReadAllText("/tmp/dsa_" + sessionId + ".params");
                TableCellLeft.Text = allDsaText.Replace("\n", "<br />\r\n");
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                TableCellLeft.Text += "\n<br>\r\n" + ex.GetType().ToString() + ": " + ex.Message + "<br />\r\n" + ex.StackTrace + "<br />\r\n";
                return allDsaText;
            }

            try
            {
                TableCellRight.Visible = true;
                string argsPriv = String.Format(opensslDsaArgs, sessionId, this.TextBox_PassKey.Text, sessionId);
                TableCellRight.Text = filepath + " " + argsPriv;
                cmdOut = ProcessCmd.ExecuteCreateWindow(filepath, argsPriv);

                System.Threading.Thread.Sleep(250);

                string desPrivKey = System.IO.File.ReadAllText("/tmp/dsa_" + sessionId + ".pem");
                allDsaText += "\n" + desPrivKey;
                TableCellRight.Text = desPrivKey.Replace("\n", "<br />\r\n");
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                TableCellRight.Text += "\n<br>\r\n" + ex.GetType().ToString() + ": " + ex.Message + "<br />\r\n" + ex.StackTrace + "<br />\r\n";
                return allDsaText;
            }

            if (System.IO.File.Exists("/tmp/dsa_" + sessionId + ".pem"))
                System.IO.File.Delete("/tmp/dsa_" + sessionId + ".pem");

            if (System.IO.File.Exists("/tmp/dsa_" + sessionId + ".params"))
                System.IO.File.Delete("/tmp/dsa_" + sessionId + ".params");

            return allDsaText;
        }


        protected void Button_Gen_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.TextBox_PassKey.Text))
            {
                this.TextBox_PassKey.BorderColor = Color.DarkRed;
                this.TextBox_PassKey.BorderStyle = BorderStyle.Dashed;
                this.TextBox_PassKey.Width = 2;
                return;
            }

            this.TextBox_PassKey.BorderColor = Color.Black;
            this.TextBox_PassKey.BorderStyle = BorderStyle.Solid;
            this.TextBox_PassKey.Width = 1;

            if (int.TryParse(this.DropDown_KeySize.SelectedValue, out int size))
                this.keySize = size;
            else
                this.keySize = 1024;

            if (this.RadioButtonList_Algorithm.SelectedValue == "Dsa" || this.algorithm == Algorithm.DSA)
            {
                this.TextBox_PassKey.Text = GenerateDsaKey(keySize);
            }
            else
            {
                this.TextBox_PassKey.Text = GenerateRsaKey(keySize);
            }
        }

        protected void TextBox_PassKey_TextChanged(object sender, EventArgs e)
        {
            Button_Gen_Click(sender, e);
        }

        protected void Algorithm_Changed(object sender, EventArgs e)
        {
            if (this.RadioButtonList_Algorithm.SelectedValue == "Dsa")
                this.algorithm = Algorithm.DSA;
            else
                this.algorithm = Algorithm.RSA;
        }


        protected void KeySize_Changed(object sender, EventArgs e)
        {
            if (int.TryParse(this.DropDown_KeySize.SelectedValue, out int size))
                this.keySize = size;
            else
                this.keySize = 1024;
        }

    }

}
