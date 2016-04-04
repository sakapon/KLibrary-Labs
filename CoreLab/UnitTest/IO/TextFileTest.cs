using System;
using System.IO;
using System.Linq;
using KLibrary.Labs.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.IO
{
    [TestClass]
    public class TextFileTest
    {
        const string TextFileName = "TextFileTest.txt";

        static readonly string[] Lines = new[] { "123", "あいう" };
        const string Content1 = @"123
あいう
";
        const string Content2 = @"123
あいう";
        const string Content3 = "123\nあいう\n";

        [TestMethod]
        public void ReadLines_Empty()
        {
            using (var stream = new MemoryStream())
            {
                var actual = stream.ReadLines().ToArray();

                Assert.AreEqual(0, actual.Length);
            }
        }

        [TestMethod]
        public void ReadLines_1()
        {
            using (var stream = new MemoryStream(TextFile.UTF8N.GetBytes(Content1)))
            {
                var actual = stream.ReadLines().ToArray();

                for (var i = 0; i < Lines.Length; i++)
                    Assert.AreEqual(Lines[i], actual[i]);
            }
        }

        [TestMethod]
        public void ReadLines_2()
        {
            using (var stream = new MemoryStream(TextFile.UTF8N.GetBytes(Content2)))
            {
                var actual = stream.ReadLines().ToArray();

                for (var i = 0; i < Lines.Length; i++)
                    Assert.AreEqual(Lines[i], actual[i]);
            }
        }

        [TestMethod]
        public void ReadLines_3()
        {
            using (var stream = new MemoryStream(TextFile.UTF8N.GetBytes(Content3)))
            {
                var actual = stream.ReadLines().ToArray();

                for (var i = 0; i < Lines.Length; i++)
                    Assert.AreEqual(Lines[i], actual[i]);
            }
        }

        [TestMethod]
        public void ReadLines_ShiftJIS()
        {
            using (var stream = new MemoryStream(TextFile.ShiftJIS.GetBytes(Content1)))
            {
                var actual = stream.ReadLines(TextFile.ShiftJIS).ToArray();

                for (var i = 0; i < Lines.Length; i++)
                    Assert.AreEqual(Lines[i], actual[i]);
            }
        }

        [TestMethod]
        public void ReadLines_Path_Empty()
        {
            File.WriteAllBytes(TextFileName, new byte[0]);

            var actual = TextFile.ReadLines(TextFileName).ToArray();

            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod]
        public void ReadLines_Path_1()
        {
            File.WriteAllBytes(TextFileName, TextFile.UTF8N.GetBytes(Content1));

            var actual = TextFile.ReadLines(TextFileName).ToArray();

            for (var i = 0; i < Lines.Length; i++)
                Assert.AreEqual(Lines[i], actual[i]);
        }

        [TestMethod]
        public void WriteLines_Empty()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteLines(Enumerable.Empty<string>());

                Assert.AreEqual(0, stream.ToArray().Length);
            }
        }

        [TestMethod]
        public void WriteLines_1()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteLines(Lines);

                CollectionAssert.AreEqual(TextFile.UTF8N.GetBytes(Content1), stream.ToArray());
            }
        }

        [TestMethod]
        public void WriteLines_ShiftJIS()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteLines(Lines, TextFile.ShiftJIS);

                CollectionAssert.AreEqual(TextFile.ShiftJIS.GetBytes(Content1), stream.ToArray());
            }
        }

        [TestMethod]
        public void WriteLines_Path_Empty()
        {
            TextFile.WriteLines(TextFileName, Enumerable.Empty<string>());

            var bytes_actual = File.ReadAllBytes(TextFileName);
            Assert.AreEqual(0, bytes_actual.Length);
        }

        [TestMethod]
        public void WriteLines_Path_1()
        {
            TextFile.WriteLines(TextFileName, Lines);

            var bytes_actual = File.ReadAllBytes(TextFileName);
            CollectionAssert.AreEqual(TextFile.UTF8N.GetBytes(Content1), bytes_actual);
        }
    }
}
