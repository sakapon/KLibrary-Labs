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
            yield return initialValue;

            foreach (var child in getNextItems(initialValue))
            {
                foreach (var o in EnumerateRecursively_Private(child, getNextItems))
                {
                    yield return o;
                }
            }
        }
    }
}
