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
            buttonDelete = new Button();
            buttonSend = new Button();
            panelSource = new Panel();
            buttonSecretKey = new Button();
            pictureBoxYou = new PictureBox();
            pictureBoxSource = new PictureBox();
            buttonAddToPipeline = new Button();
            ComboBox_RemoteEndPoint = new ComboBox();
            ComboBox_LocalEndPoint = new ComboBox();
            buttonHashIv = new Button();
            panelEnCodeCrypt = new Panel();
            pictureBoxDestination = new PictureBox();
            richTextBoxChat = new RichTextBox();
            pictureBoxPartner = new PictureBox();
            panelDestination = new Panel();
            buttonExit = new Button();
            buttonReload = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panelSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxYou).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).BeginInit();
            panelEnCodeCrypt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDestination).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPartner).BeginInit();
            panelDestination.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.BackColor = SystemColors.ControlLight;
            splitContainer.IsSplitterFixed = true;
            splitContainer.Location = new Point(148, 72);
            splitContainer.Margin = new Padding(0);
            splitContainer.MaximumSize = new Size(800, 600);
            splitContainer.MinimumSize = new Size(320, 200);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.AllowDrop = true;
            splitContainer.Panel1.BackgroundImageLayout = ImageLayout.None;
            splitContainer.Panel1.Controls.Add(TextBoxSource);
            splitContainer.Panel1MinSize = 300;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.BackgroundImageLayout = ImageLayout.None;
            splitContainer.Panel2.Controls.Add(TextBoxDestionation);
            splitContainer.Panel2MinSize = 300;
            splitContainer.Size = new Size(680, 472);
            splitContainer.SplitterDistance = 336;
            splitContainer.SplitterIncrement = 8;
            splitContainer.SplitterWidth = 8;
            splitContainer.TabIndex = 20;
            splitContainer.TabStop = false;
            // 
            // TextBoxSource
            // 
            TextBoxSource.BackColor = SystemColors.GradientActiveCaption;
            TextBoxSource.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSource.Dock = DockStyle.Fill;
            TextBoxSource.Font = new Font("Lucida Sans Unicode", 10F);
            TextBoxSource.Location = new Point(0, 0);
            TextBoxSource.Margin = new Padding(1);
            TextBoxSource.MaxLength = 65536;
            TextBoxSource.Multiline = true;
            TextBoxSource.Name = "TextBoxSource";
            TextBoxSource.ScrollBars = ScrollBars.Both;
            TextBoxSource.Size = new Size(336, 472);
            TextBoxSource.TabIndex = 23;
            // 
            // TextBoxDestionation
            // 
            TextBoxDestionation.BackColor = SystemColors.GradientInactiveCaption;
            TextBoxDestionation.BorderStyle = BorderStyle.FixedSingle;
            TextBoxDestionation.Dock = DockStyle.Fill;
            TextBoxDestionation.Font = new Font("Lucida Sans Unicode", 10F);
            TextBoxDestionation.Location = new Point(0, 0);
            TextBoxDestionation.Margin = new Padding(1);
            TextBoxDestionation.MaxLength = 65536;
            TextBoxDestionation.Multiline = true;
            TextBoxDestionation.Name = "TextBoxDestionation";
            TextBoxDestionation.ScrollBars = ScrollBars.Both;
            TextBoxDestionation.Size = new Size(336, 472);
            TextBoxDestionation.TabIndex = 43;
            // 
            // buttonDelete
            // 
            buttonDelete.BackColor = SystemColors.ButtonHighlight;
            buttonDelete.Location = new Point(12, 576);
            buttonDelete.Margin = new Padding(1);
            buttonDelete.Name = "buttonDelete";
            buttonDelete.Padding = new Padding(1);
            buttonDelete.Size = new Size(120, 28);
            buttonDelete.TabIndex = 48;
            buttonDelete.Text = "&Delete";
            buttonDelete.UseVisualStyleBackColor = false;
            buttonDelete.Click += Button_Encode_Click;
            // 
            // buttonSend
            // 
            buttonSend.BackColor = SystemColors.ButtonHighlight;
            buttonSend.Location = new Point(12, 536);
            buttonSend.Margin = new Padding(1);
            buttonSend.Name = "buttonSend";
            buttonSend.Padding = new Padding(1);
            buttonSend.Size = new Size(120, 28);
            buttonSend.TabIndex = 47;
            buttonSend.Text = "&Send";
            buttonSend.UseVisualStyleBackColor = false;
            buttonSend.Click += Button_Save_Click;
            // 
            // panelSource
            // 
            panelSource.BackColor = SystemColors.Control;
            panelSource.Controls.Add(buttonDelete);
            panelSource.Controls.Add(pictureBoxYou);
            panelSource.Controls.Add(buttonSend);
            panelSource.Controls.Add(pictureBoxSource);
            panelSource.ForeColor = SystemColors.ActiveCaptionText;
            panelSource.Location = new Point(0, 72);
            panelSource.Margin = new Padding(0);
            panelSource.Name = "panelSource";
            panelSource.Size = new Size(148, 610);
            panelSource.TabIndex = 40;
            // 
            // buttonSecretKey
            // 
            buttonSecretKey.BackColor = SystemColors.ButtonHighlight;
            buttonSecretKey.BackgroundImage = Properties.Resources.a_right_key;
            buttonSecretKey.BackgroundImageLayout = ImageLayout.Center;
            buttonSecretKey.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonSecretKey.ForeColor = SystemColors.ActiveCaptionText;
            buttonSecretKey.Location = new Point(87, 1);
            buttonSecretKey.Margin = new Padding(1);
            buttonSecretKey.Name = "buttonSecretKey";
            buttonSecretKey.Padding = new Padding(1);
            buttonSecretKey.Size = new Size(48, 28);
            buttonSecretKey.TabIndex = 12;
            buttonSecretKey.UseVisualStyleBackColor = false;
            buttonSecretKey.Click += Button_SecretKey_Click;
            // 
            // pictureBoxYou
            // 
            pictureBoxYou.Location = new Point(10, 2);
            pictureBoxYou.Margin = new Padding(1);
            pictureBoxYou.Name = "pictureBoxYou";
            pictureBoxYou.Padding = new Padding(1);
            pictureBoxYou.Size = new Size(128, 128);
            pictureBoxYou.TabIndex = 58;
            pictureBoxYou.TabStop = false;
            // 
            // pictureBoxSource
            // 
            pictureBoxSource.BackColor = SystemColors.Control;
            pictureBoxSource.Location = new Point(8, 396);
            pictureBoxSource.Margin = new Padding(1);
            pictureBoxSource.Name = "pictureBoxSource";
            pictureBoxSource.Padding = new Padding(1);
            pictureBoxSource.Size = new Size(128, 128);
            pictureBoxSource.TabIndex = 55;
            pictureBoxSource.TabStop = false;
            // 
            // buttonAddToPipeline
            // 
            buttonAddToPipeline.BackColor = SystemColors.ButtonHighlight;
            buttonAddToPipeline.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonAddToPipeline.ForeColor = SystemColors.ActiveCaptionText;
            buttonAddToPipeline.Location = new Point(462, 2);
            buttonAddToPipeline.Margin = new Padding(0);
            buttonAddToPipeline.Name = "buttonAddToPipeline";
            buttonAddToPipeline.Size = new Size(28, 28);
            buttonAddToPipeline.TabIndex = 15;
            buttonAddToPipeline.Text = "⇒";
            buttonAddToPipeline.UseVisualStyleBackColor = false;
            buttonAddToPipeline.Click += Button_AddToPipeline_Click;
            // 
            // ComboBox_RemoteEndPoint
            // 
            ComboBox_RemoteEndPoint.BackColor = SystemColors.ControlLightLight;
            ComboBox_RemoteEndPoint.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_RemoteEndPoint.ForeColor = SystemColors.ControlText;
            ComboBox_RemoteEndPoint.FormattingEnabled = true;
            ComboBox_RemoteEndPoint.Items.AddRange(new object[] { "3DES", "2FISH", "3FISH", "AES", "Cast5", "Cast6", "Camellia", "Ghost28147", "Idea", "Noekeon", "Rijndael", "RC2", "RC532", "RC6", "Seed", "Serpent", "Skipjack", "Tea", "Tnepres", "XTea", "ZenMatrix" });
            ComboBox_RemoteEndPoint.Location = new Point(492, 4);
            ComboBox_RemoteEndPoint.Margin = new Padding(1);
            ComboBox_RemoteEndPoint.Name = "ComboBox_RemoteEndPoint";
            ComboBox_RemoteEndPoint.Size = new Size(312, 24);
            ComboBox_RemoteEndPoint.TabIndex = 16;
            // 
            // ComboBox_LocalEndPoint
            // 
            ComboBox_LocalEndPoint.BackColor = SystemColors.ControlLightLight;
            ComboBox_LocalEndPoint.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_LocalEndPoint.ForeColor = SystemColors.ControlText;
            ComboBox_LocalEndPoint.FormattingEnabled = true;
            ComboBox_LocalEndPoint.Items.AddRange(new object[] { "hex16", "base16", "base32", "base64", "unix2unix", "html", "url" });
            ComboBox_LocalEndPoint.Location = new Point(148, 4);
            ComboBox_LocalEndPoint.Margin = new Padding(1);
            ComboBox_LocalEndPoint.Name = "ComboBox_LocalEndPoint";
            ComboBox_LocalEndPoint.Size = new Size(312, 24);
            ComboBox_LocalEndPoint.TabIndex = 14;
            // 
            // buttonHashIv
            // 
            buttonHashIv.BackColor = SystemColors.ButtonHighlight;
            buttonHashIv.BackgroundImage = Properties.Resources.a_hash5;
            buttonHashIv.BackgroundImageLayout = ImageLayout.Center;
            buttonHashIv.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonHashIv.ForeColor = SystemColors.ActiveCaptionText;
            buttonHashIv.Location = new Point(840, 4);
            buttonHashIv.Margin = new Padding(1);
            buttonHashIv.Name = "buttonHashIv";
            buttonHashIv.Padding = new Padding(1);
            buttonHashIv.Size = new Size(48, 28);
            buttonHashIv.TabIndex = 18;
            buttonHashIv.UseVisualStyleBackColor = false;
            // 
            // panelEnCodeCrypt
            // 
            panelEnCodeCrypt.BackColor = SystemColors.ActiveCaption;
            panelEnCodeCrypt.Controls.Add(buttonSecretKey);
            panelEnCodeCrypt.Controls.Add(buttonHashIv);
            panelEnCodeCrypt.Controls.Add(ComboBox_LocalEndPoint);
            panelEnCodeCrypt.Controls.Add(ComboBox_RemoteEndPoint);
            panelEnCodeCrypt.Controls.Add(buttonAddToPipeline);
            panelEnCodeCrypt.ForeColor = SystemColors.WindowText;
            panelEnCodeCrypt.Location = new Point(0, 28);
            panelEnCodeCrypt.Margin = new Padding(0);
            panelEnCodeCrypt.Name = "panelEnCodeCrypt";
            panelEnCodeCrypt.Size = new Size(976, 36);
            panelEnCodeCrypt.TabIndex = 10;
            // 
            // pictureBoxDestination
            // 
            pictureBoxDestination.Location = new Point(10, 396);
            pictureBoxDestination.Margin = new Padding(1);
            pictureBoxDestination.Name = "pictureBoxDestination";
            pictureBoxDestination.Padding = new Padding(1);
            pictureBoxDestination.Size = new Size(128, 128);
            pictureBoxDestination.TabIndex = 56;
            pictureBoxDestination.TabStop = false;
            // 
            // richTextBoxChat
            // 
            richTextBoxChat.BorderStyle = BorderStyle.FixedSingle;
            richTextBoxChat.Cursor = Cursors.No;
            richTextBoxChat.ForeColor = SystemColors.WindowText;
            richTextBoxChat.Location = new Point(148, 564);
            richTextBoxChat.Margin = new Padding(2);
            richTextBoxChat.Name = "richTextBoxChat";
            richTextBoxChat.Size = new Size(678, 116);
            richTextBoxChat.TabIndex = 57;
            richTextBoxChat.Text = "";
            // 
            // pictureBoxPartner
            // 
            pictureBoxPartner.Location = new Point(10, 2);
            pictureBoxPartner.Margin = new Padding(1);
            pictureBoxPartner.Name = "pictureBoxPartner";
            pictureBoxPartner.Padding = new Padding(1);
            pictureBoxPartner.Size = new Size(128, 128);
            pictureBoxPartner.TabIndex = 59;
            pictureBoxPartner.TabStop = false;
            // 
            // panelDestination
            // 
            panelDestination.BackColor = SystemColors.Control;
            panelDestination.Controls.Add(buttonExit);
            panelDestination.Controls.Add(pictureBoxPartner);
            panelDestination.Controls.Add(pictureBoxDestination);
            panelDestination.Controls.Add(buttonReload);
            panelDestination.ForeColor = SystemColors.ActiveCaptionText;
            panelDestination.Location = new Point(828, 72);
            panelDestination.Margin = new Padding(0);
            panelDestination.Name = "panelDestination";
            panelDestination.Size = new Size(148, 610);
            panelDestination.TabIndex = 80;
            // 
            // buttonExit
            // 
            buttonExit.BackColor = SystemColors.ButtonHighlight;
            buttonExit.Location = new Point(12, 576);
            buttonExit.Margin = new Padding(1);
            buttonExit.Name = "buttonExit";
            buttonExit.Padding = new Padding(1);
            buttonExit.Size = new Size(120, 28);
            buttonExit.TabIndex = 88;
            buttonExit.Text = "E&xit";
            buttonExit.UseVisualStyleBackColor = false;
            // 
            // buttonReload
            // 
            buttonReload.BackColor = SystemColors.ButtonHighlight;
            buttonReload.ForeColor = SystemColors.ActiveCaptionText;
            buttonReload.Location = new Point(12, 536);
            buttonReload.Margin = new Padding(1);
            buttonReload.Name = "buttonReload";
            buttonReload.Padding = new Padding(1);
            buttonReload.Size = new Size(120, 28);
            buttonReload.TabIndex = 84;
            buttonReload.Text = "&Reload";
            buttonReload.UseVisualStyleBackColor = false;
            // 
            // SecureChat
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(976, 711);
            Controls.Add(panelDestination);
            Controls.Add(richTextBoxChat);
            Controls.Add(panelSource);
            Controls.Add(panelEnCodeCrypt);
            Controls.Add(splitContainer);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "SecureChat";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "SecureChat";
            Load += SecureChat_Load;
            Controls.SetChildIndex(splitContainer, 0);
            Controls.SetChildIndex(panelEnCodeCrypt, 0);
            Controls.SetChildIndex(panelSource, 0);
            Controls.SetChildIndex(richTextBoxChat, 0);
            Controls.SetChildIndex(panelDestination, 0);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panelSource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxYou).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSource).EndInit();
            panelEnCodeCrypt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxDestination).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPartner).EndInit();
            panelDestination.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer;
        private TextBox TextBoxSource;
        private TextBox TextBoxDestionation;
        private Button buttonDelete;
        private Button buttonSend;
        private Panel panelSource;
        private Button buttonAddToPipeline;
        private ComboBox ComboBox_RemoteEndPoint;
        private Button buttonSecretKey;
        private ComboBox ComboBox_LocalEndPoint;
        private Button buttonHashIv;
        private Panel panelEnCodeCrypt;
        private PictureBox pictureBoxSource;
        private PictureBox pictureBoxDestination;
        private RichTextBox richTextBoxChat;
        private PictureBox pictureBoxYou;
        private PictureBox pictureBoxPartner;
        private Panel panelDestination;
        private Button buttonExit;
        private Button buttonReload;
    }
}