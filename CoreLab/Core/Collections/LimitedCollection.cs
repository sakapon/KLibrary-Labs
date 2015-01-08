using System;
using System.Collections.ObjectModel;

namespace KLibrary.Labs.Collections
{
    public class LimitedCollection<T> : Collection<T>
    {
        public int? MaxCount { get; private set; }

        public bool IsFull
        {
            get { return Count == MaxCount; }
        }

        public T FirstItem
        {
            get { return this[0]; }
        }

        public T LastItem
        {
            get { return this[Count - 1]; }
        }

        public LimitedCollection() { }

        public LimitedCollection(int maxCount)
        {
            if (maxCount <= 0) throw new ArgumentOutOfRangeException("maxCount", maxCount, "The value must be positive.");

            MaxCount = maxCount;
        }

        protected override void InsertItem(int index, T item)
        {
            throw new NotSupportedException();
        }

        public new void Add(T value)
        {
            if (IsFull) RemoveAt(0);

            base.InsertItem(Count, value);
        }
    }
}
