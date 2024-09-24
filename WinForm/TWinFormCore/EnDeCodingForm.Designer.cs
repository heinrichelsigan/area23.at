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
            textBoxIn = new TextBox();
            textBoxOut = new TextBox();
            comboBoxEnDeCoding = new ComboBox();
            buttonEncode = new Button();
            buttonDecode = new Button();
            comboBoxCryptAlgo = new ComboBox();
            buttonSave = new Button();
            buttonLoad = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Location = new Point(2, 72);
            splitContainer.Margin = new Padding(2);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(textBoxIn);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(textBoxOut);
            splitContainer.Panel2MinSize = 40;
            splitContainer.Size = new Size(940, 568);
            splitContainer.SplitterDistance = 466;
            splitContainer.TabIndex = 1;
            // 
            // textBoxIn
            // 
            textBoxIn.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxIn.BackColor = SystemColors.Control;
            textBoxIn.Location = new Point(2, 2);
            textBoxIn.Margin = new Padding(2);
            textBoxIn.MaxLength = 65536;
            textBoxIn.Multiline = true;
            textBoxIn.Name = "textBoxIn";
            textBoxIn.Size = new Size(462, 564);
            textBoxIn.TabIndex = 0;
            // 
            // textBoxOut
            // 
            textBoxOut.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxOut.BackColor = SystemColors.Control;
            textBoxOut.Location = new Point(4, 2);
            textBoxOut.Margin = new Padding(2);
            textBoxOut.MaxLength = 65536;
            textBoxOut.Multiline = true;
            textBoxOut.Name = "textBoxOut";
            textBoxOut.Size = new Size(462, 564);
            textBoxOut.TabIndex = 1;
            // 
            // comboBoxEnDeCoding
            // 
            comboBoxEnDeCoding.FormattingEnabled = true;
            comboBoxEnDeCoding.Items.AddRange(new object[] { "hex16", "base32", "base64", "unix2unix", "html", "url" });
            comboBoxEnDeCoding.Location = new Point(12, 36);
            comboBoxEnDeCoding.Margin = new Padding(2);
            comboBoxEnDeCoding.Name = "comboBoxEnDeCoding";
            comboBoxEnDeCoding.Size = new Size(144, 24);
            comboBoxEnDeCoding.TabIndex = 2;
            comboBoxEnDeCoding.SelectedIndexChanged += comboBoxEnDeCoding_SelectedIndexChanged;
            // 
            // buttonEncode
            // 
            buttonEncode.BackColor = SystemColors.ControlLight;
            buttonEncode.Location = new Point(12, 648);
            buttonEncode.Margin = new Padding(2);
            buttonEncode.Name = "buttonEncode";
            buttonEncode.Size = new Size(75, 23);
            buttonEncode.TabIndex = 3;
            buttonEncode.Text = "Encode";
            buttonEncode.UseVisualStyleBackColor = false;
            buttonEncode.Click += buttonEncode_Click;
            // 
            // buttonDecode
            // 
            buttonDecode.BackColor = SystemColors.ControlLight;
            buttonDecode.Location = new Point(863, 648);
            buttonDecode.Margin = new Padding(2);
            buttonDecode.Name = "buttonDecode";
            buttonDecode.Size = new Size(75, 23);
            buttonDecode.TabIndex = 4;
            buttonDecode.Text = "Decode";
            buttonDecode.UseVisualStyleBackColor = false;
            buttonDecode.Click += buttonDecode_Click;
            // 
            // comboBoxCryptAlgo
            // 
            comboBoxCryptAlgo.FormattingEnabled = true;
            comboBoxCryptAlgo.Items.AddRange(new object[] { "3DES", "2FISH", "3FISH", "AES", "Cast5", "Cast6", "Camellia", "Ghost28147", "Idea", "Noekeon", "Rijndael", "RC2", "RC532", "RC6", "Seed", "Serpent", "Skipjack", "Tea", "Tnepres", "XTea", "ZenMatrix" });
            comboBoxCryptAlgo.Location = new Point(170, 36);
            comboBoxCryptAlgo.Margin = new Padding(2);
            comboBoxCryptAlgo.Name = "comboBoxCryptAlgo";
            comboBoxCryptAlgo.Size = new Size(144, 24);
            comboBoxCryptAlgo.TabIndex = 5;
            // 
            // buttonSave
            // 
            buttonSave.BackColor = SystemColors.ControlLight;
            buttonSave.Location = new Point(784, 648);
            buttonSave.Margin = new Padding(2);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(75, 23);
            buttonSave.TabIndex = 6;
            buttonSave.Text = "Save";
            buttonSave.UseVisualStyleBackColor = false;
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonLoad
            // 
            buttonLoad.BackColor = SystemColors.ControlLight;
            buttonLoad.Location = new Point(91, 648);
            buttonLoad.Margin = new Padding(2);
            buttonLoad.Name = "buttonLoad";
            buttonLoad.Size = new Size(75, 23);
            buttonLoad.TabIndex = 7;
            buttonLoad.Text = "Load";
            buttonLoad.UseVisualStyleBackColor = false;
            // 
            // EnDeCodingForm
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(944, 681);
            Controls.Add(buttonLoad);
            Controls.Add(buttonSave);
            Controls.Add(comboBoxCryptAlgo);
            Controls.Add(buttonDecode);
            Controls.Add(buttonEncode);
            Controls.Add(comboBoxEnDeCoding);
            Controls.Add(splitContainer);
            Name = "EnDeCodingForm";
            Text = "EnDeCodingForm";
            Controls.SetChildIndex(splitContainer, 0);
            Controls.SetChildIndex(comboBoxEnDeCoding, 0);
            Controls.SetChildIndex(buttonEncode, 0);
            Controls.SetChildIndex(buttonDecode, 0);
            Controls.SetChildIndex(comboBoxCryptAlgo, 0);
            Controls.SetChildIndex(buttonSave, 0);
            Controls.SetChildIndex(buttonLoad, 0);
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
        private TextBox textBoxIn;
        private TextBox textBoxOut;
        private ComboBox comboBoxEnDeCoding;
        private Button buttonEncode;
        private Button buttonDecode;
        private ComboBox comboBoxCryptAlgo;
        private Button buttonSave;
        private Button buttonLoad;
    }
}