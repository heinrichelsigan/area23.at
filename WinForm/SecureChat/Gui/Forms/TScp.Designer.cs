namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    partial class TScp
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
            panelButtons = new Panel();
            textBoxUserName = new TextBox();
            labelAt = new Label();
            comboBoxHosts = new ComboBox();
            comboBoxLeft = new ComboBox();
            comboBoxRight = new ComboBox();
            buttonLeftToRight = new Button();
            buttonRightToLeft = new Button();
            buttonSelectLeft = new Button();
            buttonSelectRight = new Button();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // panelButtons
            // 
            panelButtons.BackColor = SystemColors.ActiveCaption;
            panelButtons.Controls.Add(textBoxUserName);
            panelButtons.Controls.Add(labelAt);
            panelButtons.Controls.Add(comboBoxHosts);
            panelButtons.Font = new Font("Lucida Sans Unicode", 10F);
            panelButtons.Location = new Point(0, 32);
            panelButtons.Margin = new Padding(0);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(723, 36);
            panelButtons.TabIndex = 10;
            // 
            // textBoxUserName
            // 
            textBoxUserName.BorderStyle = BorderStyle.None;
            textBoxUserName.Font = new Font("Lucida Sans Unicode", 10F);
            textBoxUserName.Location = new Point(12, 6);
            textBoxUserName.Margin = new Padding(1);
            textBoxUserName.Name = "textBoxUserName";
            textBoxUserName.Size = new Size(162, 21);
            textBoxUserName.TabIndex = 2;
            textBoxUserName.Text = "ubuntu";
            textBoxUserName.TextAlign = HorizontalAlignment.Right;
            // 
            // labelAt
            // 
            labelAt.AutoSize = true;
            labelAt.Font = new Font("Lucida Sans Unicode", 12F);
            labelAt.ForeColor = SystemColors.HighlightText;
            labelAt.Location = new Point(175, 7);
            labelAt.Margin = new Padding(0);
            labelAt.Name = "labelAt";
            labelAt.Size = new Size(23, 20);
            labelAt.TabIndex = 3;
            labelAt.Text = "@";
            // 
            // comboBoxHosts
            // 
            comboBoxHosts.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxHosts.FlatStyle = FlatStyle.Flat;
            comboBoxHosts.FormattingEnabled = true;
            comboBoxHosts.Location = new Point(200, 6);
            comboBoxHosts.Margin = new Padding(1);
            comboBoxHosts.Name = "comboBoxHosts";
            comboBoxHosts.Size = new Size(514, 24);
            comboBoxHosts.TabIndex = 4;
            // 
            // comboBoxLeft
            // 
            comboBoxLeft.FormattingEnabled = true;
            comboBoxLeft.ItemHeight = 16;
            comboBoxLeft.Location = new Point(12, 114);
            comboBoxLeft.Name = "comboBoxLeft";
            comboBoxLeft.Size = new Size(340, 24);
            comboBoxLeft.TabIndex = 12;
            // 
            // comboBoxRight
            // 
            comboBoxRight.FormattingEnabled = true;
            comboBoxRight.Location = new Point(373, 114);
            comboBoxRight.Name = "comboBoxRight";
            comboBoxRight.Size = new Size(338, 24);
            comboBoxRight.TabIndex = 13;
            // 
            // buttonLeftToRight
            // 
            buttonLeftToRight.BackColor = SystemColors.ButtonHighlight;
            buttonLeftToRight.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonLeftToRight.ForeColor = SystemColors.ActiveCaptionText;
            buttonLeftToRight.Location = new Point(373, 85);
            buttonLeftToRight.Margin = new Padding(1);
            buttonLeftToRight.Name = "buttonLeftToRight";
            buttonLeftToRight.Padding = new Padding(1);
            buttonLeftToRight.Size = new Size(48, 25);
            buttonLeftToRight.TabIndex = 15;
            buttonLeftToRight.Text = "⇒";
            buttonLeftToRight.UseVisualStyleBackColor = false;
            // 
            // buttonRightToLeft
            // 
            buttonRightToLeft.BackColor = SystemColors.ButtonHighlight;
            buttonRightToLeft.Font = new Font("Lucida Sans Unicode", 10F, FontStyle.Bold);
            buttonRightToLeft.ForeColor = SystemColors.ActiveCaptionText;
            buttonRightToLeft.Location = new Point(304, 85);
            buttonRightToLeft.Margin = new Padding(1);
            buttonRightToLeft.Name = "buttonRightToLeft";
            buttonRightToLeft.Padding = new Padding(1);
            buttonRightToLeft.Size = new Size(48, 25);
            buttonRightToLeft.TabIndex = 14;
            buttonRightToLeft.Text = "⇐";
            buttonRightToLeft.UseVisualStyleBackColor = false;
            // 
            // buttonSelectLeft
            // 
            buttonSelectLeft.BackColor = SystemColors.ButtonHighlight;
            buttonSelectLeft.Font = new Font("Lucida Sans Unicode", 10F);
            buttonSelectLeft.ForeColor = SystemColors.ActiveCaptionText;
            buttonSelectLeft.Location = new Point(12, 79);
            buttonSelectLeft.Margin = new Padding(1);
            buttonSelectLeft.Name = "buttonSelectLeft";
            buttonSelectLeft.Padding = new Padding(1);
            buttonSelectLeft.Size = new Size(36, 36);
            buttonSelectLeft.TabIndex = 10;
            buttonSelectLeft.Text = "📁";
            buttonSelectLeft.UseVisualStyleBackColor = false;
            buttonSelectLeft.Click += buttonSelectLeft_Click;
            // 
            // buttonSelectRight
            // 
            buttonSelectRight.BackColor = SystemColors.ButtonHighlight;
            buttonSelectRight.Font = new Font("Lucida Sans Unicode", 10F);
            buttonSelectRight.ForeColor = SystemColors.ActiveCaptionText;
            buttonSelectRight.Location = new Point(675, 74);
            buttonSelectRight.Margin = new Padding(1);
            buttonSelectRight.Name = "buttonSelectRight";
            buttonSelectRight.Padding = new Padding(1);
            buttonSelectRight.Size = new Size(36, 36);
            buttonSelectRight.TabIndex = 11;
            buttonSelectRight.Text = "🗁";
            buttonSelectRight.UseVisualStyleBackColor = false;
            // 
            // TScp
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.GradientActiveCaption;
            ClientSize = new Size(723, 201);
            Controls.Add(buttonRightToLeft);
            Controls.Add(buttonLeftToRight);
            Controls.Add(comboBoxRight);
            Controls.Add(comboBoxLeft);
            Controls.Add(buttonSelectLeft);
            Controls.Add(buttonSelectRight);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "TScp";
            Text = "NetAddr";
            Controls.SetChildIndex(panelButtons, 0);
            Controls.SetChildIndex(buttonSelectRight, 0);
            Controls.SetChildIndex(buttonSelectLeft, 0);
            Controls.SetChildIndex(comboBoxLeft, 0);
            Controls.SetChildIndex(comboBoxRight, 0);
            Controls.SetChildIndex(buttonLeftToRight, 0);
            Controls.SetChildIndex(buttonRightToLeft, 0);
            panelButtons.ResumeLayout(false);
            panelButtons.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panelButtons;
        private ComboBox comboBoxHosts;
        private ComboBox comboBoxLeft;
        private ComboBox comboBoxRight;
        private Button buttonLeftToRight;
        private Button buttonRightToLeft;
        private TextBox textBoxUserName;
        private Label labelAt;
        private Button buttonSelectLeft;
        private Button buttonSelectRight;
    }
}