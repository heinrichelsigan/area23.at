using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.CqrXs
{
    /// <summary>
    /// CqrException is inherited from <see cref="ApplicationException"/>
    /// </summary>
    public class CqrException : ApplicationException
    {
        public static CqrException LastException
        {
            get => (CqrException)AppDomain.CurrentDomain.GetData(Constants.LAST_EXCEPTION);
            protected set => AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, value);
        }

        public CqrException? Previous { get; protected set; }

        public DateTime? TimeStampException { get; set; }



        public CqrException(string message) : base(message)
        {
            TimeStampException = DateTime.UtcNow;
            CqrException? lastButNotLeast = null;
            try
            {
                lastButNotLeast = (CqrException)LastException;
            }
            catch { }
            
            Previous = (lastButNotLeast != null) ? (CqrException)lastButNotLeast : null;
            
            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);
        }

        public CqrException(string message, Exception innerException) : base(message, innerException)
        {
            TimeStampException = DateTime.UtcNow;
            CqrException? lastButNotLeast = null;
            try
            {
                if (AppDomain.CurrentDomain.GetData(Constants.LAST_EXCEPTION) != null)
                    lastButNotLeast = (CqrException)LastException;
            }
            catch { }

            Previous = (lastButNotLeast != null) ? lastButNotLeast : null;

            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);
        }

        public static Exception SetLastException(Exception? exc, bool logException = true)
        {
            CqrException cqrLastEx = (exc != null && exc is CqrException) ? (CqrException)exc :
                ((exc != null && exc.Message != null && exc.InnerException != null) ? new CqrException(exc.Message, exc.InnerException) :
                    ((exc != null && exc.Message != null) ? new CqrException(exc.Message) : new CqrException(Constants.UNKNOWN)));

            if (exc != null) 
            {
                cqrLastEx.Source = exc.Source;
                cqrLastEx.HelpLink = exc.HelpLink;
                cqrLastEx.HResult = exc.HResult;
                cqrLastEx.Previous = (CqrException)LastException;
            }

            if (logException) 
                Area23Log.LogStatic(exc ?? cqrLastEx);

            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, exc ?? cqrLastEx);

            return exc ?? cqrLastEx;
        }
    }
}
