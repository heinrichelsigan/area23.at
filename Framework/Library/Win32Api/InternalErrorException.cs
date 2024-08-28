namespace Area23.At.Framework.Library.Win32Api
{

    /// <summary>
    /// InternalError Exception derived from <see cref="System.ComponentModel.Win32Exception"/>
    /// </summary>
    public class InternalErrorException : System.ComponentModel.Win32Exception
    {

        /// <summary>
        /// InternalErrorException parameterless constructor
        /// </summary>
        public InternalErrorException() : base() { }

        /// <summary>
        /// InternalErrorException constructor with simple msg
        /// </summary>
        /// <param name="msg"><see cref="string">string msg</see> a message to describe the <see cref="EnablerSpoolerException"/></param>
        public InternalErrorException(string msg) : base(msg)
        {
        }

        /// <summary>
        /// InternalErrorException constructor with simple msg and innerException
        /// </summary>
        /// <param name="msg"><see cref="string">string msg</see> a message to describe the <see cref="EnablerSpoolerException"/></param>
        /// <param name="innerEx"><see cref="Exception">Exception innerEx</see> inner Exception, that was previously thrown</param>        
        public InternalErrorException(string msg, System.Exception innerEx) : base(msg, innerEx)
        {
        }

    }

}