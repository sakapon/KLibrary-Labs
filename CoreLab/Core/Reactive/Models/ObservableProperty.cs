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
    }

    public class ObservableGetProperty<T> : ObservableGetPropertyBase<T>
    {
        Func<T> _getValue;
        T _value;

        public override T Value
        {
            get { return _value; }
        }

        void SetValue(T value)
        {
            if (object.Equals(_value, value)) return;
            _value = value;
            NotifyValueChanged();
        }

        // 初期値を getValue で取得します。
        public ObservableGetProperty(Func<T> getValue)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            Initialize(getValue, getValue());
        }

        public ObservableGetProperty(Func<T> getValue, T defaultValue)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            Initialize(getValue, defaultValue);
        }

        void Initialize(Func<T> getValue, T value)
        {
            _getValue = getValue;
            _value = value;
        }

        public override void OnNext()
        {
            SetValue(_getValue());
        }
    }

    // 固定値プロパティの Observable 化 (非実用的)。
    [Obsolete]
    public class ReadOnlyProperty<T> : ObservableGetPropertyBase<T>
    {
        T _value;

        public override T Value
        {
            get { return _value; }
        }

        public ReadOnlyProperty() { }

        public ReadOnlyProperty(T value)
        {
            _value = value;
        }

        public override void OnNext()
        {
            NotifyValueChanged();
        }
    }

    // Lazy<T> の Observable 化 (非実用的)。
    [Obsolete]
    public class LazyReadOnlyProperty<T> : ObservableGetPropertyBase<T>
    {
        Lazy<T> _lazy;

        public override T Value
        {
            get { return _lazy.Value; }
        }

        public LazyReadOnlyProperty(Func<T> getValue)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            _lazy = new Lazy<T>(getValue);
        }

        public override void OnNext()
        {
            NotifyValueChanged();
        }
    }

    // 通常の get アクセサーの Observable 化。値は毎回評価されます。
    public class DirectGetProperty<T> : ObservableGetPropertyBase<T>
    {
        Func<T> _getValue;

        public override T Value
        {
            get { return _getValue(); }
        }

        public DirectGetProperty(Func<T> getValue)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            _getValue = getValue;
        }

        public override void OnNext()
        {
            NotifyValueChanged();
        }
    }

    // イベントで通知される値を持つプロパティ。
    public class FollowingGetProperty<T> : ObservableGetPropertyBase<T>
    {
        T _value;

        public override T Value
        {
            get { return _value; }
        }

        void SetValue(T value)
        {
            if (object.Equals(_value, value)) return;
            _value = value;
            NotifyValueChanged();
        }

        public FollowingGetProperty(IObservable<T> predecessor) : this(predecessor, default(T)) { }

        public FollowingGetProperty(IObservable<T> predecessor, T defaultValue)
        {
            if (predecessor == null) throw new ArgumentNullException("predecessor");

            predecessor.Subscribe(Observer2.Create<T>(SetValue));
            _value = defaultValue;
        }

        public override void OnNext()
        {
            throw new NotSupportedException();
        }
    }
}
