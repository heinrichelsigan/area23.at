using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.CqrXs.CqrMsg
{

    /// <summary>
    /// Full SrvMsg
    /// </summary>
    /// <typeparam name="TC"></typeparam>
    [JsonObject]
    [Serializable]
    public class FullSrvMsg<TC> : MsgContent where TC : class
    {
        #region properties

        public CqrContact Sender { get; set; }

        public List<CqrContact> Recipients { get; set; }

        public CqrContact Recipient {
            get => Recipients[0];
            set
            {
                if (Recipients == null || Recipients.Count == 0)
                {
                    Recipients = new List<CqrContact>();
                    Recipients.Add(value);
                }
                else
                { Recipients[0] = value; }
            }
        }

        public TC TContent { get; set; }

        #endregion properties

        #region ctor

        public FullSrvMsg() : base()
        {
            _message = string.Empty;
            _rawMessage = string.Empty;
            _hash = string.Empty;
            Sender = null;
            Recipients = new List<CqrContact>();
            Recipient = null;
            TContent = null;
        }

        public FullSrvMsg(string fm, MsgEnum msgArt = MsgEnum.JsonSerialized) : base()
        {
            this.FromJson<FullSrvMsg<TC>>(fm);
        }

        [Obsolete("Always user FullSrvMsg(CqrContact sender, CqrContact to, TC tc, string hash) : base() ctor", false)]
        public FullSrvMsg(CqrContact sender, CqrContact to, TC tc) : base()
        {
            Sender = sender;
            Recipient = to;
            TContent = tc;
        }

        /// <summary>
        /// Please always use this constuctor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="to"></param>
        /// <param name="tc"></param>
        /// <param name="hash"></param>
        public FullSrvMsg(CqrContact sender, CqrContact to, TC tc, string hash) : base()
        {
            Sender = sender;
            Recipient = to;
            TContent = tc;
            _hash = hash;
        }

        #endregion ctor


        public override string ToJson()
        {
            string jsonText = JsonConvert.SerializeObject(this);
            this._rawMessage = jsonText;
            return jsonText;
        }

        public new FullSrvMsg<TC> FromJson(string jsonText)
        {
            FullSrvMsg<TC> tc = JsonConvert.DeserializeObject<FullSrvMsg<TC>>(jsonText);
            try
            {
                if (tc != null && tc is FullSrvMsg<TC> fullSrvMsg)
                {
                    if (fullSrvMsg != null && !string.IsNullOrEmpty(fullSrvMsg?.Message))
                    {
                        Sender = fullSrvMsg.Sender;
                        // Recipient = fullSrvMsg.Recipient;
                        Recipients = fullSrvMsg.Recipients;
                        TContent = fullSrvMsg.TContent;
                    }
                    return tc;
                }
            }
            catch (Exception exJson)
            {
                Area23Log.LogStatic(exJson);
            }

            return default(FullSrvMsg<TC>);
        }

    }

}
