using Area23.At.Framework.Core.Static;

namespace Area23.At.Framework.Core.Cqr
{
    public class CqrException : ApplicationException
    {
        public static CqrException LastException
        {
            get => (CqrException)AppDomain.CurrentDomain.GetData(Constants.LAST_EXCEPTION);
            protected set => AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, value);
        }

        public CqrException Previous { get; protected set; }

        public DateTime TimeStampException { get; set; }



        public CqrException(string message) : base(message)
        {
            TimeStampException = DateTime.UtcNow;
            CqrException lastButNotLeast = (CqrException)LastException;
            Previous = (lastButNotLeast != null) ? (CqrException)lastButNotLeast : null;
            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);
        }

        public CqrException(string message, Exception innerException) : base(message, innerException)
        {
            TimeStampException = DateTime.UtcNow;
            CqrException lastButNotLeast = (CqrException)LastException;
            Previous = (lastButNotLeast != null) ? lastButNotLeast : null;
            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, this);
        }

        public static void SetLastException(Exception exc)
        {
            CqrException cqrLastEx = (exc != null && exc is CqrException) ? (CqrException)exc :
                ((exc != null && exc.InnerException != null) ? new CqrException(exc.Message, exc.InnerException) :
                    ((exc != null && exc.Message != null) ? new CqrException(exc.Message) : null));

            cqrLastEx.Source = exc.Source;
            cqrLastEx.HelpLink = exc.HelpLink;
            cqrLastEx.HResult = exc.HResult;
            cqrLastEx.Previous = (CqrException)LastException;

            AppDomain.CurrentDomain.SetData(Constants.LAST_EXCEPTION, cqrLastEx);
        }
    }

}
