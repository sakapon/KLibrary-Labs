using KLibrary.Labs.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTest.Collections
{
    [TestClass]
    public class LimitedCollectionTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ctor_0()
        {
            var history = new LimitedCollection<int>(0);
        }

        [TestMethod]
        public void ctor_1()
        {
            var history = new LimitedCollection<int>(1);
            Assert.AreEqual(1, history.MaxCount);
            Assert.AreEqual(false, history.IsFull);

            history.Record(123);
            Assert.AreEqual(1, history.MaxCount);
            Assert.AreEqual(true, history.IsFull);
            Assert.AreEqual(123, history.First());

            history.Record(456);
            Assert.AreEqual(1, history.MaxCount);
            Assert.AreEqual(true, history.IsFull);
            Assert.AreEqual(456, history.First());
        }

        [TestMethod]
        public void ctor_3()
        {
            var history = new LimitedCollection<int>(3);
            Assert.AreEqual(3, history.MaxCount);
            Assert.AreEqual(false, history.IsFull);

            history.Record(123);
            history.Record(234);
            Assert.AreEqual(3, history.MaxCount);
            Assert.AreEqual(false, history.IsFull);
            Assert.AreEqual(234, history.Last());

            history.Record(345);
            Assert.AreEqual(3, history.MaxCount);
            Assert.AreEqual(true, history.IsFull);
            Assert.AreEqual(345, history.Last());

            history.Record(456);
            Assert.AreEqual(3, history.MaxCount);
            Assert.AreEqual(true, history.IsFull);
            Assert.AreEqual(456, history.Last());
        }
    }
}
