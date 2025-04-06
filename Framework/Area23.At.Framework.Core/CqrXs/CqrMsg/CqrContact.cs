using Area23.At.Framework.Core.Static;
using Newtonsoft.Json;

namespace Area23.At.Framework.Core.CqrXs.CqrMsg
{

    /// <summary>
    /// CqrContact is a contact for CqrJd
    /// </summary>
    [Serializable]
    public class CqrContact : MsgContent, ICqrMessagable
    {

        #region properties

        public int ContactId { get; set; }

        public Guid Cuid { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string SecretKey { get; set; }

        public CqrImage? ContactImage { get; set; }

        public string NameEmail { get => string.IsNullOrEmpty(Email) ? Name : $"{Name} <{Email}>"; }

        #region from server given properties

        /// <summary>
        /// Chat Room unique id
        /// </summary>
        public Guid ChatRuid { get; set; }

        /// <summary>
        /// ChatRoom number of Chat Room on Sessiopn Server
        /// </summary>
        public string ChatRoomNr { get; set; }

        /// <summary>
        /// List of message indices, which user has already received
        /// </summary>
        public List<long> TicksLong { get; set; }

        /// <summary>
        /// Date, where user pushed last message to server
        /// </summary>
        public DateTime LastPushed { get; set; }

        /// <summary>
        /// DateTime, where user polled last time server
        /// </summary>
        public DateTime LastPolled { get; set; }

        #endregion from server given properties

        #endregion properties

        #region constructors

        /// <summary>
        /// Parameterless default constructor
        /// </summary>
        public CqrContact() : base()
        {
            ContactId = -1;
            Cuid = Guid.NewGuid();
            Name = string.Empty;
            Email = string.Empty;
            Mobile = string.Empty;
            Address = string.Empty;
            SecretKey = string.Empty;
            ContactImage = null;
            ChatRuid = Guid.Empty;
            ChatRoomNr = string.Empty;
            LastPolled = DateTime.MinValue;
            LastPushed = DateTime.MinValue;
            TicksLong = new List<long>();
        }

        public CqrContact(string cs, MsgEnum msgArt = MsgEnum.Json)
        {
            CqrContact cqrContactJson = FromJson<CqrContact>(cs);            
            ContactId = cqrContactJson.ContactId;
            Cuid = cqrContactJson.Cuid;
            Name = cqrContactJson.Name;
            Email = cqrContactJson.Email;
            Mobile = cqrContactJson.Mobile;
            Address = cqrContactJson.Address;
            SecretKey = cqrContactJson.SecretKey;
            ContactImage = cqrContactJson.ContactImage;

            ChatRuid = cqrContactJson.ChatRuid;
            ChatRoomNr = cqrContactJson.ChatRoomNr;
            TicksLong = cqrContactJson.TicksLong;
            LastPolled = cqrContactJson.LastPolled;
            LastPushed = cqrContactJson.LastPushed;

            _message = cqrContactJson._message;
            _hash = cqrContactJson._hash;
            RawMessage = cqrContactJson.RawMessage;
            MsgType = cqrContactJson.MsgType;
            Md5Hash = cqrContactJson.Md5Hash;
        }

        public CqrContact(int cid, string name, string email, string mobile, string address) : base()
        {
            ContactId = cid;
            Name = name;
            Email = email;
            Mobile = mobile;
            Address = address;
            SecretKey = "";
            ChatRuid = Guid.Empty;
            ChatRoomNr = "";
            LastPolled = DateTime.MinValue;
            LastPushed = DateTime.MinValue;
            ContactImage = null;
            TicksLong = new List<long>();
        }

        public CqrContact(Guid guid, string name, string email, string mobile, string address) : base()
        {
            Cuid = guid;
            Name = name;
            Email = email;
            Mobile = mobile;
            Address = address;
            SecretKey = "";
            ChatRuid = Guid.Empty;
            ChatRoomNr = "";
            LastPolled = DateTime.MinValue;
            LastPushed = DateTime.MinValue;
            ContactImage = null;
            TicksLong = new List<long>();
            // ClientIp = null;
        }

        public CqrContact(int cid, string name, string email, string mobile, string address, CqrImage cqrImage)
            : this(cid, name, email, mobile, address)
        {
            ContactImage = cqrImage;
        }

        public CqrContact(int cid, Guid cuid, string name, string email, string mobile, string address, CqrImage? cqrImage)
            : this(cid, name, email, mobile, address)
        {
            Cuid = cuid;
            ContactImage = cqrImage;
        }

        public CqrContact(int cid, Guid cuid, string name, string email, string mobile, string address, CqrImage? cqrImage, string hash)
            : this(cid, cuid, name, email, mobile, address, cqrImage)
        {
            _hash = hash;
        }

        public CqrContact(int cid, string name, string email, string mobile, string address, Image image)
            : this(cid, name, email, mobile, address)
        {
            ContactImage = CqrImage.FromDrawingImage(image);
        }

        public CqrContact(int cid, Guid cuid, string name, string email, string mobile, string address, Image image)
            : this(cid, name, email, mobile, address)
        {
            Cuid = cuid;
            ContactImage = CqrImage.FromDrawingImage(image);
        }

        public CqrContact(int cid, Guid cuid, string name, string email, string mobile, string address, Image image, string hash)
            : this(cid, cuid, name, email, mobile, address, image)
        {
            this._hash = hash;
        }

        public CqrContact(CqrContact c, string hash) : this(c.ContactId, c.Cuid, c.Name, c.Email, c.Mobile, c.Address, c.ContactImage, hash)
        {
            this._hash = hash;
            ChatRoomNr = c.ChatRoomNr;
            Cuid = (c.Cuid == Guid.Empty) ? Guid.NewGuid() : c.Cuid;
            ChatRuid = (c.ChatRuid == Guid.Empty) ? Guid.NewGuid() : c.ChatRuid;
            TicksLong = c.TicksLong ?? new List<long>();
            LastPolled = c.LastPolled;
            LastPushed = c.LastPushed;
            // ClientIp = c.ClientIp ?? null;
        }


        /// <summary>
        /// Constructor only for sending contact to service with null empty picture
        /// </summary>
        /// <param name="c"></param>
        /// <param name="chatRoomNr"></param>
        /// <param name="hash"></param>
        public CqrContact(CqrContact c, string chatRoomNr, string hash) : this(c, hash)
        {
            _hash = hash;
            ChatRoomNr = chatRoomNr;
            ContactImage = null;
            Cuid = (c.Cuid == Guid.Empty) ? Guid.NewGuid() : c.Cuid;
            ChatRuid = (c.ChatRuid == Guid.Empty) ? Guid.NewGuid() : c.ChatRuid;
            ChatRoomNr = c.ChatRoomNr;
            TicksLong = c.TicksLong ?? new List<long>();
            LastPolled = c.LastPolled;
            LastPushed = c.LastPushed;
            // ClientIp = c.ClientIp ?? null;
        }

        /// <summary>
        /// constuctor to update local contact on Windows Chat Client, picture will be set again
        /// </summary>
        /// <param name="c"></param>
        /// <param name="chatRoomNr"></param>
        /// <param name="hash"></param>
        /// <param name="contactImage"></param>
        public CqrContact(CqrContact c, string chatRoomNr, string hash, CqrImage? contactImage) : 
            this(c.Cuid, c.Name, c.Email, c.Mobile, c.Address)
        {
            _hash = hash;
            ChatRoomNr = chatRoomNr;
            if (contactImage != null)
                ContactImage = contactImage;

            ContactId = c.ContactId;
            Cuid = (c.Cuid == Guid.Empty) ? Guid.NewGuid() : c.Cuid;
            ChatRuid = (c.ChatRuid == Guid.Empty) ? Guid.NewGuid() : c.ChatRuid;
            ChatRoomNr = c.ChatRoomNr;
            TicksLong = c.TicksLong ?? new List<long>();
            LastPolled = c.LastPolled;
            LastPushed = c.LastPolled;
            // ClientIp = c.ClientIp ?? null;
        }

        #endregion constructors

        #region members

        public override string ToJson()
        {
            // CqrContact cqrContact = new CqrContact(ContactId, Cuid, Name, Email, Mobile, Address, ContactImage);
            string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            this.RawMessage = jsonString;
            return jsonString;
        }

        public override T FromJson<T>(string jsonText)
        {
            T tt = default(T);
            try
            {
                tt = JsonConvert.DeserializeObject<T>(jsonText);
                if (tt != null && tt is CqrContact cqrContactJson)
                {
                    if (cqrContactJson != null && cqrContactJson.ContactId > -1 && !string.IsNullOrEmpty(cqrContactJson.Name))
                    {
                        ContactId = cqrContactJson.ContactId;
                        Cuid = cqrContactJson.Cuid;
                        Name = cqrContactJson.Name;
                        Email = cqrContactJson.Email;
                        Mobile = cqrContactJson.Mobile;
                        Address = cqrContactJson.Address;
                        ContactImage = cqrContactJson.ContactImage;
                        SecretKey = cqrContactJson.SecretKey;

                        ChatRuid = cqrContactJson.ChatRuid;
                        ChatRoomNr = cqrContactJson.ChatRoomNr;
                        TicksLong = cqrContactJson.TicksLong;
                        LastPolled = cqrContactJson.LastPolled;
                        LastPushed = cqrContactJson.LastPushed;

                        _message = cqrContactJson._message;
                        _hash = cqrContactJson._hash;
                        RawMessage = cqrContactJson.RawMessage;
                        MsgType = cqrContactJson.MsgType;
                        Md5Hash = cqrContactJson.Md5Hash;

                        return (T)tt;
                    }
                }
            }
            catch (Exception exJson)
            {
                SLog.Log(exJson);
            }

            return (T)tt;

        }


        public override string ToXml() => this.ToXml();

        public override T FromXml<T>(string xmlText)
        {
            T cqrT = base.FromXml<T>(xmlText);
            if (cqrT is CqrContact cCnt)
            {
                ContactId = cCnt.ContactId;
                Cuid = cCnt.Cuid;
                Name = cCnt.Name;
                Email = cCnt.Email;
                Mobile = cCnt.Mobile;
                Address = cCnt.Address;
                ContactImage = cCnt.ContactImage;

                ChatRoomNr = cCnt.ChatRoomNr;
                ChatRuid = cCnt.ChatRuid;
                TicksLong = cCnt.TicksLong;
                LastPolled = cCnt.LastPolled;
                LastPushed = cCnt.LastPushed;

                _message = cCnt._message;
                _hash = cCnt._hash ?? string.Empty;
                RawMessage = cCnt.RawMessage;
                MsgType = cCnt.MsgType;
                Md5Hash = cCnt.Md5Hash;

            }

            return cqrT;

        }


        public override string ToString()
        {
            return
                "\"ContactId\": \t\"" + ContactId + "\";" + Environment.NewLine +
                "\"Cuid\": \t\"" + Cuid + "\";" + Environment.NewLine +
                "\"Name\": \t\"" + Name + "\";" + Environment.NewLine +
                "\"Email\": \t\"" + Email ?? "" + "\";" + Environment.NewLine +
                "\"Mobile\": \t\"" + Mobile ?? "" + "\";" + Environment.NewLine +
                "\"Address\": \t\"" + Address ?? "" + "\";" + Environment.NewLine +
                "\"NameEmail\": \t\"" + NameEmail ?? "" + "\";" + Environment.NewLine +
                ((ContactImage != null) ?
                    "\"ContactImage\": \t" + Environment.NewLine +
                    "\"ContactImage.ImageFileName\": \t\"" + ContactImage.ImageFileName + "\";" + Environment.NewLine +
                    "\"ContactImage.ImageMimeType\": \t\"" + ContactImage.ImageMimeType + "\";" + Environment.NewLine +
                    "\"ContactImage.ImageBase64\": \t\"" + ContactImage.ImageBase64 + "\";" + Environment.NewLine
                    : "")
                ;
        }


        /// <summary>
        /// <see cref="object[]">RowParams</see> gets an object array of row parameters to show in <see cref="DataGridView"/>
        /// </summary>
        public object[] GetRowParams()
        {
            List<object> oList = new List<object>();
            oList.Add(ContactId);
            oList.Add(Name);
            oList.Add(Email);
            oList.Add(Mobile);
            oList.Add(Address);
            return oList.ToArray();
        }


        #endregion members

    }

}
