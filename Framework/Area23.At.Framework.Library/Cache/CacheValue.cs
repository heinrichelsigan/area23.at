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
        public object _Value { get; protected internal set; }
        public Type _Type { get; protected internal set; }
        #endregion properties

        /// <summary>
        /// Empty default ctor
        /// </summary>
        public CacheValue()
        {
            _Type = null;
            _Value = null;
        }

        /// <summary>
        /// Obsolete ctor, please use default empty ctor <see cref="CacheValue()"/> 
        /// and then <see cref="SetValue{T}(T)"/> to set a cached value instead.
        /// </summary>
        /// <param name="ovalue"><see cref="object" /> ovalue</param>
        /// <param name="atype"><see cref="Type"/> atype</param>
        [Obsolete("Don't use ctor CacheValue(object, Type) to set a cache value, use SetValue<T>(T tvalue) instead.", false)]
        public CacheValue(object ovalue, Type atype)
        {
            _Type = atype;
            _Value = ovalue;
        }

        /// <summary>
        /// gets the <see cref="Type"/> of generic cached value
        /// </summary>
        /// <returns><see cref="Type"/> of generic value or null if cached value is <see cref="null"/></returns>
        public new Type GetType()
        {
            return _Type;
        }

        /// <summary>
        /// Get a value from cache
        /// </summary>
        /// <typeparam name="T">generic type of value passed by typeparameter</typeparam>
        /// <returns>generic T value</returns>
        /// <exception cref="InvalidOperationException">thrown, when cached value isn't of typeof(T)</exception>
        internal T GetValue<T>()
        {
            if (_Type != null && _Value != null && typeof(T) == _Type)
                return (T)_Value;
            else
                return default(T);
        }

        /// <summary>
        /// Get a nullable value from cache
        /// </summary>
        /// <typeparam name="T">generic type of value passed by type parameter</typeparam>
        /// <returns><see cref="Nullable{T}">Nullable{T} now T?</see></returns>
        /// <exception cref="InvalidOperationException">thrown, when cached value isn't of typeof(T)</exception>
        public Nullable<T> GetNullableValue<T>() where T : struct 
        {            
            Nullable<T> tNullValue = null;

            if (_Type == null || _Value == null)
                tNullValue = null;
            else if (typeof(T) == _Type)
                tNullValue = new Nullable<T>((T)_Value);
            else
                throw new InvalidOperationException($"typeof(T) = {typeof(T)} while _type = {_Type}");

            return tNullValue;                
        }

        /// <summary>
        /// Sets a generic cached value
        /// </summary>
        /// <typeparam name="T">generic type of value passed by typeparameter</typeparam>
        /// <param name="tvalue">generic value to set cached</param>
        public void SetValue<T>(T tvalue)
        {
            _Type = typeof(T);
            _Value = (object)tvalue;
        }

        /// <summary>
        /// override ToString() returns <see cref="_Value"/>
        /// </summary>
        /// <returns>returns <see cref="_Value"/></returns>
        public override string ToString()
        {
            return (_Value == null) ? null : _Value.ToString();
        }
    
    }

}
