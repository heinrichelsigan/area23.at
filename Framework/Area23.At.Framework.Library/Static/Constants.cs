using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;


namespace Area23.At.Framework.Library.Static
{

    /// <summary>
    /// static Constants including static application settings
    /// </summary>
    public static class Constants
    {

        #region public const
#pragma warning disable CA1707 // Identifiers should not contain underscores

        public const int BACKLOG = 8;
        public const int CHAT_PORT = 7777;
        public const int MAX_KEY_LEN = 4096;
        public const int MAX_PIPE_LEN = 8;
        public const int MAX_SERVER_SOCKET_ADDRESSES = 16;
        public const int CLOSING_TIMEOUT = 6000;
        public const int MIN_SOCKET_BYTE_BUFFEER = 65536;       // 64 KB Buffer
        public const int SOCKET_BYTE_BUFFEER = 1048576;         //  1 MB Buffer
        public const int MAX_BYTE_BUFFEER = 4194240;            //  4 MB Buffer
        public const int MAX_SOCKET_BYTE_BUFFEER = 33554432;    //  32 MB Buffer  2^25
        public const bool CQR_ENCRYPT = true;
        public const bool ZEN_MATRIX_SYMMETRIC = false;

        public const char ANNOUNCE = ':';
        public const char DATE_DELIM = '-';
        public const char WHITE_SPACE = ' ';
        public const char UNDER_SCORE = '_';

        public const string APP_NAME = "Area23.At";
        public const string APP_DIR = "net";
        public const string APP_ERROR = "AppError";
        public const string VERSION = "v2.25.411";
        public const string VALKEY_CACHE_HOST = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com";
        public const int VALKEY_CACHE_PORT = 6379;
        public const string VALKEY_CACHE_HOST_PORT = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        public const string VALKEY_CACHE_HOST_PORT_KEY = "ValkeyCacheHostPort";
        public const string EXTERNAL_CLIENT_IP = "ExternalClientIP";
        public const string EXTERNAL_CLIENT_IP_V4 = "ExternalClientIPv4";
        public const string SERVER_IP_V4 = "ServerIPv4";
        public const string SERVER_IP_V6 = "ServerIPv6";
        public const string CQR_SERVICE_SOAP = "CqrServiceSoap";
        public const string CQR_SERVICE_SOAP12 = "CqrServiceSoap12";
        public const string CQR_SRV_SOAP = "CqrSrvSoap";
        public const string CQR_SRV_SOAP12 = "CqrSrvSoap12";


        public const string AREA23_URL = "https://area23.at";
        public const string APP_PATH = "https://area23.at/net/";
        public const string RPN_URL = "https://area23.at/net/RpnCalc.aspx";
        public const string GIT_URL = "https://github.com/heinrichelsigan/area23.at";
        public const string URL_PIC = "https://area23.at/net/res/img/";
        public const string URL_PREFIX = "https://area23.at/net/res/";
        public const string AREA23_S = "https://area23.at/s/";
        public const string URL_SHORT = "https://area23.at/s/?";
        public const string AREA23_UTF8_URL = "https://area23.at/u/";

        public const string AREA23_AT = "area23.at";
        public const string VIRGINA_AREA23_AT = "virginia.area23.at";
        public const string PARIS_AREA23_AT = "paris.area23.at";
        public const string PARISIENNE_AREA23_AT = "parisienne.area23.at";
        public const string CQRXS_EU = "cqrxs.eu";
        public const string IPV4_CQRXS_EU = "ipv4.cqrxs.eu";
        public const string IPV6_CQRXS_EU = "ipv6.cqrxs.eu";

        public const string ES_CQRXS_EU = "es.cqrxs.eu";
        public const string MADRID_CQRXS_EU = "madrid.cqrxs.eu";
        public const string BARCELONA_CQRXS_EU = "barcelona.cqrxs.eu";

        public const string IT_CQRXS_EU = "it.cqrxs.eu";
        public const string MILAN_CQRXS_EU = "milan.cqrxs.eu";
        public const string SICILIENNE_CQRXS_EU = "sicilienne.cqrxs.eu";


        public const string FR_CQRXS_EU = "fr.cqrxs.eu";
        public const string PARIS_CQRXS_EU = "paris.cqrxs.eu";
        public const string PARISIENNSE_CQRXS_EU = "parisienne.cqrxs.eu";

        public const string DE_CQRXS_EU = "de.cqrxs.eu";
        public const string FRANKFURT_CQRXS_EU = "frankfurt.cqrxs.eu";
        public const string BERLINERIN_CQRXS_EU = "berlinerin.cqrxs.eu";

