using System;
using System.ComponentModel;

namespace KLibrary.Labs.ObservableModel
{
    public interface IObservable2<out T> : IObservable<T>
    {
        bool HasObservers { get; }
    }

    public interface IJunction<T> : IObservable2<T>, IObserver<T>
    {
    }

    public interface ISettableProperty<T> : IJunction<T>, INotifyPropertyChanged
    {
        T Value { get; set; }
        bool NotifiesUnchanged { get; }
    }

    public interface IGetOnlyProperty<out T> : IObservable2<T>, INotifyPropertyChanged
    {
        T Value { get; }
        bool NotifiesUnchanged { get; }
        void OnNext();
    }
}
