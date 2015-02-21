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

        public static IObservable<TResult> ChainNext<TSource, TResult>(this IObservable<TSource> source, Func<IObserver<TResult>, Action<TSource>> getOnNext)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (getOnNext == null) throw new ArgumentNullException("getOnNext");

            var chain = new ObservableChain<TResult>();
            var observer = Observer2.Create(getOnNext(chain), chain.OnError, chain.OnCompleted);
            chain.Subscription = source.Subscribe(observer);
            return chain;
        }

        public static IObservable<TSource> SetMaxFrequency<TSource>(this IObservable<TSource> source, double maxFrequency)
        {
            var filter = new FrequencyFilter(maxFrequency);

            return ChainNext<TSource, TSource>(source, obs => o => { if (filter.CheckLap()) obs.OnNext(o); });
        }

        public static IObservable<TSource> ToAsync<TSource>(this IObservable<TSource> source)
        {
            return ChainNext<TSource, TSource>(source, obs => o => Task.Run(() => obs.OnNext(o)));
        }

        public static IObservable<TSource> Filter<TSource>(this IObservable<TSource> source, Func<TSource, bool> filter)
        {
            return ChainNext<TSource, TSource>(source, obs => o => { if (filter(o)) obs.OnNext(o); });
        }

        public static IObservable<TResult> Map<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> mapping)
        {
            return ChainNext<TSource, TResult>(source, obs => o => obs.OnNext(mapping(o)));
        }

        [Obsolete("Use Take method.")]
        public static IObservable<TSource> Take2<TSource>(this IObservable<TSource> source, int count)
        {
            var isCompleted = false;
            var i = 0;

            return ChainNext<TSource, TSource>(source, obs => o =>
            {
                if (isCompleted) return;
                if (i < count) obs.OnNext(o);
                i++;
                if (i >= count)
                {
                    isCompleted = true;
                    obs.OnCompleted();
                }
            });
        }
    }
}
