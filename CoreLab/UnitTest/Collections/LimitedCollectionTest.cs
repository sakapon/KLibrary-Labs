using KLibrary.Labs.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.Collections
{
    [TestClass]
    public class LimitedCollectionTest
    {
        [TestMethod]
        public void ctor_null()
        {
            var collection = new LimitedCollection<int>();
            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(null, collection.MaxCount);
            Assert.AreEqual(false, collection.IsFull);

            for (int i = 0; i < 100; i++)
            {
                collection.Record(i);
            }
            Assert.AreEqual(100, collection.Count);
            Assert.AreEqual(null, collection.MaxCount);
            Assert.AreEqual(false, collection.IsFull);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ctor_0()
        {
            var collection = new LimitedCollection<int>(0);
        }

        [TestMethod]
        public void ctor_1()
        {
            var collection = new LimitedCollection<int>(1);
            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(1, collection.MaxCount);
            Assert.AreEqual(false, collection.IsFull);

            collection.Record(123);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(1, collection.MaxCount);
            Assert.AreEqual(true, collection.IsFull);
            Assert.AreEqual(123, collection.FirstItem);

            collection.Record(456);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(1, collection.MaxCount);
            Assert.AreEqual(true, collection.IsFull);
            Assert.AreEqual(456, collection.FirstItem);
        }

        [TestMethod]
        public void ctor_3()
        {
            var collection = new LimitedCollection<int>(3);
            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(3, collection.MaxCount);
            Assert.AreEqual(false, collection.IsFull);

            collection.Record(123);
            collection.Record(234);
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(3, collection.MaxCount);
            Assert.AreEqual(false, collection.IsFull);
            Assert.AreEqual(234, collection.LastItem);

            collection.Record(345);
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual(3, collection.MaxCount);
            Assert.AreEqual(true, collection.IsFull);
            Assert.AreEqual(345, collection.LastItem);

            collection.Record(456);
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual(3, collection.MaxCount);
            Assert.AreEqual(true, collection.IsFull);
            Assert.AreEqual(456, collection.LastItem);
        }
    }
}
