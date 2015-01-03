using System;

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
}
