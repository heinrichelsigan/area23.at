using Area23.At.Framework.Library.Crypt.Cipher.Asymmetric;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
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
        protected internal const string opensslRsaArgs = "openssl genpkey -algorithm rsa -pkeyopt rsa_keygen_bits:{0} -out /tmp/rsa_{1}.pk8 ";
        protected internal const string opensslRsaPrivCmd = "openssl rsa -in /tmp/rsa_{0}.pk8 -pubout ";
        protected internal const string opensslDsaParams = "openssl dsaparam -verbose -out /tmp/dsa_{0}.params {1} ";
        protected internal const string opensslDsaArgs = "openssl gendsa -verbose -out /tmp/dsa_{0}.pem /tmp/dsa_{1}.params ";
        protected internal static string[] linesOut = { };
        public readonly string ALLOWED_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-_";

        protected enum Algorithm { RSA, DSA };
        protected Algorithm algorithm = Algorithm.RSA;
        protected int keySize = 1024;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.DropDown_KeySize.Items.Clear();
                this.DropDown_KeySize.Items.Add(new ListItem("512", "512"));
                this.DropDown_KeySize.Items.Add(new ListItem("768", "768"));
                this.DropDown_KeySize.Items.Add(new ListItem("1024", "1024"));
                this.DropDown_KeySize.Items.Add(new ListItem("2048", "2048"));
                this.DropDown_KeySize.Items.Add(new ListItem("3072", "3072"));
                this.DropDown_KeySize.Items.Add(new ListItem("4096", "4096"));
                this.DropDown_KeySize.SelectedIndex = 2;
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
            TableCellLeft.Visible = true;
            TableCellLeft.Text = "";
            TableCellRight.Text = "";
            TableHeaderCellLeft.Text = "Rsa private key";
            TableHeaderCellRight.Text = "Rsa public key";
            string allRsaText = "", rsaPublic = "", rsaPrivate = "";
            try
            {
                // string argsPub = string.Format(opensslRsaArgs, this.TextBox_PassKey.Text, keySize, sessionId);
                // cmdOut = ProcessCmd.ExecuteCreateWindow(filepath, argsPub);
                AsymmetricCipherKeyPair rsaKeyPair = Rsa.GenerateNewRsaKeyPair(keySize);                

                string[] rsaKeyStrings = Rsa.GetStringKeys(rsaKeyPair);
                rsaPrivate = rsaKeyStrings[0];
                rsaPublic = rsaKeyStrings[1];
                
                allRsaText = rsaPublic + "\n" + rsaPrivate;

                TableCellRight.Visible = true;
                this.TableRowCmd1.Visible = true;
                this.TableRowCmd2.Visible = true;
                this.TableRowCmd1.Text = string.Format(opensslRsaArgs, keySize, this.Session.SessionID);
                this.TableRowCmd2.Text = string.Format(opensslRsaPrivCmd, this.Session.SessionID);
                TableCellLeft.Text = rsaPrivate.Replace("\n", "<br />\r\n");                
                TableCellRight.Text = rsaPublic.Replace("\n", "<br />\r\n");
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                TableCellLeft.Text += "\n<br>\r\n" + ex.GetType().ToString() + ": " + ex.Message + "<br />\r\n" + ex.StackTrace + "<br />\r\n";
                return allRsaText; 
            }
           
            return allRsaText;
        }


        /// <summary>
        /// GenerateDsaKey
        /// </summary>        
        /// <returns>output of od cmd</returns>
        protected string GenerateDsaKey(int keySize)
        {
            TableCellLeft.Visible = true;
            TableCellLeft.Text = "";
            TableCellRight.Text = "";
            TableHeaderCellLeft.Text = "Dsa private key";
            TableHeaderCellRight.Text = "Dsa public key";
            string allDsaText = "", dsaPublic = "", dsaPrivate = "";
            try
            {
                // string argsParams = string.Format(opensslDsaParams, sessionId, keySize);                
                // cmdOut = ProcessCmd.ExecuteCreateWindow(filepath, argsParams);
                AsymmetricCipherKeyPair dsaKeyPair = Dsa.GetDsaKeyPair(keySize);

                Tuple<string, string> dsaKeyTuple = Dsa.GetKeysTuple(dsaKeyPair);
                dsaPrivate = dsaKeyTuple.Item1.ToString();
                dsaPublic = dsaKeyTuple.Item2.ToString();
                
                allDsaText = dsaPublic + "\n" + dsaPrivate;

                TableCellRight.Visible = true;
                TableRowCmd1.Visible = true;
                TableRowCmd2.Visible = true;
                TableRowCmd1.Text = string.Format(opensslDsaParams,  this.Session.SessionID, keySize);
                TableRowCmd2.Text = string.Format(opensslDsaArgs , this.Session.SessionID, keySize);
                TableCellLeft.Text = dsaPrivate.Replace("\n", "<br />\r\n");
                TableCellRight.Text = dsaPublic.Replace("\n", "<br />\r\n");
                          
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                TableCellLeft.Text += "\n<br>\r\n" + ex.GetType().ToString() + ": " + ex.Message + "<br />\r\n" + ex.StackTrace + "<br />\r\n";
                return allDsaText;
            }

            return allDsaText;
        }


        protected void Button_Gen_Click(object sender, EventArgs e)
        {
            if (int.TryParse(this.DropDown_KeySize.SelectedValue, out int size))
                this.keySize = size;
            else
                this.keySize = 1024;

            string allKey = (this.RadioButtonList_Algorithm.SelectedValue == "Dsa" || this.algorithm == Algorithm.DSA) ?
                GenerateDsaKey(keySize) : GenerateRsaKey(keySize);
        }
        

        protected void KeySize_Changed(object sender, EventArgs e)
        {
            if (int.TryParse(this.DropDown_KeySize.SelectedValue, out int size))
                this.keySize = size;
            else
            {
                this.keySize = 1024;
                this.DropDown_KeySize.SelectedIndex = 2;
            }
        }

    }

}
