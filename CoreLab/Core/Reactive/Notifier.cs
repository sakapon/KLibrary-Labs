using System;
using System.Collections.Generic;
using System.Linq;

namespace KLibrary.Labs.Reactive
{
    public class Notifier<T> : IObservable<T>
    {
        List<IObserver<T>> observers = new List<IObserver<T>>();

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");

            observers.Add(observer);
            return Disposable.FromAction(() => observers.Remove(observer));
        }

        public void NotifyNext(T value)
        {
            observers.ForEach(o => o.OnNext(value));
        }

        public void NotifyError(Exception error)
        {
            observers.ForEach(o => o.OnError(error));
        }

        public void NotifyCompleted()
        {
            observers.ForEach(o => o.OnCompleted());
        }
    }
}
