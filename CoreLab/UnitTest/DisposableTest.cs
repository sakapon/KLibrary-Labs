using System;
using KLibrary.Labs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class DisposableTest
    {
        [TestMethod]
        public void FromAction()
        {
            var count = 0;
            using (var action = Disposable.FromAction(() => count++))
            {
                Assert.AreEqual(0, count);
            }
            Assert.AreEqual(1, count);
        }
    }
}
