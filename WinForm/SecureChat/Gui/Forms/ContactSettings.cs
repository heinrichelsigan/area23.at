using Area23.At.Framework.Library.Core.Util;
using Area23.At.WinForm.SecureChat.Entities;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Area23.At.WinForm.SecureChat.Gui.Forms
{
    partial class ContactSettings : Form
    {
        private int _id = 1;
        private string base64image = string.Empty;
        private System.ComponentModel.ComponentResourceManager res = new System.ComponentModel.ComponentResourceManager(typeof(ContactSettings));
        public ContactSettings()
        {
            InitializeComponent();
            pictureBoxImage.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        public ContactSettings(string name, int id = 1) : this()
        {
            this.Text = name;
            _id = id;
            if (Entities.Settings.Instance != null)
            {
                if (_id == 0 && Entities.Settings.Instance.MyContact != null)
                {
                    this.comboBoxName.Text = Entities.Settings.Instance.MyContact.Name;
                    this.textBoxEmail.Text = Entities.Settings.Instance.MyContact.Email;
                    this.textBoxMobile.Text = Entities.Settings.Instance.MyContact.Mobile;
                    base64image = Entities.Settings.Instance.MyContact.ImageBase64 ?? string.Empty;
                    if (!string.IsNullOrEmpty(base64image))
                        this.pictureBoxImage.Image = base64image.Base64ToImage();
                }
                else if (_id > 0 && Entities.Settings.Instance.Contacts.Count > 0)
                {
                    foreach (Contact contact in Entities.Settings.Instance.Contacts)
                    {
                        if (!string.IsNullOrEmpty(contact.Name))
                            comboBoxName.Items.Add(contact.Name);
                    }
                }
            }
        }

        private void Image_Clicked(object sender, EventArgs e)
        {
            openFileDialog.RestoreDirectory = true;
            openFileDialog.AddExtension = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "Images|*.jpg|*.png|*.gif";
            DialogResult diaRes = openFileDialog.ShowDialog();
            if (diaRes == DialogResult.OK)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    byte[] bitmapBytes = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                    base64image = Framework.Library.Core.EnDeCoding.Base64.Encode(bitmapBytes);
                    Bitmap bmp = new Bitmap(openFileDialog.FileName);
                    int h = bmp.Size.Height;
                    int w = bmp.Size.Width;
                    if (h > 150)
                    {
                        float fh = (h / 150);
                        float fw = (w / fh);
                        Bitmap bitmap = ResizeImage(bmp, (int)fw, (int)150);
                        this.pictureBoxImage.Image = bitmap;
                    }
                    else 
                        this.pictureBoxImage.Image = bmp;
                    this.pictureBoxImage.Tag = openFileDialog.FileName;
                }
            }
        }

        private void Form_Closed(object sender, FormClosedEventArgs e)
        {
            bool foundContact = false;
            if (_id > 0)
            {
                string? currentSelectedName = (comboBoxName.SelectedItem != null) ? comboBoxName.SelectedItem.ToString() : null;
                if (!string.IsNullOrEmpty(currentSelectedName))
                {
                    foreach (Contact contact in Entities.Settings.Instance.Contacts)
                    {
                        if (contact != null && !string.IsNullOrEmpty(contact.Name) && contact.Name.Equals(currentSelectedName))
                        {
                            contact.Email = this.textBoxEmail.Text;
                            contact.Mobile = this.textBoxMobile.Text;
                            contact.Address = this.textBoxAddress.Text;                            
                            contact.ImageBase64 = (pictureBoxImage.Tag != null && pictureBoxImage.Tag.ToString() == "Upload image") ? 
                                null : this.pictureBoxImage.Image.ToBase64();

                            foundContact = true;
                            break;
                        }
                    }
                    if (!foundContact)
                    {
                        _id = Entities.Settings.Instance.Contacts.Count + 1;
                        Entities.Settings.Instance.Contacts.Add(
                            new Entities.Contact()
                            {
                                ContactId = _id,
                                Name = this.comboBoxName.Text ?? string.Empty,
                                Email = this.textBoxEmail.Text ?? string.Empty,
                                Mobile = this.textBoxMobile.Text ?? string.Empty,
                                Address = this.textBoxAddress.Text ?? string.Empty,
                                ImageBase64 = (pictureBoxImage.Tag != null && pictureBoxImage.Tag.ToString() == "Upload image") ?
                                    null : this.pictureBoxImage.Image.ToBase64()
                            }); 
                    }
                }
                Entities.Settings.Save(Entities.Settings.Instance);
                return;
            }
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (_id == 0)
            {
                Bitmap bitmap = new Bitmap(pictureBoxImage.Image);
                
                byte[] bytes = bitmap.ToByteArray();
                Entities.Settings.Instance.MyContact = new Entities.Contact()
                {
                    ContactId = 0,
                    Name = this.comboBoxName.Text ?? string.Empty,
                    Email = this.textBoxEmail.Text ?? string.Empty,
                    Mobile = this.textBoxMobile.Text ?? string.Empty,
                    Address = this.textBoxAddress.Text ?? string.Empty,
                    ImageBase64 = (pictureBoxImage.Tag != null && pictureBoxImage.Tag.ToString() == "Upload image") ? 
                        null : Framework.Library.Core.EnDeCoding.Base64.Encode(bytes)
                };
                Entities.Settings.Save(Entities.Settings.Instance);
            }
        }

        private void comboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? currentSelectedName = (comboBoxName.SelectedItem != null) ? comboBoxName.SelectedItem.ToString() : null;
            if (!string.IsNullOrEmpty(currentSelectedName))
            {
                foreach (Contact contact in Entities.Settings.Instance.Contacts)
                {
                    if (contact != null && !string.IsNullOrEmpty(contact.Name) && contact.Name.Equals(currentSelectedName))
                    {
                        this.textBoxEmail.Text = contact.Email ?? string.Empty;
                        this.textBoxMobile.Text = contact.Mobile ?? string.Empty;
                        this.textBoxAddress.Text = contact.Address ?? string.Empty;
                        base64image = contact.ImageBase64 ?? string.Empty;
                        if (!string.IsNullOrEmpty(base64image))
                        {
                            this.pictureBoxImage.Image = base64image.Base64ToImage();                            
                        }
                        else
                        {
                            pictureBoxImage.Tag = "Upload image";
                            logoPictureBox.Image = (Image)res.GetObject("logoPictureBox.Image");
                        }
                    }
                }

            }
        }

        private void comboBoxName_TextUpdate(object sender, EventArgs e)
        {
            string? currentSelectedName = (comboBoxName.Text != null) ? comboBoxName.Text.ToString() : null;
            if (!string.IsNullOrEmpty(currentSelectedName))
            {
                foreach (Contact contact in Entities.Settings.Instance.Contacts)
                {
                    if (contact != null && !string.IsNullOrEmpty(contact.Name) && contact.Name.Equals(currentSelectedName))
                    {
                        this.textBoxEmail.Text = contact.Email ?? string.Empty;
                        this.textBoxMobile.Text = contact.Mobile ?? string.Empty;
                        base64image = contact.ImageBase64 ?? string.Empty;
                        if (!string.IsNullOrEmpty(base64image))
                            this.pictureBoxImage.Image = base64image.Base64ToImage();
                        else
                        {
                            pictureBoxImage.Tag = "Upload image";
                            logoPictureBox.Image = (Image)res.GetObject("logoPictureBox.Image");
                        }
                            
                        break;
                    }
                }

            }
        }

        // <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
