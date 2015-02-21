using KLibrary.Labs.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.Collections
{
    [TestClass]
    public class QueueCollectionTest
    {
        [TestMethod]
        public void ctor_null()
        {
            var collection = new QueueCollection<int>();
            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(null, collection.MaxCount);
            Assert.AreEqual(false, collection.IsFull);

            for (int i = 0; i < 100; i++)
            {
                collection.Add(i);
            }
            Assert.AreEqual(100, collection.Count);
            Assert.AreEqual(null, collection.MaxCount);
            Assert.AreEqual(false, collection.IsFull);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ctor_0()
        {
            var collection = new QueueCollection<int>(0);
        }

        [TestMethod]
        public void ctor_1()
        {
            var collection = new QueueCollection<int>(1);
            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(1, collection.MaxCount);
            Assert.AreEqual(false, collection.IsFull);

            collection.Add(123);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(1, collection.MaxCount);
            Assert.AreEqual(true, collection.IsFull);
            Assert.AreEqual(123, collection.FirstItem);

            collection.Add(456);
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(1, collection.MaxCount);
            Assert.AreEqual(true, collection.IsFull);
            Assert.AreEqual(456, collection.FirstItem);
        }

        [TestMethod]
        public void ctor_3()
        {
            var collection = new QueueCollection<int>(3);
            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(3, collection.MaxCount);
            Assert.AreEqual(false, collection.IsFull);

            collection.Add(123);
            collection.Add(234);
            Assert.AreEqual(2, collection.Count);
            Assert.AreEqual(3, collection.MaxCount);
            Assert.AreEqual(false, collection.IsFull);
            Assert.AreEqual(234, collection.LastItem);

            collection.Add(345);
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual(3, collection.MaxCount);
            Assert.AreEqual(true, collection.IsFull);
            Assert.AreEqual(345, collection.LastItem);

            collection.Add(456);
            Assert.AreEqual(3, collection.Count);
            Assert.AreEqual(3, collection.MaxCount);
            Assert.AreEqual(true, collection.IsFull);
            Assert.AreEqual(456, collection.LastItem);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void Insert()
        {
            var collection = new QueueCollection<int>();
            collection.Insert(0, 123);
        }
    }
}
