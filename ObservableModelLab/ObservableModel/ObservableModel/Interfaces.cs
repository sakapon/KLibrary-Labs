using System;
using System.ComponentModel;

namespace KLibrary.Labs.ObservableModel
{
    /// <summary>
    /// Represents an extended <see cref="IObservable&lt;T&gt;"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to be provided.</typeparam>
    public interface IObservable2<out T> : IObservable<T>
    {
        /// <summary>
        /// Gets a value indicating whether this observable object has observers.
        /// </summary>
        bool HasObservers { get; }
    }

    /// <summary>
    /// Represents an IObservable-based event.
    /// </summary>
    /// <typeparam name="T">The type of objects to be provided.</typeparam>
    public interface IJunction<T> : IObservable2<T>, IObserver<T>
    {
    }

    /// <summary>
    /// Represents an IObservable-based settable property.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public interface ISettableProperty<T> : IJunction<T>, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the value of the property.
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// Gets a value indicating whether notifications occur even if the value has been unchanged.
        /// </summary>
        bool NotifiesUnchanged { get; }
    }

    /// <summary>
    /// Represents an IObservable-based get-only property.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    public interface IGetOnlyProperty<out T> : IObservable2<T>, INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Gets a value indicating whether notifications occur even if the value has been unchanged.
        /// </summary>
        bool NotifiesUnchanged { get; }

        /// <summary>
        /// Notifies this object to update the value.
        /// </summary>
        void OnNext();
    }
}
