namespace Area23.At.WinForm.TWinFormCore.UI.Forms
{
    partial class TransparentBadge
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
            labelBadge = new Label();
            SuspendLayout();
            // 
            // labelBadge
            // 
            labelBadge.Dock = DockStyle.Fill;
            labelBadge.Location = new Point(0, 0);
            labelBadge.Margin = new Padding(2, 0, 2, 0);
            labelBadge.Name = "labelBadge";
            labelBadge.Size = new Size(480, 144);
            labelBadge.TabIndex = 0;
            labelBadge.Text = "enter badge text here";
            labelBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // TransparentBadge
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = SystemColors.Control;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(480, 144);
            Controls.Add(labelBadge);
            Font = new Font("Lucida Sans Unicode", 10F);
            FormBorderStyle = FormBorderStyle.None;
            Name = "TransparentBadge";
            Text = "TransparentFormCore8";
            TransparencyKey = SystemColors.Control;
            ResumeLayout(false);
        }

        #endregion
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem toolStripMenuItemOpen;
        private Label labelBadge;
    }
}