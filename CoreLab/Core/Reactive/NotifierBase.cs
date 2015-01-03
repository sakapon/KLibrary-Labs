using System;
using System.Collections.Generic;
using System.Linq;

namespace KLibrary.Labs.Reactive
{
    public abstract class NotifierBase<T> : IObservable<T>
    {
        protected List<IObserver<T>> Observers { get; private set; }

        public NotifierBase()
        {
            Observers = new List<IObserver<T>>();
        }

        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");

            Observers.Add(observer);
            return Disposable.FromAction(() => Observers.Remove(observer));
        }

        protected void OnNext(T value)
        {
            Observers.ForEach(o => o.OnNext(value));
        }

        protected void OnError(Exception error)
        {
            Observers.ForEach(o => o.OnError(error));
        }

        protected void OnCompleted()
        {
            Observers.ForEach(o => o.OnCompleted());
        }
    }
}
