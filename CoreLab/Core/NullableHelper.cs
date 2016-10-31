using System;

namespace KLibrary.Labs
{
    /// <summary>
    /// Provides a set of methods for nullable objects (class or <see cref="Nullable{T}"/>).
    /// </summary>
    public static class NullableHelper
    {
        /// <summary>
        /// Does an action for an object, if the object is not null.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">An object.</param>
        /// <param name="action">An action.</param>
        /// <returns>The original object.</returns>
        public static T IfNotNull<T>(this T obj, Action<T> action)
            where T : class
        {
            if (obj != null) action(obj);
            return obj;
        }

        /// <summary>
        /// Does an action for an object, if the object is not null.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">An object.</param>
        /// <param name="action">An action.</param>
        /// <returns>The original object.</returns>
        public static T? IfNotNull<T>(this T? obj, Action<T> action)
            where T : struct
        {
            if (obj.HasValue) action(obj.Value);
            return obj;
        }

        public static TResult IfNotNull<T, TResult>(this T obj, Func<T, TResult> func)
            where T : class
        {
            return obj != null ? func(obj) : default(TResult);
        }

        public static TResult IfNotNull<T, TResult>(this T? obj, Func<T, TResult> func)
            where T : struct
        {
            return obj.HasValue ? func(obj.Value) : default(TResult);
        }

        public static TResult? IfNotNull2<T, TResult>(this T obj, Func<T, TResult> func)
            where T : class
            where TResult : struct
        {
            return obj != null ? func(obj) : default(TResult?);
        }

        public static TResult? IfNotNull2<T, TResult>(this T? obj, Func<T, TResult> func)
            where T : struct
            where TResult : struct
        {
            return obj.HasValue ? func(obj.Value) : default(TResult?);
        }
    }
}
