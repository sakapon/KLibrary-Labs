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

        T _value;

        public T Value
        {
            get
            {
                if (!HasValue) throw new InvalidOperationException();
                return _value;
            }
        }

        public bool HasValue { get; private set; }

        public Maybe(T value)
            : this()
        {
            _value = value;
            HasValue = true;
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
            throw new NotImplementedException();
        }

        public static bool operator !=(Maybe<T> value1, Maybe<T> value2)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func)
        {
            return HasValue
                ? func(_value)
                : Maybe<TResult>.None;
        }

        public override string ToString()
        {
            return HasValue
                ? _value.ToString()
                : "{None}";
        }
    }

    public static class Maybe
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return value;
        }

        public static Maybe<T> ToMaybeWithoutDefault<T>(this T value)
        {
            return ToMaybeWithoutDefault(value, default(T));
        }

        public static Maybe<T> ToMaybeWithoutDefault<T>(this T value, T defaultValue)
        {
            return !object.Equals(value, defaultValue)
                ? value
                : Maybe<T>.None;
        }

        public static T GetValueOrDefault<T>(this Maybe<T> maybe)
        {
            return GetValueOrDefault(maybe, default(T));
        }

        public static T GetValueOrDefault<T>(this Maybe<T> maybe, T defaultValue)
        {
            return maybe.HasValue
                ? maybe.Value
                : defaultValue;
        }

        public static Maybe<TResult> Select<T, TResult>(this Maybe<T> maybe, Func<T, TResult> selector)
        {
            return maybe.HasValue
                ? selector((T)maybe)
                : Maybe<TResult>.None;
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
    }
}
