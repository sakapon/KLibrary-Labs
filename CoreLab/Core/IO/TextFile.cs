﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KLibrary.Labs.IO
{
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
