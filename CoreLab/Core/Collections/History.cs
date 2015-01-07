using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KLibrary.Labs.Collections
{
    public class History<T> : LimitedCollection<HistoryItem<T>>
    {
        public TimeSpan MaxSpan { get; private set; }

        public History(int maxCount, TimeSpan maxSpan)
            : base(maxCount)
        {
            MaxSpan = maxSpan;
        }

        public void Record(T value)
        {
            var now = DateTime.Now;

            while (Count > 0 && now - this[0].Timestamp > MaxSpan)
            {
                RemoveAt(0);
            }

            Record(new HistoryItem<T>(value, now));
        }
    }

    [DebuggerDisplay(@"\{{Timestamp}: {Value}\}")]
    public struct HistoryItem<T>
    {
        public T Value { get; private set; }
        public DateTime Timestamp { get; private set; }

        public HistoryItem(T value, DateTime timestamp)
            : this()
        {
            Value = value;
            Timestamp = timestamp;
        }
    }
}
