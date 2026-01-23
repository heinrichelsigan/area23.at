namespace Area23.At.WinForm.WinRoachCore
{
    partial class WinForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            toolMenuMain = new ToolStripMenuItem();
            menuFileOpen = new ToolStripMenuItem();
            menuMainSave = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            toolStripMenuItem3 = new ToolStripMenuItem();
            toolStripMenuItem4 = new ToolStripMenuItem();
            menuMainSetPipe = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            toolStripMenuItem2 = new ToolStripMenuItem();
            menuMainDecrypt = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuFileExit = new ToolStripMenuItem();
            menuCompression = new ToolStripMenuItem();
            menu7z = new ToolStripMenuItem();
            menuBZip2 = new ToolStripMenuItem();
            menuGzip = new ToolStripMenuItem();
            menuZip = new ToolStripMenuItem();
            menuCompressionNone = new ToolStripMenuItem();
            menuEncoding = new ToolStripMenuItem();
            menuBase16 = new ToolStripMenuItem();
            menuHex16 = new ToolStripMenuItem();
            menuBase32 = new ToolStripMenuItem();
            menuHex32 = new ToolStripMenuItem();
            menuBase64 = new ToolStripMenuItem();
            menuUu = new ToolStripMenuItem();
            menuHash = new ToolStripMenuItem();
            menuHashBCrypt = new ToolStripMenuItem();
            menuHashMD5 = new ToolStripMenuItem();
            menuHashHex = new ToolStripMenuItem();
            menuHashOpenBsd = new ToolStripMenuItem();
            menuHashSha1 = new ToolStripMenuItem();
            menuHashSha256 = new ToolStripMenuItem();
            menuHashSha512 = new ToolStripMenuItem();
            menuHashSCrypt = new ToolStripMenuItem();
            menuSerialize = new ToolStripMenuItem();
            menuJson = new ToolStripMenuItem();
            menuXml = new ToolStripMenuItem();
            menuRaw = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            menuHelpHelp = new ToolStripMenuItem();
            comboBoxAlgo = new ComboBox();
            cipherEnumBindingSource2 = new BindingSource(components);
            cipherEnumBindingSource = new BindingSource(components);
            enumOptionsBindingSource = new BindingSource(components);
            textBoxKey = new TextBox();
            pictureBoxKey = new PictureBox();
            pictureBoxHash = new PictureBox();
            textBoxHash = new TextBox();
            pictureBoxSetPipeline = new PictureBox();
            buttonSetPipeline = new Button();
            buttonClear = new Button();
            pictureBoxFileIn = new PictureBox();
            pictureBoxAddAlgo = new PictureBox();
            textBoxPipe = new TextBox();
            labelFileIn = new Label();
            labelOutputFile = new Label();
            pictureBoxOutFile = new PictureBox();
            textBoxSrc = new TextBox();
            textBoxOut = new TextBox();
            buttonEncrypt = new Button();
            buttonDecrypt = new Button();
            cipherEnumBindingSource1 = new BindingSource(components);
            groupBoxFiles = new GroupBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)enumOptionsBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxKey).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHash).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSetPipeline).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFileIn).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAddAlgo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOutFile).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource1).BeginInit();
            groupBoxFiles.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolMenuMain, menuCompression, menuEncoding, menuHash, menuSerialize, menuHelp });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(801, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolMenuMain
            // 
            toolMenuMain.DropDownItems.AddRange(new ToolStripItem[] { menuFileOpen, menuMainSave, toolStripSeparator2, toolStripMenuItem3, toolStripMenuItem4, menuMainSetPipe, toolStripSeparator3, toolStripMenuItem2, menuMainDecrypt, toolStripSeparator1, menuFileExit });
            toolMenuMain.Name = "toolMenuMain";
            toolMenuMain.Size = new Size(46, 20);
            toolMenuMain.Text = "Main";
            // 
            // menuFileOpen
            // 
            menuFileOpen.Name = "menuFileOpen";
            menuFileOpen.ShortcutKeys = Keys.Control | Keys.O;
            menuFileOpen.Size = new Size(146, 22);
            menuFileOpen.Text = "Open";
            menuFileOpen.Click += menuFileOpen_Click;
            // 
            // menuMainSave
            // 
            menuMainSave.Name = "menuMainSave";
            menuMainSave.ShortcutKeys = Keys.Control | Keys.S;
            menuMainSave.Size = new Size(146, 22);
            menuMainSave.Text = "Save";
            menuMainSave.Click += menuMainSave_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(143, 6);
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new Size(146, 22);
            toolStripMenuItem3.Text = "Clear";
            toolStripMenuItem3.Click += Clear_Click;
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new Size(146, 22);
            toolStripMenuItem4.Text = "Hash Key";
            toolStripMenuItem4.Click += Hash_Click;
            // 
            // menuMainSetPipe
            // 
            menuMainSetPipe.Name = "menuMainSetPipe";
            menuMainSetPipe.Size = new Size(146, 22);
            menuMainSetPipe.Text = "Set Pipe";
            menuMainSetPipe.Click += SetPipeline_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(143, 6);
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(146, 22);
            toolStripMenuItem2.Text = "Encrypt";
            toolStripMenuItem2.Click += Encrypt_Click;
            // 
            // menuMainDecrypt
            // 
            menuMainDecrypt.Name = "menuMainDecrypt";
            menuMainDecrypt.Size = new Size(146, 22);
            menuMainDecrypt.Text = "Decrypt";
            menuMainDecrypt.Click += Decrypt_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(143, 6);
            // 
            // menuFileExit
            // 
            menuFileExit.Name = "menuFileExit";
            menuFileExit.ShortcutKeys = Keys.Alt | Keys.F4;
            menuFileExit.Size = new Size(146, 22);
            menuFileExit.Text = "Exit";
            // 
            // menuCompression
            // 
            menuCompression.DropDownItems.AddRange(new ToolStripItem[] { menu7z, menuBZip2, menuGzip, menuZip, menuCompressionNone });
            menuCompression.Name = "menuCompression";
            menuCompression.Size = new Size(89, 20);
            menuCompression.Text = "Compression";
            // 
            // menu7z
            // 
            menu7z.Enabled = false;
            menu7z.Name = "menu7z";
            menu7z.ShortcutKeys = Keys.Control | Keys.D7;
            menu7z.Size = new Size(146, 22);
            menu7z.Text = "7z";
            menu7z.Click += menuCompression_Click;
            // 
            // menuBZip2
            // 
            menuBZip2.Name = "menuBZip2";
            menuBZip2.ShortcutKeys = Keys.Control | Keys.B;
            menuBZip2.Size = new Size(146, 22);
            menuBZip2.Text = "BZip2";
            menuBZip2.Click += menuCompression_Click;
            // 
            // menuGzip
            // 
            menuGzip.Name = "menuGzip";
            menuGzip.ShortcutKeys = Keys.Control | Keys.G;
            menuGzip.Size = new Size(146, 22);
            menuGzip.Text = "GZip";
            menuGzip.Click += menuCompression_Click;
            // 
            // menuZip
            // 
            menuZip.Name = "menuZip";
            menuZip.ShortcutKeys = Keys.Control | Keys.Z;
            menuZip.Size = new Size(146, 22);
            menuZip.Text = "Zip";
            menuZip.Click += menuCompression_Click;
            // 
            // menuCompressionNone
            // 
            menuCompressionNone.Checked = true;
            menuCompressionNone.CheckState = CheckState.Checked;
            menuCompressionNone.Name = "menuCompressionNone";
            menuCompressionNone.ShortcutKeys = Keys.Control | Keys.N;
            menuCompressionNone.Size = new Size(146, 22);
            menuCompressionNone.Text = "None";
            menuCompressionNone.Click += menuCompression_Click;
            // 
            // menuEncoding
            // 
            menuEncoding.DropDownItems.AddRange(new ToolStripItem[] { menuBase16, menuHex16, menuBase32, menuHex32, menuBase64, menuUu });
            menuEncoding.Name = "menuEncoding";
            menuEncoding.ShortcutKeys = Keys.Alt | Keys.E;
            menuEncoding.Size = new Size(69, 20);
            menuEncoding.Text = "Encoding";
            // 
            // menuBase16
            // 
            menuBase16.Name = "menuBase16";
            menuBase16.Size = new Size(144, 22);
            menuBase16.Text = "Base16";
            menuBase16.Click += menuEncodingKind_Click;
            // 
            // menuHex16
            // 
            menuHex16.Name = "menuHex16";
            menuHex16.Size = new Size(144, 22);
            menuHex16.Text = "Hex16";
            menuHex16.Click += menuEncodingKind_Click;
            // 
            // menuBase32
            // 
            menuBase32.Name = "menuBase32";
            menuBase32.Size = new Size(144, 22);
            menuBase32.Text = "Base32";
            menuBase32.Click += menuEncodingKind_Click;
            // 
            // menuHex32
            // 
            menuHex32.Name = "menuHex32";
            menuHex32.Size = new Size(144, 22);
            menuHex32.Text = "Hex32";
            menuHex32.Click += menuEncodingKind_Click;
            // 
            // menuBase64
            // 
            menuBase64.Checked = true;
            menuBase64.CheckState = CheckState.Checked;
            menuBase64.Name = "menuBase64";
            menuBase64.Size = new Size(144, 22);
            menuBase64.Text = "Base64 Mime";
            menuBase64.Click += menuEncodingKind_Click;
            // 
            // menuUu
            // 
            menuUu.Name = "menuUu";
            menuUu.Size = new Size(144, 22);
            menuUu.Text = "Uu";
            menuUu.Click += menuEncodingKind_Click;
            // 
            // menuHash
            // 
            menuHash.DropDownItems.AddRange(new ToolStripItem[] { menuHashBCrypt, menuHashMD5, menuHashHex, menuHashOpenBsd, menuHashSha1, menuHashSha256, menuHashSha512, menuHashSCrypt });
            menuHash.Name = "menuHash";
            menuHash.Size = new Size(46, 20);
            menuHash.Text = "Hash";
            // 
            // menuHashBCrypt
            // 
            menuHashBCrypt.Name = "menuHashBCrypt";
            menuHashBCrypt.Size = new Size(156, 22);
            menuHashBCrypt.Text = "B-Crypt";
            menuHashBCrypt.Click += menuHash_Click;
            // 
            // menuHashMD5
            // 
            menuHashMD5.Name = "menuHashMD5";
            menuHashMD5.Size = new Size(156, 22);
            menuHashMD5.Tag = "";
            menuHashMD5.Text = "MD5";
            menuHashMD5.Click += menuHash_Click;
            // 
            // menuHashHex
            // 
            menuHashHex.Checked = true;
            menuHashHex.CheckState = CheckState.Checked;
            menuHashHex.Name = "menuHashHex";
            menuHashHex.Size = new Size(156, 22);
            menuHashHex.Text = "Hex";
            menuHashHex.Click += menuHash_Click;
            // 
            // menuHashOpenBsd
            // 
            menuHashOpenBsd.Name = "menuHashOpenBsd";
            menuHashOpenBsd.Size = new Size(156, 22);
            menuHashOpenBsd.Text = "OpenBsd-Crypt";
            menuHashOpenBsd.Click += menuHash_Click;
            // 
            // menuHashSha1
            // 
            menuHashSha1.Name = "menuHashSha1";
            menuHashSha1.Size = new Size(156, 22);
            menuHashSha1.Text = "Sha1";
            menuHashSha1.Click += menuHash_Click;
            // 
            // menuHashSha256
            // 
            menuHashSha256.Name = "menuHashSha256";
            menuHashSha256.Size = new Size(156, 22);
            menuHashSha256.Text = "Sha256";
            menuHashSha256.Click += menuHash_Click;
            // 
            // menuHashSha512
            // 
            menuHashSha512.Name = "menuHashSha512";
            menuHashSha512.Size = new Size(156, 22);
            menuHashSha512.Text = "Sha512";
            menuHashSha512.Click += menuHash_Click;
            // 
            // menuHashSCrypt
            // 
            menuHashSCrypt.Name = "menuHashSCrypt";
            menuHashSCrypt.Size = new Size(156, 22);
            menuHashSCrypt.Text = "S-Crypt";
            menuHashSCrypt.Click += menuHash_Click;
            // 
            // menuSerialize
            // 
            menuSerialize.DropDownItems.AddRange(new ToolStripItem[] { menuJson, menuXml, menuRaw });
            menuSerialize.Name = "menuSerialize";
            menuSerialize.ShortcutKeys = Keys.Alt | Keys.S;
            menuSerialize.Size = new Size(61, 20);
            menuSerialize.Text = "Serialize";
            // 
            // menuJson
            // 
            menuJson.Name = "menuJson";
            menuJson.ShortcutKeys = Keys.Control | Keys.J;
            menuJson.Size = new Size(137, 22);
            menuJson.Text = "Json";
            // 
            // menuXml
            // 
            menuXml.Name = "menuXml";
            menuXml.ShortcutKeys = Keys.Control | Keys.X;
            menuXml.Size = new Size(137, 22);
            menuXml.Text = "Xml";
            // 
            // menuRaw
            // 
            menuRaw.Name = "menuRaw";
            menuRaw.ShortcutKeys = Keys.Control | Keys.R;
            menuRaw.Size = new Size(137, 22);
            menuRaw.Text = "Raw";
            // 
            // menuHelp
            // 
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuAbout, menuHelpHelp });
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(24, 20);
            menuHelp.Text = "?";
            // 
            // menuAbout
            // 
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(141, 22);
            menuAbout.Text = "About";
            // 
            // menuHelpHelp
            // 
            menuHelpHelp.Name = "menuHelpHelp";
            menuHelpHelp.ShortcutKeys = Keys.Alt | Keys.F3;
            menuHelpHelp.Size = new Size(141, 22);
            menuHelpHelp.Text = "Help";
            // 
            // comboBoxAlgo
            // 
            comboBoxAlgo.FormattingEnabled = true;
            comboBoxAlgo.Location = new Point(12, 103);
            comboBoxAlgo.Margin = new Padding(2);
            comboBoxAlgo.Name = "comboBoxAlgo";
            comboBoxAlgo.Size = new Size(140, 23);
            comboBoxAlgo.TabIndex = 2;
            // 
            // cipherEnumBindingSource2
            // 
            cipherEnumBindingSource2.DataSource = typeof(Area23.At.Framework.Core.Crypt.Cipher.CipherEnum);
            // 
            // cipherEnumBindingSource
            // 
            cipherEnumBindingSource.DataSource = typeof(Area23.At.Framework.Core.Crypt.Cipher.CipherEnum);
            // 
            // enumOptionsBindingSource
            // 
            enumOptionsBindingSource.DataSource = typeof(Area23.At.Framework.Core.Crypt.Cipher.CipherEnum);
            // 
            // textBoxKey
            // 
            textBoxKey.Location = new Point(53, 31);
            textBoxKey.Name = "textBoxKey";
            textBoxKey.Size = new Size(642, 23);
            textBoxKey.TabIndex = 3;
            textBoxKey.Text = "anonymous@ftp.cdrom.com";
            // 
            // pictureBoxKey
            // 
            pictureBoxKey.Image = Properties.Resource.a_right_key;
            pictureBoxKey.Location = new Point(10, 27);
            pictureBoxKey.Margin = new Padding(1);
            pictureBoxKey.Name = "pictureBoxKey";
            pictureBoxKey.Size = new Size(32, 27);
            pictureBoxKey.TabIndex = 4;
            pictureBoxKey.TabStop = false;
            // 
            // pictureBoxHash
            // 
            pictureBoxHash.Image = Properties.Resource.a_hash;
            pictureBoxHash.Location = new Point(10, 65);
            pictureBoxHash.Margin = new Padding(1);
            pictureBoxHash.Name = "pictureBoxHash";
            pictureBoxHash.Size = new Size(32, 27);
            pictureBoxHash.TabIndex = 5;
            pictureBoxHash.TabStop = false;
            pictureBoxHash.Click += Hash_Click;
            // 
            // textBoxHash
            // 
            textBoxHash.Location = new Point(53, 65);
            textBoxHash.Name = "textBoxHash";
            textBoxHash.ReadOnly = true;
            textBoxHash.Size = new Size(642, 23);
            textBoxHash.TabIndex = 6;
            // 
            // pictureBoxSetPipeline
            // 
            pictureBoxSetPipeline.Image = Properties.Resource.key_ring1;
            pictureBoxSetPipeline.Location = new Point(759, 99);
            pictureBoxSetPipeline.Margin = new Padding(1);
            pictureBoxSetPipeline.Name = "pictureBoxSetPipeline";
            pictureBoxSetPipeline.Size = new Size(32, 27);
            pictureBoxSetPipeline.TabIndex = 7;
            pictureBoxSetPipeline.TabStop = false;
            // 
            // buttonSetPipeline
            // 
            buttonSetPipeline.Location = new Point(703, 65);
            buttonSetPipeline.Name = "buttonSetPipeline";
            buttonSetPipeline.Size = new Size(88, 23);
            buttonSetPipeline.TabIndex = 8;
            buttonSetPipeline.Text = "Set Pipeline";
            buttonSetPipeline.UseVisualStyleBackColor = true;
            buttonSetPipeline.Click += SetPipeline_Click;
            // 
            // buttonClear
            // 
            buttonClear.Location = new Point(703, 31);
            buttonClear.Margin = new Padding(1);
            buttonClear.Name = "buttonClear";
            buttonClear.Size = new Size(88, 23);
            buttonClear.TabIndex = 9;
            buttonClear.Text = "Clear Form";
            buttonClear.UseVisualStyleBackColor = true;
            buttonClear.Click += Clear_Click;
            // 
            // pictureBoxFileIn
            // 
            pictureBoxFileIn.Image = Properties.Resource.file;
            pictureBoxFileIn.Location = new Point(20, 24);
            pictureBoxFileIn.Name = "pictureBoxFileIn";
            pictureBoxFileIn.Size = new Size(66, 69);
            pictureBoxFileIn.TabIndex = 10;
            pictureBoxFileIn.TabStop = false;
            // 
            // pictureBoxAddAlgo
            // 
            pictureBoxAddAlgo.Image = Properties.Resource.AddAesArrowHover;
            pictureBoxAddAlgo.Location = new Point(156, 101);
            pictureBoxAddAlgo.Margin = new Padding(1);
            pictureBoxAddAlgo.Name = "pictureBoxAddAlgo";
            pictureBoxAddAlgo.Size = new Size(32, 27);
            pictureBoxAddAlgo.TabIndex = 11;
            pictureBoxAddAlgo.TabStop = false;
            pictureBoxAddAlgo.Click += pictureBoxAddAlgo_Click;
            // 
            // textBoxPipe
            // 
            textBoxPipe.Location = new Point(192, 103);
            textBoxPipe.Name = "textBoxPipe";
            textBoxPipe.ReadOnly = true;
            textBoxPipe.Size = new Size(553, 23);
            textBoxPipe.TabIndex = 12;
            // 
            // labelFileIn
            // 
            labelFileIn.AutoSize = true;
            labelFileIn.Location = new Point(20, 96);
            labelFileIn.Name = "labelFileIn";
            labelFileIn.Size = new Size(67, 16);
            labelFileIn.TabIndex = 13;
            labelFileIn.Text = "[Input File]";
            // 
            // labelOutputFile
            // 
            labelOutputFile.AutoSize = true;
            labelOutputFile.Location = new Point(410, 96);
            labelOutputFile.Name = "labelOutputFile";
            labelOutputFile.Size = new Size(77, 16);
            labelOutputFile.TabIndex = 15;
            labelOutputFile.Text = "[Output File]";
            labelOutputFile.Visible = false;
            // 
            // pictureBoxOutFile
            // 
            pictureBoxOutFile.Image = Properties.Resource.encrypted;
            pictureBoxOutFile.Location = new Point(410, 21);
            pictureBoxOutFile.Margin = new Padding(2);
            pictureBoxOutFile.Name = "pictureBoxOutFile";
            pictureBoxOutFile.Size = new Size(66, 69);
            pictureBoxOutFile.TabIndex = 14;
            pictureBoxOutFile.TabStop = false;
            pictureBoxOutFile.Visible = false;
            // 
            // textBoxSrc
            // 
            textBoxSrc.Font = new Font("Lucida Sans Unicode", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxSrc.Location = new Point(0, 300);
            textBoxSrc.Margin = new Padding(2);
            textBoxSrc.Multiline = true;
            textBoxSrc.Name = "textBoxSrc";
            textBoxSrc.Size = new Size(396, 212);
            textBoxSrc.TabIndex = 16;
            // 
            // textBoxOut
            // 
            textBoxOut.Font = new Font("Lucida Sans Unicode", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxOut.Location = new Point(400, 300);
            textBoxOut.Margin = new Padding(2);
            textBoxOut.Multiline = true;
            textBoxOut.Name = "textBoxOut";
            textBoxOut.ReadOnly = true;
            textBoxOut.Size = new Size(396, 212);
            textBoxOut.TabIndex = 17;
            // 
            // buttonEncrypt
            // 
            buttonEncrypt.Location = new Point(10, 267);
            buttonEncrypt.Margin = new Padding(1);
            buttonEncrypt.Name = "buttonEncrypt";
            buttonEncrypt.Size = new Size(88, 23);
            buttonEncrypt.TabIndex = 18;
            buttonEncrypt.Text = "Encrypt";
            buttonEncrypt.UseVisualStyleBackColor = true;
            buttonEncrypt.Click += Encrypt_Click;
            // 
            // buttonDecrypt
            // 
            buttonDecrypt.Location = new Point(400, 267);
            buttonDecrypt.Margin = new Padding(1);
            buttonDecrypt.Name = "buttonDecrypt";
            buttonDecrypt.Size = new Size(88, 23);
            buttonDecrypt.TabIndex = 19;
            buttonDecrypt.Text = "Decrypt";
            buttonDecrypt.UseVisualStyleBackColor = true;
            buttonDecrypt.Click += Decrypt_Click;
            // 
            // cipherEnumBindingSource1
            // 
            cipherEnumBindingSource1.DataSource = typeof(Area23.At.Framework.Core.Crypt.Cipher.CipherEnum);
            // 
            // groupBoxFiles
            // 
            groupBoxFiles.AllowDrop = true;
            groupBoxFiles.Controls.Add(pictureBoxFileIn);
            groupBoxFiles.Controls.Add(labelFileIn);
            groupBoxFiles.Controls.Add(pictureBoxOutFile);
            groupBoxFiles.Controls.Add(labelOutputFile);
            groupBoxFiles.Font = new Font("Lucida Sans Unicode", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBoxFiles.Location = new Point(12, 137);
            groupBoxFiles.Margin = new Padding(2);
            groupBoxFiles.Name = "groupBoxFiles";
            groupBoxFiles.Padding = new Padding(2);
            groupBoxFiles.Size = new Size(780, 124);
            groupBoxFiles.TabIndex = 20;
            groupBoxFiles.TabStop = false;
            groupBoxFiles.Text = "Files (drag files into)";
            // 
            // WinForm
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(801, 521);
            Controls.Add(groupBoxFiles);
            Controls.Add(buttonDecrypt);
            Controls.Add(buttonEncrypt);
            Controls.Add(textBoxOut);
            Controls.Add(textBoxSrc);
            Controls.Add(textBoxPipe);
            Controls.Add(pictureBoxAddAlgo);
            Controls.Add(buttonClear);
            Controls.Add(buttonSetPipeline);
            Controls.Add(pictureBoxSetPipeline);
            Controls.Add(textBoxHash);
            Controls.Add(pictureBoxHash);
            Controls.Add(pictureBoxKey);
            Controls.Add(textBoxKey);
            Controls.Add(comboBoxAlgo);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "WinForm";
            Text = "WinForm";
            Load += WinForm_Load;
            DragDrop += Drag_Drop;
            DragEnter += Drag_Enter;
            DragOver += Drag_Over;
            DragLeave += Drag_Leave;
            QueryContinueDrag += QueryContinue_Drag;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource2).EndInit();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)enumOptionsBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxKey).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxHash).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSetPipeline).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxFileIn).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxAddAlgo).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxOutFile).EndInit();
            ((System.ComponentModel.ISupportInitialize)cipherEnumBindingSource1).EndInit();
            groupBoxFiles.ResumeLayout(false);
            groupBoxFiles.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem toolMenuMain;
        private ToolStripMenuItem menuFileOpen;
        private ToolStripMenuItem menuMainDecrypt;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem menuFileExit;
        private ToolStripMenuItem menuCompression;
        private ToolStripMenuItem menuBZip2;
        private ToolStripMenuItem menuZip;
        private ToolStripMenuItem menu7z;
        private ToolStripMenuItem menuGzip;
        private ToolStripMenuItem menuCompressionNone;
        private ToolStripMenuItem menuEncoding;
        private ToolStripMenuItem menuBase16;
        private ToolStripMenuItem menuHex16;
        private ToolStripMenuItem menuBase32;
        private ToolStripMenuItem menuHex32;
        private ToolStripMenuItem menuBase64;
        private ToolStripMenuItem menuUu;
        private ToolStripMenuItem menuSerialize;
        private ToolStripMenuItem menuJson;
        private ToolStripMenuItem menuXml;
        private ToolStripMenuItem menuRaw;
        private ToolStripMenuItem menuHash;
        private ToolStripMenuItem menuHashBCrypt;
        private ToolStripMenuItem menuHashSCrypt;
        private ToolStripMenuItem menuHashMD5;
        private ToolStripMenuItem menuHashSha1;
        private ToolStripMenuItem menuHashSha512;
        private ToolStripMenuItem menuHashOpenBsd;
        private ToolStripMenuItem menuHashSha256;
        private ToolStripMenuItem menuHashHex;
        private ComboBox comboBoxAlgo;
        private TextBox textBoxKey;
        private PictureBox pictureBoxKey;
        private PictureBox pictureBoxHash;
        private TextBox textBoxHash;
        private PictureBox pictureBoxSetPipeline;
        private Button buttonSetPipeline;
        private Button buttonClear;
        private BindingSource enumOptionsBindingSource;
        private BindingSource cipherEnumBindingSource;
        private PictureBox pictureBoxFileIn;
        private PictureBox pictureBoxAddAlgo;
        private TextBox textBoxPipe;
        private Label labelFileIn;
        private Label labelOutputFile;
        private PictureBox pictureBoxOutFile;
        private TextBox textBoxSrc;
        private TextBox textBoxOut;
        private ToolStripMenuItem menuMainSave;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem menuMainSetPipe;
        private ToolStripSeparator toolStripSeparator3;
        private Button buttonEncrypt;
        private Button buttonDecrypt;
        private BindingSource cipherEnumBindingSource2;
        private BindingSource cipherEnumBindingSource1;
        private GroupBox groupBoxFiles;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuAbout;
        private ToolStripMenuItem menuHelpHelp;
    }
}