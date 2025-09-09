using Area23.At.Framework.Library.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Area23.At.Framework.Library.Cqr.Msg
{

    [Serializable]
    public class CChatRoom : CContent, IMsgAble
    {
        public Guid ChatRuid { get; set; }

        public string ChatRoomNr { get; set; }

        [JsonIgnore]
        public List<long> TicksLong { get => MsgDict.Keys.ToList(); }

        public DateTime LastPushed { get; set; }

        public DateTime LastPolled { get; set; }

        public List<string> InvitedEmails { get; set; }

        public Dictionary<long, string> MsgDict { get; set; }   


        #region ctor

        public CChatRoom() : base()
        {
            ChatRuid = Guid.NewGuid();
            InvitedEmails = new List<string>();
            ChatRoomNr = "";
            Message = "";
            MsgDict = new Dictionary<long, string>();
            LastPushed = DateTime.MinValue;
            LastPolled = DateTime.MinValue;
            Hash = "";
            Md5Hash = "";
            MsgType = SerType.None;
            CBytes = new byte[0];
        }

        public CChatRoom(string chatRoomNr) : this()
        {
            ChatRoomNr = chatRoomNr;
            ChatRuid = Guid.NewGuid();
            LastPushed = DateTime.MinValue;
            LastPolled = DateTime.MinValue;
            MsgDict = new Dictionary<long, string>();
        }

        public CChatRoom(string chatRoomNr, Guid chatRuid, DateTime lastPushed, DateTime lastPolled) : this()
        {
            ChatRoomNr = chatRoomNr;
            ChatRuid = (chatRuid == Guid.Empty) ? Guid.NewGuid() : chatRuid;
            LastPushed = lastPushed;
            LastPolled = lastPolled;
            MsgDict = new Dictionary<long, string>();
        }

        public CChatRoom(string chatRoomNr, Guid chatRuid, DateTime lastPushed, DateTime lastPolled, Dictionary<long, string> msgDict, List<string> invited, string hash, string md5sum, byte[] bytes) : this()
        {
            ChatRoomNr = chatRoomNr;
            ChatRuid = (chatRuid == Guid.Empty) ? Guid.NewGuid() : chatRuid;
            LastPushed = lastPushed;
            LastPolled = lastPolled;
            MsgDict = msgDict;
            InvitedEmails = new List<string>(invited);
            Hash = hash;
            Md5Hash = md5sum;
            CBytes = bytes;
            MsgType = SerType.Json;
        }

        public CChatRoom(CChatRoom chatRoom) : this()
        {
            if (chatRoom != null)
            {
                CChatRoom.CloneCopy(chatRoom, this);
            }
        }

        #endregion ctor

        public override CContent CCopy(CContent leftDest, CContent rightSrc)
        {
            if (leftDest is CChatRoom && rightSrc is CChatRoom)
                return CChatRoom.CloneCopy(rightSrc, leftDest);

            return base.CCopy(leftDest, rightSrc);
        }


        #region members

        public override string ToXml() => Utils.SerializeToXml<CChatRoom>(this);

        #endregion members

        #region static members

        public static CChatRoom CloneCopy(CChatRoom source, CChatRoom destination)
        {
            if (source == null)
                return null;
            if (destination == null)
                destination = new CChatRoom();

            destination.Message = source.Message;
            destination.Hash = source.Hash;
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;

            destination.ChatRoomNr = source.ChatRoomNr;
            destination.ChatRuid = source.ChatRuid;
            destination.MsgDict = source.MsgDict;
            // destination.TicksLong = source.TicksLong;
            destination.LastPolled = source.LastPolled;
            destination.LastPushed = source.LastPushed;
            destination.InvitedEmails = source.InvitedEmails;

            return destination;
        }

        #endregion static members

    }

}
