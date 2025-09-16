using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Zfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.TextFormatting;

namespace Area23.At.WinForm.WinRoachCore
{
    public partial class WinForm : Form
    {
        public WinForm()
        {
            InitializeComponent();
        }


        private void menuCompression_Click(object sender, EventArgs e) => SetCompression((ToolStripMenuItem)sender);

        private void SetCompression(ToolStripMenuItem mi)
        {
            menu7z.Checked = false;
            menuBZip2.Checked = false;
            menuGzip.Checked = false;
            menuZip.Checked = false;
            menuCompressionNone.Checked = false;

            if (mi != null && mi.Name != null &&
                (mi.Name.StartsWith("menu") && (mi.Name.EndsWith("7z") || mi.Name.EndsWith("BZip2") || mi.Name.EndsWith("Gzip") || mi.Name.EndsWith("Zip") || mi.Name.EndsWith("None"))))
            {
                mi.Checked = true;
            }
            ZipType zipType = ZipTypeExtensions.GetZipType(mi.Name);
        }

        protected ZipType GetZip()
        {
            if (menu7z.Checked) return ZipType.Z7;
            if (menuBZip2.Checked) return ZipType.BZip2;
            if (menuGzip.Checked) return ZipType.GZip;
            if (menuZip.Checked) return ZipType.Zip;
            // if (menuCompressionNone.Checked) return ZipType.None;
            menuCompressionNone.Checked = true;
            return ZipType.None;
        }

        private void menuEncodingKind_Click(object sender, EventArgs e) => SetEncoding((ToolStripMenuItem)sender);

        protected void SetEncoding(ToolStripMenuItem mi)
        {
            menuBase16.Checked = false;
            menuHex16.Checked = false;
            menuBase32.Checked = false;
            menuHex32.Checked = false;
            menuBase64.Checked = false;
            menuUu.Checked = false;

            if (mi != null && mi.Name != null &&
                (mi.Name.StartsWith("menuBase") || mi.Name.StartsWith("menuHex") || mi.Name.StartsWith("menuUu")))
            {
                mi.Checked = true;
            }

        }

        protected EncodingType GetEncoding()
        {
            if (menuBase16.Checked) return EncodingType.Base16;
            if (menuHex16.Checked) return EncodingType.Hex16;
            if (menuBase32.Checked) return EncodingType.Base32;
            if (menuHex32.Checked) return EncodingType.Hex32;
            if (menuUu.Checked) return EncodingType.Uu;
            // if (menuBase64.Checked) return EncodingType.Base64;
            menuBase64.Checked = true;
            return EncodingType.Base64;

        }

        private void menuHash_Click(object sender, EventArgs e) => SetHash((ToolStripMenuItem)sender);

        protected void SetHash(ToolStripMenuItem mi)
        {

            KeyHash[] keyHashes = KeyHash_Extensions.GetHashTypes();
            menuHashBCrypt.Checked = false;
            menuHashHex.Checked = false;
            menuHashMD5.Checked = false;
            menuHashOpenBsd.Checked = false;
            menuHashSCrypt.Checked = false;
            menuHashSha1.Checked = false;
            menuHashSha256.Checked = false;
            menuHashSha512.Checked = false;


            if (mi != null && mi.Name != null && mi.Name.StartsWith("menuHash"))
            {
                mi.Checked = true;
            }

        }

        protected KeyHash GetHash()
        {
            if (menuHashBCrypt.Checked) return KeyHash.BCrypt;
            if (menuHashHex.Checked) return KeyHash.Hex;
            if (menuHashMD5.Checked) return KeyHash.MD5;
            if (menuHashOpenBsd.Checked) return KeyHash.OpenBSDCrypt;
            if (menuHashSCrypt.Checked) return KeyHash.SCrypt;
            if (menuHashSha1.Checked) return KeyHash.Sha1;
            if (menuHashSha256.Checked) return KeyHash.Sha256;
            if (menuHashSha512.Checked) return KeyHash.Sha512;

            menuHashHex.Checked = true;
            return KeyHash.Hex;
        }

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open File";
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName) && System.IO.File.Exists(dialog.FileName))
            {
                // textBoxFile.Text = dialog.FileName;
            }
        }

        private void WinForm_Load(object sender, EventArgs e)
        {

            this.comboBoxAlgo.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(Area23.At.Framework.Core.Crypt.Cipher.CipherEnum)))
            {
                // System.Windows.Forms.ListViewItem listViewItem = new System.Windows.Forms.ListViewItem(item.ToString());
                this.comboBoxAlgo.Items.Add(item.ToString());
            }
        }

        private void pictureBoxAddAlgo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxAlgo.SelectedText) && Enum.TryParse<CipherEnum>(comboBoxAlgo.SelectedText, out CipherEnum cipherEnum))
            {
                this.textBoxPipe.Text += cipherEnum.ToString() + ";";
            }
        }


        private void Clear_Click(object sender, EventArgs e)
        {
            this.textBoxHash.Text = string.Empty;
            this.textBoxKey.Text = string.Empty;
            this.textBoxPipe.Text = string.Empty;
            this.textBoxSrc.Text = string.Empty;
            this.textBoxOut.Text = string.Empty;
        }

        private void Hash_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                this.textBoxHash.Text = GetHash().Hash(this.textBoxKey.Text);
            }
        }

        private void SetPipeline_Click(object sender, EventArgs e)
        {
            this.textBoxPipe.Text = string.Empty;
            CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
            foreach (CipherEnum cipher in cPipe.InPipe)
            {
                this.textBoxPipe.Text += cipher.ToString() + ";";
            }
        }

        private void Encrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxPipe.Text) && !string.IsNullOrEmpty(this.textBoxKey.Text) && !string.IsNullOrEmpty(this.textBoxSrc.Text))
            {
                CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                string encrypted = cPipe.EncrpytTextGoRounds(this.textBoxSrc.Text, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                this.textBoxOut.Text = encrypted;
            }
        }

        private void Decrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxPipe.Text) && !string.IsNullOrEmpty(this.textBoxKey.Text) && !string.IsNullOrEmpty(this.textBoxSrc.Text))
            {
                CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                string decrypted = cPipe.DecryptTextRoundsGo(this.textBoxSrc.Text, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                this.textBoxOut.Text = decrypted;
            }
        }
    }
}
