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
    }
}
