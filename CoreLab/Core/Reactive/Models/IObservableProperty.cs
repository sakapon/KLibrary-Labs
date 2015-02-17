using System;
using System.ComponentModel;

namespace KLibrary.Labs.Reactive.Models
{
    /// <summary>
    /// Represents an IObservable-based settable property, which notifies the value has been changed.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public interface IObservableProperty<T> : IObservable<T>, IObserver<T>, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        T Value { get; set; }
    }

    /// <summary>
    /// Represents an IObservable-based get-only property, which notifies the value has been changed.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public interface IObservableGetProperty<out T> : IObservable<T>, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Notifies this object to update the value.
        /// </summary>
        void OnNext();
    }
}
