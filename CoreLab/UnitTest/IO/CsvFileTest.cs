using System;
using KLibrary.Labs.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.IO
{
    [TestClass]
    public class CsvFileTest
    {
        [TestMethod]
        public void ReadRecords_1()
        {
        }

        [TestMethod]
        public void SplitLine_1()
        {
            CollectionAssert.AreEqual(new[] { "" }, CsvFile.SplitLine(""));
            CollectionAssert.AreEqual(new[] { "0" }, CsvFile.SplitLine("0"));

            CollectionAssert.AreEqual(new[] { "", "" }, CsvFile.SplitLine(","));
            CollectionAssert.AreEqual(new[] { "0", "1" }, CsvFile.SplitLine("0,1"));
            CollectionAssert.AreEqual(new[] { "", "1" }, CsvFile.SplitLine(",1"));
            CollectionAssert.AreEqual(new[] { "0", "" }, CsvFile.SplitLine("0,"));

            CollectionAssert.AreEqual(new[] { "", "", "" }, CsvFile.SplitLine(",,"));
            CollectionAssert.AreEqual(new[] { "0", "1", "2" }, CsvFile.SplitLine("0,1,2"));
            CollectionAssert.AreEqual(new[] { "", "1", "" }, CsvFile.SplitLine(",1,"));
            CollectionAssert.AreEqual(new[] { "0", "", "2" }, CsvFile.SplitLine("0,,2"));
        }

        [TestMethod]
        public void SplitLine_2()
        {
            CollectionAssert.AreEqual(new[] { "" }, CsvFile.SplitLine("\"\""));
            CollectionAssert.AreEqual(new[] { "0" }, CsvFile.SplitLine("\"0\""));

            CollectionAssert.AreEqual(new[] { "", "" }, CsvFile.SplitLine("\"\",\"\""));
            CollectionAssert.AreEqual(new[] { "0", "1" }, CsvFile.SplitLine("\"0\",\"1\""));
            CollectionAssert.AreEqual(new[] { "", "1" }, CsvFile.SplitLine("\"\",\"1\""));
            CollectionAssert.AreEqual(new[] { "0", "" }, CsvFile.SplitLine("\"0\",\"\""));

            CollectionAssert.AreEqual(new[] { "", "", "" }, CsvFile.SplitLine("\"\",\"\",\"\""));
            CollectionAssert.AreEqual(new[] { "0", "1", "2" }, CsvFile.SplitLine("\"0\",\"1\",\"2\""));
            CollectionAssert.AreEqual(new[] { "", "1", "" }, CsvFile.SplitLine("\"\",\"1\",\"\""));
            CollectionAssert.AreEqual(new[] { "0", "", "2" }, CsvFile.SplitLine("\"0\",\"\",\"2\""));
        }

        [TestMethod]
        public void SplitLine_3()
        {
            CollectionAssert.AreEqual(new[] { "," }, CsvFile.SplitLine("\",\""));
            CollectionAssert.AreEqual(new[] { "0," }, CsvFile.SplitLine("\"0,\""));
            CollectionAssert.AreEqual(new[] { ",0" }, CsvFile.SplitLine("\",0\""));

            CollectionAssert.AreEqual(new[] { ",0", "1," }, CsvFile.SplitLine("\",0\",\"1,\""));

            CollectionAssert.AreEqual(new[] { "", ",", "2,2" }, CsvFile.SplitLine("\"\",\",\",\"2,2\""));
        }

        [TestMethod]
        public void SplitLine_4()
        {
            CollectionAssert.AreEqual(new[] { "\"" }, CsvFile.SplitLine("\"\"\""));
            CollectionAssert.AreEqual(new[] { "\"" }, CsvFile.SplitLine("\"\"\"\""));

            CollectionAssert.AreEqual(new[] { "0\"", "\"1" }, CsvFile.SplitLine("\"0\"\"\",\"\"1\""));

            CollectionAssert.AreEqual(new[] { "\"", "", "2\"2," }, CsvFile.SplitLine("\"\"\"\",\"\",\"2\"2,\""));
        }

        [TestMethod]
        public void SplitLine_4_Invalid()
        {
            CollectionAssert.AreEqual(new[] { "\"" }, CsvFile.SplitLine("\""));
            CollectionAssert.AreEqual(new[] { "0\"" }, CsvFile.SplitLine("0\""));
            CollectionAssert.AreEqual(new[] { "\"0" }, CsvFile.SplitLine("\"0"));
            CollectionAssert.AreEqual(new[] { "0\"0" }, CsvFile.SplitLine("0\"0"));
            CollectionAssert.AreEqual(new[] { "0\"0" }, CsvFile.SplitLine("0\"\"0"));

            CollectionAssert.AreEqual(new[] { "0\"", "\"1" }, CsvFile.SplitLine("0\"\",\"1"));

            CollectionAssert.AreEqual(new[] { "\"", "", "2\"2" }, CsvFile.SplitLine("\",,2\"2"));
        }

        [TestMethod]
        public void ToLine_1()
        {
            Assert.AreEqual("", CsvFile.ToLine(new string[0]));
            Assert.AreEqual("", CsvFile.ToLine(new[] { "" }));
            Assert.AreEqual("0", CsvFile.ToLine(new[] { "0" }));

            Assert.AreEqual(",", CsvFile.ToLine(new[] { "", "" }));
            Assert.AreEqual("0,1", CsvFile.ToLine(new[] { "0", "1" }));
            Assert.AreEqual(",1", CsvFile.ToLine(new[] { "", "1" }));
            Assert.AreEqual("0,", CsvFile.ToLine(new[] { "0", "" }));

            Assert.AreEqual(",,", CsvFile.ToLine(new[] { "", "", "" }));
            Assert.AreEqual("0,1,2", CsvFile.ToLine(new[] { "0", "1", "2" }));
            Assert.AreEqual(",1,", CsvFile.ToLine(new[] { "", "1", "" }));
            Assert.AreEqual("0,,2", CsvFile.ToLine(new[] { "0", "", "2" }));
        }

        [TestMethod]
        public void ToLine_3()
        {
            Assert.AreEqual("\",\"", CsvFile.ToLine(new[] { "," }));
            Assert.AreEqual("\"0,\"", CsvFile.ToLine(new[] { "0," }));
            Assert.AreEqual("\",0\"", CsvFile.ToLine(new[] { ",0" }));

            Assert.AreEqual("\",0\",\"1,\"", CsvFile.ToLine(new[] { ",0", "1," }));

            Assert.AreEqual(",\",\",\"2,2\"", CsvFile.ToLine(new[] { "", ",", "2,2" }));
        }

        [TestMethod]
        public void ToLine_4()
        {
            Assert.AreEqual("\"\"\"\"", CsvFile.ToLine(new[] { "\"" }));

            Assert.AreEqual("\"0\"\"\",\"\"\"1\"", CsvFile.ToLine(new[] { "0\"", "\"1" }));

            Assert.AreEqual("\"\"\"\",,\"2\"\"2,\"", CsvFile.ToLine(new[] { "\"", "", "2\"2," }));
        }
    }
}
