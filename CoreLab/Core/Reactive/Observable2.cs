using KLibrary.Labs.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KLibrary.Labs.Reactive
{
    public static class Observable2
    {
        public static IObservable<long> Interval(TimeSpan interval)
        {
            return new TimeTicker(interval);
        }

        public static IObservable<long> Interval(TimeSpan interval, bool forAsync)
        {
            return new TimeTicker(interval, forAsync);
        }

        public static IObservable<T> SetMaxFrequency<T>(this IObservable<T> observable, double maxFrequency)
        {
            if (observable == null) throw new ArgumentNullException("observable");

            var filter = new FrequencyFilter(maxFrequency);

            return new ChainNotifier<T, T>(observable, (o, onNext) => { if (filter.CheckLap()) onNext(o); });
        }
    }
}
