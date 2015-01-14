using KLibrary.Labs.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public static IObservable<TSource> SetMaxFrequency<TSource>(this IObservable<TSource> source, double maxFrequency)
        {
            if (source == null) throw new ArgumentNullException("source");

            var filter = new FrequencyFilter(maxFrequency);

            return new ChainNotifier<TSource, TSource>(source, (o, onNext) => { if (filter.CheckLap()) onNext(o); });
        }

        public static IObservable<TSource> DoAsync<TSource>(this IObservable<TSource> source, Action<TSource> onNext)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (onNext == null) throw new ArgumentNullException("onNext");

            return new ChainNotifier<TSource, TSource>(source, (o, onNext2) =>
                Task.Run(() =>
                {
                    onNext(o);
                    onNext2(o);
                }));
        }
    }
}
