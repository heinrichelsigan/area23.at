using System.Drawing;
using System.Windows.Forms;

namespace Area23.At.WinForm.TransparentForms.TForms
{
    partial class TBadge
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
            // TBadge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BackgroundImageLayout = ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(480, 144);
            this.Controls.Add(labelBadge);
            this.Font = new System.Drawing.Font("Lucida Console", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.TransparencyKey = SystemColors.ControlLight;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "TBadge";
            this.Text = "TBadge";
            this.ResumeLayout(false);
            this.SuspendLayout();
        }

        #endregion

        private Label labelBadge;
    }
}