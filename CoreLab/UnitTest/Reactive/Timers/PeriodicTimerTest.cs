using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using KLibrary.Labs.Reactive;
using KLibrary.Labs.Reactive.Timers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Reactive.Timers
{
    [TestClass]
    public class PeriodicTimerTest
    {
        static readonly Action<long> WriteLong = i => Debug.WriteLine("{0}: {1:HH:mm:ss.fff}", i, DateTime.Now);

        [TestMethod]
        public void ctor_Thread_Sync()
        {
            var threadIds = new PeriodicTimer(TimeSpan.FromMilliseconds(20))
                .Take(100)
                .Select(_ => Thread.CurrentThread.ManagedThreadId)
                .ToEnumerable()
                .Distinct()
                .ToArray();

            Assert.IsTrue(threadIds.Length == 1);
        }

        [TestMethod]
        public void ctor_Thread_Async()
        {
            var threadIds = new PeriodicTimer(TimeSpan.FromMilliseconds(20))
                .ToAsync()
                .Take(100)
                .Select(_ => Thread.CurrentThread.ManagedThreadId)
                .ToEnumerable()
                .Distinct()
                .ToArray();

            Assert.IsTrue(threadIds.Length > 1);
        }

        [TestMethod]
        public void ctor_Time_Sync()
        {
            WriteLong(-1);
            new PeriodicTimer(TimeSpan.FromSeconds(1))
                .Take(10)
                .Do(WriteLong)
                .Wait();
        }

        [TestMethod]
        public void ctor_Time_Async()
        {
            WriteLong(-1);
            new PeriodicTimer(TimeSpan.FromSeconds(1))
                .ToAsync()
                .Take(10)
                .Do(WriteLong)
                .Wait();
        }

        [TestMethod]
        public void Dispose()
        {
            var context = new PeriodicTimer(TimeSpan.FromMilliseconds(500))
                .Subscribe(WriteLong);

            using (context)
            {
                Thread.Sleep(6000);
            }
        }

        [TestMethod]
        public void Dispose_not()
        {
            var context = new PeriodicTimer(TimeSpan.FromMilliseconds(500))
                .Take(10)
                .Subscribe(WriteLong);

            Thread.Sleep(6000);
        }
    }
}
