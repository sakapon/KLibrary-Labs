using System;

namespace KLibrary.Labs.Reactive
{
    /// <summary>
    /// Represents an IObservable-based event.
    /// </summary>
    /// <typeparam name="T">The type of objects to be provided.</typeparam>
    public interface IObservableEvent<T> : IObservable<T>, IObserver<T>
    {
        /// <summary>
        /// Gets a value indicating whether this observable object has observers.
        /// </summary>
        bool HasObservers { get; }
    }

    // Use System.Reactive.Subjects.Subject<T> class.
    public class ObservableEvent<T> : NotifierBase<T>, IObservableEvent<T>
    {
        public virtual void OnNext(T value)
        {
            NotifyNext(value);
        }

        public virtual void OnError(Exception error)
        {
            NotifyError(error);
        }

        public virtual void OnCompleted()
        {
            NotifyCompleted();
        }
    }

    internal class ObservableEvent_Old<T> : IObservable<T>
    {
        event Action<T> TheEvent;

        public IDisposable AddHandler(Action<T> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");

            TheEvent += handler;
            return Disposable.FromAction(() => TheEvent -= handler);
        }

        public void Raise(T arg)
        {
            var h = TheEvent;
            if (h != null) h(arg);
        }

        IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");

            return AddHandler(observer.OnNext);
        }
    }
}
