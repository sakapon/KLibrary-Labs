﻿using System;
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

        public LimitedCollection(int? maxCount)
        {
            if (maxCount <= 0) throw new ArgumentOutOfRangeException("maxCount", maxCount, "The value must be positive or null.");

            MaxCount = maxCount;
        }

        public void Record(T value)
        {
            if (IsFull) RemoveAt(0);

            Add(value);
        }
    }
}
