using Area23.At.Framework.Core.Static;
using Newtonsoft.Json;
using System.Net;
using System.Security.Policy;

namespace Area23.At.Framework.Core.CqrXs.CqrMsg
{


    /// <summary>
    /// CqrChatRoom is a contact for CqrJd
    /// </summary>
    [Serializable]
    public class CqrChatRoom : MsgContent, ICqrMessagable
    {

        #region from server given properties

        public Guid ChatRuid { get; set; }

        public string ChatRoomId { get; set; }

        public List<long> TicksLong { get; set; }

        public DateTime LastPushed { get; set; }

        public DateTime LastPolled { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        internal IPAddress? ClientIp { get; set; }

        #endregion from server given properties

        public CqrChatRoom() : base()
        {
            ChatRuid = Guid.NewGuid();
            ChatRoomId = "";
            TicksLong = new List<long>();
            LastPushed = DateTime.MinValue;
            LastPolled = DateTime.MinValue;
            _message = "";
            _hash = "";
            RawMessage = "";
            Md5Hash = "";
        }


        #region members

        public override string ToJson()
        {
            // CqrContact cqrContact = new CqrContact(ContactId, Cuid, Name, Email, Mobile, Address, ContactImage);
            string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
            this.RawMessage = jsonString;
            return jsonString;
        }

        public override T? FromJson<T>(string jsonText) where T : default
        {
            T? tt = default(T);
            try
            {
                tt = JsonConvert.DeserializeObject<T>(jsonText);
                if (tt != null && tt is CqrChatRoom chatRoom)
                {
                    if (chatRoom != null && chatRoom.ChatRuid != Guid.Empty && !string.IsNullOrEmpty(chatRoom.ChatRoomId))
                    {
                        ChatRuid = chatRoom.ChatRuid;
                        ChatRoomId = chatRoom.ChatRoomId;
                        TicksLong = chatRoom.TicksLong;
                        LastPushed = chatRoom.LastPushed;
                        LastPolled = chatRoom.LastPolled;
                        _message = chatRoom._message;
                        _hash = chatRoom._hash;
                        RawMessage = chatRoom.RawMessage;
                        Md5Hash = chatRoom.Md5Hash;

                        return (T?)tt;
                    }
                }
            }
            catch (Exception exJson)
            {
                SLog.Log(exJson);
            }

            return (T?)tt;

        }

        public override string ToXml() => this.ToXml();

        public override T? FromXml<T>(string xmlText) where T : default
        {
            T? cqrT = base.FromXml<T>(xmlText);
            if (cqrT is CqrChatRoom chatRoom && chatRoom != null && chatRoom.ChatRuid != Guid.Empty && !string.IsNullOrEmpty(chatRoom.ChatRoomId))
            {
                ChatRuid = chatRoom.ChatRuid;
                ChatRoomId = chatRoom.ChatRoomId;
                TicksLong = chatRoom.TicksLong;
                LastPushed = chatRoom.LastPushed;
                LastPolled = chatRoom.LastPolled;
                _message = chatRoom._message;
                _hash = chatRoom._hash;
                RawMessage = chatRoom.RawMessage;
                Md5Hash = chatRoom.Md5Hash;

                return (T?)cqrT;
            }

            return cqrT;

        }


        public override string ToString()
        {

            return
                "\"ChatRuid\": \t\"" + ChatRuid + "\";" + Environment.NewLine +
                "\"ChatRoomId\": \t\"" + ChatRoomId + "\";" + Environment.NewLine +
                "\"TicksLong\": \t\"" + String.Join(",", TicksLong) + "\";" + Environment.NewLine +
                "\"LastPushed\": \t\"" + LastPushed ?? "" + "\";" + Environment.NewLine +
                "\"LastPolled\": \t\"" + LastPolled ?? "" + "\";" + Environment.NewLine +
                "\"_message\": \t\"" + _message ?? "" + "\";" + Environment.NewLine +
                "\"_hash\": \t\"" + _hash ?? "" + "\";" + Environment.NewLine +
                "\"RawMessage\": \t\"" + RawMessage ?? "" + "\";" + Environment.NewLine +
                "\"Md5Hash\": \t\"" + Md5Hash ?? "" + "\";" + Environment.NewLine;
        }

        #endregion members

    }


}
