using KLibrary.Labs.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;

namespace UnitTest.Reactive
{
    [TestClass]
    public class TimeTickerTest
    {
        [TestMethod]
        public void TimeTicker_ctor()
        {
            new TimeTicker(TimeSpan.FromSeconds(1))
                .Subscribe(i => Debug.WriteLine("{0}: {1:HH:mm:ss.fff}", i, DateTime.Now));

            Thread.Sleep(20000);
        }
    }
}
