﻿using System;
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
        // Excepts the definition for CRLF in a field.
        // Uses ?: to minimize capturing groups.
        static readonly Regex CsvFieldPattern = new Regex("(?<=^|,)" + "(?:\"(.*?)\"|[^,]*?)" + "(?=$|,)");

        static readonly Func<string, IEnumerable<string>> SplitLine0 = line =>
            CsvFieldPattern.Matches(line)
                .Cast<Match>()
                .Select(m => m.Groups[1].Success ? m.Groups[1].Value : m.Value)
                .Select(s => s.Replace("\"\"", "\""));

        public static readonly Func<string, string[]> SplitLine = line => SplitLine0(line).ToArray();

        static readonly Regex QualifyingFieldPattern = new Regex("^.*[,\"].*$");

        public static readonly Func<IEnumerable<string>, string> ToLine = fields => string.Join(",",
            fields
                .Select(f => f.Replace("\"", "\"\""))
                .Select(f => QualifyingFieldPattern.Replace(f, "\"$&\""))
        );

        public static IEnumerable<string[]> ReadRecordsByArray(Stream stream, bool hasHeader = false, Encoding encoding = null)
        {
            return stream.ReadLines(encoding)
                .Skip(hasHeader ? 1 : 0)
                .Select(SplitLine);
        }

        public static IEnumerable<string[]> ReadRecordsByArray(string path, bool hasHeader = false, Encoding encoding = null)
        {
            using (var stream = File.OpenRead(path))
            {
                return ReadRecordsByArray(stream, hasHeader, encoding);
            }
        }

        public static void WriteRecordsByArray(Stream stream, IEnumerable<string[]> records, string[] columnNames = null, Encoding encoding = null)
        {
            if (records == null) throw new ArgumentNullException("records");

            var lines = Enumerable.Repeat(columnNames, columnNames != null ? 1 : 0)
                .Concat(records)
                .Select(ToLine);

            stream.WriteLines(lines, encoding);
        }

        public static void WriteRecordsByArray(string path, IEnumerable<string[]> records, string[] columnNames = null, Encoding encoding = null)
        {
            using (var stream = File.Create(path))
            {
                WriteRecordsByArray(stream, records, columnNames, encoding);
            }
        }

        public static IEnumerable<Dictionary<string, string>> ReadRecordsByDictionary(Stream stream, string[] columnNames, Encoding encoding = null)
        {
            if (columnNames == null) throw new ArgumentNullException("columnNames");

            return stream.ReadLines(encoding)
                .Select(SplitLine0)
                .Select(fields => fields
                    .Select((f, i) => new { c = columnNames[i], f })
                    .ToDictionary(o => o.c, o => o.f));
        }

        public static IEnumerable<Dictionary<string, string>> ReadRecordsByDictionary(string path, string[] columnNames, Encoding encoding = null)
        {
            using (var stream = File.OpenRead(path))
            {
                return ReadRecordsByDictionary(stream, columnNames, encoding);
            }
        }

        public static void WriteRecordsByDictionary(Stream stream, IEnumerable<Dictionary<string, string>> records, string[] columnNames = null, Encoding encoding = null)
        {
            if (records == null) throw new ArgumentNullException("records");

            var records2 = columnNames == null ?
                records.Select(d => d.Values) :
                Enumerable.Repeat(columnNames, 1)
                    .Concat(records.Select(d => columnNames.Select(c => d[c])));
            var lines = records2.Select(ToLine);

            stream.WriteLines(lines, encoding);
        }

        public static void WriteRecordsByDictionary(string path, IEnumerable<Dictionary<string, string>> records, string[] columnNames = null, Encoding encoding = null)
        {
            using (var stream = File.Create(path))
            {
                WriteRecordsByDictionary(stream, records, columnNames, encoding);
            }
        }
    }
}
