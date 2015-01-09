using System;
using System.Threading;
using System.Threading.Tasks;

namespace KLibrary.Labs.Reactive
{
    public class TimeTicker : NotifierBase<long>
    {
        bool _isCompleted;

        public TimeTicker(TimeSpan interval) : this(interval, false) { }

        public TimeTicker(TimeSpan interval, bool forAsync)
        {
            Task.Run(() =>
            {
                var intervalMilliseconds = interval.TotalMilliseconds;
                var startTime = DateTime.Now;

                for (long i = 0; !_isCompleted; i++)
                {
                    var timeout = (i + 1) * intervalMilliseconds + (startTime - DateTime.Now).TotalMilliseconds;
                    Thread.Sleep(Math.Max(0, (int)timeout));

                    if (forAsync)
                    {
                        var j = i;
                        Task.Run(() => OnNext(j));
                    }
                    else
                    {
                        OnNext(i);
                    }
                }
            });
        }

        public override IDisposable Subscribe(IObserver<long> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");

            Observers.Add(observer);
            return Disposable.FromAction(() =>
            {
                Observers.Remove(observer);
                if (Observers.Count == 0) _isCompleted = true;
            });
        }
    }
}
