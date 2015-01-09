using KLibrary.Labs.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;

namespace UnitTest.Reactive
{
    [TestClass]
    public class TimeTickerTest
    {
        [TestMethod]
        public void ctor_Thread_Sync()
        {
            var threadIds = new TimeTicker(TimeSpan.FromMilliseconds(20))
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
            var threadIds = new TimeTicker(TimeSpan.FromMilliseconds(20), true)
                .Take(100)
                .Select(_ => Thread.CurrentThread.ManagedThreadId)
                .ToEnumerable()
                .Distinct()
                .ToArray();

            Assert.IsTrue(threadIds.Length > 1);
        }

        [TestMethod]
        public void ctor_Sync()
        {
            new TimeTicker(TimeSpan.FromSeconds(1))
                .Take(10)
                .Do(i => Debug.WriteLine("{0}: {1:HH:mm:ss.fff}", i, DateTime.Now))
                .Wait();
        }

        [TestMethod]
        public void ctor_Async()
        {
            new TimeTicker(TimeSpan.FromSeconds(1), true)
                .Take(10)
                .Do(i => Debug.WriteLine("{0}: {1:HH:mm:ss.fff}", i, DateTime.Now))
                .Wait();
        }

        [TestMethod]
        public void Dispose()
        {
            var context = new TimeTicker(TimeSpan.FromMilliseconds(500))
                .Subscribe(i => Debug.WriteLine("{0}: {1:HH:mm:ss.fff}", i, DateTime.Now));

            using (context)
            {
                Thread.Sleep(6000);
            }
        }

        [TestMethod]
        public void Dispose_not()
        {
            var context = new TimeTicker(TimeSpan.FromMilliseconds(500))
                .Take(10)
                .Subscribe(i => Debug.WriteLine("{0}: {1:HH:mm:ss.fff}", i, DateTime.Now));

            Thread.Sleep(6000);
        }
    }
}
