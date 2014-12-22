using System;

namespace KLibrary.Labs.Reactive
{
    public class ObservableEvent<T> : IObservable<T>
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
