using System;

namespace Area23.At.Framework.Core.Util
{
    /// <summary>
    /// Area23EventArgs generic event args
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class Area23EventArgs<T> : EventArgs
    {
        
        /// <summary>
        /// <typeparamref name="T">geneneric T type or class</typeparamref>
        /// </summary>
        public T GenericTData { get; protected set; }


        /// <summary>
        /// ctor Area23EventArgs
        /// </summary>
        /// <param name="genericTData">generic T data</param>
        public Area23EventArgs(T genericTData)
        {
            GenericTData = genericTData;
        }


        public override string? ToString()
        {
            return (GenericTData != null) ? GenericTData.ToString() : null;
        }

    }
}
