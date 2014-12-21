using System;

namespace KLibrary.Labs.Pipeline
{
    public static class FuncHelper
    {
        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> f1, Func<T2, T3> f2)
        {
            return x => f2(f1(x));
        }
    }
}
