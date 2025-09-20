using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zfx;
using Area23.At.WinForm.TWinFormCore.Helper;

namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    public partial class EncryptForm : TransparentFormCore
    {

        Cursor NormalCursor, NoDropCursor;
        internal System.Windows.Forms.DragDropEffects _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
        private bool isDragMode = false;
        private readonly Lock _Lock = new Lock();

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
                                string outFilePath = (file + "." + cPipe.PipeString + "." + GetEncoding().GetEnCodingExtension());
                                File.WriteAllBytes(outFilePath, outBytes);
                                pictureBoxOutFile.Visible = true;
                                pictureBoxOutFile.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.encrypted;
                                string outFileName = Path.GetFileName(outFilePath);
                                labelOutputFile.Text = file;
                                HashFiles.Add(outFilePath);
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
                                string outFileDecrypt = file.Replace("." + cPipe.PipeString + GetEncoding().GetEnCodingExtension(), "");
                                File.WriteAllBytes(outFileDecrypt, outBytes);
                                HashFiles.Add(outFileDecrypt);
                                pictureBoxOutFile.Visible = true;
                                pictureBoxOutFile.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.decrypted;
                                labelOutputFile.Text = Path.GetFileName(outFileDecrypt);

                                string decrypted = GetEncoding().EnCode(outBytes);
                                this.textBoxOut.Text = decrypted;
                                break;
                            }
                        }
                    }
                }
            }
        }


        internal void Drag_Enter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] files = new string[1];

            if (e != null && e.Data != null)
            {
                if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(string[])))
                {
                    if (((files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)) != null) && files.Length > 0)
                    {
                        DragEnterOver(files, DragNDropState.DragEnter, e);
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
        }


        internal void Drag_Over(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] files = new string[1];

            if (e != null && e.Data != null && (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(string[]))))
            {
                if (((files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)) != null) && files.Length > 0)
                {
                    DragEnterOver(files, DragNDropState.DragOver, e);
                }
            }
        }


        public virtual void DragEnterOver(string[] files, DragNDropState dragNDropState, System.Windows.Forms.DragEventArgs e)
        {
            lock (_Lock)
            {
                if (HashFiles == null || HashFiles.Count == 0)
                    HashFiles = new HashSet<string>(files);
                else
                    foreach (string file in files)
                    {
                        if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                            if (!HashFiles.Contains(file))
                                HashFiles.Add(file);
                    }

                if (dragNDropState == DragNDropState.DragEnter)
                    e.Effect = DragDropEffects.Copy;
                if (dragNDropState != DragNDropState.DragLeave)
                    isDragMode = true;

                _dragDropEffect = e.Effect;
                if (e.Effect != System.Windows.Forms.DragDropEffects.None)
                {
                    string textSet = Path.GetFileName(files[0]) ?? files[0] ?? "";
                    textSet += dragNDropState.ToString() + ": " + _dragDropEffect;
                    SetGBoxText(textSet);
                }

                if (NormalCursor == null || NoDropCursor == null)
                {
                    Icon iconFileWork = new Icon(Properties.Resources.icon_file_working, new Size(32, 32));
                    Icon iconFileWarn = new Icon(Properties.Resources.icon_file_warning, new Size(32, 32));
                    NormalCursor = new Cursor(iconFileWork.Handle);
                    NoDropCursor = new Cursor(iconFileWarn.Handle);
                }

                Cursor.Current = (isDragMode) ? NormalCursor : NoDropCursor;
            }
        }


        internal void Give_FeedBack(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
        {
            if (e != null)
            {
                // Sets the custom cursor based upon the effect.
                e.UseDefaultCursors = false;
                _dragDropEffect = e.Effect;
                NormalCursor = new Cursor(Properties.Resources.icon_file_warning.Handle);
                NoDropCursor = new Cursor(Properties.Resources.icon_file_working.Handle);
                Cursor.Current = (isDragMode) ? NormalCursor : NoDropCursor;
                // HOTFIX: no drop cursor
                // Cursor.Current = (!firstLeavedDropTarget) ? MyNormalCursor : MyNoDropCursor;
            }
        }

        internal void Drag_Leave(object sender, EventArgs e)
        {
            isDragMode = false;
            Cursor.Current = DefaultCursor;
            _dragDropEffect = DragDropEffects.None;
            SetGBoxText("Files Group Box");
        }



        internal void Drag_Drop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            string[] files = new string[1];

            if (e != null && e.Data != null && (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop) ||
                e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop) || e.Data.GetDataPresent(typeof(string[]))))
            {
                if ((files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop)) != null)
                    Drop_Files(files);
            }
            return;
        }

        internal void Drop_Files(string[] files)
        {
            if (isDragMode && files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                    {
                        lock (_Lock)
                        {
                            this.textBoxSrc.Text = string.Empty;
                            this.textBoxOut.Text = string.Empty;
                            if (Path.GetExtension(file).Length > 7)
                                pictureBoxFileIn.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.encrypted;
                            else
                                pictureBoxFileIn.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.file;
                            this.labelFileIn.Text = Path.GetFileName(file);
                            Task.Run(() => PlaySoundFromResource("sound_arrow"));
                            // HashFiles = new HashSet<string>();
                            _dragDropEffect = System.Windows.Forms.DragDropEffects.None;
                            isDragMode = false;
                            SetGBoxText("Files Group Box");
                            break;
                        }
                    }
                }

            }

            Cursor.Current = DefaultCursor;
        }


        internal void menuFileOpen_Click(object sender, EventArgs e)
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


        internal void menuMainSave_Click(object sender, EventArgs e)
        {
            if (this.pictureBoxOutFile.Visible || labelOutputFile.Visible)
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

        private void menuFileExit_Click(object sender, EventArgs e)
        {
            foreach (var t in Program.tFormsNew)
            {
                try
                {
                    t.Close();
                }
                catch { }
                try { t.Dispose(); } catch { }
            }
            if (Program.tWinFormOld != null)
            {
                try
                {
                    Program.tWinFormOld.Close();
                }
                catch { }
                try { Program.tWinFormOld.Dispose(); } catch { }
            }

            try
            {
                Program.ReleaseCloseDisposeMutex();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
            }

            Application.ExitThread();
            Dispose();
            Application.Exit();
            Environment.Exit(0);
        }

        private void menuFileExit_Close(object sender, FormClosingEventArgs e)
        {
            menuFileExit_Click(sender, e);
        }
    }
}
