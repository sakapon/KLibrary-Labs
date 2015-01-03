using System;
using System.Collections.Generic;
using System.Linq;

namespace KLibrary.Labs.Reactive
{
    public abstract class NotifierBase<T> : IObservable<T>
    {
        List<IObserver<T>> observers = new List<IObserver<T>>();

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");

            observers.Add(observer);
            return Disposable.FromAction(() => observers.Remove(observer));
        }

        protected void OnNext(T value)
        {
            observers.ForEach(o => o.OnNext(value));
        }

        protected void OnError(Exception error)
        {
            observers.ForEach(o => o.OnError(error));
        }

        protected void OnCompleted()
        {
            observers.ForEach(o => o.OnCompleted());
        }
    }
}
