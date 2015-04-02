using System;

namespace KLibrary.Labs.ObservableModel
{
    class SettableProperty<T> : SettablePropertyBase<T>
    {
        T _value;

        public override T Value
        {
            get { return _value; }
            set
            {
                if (!NotifiesUnchanged && object.Equals(_value, value)) return;
                _value = value;
                NotifyValueChanged();
            }
        }

        public SettableProperty(T defaultValue, bool notifiesUnchanged)
        {
            _value = defaultValue;
            NotifiesUnchanged = notifiesUnchanged;
        }
    }

    public abstract class CachingGetOnlyPropertyBase<T> : GetOnlyPropertyBase<T>
    {
        protected T _value;

        public override T Value
        {
            get { return _value; }
        }

        protected void SetValue(T value)
        {
            if (!NotifiesUnchanged && object.Equals(_value, value)) return;
            _value = value;
            NotifyValueChanged();
        }
    }

    // 値を取得するためのメソッドを持つプロパティ。
    class CachingGetOnlyProperty<T> : CachingGetOnlyPropertyBase<T>
    {
        Func<T> _getValue;

        public CachingGetOnlyProperty(Func<T> getValue, T defaultValue, bool notifiesUnchanged)
        {
            if (getValue == null) throw new ArgumentNullException("getValue");

            _getValue = getValue;
            _value = defaultValue;
            NotifiesUnchanged = notifiesUnchanged;
        }

        public override void OnNext()
        {
            SetValue(_getValue());
        }
    }

    // 値が通知されるプロパティ。
    class FollowingGetOnlyProperty<T> : CachingGetOnlyPropertyBase<T>
    {
        public FollowingGetOnlyProperty(IObservable<T> predecessor, T defaultValue, bool notifiesUnchanged)
        {
            if (predecessor == null) throw new ArgumentNullException("predecessor");

            predecessor.Subscribe(SetValue);
            _value = defaultValue;
            NotifiesUnchanged = notifiesUnchanged;
        }
    }
}