        public const string SE_CQRXS_EU = "se.cqrxs.eu";
        public const string STOCKHOLM_CQRXS_EU = "stockholm.cqrxs.eu";

        public const string IE_CQRXS_EU = "ie.cqrxs.eu";
        public const string DUBLIN_CQRXS_EU = "dublin.cqrxs.eu";
        public const string GALWAY_CQRXS_EU = "galway.cqrxs.eu";

        public const string UK_CQRXS_EU = "uk.cqrxs.eu";
        public const string LONDON_CQRXS_EU = "london.cqrxs.eu";
        public const string EDINBURGH_CQRXS_EU = "edinburgh.cqrxs.eu";

        public const string CH_CQRXS_EU = "ch.cqrxs.eu";
        public const string ZURICH_CQRXS_EU = "zurich.cqrxs.eu";
        public const string BERNERIN_CQRXS_EU = "bernerin.cqrxs.eu";


        public const string ALL_KEYS = "AllKeys";
        public const string CHATROOMS = "ChatRooms";
        public const string CQRXS_URL = "https://cqrxs.eu/";
        public const string CQRXS_HELP_URL = "https://cqrxs.eu/help/";
        public const string DECRYPTED_TEXT_AREA = "<textarea cols = \"48\" rows=\"10\" name=\"TextBoxDecrypted\" id=\"TextBoxDecrypted\" title=\"TextBox Current Message\" ValidateRequestMode=\"Enabled\" style=\"width:480px;\" >";
        public const string DECRYPTED_TEXT_BOX = "TextBoxDecrypted";
        public const string DECRYPTED_TEXT_AREA_END = "</textarea>";
        public const string CQRXS_TEST_FORM = "CqrXsTestForm";
        public const string FISH_ON_AES_ENGINE = "FishOnAesEngine";
        public const string CQRXS_DELETE_DATA_ON_CLOSE = "CqrXsDeleteDataOnClose";
        public const string PERSIST_MSG_IN = "PersistMsgIn";
        public const string PERSIST_MSG_IN_APPLICATION_STATE = "ApplicationState";
        public const string PERSIST_MSG_IN_AMAZON_ELASTIC_CACHE = "AmazonElasticCache";
        public const string PERSIST_MSG_IN_FILE_SYSTEM = "FileSystem";

        public const string ACK = "Ack";
        public const string NACK = "Nack";
        public const string ENTER_SECRET_KEY = "[enter secret key here]";
        public const string ENTER_IP_CONTACT = "[Enter IPv4/IPv6 or select Contact]";
        public const string ENTER_IP = "[Enter peer IPv4/IPv6]";
        public const string ENTER_CONTACT = "[Select Contact]";

        public const string ACCEPT_LANGUAGE = "Accept-Language";
        public const string AES_ENVIROMENT_KEY = "APP_ENCRYPTION_SECRET_KEY";
        public const string AUTHOR = "Heinrich Elsigan";
        public const string AUTHOR_EMAIL = "heinrich.elsigan@area23.at";
        public const string AUTHOR_IV = "6865696e726963682e656c736967616e406172656132332e6174";
        public const string AREA23_EMAIL = "zen@area23.at";
        public const string AUTHOR_SIGNATURE = "-- \nHeinrich G.Elsigan\nTheresianumgasse 6/28, A-1040 Vienna\n phone: +43 650 752 79 28 \nmobile: +43 670 406 89 83 \nemails: heinrich.elsigan @gmail.com\n        heinrich.elsigan@live.at\n        sites: area23.at cqrxs.eu\nweblog: blog.area23.at\n   wko: https://firmen.wko.at/DetailsKontakt.aspx?FirmaID=19800fbd-84a2-456d-890e-eb1fa213100f";

        public const string APP_CONCURRENT_DICT = "APP_CONCURRENT_DICT";
        public const string APP_FIRST_REG = "APP_FIRST_REG";
        public const string APP_TRANSPARENT_BADGE = "APP_TRANSPARENT_BADGE";
        public const string APP_SERVER_KEY = "APP_SERVER_KEY";
        public const string APP_INPUT_DIALOG = "APP_INPUT_DIALOG";
        public const string APP_MY_CONTACT = "APP_MY_CONTACT";

        public const string APP_DIR_PATH_WIN = "AppDirPathWin";
        public const string BASE_APP_PATH_WIN = "BaseAppPathWin";
        public const string APP_DIR_PATH_UNIX = "AppDirPathUnix";
        public const string BASE_APP_PATH_UNIX = "BaseAppPathUnix";

