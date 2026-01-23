using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Mono.Qr;
using Org.BouncyCastle.Utilities;
using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;


namespace Area23.At.Mono.Crypt
{
    public partial class CharHexDecOctBin : System.Web.UI.Page
    {

        char tChar = '\0';
        uint tNum = 0x0;
        uint tOct = 0;
        string tBin = "0000 0000";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                showMaps(tNum, tChar);                
            }
        }

        protected  void showMaps(uint num = 0, char ch = '\0')
        {
            if (ch == '\0' || ch == ' ' || ch == '\n' || ch == '\t' || ch == '\v' || ch == '\r' || ch == '\b')
                this.TextBoxChar.Text = "";
            else
                this.TextBoxChar.Text = ch.ToString();
            this.TextBoxHex.Text = $"{num:x}";
            this.TextBoxDec.Text = $"{num:d}";
            this.TextBoxOct.Text = MapOct(num);
            this.TextBoxBin.Text = MapBin(num);
        }

        protected uint MapChar(char ch)
        {
            uint unum = 0;
            if ((uint)ch >= (uint)'0' && (uint)ch <= (uint)'9')
                unum = (uint)ch - (uint)'0';
            else if ((uint)ch >= (uint)'A' && (uint)ch <= (uint)'F')
                unum = 10 + (uint)ch - (uint)'A';
            else if ((uint)ch >= (uint)'a' && (uint)ch <= (uint)'f')
                unum = 10 + (uint)ch - (uint)'a';
            return unum;
        }

        protected string MapOct(uint num)
        {
            string octString = Convert.ToString(num, 8);
            return octString;
        }

        protected string MapBin(uint num)
        {
            string b = "", s = string.Format("x2", num);
            s = s.Replace("0x", "");
            s = s.Replace("x", "");
            s = (s.Length == 2) ? s : "0" + s;             
            b = MapBin(s[0]) + " " + MapBin(s[1]);
            return b;
        }

        protected string MapBin(char ch)
        {
            switch (ch)
            {
                case '0':   return ("0000");
                case '1':   return ("0001");
                case '2':   return ("0010");
                case '3':   return ("0011");
                case '4':   return ("0100");
                case '5':   return ("0101");
                case '6':   return ("0110");
                case '7':   return ("0111");
                case '8':   return ("1000");
                case '9':   return ("1001");
                case 'A':
                case 'a':   return ("1010");
                case 'B':
                case 'b':   return ("1011");
                case 'C':
                case 'c':   return ("1100");
                case 'D':
                case 'd':   return ("1101");
                case 'E':
                case 'e':   return ("1110");
                case 'F':
                case 'f':   return ("1111");
                default:
                    throw new ArgumentException("Ilegal number char " + ch);
                    break;
            }
        }

        protected char IMapBin(string s)
        {
            string sf = "";
            for (int i = 0; i < s.Length; i++)
                sf += (s[i] == '0' || s[i] == '1') ? s[i].ToString() : "";
            if (sf == "0000" || sf == "000" || sf == "00" || sf == "0")
                return '0';
            else if (sf == "0001" || sf == "001" || sf == "01" || sf == "1")
                return '1';
            else if (sf == "0010" || sf == "010" || sf == "10")
                return '2';
            else if (sf == "0011" || sf == "011" || sf == "11")
                return '3';
            else if (sf == "0100" || sf == "100")
                return '4';
            else if (sf == "0101" || sf == "101")
                return '5';
            else if (sf == "0110" || sf == "110")
                return '6';
            else if (sf == "0111" || sf == "111")
                return '7';
            else if (sf == "1000")
                return '8';
            else if (sf == "1001")
                return '9';
            else if (sf == "1010")
                return 'a';
            else if (sf == "1011")
                return 'b';
            else if (sf == "1100")
                return 'c';
            else if (sf == "1101")
                return 'd';
            else if (sf == "1110")
                return 'e';
            else if (sf == "1111")
                return 'f';
            else
                throw new ArgumentException("Ilegal number char " + sf);

        }

        protected void MapCharHexDecOctBin(object sender, EventArgs e)
        {
            if (sender != null && sender is TextBox t)
            {
                try
                {
                    byte[] bytes = null;
                    switch (t.ID)
                    {
                        case "TextBoxChar":
                            tChar = TextBoxChar.Text[0];
                            tNum = (uint)tChar;
                            showMaps(tNum, tChar);
                            break;
                        case "TextBoxHex":
                            if (Byte.TryParse(TextBoxHex.Text, out byte tByte))
                            {
                                tNum = Convert.ToUInt32(tByte);
                                tChar = (char)tNum;
                                showMaps(tNum, tChar);
                            }
                            break;
                        case "TextBoxDec":
                            if (UInt32.TryParse(this.TextBoxDec.Text, out tNum))
                            {
                                tChar = (char)tNum;
                                showMaps(tNum, tChar);
                            }
                            break;
                        case "TextBoxOct":
                            string os = this.TextBoxOct.Text;
                            tNum = Convert.ToUInt32(os[0]);
                            if (os.Length > 1)
                                tNum += (8 * Convert.ToUInt32(os[1]));
                            if (os.Length > 2)
                                tNum += 64 * Convert.ToUInt32(os[2]);
                            tChar = (char) tNum;
                            showMaps(tNum, tChar);
                            break;

                        case "TextBoxBin":
                            string ob = this.TextBoxBin.Text;
                            string[] obs = ob.Split(" -.".ToCharArray());
                            char c1 = IMapBin(obs[1]);
                            char c0 = IMapBin(obs[0]);
                            tNum = 16 * MapChar(c0) + MapChar(c1);
                            tChar = (char)tNum;
                            showMaps(tNum, tChar);
                            break;

                        default:
                            break;
                    }
                    
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        protected void Button_Clear_Click(object sender, EventArgs e)
        {
            this.TextBoxChar.Text = string.Empty;
            this.TextBoxHex.Text = string.Empty;
            this.TextBoxDec.Text = string.Empty;
            this.TextBoxOct.Text = string.Empty;
            this.TextBoxBin.Text = string.Empty;
        }
    }
}