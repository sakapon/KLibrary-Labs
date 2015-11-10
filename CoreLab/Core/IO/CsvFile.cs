using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KLibrary.Labs.IO
{
    public static class CsvFile
    {
        public static readonly Encoding UTF8N = new UTF8Encoding();

        public static IEnumerable<Dictionary<string, string>> ReadLines(string path, Encoding encoding = null)
        {
            // Simple implementation.
            var lines = File.ReadLines(path, encoding ?? UTF8N).Select(l => l.Split(','));
            string[] columnNames = null;

            foreach (var line in lines)
            {
                if (columnNames == null)
                    columnNames = line;
                else
                    yield return columnNames.Zip(line, (c, v) => new { c, v }).ToDictionary(o => o.c, o => o.v);
            }
        }

        // Uses ?: to minimize capturing groups.
        static readonly Regex CsvFieldPattern = new Regex("(?<=^|,)" + "(?:\"(.*?)\"|[^,]*?)" + "(?=$|,)");

        static readonly Func<string, IEnumerable<string>> SplitLine0 = line =>
            CsvFieldPattern.Matches(line)
                .Cast<Match>()
                .Select(m => m.Groups[1].Success ? m.Groups[1].Value : m.Value)
                .Select(s => s.Replace("\"\"", "\""));

        public static readonly Func<string, string[]> SplitLine = line => SplitLine0(line).ToArray();
    }
}
