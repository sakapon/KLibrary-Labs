using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace KLibrary.Labs.ObservableModel
{
    [DebuggerDisplay("{Value}")]
    public abstract class SettablePropertyCore<T> : Junction<T>
    {
        public abstract T Value { get; set; }

        public bool NotifiesUnchanged { get; protected set; }

        public static explicit operator T(SettablePropertyCore<T> value)
        {
            return value.Value;
        }

        public override void OnNext(T value)
        {
            Value = value;
        }

        protected virtual void NotifyValueChanged()
        {
            NotifyNext(Value);
        }
    }

    [DebuggerDisplay("{Value}")]
    public abstract class GetOnlyPropertyCore<T> : NotifierBase<T>
    {
        public abstract T Value { get; }

        public bool NotifiesUnchanged { get; protected set; }

        public static explicit operator T(GetOnlyPropertyCore<T> value)
        {
            return value.Value;
        }

        public virtual void OnNext()
        {
            NotifyValueChanged();
        }

        protected virtual void NotifyValueChanged()
        {
            NotifyNext(Value);
        }
    }

    public abstract class SettablePropertyBase<T> : SettablePropertyCore<T>, ISettableProperty<T>
    {
        static readonly PropertyChangedEventArgs EventArgs_ValueChanged = new PropertyChangedEventArgs("Value");

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void NotifyValueChanged()
        {
            base.NotifyValueChanged();

            var h = PropertyChanged;
            if (h != null) h(this, EventArgs_ValueChanged);
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null) h(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class GetOnlyPropertyBase<T> : GetOnlyPropertyCore<T>, IGetOnlyProperty<T>
    {
        static readonly PropertyChangedEventArgs EventArgs_ValueChanged = new PropertyChangedEventArgs("Value");

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void NotifyValueChanged()
        {
            base.NotifyValueChanged();

            var h = PropertyChanged;
            if (h != null) h(this, EventArgs_ValueChanged);
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null) h(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
