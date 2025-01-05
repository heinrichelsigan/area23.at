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
            logoPictureBox = new PictureBox();
            labelName = new Label();
            labelEmail = new Label();
            labelMobile = new Label();
            labelAddress = new Label();
            labelKey = new Label();
            labelPicture = new Label();
            comboBoxName = new ComboBox();
            textBoxEmail = new TextBox();
            textBoxMobile = new TextBox();
            textBoxAddress = new TextBox();
            textBoxKey = new TextBox();
            pictureBoxImage = new PictureBox();
            okButton = new Button();
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
            tableLayoutPanel.Controls.Add(logoPictureBox, 0, 0);
            tableLayoutPanel.Controls.Add(labelName, 1, 0);
            tableLayoutPanel.Controls.Add(labelEmail, 1, 1);
            tableLayoutPanel.Controls.Add(labelMobile, 1, 2);
            tableLayoutPanel.Controls.Add(labelAddress, 1, 3);
            tableLayoutPanel.Controls.Add(labelKey, 1, 4);
            tableLayoutPanel.Controls.Add(labelPicture, 1, 5);
            tableLayoutPanel.Controls.Add(comboBoxName, 2, 0);
            tableLayoutPanel.Controls.Add(textBoxEmail, 2, 1);
            tableLayoutPanel.Controls.Add(textBoxMobile, 2, 2);
            tableLayoutPanel.Controls.Add(textBoxAddress, 2, 3);
            tableLayoutPanel.Controls.Add(textBoxKey, 2, 4);
            tableLayoutPanel.Controls.Add(pictureBoxImage, 2, 5);
            tableLayoutPanel.Controls.Add(okButton, 2, 6);
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
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel.Size = new Size(519, 326);
            tableLayoutPanel.TabIndex = 0;
            // 
            // logoPictureBox
            // 
            logoPictureBox.Dock = DockStyle.Fill;
            logoPictureBox.Image = (Image)resources.GetObject("logoPictureBox.Image");
            logoPictureBox.Location = new Point(3, 2);
            logoPictureBox.Margin = new Padding(3, 2, 3, 2);
            logoPictureBox.Name = "logoPictureBox";
            tableLayoutPanel.SetRowSpan(logoPictureBox, 6);
            logoPictureBox.Size = new Size(149, 286);
            logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            logoPictureBox.TabIndex = 25;
            logoPictureBox.TabStop = false;
            // 
            // labelName
            // 
            labelName.Dock = DockStyle.Fill;
            labelName.Location = new Point(162, 0);
            labelName.Margin = new Padding(7, 0, 4, 0);
            labelName.MaximumSize = new Size(0, 20);
            labelName.Name = "labelName";
            labelName.Size = new Size(92, 20);
            labelName.TabIndex = 26;
            labelName.Text = "Name";
            labelName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelEmail
            // 
            labelEmail.Dock = DockStyle.Fill;
            labelEmail.Location = new Point(162, 32);
            labelEmail.Margin = new Padding(7, 0, 4, 0);
            labelEmail.MaximumSize = new Size(0, 20);
            labelEmail.Name = "labelEmail";
            labelEmail.Size = new Size(92, 20);
            labelEmail.TabIndex = 27;
            labelEmail.Text = "Email";
            labelEmail.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelMobile
            // 
            labelMobile.Dock = DockStyle.Fill;
            labelMobile.Location = new Point(162, 64);
            labelMobile.Margin = new Padding(7, 0, 4, 0);
            labelMobile.MaximumSize = new Size(0, 20);
            labelMobile.Name = "labelMobile";
            labelMobile.Size = new Size(92, 20);
            labelMobile.TabIndex = 28;
            labelMobile.Text = "Mobile";
            labelMobile.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelAddress
            // 
            labelAddress.Dock = DockStyle.Fill;
            labelAddress.Location = new Point(162, 96);
            labelAddress.Margin = new Padding(7, 0, 4, 0);
            labelAddress.MaximumSize = new Size(0, 20);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(92, 20);
            labelAddress.TabIndex = 29;
            labelAddress.Text = "Address";
            labelAddress.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelKey
            // 
            labelKey.Dock = DockStyle.Fill;
            labelKey.Location = new Point(162, 128);
            labelKey.Margin = new Padding(7, 0, 4, 0);
            labelKey.MaximumSize = new Size(0, 20);
            labelKey.Name = "labelKey";
            labelKey.Size = new Size(92, 20);
            labelKey.TabIndex = 30;
            labelKey.Text = "Secret Key";
            labelKey.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelPicture
            // 
            labelPicture.Dock = DockStyle.Fill;
            labelPicture.Location = new Point(162, 160);
            labelPicture.Margin = new Padding(7, 0, 4, 0);
            labelPicture.MaximumSize = new Size(0, 20);
            labelPicture.Name = "labelPicture";
            labelPicture.Size = new Size(92, 20);
            labelPicture.TabIndex = 31;
            labelPicture.Text = "Picture";
            labelPicture.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboBoxName
            // 
            comboBoxName.Dock = DockStyle.Fill;
            comboBoxName.FormattingEnabled = true;
            comboBoxName.Location = new Point(260, 2);
            comboBoxName.Margin = new Padding(2);
            comboBoxName.Name = "comboBoxName";
            comboBoxName.Size = new Size(257, 24);
            comboBoxName.TabIndex = 20;
            comboBoxName.SelectedIndexChanged += comboBoxName_SelectedIndexChanged;
            comboBoxName.TextUpdate += comboBoxName_TextUpdate;
            // 
            // textBoxEmail
            // 
            textBoxEmail.Dock = DockStyle.Fill;
            textBoxEmail.Location = new Point(260, 34);
            textBoxEmail.Margin = new Padding(2);
            textBoxEmail.Name = "textBoxEmail";
            textBoxEmail.Size = new Size(257, 28);
            textBoxEmail.TabIndex = 21;
            // 
            // textBoxMobile
            // 
            textBoxMobile.Dock = DockStyle.Fill;
            textBoxMobile.Location = new Point(260, 66);
            textBoxMobile.Margin = new Padding(2);
            textBoxMobile.Name = "textBoxMobile";
            textBoxMobile.Size = new Size(257, 28);
            textBoxMobile.TabIndex = 22;
            // 
            // textBoxAddress
            // 
            textBoxAddress.Dock = DockStyle.Fill;
            textBoxAddress.Location = new Point(260, 98);
            textBoxAddress.Margin = new Padding(2);
            textBoxAddress.Name = "textBoxAddress";
            textBoxAddress.Size = new Size(257, 28);
            textBoxAddress.TabIndex = 23;
            // 
            // textBoxKey
            // 
            textBoxKey.Dock = DockStyle.Fill;
            textBoxKey.Location = new Point(260, 130);
            textBoxKey.Margin = new Padding(2);
            textBoxKey.Name = "textBoxKey";
            textBoxKey.Size = new Size(257, 28);
            textBoxKey.TabIndex = 24;
            // 
            // pictureBoxImage
            // 
            pictureBoxImage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBoxImage.BackgroundImageLayout = ImageLayout.Center;
            pictureBoxImage.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxImage.Location = new Point(260, 162);
            pictureBoxImage.Margin = new Padding(2);
            pictureBoxImage.Name = "pictureBoxImage";
            pictureBoxImage.Size = new Size(257, 126);
            pictureBoxImage.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBoxImage.TabIndex = 3;
            pictureBoxImage.TabStop = false;
            pictureBoxImage.Click += Image_Clicked;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.DialogResult = DialogResult.Cancel;
            okButton.Location = new Point(428, 297);
            okButton.Margin = new Padding(3, 2, 3, 2);
            okButton.Name = "okButton";
            okButton.Size = new Size(88, 27);
            okButton.TabIndex = 32;
            okButton.Text = "&OK";
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
            ClientSize = new Size(539, 346);
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
        private Label labelKey;
        private Label labelPicture;
        private TextBox textBoxAddress;
        private TextBox textBoxMobile;
        private TextBox textBoxEmail;
        private TextBox textBoxKey;
        private PictureBox pictureBoxImage;
        private OpenFileDialog openFileDialog;
        private ComboBox comboBoxName;
    }
}
