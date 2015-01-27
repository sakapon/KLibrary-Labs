using System;

namespace KLibrary.Labs.Reactive.Models
{
    public class ObservableProperty<T> : ObservablePropertyBase<T>
    {
        T _value;

        public override T Value
        {
            get { return _value; }
            set
            {
                if (object.Equals(_value, value)) return;
                _value = value;
                NotifyValueChanged();
            }
        }

        public ObservableProperty() { }

        public ObservableProperty(T defaultValue)
        {
            _value = defaultValue;
        }

        public static explicit operator ObservableProperty<T>(T value)
        {
            return new ObservableProperty<T>(value);
        }
    }

    // 固定値プロパティの Observable 化 (非実用的)。
    public class ReadOnlyProperty<T> : ObservablePropertyBase<T>
    {
        T _value;

        public override T Value
        {
            get { return _value; }
            set { throw new NotSupportedException(); }
        }

        public ReadOnlyProperty() { }

        public ReadOnlyProperty(T value)
        {
            _value = value;
        }

        public static explicit operator ReadOnlyProperty<T>(T value)
        {
            return new ReadOnlyProperty<T>(value);
        }
    }

    // Lazy<T> の Observable 化 (非実用的)。
    public class LazyReadOnlyProperty<T> : ObservablePropertyBase<T>
    {
        Lazy<T> _value;

        public override T Value
        {
            get { return _value.Value; }
            set { throw new NotSupportedException(); }
        }

        public LazyReadOnlyProperty(Func<T> getValue)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            _value = new Lazy<T>(getValue);
        }
    }

    // 通常の get アクセサーの Observable 化。値は毎回評価されます。
    public class GetProperty<T> : ObservablePropertyBase<T>
    {
        Func<T> _getValue;

        public override T Value
        {
            get { return _getValue(); }
            set { throw new NotSupportedException(); }
        }

        public GetProperty(Func<T> getValue, params IObservableProperty[] parentProperties)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            _getValue = getValue;
            Array.ForEach(parentProperties, p => p.Subscribe(NotifyValueChanged));
        }
    }

    public class CachingGetProperty<T> : ObservablePropertyBase<T>
    {
        T _value;
        Func<T> _getValue;

        public override T Value
        {
            get { return _value; }
            set { throw new NotSupportedException(); }
        }

        // 初期値を getValue で取得します。
        public CachingGetProperty(Func<T> getValue, params IObservableProperty[] parentProperties)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");
            if (parentProperties == null) throw new ArgumentNullException("parentProperties");

            Initialize(getValue(), getValue, parentProperties);
        }

        public CachingGetProperty(T defaultValue, Func<T> getValue, params IObservableProperty[] parentProperties)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");
            if (parentProperties == null) throw new ArgumentNullException("parentProperties");

            Initialize(defaultValue, getValue, parentProperties);
        }

        void Initialize(T value, Func<T> getValue, IObservableProperty[] parentProperties)
        {
            _value = value;
            _getValue = getValue;
            Array.ForEach(parentProperties, p => p.Subscribe(UpdateValue));
        }

        // Manual update
        public void UpdateValue()
        {
            var value = _getValue();

            if (object.Equals(_value, value)) return;
            _value = value;
            NotifyValueChanged();
        }
    }
}
