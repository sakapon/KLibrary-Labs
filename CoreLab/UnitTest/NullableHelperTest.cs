using System;
using KLibrary.Labs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class NullableHelperTest
    {
        [TestMethod]
        public void IfNotNull_Action_Class()
        {
            var i = 0;
            Action<string> action = _ => i++;

            "".IfNotNull(action);
            Assert.AreEqual(1, i);
            default(string).IfNotNull(action);
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void IfNotNull_Action_Struct()
        {
            var i = 0;
            Action<int> action = _ => i++;

            ((int?)0).IfNotNull(action);
            Assert.AreEqual(1, i);
            default(int?).IfNotNull(action);
            Assert.AreEqual(1, i);
        }

        [TestMethod]
        public void IfNotNull_Func_Class()
        {
            Func<string, string> func = s => s + s;

            Assert.AreEqual("aa", "a".IfNotNull(func));
            Assert.AreEqual(null, default(string).IfNotNull(func));
        }

        [TestMethod]
        public void IfNotNull_Func_Struct()
        {
            Func<int, string> func = i => i.ToString();

            Assert.AreEqual("1", ((int?)1).IfNotNull(func));
            Assert.AreEqual(null, default(int?).IfNotNull(func));
        }

        [TestMethod]
        public void IfNotNull2_Func_Class()
        {
            Func<string, int> func = s => int.Parse(s);

            Assert.AreEqual(1, "1".IfNotNull2(func));
            Assert.AreEqual(null, default(string).IfNotNull2(func));
        }

        [TestMethod]
        public void IfNotNull2_Func_Struct()
        {
            Func<int, int> func = i => 2 * i;

            Assert.AreEqual(6, ((int?)3).IfNotNull2(func));
            Assert.AreEqual(null, default(int?).IfNotNull2(func));
        }
    }
}
