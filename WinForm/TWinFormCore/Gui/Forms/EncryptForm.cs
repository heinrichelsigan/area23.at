using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zfx;

namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    public partial class EncryptForm : TransparentFormCore
    {

        Cursor NormalCursor, NoDropCursor;
        internal bool fileUploaded = false;
        internal System.Windows.Forms.DragDropEffects _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
        internal static HashSet<string> HashFiles = new HashSet<string>();
        internal delegate void SetGroupBoxTextCallback(System.Windows.Forms.GroupBox groupBox, string headerText);

        public EncryptForm()
        {
            InitializeComponent();
            this.menuStrip.Visible = false;
        }


        internal virtual void SetGBoxText(string text)
        {
            string textToSet = (!string.IsNullOrEmpty(text)) ? text : string.Empty;
            if (InvokeRequired)
            {
                SetGroupBoxTextCallback setGroupBoxText = delegate (GroupBox gBox, string hText)
                {
                    if (gBox != null && gBox.Name != null && !string.IsNullOrEmpty(hText))
                        gBox.Text = hText;
                };
                try
                {
                    Invoke(setGroupBoxText, new object[] { groupBoxFiles, textToSet });
                }
                catch (System.Exception exDelegate)
                {
                    Area23Log.LogOriginMsg(this.Name, $"Exception in delegate SetGBoxText text: \"{textToSet}\".\n");
                }
            }
            else
            {
                if (this != null && this.Name != null && textToSet != null)
                    groupBoxFiles.Text = textToSet;
            }
        }


        internal void EncryptForm_Load(object sender, EventArgs e)
        {

            this.comboBoxAlgo.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(Area23.At.Framework.Core.Crypt.Cipher.CipherEnum)))
            {
                // System.Windows.Forms.ListViewItem listViewItem = new System.Windows.Forms.ListViewItem(item.ToString());
                this.comboBoxAlgo.Items.Add(item.ToString());
            }
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
            if (!string.IsNullOrEmpty(this.textBoxPipe.Text) && !string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                if (!string.IsNullOrEmpty(this.textBoxSrc.Text))
                {
                    CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                    string encrypted = cPipe.EncrpytTextGoRounds(this.textBoxSrc.Text, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                    this.textBoxOut.Text = encrypted;
                }
                else if (!string.IsNullOrEmpty(this.labelFileIn.Text) && !labelFileIn.Text.StartsWith("["))
                {
                    foreach (string file in HashFiles)
                    {
                        if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                        {
                            if (Path.GetFileName(file) == labelFileIn.Text)
                            {
                                CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                                byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                                byte[] outBytes = cPipe.EncrpytFileBytesGoRounds(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                                string outFilePath = (file + "." + cPipe.PipeString + GetZip().GetZipTypeExtension());
                                File.WriteAllBytes(outFilePath, outBytes);
                                pictureBoxOutFile.Visible = true;
                                pictureBoxOutFile.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.encrypted;
                                string outFileName = Path.GetFileName(outFilePath);                                
                                labelOutputFile.Text = file;
                                string encrypted = GetEncoding().EnCode(outBytes);
                                this.textBoxOut.Text = encrypted;
                                break;
                            }
                        }
                    }

                }
            }
        }

        private void Decrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxPipe.Text) && !string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                if (!string.IsNullOrEmpty(this.textBoxSrc.Text))
                {
                    CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                    string decrypted = cPipe.DecryptTextRoundsGo(this.textBoxSrc.Text, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                    this.textBoxOut.Text = decrypted;
                }
                else if (!string.IsNullOrEmpty(this.labelFileIn.Text) && !labelFileIn.Text.StartsWith("["))
                {
                    foreach (string file in HashFiles)
                    {
                        if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                        {
                            if (Path.GetFileName(file) == labelFileIn.Text)
                            {
                                CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                                byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                                byte[] outBytes = cPipe.DecryptFileBytesRoundsGo(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                                File.WriteAllBytes(file.Replace("." + cPipe.PipeString, ""), outBytes);
                                pictureBoxOutFile.Visible = true;
                                pictureBoxOutFile.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.decrypted;
                                labelOutputFile.Text = Path.GetFileName(file.Replace("." + cPipe.PipeString + GetZip().GetZipTypeExtension(), ""));

                                string decrypted = GetEncoding().EnCode(outBytes);
                                this.textBoxOut.Text = decrypted;
                                break;
                            }
                        }
                    }
                }
            }
        }


        private void Drag_Drop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] files = new string[1];

            if (e != null && e.Data != null && (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) ||
                e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop)  || e.Data.GetDataPresent(typeof(string[]))))
            {
                files = (string[])e.Data.GetData(System.Windows.DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    if (HashFiles == null || HashFiles.Count == 0)
                        HashFiles = new HashSet<string>(files);

                    string textSet = Path.GetFileName(files[0]) ?? files[0] ?? "";
                    if (e.Effect != System.Windows.Forms.DragDropEffects.None)
                    {
                        _dragDropEffect = e.Effect;
                        textSet += " DragDrop: " + e.Effect;
                        SetGBoxText(textSet);
                    }

                    foreach (string file in files)
                    {
                        if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                        {
                            this.textBoxSrc.Text = string.Empty;
                            this.textBoxOut.Text = string.Empty;
                            fileUploaded = true;
                            if (Path.GetExtension(file).Length > 7)
                                pictureBoxFileIn.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.encrypted;
                            else
                                pictureBoxFileIn.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.file;
                            this.labelFileIn.Text = Path.GetFileName(file);
                            break;
                        }
                    }
                }
            }
            return;
        }

        private void Drag_Enter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] files = new string[1];
            fileUploaded = false;

            if (e != null && e.Data != null)
            {
                if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(string[])))
                {
                    files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
                    if (files != null && files.Length > 0)
                    {
                        HashFiles = new HashSet<string>(files);
                        _dragDropEffect = e.Effect;
                        string textSet = Path.GetFileName(files[0]) ?? files[0] ?? "";
                        textSet += " DragEnter: " + _dragDropEffect;
                        SetGBoxText(textSet);
                    }
                }
                if (Cursor.Current != NormalCursor)
                {
                    Cursor.Current = NormalCursor;
                }
            }
        }

        private void QueryContinue_Drag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e)
        {
            string[] files = (HashFiles != null && HashFiles.Count > 0) ? HashFiles.ToArray() : new string[0];

            if (e.Action == System.Windows.Forms.DragAction.Cancel || e.Action == System.Windows.Forms.DragAction.Drop)
            {
                if (files != null && files.Length > 0)
                {
                    string textSet = Path.GetFileName(files[0]) ?? files[0] ?? "";

                    foreach (string file in files)
                    {
                        if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                        {
                            this.textBoxSrc.Text = string.Empty;
                            this.textBoxOut.Text = string.Empty;
                            fileUploaded = true;
                            if (Path.GetExtension(file).Length > 7)
                                pictureBoxFileIn.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.encrypted;
                            else
                                pictureBoxFileIn.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.file;
                            this.labelFileIn.Text = Path.GetFileName(file);
                            fileUploaded = false;
                            // HashFiles = new HashSet<string>();
                            _dragDropEffect = System.Windows.Forms.DragDropEffects.Copy;
                            SetGBoxText("Files Group Box");
                            break;
                        }
                    }
                }

            }
        }

        protected virtual void GiveFeedBack(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
        {
            if (e != null && e.Effect == DragDropEffects.None)
            {
                // Sets the custom cursor based upon the effect.
                e.UseDefaultCursors = false;
                NormalCursor = new Cursor(Properties.Resources.icon_file_warning.Handle);
                NoDropCursor = new Cursor(Properties.Resources.icon_file_working.Handle);
                // Cursor.Current = MyNormalCursor;
                // HOTFIX: no drop cursor
                // Cursor.Current = (!firstLeavedDropTarget) ? MyNormalCursor : MyNoDropCursor;
            }
        }

        private void Drag_Leave(object sender, EventArgs e)
        {
            string[] files = (HashFiles != null && HashFiles.Count > 0) ? HashFiles.ToArray() : new string[0];

            if (files != null && files.Length > 0)
            {
                string textSet = Path.GetFileName(files[0]) ?? files[0] ?? "";

                foreach (string file in files)
                {
                    if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                    {
                        this.textBoxSrc.Text = string.Empty;
                        this.textBoxOut.Text = string.Empty;
                        fileUploaded = true;
                        if (Path.GetExtension(file).Length > 7)
                            pictureBoxFileIn.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.encrypted;
                        else
                            pictureBoxFileIn.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.file;
                        this.labelFileIn.Text = Path.GetFileName(file);
                        fileUploaded = false;
                        // HashFiles = new HashSet<string>();
                        _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
                        SetGBoxText("Files Group Box");
                        break;
                    }
                }

            }

            Cursor.Current = DefaultCursor;
        }

        private void Drag_Over(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] files = new string[1];

            if (e != null && e.Data != null && (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(string[]))))
            {
                files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    if (HashFiles == null || HashFiles.Count == 0)
                        HashFiles = new HashSet<string>(files);

                    string textSet = Path.GetFileName(files[0]) ?? files[0] ?? "";
                    if (e.Effect != System.Windows.Forms.DragDropEffects.None)
                    {
                        _dragDropEffect = e.Effect;
                        textSet += " DragOver: " + e.Effect;
                        SetGBoxText(textSet);
                    }
                }
            }
        }

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open File";
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true; ;
            dialog.RestoreDirectory = true;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName) && System.IO.File.Exists(dialog.FileName))
            {
                if (Path.GetExtension(dialog.FileName).Length > 7)
                    pictureBoxFileIn.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.encrypted;
                else
                    pictureBoxFileIn.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.file;

                this.labelFileIn.Text = Path.GetFileName(dialog.FileName);
                HashFiles = new HashSet<string>();
                HashFiles.Add(dialog.FileName);
            }
        }


        private void menuMainSave_Click(object sender, EventArgs e)
        {
            if (this.pictureBoxOutFile.Visible == true)
            {
                byte[] fileBytes = new byte[0];
                SaveFileDialog dialog = new SaveFileDialog();

                foreach (string filePath in HashFiles)
                {
                    if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                    {
                        if (Path.GetFileName(filePath) == labelOutputFile.Text)
                        {
                            fileBytes = System.IO.File.ReadAllBytes(filePath);
                            break;
                        }
                    }
                }
                if (fileBytes != null && fileBytes.Length > 0)
                {
                    dialog.Title = "Save File";
                    dialog.CheckFileExists = true;
                    dialog.CheckPathExists = true;
                    dialog.CheckWriteAccess = true;
                    dialog.RestoreDirectory = true;
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName) && System.IO.File.Exists(dialog.FileName))
                    {
                        File.WriteAllBytes(dialog.FileName, fileBytes);
                    }
                }
                if (HashFiles.Contains(dialog.FileName))
                    HashFiles.Remove(dialog.FileName);
                this.pictureBoxOutFile.Visible = false;
                this.labelOutputFile.Visible = false;

            }
        }

     
    }
}
