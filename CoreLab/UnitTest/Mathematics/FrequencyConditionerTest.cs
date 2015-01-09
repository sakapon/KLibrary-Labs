using KLibrary.Labs.Mathematics;
using KLibrary.Labs.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reactive.Linq;
using System.Threading;

namespace UnitTest.Mathematics
{
    [TestClass]
    public class FrequencyConditionerTest
    {
        [TestMethod]
        public void ctor()
        {
            var conditioner = new FrequencyConditioner(30);

            Observable2.Interval(TimeSpan.FromMilliseconds(33))
                .Take(150)
                .Where(_ => conditioner.CheckLap())
                .Do(i => Console.WriteLine("{0}: {1}", i, conditioner.Frequency))
                .Do(_ => Thread.Sleep(40))
                .Wait();
        }
    }
}
