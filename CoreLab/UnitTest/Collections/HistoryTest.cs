using KLibrary.Labs.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace UnitTest.Collections
{
    [TestClass]
    public class HistoryTest
    {
        [TestMethod]
        public void ctor_null_null()
        {
            var history = new History<int>();
            Assert.AreEqual(null, history.MaxSpan);

            for (int i = 0; i < 100; i++)
            {
                history.Record(i);
                Thread.Sleep(30);
            }

            Assert.AreEqual(100, history.Count);
        }

        [TestMethod]
        public void ctor_null_1()
        {
            var span = TimeSpan.FromSeconds(1);
            var history = new History<int>(span);
            Assert.AreEqual(span, history.MaxSpan);

            for (int i = 0; i < 100; i++)
            {
                history.Record(i);
                Thread.Sleep(30);
            }

            Assert.IsTrue(history.Count < 100);
            Assert.IsTrue(history.LastItem.Timestamp - history.FirstItem.Timestamp < span);
        }

        [TestMethod]
        public void ctor_10_null()
        {
            var history = new History<int>(10);
            Assert.AreEqual(null, history.MaxSpan);

            for (int i = 0; i < 100; i++)
            {
                history.Record(i);
                Thread.Sleep(30);
            }

            Assert.AreEqual(10, history.Count);
        }

        [TestMethod]
        public void ctor_10_1()
        {
            var span = TimeSpan.FromSeconds(1);
            var history = new History<int>(10, span);

            for (int i = 0; i < 100; i++)
            {
                history.Record(i);
                Thread.Sleep(30);
            }

            Assert.AreEqual(10, history.Count);
        }

        [TestMethod]
        public void ctor_50_1()
        {
            var span = TimeSpan.FromSeconds(1);
            var history = new History<int>(50, span);

            for (int i = 0; i < 100; i++)
            {
                history.Record(i);
                Thread.Sleep(30);
            }

            Assert.IsTrue(history.Count > 30);
            Assert.IsTrue(history.Count < 40);
        }
    }
}
