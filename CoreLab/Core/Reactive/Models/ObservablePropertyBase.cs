using System;
using System.ComponentModel;

namespace KLibrary.Labs.Reactive.Models
{
    public abstract class ObservablePropertyBaseCore<T> : NotifierBase<T>, IObservableProperty
    {
        public abstract T Value { get; set; }

        public virtual void NotifyValueChanged()
        {
            OnNext(Value);
        }

        public IDisposable Subscribe(Action onValueChanged)
        {
            if (onValueChanged == null) throw new ArgumentNullException("onValueChanged");

            return Subscribe(new ActionObserver<T>(o => onValueChanged()));
        }

        public override string ToString()
        {
            // For designers.
            return GetType().Name;
        }
    }

    public abstract class ObservablePropertyBase<T> : ObservablePropertyBaseCore<T>, INotifyPropertyChanged
    {
        static readonly PropertyChangedEventArgs EventArgs_ValueChanged = new PropertyChangedEventArgs("Value");

        public event PropertyChangedEventHandler PropertyChanged;

        public override void NotifyValueChanged()
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
