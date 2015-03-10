using System;
using System.Windows.Media;
using KLibrary.Labs.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.UI
{
    [TestClass]
    public class ColorsHelperTest
    {
        [TestMethod]
        public void ToHeatColor()
        {
            Assert.AreEqual(Colors.Blue, ColorsHelper.ToHeatColor(0.0));
            Assert.AreEqual(Colors.Cyan, ColorsHelper.ToHeatColor(0.25));
            Assert.AreEqual(Color.FromRgb(0, 255, 0), ColorsHelper.ToHeatColor(0.5));
            Assert.AreEqual(Colors.Yellow, ColorsHelper.ToHeatColor(0.75));
            Assert.AreEqual(Colors.Red, ColorsHelper.ToHeatColor(1.0));
        }
    }
}
