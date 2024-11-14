namespace Area23.At.WinForm.TWinFormCore.UI.Forms
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
            buttonMacs = new Button();
            buttonIpAddr = new Button();
            buttonClose = new Button();
            buttonIpHostAddr = new Button();
            SuspendLayout();
            // 
            // listBoxAddrs
            // 
            listBoxAddrs.BorderStyle = BorderStyle.FixedSingle;
            listBoxAddrs.Font = new Font("Lucida Console", 10F);
            listBoxAddrs.FormattingEnabled = true;
            listBoxAddrs.ItemHeight = 13;
            listBoxAddrs.Location = new Point(12, 42);
            listBoxAddrs.Margin = new Padding(1);
            listBoxAddrs.Name = "listBoxAddrs";
            listBoxAddrs.Size = new Size(352, 353);
            listBoxAddrs.TabIndex = 1;
            // 
            // buttonMacs
            // 
            buttonMacs.Font = new Font("Lucida Console", 10F);
            buttonMacs.Location = new Point(12, 415);
            buttonMacs.Margin = new Padding(1);
            buttonMacs.Name = "buttonMacs";
            buttonMacs.Size = new Size(101, 25);
            buttonMacs.TabIndex = 3;
            buttonMacs.Text = "MacAddr";
            buttonMacs.UseVisualStyleBackColor = true;
            buttonMacs.Click += buttonMacs_Click;
            // 
            // buttonIpAddr
            // 
            buttonIpAddr.Font = new Font("Lucida Console", 10F);
            buttonIpAddr.Location = new Point(140, 415);
            buttonIpAddr.Margin = new Padding(1);
            buttonIpAddr.Name = "buttonIpAddr";
            buttonIpAddr.Size = new Size(101, 25);
            buttonIpAddr.TabIndex = 4;
            buttonIpAddr.Text = "IpAddr";
            buttonIpAddr.UseVisualStyleBackColor = true;
            buttonIpAddr.Click += buttonIpAddr_Click;
            // 
            // buttonClose
            // 
            buttonClose.Font = new Font("Lucida Console", 10F);
            buttonClose.Location = new Point(689, 415);
            buttonClose.Margin = new Padding(1);
            buttonClose.Name = "buttonClose";
            buttonClose.Size = new Size(101, 25);
            buttonClose.TabIndex = 5;
            buttonClose.Text = "Close";
            buttonClose.UseVisualStyleBackColor = true;
            buttonClose.Click += buttonClose_Click;
            // 
            // buttonIpHostAddr
            // 
            buttonIpHostAddr.Font = new Font("Lucida Console", 10F);
            buttonIpHostAddr.Location = new Point(263, 415);
            buttonIpHostAddr.Margin = new Padding(1);
            buttonIpHostAddr.Name = "buttonIpHostAddr";
            buttonIpHostAddr.Size = new Size(101, 25);
            buttonIpHostAddr.TabIndex = 6;
            buttonIpHostAddr.Text = "IpHostAddr";
            buttonIpHostAddr.UseVisualStyleBackColor = true;
            buttonIpHostAddr.Click += buttonIpHostAddr_Click;
            // 
            // NetAddr
            // 
            AutoScaleDimensions = new SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonIpHostAddr);
            Controls.Add(buttonClose);
            Controls.Add(buttonIpAddr);
            Controls.Add(buttonMacs);
            Controls.Add(listBoxAddrs);
            Name = "NetAddr";
            Text = "NetAddr";
            Controls.SetChildIndex(listBoxAddrs, 0);
            Controls.SetChildIndex(buttonMacs, 0);
            Controls.SetChildIndex(buttonIpAddr, 0);
            Controls.SetChildIndex(buttonClose, 0);
            Controls.SetChildIndex(buttonIpHostAddr, 0);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBoxAddrs;
        private Button buttonMacs;
        private Button buttonIpAddr;
        private Button buttonClose;
        private Button buttonIpHostAddr;
    }
}