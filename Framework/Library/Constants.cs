using System;
using System.Text;
using System.Web;

namespace Area23.At.Framework.Library
{

    /// <summary>
    /// static Constants including static application settings
    /// </summary>
    public static class Constants
    {

        #region public const

        public const int BACKLOG = 8;
        public const int CHAT_PORT = 7777;

        public const char ANNOUNCE = ':';
        public const char DATE_DELIM = '-';
        public const char WHITE_SPACE = ' ';
        public const char UNDER_SCORE = '_';

        public const string APP_NAME = "Area23.At";
        public const string APP_DIR = "net";
        public const string VERSION = "v2.24.1212";

        public const string AREA23_URL = "https://area23.at";
        public const string APP_PATH = "https://area23.at/net/";
        public const string RPN_URL = "https://area23.at/net/RpnCalc.aspx";
        public const string GIT_URL = "https://github.com/heinrichelsigan/area23.at";
        public const string URL_PIC = "https://area23.at/net/res/img/";
        public const string URL_PREFIX = "https://area23.at/net/res/";
        public const string AREA23_S = "https://area23.at/s/";
        public const string URL_SHORT = "https://area23.at/s/?";
        public const string AREA23_UTF8_URL = "https://area23.at/u/";

        public const string AUTHOR = "Heinrich Elsigan";
        public const string AUTHOR_EMAIL = "heinrich.elsigan@gmail.com";
        public const string AUTHOR_IV = "6865696E726963682E656C736967616E40676D61696C2E636F6D";
        public const string AREA23_EMAIL = "zen@area23.at";
        public const string AUTHOR_SIGNATURE = "-- \nHeinrich G.Elsigan\nTheresianumgasse 6/28, A-1040 Vienna\n phone: +43 650 752 79 28 \nmobile: +43 670 406 89 83 \nemails: heinrich.elsigan @gmail.com\n        root@darkstar.work he@area23.at\n        heinrich.elsigan @live.at\n        sites: darkstar.work area23.at\nweblog: blog.darkstar.work\n   wko: https://firmen.wko.at/DetailsKontakt.aspx?FirmaID=19800fbd-84a2-456d-890e-eb1fa213100f";

        public const string CALC_DIR = "Calc";
        public const string CSS_DIR = "css";
        public const string ENCODE_DIR = "Encode";
        public const string GAMES_DIR = "Gamez";
        public const string JS_DIR = "js";
        public const string JSON_SAVE_FILE = "urlshort.json";
        public const string LOG_DIR = "log";
        public const string LOG_EXT = ".log";
        public const string OUT_DIR = "out";
        public const string QR_DIR = "Qr";
        public const string RES_DIR = "res";
        public const string RES_FOLDER = "res";
        public const string TEXT_DIR = "text";
        public const string UNIX_DIR = "Unix";
        public const string UTF8_DIR = "Utf8";
        public const string UTF8_JSON = "utf8symol.json";
        public const string UU_DIR = "uu";

        public const string BIN_DIR = "bin";
        public const string OBJ_DIR = "obj";
        public const string RELEASE_DIR = "Release";
        public const string DEBUG_DIR = "Release";

        public const string ACCEPT_LANGUAGE = "Accept-Language";
        public const string FORTUNE_BOOL = "FORTUNE_BOOL";
        public const string UNKNOWN = "UnKnown";
        public const string DEFAULT_MIMETYPE = "application/octet-stream";
        public const string RPN_STACK = "rpnStack";
        public const string CHANGE_CLICK_EVENTCNT = "change_Click_EventCnt";
        public const string BC_START_MSG = "bc 1.07.1\r\nCopyright 1991-1994, 1997, 1998, 2000, 2004, 2006, 2008, 2012-2017 Free Software Foundation, Inc.\r\nThis is free software with ABSOLUTELY NO WARRANTY.\r\nFor details type `warranty'.\r\n";

        public const string BACK_COLOR = "BackColor";
        public const string QR_COLOR = "QrColor";
        public const string BACK_COLOR_STRING = "BackColorString";
        public const string QR_COLOR_STRING = "QrColorString";

        public const string ROACH_DESKTOP_WINDOW = "Roach.Desktop.Window";

        public const string EXE_COMMAND_CMD = "cmd";
        public const string EXE_POWER_SHELL = "powershell";

