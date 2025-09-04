using System;

namespace Area23.At.Framework.Library.Cache
{

    /// <summary>
    /// CacheValue any cached value.
    /// Use default empty ctor <see cref="CacheValue()"/> and
    /// <see cref="SetValue{T}(T)"/> to set the cached value;
    /// setting cache value via <see cref="CacheValue(object, Type)"/> ctor is obsolete.
    /// Use <see cref="GetValue{T}"/> to get the cached value
    /// </summary>
    [Serializable]
    public class CacheValue 
    {

        #region properties
        public object CValue { get; protected internal set; }
        public Type CType { get; protected internal set; }
        #endregion properties

        /// <summary>
        /// Empty default ctor
        /// </summary>
        public CacheValue()
        {
            CType = null;
            CValue = null;
        }

        /// <summary>
        /// Obsolete ctor, please use default empty ctor <see cref="CacheValue()"/> 
        /// and then <see cref="SetValue{T}(T)"/> to set a cached value instead.
        /// </summary>
        /// <param name="oValue"><see cref="object" /> oValue</param>
        /// <param name="aType"><see cref="Type"/> atype</param>
        [Obsolete("Don't use ctor CacheValue(object, Type) to set a cache value, use SetValue<T>(T tvalue) instead.", false)]
        public CacheValue(object oValue, Type aType)
        {
            CType = aType;
            CValue = oValue;
        }

        /// <summary>
        /// gets the <see cref="Type"/> of generic cached value
        /// </summary>
        /// <returns><see cref="Type"/> of generic value or null if cached value is <see cref="null"/></returns>
        public new Type GetType() => CType;

        /// <summary>
        /// Get a value from cache
        /// </summary>
        /// <typeparam name="T">generic type of value passed by typeparameter</typeparam>
        /// <returns>generic T value</returns>        
        internal T GetValue<T>() => (CType != null && CValue != null && typeof(T) == CType) ? (T)CValue : default;

        /// <summary>
        /// Get a nullable value from cache
        /// </summary>
        /// <typeparam name="T">generic type of value passed by type parameter</typeparam>
        /// <returns><see cref="Nullable{T}">Nullable{T} now T?</see></returns>
        /// <exception cref="InvalidOperationException">thrown, when cached value isn't of typeof(T)</exception>
        public Nullable<T> GetNullableValue<T>() where T : struct 
        {                                    
            Nullable<T> tNullValue = null;
            // return (_Type != null && _Value != null && typeof(T) == _Type) ? new Nullable<T>((T)_Value) : null;

            if (CType != null && CValue != null)
            {
                if (typeof(T) == CType)
                    tNullValue = new Nullable<T>((T)CValue);                
                else
                    throw new InvalidOperationException($"typeof(T) = {typeof(T)} while _type = {CType}");
            }

            return tNullValue;                
        }

        /// <summary>
        /// Sets a generic cached value
        /// </summary>
        /// <typeparam name="T">generic type of value passed by typeparameter</typeparam>
        /// <param name="tValue">generic value to set cached</param>
        public void SetValue<T>(T tValue)
        {
            CType = typeof(T);
            CValue = (object)tValue;
        }

        /// <summary>
        /// override ToString() returns <see cref="CValue"/>
        /// </summary>
        /// <returns>returns <see cref="CValue"/></returns>
        public override string ToString() => (CValue == null) ? null : CValue.ToString();
    
    }

}
