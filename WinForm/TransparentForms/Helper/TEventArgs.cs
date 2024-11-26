using System;

namespace Area23.At.WinForm.TransparentForms.Helper
{
    /// <summary>
    /// abstract class TBaseArgs
    /// </summary>
    public abstract class TBaseArgs : EventArgs
    {
        protected internal readonly object oDataPassed = null;

        /// <summary>
        /// <see cref="object"/>OData generic data, that will be passed, are here in base a down cast as object 
        /// </summary>
        public object OData => oDataPassed;

        public TBaseArgs() : base() { }

        public TBaseArgs(object oData)
        {
            oDataPassed = oData;
        }
    }

    /// <summary>
    /// TEventArgs generic event args
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TEventArgs<T> : TBaseArgs
    {
        protected internal readonly T tData;
        /// <summary>
        /// <typeparamref name="T"/> type save generic data
        /// </summary>
        public T Data => tData;


        /// <summary>
        /// Constructor for SpoolerEventArgs with T data
        /// </summary>
        /// <param name="data">T data</param>
        public TEventArgs(T data) : base(data)
        {
            tData = data;
        }


        public override string ToString()
        {
            return tData.ToString();
        }

    }

}
