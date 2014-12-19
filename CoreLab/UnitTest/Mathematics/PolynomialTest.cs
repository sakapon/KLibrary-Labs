using KLibrary.Labs.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Windows;

namespace UnitTest.Mathematics
{
    [TestClass]
    public class PolynomialTest
    {
        static readonly Polynomial x = Polynomial.X;
        static readonly Polynomial x2 = x ^ 2;

        [TestMethod]
        public void LinearEquationTest()
        {
            Assert.AreEqual(0.0, x.SolveLinearEquation());
            Assert.AreEqual(2.0, (x - 2).SolveLinearEquation());
            Assert.AreEqual(-0.5, (2 * x + 1).SolveLinearEquation());
        }

        [TestMethod]
        public void QuadraticEquationTest()
        {
            CollectionAssert.AreEqual(new double[0], (x2 + 1).SolveQuadraticEquation());
            CollectionAssert.AreEqual(new[] { 0.0 }, x2.SolveQuadraticEquation());
            CollectionAssert.AreEqual(new[] { 3.0 }, (x2 - 6 * x + 9).SolveQuadraticEquation());
            CollectionAssert.AreEqual(new[] { -2.0, 0.5 }, ((x + 2) * (2 * x - 1)).SolveQuadraticEquation());
            CollectionAssert.AreEqual(new[] { -0.618, 1.618 }, (x2 - x - 1).SolveQuadraticEquation().Select(d => Math.Round(d, 3, MidpointRounding.AwayFromZero)).ToArray());
            CollectionAssert.AreEqual(new[] { -1.618, 0.618 }, (x2 + x - 1).SolveQuadraticEquation().Select(d => Math.Round(d, 3, MidpointRounding.AwayFromZero)).ToArray());
        }

        [TestMethod]
        public void IntersectionTest()
        {
            // The intersection of line y = x - 1 and line y = -2x + 5.
            var l1 = x - 1;
            var l2 = -2 * x + 5;
            var p_x = (l1 - l2).SolveLinearEquation();
            var p_y = l1.Substitute(p_x);
            Assert.AreEqual(new Point(2, 1), new Point(p_x, p_y));
        }

        [TestMethod]
        public void FahrenheitTest()
        {
            Func<double, double> c_to_f = c => (x - 1.8 * c - 32).SolveLinearEquation();

            Assert.AreEqual(32.0, c_to_f(0));
            Assert.AreEqual(50.0, c_to_f(10));
            Assert.AreEqual(212.0, c_to_f(100));
        }

        [TestMethod]
        public void PointsOnLineTest()
        {
            var p1 = new Point(0, -300);
            var p2 = new Point(1800, 300);
            var y_to_x = GetFunc_y_to_x(p1, p2);
            Assert.AreEqual(900.0, y_to_x(0));
            Assert.AreEqual(600.0, y_to_x(-100));
        }

        // P1, P2 を通る直線上で、指定された y 座標に対応する x 座標を求めるための関数。
        static Func<double, double> GetFunc_y_to_x(Point p1, Point p2)
        {
            // P1 (x1, y1) および P2 (x2, y2) を通る直線の方程式:
            // (x - x1) (y2 - y1) - (x2 - x1) (y - y1) = 0
            return y => ((x - p1.X) * (p2.Y - p1.Y) - (p2.X - p1.X) * (y - p1.Y)).SolveLinearEquation();
        }

        static Func<double, double> GetFunc_y_to_x_Old(Point p1, Point p2)
        {
            return y => (p2.X - p1.X) * (y - p1.Y) / (p2.Y - p1.Y) + p1.X;
        }

        [TestMethod]
        public void Equals()
        {
            var p1 = 2 * x + 1;
            Assert.IsFalse(object.ReferenceEquals(p1, p1));

            Assert.IsFalse(object.Equals(x, null));
            Assert.IsFalse(object.Equals((Polynomial)1, 1));
            Assert.IsFalse(object.Equals(x, x + 1));
            Assert.IsTrue(object.Equals(x + 1, x + 1));

            Assert.IsFalse(x.Equals(null));
            Assert.IsFalse(((Polynomial)1).Equals(1));
            Assert.IsFalse(x.Equals(x + 1));
            Assert.IsTrue((x + 1).Equals(x + 1));

            Assert.IsFalse(x == x + 1);
            Assert.IsTrue(x + 1 == x + 1);
        }

        [TestMethod]
        public void _GetHashCode()
        {
            Assert.AreEqual((x + 1).GetHashCode(), (x + 1).GetHashCode());
            Assert.AreEqual((x + 2).GetHashCode(), (x + 2).GetHashCode());
            Assert.AreNotEqual((x + 1).GetHashCode(), (x + 2).GetHashCode());
        }
    }
}
