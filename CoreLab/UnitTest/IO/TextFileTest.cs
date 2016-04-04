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
        static readonly string[] Lines = new[] { "123", "あいう" };
        const string Content1 = @"123
あいう
";
        const string Content2 = @"123
あいう";

        [TestMethod]
        public void ReadLines_1()
        {
            using (var stream = new MemoryStream(TextFile.UTF8N.GetBytes(Content1)))
            {
                var records_actual = stream.ReadLines().ToArray();

                for (var i = 0; i < Lines.Length; i++)
                    Assert.AreEqual(Lines[i], records_actual[i]);
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
    }
}
