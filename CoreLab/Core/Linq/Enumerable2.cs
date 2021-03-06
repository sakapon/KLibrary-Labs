﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace KLibrary.Labs.Linq
{
    public static class Enumerable2
    {
        public static IEnumerable<TSource> ToEnumerable<TSource>(this TSource obj)
        {
            yield return obj;
        }

        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }

        public static void Execute<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            foreach (var item in source)
            {
            }
        }

        public static void Execute<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in source)
            {
                action(item);
            }
        }

        public static Maybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            foreach (var item in source)
            {
                return item;
            }
            return Maybe<TSource>.None;
        }

        public static Maybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");

            foreach (var item in source)
            {
                if (predicate(item)) return item;
            }
            return Maybe<TSource>.None;
        }

        public static Maybe<TSource> LastMaybe<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return source.Reverse().FirstMaybe();
        }

        public static Maybe<TSource> LastMaybe<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");

            return source.Reverse().FirstMaybe(predicate);
        }
    }
}
