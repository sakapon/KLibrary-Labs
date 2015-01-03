using System;
using System.Threading;
using System.Threading.Tasks;

namespace KLibrary.Labs.Reactive
{
    public class TimeTicker : NotifierBase<long>
    {
        bool _isCompleted;

        public TimeTicker(TimeSpan interval)
        {
            Task.Run(() =>
            {
                var intervalMilliseconds = interval.TotalMilliseconds;
                var startTime = DateTime.Now;

                for (long i = 0; !_isCompleted; i++)
                {
                    var timeout = (i + 1) * intervalMilliseconds + (startTime - DateTime.Now).TotalMilliseconds;
                    Thread.Sleep((int)timeout);
                    OnNext(i);
                }
            });
        }
        public override IDisposable Subscribe(IObserver<long> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");

            observers.Add(observer);
            return Disposable.FromAction(() =>
            {
                observers.Remove(observer);
                if (observers.Count == 0) _isCompleted = true;
            });
        }
    }
}
