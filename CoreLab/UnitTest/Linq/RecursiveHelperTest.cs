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
            var query1 = Environment.CurrentDirectory
                .ToMaybe()
                .EnumerateRecursively(path => path
                    .Select(p => Directory.GetParent(p))
                    .Select(d => d.FullName))
                .TakeWhile(path => path.HasValue)
                .Select(path => path.Value);
            foreach (var path in query1)
            {
                Console.WriteLine(path);
            }

            var query2 = new DirectoryInfo(Environment.CurrentDirectory)
                .EnumerateRecursively(d => d.Parent)
                .TakeWhile(d => d != null);
            foreach (var dir in query2)
            {
                Console.WriteLine(dir.FullName);
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

        [TestMethod]
        public void EnumerateRecursively_Children_Index()
        {
            var query = new { Index = 0, Path = Path.GetFullPath(@"..\..\") }
                .EnumerateRecursively(_ => Directory.EnumerateDirectories(_.Path)
                    .Select(p => new { Index = _.Index + 1, Path = p }));

            foreach (var item in query)
            {
                Console.WriteLine("{0}: {1}", item.Index, item.Path);
            }
        }

        [TestMethod]
        public void EnumerateRecursively2_Children_Index()
        {
            var query = new { Index = 0, Path = Path.GetFullPath(@"..\..\") }
                .EnumerateRecursively2(_ => Directory.EnumerateDirectories(_.Path)
                    .Select(p => new { Index = _.Index + 1, Path = p }));

            foreach (var item in query)
            {
                Console.WriteLine("{0}: {1}", item.Index, item.Path);
            }
        }
    }
}
