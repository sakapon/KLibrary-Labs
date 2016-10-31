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
    }
}
