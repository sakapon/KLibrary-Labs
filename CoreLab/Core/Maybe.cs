using System;

namespace KLibrary.Labs
{
    /// <summary>
    /// Represents a value type that can be assigned the unset state.
    /// </summary>
    /// <typeparam name="T">The underlying value type.</typeparam>
    public struct Maybe<T>
    {
        /// <summary>
        /// Represents the unset state.
        /// </summary>
        public static readonly Maybe<T> None = new Maybe<T>();

        public T Value { get; private set; }
        public bool HasValue { get; private set; }

        public Maybe(T value)
            : this()
        {
            Value = value;
            HasValue = value != null;
        }

        public static explicit operator T(Maybe<T> value)
        {
            return value.Value;
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public static bool operator ==(Maybe<T> value1, Maybe<T> value2)
        {
            return value1.HasValue
                ? (value2.HasValue && object.Equals(value1.Value, value2.Value))
                : !value2.HasValue;
        }

        public static bool operator !=(Maybe<T> value1, Maybe<T> value2)
        {
            return !(value1 == value2);
        }

        public override bool Equals(object obj)
        {
            return obj is Maybe<T> && this == (Maybe<T>)obj;
        }

        public override int GetHashCode()
        {
            return HasValue
                ? Value.GetHashCode()
                : 0;
        }

        public override string ToString()
        {
            return HasValue
                ? Value.ToString()
                : "{None}";
        }

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func)
        {
            return HasValue
                ? func(Value)
                : Maybe<TResult>.None;
        }
    }

    public static class Maybe
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return value;
        }

        public static Maybe<T> Do<T>(this Maybe<T> maybe, Action<T> action)
        {
            if (maybe.HasValue) action((T)maybe);
            return maybe;
        }

        public static Maybe<TResult> Select<T, TResult>(this Maybe<T> maybe, Func<T, TResult> selector)
        {
            return maybe.HasValue
                ? selector((T)maybe)
                : Maybe<TResult>.None;
        }

        public static Maybe<TResult> SelectMany<T, TResult>(this Maybe<T> maybe, Func<T, Maybe<TResult>> selector)
        {
            return maybe.Bind(selector);
        }

        public static Maybe<TResult> SelectMany<T, U, TResult>(this Maybe<T> maybe, Func<T, Maybe<U>> selector, Func<T, U, TResult> resultSelector)
        {
            var selected = maybe.Bind(selector);
            return selected.HasValue
                ? resultSelector((T)maybe, (U)selected)
                : Maybe<TResult>.None;
        }

        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
        {
            return maybe.HasValue && predicate((T)maybe)
                ? maybe
                : Maybe<T>.None;
        }

        public static Maybe<TResult> Combine<T1, T2, TResult>(this Maybe<T1> maybe1, Maybe<T2> maybe2, Func<T1, T2, TResult> resultSelector)
        {
            return maybe1.HasValue && maybe2.HasValue
                ? resultSelector((T1)maybe1, (T2)maybe2)
                : Maybe<TResult>.None;
        }
    }
}
