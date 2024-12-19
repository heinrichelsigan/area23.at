namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    partial class ContactSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContactSettings));
            tableLayoutPanel = new TableLayoutPanel();
            label1 = new Label();
            textBoxAddress = new TextBox();
            textBoxMobile = new TextBox();
            textBoxEmail = new TextBox();
            logoPictureBox = new PictureBox();
            labelName = new Label();
            labelEmail = new Label();
            labelMobile = new Label();
            labelAddress = new Label();
            okButton = new Button();
            pictureBoxImage = new PictureBox();
            comboBoxName = new ComboBox();
            openFileDialog = new OpenFileDialog();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Controls.Add(label1, 1, 4);
            tableLayoutPanel.Controls.Add(textBoxAddress, 2, 3);
            tableLayoutPanel.Controls.Add(textBoxMobile, 2, 2);
            tableLayoutPanel.Controls.Add(textBoxEmail, 2, 1);
            tableLayoutPanel.Controls.Add(logoPictureBox, 0, 0);
            tableLayoutPanel.Controls.Add(labelName, 1, 0);
            tableLayoutPanel.Controls.Add(labelEmail, 1, 1);
            tableLayoutPanel.Controls.Add(labelMobile, 1, 2);
            tableLayoutPanel.Controls.Add(labelAddress, 1, 3);
            tableLayoutPanel.Controls.Add(okButton, 2, 5);
            tableLayoutPanel.Controls.Add(pictureBoxImage, 2, 4);
            tableLayoutPanel.Controls.Add(comboBoxName, 2, 0);
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Font = new Font("Lucida Sans Unicode", 10F);
            tableLayoutPanel.Location = new Point(10, 10);
            tableLayoutPanel.Margin = new Padding(2, 1, 2, 1);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 6;
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel.Size = new Size(506, 309);
            tableLayoutPanel.TabIndex = 0;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(158, 120);
            label1.Margin = new Padding(7, 0, 4, 0);
            label1.MaximumSize = new Size(0, 20);
            label1.Name = "label1";
            label1.Size = new Size(90, 20);
            label1.TabIndex = 29;
            label1.Text = "Picture";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // textBoxAddress
            // 
            textBoxAddress.Dock = DockStyle.Fill;
            textBoxAddress.Location = new Point(254, 92);
            textBoxAddress.Margin = new Padding(2);
            textBoxAddress.Name = "textBoxAddress";
            textBoxAddress.Size = new Size(250, 28);
            textBoxAddress.TabIndex = 28;
            // 
            // textBoxMobile
            // 
            textBoxMobile.Dock = DockStyle.Fill;
            textBoxMobile.Location = new Point(254, 62);
            textBoxMobile.Margin = new Padding(2);
            textBoxMobile.Name = "textBoxMobile";
            textBoxMobile.Size = new Size(250, 28);
            textBoxMobile.TabIndex = 27;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Dock = DockStyle.Fill;
            textBoxEmail.Location = new Point(254, 32);
            textBoxEmail.Margin = new Padding(2);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(250, 28);
            textBoxEmail.TabIndex = 26;
            // 
            // logoPictureBox
            // 
            logoPictureBox.Dock = DockStyle.Fill;
            logoPictureBox.Image = (Image)resources.GetObject("logoPictureBox.Image");
            logoPictureBox.Location = new Point(3, 2);
            logoPictureBox.Margin = new Padding(3, 2, 3, 2);
            logoPictureBox.Name = "logoPictureBox";
            tableLayoutPanel.SetRowSpan(logoPictureBox, 6);
            logoPictureBox.Size = new Size(145, 305);
            logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            logoPictureBox.TabIndex = 12;
            logoPictureBox.TabStop = false;
            // 
            // labelName
            // 
            labelName.Dock = DockStyle.Fill;
            labelName.Location = new Point(158, 0);
            labelName.Margin = new Padding(7, 0, 4, 0);
            labelName.MaximumSize = new Size(0, 20);
            labelName.Name = "labelName";
            labelName.Size = new Size(90, 20);
            labelName.TabIndex = 19;
            labelName.Text = "Name";
            labelName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelEmail
            // 
            labelEmail.Dock = DockStyle.Fill;
            labelEmail.Location = new Point(158, 30);
            labelEmail.Margin = new Padding(7, 0, 4, 0);
            labelEmail.MaximumSize = new Size(0, 20);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(90, 20);
            labelEmail.TabIndex = 0;
            labelEmail.Text = "Email";
            labelEmail.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelMobile
            // 
            labelMobile.Dock = DockStyle.Fill;
            labelMobile.Location = new Point(158, 60);
            labelMobile.Margin = new Padding(7, 0, 4, 0);
            labelMobile.MaximumSize = new Size(0, 20);
            labelMobile.Name = "labelMobile";
            labelMobile.Size = new Size(90, 20);
            labelMobile.TabIndex = 21;
            labelMobile.Text = "Mobile";
            labelMobile.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelAddress
            // 
            labelAddress.Dock = DockStyle.Fill;
            labelAddress.Location = new Point(158, 90);
            labelAddress.Margin = new Padding(7, 0, 4, 0);
            labelAddress.MaximumSize = new Size(0, 20);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(90, 20);
            labelAddress.TabIndex = 22;
            labelAddress.Text = "Address";
            labelAddress.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.DialogResult = DialogResult.Cancel;
            okButton.Location = new Point(415, 280);
            okButton.Margin = new Padding(3, 2, 3, 2);
            okButton.Name = "okButton";
            okButton.Size = new Size(88, 27);
            okButton.TabIndex = 24;
            okButton.Text = "&OK";
            // 
            // pictureBoxImage
            // 
            pictureBoxImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBoxImage.BackgroundImageLayout = ImageLayout.Center;
            pictureBoxImage.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxImage.Location = new Point(254, 122);
            pictureBoxImage.Margin = new Padding(2);
            pictureBoxImage.Name = "pictureBoxImage";
            pictureBoxImage.Size = new Size(250, 150);
            pictureBoxImage.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBoxImage.TabIndex = 30;
            pictureBoxImage.TabStop = false;
            pictureBoxImage.Click += Image_Clicked;
            // 
            // comboBoxName
            // 
            comboBoxName.Dock = DockStyle.Fill;
            comboBoxName.FormattingEnabled = true;
            comboBoxName.Location = new Point(254, 2);
            comboBoxName.Margin = new Padding(2);
            comboBoxName.Name = "comboBoxName";
            comboBoxName.Size = new Size(250, 24);
            comboBoxName.TabIndex = 31;
            comboBoxName.SelectedIndexChanged += comboBoxName_SelectedIndexChanged;
            comboBoxName.TextUpdate += comboBoxName_TextUpdate;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // ContactSettings
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(526, 329);
            Controls.Add(tableLayoutPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ContactSettings";
            Padding = new Padding(10);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "ContactSettings";
            FormClosing += Form_Closing;
            FormClosed += Form_Closed;
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxImage).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelMobile;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.Button okButton;
        private Label label1;
        private TextBox textBoxAddress;
        private TextBox textBoxMobile;
        private TextBox textBoxEmail;
        private PictureBox pictureBoxImage;
        private OpenFileDialog openFileDialog;
        private ComboBox comboBoxName;
    }
}
