using System;
using System.ComponentModel;

namespace KLibrary.Labs.Reactive.Models
{
    public interface IObservableProperty<T> : IObservable<T>, IObserver<T>, INotifyPropertyChanged
    {
        T Value { get; set; }
    }

    public interface IObservableGetProperty<T> : IObservable<T>, INotifyPropertyChanged
    {
        T Value { get; }

        void OnNext();
    }
}
