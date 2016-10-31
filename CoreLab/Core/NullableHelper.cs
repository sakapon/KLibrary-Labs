using System;

namespace KLibrary.Labs
{
    public static class NullableHelper
    {
        public static T IfNotNull<T>(this T obj, Action<T> action)
            where T : class
        {
            if (obj != null) action(obj);
            return obj;
        }

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
