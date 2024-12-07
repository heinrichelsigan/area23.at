namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    partial class EnDeCodingForm
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
            ComboBox_EnDeCoding = new ComboBox();
            buttonEncode = new Button();
            buttonDecode = new Button();
            ComboBox_SymChiffer = new ComboBox();
            buttonSave = new Button();
            buttonLoad = new Button();
            buttonAddToPipeline = new Button();
            TextBox_CryptPipeline = new TextBox();
            TextBox_Key = new TextBox();
            TextBox_IV = new TextBox();
            buttonClearPipeline = new Button();
            buttonSecretKey = new Button();
            buttonHashIv = new Button();
            panelEnCodeCrypt = new Panel();
            panelButtons = new Panel();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panelEnCodeCrypt.SuspendLayout();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Location = new Point(1, 144);
            splitContainer.Margin = new Padding(1);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(TextBoxSource);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(TextBoxDestionation);
            splitContainer.Panel2MinSize = 40;
            splitContainer.Size = new Size(980, 496);
            splitContainer.SplitterDistance = 490;
            splitContainer.SplitterWidth = 3;
            splitContainer.TabIndex = 41;
            splitContainer.TabStop = false;
            // 
            // TextBoxSource
            // 
            TextBoxSource.BackColor = SystemColors.ControlLightLight;
            TextBoxSource.BorderStyle = BorderStyle.FixedSingle;
            TextBoxSource.Dock = DockStyle.Fill;
            TextBoxSource.Font = new Font("Consolas", 9F);
            TextBoxSource.Location = new Point(0, 0);
            TextBoxSource.Margin = new Padding(1);
            TextBoxSource.MaxLength = 65536;
            TextBoxSource.Multiline = true;
            TextBoxSource.Name = "TextBoxSource";
            TextBoxSource.ScrollBars = ScrollBars.Both;
            TextBoxSource.Size = new Size(490, 496);
            TextBoxSource.TabIndex = 42;
            // 
            // TextBoxDestionation
            // 
            TextBoxDestionation.BackColor = SystemColors.Control;
            TextBoxDestionation.BorderStyle = BorderStyle.FixedSingle;
            TextBoxDestionation.Dock = DockStyle.Fill;
            TextBoxDestionation.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TextBoxDestionation.Location = new Point(0, 0);
            TextBoxDestionation.Margin = new Padding(1);
            TextBoxDestionation.MaxLength = 65536;
            TextBoxDestionation.Multiline = true;
            TextBoxDestionation.Name = "TextBoxDestionation";
            TextBoxDestionation.ScrollBars = ScrollBars.Both;
            TextBoxDestionation.Size = new Size(487, 496);
            TextBoxDestionation.TabIndex = 43;
            // 
            // ComboBox_EnDeCoding
            // 
            ComboBox_EnDeCoding.BackColor = SystemColors.ControlLightLight;
            ComboBox_EnDeCoding.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_EnDeCoding.ForeColor = SystemColors.ControlText;
            ComboBox_EnDeCoding.FormattingEnabled = true;
            ComboBox_EnDeCoding.Items.AddRange(new object[] { "hex16", "base16", "base32", "base64", "unix2unix", "html", "url" });
            ComboBox_EnDeCoding.Location = new Point(8, 12);
            ComboBox_EnDeCoding.Margin = new Padding(1);
            ComboBox_EnDeCoding.Name = "ComboBox_EnDeCoding";
            ComboBox_EnDeCoding.Size = new Size(165, 24);
            ComboBox_EnDeCoding.TabIndex = 3;
            // 
            // buttonEncode
            // 
            buttonEncode.BackColor = SystemColors.ButtonHighlight;
            buttonEncode.Location = new Point(9, 5);
            buttonEncode.Margin = new Padding(1);
            buttonEncode.Name = "buttonEncode";
            buttonEncode.Padding = new Padding(1);
            buttonEncode.Size = new Size(78, 28);
            buttonEncode.TabIndex = 51;
            buttonEncode.Text = "&Encode";
            buttonEncode.UseVisualStyleBackColor = false;
            buttonEncode.Click += Button_Encode_Click;
            // 
            // buttonDecode
            // 
            buttonDecode.BackColor = SystemColors.ButtonHighlight;
            buttonDecode.Location = new Point(895, 5);
            buttonDecode.Margin = new Padding(1);
            buttonDecode.Name = "buttonDecode";
            buttonDecode.Padding = new Padding(1);
            buttonDecode.Size = new Size(78, 28);
            buttonDecode.TabIndex = 54;
            buttonDecode.Text = "&Decode";
            buttonDecode.UseVisualStyleBackColor = false;
            buttonDecode.Click += Button_Decode_Click;
            // 
            // ComboBox_SymChiffer
            // 
            ComboBox_SymChiffer.BackColor = SystemColors.ControlLightLight;
            ComboBox_SymChiffer.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_SymChiffer.ForeColor = SystemColors.ControlText;
            ComboBox_SymChiffer.FormattingEnabled = true;
            ComboBox_SymChiffer.Items.AddRange(new object[] { "3DES", "2FISH", "3FISH", "AES", "Cast5", "Cast6", "Camellia", "Ghost28147", "Idea", "Noekeon", "Rijndael", "RC2", "RC532", "RC6", "Seed", "Serpent", "Skipjack", "Tea", "Tnepres", "XTea", "ZenMatrix" });
            ComboBox_SymChiffer.Location = new Point(180, 12);
            ComboBox_SymChiffer.Margin = new Padding(1);
            ComboBox_SymChiffer.Name = "ComboBox_SymChiffer";
            ComboBox_SymChiffer.Size = new Size(144, 24);
            ComboBox_SymChiffer.TabIndex = 4;
            // 
            // buttonSave
            // 
            buttonSave.BackColor = SystemColors.ButtonHighlight;
            buttonSave.Location = new Point(809, 5);
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
            buttonLoad.Location = new Point(96, 5);
            buttonLoad.Margin = new Padding(1);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Padding = new Padding(1);
            buttonLoad.Size = new Size(78, 28);
            buttonLoad.TabIndex = 52;
            buttonLoad.Text = "&Load";
            buttonLoad.UseVisualStyleBackColor = false;
            buttonLoad.Click += Button_Load_Click;
            // 
            // buttonAddToPipeline
            // 
            buttonAddToPipeline.BackColor = SystemColors.ButtonHighlight;
            buttonAddToPipeline.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonAddToPipeline.ForeColor = SystemColors.ActiveCaptionText;
            buttonAddToPipeline.Location = new Point(325, 12);
            buttonAddToPipeline.Margin = new Padding(1);
            buttonAddToPipeline.Name = "buttonAddToPipeline";
            buttonAddToPipeline.Padding = new Padding(1);
            buttonAddToPipeline.Size = new Size(48, 28);
            buttonAddToPipeline.TabIndex = 5;
            buttonAddToPipeline.Text = "⇒";
            buttonAddToPipeline.UseVisualStyleBackColor = false;
            buttonAddToPipeline.Click += Button_AddToPipeline_Click;
            // 
            // TextBox_CryptPipeline
            // 
            TextBox_CryptPipeline.BorderStyle = BorderStyle.None;
            TextBox_CryptPipeline.ForeColor = SystemColors.ControlText;
            TextBox_CryptPipeline.Location = new Point(384, 12);
            TextBox_CryptPipeline.Margin = new Padding(1);
            TextBox_CryptPipeline.Name = "TextBox_CryptPipeline";
            TextBox_CryptPipeline.ReadOnly = true;
            TextBox_CryptPipeline.Size = new Size(502, 21);
            TextBox_CryptPipeline.TabIndex = 6;
            TextBox_CryptPipeline.TabStop = false;
            // 
            // TextBox_Key
            // 
            TextBox_Key.BackColor = SystemColors.ControlLightLight;
            TextBox_Key.BorderStyle = BorderStyle.None;
            TextBox_Key.ForeColor = SystemColors.ControlText;
            TextBox_Key.Location = new Point(64, 48);
            TextBox_Key.Margin = new Padding(1);
            TextBox_Key.Name = "TextBox_Key";
            TextBox_Key.Size = new Size(310, 21);
            TextBox_Key.TabIndex = 12;
            TextBox_Key.TextChanged += TextBox_Key_TextChanged;
            // 
            // TextBox_IV
            // 
            TextBox_IV.BorderStyle = BorderStyle.None;
            TextBox_IV.ForeColor = SystemColors.ControlText;
            TextBox_IV.Location = new Point(444, 48);
            TextBox_IV.Margin = new Padding(1);
            TextBox_IV.Name = "TextBox_IV";
            TextBox_IV.Size = new Size(528, 21);
            TextBox_IV.TabIndex = 14;
            // 
            // buttonClearPipeline
            // 
            buttonClearPipeline.BackColor = SystemColors.ButtonHighlight;
            buttonClearPipeline.Font = new Font("Lucida Sans Unicode", 10F);
            buttonClearPipeline.ForeColor = SystemColors.ActiveCaptionText;
            buttonClearPipeline.Location = new Point(900, 12);
            buttonClearPipeline.Margin = new Padding(1);
            buttonClearPipeline.Name = "buttonClearPipeline";
            buttonClearPipeline.Padding = new Padding(1);
            buttonClearPipeline.Size = new Size(72, 28);
            buttonClearPipeline.TabIndex = 7;
            buttonClearPipeline.Text = "&Clear";
            buttonClearPipeline.UseVisualStyleBackColor = false;
            buttonClearPipeline.Click += Button_ClearPipeline_Click;
            // 
            // buttonSecretKey
            // 
            buttonSecretKey.BackColor = SystemColors.ButtonHighlight;
            buttonSecretKey.BackgroundImage = Properties.Resources.a_right_key;
            buttonSecretKey.BackgroundImageLayout = ImageLayout.None;
            buttonSecretKey.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonSecretKey.ForeColor = SystemColors.ActiveCaptionText;
            buttonSecretKey.Location = new Point(8, 48);
            buttonSecretKey.Margin = new Padding(1);
            buttonSecretKey.Name = "buttonSecretKey";
            buttonSecretKey.Padding = new Padding(1);
            buttonSecretKey.Size = new Size(48, 28);
            buttonSecretKey.TabIndex = 11;
            buttonSecretKey.UseVisualStyleBackColor = false;
            buttonSecretKey.Click += Button_SecretKey_Click;
            // 
            // buttonHashIv
            // 
            buttonHashIv.BackColor = SystemColors.ButtonHighlight;
            buttonHashIv.BackgroundImage = Properties.Resources.a_hash5;
            buttonHashIv.BackgroundImageLayout = ImageLayout.None;
            buttonHashIv.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonHashIv.ForeColor = SystemColors.ActiveCaptionText;
            buttonHashIv.Location = new Point(384, 48);
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
            panelEnCodeCrypt.Controls.Add(TextBox_CryptPipeline);
            panelEnCodeCrypt.Controls.Add(buttonHashIv);
            panelEnCodeCrypt.Controls.Add(ComboBox_EnDeCoding);
            panelEnCodeCrypt.Controls.Add(buttonSecretKey);
            panelEnCodeCrypt.Controls.Add(ComboBox_SymChiffer);
            panelEnCodeCrypt.Controls.Add(buttonClearPipeline);
            panelEnCodeCrypt.Controls.Add(buttonAddToPipeline);
            panelEnCodeCrypt.Controls.Add(TextBox_IV);
            panelEnCodeCrypt.Controls.Add(TextBox_Key);
            panelEnCodeCrypt.ForeColor = SystemColors.WindowText;
            panelEnCodeCrypt.Location = new Point(1, 28);
            panelEnCodeCrypt.Margin = new Padding(0);
            panelEnCodeCrypt.Name = "panelEnCodeCrypt";
            panelEnCodeCrypt.Size = new Size(983, 96);
            panelEnCodeCrypt.TabIndex = 2;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = SystemColors.ActiveCaption;
            panelButtons.Controls.Add(buttonDecode);
            panelButtons.Controls.Add(buttonSave);
            panelButtons.Controls.Add(buttonLoad);
            panelButtons.Controls.Add(buttonEncode);
            panelButtons.ForeColor = SystemColors.ActiveCaptionText;
            panelButtons.Location = new Point(1, 656);
            panelButtons.Margin = new Padding(1);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(986, 36);
            panelButtons.TabIndex = 55;
            // 
            // EnDeCodingForm
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(984, 721);
            Controls.Add(panelButtons);
            Controls.Add(panelEnCodeCrypt);
            Controls.Add(splitContainer);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "EnDeCodingForm";
            Text = "EnDeCodingForm";
            Load += EnDeCodingForm_Load;
            Controls.SetChildIndex(splitContainer, 0);
            Controls.SetChildIndex(panelEnCodeCrypt, 0);
            Controls.SetChildIndex(panelButtons, 0);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panelEnCodeCrypt.ResumeLayout(false);
            panelEnCodeCrypt.PerformLayout();
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer;
        private TextBox TextBoxSource;
        private TextBox TextBoxDestionation;
        private ComboBox ComboBox_EnDeCoding;
        private Button buttonEncode;
        private Button buttonDecode;
        private ComboBox ComboBox_SymChiffer;
        private Button buttonSave;
        private Button buttonLoad;
        private Button buttonAddToPipeline;
        private TextBox TextBox_CryptPipeline;
        private TextBox TextBox_Key;
        private TextBox TextBox_IV;
        private Button buttonClearPipeline;
        private Button buttonSecretKey;
        private Button buttonHashIv;
        private Panel panelEnCodeCrypt;
        private Panel panelButtons;
    }
}