using System;

namespace KLibrary.Labs.Pipeline
{
    public static class TupleHelper
    {
        public static Tuple<T1> DoAction<T1>(this Tuple<T1> tuple, Action<T1> action)
        {
            if (action != null) action(tuple.Item1);
            return tuple;
        }

        public static Tuple<T1, T2> DoAction<T1, T2>(this Tuple<T1, T2> tuple, Action<T1, T2> action)
        {
            if (action != null) action(tuple.Item1, tuple.Item2);
            return tuple;
        }

        public static TResult DoFunc<T1, TResult>(this Tuple<T1> tuple, Func<T1, TResult> func)
        {
            if (func == null) throw new ArgumentNullException("func");

            return func(tuple.Item1);
        }

        public static TResult DoFunc<T1, T2, TResult>(this Tuple<T1, T2> tuple, Func<T1, T2, TResult> func)
        {
            if (func == null) throw new ArgumentNullException("func");

            return func(tuple.Item1, tuple.Item2);
        }
    }
}
