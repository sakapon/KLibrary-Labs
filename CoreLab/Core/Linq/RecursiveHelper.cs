using System;
using System.Collections.Generic;
using System.Linq;

namespace KLibrary.Labs.Linq
{
    public static class RecursiveHelper
    {
        public static IEnumerable<TSource> EnumerateRecursively<TSource>(this TSource initialValue, Func<TSource, TSource> getNextItem)
        {
            if (getNextItem == null) throw new ArgumentNullException("getNextItem");

            var o = initialValue;
            while (true)
            {
                yield return o;
                o = getNextItem(o);
            }
        }

        public static IEnumerable<TSource> EnumerateRecursively<TSource>(this TSource initialValue, Func<TSource, IEnumerable<TSource>> getNextItems)
        {
            if (getNextItems == null) throw new ArgumentNullException("getNextItems");

            return EnumerateRecursively_Private(initialValue, getNextItems);
        }

        static IEnumerable<TSource> EnumerateRecursively_Private<TSource>(TSource initialValue, Func<TSource, IEnumerable<TSource>> getNextItems)
        {
            return initialValue.ToEnumerable()
                .Concat(getNextItems(initialValue).SelectMany(o => EnumerateRecursively_Private(o, getNextItems)));
        }

        public static IEnumerable<TSource> EnumerateRecursively2<TSource>(this TSource initialValue, Func<TSource, IEnumerable<TSource>> getNextItems)
        {
            if (getNextItems == null) throw new ArgumentNullException("getNextItems");

            return EnumerateRecursively2_Private(new[] { initialValue }, getNextItems);
        }

        static IEnumerable<TSource> EnumerateRecursively2_Private<TSource>(TSource[] initialValues, Func<TSource, IEnumerable<TSource>> getNextItems)
        {
            if (initialValues.Length == 0) return initialValues;

            // 上の階層から順に返します。
            return initialValues
                .Concat(EnumerateRecursively2_Private(initialValues.SelectMany(getNextItems).ToArray(), getNextItems));
        }
    }
}
