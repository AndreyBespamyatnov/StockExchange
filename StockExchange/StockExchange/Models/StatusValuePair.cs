using System;
using StockExchange.Extensions;

namespace StockExchange.Models
{
    public sealed class StatusValuePair<T>
    {
        private readonly T _instance;
 
        public StatusValuePair(T instance, ErrorCode errorCode)
        {
            _instance = instance;
            ErrorCode = errorCode;
        }
         
        public T Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new NullReferenceException(string.Format("Value has not been set. Error code is: '{0}'", ErrorCode));
                }
 
                return _instance;
            }
        }
 
        public bool HasValue
        {
            get
            {
                return !_instance.IsDefault();
            }
        }
 
        public ErrorCode ErrorCode { get; private set; }
         
        public static implicit operator StatusValuePair<T>(T instance)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (instance == null)
            {
                throw new ArgumentException("Cannot instantiate StatusValuePair<T> with null.", "instance");
            }
            // ReSharper restore CompareNonConstrainedGenericWithNull
 
            return new StatusValuePair<T>(instance, ErrorCode.NoError);
        }
 
        /// <summary>
        /// TODO: consider usage of operators, especially this one
        /// </summary>
        public static implicit operator T(StatusValuePair<T> instance)
        {
            return instance.Value;
        }
    }
 
    public static class Extensions
    {
        public static T GetSafe<T>(this StatusValuePair<T> valuePair)
        {
            if (valuePair == null)
            {
                return default(T);
            }
 
            return !valuePair.HasValue ? default(T) : valuePair.Value;
        }
    }
}