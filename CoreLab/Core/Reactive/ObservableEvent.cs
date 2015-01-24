using System;

namespace KLibrary.Labs.Reactive
{
    // Use System.Reactive.Subjects.Subject<T>.
    public class ObservableEvent<T> : NotifierBase<T>, IObserver<T>
    {
        public void OnNext(T value)
        {
            NotifyNext(value);
        }

        public void OnError(Exception error)
        {
            NotifyError(error);
        }

        public void OnCompleted()
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
