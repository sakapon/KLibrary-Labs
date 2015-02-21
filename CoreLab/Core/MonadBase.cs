using System;
using System.Diagnostics;

namespace KLibrary.Labs
{
    [DebuggerDisplay("{Value}")]
    public abstract class MonadBase<T>
    {
        public T Value { get; protected set; }

        // Requirement for monad 1: Unit.
        protected MonadBase(T value)
        {
            Value = value;
        }

        public static explicit operator T(MonadBase<T> value)
        {
            return value.Value;
        }

        // Requirement for monad 2: Bind.
        public virtual MonadBase<TResult> Bind<TResult>(Func<T, MonadBase<TResult>> func)
        {
            return func(Value);
        }
    }
}
