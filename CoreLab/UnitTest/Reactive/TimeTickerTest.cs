using KLibrary.Labs.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;

namespace UnitTest.Reactive
{
    [TestClass]
    public class TimeTickerTest
    {
        [TestMethod]
        public void TimeTicker_ctor()
        {
            var context = new TimeTicker(TimeSpan.FromSeconds(1))
                .Do(i => Debug.WriteLine("Thread: {0}", Thread.CurrentThread.ManagedThreadId))
                .Subscribe(i => Debug.WriteLine("{0}: {1:HH:mm:ss.fff}", i, DateTime.Now));

            using (context)
            {
                Thread.Sleep(20000);
            }
        }

        [TestMethod]
        public void TimeTicker_ctor2()
        {
            var context = new TimeTicker(TimeSpan.FromSeconds(1))
                .Take(10)
                .Do(i => Debug.WriteLine("Thread: {0}", Thread.CurrentThread.ManagedThreadId))
                .Subscribe(i => Debug.WriteLine("{0}: {1:HH:mm:ss.fff}", i, DateTime.Now));

            Thread.Sleep(20000);
        }
    }
}
