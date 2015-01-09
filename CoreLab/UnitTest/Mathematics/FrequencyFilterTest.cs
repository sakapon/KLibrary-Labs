using KLibrary.Labs.Mathematics;
using KLibrary.Labs.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reactive.Linq;
using System.Threading;

namespace UnitTest.Mathematics
{
    [TestClass]
    public class FrequencyFilterTest
    {
        [TestMethod]
        public void ctor_1()
        {
            var filter = new FrequencyFilter(25);

            Observable2.Interval(TimeSpan.FromMilliseconds(20))
                .Where(_ => filter.CheckLap())
                .Subscribe(Console.WriteLine);

            Thread.Sleep(3000);
        }

        [TestMethod]
        public void ctor_2()
        {
            var filter = new FrequencyFilter(25);

            Observable2.Interval(TimeSpan.FromMilliseconds(20))
                .Do(_ => filter.CheckLap())
                .Select(_ => filter.ArrangedFrequency)
                .Subscribe(Console.WriteLine);

            Thread.Sleep(3000);
        }
    }
}
