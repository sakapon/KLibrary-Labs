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

        const string CsvFileName = "CsvFileTest.csv";

        static readonly string[] ColumnNames = new[] { "Id", "Name" };
        static readonly string[][] Records1 = new[]
        {
            new[] { "123", "太郎" },
            new[] { "456", "次郎" },
        };
        static readonly Dictionary<string, string>[] Records2 = new[]
        {
            new Dictionary<string, string> { { "Id", "123" }, { "Name", "太郎" } },
            new Dictionary<string, string> { { "Id", "456" }, { "Name", "次郎" } },
        };

        const string Content1 = @"123,太郎
456,次郎
";
        const string Content2 = @"Id,Name
123,太郎
456,次郎
";

        [TestMethod]
        public void ReadRecordsByArray_Empty()
        {
            using (var stream = new MemoryStream())
            {
                var actual = CsvFile.ReadRecordsByArray(stream, false).ToArray();

                Assert.AreEqual(0, actual.Length);
            }
        }

        [TestMethod]
        public void ReadRecordsByArray_1()
        {
            using (var stream = new MemoryStream(TextFile.UTF8N.GetBytes(Content1)))
            {
                var actual = CsvFile.ReadRecordsByArray(stream, false).ToArray();

                TestHelper.AreCollectionEqual(Records1, actual);
            }
        }

        [TestMethod]
        public void ReadRecordsByArray_2()
        {
            using (var stream = new MemoryStream(TextFile.UTF8N.GetBytes(Content2)))
            {
                var actual = CsvFile.ReadRecordsByArray(stream, true).ToArray();

                TestHelper.AreCollectionEqual(Records1, actual);
            }
        }

        [TestMethod]
        public void ReadRecordsByArray_ShiftJIS()
        {
            using (var stream = new MemoryStream(TextFile.ShiftJIS.GetBytes(Content2)))
            {
                var actual = CsvFile.ReadRecordsByArray(stream, true, TextFile.ShiftJIS).ToArray();

                TestHelper.AreCollectionEqual(Records1, actual);
            }
        }

        [TestMethod]
        public void ReadRecordsByArray_Path_1()
        {
            File.WriteAllBytes(CsvFileName, TextFile.UTF8N.GetBytes(Content1));

            var actual = CsvFile.ReadRecordsByArray(CsvFileName, false).ToArray();

            TestHelper.AreCollectionEqual(Records1, actual);
        }

        [TestMethod]
        public void WriteRecordsByArray_Empty()
        {
            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByArray(stream, Enumerable.Empty<string[]>());

                Assert.AreEqual(0, stream.ToArray().Length);
            }
        }

        [TestMethod]
        public void WriteRecordsByArray_1()
        {
            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByArray(stream, Records1);

                CollectionAssert.AreEqual(TextFile.UTF8N.GetBytes(Content1), stream.ToArray());
            }
        }

        [TestMethod]
        public void WriteRecordsByArray_2()
        {
            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByArray(stream, Records1, ColumnNames);

                CollectionAssert.AreEqual(TextFile.UTF8N.GetBytes(Content2), stream.ToArray());
            }
        }

        [TestMethod]
        public void WriteRecordsByArray_ShiftJIS()
        {
            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByArray(stream, Records1, ColumnNames, TextFile.ShiftJIS);

                CollectionAssert.AreEqual(TextFile.ShiftJIS.GetBytes(Content2), stream.ToArray());
            }
        }

        [TestMethod]
        public void WriteRecordsByArray_Path_1()
        {
            CsvFile.WriteRecordsByArray(CsvFileName, Records1);

            var bytes_actual = File.ReadAllBytes(CsvFileName);
            CollectionAssert.AreEqual(TextFile.UTF8N.GetBytes(Content1), bytes_actual);
        }

        [TestMethod]
        public void ReadRecordsByDictionary_Empty()
        {
            using (var stream = new MemoryStream())
            {
                var actual = CsvFile.ReadRecordsByDictionary(stream).ToArray();

                Assert.AreEqual(0, actual.Length);
            }
        }

        [TestMethod]
        public void ReadRecordsByDictionary_1()
        {
            using (var stream = new MemoryStream(TextFile.UTF8N.GetBytes(Content2)))
            {
                var actual = CsvFile.ReadRecordsByDictionary(stream).ToArray();

                for (var i = 0; i < Records2.Length; i++)
                    DictionaryAssert(Records2[i], actual[i]);
            }
        }

        [TestMethod]
        public void ReadRecordsByDictionary_ShiftJIS()
        {
            using (var stream = new MemoryStream(TextFile.ShiftJIS.GetBytes(Content2)))
            {
                var actual = CsvFile.ReadRecordsByDictionary(stream, TextFile.ShiftJIS).ToArray();

                for (var i = 0; i < Records2.Length; i++)
                    DictionaryAssert(Records2[i], actual[i]);
            }
        }

        [TestMethod]
        public void WriteRecordsByDictionary_Empty()
        {
            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByDictionary(stream, Enumerable.Empty<Dictionary<string, string>>());

                Assert.AreEqual(0, stream.ToArray().Length);
            }
        }

        [TestMethod]
        public void WriteRecordsByDictionary_1()
        {
            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByDictionary(stream, Records2);

                CollectionAssert.AreEqual(TextFile.UTF8N.GetBytes(Content1), stream.ToArray());
            }
        }

        [TestMethod]
        public void WriteRecordsByDictionary_2()
        {
            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByDictionary(stream, Records2, ColumnNames);

                CollectionAssert.AreEqual(TextFile.UTF8N.GetBytes(Content2), stream.ToArray());
            }
        }

        [TestMethod]
        public void WriteRecordsByDictionary_ShiftJIS()
        {
            using (var stream = new MemoryStream())
            {
                CsvFile.WriteRecordsByDictionary(stream, Records2, ColumnNames, TextFile.ShiftJIS);

                CollectionAssert.AreEqual(TextFile.ShiftJIS.GetBytes(Content2), stream.ToArray());
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
