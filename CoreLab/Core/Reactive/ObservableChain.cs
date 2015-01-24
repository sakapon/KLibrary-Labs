using System;

namespace KLibrary.Labs.Reactive
{
    public class ObservableChain<T> : ObservableEvent<T>
    {
        public IDisposable Subscription { get; set; }

        public override void OnCompleted()
        {
            base.OnCompleted();

            if (Subscription != null) Subscription.Dispose();
        }

        protected override void OnObservationStopped()
        {
            if (Subscription != null) Subscription.Dispose();
        }
    }
}