        public const string EXE_WIN_INIT = "wininit";
        public const string EXE_SERVICES = "services";
        public const string EXE_SVC_HOST = "svchost";
        public const string EXE_TASK_HOST = "taskhostw";
        public const string EXE_DLL_HOST = "dllhost";
        public const string EXE_SCHEDULER = "scheduler";
        public const string EXE_VM_COMPUTE = "vmcompute";
        public const string EXE_WIN_DEFENDER = "MsMpEng";
        public const string EXE_LASS = "lsass";                     // local Security Authority Subsystem Service. 
        public const string EXE_CSRSS = "csrss";                    // hosts the server side of the Win32 subsystem

        public const string EXE_WIN_LOGON = "winlogon";             // windows logon handler for current logon
        public const string EXE_DESKTOP_WINDOW_MANAGER = "dwm";     // window manager for current logon

        public const string STRING_EMPTY = "";
        public const string STRING_NULL = null;
        public const string SNULL = "(null)";

        public const string AES_ENVIROMENT_KEY = "APP_ENCRYPTION_SECRET_KEY";

        public const string JSON_SAMPLE = @"{ 
 	""quiz"": { 
 		""sport"": { 
 			""q1"": { 
 				""question"": ""Which one is correct team name in NBA?"", 
 					""options"": [ 
 						""New York Bulls"", 
 							""Los Angeles Kings"", 
 							""Golden State Warriros"", 
 							""Huston Rocket"" 
 						], 
 					""answer"": ""Huston Rocket"" 
 				} 
 			}, 
 		""maths"": { 
 			""q1"": { 
 				""question"": ""5 + 7 = ?"", 
 					""options"": [ 
 						""10"", 
 						""11"", 
 						""12"", 
 						""13"" 
 					], 
 					""answer"": ""12"" 
				}, 
 			""q2"": { 
 				""question"": ""12 - 8 = ?"", 
 				""options"": [ 
 						""1"", 
 						""2"", 
 						""3"", 
 						""4"" 
 						], 
 					""answer"": ""4"" 
 				}, 
 		} 
 	} 
 }";

