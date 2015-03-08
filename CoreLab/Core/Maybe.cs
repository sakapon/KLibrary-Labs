using System;
using System.Diagnostics;

namespace KLibrary.Labs
{
    /// <summary>
    /// Represents a value type that can be assigned the unset state.
    /// </summary>
    /// <typeparam name="T">The underlying value type.</typeparam>
    [DebuggerDisplay("{HasValue ? Value}")]
    public struct Maybe<T>
    {
        /// <summary>
        /// Represents the unset state.
        /// </summary>
        public static readonly Maybe<T> None = new Maybe<T>();

        /// <summary>
        /// Gets the underlying value.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this object has a non-null value.
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe&lt;T&gt;"/> structure.
        /// </summary>
        /// <param name="value">The underlying value.</param>
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
                : default(int);
        }

        public override string ToString()
        {
            return HasValue
                ? Value.ToString()
                : "";
        }

        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func)
        {
            return HasValue
                ? func(Value)
                : Maybe<TResult>.None;
        }
    }

    /// <summary>
    /// Provides a set of static methods for the <see cref="Maybe&lt;T&gt;"/> structure.
    /// </summary>
    public static class Maybe
    {
        /// <summary>
        /// Creates a <see cref="Maybe&lt;T&gt;"/> from a value.
        /// </summary>
        /// <typeparam name="T">The type of a value.</typeparam>
        /// <param name="value">A value.</param>
        /// <returns>A <see cref="Maybe&lt;T&gt;"/>.</returns>
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return value;
        }

        /// <summary>
        /// Creates a <see cref="Maybe&lt;T&gt;"/> from a nullable value.
        /// </summary>
        /// <typeparam name="T">The type of a value.</typeparam>
        /// <param name="value">A value.</param>
        /// <returns>A <see cref="Maybe&lt;T&gt;"/>.</returns>
        public static Maybe<T> ToMaybe<T>(this T? value) where T : struct
        {
            return value.HasValue
                ? value.Value
                : Maybe<T>.None;
        }

        /// <summary>
        /// Creates a nullable value from a <see cref="Maybe&lt;T&gt;"/>.
        /// </summary>
        /// <typeparam name="T">The type of a value.</typeparam>
        /// <param name="value">A <see cref="Maybe&lt;T&gt;"/>.</param>
        /// <returns>A <see cref="Nullable&lt;T&gt;"/>.</returns>
        public static T? ToNullable<T>(this Maybe<T> value) where T : struct
        {
            return value.HasValue
                ? value.Value
                : default(T?);
        }

        public static Maybe<T> Do<T>(this Maybe<T> maybe, Action<T> action)
        {
            if (maybe.HasValue) action(maybe.Value);
            return maybe;
        }

        public static Maybe<TResult> Select<T, TResult>(this Maybe<T> maybe, Func<T, TResult> selector)
        {
            return maybe.HasValue
                ? selector(maybe.Value)
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
                ? resultSelector(maybe.Value, selected.Value)
                : Maybe<TResult>.None;
        }

        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
        {
            return maybe.HasValue && predicate(maybe.Value)
                ? maybe
                : Maybe<T>.None;
        }

        public static Maybe<TResult> Combine<T1, T2, TResult>(this Maybe<T1> maybe1, Maybe<T2> maybe2, Func<T1, T2, TResult> resultSelector)
        {
            return maybe1.HasValue && maybe2.HasValue
                ? resultSelector(maybe1.Value, maybe2.Value)
                : Maybe<TResult>.None;
        }
    }
}
