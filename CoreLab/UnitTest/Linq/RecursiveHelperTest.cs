using System;
using System.IO;
using System.Linq;
using KLibrary.Labs;
using KLibrary.Labs.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Linq
{
    [TestClass]
    public class RecursiveHelperTest
    {
        [TestMethod]
        public void EnumerateRecursively_Seq()
        {
            var actual = 1
                .EnumerateRecursively(i => i + 2)
                .TakeWhile(i => i < 10)
                .ToArray();
            var expected = new[] { 1, 3, 5, 7, 9 };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnumerateRecursively_Parents()
        {
            var query = Environment.CurrentDirectory
                .EnumerateRecursively(p =>
                {
                    var di = Directory.GetParent(p);
                    return di != null ? di.FullName : null;
                })
                .TakeWhile(p => p != null);

            foreach (var path in query)
            {
                Console.WriteLine(path);
            }
        }

        [TestMethod]
        public void EnumerateRecursively_Children()
        {
            var query = Path.GetFullPath(@"..\..\")
                .EnumerateRecursively(p => Directory.EnumerateDirectories(p));

            foreach (var path in query)
            {
                Console.WriteLine(path);
            }
        }
    }
}