        public const string BIN_DIR = "bin";
        public const string CALC_DIR = "Calc";
        public const string CSS_DIR = "css";
        public const string CRYPT_DIR = "Crypt";
        public const string ENCODE_DIR = "Crypt";
        public const string GAMES_DIR = "Gamez";
        public const string IMG_DIR = "img";
        public const string IMG_FOLDER = "Image";
        public const string JS_DIR = "js";
        public const string JSON_DIR = "json";
        public const string LOG_DIR = "log";
        public const string LOG_EXT = ".log";
        public const string LOG_EXCEPTION_STATIC = "LogExceptionStatic";
        public const string OUT_DIR = "out";
        public const string QR_DIR = "Qr";
        public const string RES_DIR = "res";
        public const string RES_FOLDER = "res";
        public const string TEXT_DIR = "text";
        public const string TMP_DIR = "tmp";
        public const string UNIX_DIR = "Unix";
        public const string UTF8_DIR = "Utf8";
        public const string UU_DIR = "uu";

        public const string OBJ_DIR = "obj";
        public const string RELEASE_DIR = "Release";
        public const string DEBUG_DIR = "Release";
        public const string NET9_WINDOWS7 = "net9.0-windows7.0";
        public const string NET9_WINDOWS8 = "net9.0-windows8.0";
        public const string NET9_WINDOWS10 = "net9.0-windows10";
        public const string NET9_WINDOWS11 = "net9.0-windows11";
        public const string WIN_X86 = "win-x86";
        public const string WIN_X64 = "win-x86";
        public const string MIME_EXT = ".mime";
        public const string BASE64_EXT = ".base64";
        public const string ATTACH_FILES_DIR = "AttachFiles";
        public const string UPSAVED_FILE = "SavedFile";

        public const string UTF8_JSON = "utf8symol.json";
        public const string JSON_SAVE_FILE = "urlshort.json";
        public const string JSON_CONTACTS = "contacts";
        public const string JSON_CONTACTS_FILE = "contacts.json";
        public const string JSON_SETTINGS_FILE = "settings.json";
        public const string CQR_CHAT_FILE = "cqr{0}chat.json";
        public const string PREVIOUS_EXCEPTION = "previous_exception";
        public const string LAST_EXCEPTION = "last_exception";
        public const string COOL_CRYPT_SPLIT = "+-;,:→⇛\t ";


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
        public const string MUTEX_REGOPS = "Mutex.Registry.Operations";

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


        public const string RSA_PUB = @"-----BEGIN PUBLIC KEY-----
MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEA468PZ0zl0lXQXX6vkpeM
ciGeffjHa1Uv+YSxGKxkn+0km7HZ8EwFU5ia01Jkk+VevPCQQiTusY3Renfau4pE
cgvGHEqgUG3XHPFmtlEJh6Cz9DcLajKC4a281UAEq/D108CSDHkNbxp2xpZTqJ+l
0aNjY+UUv5IFm5wfoPsJ0QghQ1Z3XsOcKgf0ztUZ1IpbmnfSkQO21EjUUeGqhHiv
nfri3/c7nx/adUismR5gzR8yxgU3OyJIDAr9JLzKCbaoWwokfID+oX3tibHjCKEo
6lnzfO3LpGCb11Dhg77+nKi4GcHF7GZBdjhnVfFo/313Qcewu4kVK8rKJ2K3NIl4
j85V6oaPPRzw+iR1zfr6J4mGMnAmIY0C3EBYjVpuhTZS06kRsSFOlYmwxeg8Ig16
GXVCC9UONsRIY7nABLnZ3NQREpqHzX7iQVL0gXFidz0sDcJmxxFM56Oa64+Hbihj
PLZZAas9p5Uie5W7k2wsxTNwI6tRZPIKUZ59czbnLFoocWERh2/D5K0z4TUhUen5
6x0m8uvhqfQ1hRt9aoqCMvTCDNB384MTAh2bYDQpOnx81i/Jgr6HVTGajScd/KqW
HQQzvEE8gcOOxbyZ2p34QyKyyei8tKLRu0AUwJaGc/NErkKzHIIIziMJVx5LfxWU
8zrWQz53qDfl3xmZWZJDcfkCAwEAAQ==
-----END PUBLIC KEY-----";

