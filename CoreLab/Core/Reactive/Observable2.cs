using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KLibrary.Labs.Mathematics;
using KLibrary.Labs.Reactive.Timers;

namespace KLibrary.Labs.Reactive
{
    public static class Observable2
    {
        public static IObservable<long> Interval(TimeSpan interval)
        {
            return new PeriodicTimer(interval);
        }

        public static IObservable<TSource> SetMaxFrequency<TSource>(this IObservable<TSource> source, double maxFrequency)
        {
            if (source == null) throw new ArgumentNullException("source");

            var filter = new FrequencyFilter(maxFrequency);

            return new ChainNotifier<TSource, TSource>(source, (o, onNext) => { if (filter.CheckLap()) onNext(o); });
        }

        public static IObservable<TSource> ToAsync<TSource>(this IObservable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new ChainNotifier<TSource, TSource>(source, (o, onNext) => Task.Run(() => onNext(o)));
        }
    }
}
