using KLibrary.Labs.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTest.Pipeline
{
    [TestClass]
    public class ObjectHelperTest
    {
        [TestMethod]
        public void DoAction_01()
        {
            Action action = () => Console.WriteLine("Event Handlers");
            action.DoAction(Usual.IsNotNull, Usual.DoAction);

            var count = 0;
            Action action2;
            action2 = null;
            action2.DoAction(Usual.IsNotNull, a => count++);
            Assert.AreEqual(0, count);
            action2 = () => { };
            action2.DoAction(Usual.IsNotNull, a => count++);
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void DoAction_02()
        {
            Action<int> WriteEvenOdd = n =>
                n.DoAction(i => i % 2 == 0, i => Console.WriteLine("Even"), i => Console.WriteLine("Odd"));

            foreach (var i in Enumerable.Range(1, 30))
            {
                i.DoAction(WriteEvenOdd);
            }
        }

        [TestMethod]
        public void DoFunc()
        {
            Func<int, string> FizzBuzz = n =>
                n.DoFunc(Convert.ToString,
                    Case.ForFunc((int i) => i <= 0, new ArgumentOutOfRangeException("i").ToFunc<int, string>()),
                    Case.ForFunc((int i) => i % 15 == 0, _ => "FizzBuzz"),
                    Case.ForFunc((int i) => i % 5 == 0, _ => "Buzz"),
                    Case.ForFunc((int i) => i % 3 == 0, _ => "Fizz"));

            foreach (var i in Enumerable.Range(1, 30))
            {
                i
                    .DoFunc(FizzBuzz)
                    .DoAction(Console.WriteLine);
            }
        }

        [TestMethod]
        public void Fallback()
        {
            Func<int?, string> ToText = i => i.Fallback(default(int?), _ => -1).ToString();
            Assert.AreEqual("-1", ToText(null));
            Assert.AreEqual("-1", ToText(-1));
            Assert.AreEqual("-2", ToText(-2));

            Func<string, string> Fill = s => s.Fallback(Usual.IsNullOrWhiteSpace, _ => "N/A");
            Assert.AreEqual("N/A", Fill(null));
            Assert.AreEqual("N/A", Fill(""));
            Assert.AreEqual("N/A", Fill(" "));
            Assert.AreEqual("N/A", Fill("　"));
            Assert.AreEqual("N/A", Fill("N/A"));
            Assert.AreEqual("abc", Fill("abc"));
        }
    }
}
