using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KLibrary.Labs.IO;
using KLibrary.Labs.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.IO
{
    [TestClass]
    public class CsvFileTest
    {
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

        [TestMethod]
        public void ReadWriteRecordsByArray_1()
        {
            var records = new[]
            {
                new[] { "123", "太郎" },
                new[] { "456", "次郎" },
            };
            var content = @"123,太郎
456,次郎
";

            using (var stream = new MemoryStream(TextFile.UTF8N.GetBytes(content)))
            {
                var records_actual = CsvFile.ReadRecordsByArray(stream, false).ToArray();

                for (var i = 0; i < records.Length; i++)
                    CollectionAssert.AreEqual(records[i], records_actual[i]);
            }

            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByArray(stream, records);

                CollectionAssert.AreEqual(TextFile.UTF8N.GetBytes(content), stream.ToArray());
            }
        }

        [TestMethod]
        public void ReadWriteRecordsByArray_2()
        {
            var columnNames = new[] { "Id", "Name" };
            var records = new[]
            {
                new[] { "123", "太郎" },
                new[] { "456", "次郎" },
            };
            var content = @"Id,Name
123,太郎
456,次郎
";

            using (var stream = new MemoryStream(TextFile.UTF8N.GetBytes(content)))
            {
                var records_actual = CsvFile.ReadRecordsByArray(stream, true).ToArray();

                for (var i = 0; i < records.Length; i++)
                    CollectionAssert.AreEqual(records[i], records_actual[i]);
            }

            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByArrayWithColumnNames(stream, records, columnNames);

                CollectionAssert.AreEqual(TextFile.UTF8N.GetBytes(content), stream.ToArray());
            }
        }

        [TestMethod]
        public void ReadWriteRecordsByArray_3()
        {
            var columnNames = new[] { "Id", "Name" };
            var records = new[]
            {
                new[] { "123", "太郎" },
                new[] { "456", "次郎" },
            };
            var content = @"Id,Name
123,太郎
456,次郎
";

            using (var stream = new MemoryStream(TextFile.ShiftJIS.GetBytes(content)))
            {
                var records_actual = CsvFile.ReadRecordsByArray(stream, true, TextFile.ShiftJIS).ToArray();

                for (var i = 0; i < records.Length; i++)
                    CollectionAssert.AreEqual(records[i], records_actual[i]);
            }

            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByArrayWithColumnNames(stream, records, columnNames, TextFile.ShiftJIS);

                CollectionAssert.AreEqual(TextFile.ShiftJIS.GetBytes(content), stream.ToArray());
            }
        }

        [TestMethod]
        public void ReadWriteRecordsByDictionary_1()
        {
            var columnNames = new[] { "Id", "Name" };
            var records = new[]
            {
                new Dictionary<string, string> { { "Id", "123" }, { "Name", "太郎" } },
                new Dictionary<string, string> { { "Id", "456" }, { "Name", "次郎" } },
            };
            var content = @"Id,Name
123,太郎
456,次郎
";

            using (var stream = new MemoryStream(TextFile.UTF8N.GetBytes(content)))
            {
                var records_actual = CsvFile.ReadRecordsByDictionary(stream).ToArray();

                for (var i = 0; i < records.Length; i++)
                    DictionaryAssert(records[i], records_actual[i]);
            }

            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByDictionary(stream, records, columnNames);

                CollectionAssert.AreEqual(TextFile.UTF8N.GetBytes(content), stream.ToArray());
            }
        }

        [TestMethod]
        public void ReadWriteRecordsByDictionary_2()
        {
            var columnNames = new[] { "Id", "Name" };
            var records = new[]
            {
                new Dictionary<string, string> { { "Id", "123" }, { "Name", "太郎" } },
                new Dictionary<string, string> { { "Id", "456" }, { "Name", "次郎" } },
            };
            var content = @"Id,Name
123,太郎
456,次郎
";

            using (var stream = new MemoryStream(TextFile.ShiftJIS.GetBytes(content)))
            {
                var records_actual = CsvFile.ReadRecordsByDictionary(stream, TextFile.ShiftJIS).ToArray();

                for (var i = 0; i < records.Length; i++)
                    DictionaryAssert(records[i], records_actual[i]);
            }

            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByDictionary(stream, records, columnNames, TextFile.ShiftJIS);

                CollectionAssert.AreEqual(TextFile.ShiftJIS.GetBytes(content), stream.ToArray());
            }
        }

        static void DictionaryAssert<TKey, TValue>(Dictionary<TKey, TValue> expected, Dictionary<TKey, TValue> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            expected.Zip(actual, (e, a) => new { e, a })
                .Execute(_ => KeyValuePairAssert(_.e, _.a));
        }

        static void KeyValuePairAssert<TKey, TValue>(KeyValuePair<TKey, TValue> expected, KeyValuePair<TKey, TValue> actual)
        {
            Assert.AreEqual(expected.Key, actual.Key);
            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}
