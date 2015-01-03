using System;
using System.Threading.Tasks;

namespace KLibrary.Labs.Reactive
{
    public class TimeTicker : NotifierBase<long>
    {
        bool _isCompleted;

        public TimeTicker(TimeSpan interval)
        {
            Task.Run(async () =>
            {
                var intervalMilliseconds = interval.TotalMilliseconds;
                var startTime = DateTime.Now;

                for (long i = 0; !_isCompleted; i++)
                {
                    var timeout = (i + 1) * intervalMilliseconds + (startTime - DateTime.Now).TotalMilliseconds;
                    await Task.Delay(Math.Max(0, (int)timeout));
                    OnNext(i);
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
