using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KLibrary.Labs.IO
{
    /// <summary>
    /// Provides a set of methods to access CSV files.
    /// </summary>
    /// <remarks>
    /// RFC 4180
    /// https://www.ietf.org/rfc/rfc4180.txt
    /// </remarks>
    public static class CsvFile
    {
        public static IEnumerable<string[]> ReadRecords(Stream stream, Encoding encoding = null)
        {
            return stream.ReadLines(encoding)
                .Select(SplitLine);
        }

        public static IEnumerable<string[]> ReadRecords(string path, Encoding encoding = null)
        {
            using (var stream = File.OpenRead(path))
            {
                return ReadRecords(stream, encoding);
            }
        }

        public static IEnumerable<Dictionary<string, string>> ReadRecords(Stream stream, string[] columnNames, Encoding encoding = null)
        {
            if (columnNames == null) throw new ArgumentNullException("columnNames");

            return stream.ReadLines(encoding)
                .Select(SplitLine0)
                .Select(fields => fields
                    .Select((f, i) => new { c = columnNames[i], f })
                    .ToDictionary(o => o.c, o => o.f));
        }

        public static IEnumerable<Dictionary<string, string>> ReadRecords(string path, string[] columnNames, Encoding encoding = null)
        {
            using (var stream = File.OpenRead(path))
            {
                return ReadRecords(stream, columnNames, encoding);
            }
        }

        // Excepts the definition for CRLF in a field.
        // Uses ?: to minimize capturing groups.
        static readonly Regex CsvFieldPattern = new Regex("(?<=^|,)" + "(?:\"(.*?)\"|[^,]*?)" + "(?=$|,)");

        static readonly Func<string, IEnumerable<string>> SplitLine0 = line =>
            CsvFieldPattern.Matches(line)
                .Cast<Match>()
                .Select(m => m.Groups[1].Success ? m.Groups[1].Value : m.Value)
                .Select(s => s.Replace("\"\"", "\""));

        public static readonly Func<string, string[]> SplitLine = line => SplitLine0(line).ToArray();

        public static void WriteRecords(Stream stream, string[] columnNames, IEnumerable<Dictionary<string, string>> records, Encoding encoding = null)
        {
            if (columnNames == null) throw new ArgumentNullException("columnNames");
            if (records == null) throw new ArgumentNullException("records");

            var lines = Enumerable.Repeat(columnNames, 1)
                .Concat(records.Select(r => columnNames.Select(c => r[c])))
                .Select(ToLine);

            stream.WriteLines(lines, encoding);
        }

        public static void WriteRecords(string path, string[] columnNames, IEnumerable<Dictionary<string, string>> records, Encoding encoding = null)
        {
            using (var stream = File.Create(path))
            {
                WriteRecords(stream, columnNames, records, encoding);
            }
        }

        static readonly Regex EscapingFieldPattern = new Regex("^.*[,\"].*$");

        public static readonly Func<IEnumerable<string>, string> ToLine = fields => string.Join(",",
            fields
                .Select(f => f.Replace("\"", "\"\""))
                .Select(f => EscapingFieldPattern.Replace(f, "\"$&\""))
        );
    }

    public static class TextFile
    {
        public static readonly Encoding UTF8N = new UTF8Encoding();
        public static readonly Encoding ShiftJIS = Encoding.GetEncoding("shift_jis");

        public static IEnumerable<string> ReadLines(this Stream stream, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            using (var reader = new StreamReader(stream, encoding ?? UTF8N))
            {
                while (!reader.EndOfStream)
                    yield return reader.ReadLine();
            }
        }

        public static void WriteLines(this Stream stream, IEnumerable<string> lines, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            using (var writer = new StreamWriter(stream, encoding ?? UTF8N))
            {
                foreach (var line in lines)
                    writer.WriteLine(line);
            }
        }
    }
}
