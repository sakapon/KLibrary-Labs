using System;
using KLibrary.Labs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class MaybeTest
    {
        [TestMethod]
        public void ToMaybe()
        {
            var m1 = 1.ToMaybe();
            Assert.AreEqual(true, m1.HasValue);
            Assert.AreEqual(1, m1.Value);

            var m2 = ((int?)2).ToMaybe();
            Assert.AreEqual(true, m2.HasValue);
            Assert.AreEqual(2, m2.Value);

            var m3 = default(int?).ToMaybe();
            Assert.AreEqual(false, m3.HasValue);
            Assert.AreEqual(0, m3.Value);
        }

        [TestMethod]
        public void Equals()
        {
            Assert.IsTrue(Maybe<int>.None == default(Maybe<int>));
            Assert.IsTrue(0.ToMaybe() == 0);
            Assert.IsTrue(1.ToMaybe() == 1);
            Assert.IsFalse(Maybe<int>.None == 0);
            Assert.IsFalse(0 == Maybe<int>.None);
            Assert.IsFalse(1.ToMaybe() == 2);

            var obj = new object();
            Assert.IsTrue(Maybe<object>.None == default(object));
            Assert.IsTrue(new Maybe<object>(null) == default(object));
            Assert.IsTrue(obj.ToMaybe() == obj);
            Assert.IsFalse(Maybe<object>.None == obj);
            Assert.IsFalse(obj == Maybe<object>.None);
            Assert.IsFalse(obj.ToMaybe() == new object());
        }

        [TestMethod]
        public void Do()
        {
            var flag = 0;
            Action<int> action = i => flag = i;

            var m1 = 1.ToMaybe().Where(i => i % 2 == 0).Do(action);
            Assert.AreEqual(0, flag);
            Assert.AreEqual(0, m1.Value);

            var m2 = 2.ToMaybe().Where(i => i % 2 == 0).Do(action);
            Assert.AreEqual(2, flag);
            Assert.AreEqual(2, m2.Value);
        }

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
        public void Combine()
        {
            Func<int, string, string> format = (i, f) => string.Format(f, i);

            var m1 = 1.ToMaybe().Combine("int:{0}", format);
            Assert.AreEqual(true, m1.HasValue);
            Assert.AreEqual("int:1", m1.Value);
            var m2 = 1.ToMaybe().Combine(null, format);
            Assert.AreEqual(false, m2.HasValue);
            Assert.AreEqual(null, m2.Value);
            var m3 = Maybe<int>.None.Combine("int:{0}", format);
            Assert.AreEqual(false, m3.HasValue);
            Assert.AreEqual(null, m3.Value);
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
