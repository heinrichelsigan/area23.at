namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    partial class SecureChat
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        protected internal System.ComponentModel.IContainer components = null;

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
            splitContainer = new SplitContainer();
            TextBoxSource = new TextBox();
            TextBoxDestionation = new TextBox();
            buttonEncode = new Button();
            buttonSave = new Button();
            buttonLoad = new Button();
            panelButtons = new Panel();
            buttonAddToPipeline = new Button();
            buttonClear = new Button();
            ComboBox_RemoteEndPoint = new ComboBox();
            buttonSecretKey = new Button();
            ComboBox_LocalEndPoint = new ComboBox();
            buttonHashIv = new Button();
            panelEnCodeCrypt = new Panel();
            textBoxEnter = new TextBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panelButtons.SuspendLayout();
            panelEnCodeCrypt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.BackColor = SystemColors.ControlLight;
            splitContainer.IsSplitterFixed = true;
            splitContainer.Location = new Point(108, 108);
            splitContainer.Margin = new Padding(0);
            splitContainer.MaximumSize = new Size(720, 400);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.AllowDrop = true;
            splitContainer.Panel1.BackgroundImageLayout = ImageLayout.None;
            splitContainer.Panel1.Controls.Add(TextBoxSource);
            splitContainer.Panel1MinSize = 340;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.BackgroundImageLayout = ImageLayout.None;
            splitContainer.Panel2.Controls.Add(TextBoxDestionation);
            splitContainer.Panel2MinSize = 340;
            splitContainer.Size = new Size(720, 400);
            splitContainer.SplitterDistance = 351;
            splitContainer.SplitterIncrement = 4;
            splitContainer.TabIndex = 41;
            splitContainer.TabStop = false;
            // 
            // TextBoxSource
            // 
            TextBoxSource.BackColor = SystemColors.GradientInactiveCaption;
            TextBoxSource.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSource.Dock = DockStyle.Fill;
            TextBoxSource.Font = new Font("Consolas", 9F);
            TextBoxSource.Location = new Point(0, 0);
            TextBoxSource.Margin = new Padding(1);
            TextBoxSource.MaxLength = 65536;
            TextBoxSource.Multiline = true;
            TextBoxSource.Name = "TextBoxSource";
            TextBoxSource.ScrollBars = ScrollBars.Both;
            TextBoxSource.Size = new Size(351, 400);
            TextBoxSource.TabIndex = 42;
            // 
            // TextBoxDestionation
            // 
            TextBoxDestionation.BackColor = SystemColors.GradientActiveCaption;
            TextBoxDestionation.BorderStyle = BorderStyle.FixedSingle;
            TextBoxDestionation.Dock = DockStyle.Fill;
            TextBoxDestionation.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TextBoxDestionation.Location = new Point(0, 0);
            TextBoxDestionation.Margin = new Padding(1);
            TextBoxDestionation.MaxLength = 65536;
            TextBoxDestionation.Multiline = true;
            TextBoxDestionation.Name = "TextBoxDestionation";
            TextBoxDestionation.ScrollBars = ScrollBars.Both;
            TextBoxDestionation.Size = new Size(365, 400);
            TextBoxDestionation.TabIndex = 43;
            // 
            // buttonEncode
            // 
            buttonEncode.BackColor = SystemColors.ButtonHighlight;
            buttonEncode.Location = new Point(8, 100);
            buttonEncode.Margin = new Padding(1);
            buttonEncode.Name = "buttonEncode";
            buttonEncode.Padding = new Padding(1);
            buttonEncode.Size = new Size(78, 28);
            buttonEncode.TabIndex = 51;
            buttonEncode.Text = "&Encode";
            buttonEncode.UseVisualStyleBackColor = false;
            buttonEncode.Click += Button_Encode_Click;
            // 
            // buttonSave
            // 
            buttonSave.BackColor = SystemColors.ButtonHighlight;
            buttonSave.Location = new Point(8, 56);
            buttonSave.Margin = new Padding(1);
            buttonSave.Name = "buttonSave";
            buttonSave.Padding = new Padding(1);
            buttonSave.Size = new Size(78, 28);
            buttonSave.TabIndex = 53;
            buttonSave.Text = "&Save";
            buttonSave.UseVisualStyleBackColor = false;
            buttonSave.Click += Button_Save_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.BackColor = SystemColors.ButtonHighlight;
            buttonLoad.ForeColor = SystemColors.ActiveCaptionText;
            buttonLoad.Location = new Point(8, 16);
            buttonLoad.Margin = new Padding(1);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Padding = new Padding(1);
            buttonLoad.Size = new Size(78, 28);
            buttonLoad.TabIndex = 52;
            buttonLoad.Text = "&Load";
            buttonLoad.UseVisualStyleBackColor = false;
            buttonLoad.Click += Button_Load_Click;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = SystemColors.ActiveCaption;
            panelButtons.Controls.Add(pictureBox1);
            panelButtons.Controls.Add(textBoxEnter);
            panelButtons.Controls.Add(buttonEncode);
            panelButtons.Controls.Add(buttonSave);
            panelButtons.Controls.Add(buttonLoad);
            panelButtons.ForeColor = SystemColors.ActiveCaptionText;
            panelButtons.Location = new Point(1, 535);
            panelButtons.Margin = new Padding(1);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(986, 137);
            panelButtons.TabIndex = 55;
            // 
            // buttonAddToPipeline
            // 
            buttonAddToPipeline.BackColor = SystemColors.ButtonHighlight;
            buttonAddToPipeline.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonAddToPipeline.ForeColor = SystemColors.ActiveCaptionText;
            buttonAddToPipeline.Location = new Point(450, 10);
            buttonAddToPipeline.Margin = new Padding(1);
            buttonAddToPipeline.Name = "buttonAddToPipeline";
            buttonAddToPipeline.Padding = new Padding(1);
            buttonAddToPipeline.Size = new Size(48, 28);
            buttonAddToPipeline.TabIndex = 5;
            buttonAddToPipeline.Text = "⇒";
            buttonAddToPipeline.UseVisualStyleBackColor = false;
            buttonAddToPipeline.Click += Button_AddToPipeline_Click;
            // 
            // buttonClear
            // 
            buttonClear.BackColor = SystemColors.ButtonHighlight;
            buttonClear.Font = new Font("Lucida Sans Unicode", 10F);
            buttonClear.ForeColor = SystemColors.ActiveCaptionText;
            buttonClear.Location = new Point(901, 9);
            buttonClear.Margin = new Padding(1);
            buttonClear.Name = "buttonClear";
            buttonClear.Padding = new Padding(1);
            buttonClear.Size = new Size(72, 28);
            buttonClear.TabIndex = 7;
            buttonClear.Text = "&Clear";
            buttonClear.UseVisualStyleBackColor = false;
            buttonClear.Click += Button_ClearPipeline_Click;
            // 
            // ComboBox_RemoteEndPoint
            // 
            ComboBox_RemoteEndPoint.BackColor = SystemColors.ControlLightLight;
            ComboBox_RemoteEndPoint.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_RemoteEndPoint.ForeColor = SystemColors.ControlText;
            ComboBox_RemoteEndPoint.FormattingEnabled = true;
            ComboBox_RemoteEndPoint.Items.AddRange(new object[] { "3DES", "2FISH", "3FISH", "AES", "Cast5", "Cast6", "Camellia", "Ghost28147", "Idea", "Noekeon", "Rijndael", "RC2", "RC532", "RC6", "Seed", "Serpent", "Skipjack", "Tea", "Tnepres", "XTea", "ZenMatrix" });
            ComboBox_RemoteEndPoint.Location = new Point(503, 12);
            ComboBox_RemoteEndPoint.Margin = new Padding(1);
            ComboBox_RemoteEndPoint.Name = "ComboBox_RemoteEndPoint";
            ComboBox_RemoteEndPoint.Size = new Size(324, 24);
            ComboBox_RemoteEndPoint.TabIndex = 4;
            // 
            // buttonSecretKey
            // 
            buttonSecretKey.BackColor = SystemColors.ButtonHighlight;
            buttonSecretKey.BackgroundImage = Properties.Resources.a_right_key;
            buttonSecretKey.BackgroundImageLayout = ImageLayout.None;
            buttonSecretKey.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonSecretKey.ForeColor = SystemColors.ActiveCaptionText;
            buttonSecretKey.Location = new Point(8, 10);
            buttonSecretKey.Margin = new Padding(1);
            buttonSecretKey.Name = "buttonSecretKey";
            buttonSecretKey.Padding = new Padding(1);
            buttonSecretKey.Size = new Size(48, 28);
            buttonSecretKey.TabIndex = 11;
            buttonSecretKey.UseVisualStyleBackColor = false;
            buttonSecretKey.Click += Button_SecretKey_Click;
            // 
            // ComboBox_LocalEndPoint
            // 
            ComboBox_LocalEndPoint.BackColor = SystemColors.ControlLightLight;
            ComboBox_LocalEndPoint.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_LocalEndPoint.ForeColor = SystemColors.ControlText;
            ComboBox_LocalEndPoint.FormattingEnabled = true;
            ComboBox_LocalEndPoint.Items.AddRange(new object[] { "hex16", "base16", "base32", "base64", "unix2unix", "html", "url" });
            ComboBox_LocalEndPoint.Location = new Point(120, 12);
            ComboBox_LocalEndPoint.Margin = new Padding(1);
            ComboBox_LocalEndPoint.Name = "ComboBox_LocalEndPoint";
            ComboBox_LocalEndPoint.Size = new Size(324, 24);
            ComboBox_LocalEndPoint.TabIndex = 3;
            // 
            // buttonHashIv
            // 
            buttonHashIv.BackColor = SystemColors.ButtonHighlight;
            buttonHashIv.BackgroundImage = Properties.Resources.a_hash5;
            buttonHashIv.BackgroundImageLayout = ImageLayout.None;
            buttonHashIv.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonHashIv.ForeColor = SystemColors.ActiveCaptionText;
            buttonHashIv.Location = new Point(64, 10);
            buttonHashIv.Margin = new Padding(1);
            buttonHashIv.Name = "buttonHashIv";
            buttonHashIv.Padding = new Padding(1);
            buttonHashIv.Size = new Size(48, 28);
            buttonHashIv.TabIndex = 13;
            buttonHashIv.UseVisualStyleBackColor = false;
            // 
            // panelEnCodeCrypt
            // 
            panelEnCodeCrypt.BackColor = SystemColors.ActiveCaption;
            panelEnCodeCrypt.Controls.Add(buttonHashIv);
            panelEnCodeCrypt.Controls.Add(ComboBox_LocalEndPoint);
            panelEnCodeCrypt.Controls.Add(buttonSecretKey);
            panelEnCodeCrypt.Controls.Add(ComboBox_RemoteEndPoint);
            panelEnCodeCrypt.Controls.Add(buttonClear);
            panelEnCodeCrypt.Controls.Add(buttonAddToPipeline);
            panelEnCodeCrypt.ForeColor = SystemColors.WindowText;
            panelEnCodeCrypt.Location = new Point(1, 28);
            panelEnCodeCrypt.Margin = new Padding(0);
            panelEnCodeCrypt.Name = "panelEnCodeCrypt";
            panelEnCodeCrypt.Size = new Size(983, 51);
            panelEnCodeCrypt.TabIndex = 2;
            // 
            // textBoxEnter
            // 
            textBoxEnter.Location = new Point(107, 16);
            textBoxEnter.Margin = new Padding(2);
            textBoxEnter.Multiline = true;
            textBoxEnter.Name = "textBoxEnter";
            textBoxEnter.Size = new Size(720, 112);
            textBoxEnter.TabIndex = 54;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(847, 16);
            pictureBox1.Margin = new Padding(2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(124, 112);
            pictureBox1.TabIndex = 55;
            pictureBox1.TabStop = false;
            // 
            // SecureChat
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(984, 721);
            Controls.Add(panelButtons);
            Controls.Add(panelEnCodeCrypt);
            Controls.Add(splitContainer);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "SecureChat";
            Text = "SecureChat";
            Load += SecureChat_Load;
            Controls.SetChildIndex(splitContainer, 0);
            Controls.SetChildIndex(panelEnCodeCrypt, 0);
            Controls.SetChildIndex(panelButtons, 0);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panelButtons.ResumeLayout(false);
            panelButtons.PerformLayout();
            panelEnCodeCrypt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer;
        private TextBox TextBoxSource;
        private TextBox TextBoxDestionation;
        private Button buttonEncode;
        private Button buttonSave;
        private Button buttonLoad;
        private Panel panelButtons;
        private Button buttonAddToPipeline;
        private Button buttonClear;
        private ComboBox ComboBox_RemoteEndPoint;
        private Button buttonSecretKey;
        private ComboBox ComboBox_LocalEndPoint;
        private Button buttonHashIv;
        private Panel panelEnCodeCrypt;
        private TextBox textBoxEnter;
        private PictureBox pictureBox1;
    }
}