using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Zfx;
using Area23.At.WinForm.TWinFormCore.Helper;
using Area23.At.WinForm.TWinFormCore.Properties;
using Area23.At.WinForm.TWinFormCore;
using System.Media;
using Area23.At.WinForm.TWinFormCore.Gui.Forms;

namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    public partial class EncryptForm : System.Windows.Forms.Form
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
            this.textBoxKey.Text = GetEmailFromRegistry();
        }


        #region MenuCompressionEncodingZipHash

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
            menuItemNone.Checked = false;
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
            if (menuItemNone.Checked) return EncodingType.None;
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

        #endregion MenuCompressionEncodingZipHash

        #region ButtonPictureBoxClickEvents

        private void pictureBoxKey_Click(object sender, EventArgs e)
        {
            this.textBoxKey.Text = GetEmailFromRegistry();
        }

        private void pictureBoxAddAlgo_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxAlgo.SelectedText) && Enum.TryParse<CipherEnum>(comboBoxAlgo.SelectedText, out CipherEnum cipherEnum))
            {
                this.textBoxPipe.Text += cipherEnum.ToString() + ";";
            }
        }


        private void pictureBoxDelete_Click(object sender, EventArgs e)
        {
            this.textBoxPipe.Text = "";
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

        #endregion ButtonPictureBoxClickEvents


        #region EncryptDecrypt_Click

        /// <summary>
        /// Encrypt_Click - encrypts text or file with given key, hash, zip and encoding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Encrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxHash.Text) && !string.IsNullOrEmpty(this.textBoxKey.Text))
            {
                CipherEnum[] pipeAlgos = CipherEnumExtensions.FromString(this.textBoxKey.Text);
                CipherPipe cPipe = new CipherPipe(pipeAlgos);

                if (!string.IsNullOrEmpty(this.textBoxSrc.Text))
                {
                    if (menuItemNone.Checked)
                        SetEncoding(menuBase64);

                    string encrypted = cPipe.EncrpytTextGoRounds(this.textBoxSrc.Text, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                    this.textBoxOut.Text = encrypted;
                }
                if (!string.IsNullOrEmpty(this.labelFileIn.Text) && !labelFileIn.Text.StartsWith("["))
                {
                    foreach (string file in HashFiles)
                    {
                        if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                        {
                            if (Path.GetFileName(file) == labelFileIn.Text)
                            {
                                // CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);                                
                                byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                                byte[] outBytes = cPipe.EncrpytFileBytesGoRounds(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                                string outFilePath = (file + GetZip().GetZipFileExtension() + "." + cPipe.PipeString + "." + GetHash());
                                SaveBytesDialog(outBytes, ref outFilePath);
                                pictureBoxOutFile.Visible = true;
                                pictureBoxOutFile.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.encrypted;
                                string outFileName = Path.GetFileName(outFilePath);
                                labelOutputFile.Text = outFileName;
                                labelOutputFile.Visible = true;
                                HashFiles.Add(outFilePath);

                                break;
                            }
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Decrypt_Click - decrypts text or file with given key, hash, zip and encoding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Decrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.textBoxHash.Text) && !string.IsNullOrEmpty(this.textBoxKey.Text))
            {

                CipherEnum[] pipeAlgos = CipherEnumExtensions.FromString(this.textBoxKey.Text);
                CipherPipe cPipe = new CipherPipe(pipeAlgos);

                if (!string.IsNullOrEmpty(this.textBoxHash.Text))
                {
                    if (menuItemNone.Checked)
                        SetEncoding(menuBase64);

                    // CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                    string decrypted = cPipe.DecryptTextRoundsGo(this.textBoxSrc.Text, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                    this.textBoxOut.Text = decrypted;
                }
                if (!string.IsNullOrEmpty(this.labelFileIn.Text) && !labelFileIn.Text.StartsWith("["))
                {
                    foreach (string file in HashFiles)
                    {
                        if (!string.IsNullOrEmpty(file) && System.IO.File.Exists(file))
                        {
                            if (Path.GetFileName(file) == labelFileIn.Text)
                            {
                                // CipherPipe cPipe = new CipherPipe(this.textBoxKey.Text, this.textBoxHash.Text);
                                byte[] fileBytes = System.IO.File.ReadAllBytes(file);
                                byte[] outBytes = cPipe.DecryptFileBytesRoundsGo(fileBytes, this.textBoxKey.Text, this.textBoxHash.Text, GetEncoding(), GetZip(), GetHash());
                                string outFileDecrypt = file.Replace(GetZip().GetZipFileExtension() + "." + cPipe.PipeString + "." + GetHash(), "");
                                SaveBytesDialog(outBytes, ref outFileDecrypt);
                                HashFiles.Add(outFileDecrypt);
                                pictureBoxOutFile.Visible = true;
                                pictureBoxOutFile.Image = Area23.At.WinForm.TWinFormCore.Properties.Resources.decrypted;
                                pictureBoxOutFile.Tag = outFileDecrypt;
                                labelOutputFile.Text = Path.GetFileName(outFileDecrypt);
                                labelOutputFile.Visible = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public string GetEmailFromRegistry()
        {
            string userEmail = "anonymous@ftp.cdrom.com";
            try
            {
                userEmail = (string)RegistryAccessor.GetRegistryEntry(Microsoft.Win32.RegistryHive.CurrentUser, "Software\\Microsoft\\OneDrive\\Accounts\\Personal", "UserEmail");
                if (userEmail.Contains('@') && userEmail.Contains('.'))
                    return userEmail;
            }
            catch (Exception)
            {
            }
            try
            {
                userEmail = (string)RegistryAccessor.GetRegistryEntry(Microsoft.Win32.RegistryHive.CurrentUser,
                    "Software\\Microsoft\\VSCommon\\ConnectedUser\\IdeUserV4\\Cache", "EmailAddress");
                if (userEmail.Contains("@") && userEmail.Contains("."))
                    return userEmail;
            }
            catch (Exception)
            {
            }
            userEmail = "anonymous@ftp.cdrom.com";
            return userEmail;
        }

        #endregion EncryptDecrypt_Click


        #region DragNDrop

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
                    Icon iconFileWork = new Icon(TWinFormCore.Properties.Resources.icon_file_working, new Size(32, 32));
                    Icon iconFileWarn = new Icon(TWinFormCore.Properties.Resources.icon_file_warning, new Size(32, 32));
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
                NormalCursor = new Cursor(TWinFormCore.Properties.Resources.icon_file_warning.Handle);
                NoDropCursor = new Cursor(TWinFormCore.Properties.Resources.icon_file_working.Handle);
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
                e.Data.GetDataPresent(typeof(string[]))))
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

        #endregion DragNDrop


        #region OpenSave

        /// <summary>
        /// menuFileOpen_Click opens a file dialog to select a file to encrypt/decrypt
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
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


        /// <summary>
        /// SaveBytesDialog saves byte array to file with save file dialog 
        /// </summary>
        /// <param name="fileBytes">byte array to save</param>
        /// <param name="outFilePath">ref will be returned; calculated outFilePath</param>
        /// <returns>true if saved, false if not saved</returns>
        internal bool SaveBytesDialog(byte[] fileBytes, ref string outFilePath)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            outFilePath = outFilePath ?? string.Empty;
            if (fileBytes != null && fileBytes.Length > 0)
            {
                dialog.Title = "Save File";
                dialog.CheckPathExists = true;
                dialog.RestoreDirectory = true;
                dialog.SupportMultiDottedExtensions = true;
                dialog.AddExtension = true;
                dialog.FileName = Path.GetFileName(outFilePath);
                dialog.DefaultExt = Path.GetExtension(outFilePath);
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrEmpty(dialog.FileName))
                {
                    outFilePath = dialog.FileName;
                    try
                    {
                        File.WriteAllBytes(outFilePath, fileBytes);
                    }
                    catch (Exception ex)
                    {
                        Area23Log.LogOriginMsg(this.Name, $"Exception in SaveBytesDialog for file: \"{outFilePath}\".\n{ex}");
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        internal void menuMainSave_Click(object sender, EventArgs e)
        {
            if (this.pictureBoxOutFile.Visible || labelOutputFile.Visible)
            {
                byte[] fileBytes = new byte[0];
                string fileName = "";

                foreach (string filePath in HashFiles)
                {
                    if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                    {
                        if (Path.GetFileName(filePath) == labelOutputFile.Text)
                        {
                            fileName = filePath;
                            fileBytes = System.IO.File.ReadAllBytes(filePath);
                            break;
                        }
                    }
                }

                if (SaveBytesDialog(fileBytes, ref fileName))
                {
                    if (HashFiles.Contains(fileName))
                        HashFiles.Remove(fileName);
                    this.pictureBoxOutFile.Visible = false;
                    this.labelOutputFile.Visible = false;
                }


            }
        }

        #endregion OpenSave


        #region AboutHelpExitClose

        private void menuAbout_Click(object sender, EventArgs e)
        {
            TransparentDialog transparentDialog = new TransparentDialog();
            transparentDialog.ShowDialog(this);
        }


        private void menuFileExit_Click(object sender, EventArgs e)
        {
            try
            {
                Program.ReleaseCloseDisposeMutex();
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("BaseChatForm", "menuFileExit_Click", ex);
            }
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("BaseChatForm", "menuFileExit_Click", ex);
            }

            Application.ExitThread();
            Dispose();
            Application.Exit();
            Environment.Exit(0);
        }

        private void menuFileExit_Close(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
            Application.Exit();
            Environment.Exit(0);
        }

        #endregion AboutHelpExitClose


        #region Media Methods

        /// <summary>
        /// PlaySoundFromResource - plays a sound embedded in application ressource file
        /// </summary>
        /// <param name="soundName">unique qualified name for sound</param>
        protected static bool PlaySoundFromResource(string soundName)
        {
            bool played = false;
            if (true)
            {
                UnmanagedMemoryStream stream = (UnmanagedMemoryStream)Resources.ResourceManager.GetStream(soundName);


                if (stream != null)
                {
                    try
                    {
                        // Construct the sound player
                        SoundPlayer player = new SoundPlayer(stream);
                        player.Play();
                        played = true;
                        stream.Close();
                    }
                    catch (Exception exSound)
                    {
                        Area23Log.LogOriginMsgEx("EncryptForm", $"PlaySoundFromResource(string soundName = {soundName})", exSound);
                        played = false;
                    }
                    //fixed (byte* bufferPtr = &bytes[0])
                    //{
                    //    System.IO.UnmanagedMemoryStream ums = new UnmanagedMemoryStream(bufferPtr, bytes.Length);
                    //    SoundPlayer player = new SoundPlayer(ums);                        
                    //    player.Play();
                    //}
                }
            }

            return played;
        }



        protected virtual async Task<bool> PlaySoundFromResourcesAsync(string soundName)
        {
            return await Task.Run(() => PlaySoundFromResource(soundName));
        }

        private void pictureOutBoxFile_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(pictureBoxOutFile.Tag.ToString()) && pictureBoxOutFile.Visible &&
                File.Exists(pictureBoxOutFile.Tag.ToString()))
            {
                ProcessCmd.Execute("explorer", pictureBoxOutFile.Tag.ToString());
            }
        }

        #endregion Media Methods




    }
}