        public const string XML_SAMPLE = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>
<ns2:Invoice xmlns=""http://www.w3.org/2000/09/xmldsig#"" xmlns:ns2=""http://www.ebinterface.at/schema/4p1/"" xmlns:ns3=""http://www.ebinterface.at/schema/4p1/extensions/sv"" xmlns:ns4=""http://www.ebinterface.at/schema/4p1/extensions/ext"" ns2:GeneratingSystem=""AUSTRIAPRO.ebInterface.Formular"" ns2:DocumentType=""Invoice"" ns2:InvoiceCurrency=""EUR"" ns2:ManualProcessing=""false"" ns2:DocumentTitle=""20240808"" ns2:Language=""deu"">
    <ns2:InvoiceNumber>20240808</ns2:InvoiceNumber><ns2:InvoiceDate>2024-08-08</ns2:InvoiceDate>
    <ns2:Delivery><ns2:Date>2024-08-08</ns2:Date></ns2:Delivery>
    <ns2:Biller>
        <ns2:VATIdentificationNumber>ATU72804824</ns2:VATIdentificationNumber>
        <ns2:Address>   
                <ns2:AddressIdentifier ns2:AddressIdentifierType=""GLN"">9110005479907</ns2:AddressIdentifier>
            <ns2:Name>Heinrich Georg Elsigan</ns2:Name>
            <ns2:Street>Theresianumgasse 6/28</ns2:Street>
            <ns2:Town>Wien</ns2:Town>
            <ns2:ZIP>1040</ns2:ZIP>
            <ns2:Country>AT</ns2:Country>
            <ns2:Phone>+43 650 7527928</ns2:Phone>
            <ns2:Email>office.area23@gmail.com</ns2:Email>
            <ns2:Contact>Herr Heinrich Elsigan </ns2:Contact>
        </ns2:Address>
    </ns2:Biller>
    <ns2:InvoiceRecipient>
        <ns2:VATIdentificationNumber>ATU54760904</ns2:VATIdentificationNumber>
        <ns2:OrderReference>
            <ns2:OrderID>pooler_Office2PDF</ns2:OrderID>
        </ns2:OrderReference>
        <ns2:Address>
            <ns2:AddressIdentifier ns2:AddressIdentifierType=""GLN"">9110016452449</ns2:AddressIdentifier>
            <ns2:Name>Logic4BIZ Informationstechnologie Gmbh</ns2:Name>
            <ns2:Street>Reisnerstraße 53, Hofhaus</ns2:Street>
            <ns2:Town>Wien</ns2:Town>
            <ns2:ZIP>1030</ns2:ZIP>
            <ns2:Country>AT</ns2:Country>
            <ns2:Phone>+43 1 877 18 81</ns2:Phone>
            <ns2:Email>office@logic4biz.com</ns2:Email>
            <ns2:Contact>Herr Peter Fasol </ns2:Contact>
        </ns2:Address>
    </ns2:InvoiceRecipient>
    <ns2:Details>
        <ns2:ItemList>
            <ns2:HeaderDescription>
                Der am 14.05.2024 beauftragte Office2PDF Spooler [ Quelle privates Github repository:
                github.com/heinrichelsigan/Spooler_Office2PDF ] ist seit heute für den letzten
                Integrationstest bereit.
                Release: https://github.com/heinrichelsigan/Spooler_Office2PDF/releases/tag/2024-08-
                08-final_PDF_Converter_Spooler
                Ich stelle daher in Absprache mit Matthias Wohlmann den Betrag von 3.696€ inkl. USt. für
                „Leistung Erstellung PDF Converter Spooler“ Rechnungsnummer 20240808 in Rechnung:
            </ns2:HeaderDescription>
        </ns2:ItemList>
    </ns2:Details>
    <ns2:Tax>
        <ns2:VAT/>
    </ns2:Tax>
    <ns2:TotalGrossAmount>0</ns2:TotalGrossAmount>
    <ns2:PayableAmount>0</ns2:PayableAmount>
    <ns2:PaymentMethod>
        <ns2:UniversalBankTransaction>
            <ns2:BeneficiaryAccount>
                <ns2:BIC>BKAUATWW</ns2:BIC>
                <ns2:IBAN>AT88 1100 0104 7029 6400</ns2:IBAN>
                <ns2:BankAccountOwner>Heinrich Elsigan</ns2:BankAccountOwner>
            </ns2:BeneficiaryAccount>
            <ns2:PaymentReference>20240808</ns2:PaymentReference>
        </ns2:UniversalBankTransaction>
    </ns2:PaymentMethod>
    <ns2:PaymentConditions>
        <ns2:DueDate>2033-01-13</ns2:DueDate>
    </ns2:PaymentConditions>
</ns2:Invoice>";

        #endregion public const

        #region public static readonly fields

        public static readonly char SEP_CHAR = System.IO.Path.DirectorySeparatorChar;

