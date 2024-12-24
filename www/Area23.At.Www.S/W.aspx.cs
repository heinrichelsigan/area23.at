using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace Area23.At.Www.S
{
    public partial class W : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string addr = string.Empty;
            foreach (char c in Request.UserHostAddress.ToString())
            {
                foreach (char vc in "0123456789abcdef:.")
                {
                    if (c == vc)
                    {
                        addr += c;
                        break;
                    }                            
                }
            }

            Bitmap bmp = (Bitmap)MergeImage(addr);
            string phypath = Server.MapPath("~/res/img/");
            string fName = DateTime.Now.Ticks + ".png";
            
            // TODO: change this
            ImageIp4.ImageUrl = "data:image/png;base64," + bmp.ToBase64(new Guid("{b96b3caf-0728-11d3-9d7b-0000f81ef32e}"));
        }

        protected System.Drawing.Image MergeImage(string hexstring)
        {
            string phypath = Server.MapPath("~/res/img/");
            if (!phypath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                phypath += Path.DirectorySeparatorChar;
            string bmpName = phypath;
            ImageList list = new ImageList();
            Bitmap ximage = new Bitmap(720, 200);
            hexstring = hexstring.ToLower();

            System.Drawing.Bitmap mergeimg = new System.Drawing.Bitmap(720, 200);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(mergeimg))
            {
                int addInt = 0, j = 0;
                for (int i = 0; (i < hexstring.Length); i++)
                {
                    char ch = hexstring[i];
                    if ((((int)ch) >= ((int)'1')) && (((int)ch) <= ((int)'9')))
                        addInt = (((int)ch) - ((int)'1') + 1);
                    if (addInt > 0)
                        bmpName = phypath + addInt.ToString() + ".png";
                    else if (ch == '0' || ch == 'a' || ch == 'b' || ch == 'c' || ch == 'd' || ch == 'e' || ch == 'f')
                        bmpName = phypath + ch.ToString() + ".png";
                    else if (ch == '.')
                    {
                        bmpName = phypath + "Point.png";
                        if (j > 0) j--;
                    }
                    else if (ch == ':')
                    {
                        bmpName = phypath + "Colon.png";
                    }

                    ximage = new Bitmap(bmpName);
                    g.DrawImage(ximage, new System.Drawing.Rectangle(j * 60, 0, 60, 200));
                    j++;
                }
            }

            return (mergeimg);
        }
    
    
    }
}