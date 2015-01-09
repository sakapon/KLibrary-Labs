using KLibrary.Labs.Mathematics;
using KLibrary.Labs.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reactive.Linq;
using System.Threading;

namespace UnitTest.Mathematics
{
    [TestClass]
    public class FrequencyMeterTest
    {
        [TestMethod]
        public void ctor_1()
        {
            var meter = new FrequencyMeter();

            Observable2.Interval(TimeSpan.FromMilliseconds(30))
                .Select(_ => meter.RecordLap())
                .Subscribe(Console.WriteLine);

            Thread.Sleep(3000);
        }

        [TestMethod]
        public void ctor_2()
        {
            var meter = new FrequencyMeter();

            Observable2.Interval(TimeSpan.FromMilliseconds(5))
                .Select(_ => meter.RecordLap())
                .Subscribe(Console.WriteLine);

            Thread.Sleep(3000);
        }

        [TestMethod]
        public void ctor_100_5()
        {
            var meter = new FrequencyMeter(200, TimeSpan.FromSeconds(5));

            Observable2.Interval(TimeSpan.FromMilliseconds(30))
                .Select(_ => meter.RecordLap())
                .Subscribe(Console.WriteLine);

            Thread.Sleep(3000);
        }
    }
}
