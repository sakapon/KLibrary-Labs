using KLibrary.Labs.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reactive.Linq;
using System.Threading;

namespace UnitTest.Reactive
{
    [TestClass]
    public class ObservableEventTest
    {
        [TestMethod]
        public void CounterTest()
        {
            var counter = new Counter();
            counter.Counted.Subscribe(Console.WriteLine);

            Thread.Sleep(3000);
        }

        [TestMethod]
        public void SwitcherTest()
        {
            var switcher = new Switcher();
            switcher.Switched.Subscribe(Console.WriteLine);

            Thread.Sleep(5000);
        }

        [TestMethod]
        public void BasicTest()
        {
            var counter = new Counter();
            counter.Counted
                .Where(i => i % 2 == 1)
                .Select(i => i * i / 10.0)
                .Take(10)
                .Subscribe(Console.WriteLine);

            Thread.Sleep(3000);
        }

        [TestMethod]
        public void SwitchTest()
        {
            var switcher = new Switcher();
            var counter = new Counter();

            switcher.Switched
                .Where(b => b)
                .Do(b => Console.WriteLine("Begin"))
                .SelectMany(counter.Counted
                    .TakeUntil(switcher.Switched
                        .Where(b => !b)
                        .Do(i => Console.WriteLine("End"))))
                .Subscribe(Console.WriteLine);

            Thread.Sleep(5500);
        }

        public class Counter
        {
            public readonly ObservableEvent<long> Counted = new ObservableEvent<long>();

            public Counter()
            {
                Observable.Interval(TimeSpan.FromMilliseconds(100))
                    .Subscribe(Counted.NotifyNext);
            }
        }

        public class Switcher
        {
            public readonly ObservableEvent<bool> Switched = new ObservableEvent<bool>();
            bool _flag;

            public Switcher()
            {
                Observable.Interval(TimeSpan.FromSeconds(1))
                    .Select(i => (_flag = !_flag))
                    .Subscribe(Switched.NotifyNext);
            }
        }
    }
}
