using KLibrary.Labs.Mathematics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

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
    }
}
