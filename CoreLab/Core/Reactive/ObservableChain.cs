using System;

namespace KLibrary.Labs.Reactive
{
    public class ObservableChain<T> : ObservableEvent<T>
    {
        bool _isUnsubscribed;

        public IDisposable Subscription { get; set; }

        public override void OnCompleted()
        {
            base.OnCompleted();

            Unsubscribe();
        }

        protected override void OnObservationStopped()
        {
            Unsubscribe();
        }

        void Unsubscribe()
        {
            if (!_isUnsubscribed && Subscription != null)
            {
                _isUnsubscribed = true;
                Subscription.Dispose();
            }
        }
    }
}
