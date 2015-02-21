using System;

namespace KLibrary.Labs.Reactive
{
    public static class Observer2
    {
        public static IObserver<T> Create<T>(Action<T> onNext)
        {
            return new DelegateObserver<T>(onNext, null, null);
        }

        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError)
        {
            return new DelegateObserver<T>(onNext, onError, null);
        }

        public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            return new DelegateObserver<T>(onNext, onError, onCompleted);
        }
    }

    class DelegateObserver<T> : IObserver<T>
    {
        Action<T> _onNext;
        Action<Exception> _onError;
        Action _onCompleted;

        public DelegateObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public void OnNext(T value)
        {
            if (_onNext != null) _onNext(value);
        }

        public void OnError(Exception error)
        {
            if (_onError != null) _onError(error);
        }

        public void OnCompleted()
        {
            if (_onCompleted != null) _onCompleted();
        }
    }
}
