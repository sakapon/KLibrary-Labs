using System;

namespace KLibrary.Labs.Pipeline
{
    public static class FuncHelper
    {
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> f1, Func<T2, T3> f2)
        {
            return p1 => f2(f1(p1));
        }

        // カリー化 
        public static Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(this Func<T1, T2, TResult> func)
        {
            return p1 => p2 => func(p1, p2);
        }

        public static Func<T1, Func<T2, T3, TResult>> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func)
        {
            return p1 => (p2, p3) => func(p1, p2, p3);
        }

        // 非カリー化 
        public static Func<T1, T2, TResult> Uncurry<T1, T2, TResult>(this Func<T1, Func<T2, TResult>> func)
        {
            return (p1, p2) => func(p1)(p2);
        }

        public static Func<T1, T2, T3, TResult> Uncurry<T1, T2, T3, TResult>(this Func<T1, Func<T2, T3, TResult>> func)
        {
            return (p1, p2, p3) => func(p1)(p2, p3);
        }

        // 部分適用 
        public static Func<T2, TResult> Partial<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 arg1)
        {
            return func.Curry()(arg1);
        }

        public static Func<T2, T3, TResult> Partial<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 arg1)
        {
            return func.Curry()(arg1);
        }
    }
}
