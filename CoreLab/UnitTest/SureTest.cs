using System;
using KLibrary.Labs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class SureTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Default_Class()
        {
            var sure = default(Sure<string>);
            var value = sure.Value;
        }

        [TestMethod]
        public void Default_Struct()
        {
            var sure = default(Sure<int>);
            Assert.AreEqual(0, sure.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ctor_Class()
        {
            var sure = new Sure<string>();
            var value = sure.Value;
        }

        [TestMethod]
        public void ctor_Struct()
        {
            var sure = new Sure<int>();
            Assert.AreEqual(0, sure.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToSure_null()
        {
            var obj = default(object);
            var sure = obj.ToSure();
        }

        [TestMethod]
        public void Map()
        {
            var name = "Name 1";

            var sure = name.ToSure().Select(s => Tuple.Create(s));
            Assert.AreEqual(name, sure.Value.Item1);
        }
    }
}
