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
        public static readonly Encoding UTF8N = new UTF8Encoding();

        public static IEnumerable<Dictionary<string, string>> ReadRecords(Stream stream, Encoding encoding = null)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            var lines = ReadLines(stream, encoding ?? UTF8N).Select(SplitLine0);
            string[] columnNames = null;

            foreach (var fields in lines)
            {
                if (columnNames == null)
                    columnNames = fields.ToArray();
                else
                    yield return fields.Select((f, i) => new { c = columnNames[i], f }).ToDictionary(o => o.c, o => o.f);
            }
        }

        public static IEnumerable<Dictionary<string, string>> ReadRecords(string path, Encoding encoding = null)
        {
            using (var stream = File.OpenRead(path))
            {
                return ReadRecords(stream, encoding);
            }
        }

        static IEnumerable<string> ReadLines(Stream stream, Encoding encoding)
        {
            using (var reader = new StreamReader(stream, encoding))
            {
                while (!reader.EndOfStream)
                    yield return reader.ReadLine();
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

        public static void WriteLines(Stream stream, IEnumerable<string[]> lines, Encoding encoding = null)
        {
            using (var writer = new StreamWriter(stream, encoding ?? UTF8N))
            {
                foreach (var line in lines)
                    writer.WriteLine(string.Join(",", line));
            }
        }

        public static void WriteLines(Stream stream, IEnumerable<Dictionary<string, string>> lines, Encoding encoding = null)
        {
            var isColumnsWritten = false;

            using (var writer = new StreamWriter(stream, encoding ?? UTF8N))
            {
                foreach (var line in lines)
                {
                    if (!isColumnsWritten)
                    {
                        isColumnsWritten = true;
                        writer.WriteLine(string.Join(",", line.Keys));
                    }

                    writer.WriteLine(string.Join(",", line.Values));
                }
            }
        }
    }
}
