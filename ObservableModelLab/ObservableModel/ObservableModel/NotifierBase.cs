using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace KLibrary.Labs.ObservableModel
{
    public abstract class NotifierBase<T> : IObservable2<T>
    {
        protected List<IObserver<T>> Observers { get; private set; }

        public bool HasObservers
        {
            get { return Observers.Count > 0; }
        }

        public NotifierBase()
        {
            Observers = new List<IObserver<T>>();
        }

        public virtual IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");

            Observers.Add(observer);
            if (Observers.Count == 1) OnObservationStarted();

            return Disposable.Create(() =>
            {
                Observers.Remove(observer);
                if (Observers.Count == 0) OnObservationStopped();
            });
        }

        protected void NotifyNext(T value)
        {
            // 変更操作との競合を避けるため、配列にコピーします。
            foreach (var o in Observers.ToArray())
            {
                o.OnNext(value);
            }
        }

        protected void NotifyError(Exception error)
        {
            foreach (var o in Observers.ToArray())
            {
                o.OnError(error);
            }
        }

        protected void NotifyCompleted()
        {
            foreach (var o in Observers.ToArray())
            {
                o.OnCompleted();
            }
        }

        protected virtual void OnObservationStarted()
        {
        }

        protected virtual void OnObservationStopped()
        {
        }
    }

    // OnXxx: 前のシーケンスから通知を受けるためのメソッド。
    // NotifyXxx: 後ろのシーケンスに通知するためのメソッド。
    public class Junction<T> : NotifierBase<T>, IJunction<T>
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
}
