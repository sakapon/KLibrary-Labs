using System;

namespace KLibrary.Labs
{
    public struct OldNewValues<T>
    {
        public T OldValue { get; private set; }
        public T NewValue { get; private set; }

        public OldNewValues(T defaultValue)
            : this()
        {
            OldValue = defaultValue;
            NewValue = defaultValue;
        }

        public void UpdateValue(T newValue)
        {
            OldValue = NewValue;
            NewValue = newValue;
        }
    }
}
