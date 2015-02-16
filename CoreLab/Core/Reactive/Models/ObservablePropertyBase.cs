using System;
using System.ComponentModel;

namespace KLibrary.Labs.Reactive.Models
{
    public abstract class ObservablePropertyBaseCore<T> : ObservableEvent<T>
    {
        public abstract T Value { get; set; }

        public static explicit operator T(ObservablePropertyBaseCore<T> value)
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

        public override string ToString()
        {
            // For designers.
            return GetType().Name;
        }
    }

    public abstract class ObservableGetPropertyBaseCore<T> : NotifierBase<T>
    {
        public abstract T Value { get; }

        public static explicit operator T(ObservableGetPropertyBaseCore<T> value)
        {
            return value.Value;
        }

        public abstract void OnNext();

        protected virtual void NotifyValueChanged()
        {
            NotifyNext(Value);
        }

        public override string ToString()
        {
            // For designers.
            return GetType().Name;
        }
    }

    public abstract class ObservablePropertyBase<T> : ObservablePropertyBaseCore<T>, IObservableProperty<T>
    {
        static readonly PropertyChangedEventArgs EventArgs_ValueChanged = new PropertyChangedEventArgs("Value");

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void NotifyValueChanged()
        {
            base.NotifyValueChanged();

            var pc = PropertyChanged;
            if (pc != null) pc(this, EventArgs_ValueChanged);
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            var pc = PropertyChanged;
            if (pc != null) pc(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class ObservableGetPropertyBase<T> : ObservableGetPropertyBaseCore<T>, IObservableGetProperty<T>
    {
        static readonly PropertyChangedEventArgs EventArgs_ValueChanged = new PropertyChangedEventArgs("Value");

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void NotifyValueChanged()
        {
            base.NotifyValueChanged();

            var pc = PropertyChanged;
            if (pc != null) pc(this, EventArgs_ValueChanged);
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            var pc = PropertyChanged;
            if (pc != null) pc(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
