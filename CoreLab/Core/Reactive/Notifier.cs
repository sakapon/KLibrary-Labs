﻿using System;

namespace KLibrary.Labs.Reactive
{
    public class Notifier<T> : NotifierBase<T>
    {
        public void NotifyNext(T value)
        {
            OnNext(value);
        }

        public void NotifyError(Exception error)
        {
            OnError(error);
        }

        public void NotifyCompleted()
        {
            OnCompleted();
        }
    }

    public class ChainNotifier<T, TPrevious> : NotifierBase<T>
    {
        IDisposable _disposable;

        public ChainNotifier(IObservable<TPrevious> predecessor, Action<TPrevious, Action<T>> chain)
        {
            if (predecessor == null) throw new ArgumentNullException("predecessor");

            var observer = new ActionObserver<TPrevious>(o => chain(o, OnNext), OnError, OnCompleted);
            _disposable = predecessor.Subscribe(observer);
        }

        protected override void OnDisposing()
        {
            _disposable.Dispose();
        }
    }
}
