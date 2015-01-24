using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace KLibrary.Labs.Reactive.Timers
{
    public class PeriodicTimer : NotifierBase<long>
    {
        bool _isCompleted;

        public TimeSpan Interval { get; private set; }

        public PeriodicTimer(TimeSpan interval)
        {
            Interval = interval;

            Task.Run(() =>
            {
                var nextTimePoint = DateTime.Now;

                for (var i = 0L; !_isCompleted; i++)
                {
                    nextTimePoint += Interval;
                    var timeout = (nextTimePoint - DateTime.Now).TotalMilliseconds;
                    Thread.Sleep(Math.Max(0, (int)timeout));

                    OnNext(i);
                }

                Debug.WriteLine("The thread for tick is end.");
            });
        }

        protected override void OnDisposing()
        {
            _isCompleted = true;
        }
    }
}
