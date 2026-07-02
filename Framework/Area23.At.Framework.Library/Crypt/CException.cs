using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Crypt
{

    /// <summary>
    /// CException a generic app level Exception derived from <see cref="ApplicationException"/>
    /// provides a <see cref="LastException"/> chain list to the root of first runtime exception
    /// <see cref="DateTime"/> <see cref="TimeStampException"/>, when the Exception was thrown
    /// <see cref="LastException"/> a property, that stores the Exception in <see cref="AppDomain.SetData(string, object?)"/> on set
    /// and getss the last Exception during starting this instance via <see cref="AppDomain.GetData(string)" />
    /// For root first Exception <see cref="LastException"/> is always <see cref="T:null"/>
    /// </summary>
    public class CException : ApplicationException
    {
        public static CException LastException
        {
            get => (CException)AppDomain.CurrentDomain.GetData(Constants.LAST_EXCEPTION);
            protected set => AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, value);
        }

        public CException Previous { get; protected set; }

        public DateTime TimeStampException { get; set; }



        public CException(string message) : base(message)
        {
            TimeStampException = DateTime.UtcNow;
            CException lastButNotLeast = LastException;
            Previous = lastButNotLeast != null ? lastButNotLeast : null;
            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);
        }

        public CException(string message, Exception innerException) : base(message, innerException)
        {
            TimeStampException = DateTime.UtcNow;
            CException lastButNotLeast = LastException;
            Previous = lastButNotLeast != null ? lastButNotLeast : null;
            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);
        }

        public static void SetLastException(Exception exc)
        {
            CException cqrLastEx = exc != null && exc is CException ? (CException)exc :
                exc != null && exc.InnerException != null ? new CException(exc.Message, exc.InnerException) :
                    exc != null && exc.Message != null ? new CException(exc.Message) : null;

            cqrLastEx.Source = exc.Source;
            cqrLastEx.HelpLink = exc.HelpLink;
            cqrLastEx.HResult = exc.HResult;
            cqrLastEx.Previous = LastException;

            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, cqrLastEx);
        }
    }

}
