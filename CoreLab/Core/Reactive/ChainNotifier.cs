using System;

namespace KLibrary.Labs.Reactive
{
    [Obsolete]
    public class ChainNotifier<T, TPrevious> : NotifierBase<T>
    {
        IDisposable _disposable;

        public ChainNotifier(IObservable<TPrevious> predecessor, Action<TPrevious, Action<T>> chain)
        {
            if (predecessor == null) throw new ArgumentNullException("predecessor");

            var observer = Observer2.Create<TPrevious>(o => chain(o, NotifyNext), NotifyError, NotifyCompleted);
            _disposable = predecessor.Subscribe(observer);
        }

        protected override void OnObservationStopped()
        {
            _disposable.Dispose();
        }
    }
}