        public static readonly string AES_KEY = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("AesKey"));
        public static readonly string AES_IV = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("AesIv4"));
        public static readonly string DES3_KEY = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("DesKey"));
        public static readonly string DES3_IV = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("3DesIv"));
        // public static readonly string SERPENT_KEY = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("BOUNCE"));
        // public static readonly string SERPENT_IV = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("CASTLE"));
        public static readonly string BOUNCEK = Convert.ToBase64String(Encoding.UTF8.GetBytes("BOUNCE"));
        public static readonly string BOUNCE4 = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("CASTLE"));


        public static readonly string[] EXE_WIN_SYSTEM = { EXE_WIN_INIT, EXE_SERVICES,
            EXE_SVC_HOST, EXE_TASK_HOST, EXE_DLL_HOST,
            EXE_SCHEDULER, EXE_VM_COMPUTE, EXE_WIN_DEFENDER, EXE_LASS, EXE_CSRSS,
            EXE_WIN_LOGON, EXE_DESKTOP_WINDOW_MANAGER
        };

        #endregion public static readonly fields

        #region public static properties

        /// <summary>
        /// AppLogFile - logfile with <see cref="At.Framework.Library.Extensions.Area23Date(DateTime)"/> prefix
        /// </summary>
        public static string AppLogFile { get => DateTime.UtcNow.Area23Date() + UNDER_SCORE + APP_NAME + LOG_EXT; }


        public static string Json_Example { get => ResReader.GetValue("json_sample0"); }

        private static System.Globalization.CultureInfo locale = null;
        private static String defaultLang = null;

        /// <summary>
        /// Culture Info from HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE]
        /// </summary>
        public static System.Globalization.CultureInfo Locale
        {
            get
            {
                if (locale == null)
                {
                    defaultLang = "en";
                    try
                    {
                        if (HttpContext.Current.Request != null && HttpContext.Current.Request.Headers != null &&
                            HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE] != null)
                        {
                            string firstLang = HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE].
                                ToString().Split(',')[0];
                            defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                        }

                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                    catch (Exception)
                    {
                        defaultLang = "en";
                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                }
                return locale;
            }
        }

        public static string ISO2Lang { get => Locale.TwoLetterISOLanguageName; }

        /// <summary>
        /// UT DateTime @area23.at including seconds
        /// </summary>
        public static string DateArea23Seconds { get => DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"); }

        /// <summary>
        /// UTC DateTime Formated
        /// </summary>
        public static string DateArea23
        {
            get => DateTime.UtcNow.ToString("yyyy") + Constants.DATE_DELIM +
                DateTime.UtcNow.ToString("MM") + Constants.DATE_DELIM +
                DateTime.UtcNow.ToString("dd") + Constants.WHITE_SPACE +
                DateTime.UtcNow.ToString("HH") + Constants.ANNOUNCE +
                DateTime.UtcNow.ToString("mm") + Constants.ANNOUNCE + Constants.WHITE_SPACE;
        }

        /// <summary>
        /// UTC DateTime File Prefix
        /// </summary>
        public static string DateFile { get => DateArea23.Replace(WHITE_SPACE, UNDER_SCORE).Replace(ANNOUNCE, UNDER_SCORE); }

        private static readonly string backColorString = "#ffffff";
        public static string BackColorString
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[BACK_COLOR_STRING] != null) ?
                    (string)HttpContext.Current.Session[BACK_COLOR_STRING] : backColorString;
            set
            {
                HttpContext.Current.Session[BACK_COLOR] = ColorFrom.FromHtml(value);
                HttpContext.Current.Session[BACK_COLOR_STRING] = value;
            }
        }

        private static readonly string qrColorString = "#000000";
        public static string QrColorString
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[QR_COLOR_STRING] != null) ?
                    (string)HttpContext.Current.Session[QR_COLOR_STRING] : qrColorString;
            set
            {
                HttpContext.Current.Session[QR_COLOR] = ColorFrom.FromHtml(value);
                HttpContext.Current.Session[QR_COLOR_STRING] = value;
            }
        }

        public static System.Drawing.Color BackColor
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[BACK_COLOR] != null) ?
                    (System.Drawing.Color)HttpContext.Current.Session[BACK_COLOR] : ColorFrom.FromHtml(backColorString);
            set
            {
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (value != null)
                {
                    HttpContext.Current.Session[BACK_COLOR] = value;
                    HttpContext.Current.Session[BACK_COLOR_STRING] = value.ToXrgb();
                }
                else
                {
                    HttpContext.Current.Session[BACK_COLOR_STRING] = backColorString;
                    HttpContext.Current.Session[BACK_COLOR] = ColorFrom.FromHtml(backColorString);
                }
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
            }
        }

        public static System.Drawing.Color QrColor
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[QR_COLOR] != null) ?
                    (System.Drawing.Color)HttpContext.Current.Session[QR_COLOR] : ColorFrom.FromHtml(qrColorString);
            set
            {
#pragma warning disable CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
                if (value != null)
                {
                    HttpContext.Current.Session[QR_COLOR] = value;
                    HttpContext.Current.Session[QR_COLOR_STRING] = value.ToXrgb();
                }
                else
                {
                    HttpContext.Current.Session[QR_COLOR_STRING] = qrColorString;
                    HttpContext.Current.Session[QR_COLOR] = ColorFrom.FromHtml(qrColorString);
                }
#pragma warning restore CS8073 // The result of the expression is always the same since a value of this type is never equal to 'null'
            }
        }

        public static bool FortuneBool
        {
            get
            {
                if (HttpContext.Current.Session[FORTUNE_BOOL] == null)
                    HttpContext.Current.Session[FORTUNE_BOOL] = false;
                else
                    HttpContext.Current.Session[FORTUNE_BOOL] = !((bool)HttpContext.Current.Session[FORTUNE_BOOL]);

                return (bool)HttpContext.Current.Session[FORTUNE_BOOL];
            }
        }

        public static bool RandomBool { get => ((DateTime.Now.Millisecond % 2) == 0); }


        #endregion public static properties

    }

}