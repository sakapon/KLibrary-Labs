using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace KLibrary.Labs.Collections
{
    public class History<T> : Collection<HistoryItem<T>>
    {
        public int MaxCount { get; private set; }

        public bool IsFull
        {
            get { return Count == MaxCount; }
        }

        public History(int maxCount)
        {
            if (maxCount <= 0) throw new ArgumentOutOfRangeException("maxCount", maxCount, "The value must be positive.");

            MaxCount = maxCount;
        }

        public void Record(T value)
        {
            if (IsFull) RemoveAt(0);

            Add(new HistoryItem<T>(value));
        }
    }

    [DebuggerDisplay(@"\{{Timestamp}: {Value}\}")]
    public struct HistoryItem<T>
    {
        public DateTime Timestamp { get; private set; }
        public T Value { get; private set; }

        public HistoryItem(T value)
            : this()
        {
            Timestamp = DateTime.Now;
            Value = value;
        }
    }
}
