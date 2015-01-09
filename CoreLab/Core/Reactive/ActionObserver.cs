using System;

namespace KLibrary.Labs.Reactive
{
    public class ActionObserver<T> : IObserver<T>
    {
        Action<T> _onNext;
        Action<Exception> _onError;
        Action _onCompleted;

        public ActionObserver(Action<T> onNext) : this(onNext, null, null) { }

        public ActionObserver(Action<T> onNext, Action<Exception> onError) : this(onNext, onError, null) { }

        public ActionObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
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
