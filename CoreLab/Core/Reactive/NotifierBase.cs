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
            return Disposable.FromAction(() =>
            {
                Observers.Remove(observer);
                if (Observers.Count == 0) OnDisposing();
            });
        }

        protected void OnNext(T value)
        {
            // 変更操作との競合を避けるため、配列にコピーします。
            Array.ForEach(Observers.ToArray(), o => o.OnNext(value));
        }

        protected void OnError(Exception error)
        {
            Array.ForEach(Observers.ToArray(), o => o.OnError(error));
        }

        protected void OnCompleted()
        {
            Array.ForEach(Observers.ToArray(), o => o.OnCompleted());
        }

        protected virtual void OnDisposing()
        {
        }
    }
}
