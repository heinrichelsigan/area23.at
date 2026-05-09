using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Mono.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Windows.Media;
using static Area23.At.Mono.Controls.MenuControl;

namespace Area23.At.Mono.Controls
{

    public partial class MenuControl : System.Web.UI.UserControl
    {            

        string ipAddr = string.Empty;

        public bool IgnoreDefault { get; set; }


        private List<HeaderLink> _headerLinks;
        public List<HeaderLink> HeaderLinks { get => _headerLinks; }

        protected void Page_Init(object sender, EventArgs e)
        {
            ipAddr = Request.UserHostAddress;
            //if (!IsPostBack)
            //{
            //    _headerLinks = BuildMenu();
            //}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (_headerLinks == null || _headerLinks.Count == 0) 
                    _headerLinks = BuildMenu();
            }
        }

        public MenuControl()
        {
            _headerLinks = new List<HeaderLink>();
            IgnoreDefault = false; 
        }

        public static string GetCss(int num, int linksCount, bool select = false)
        {
            string cssSelect = (select) ? "Select" : "";
            if (num == 0)
                return "headerLeft" + cssSelect;
            else if (num == 1)
                return "headerLeftCenter" + cssSelect;
            else if (num == linksCount - 1)
                return "headerRight" + cssSelect;
            else if (num == linksCount - 2)
                return "headerRightCenter" + cssSelect;

            return "headerCenter" + cssSelect;
        }

        public List<HeaderLink> BuildMenu(bool ignoreDefault = false)
        {
            if (!IgnoreDefault)
                IgnoreDefault = ignoreDefault;

            var headerLinks = new List<HeaderLink>();
            string prefixPath = new Uri(ConfigurationManager.AppSettings["AppUrl"]).ToString();
            string absUriPath = Request.Url.AbsolutePath;
            
            string xyPath = Request.Url.AbsoluteUri.ToString();
            int ridx = xyPath.LastIndexOf("/");
            string relativePath = xyPath.Substring(0, ridx) + "/";

            string path = Path.GetDirectoryName(Server.MapPath(absUriPath));
            string[] files = Directory.GetFiles(path, "*.aspx");
            int h = 0;

            foreach (string file in files) 
            {
                if (file.Contains("Default.aspx") && !IgnoreDefault)
                {                    
                    HeaderLink hl = new HeaderLink();                    
                    hl.UTitle = Path.GetFileNameWithoutExtension(file);
                    hl.UUri = new Uri(relativePath + "Default.aspx");
                    hl.DivCss = GetCss(h, files.Length, (HttpContext.Current.Request.RawUrl.Contains("Default.aspx")));
                    hl.DivId = hl.DivCss.Replace("Select", "") + "Id" + h++;
                    headerLinks.Add(hl);
                    break;
                }
            }

            for (int i = 0; i < files.Length; i++)
            {
                HeaderLink hLink = new HeaderLink();
                if (File.Exists(files[i]) && !files[i].Contains("Default.aspx"))
                {
                    HeaderLink hl = new HeaderLink();
                    string uTitle = Path.GetFileNameWithoutExtension(files[i]);
                    for (int j = 0; j < uTitle.Length; j++)
                    {
                        char ch = uTitle[j];
                        if (Char.IsUpper(ch) && j > 0)
                            hl.UTitle += " " + ch;
                        else if (Char.IsDigit(ch) && j > 0 && uTitle[j - 1] != ' ' && !Char.IsDigit(uTitle[j - 1]))
                            hl.UTitle += " " + ch;
                        else if (char.IsWhiteSpace(ch) && j > 0 && Char.IsWhiteSpace(uTitle[j - 1]))
                            ;
                        else
                            hl.UTitle += ch;
                    }
                    
                    string fname = Path.GetFileName(files[i]);
                    hl.UUri = new Uri(relativePath + fname);
                    hl.DivCss = GetCss(h, files.Length, (HttpContext.Current.Request.RawUrl.Contains(fname)));
                    hl.DivId = hl.DivCss.Replace("Select", "") + "Id" + h++;
                    headerLinks.Add(hl);
                }
            }

            return headerLinks;           
        }

        public void BindMenu(List<HeaderLink> headerLinks)
        {
            if (headerLinks != null || headerLinks.Count == 0)
                headerLinks = BuildMenu(IgnoreDefault);        

            RepeaterLink.DataSource = headerLinks;
            RepeaterLink.DataBind();
        }

    }


}