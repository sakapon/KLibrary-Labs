using System;

namespace KLibrary.Labs.Pipeline
{
    public static class ExceptionHelper
    {
        public static Action ToAction(this Exception ex)
        {
            return () => { throw ex; };
        }

        public static Action<T> ToAction<T>(this Exception ex)
        {
            return o => { throw ex; };
        }

        public static Func<TResult> ToFunc<TResult>(this Exception ex)
        {
            return () => { throw ex; };
        }

        public static Func<T, TResult> ToFunc<T, TResult>(this Exception ex)
        {
            return o => { throw ex; };
        }
    }
}
