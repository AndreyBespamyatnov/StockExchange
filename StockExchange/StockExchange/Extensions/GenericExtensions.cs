using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StockExchange.Extensions
{
    public static class GenericExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        /// <summary>
        /// Checks whatever <paramref name="o"/> has default value of it's type,
        /// indifferently of value or reference type it is. 
        /// </summary>
        public static bool IsDefault<T>(this T o)
        {
            return EqualityComparer<T>.Default.Equals(o, default(T));
        }

        public static void BreakOnTrue(this bool shouldBreak, string msg)
        {
            if (!shouldBreak)
                return;

            Conditional.DevExecute(() =>
            {
                Debug.WriteLine(msg);
                Debugger.Break();
            });
        }
    }

    public static class Conditional
    {
        [Conditional("DEBUG")]
        public static void DevExecute(this Action toExecute)
        {
#if DEBUG
            if (null != toExecute)
                toExecute.Invoke();
#endif
        }

    }
}