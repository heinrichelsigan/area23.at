using System;

namespace Area23.At.Framework.Library.Cache
{

    /// <summary>
    /// CacheEntry any cached value
    /// Use default default ctor <see cref="CacheEntry{T}.CacheEntry(T)"/>
    /// or <see cref="SetValue(T)"/> to set the cached value;    
    /// Use <see cref="GetValue()"/> or <see cref="GetNullableValue()"/> to get the cached value
    /// </summary>
    [Serializable]
    public class CacheEntry<T> where T : struct
    {

        #region properties

        public T TValue { get; protected internal set; }

        #endregion properties

        /// <summary>
        /// Empty default ctor
        /// </summary>
        public CacheEntry()
        {            
            TValue = default(T);    
        }

        /// <summary>
        /// default ctor <see cref="CacheEntry{T}.CacheEntry(T)"/>
        /// and then <see cref="SetValue{T}(T)"/> to set a cached value instead.
        /// </summary>
        /// <param name="ovalue"><see cref="{T}" /> ovalue</param>
        public CacheEntry(T tValue)
        {
            TValue = tValue;
        }

        /// <summary>
        /// gets the <see cref="Type"/> of generic cached value
        /// </summary>
        /// <returns><see cref="Type"/> of generic value or null if cached value is <see cref="null"/></returns>
        public new Type GetType() => TValue.GetType();

        /// <summary>
        /// Get a value from cache
        /// </summary>
        /// <returns>generic T value</returns>
        internal T GetValue() => (TValue.Equals(default(T))) ? default : TValue;

        /// <summary>
        /// Get a nullable value from cache
        /// </summary>
        /// <returns><see cref="Nullable{T}">Nullable{T} now T?</see></returns>
        public Nullable<T> GetNullableValue()
        {
            Nullable<T> tNull = null;
            if (!TValue.Equals(default(T))) 
                tNull = new Nullable<T>((T)TValue);
            
            return tNull;
        }

        /// <summary>
        /// Sets a generic cached value
        /// </summary>
        /// <param name="tValue">generic value to set cached</param>
        public void SetValue(T tValue) => TValue = tValue;


        /// <summary>
        /// override ToString() returns <see cref="_Value"/>
        /// </summary>
        /// <returns>returns <see cref="_Value"/></returns>
        public override string ToString() => (TValue.Equals(default(T))) ? null : TValue.ToString();
        
    
    }

}
