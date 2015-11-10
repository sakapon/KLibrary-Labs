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
            CollectionAssert.AreEqual(new[] { "0\"0" }, CsvFile.SplitLine("0\"0"));
            CollectionAssert.AreEqual(new[] { "0\"0" }, CsvFile.SplitLine("0\"\"0"));

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
            CollectionAssert.AreEqual(new[] { "," }, CsvFile.SplitLine("\",\""));
            CollectionAssert.AreEqual(new[] { "\"" }, CsvFile.SplitLine("\"\"\""));
            CollectionAssert.AreEqual(new[] { ",\"" }, CsvFile.SplitLine("\",\"\""));

            CollectionAssert.AreEqual(new[] { "", "" }, CsvFile.SplitLine("\"\",\"\""));
            CollectionAssert.AreEqual(new[] { "0", "1" }, CsvFile.SplitLine("\"0\",\"1\""));
            CollectionAssert.AreEqual(new[] { "", "," }, CsvFile.SplitLine("\"\",\",\""));
            CollectionAssert.AreEqual(new[] { "\"", "" }, CsvFile.SplitLine("\"\"\",\"\""));

            CollectionAssert.AreEqual(new[] { "", "", "" }, CsvFile.SplitLine("\"\",\"\",\"\""));
            CollectionAssert.AreEqual(new[] { "0", "1", "2" }, CsvFile.SplitLine("\"0\",\"1\",\"2\""));
            CollectionAssert.AreEqual(new[] { "", ",", "" }, CsvFile.SplitLine("\"\",\",\",\"\""));
            CollectionAssert.AreEqual(new[] { "\"", "", "\"" }, CsvFile.SplitLine("\"\"\",\"\",\"\"\""));
        }
    }
}
