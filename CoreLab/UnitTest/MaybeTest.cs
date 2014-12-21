﻿using KLibrary.Labs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class MaybeTest
    {
        [TestMethod]
        public void Map()
        {
            var r1 = 2.ToMaybe().Select(i => 3 * i);
            Assert.IsTrue(r1.HasValue);
            Assert.AreEqual(6, (int)r1);

            var r2 = Maybe<int>.None.Select(i => 3 * i);
            Assert.IsFalse(r2.HasValue);
        }

        [TestMethod]
        public void QueryExpression()
        {
            Assert.IsTrue(Add(1, 2).HasValue);
            Assert.AreEqual(3, (int)Add(1, 2));
            Assert.IsFalse(Add(2, 1).HasValue);
            Assert.IsFalse(Add(1, Maybe<int>.None).HasValue);
        }

        static Maybe<int> Add(Maybe<int> x, Maybe<int> y)
        {
            return
                from _x in x
                from _y in y
                where _x < _y
                select _x + _y;
        }
    }
}