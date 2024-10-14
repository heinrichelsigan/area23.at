namespace Area23.At.WinForm.TWinFormCore
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
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();            
            // 
            // ComboBox_EnDeCoding
            // 
            ComboBox_EnDeCoding.BackColor = SystemColors.ControlLightLight;
            ComboBox_EnDeCoding.FormattingEnabled = true;
            ComboBox_EnDeCoding.Items.AddRange(new object[] { "hex16", "base16", "base32", "base64", "unix2unix", "html", "url" });
            ComboBox_EnDeCoding.Location = new Point(12, 36);
            ComboBox_EnDeCoding.Margin = new Padding(0);
            ComboBox_EnDeCoding.Name = "ComboBox_EnDeCoding";
            ComboBox_EnDeCoding.Size = new Size(144, 24);
            ComboBox_EnDeCoding.TabIndex = 1;            
            // 
            // ComboBox_SymChiffer
            // 
            ComboBox_SymChiffer.BackColor = SystemColors.ControlLightLight;
            ComboBox_SymChiffer.DropDownStyle = ComboBoxStyle.DropDownList;
            ComboBox_SymChiffer.FormattingEnabled = true;
            ComboBox_SymChiffer.Items.AddRange(new object[] { "3DES", "2FISH", "3FISH", "AES", "Cast5", "Cast6", "Camellia", "Ghost28147", "Idea", "Noekeon", "Rijndael", "RC2", "RC532", "RC6", "Seed", "Serpent", "Skipjack", "Tea", "Tnepres", "XTea", "ZenMatrix" });
            ComboBox_SymChiffer.Location = new Point(170, 36);
            ComboBox_SymChiffer.Margin = new Padding(0);
            ComboBox_SymChiffer.Name = "ComboBox_SymChiffer";
            ComboBox_SymChiffer.Size = new Size(144, 24);
            ComboBox_SymChiffer.TabIndex = 2;            
            // 
            // buttonAddToPipeline
            // 
            buttonAddToPipeline.BackColor = SystemColors.ButtonFace;
            buttonAddToPipeline.BackgroundImageLayout = ImageLayout.None;
            buttonAddToPipeline.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonAddToPipeline.Location = new Point(329, 36);
            buttonAddToPipeline.Margin = new Padding(1);
            buttonAddToPipeline.Name = "buttonAddToPipeline";
            buttonAddToPipeline.Padding = new Padding(1);
            buttonAddToPipeline.Size = new Size(48, 28);
            buttonAddToPipeline.TabIndex = 3;
            buttonAddToPipeline.Text = "⇒";
            buttonAddToPipeline.TextImageRelation = TextImageRelation.TextAboveImage;
            buttonAddToPipeline.UseVisualStyleBackColor = false;
            buttonAddToPipeline.Click += Button_AddToPipeline_Click;
            // 
            // TextBox_CryptPipeline
            // 
            TextBox_CryptPipeline.BorderStyle = BorderStyle.None;
            TextBox_CryptPipeline.Location = new Point(388, 36);
            TextBox_CryptPipeline.Margin = new Padding(1);
            TextBox_CryptPipeline.Padding = new Padding(1);
            TextBox_CryptPipeline.Name = "TextBox_CryptPipeline";
            TextBox_CryptPipeline.ReadOnly = true;
            TextBox_CryptPipeline.Size = new Size(472, 28);
            TextBox_CryptPipeline.TabIndex = 4;
            // 
            // buttonClearPipeline
            // 
            buttonClearPipeline.BackColor = SystemColors.ButtonHighlight;
            buttonClearPipeline.Font = new Font("Lucida Sans Unicode", 10F);
            buttonClearPipeline.Location = new Point(863, 36);
            buttonClearPipeline.Margin = new Padding(1);
            buttonClearPipeline.Name = "buttonClearPipeline";
            buttonClearPipeline.Padding = new Padding(1);
            buttonClearPipeline.Size = new Size(72, 28);
            buttonClearPipeline.TabIndex = 5;
            buttonClearPipeline.Text = "&Clear";
            buttonClearPipeline.UseMnemonic = true;
            buttonClearPipeline.UseVisualStyleBackColor = false;
            buttonClearPipeline.Click += Button_ClearPipeline_Click;
            // 
            // buttonSecretKey
            // 
            buttonSecretKey.BackColor = SystemColors.ButtonFace;
            buttonSecretKey.BackgroundImage = Properties.Resources.a_right_key;
            buttonSecretKey.BackgroundImageLayout = ImageLayout.None;
            buttonSecretKey.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonSecretKey.Location = new Point(12, 78);
            buttonSecretKey.Margin = new Padding(1);
            buttonSecretKey.Name = "buttonSecretKey";
            buttonSecretKey.Padding = new Padding(1);
            buttonSecretKey.Size = new Size(48, 28);
            buttonSecretKey.TabIndex = 11;
            buttonSecretKey.UseVisualStyleBackColor = false;
            buttonSecretKey.Click += Button_SecretKey_Click;
            // 
            // TextBox_Key
            // 
            TextBox_Key.BorderStyle = BorderStyle.None;
            TextBox_Key.Location = new Point(67, 78);
            TextBox_Key.Margin = new Padding(1);
            TextBox_Key.Padding = new Padding(1);
            TextBox_Key.Name = "TextBox_Key";
            TextBox_Key.Size = new Size(310, 28);
            TextBox_Key.TabIndex = 12;
            TextBox_Key.TextChanged += TextBox_Key_TextChanged;
            // 
            // buttonHashIv
            // 
            buttonHashIv.BackColor = SystemColors.ButtonFace;
            buttonHashIv.BackgroundImage = Properties.Resources.a_hash5;
            buttonHashIv.BackgroundImageLayout = ImageLayout.None;
            buttonHashIv.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonHashIv.Location = new Point(388, 78);
            buttonHashIv.Margin = new Padding(1);
            buttonHashIv.Name = "buttonHashIv";
            buttonHashIv.Padding = new Padding(1);
            buttonHashIv.Size = new Size(48, 28);
            buttonHashIv.TabIndex = 13;
            buttonHashIv.UseVisualStyleBackColor = false;
            // 
            // TextBox_IV
            // 
            TextBox_IV.BorderStyle = BorderStyle.None;
            TextBox_IV.Location = new Point(448, 78);
            TextBox_IV.Margin = new Padding(1);
            TextBox_IV.Padding = new Padding(1);
            TextBox_IV.Name = "TextBox_IV";
            TextBox_IV.Size = new Size(486, 28);
            TextBox_IV.TabIndex = 14;            
            // 
            // TextBoxSource
            // 
            TextBoxSource.BackColor = SystemColors.ControlLightLight;
            TextBoxSource.Dock = DockStyle.Fill;
            TextBoxSource.Location = new Point(0, 0);
            TextBoxSource.Margin = new Padding(1);
            TextBoxSource.MaxLength = 65536;
            TextBoxSource.Multiline = true;
            TextBoxSource.Name = "TextBoxSource";
            TextBoxSource.Size = new Size(466, 480);
            TextBoxSource.TabIndex = 42;
            // 
            // TextBoxDestionation
            // 
            TextBoxDestionation.BackColor = SystemColors.Control;
            TextBoxDestionation.Dock = DockStyle.Fill;
            TextBoxDestionation.Location = new Point(0, 0);
            TextBoxDestionation.Margin = new Padding(1);
            TextBoxDestionation.MaxLength = 65536;
            TextBoxDestionation.Multiline = true;
            TextBoxDestionation.Name = "TextBoxDestionation";
            TextBoxDestionation.Size = new Size(470, 480);
            TextBoxDestionation.TabIndex = 43;
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
            splitContainer.Size = new Size(940, 480);
            splitContainer.SplitterDistance = 466;
            splitContainer.TabIndex = 41;
            // 
            // buttonEncode
            // 
            buttonEncode.BackColor = SystemColors.ButtonHighlight;
            buttonEncode.Location = new Point(12, 640);
            buttonEncode.Margin = new Padding(1);
            buttonEncode.Name = "buttonEncode";
            buttonEncode.Padding = new Padding(1);
            buttonEncode.Size = new Size(78, 28);
            buttonEncode.TabIndex = 51;
            buttonEncode.Text = "&Encode";
            buttonEncode.UseMnemonic = true;
            buttonEncode.UseVisualStyleBackColor = false;
            buttonEncode.Click += Button_Encode_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.BackColor = SystemColors.ButtonHighlight;
            buttonLoad.Location = new Point(96, 640);
            buttonLoad.Margin = new Padding(1);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Padding = new Padding(1);
            buttonLoad.Size = new Size(78, 28);
            buttonLoad.TabIndex = 52;
            buttonLoad.Text = "&Load";
            buttonLoad.UseMnemonic = true;
            buttonLoad.UseVisualStyleBackColor = false;
            buttonLoad.Click += Button_Load_Click;
            // 
            // buttonSave
            // 
            buttonSave.BackColor = SystemColors.ButtonHighlight;
            buttonSave.Location = new Point(772, 640);
            buttonSave.Margin = new Padding(1);
            buttonSave.Name = "buttonSave";
            buttonSave.Padding = new Padding(1);
            buttonSave.Size = new Size(78, 28);
            buttonSave.TabIndex = 53;
            buttonSave.Text = "&Save";
            buttonSave.UseMnemonic = true;
            buttonSave.UseVisualStyleBackColor = false;
            buttonSave.Click += Button_Save_Click;
            // 
            // buttonDecode
            // 
            buttonDecode.BackColor = SystemColors.ButtonHighlight;
            buttonDecode.Location = new Point(856, 640);
            buttonDecode.Margin = new Padding(1);
            buttonDecode.Name = "buttonDecode";
            buttonDecode.Padding = new Padding(1);
            buttonDecode.Size = new Size(78, 28);
            buttonDecode.TabIndex = 54;
            buttonDecode.Text = "&Decode";
            buttonDecode.UseMnemonic = true;
            buttonDecode.UseVisualStyleBackColor = false;
            buttonDecode.Click += Button_Decode_Click;
            // 
            // EnDeCodingForm
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(944, 681);
            Controls.Add(buttonHashIv);
            Controls.Add(buttonSecretKey);
            Controls.Add(buttonClearPipeline);
            Controls.Add(TextBox_IV);
            Controls.Add(TextBox_Key);
            Controls.Add(TextBox_CryptPipeline);
            Controls.Add(buttonAddToPipeline);
            Controls.Add(buttonLoad);
            Controls.Add(buttonSave);
            Controls.Add(ComboBox_SymChiffer);
            Controls.Add(buttonDecode);
            Controls.Add(buttonEncode);
            Controls.Add(ComboBox_EnDeCoding);
            Controls.Add(splitContainer);
            Name = "EnDeCodingForm";
            Text = "EnDeCodingForm";
            Controls.SetChildIndex(splitContainer, 0);
            Controls.SetChildIndex(ComboBox_EnDeCoding, 0);
            Controls.SetChildIndex(buttonEncode, 0);
            Controls.SetChildIndex(buttonDecode, 0);
            Controls.SetChildIndex(ComboBox_SymChiffer, 0);
            Controls.SetChildIndex(buttonSave, 0);
            Controls.SetChildIndex(buttonLoad, 0);
            Controls.SetChildIndex(buttonAddToPipeline, 0);
            Controls.SetChildIndex(TextBox_CryptPipeline, 0);
            Controls.SetChildIndex(TextBox_Key, 0);
            Controls.SetChildIndex(TextBox_IV, 0);
            Controls.SetChildIndex(buttonClearPipeline, 0);
            Controls.SetChildIndex(buttonSecretKey, 0);
            Controls.SetChildIndex(buttonHashIv, 0);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
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
    }
}