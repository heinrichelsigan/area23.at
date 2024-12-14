namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    partial class Fortune
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
            textBoxFortune = new TextBox();
            buttonFortune = new Button();
            buttonClose = new Button();
            panelButtons = new Panel();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // textBoxFortune
            // 
            textBoxFortune.BorderStyle = BorderStyle.FixedSingle;
            textBoxFortune.Font = new Font("Lucida Console", 10F);
            textBoxFortune.ForeColor = SystemColors.ControlText;
            textBoxFortune.Location = new Point(12, 39);
            textBoxFortune.Margin = new Padding(1);
            textBoxFortune.MaxLength = 65536;
            textBoxFortune.Multiline = true;
            textBoxFortune.Name = "textBoxFortune";
            textBoxFortune.ReadOnly = true;
            textBoxFortune.Size = new Size(776, 372);
            textBoxFortune.TabIndex = 3;
            // 
            // buttonFortune
            // 
            buttonFortune.Font = new Font("Lucida Console", 10F);
            buttonFortune.Location = new Point(12, 5);
            buttonFortune.Margin = new Padding(1);
            buttonFortune.Name = "buttonFortune";
            buttonFortune.Size = new Size(101, 25);
            buttonFortune.TabIndex = 1;
            buttonFortune.Text = "Fortune";
            buttonFortune.UseVisualStyleBackColor = true;
            buttonFortune.Click += buttonFortune_Click;
            // 
            // buttonClose
            // 
            buttonClose.Font = new Font("Lucida Console", 10F);
            buttonClose.Location = new Point(687, 5);
            buttonClose.Margin = new Padding(1);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(101, 25);
            buttonClose.TabIndex = 2;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = SystemColors.ActiveCaption;
            panelButtons.Controls.Add(buttonClose);
            panelButtons.Controls.Add(buttonFortune);
            panelButtons.ForeColor = SystemColors.ActiveCaptionText;
            panelButtons.Location = new Point(0, 425);
            panelButtons.Margin = new Padding(1);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(803, 36);
            panelButtons.TabIndex = 4;
            // 
            // Fortune
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(803, 496);
            Controls.Add(panelButtons);
            Controls.Add(textBoxFortune);
            Name = "Fortune";
            Text = "Fortune";
            Controls.SetChildIndex(textBoxFortune, 0);
            Controls.SetChildIndex(panelButtons, 0);
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxFortune;
        private Button buttonFortune;
        private Button buttonClose;
        private Panel panelButtons;
    }
}