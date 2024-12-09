namespace Area23.At.WinForm.TWinFormCore.Gui.Forms
{
    partial class NetAddr
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
            listBoxAddrs = new ListBox();
            buttonIpAddr = new Button();
            buttonClose = new Button();
            buttonIpHostAddr = new Button();
            panelButtons = new Panel();
            buttonConnectedIPAddrs = new Button();
            buttonMacs = new Button();
            panelButtons.SuspendLayout();
            SuspendLayout();
            // 
            // listBoxAddrs
            // 
            listBoxAddrs.BackColor = SystemColors.ControlLightLight;
            listBoxAddrs.BorderStyle = BorderStyle.FixedSingle;
            listBoxAddrs.Font = new Font("Lucida Console", 10F);
            listBoxAddrs.ForeColor = SystemColors.ControlText;
            listBoxAddrs.FormattingEnabled = true;
            listBoxAddrs.ItemHeight = 13;
            listBoxAddrs.Location = new Point(12, 42);
            listBoxAddrs.Margin = new Padding(1);
            listBoxAddrs.Name = "listBoxAddrs";
            listBoxAddrs.Size = new Size(518, 327);
            listBoxAddrs.TabIndex = 1;
            // 
            // buttonIpAddr
            // 
            buttonIpAddr.BackColor = SystemColors.ButtonHighlight;
            buttonIpAddr.Font = new Font("Lucida Console", 10F);
            buttonIpAddr.ForeColor = SystemColors.ActiveCaptionText;
            buttonIpAddr.Location = new Point(116, 4);
            buttonIpAddr.Margin = new Padding(1);
            buttonIpAddr.Name = "buttonIpAddr";
            buttonIpAddr.Size = new Size(102, 26);
            buttonIpAddr.TabIndex = 12;
            buttonIpAddr.Text = "IpAddr";
            buttonIpAddr.UseVisualStyleBackColor = false;
            buttonIpAddr.Click += buttonIpAddr_Click;
            // 
            // buttonClose
            // 
            buttonClose.BackColor = SystemColors.ButtonHighlight;
            buttonClose.BackgroundImageLayout = ImageLayout.None;
            buttonClose.Font = new Font("Lucida Console", 10F);
            buttonClose.ForeColor = SystemColors.ActiveCaptionText;
            buttonClose.Location = new Point(428, 4);
            buttonClose.Margin = new Padding(1);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(102, 26);
            buttonClose.TabIndex = 14;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = false;
            buttonClose.Click += buttonClose_Click;
            // 
            // buttonIpHostAddr
            // 
            buttonIpHostAddr.BackColor = SystemColors.ButtonHighlight;
            buttonIpHostAddr.Font = new Font("Lucida Console", 10F);
            buttonIpHostAddr.ForeColor = SystemColors.ActiveCaptionText;
            buttonIpHostAddr.Location = new Point(324, 4);
            buttonIpHostAddr.Margin = new Padding(1);
            buttonIpHostAddr.Name = "buttonIpHostAddr";
            buttonIpHostAddr.Size = new Size(102, 26);
            buttonIpHostAddr.TabIndex = 13;
            buttonIpHostAddr.Text = "IpHostAddr";
            buttonIpHostAddr.UseVisualStyleBackColor = false;
            buttonIpHostAddr.Click += buttonIpHostAddr_Click;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = SystemColors.ActiveCaption;
            panelButtons.Controls.Add(buttonConnectedIPAddrs);
            panelButtons.Controls.Add(buttonClose);
            panelButtons.Controls.Add(buttonMacs);
            panelButtons.Controls.Add(buttonIpHostAddr);
            panelButtons.Controls.Add(buttonIpAddr);
            panelButtons.Font = new Font("Lucida Sans Typewriter", 10F);
            panelButtons.Location = new Point(0, 389);
            panelButtons.Margin = new Padding(0);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(546, 36);
            panelButtons.TabIndex = 10;
            // 
            // buttonConnectedIPAddrs
            // 
            buttonConnectedIPAddrs.BackColor = SystemColors.ButtonHighlight;
            buttonConnectedIPAddrs.Font = new Font("Lucida Console", 10F);
            buttonConnectedIPAddrs.ForeColor = SystemColors.ActiveCaptionText;
            buttonConnectedIPAddrs.Location = new Point(220, 4);
            buttonConnectedIPAddrs.Margin = new Padding(1);
            buttonConnectedIPAddrs.Name = "buttonConnectedIPAddrs";
            buttonConnectedIPAddrs.Size = new Size(102, 26);
            buttonConnectedIPAddrs.TabIndex = 15;
            buttonConnectedIPAddrs.Text = "Connected IPAddr";
            buttonConnectedIPAddrs.UseVisualStyleBackColor = false;
            buttonConnectedIPAddrs.Click += buttonConnectedIPAddrs_Click;
            // 
            // buttonMacs
            // 
            buttonMacs.BackColor = SystemColors.ButtonHighlight;
            buttonMacs.Font = new Font("Lucida Console", 10F);
            buttonMacs.ForeColor = SystemColors.ActiveCaptionText;
            buttonMacs.Location = new Point(12, 4);
            buttonMacs.Margin = new Padding(1);
            buttonMacs.Name = "buttonMacs";
            buttonMacs.Size = new Size(102, 26);
            buttonMacs.TabIndex = 11;
            buttonMacs.Text = "MacAddr";
            buttonMacs.UseVisualStyleBackColor = false;
            buttonMacs.Click += buttonMacs_Click;
            // 
            // NetAddr
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(546, 453);
            Controls.Add(panelButtons);
            Controls.Add(listBoxAddrs);
            Name = "NetAddr";
            Text = "NetAddr";
            Controls.SetChildIndex(listBoxAddrs, 0);
            Controls.SetChildIndex(panelButtons, 0);
            panelButtons.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBoxAddrs;
        private Button buttonIpAddr;
        private Button buttonClose;
        private Button buttonIpHostAddr;
        private Panel panelButtons;
        private Button buttonMacs;
        private Button buttonConnectedIPAddrs;
    }
}