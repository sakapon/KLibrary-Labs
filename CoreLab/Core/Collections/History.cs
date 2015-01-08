using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KLibrary.Labs.Collections
{
    public class History : LimitedCollection<DateTime>
    {
        public TimeSpan? MaxSpan { get; private set; }

        public History() { }
        public History(int maxCount) : base(maxCount) { }

        public History(TimeSpan maxSpan)
        {
            MaxSpan = maxSpan;
        }

        public History(int maxCount, TimeSpan maxSpan)
            : base(maxCount)
        {
            MaxSpan = maxSpan;
        }

        public void Record()
        {
            var now = DateTime.Now;

            if (MaxSpan.HasValue)
            {
                while (Count > 0 && now - this[0] >= MaxSpan)
                {
                    RemoveAt(0);
                }
            }

            Record(now);
        }
    }

    public class History<T> : LimitedCollection<HistoryItem<T>>
    {
        public TimeSpan? MaxSpan { get; private set; }

        public History() { }
        public History(int maxCount) : base(maxCount) { }

        public History(TimeSpan maxSpan)
        {
            MaxSpan = maxSpan;
        }

        public History(int maxCount, TimeSpan maxSpan)
            : base(maxCount)
        {
            MaxSpan = maxSpan;
        }

        public void Record(T value)
        {
            var now = DateTime.Now;

            if (MaxSpan.HasValue)
            {
                while (Count > 0 && now - this[0].Timestamp >= MaxSpan)
                {
                    RemoveAt(0);
                }
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
