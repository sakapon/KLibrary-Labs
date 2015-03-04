using System;

namespace KLibrary.Labs
{
    public struct Sure<T>
    {
        T _value;

        public T Value
        {
            get
            {
                if (_value == null) throw new InvalidOperationException("The value must not be null.");
                return _value;
            }
            private set
            {
                if (value == null) throw new ArgumentNullException("value");
                _value = value;
            }
        }

        // In case T is class, "default(Sure<T>)" and "new Sure<T>()" are invalid.
        public Sure(T value)
            : this()
        {
            Value = value;
        }

        public static explicit operator T(Sure<T> value)
        {
            return value.Value;
        }

        public static implicit operator Sure<T>(T value)
        {
            return new Sure<T>(value);
        }

        public static bool operator ==(Sure<T> value1, Sure<T> value2)
        {
            return object.Equals(value1.Value, value2.Value);
        }

        public static bool operator !=(Sure<T> value1, Sure<T> value2)
        {
            return !(value1 == value2);
        }

        public override bool Equals(object obj)
        {
            return obj is Sure<T> && this == (Sure<T>)obj;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public Sure<TResult> Bind<TResult>(Func<T, Sure<TResult>> func)
        {
            return func(Value);
        }
    }

    public static class Sure
    {
        public static Sure<T> ToSure<T>(this T value)
        {
            return value;
        }

        public static Sure<T> Do<T>(this Sure<T> sure, Action<T> action)
        {
            action(sure.Value);
            return sure;
        }

        public static Sure<TResult> Select<T, TResult>(this Sure<T> sure, Func<T, TResult> selector)
        {
            return selector(sure.Value);
        }

        public static Sure<TResult> SelectMany<T, TResult>(this Sure<T> sure, Func<T, Sure<TResult>> selector)
        {
            return sure.Bind(selector);
        }

        public static Sure<TResult> SelectMany<T, U, TResult>(this Sure<T> sure, Func<T, Sure<U>> selector, Func<T, U, TResult> resultSelector)
        {
            var selected = sure.Bind(selector);
            return resultSelector(sure.Value, selected.Value);
        }

        public static Sure<TResult> Combine<T1, T2, TResult>(this Sure<T1> sure1, Sure<T2> sure2, Func<T1, T2, TResult> resultSelector)
        {
            return resultSelector(sure1.Value, sure2.Value);
        }
    }
}