        public const string RSA_PRV = @"-----BEGIN PRIVATE KEY-----
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
BM9nt+Q/uD3zNnN2XmawDWK04lkPNPwVSjqTkkT5AoIBADTDj8VIDNt7hCaWNs6f
bXOcY8P6xGnllykXxoIZMM/kguGQuj3JA4/2FSesI2J52aqUzmMY4UXsRyvGI0in
WwNmzVfLPs9fVdZP5ssJFrz1riXhl+Rx1UqIuaz0H5OYnh6VkCq8v47/LOkW2+8w
COVQwo72TZIokXlaOjavnXCBS2yKPS2wfB3CZOuZ5Pne1t1CvRpnJVBj50jv1XNu
M2ums3m2Dx2rQIN+SliNNZ15aY56LqYvp+sBHGckoBt8wjYuhS4L4oTUDWLCMKoK
G2fBxPJO6VoLg+cdoeAuvq3niCIpyY+HR/eopjdri4c7BqIQvu+9hybVmDwngZL2
zSUCggEAAhKizyiTBs5EHJX/pBVu2cC3zE9JjJqebo/uIaX88fYjexiBXIqqBRHK
iDQJZz8xocNzuCPrVT194ICLXEelLsfaQhqDKLnYJwpjjaO88df3WtSnzlNRkg6o
ZUuLOSvkHGbUNYw6jATp8nbHZ1rny6b/k9R8zPStKaLWRuq9BScsNonYCP20YMYa
LzdV9UIxeQ28zY59vJnwijbb95qzK0Ei3gPwo8+WY6rBIt24800iqK5LmhswzmLc
PMsi2xTrUPC6pAERVgu7wz02ka3WPOdlxfoG0o9s/BwJmhi5EEBqGB4CriR8R8AY
2sGnnAaPJgE8Iy2z08jS3rF9npK27A==
-----END PRIVATE KEY-----";




#pragma warning restore CA1707 // Identifiers should not contain underscores
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

        public static readonly string[] OUTFILE_EXTENSIONS = {
            ".md", ".txt", ".text", ".cfg",
            ".css", ".js", ".htm", ".html", ".xhtml", ".json", ".rdf",

            ".avif", ".bmp", ".exif", ".gif", ".ico", ".ief", ".jpg", ".jpeg", ".pcx", ".pic", ".png", ".psd", ".tif", ".xcf", ".xif",
            ".3pg", ".3g2", ".aif", ".au", ".m3u", ".mid", ".midi", ".mp4", ".mpeg", ".ogg", ".webm", ".wav", ".wax", ".wma", ".mp3",
            ".avi", ".f4v", ".flx", ".m4u", ".m4v", ".mov", ".mpg", ".wmv",

            ".pdf", ".ps", ".gs", ".dvi", ".tex",
            ".ods", ".odt", ".rtf", ".doc", ".dot", ".xls", ".xlt", ".csv", ".mdb", ".ppt", ".vsx", ".vst", ".mpp",

            ".ttf", ".woff",

            ".eml", ".mbox", ".vcs", ".vcf", ".msg",

            ".zip",
            ".z", ".gz", ".bz", ".bz2", ".tar", ".tgz", ".tbz",
            ".arj", ".arc", ".rar",
            ".7z", ".xz",
            ".uu", ".base", ".mime",

            ".pki", ".cer", ".der", ".crl", ".p10", ".p7c", ".p7s",

            ".exe", ".dll", ".oct", ".bin", ".tmp", ".img"
        };

        #endregion public static readonly fields

        #region public static properties

        private static bool _unix = false;
        public static bool UNIX
        {
            get
            {
                if (_unix)
                    return _unix;

                string pathUnix = "";

                if (ConfigurationManager.AppSettings["AppDirPathUnix"] != null)
                    pathUnix = ConfigurationManager.AppSettings["AppDirPathUnix"];

                _unix = ((System.AppDomain.CurrentDomain.BaseDirectory.ToString().Contains("/") &&
                            !System.AppDomain.CurrentDomain.BaseDirectory.ToString().Contains("\\"))
                        || Directory.Exists(pathUnix));

                return _unix;
            }
        }

        private static bool _win32 = false;

        public static bool WIN32
        {
            get
            {
                if (_win32)
                    return _win32;

                string pathWin32 = "";

                if (ConfigurationManager.AppSettings["AppDirPathWin"] != null)
                    pathWin32 = ConfigurationManager.AppSettings["AppDirPathWin"];

                _win32 = ((AppDomain.CurrentDomain.BaseDirectory.Contains("\\") &&
                            !AppDomain.CurrentDomain.BaseDirectory.Contains("/"))
                        || Directory.Exists(pathWin32));

                return _win32;
            }
        }

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

        /// <summary>
        /// AppSettingsValueByKey 
        /// </summary>
        /// <param name="key">key to lookup up in appsettings key value collection</param>
        /// <returns><see cref="string"/> AppSettingsValue</returns>
        public static string AppSettingsValueByKey(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings[key] != null)
                {
                    return (string)(System.Configuration.ConfigurationManager.AppSettings[key].ToString());
                }
            }
            catch { }

            return null;
        }


    }

}