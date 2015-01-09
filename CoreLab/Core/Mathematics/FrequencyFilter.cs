using KLibrary.Labs.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KLibrary.Labs.Mathematics
{
    public class FrequencyFilter
    {
        const int DefaultMaxCount = 100;
        static readonly TimeSpan DefaultMaxSpan = TimeSpan.FromSeconds(1);

        History history;

        public double MaxFrequency { get; set; }
        public double ArrangedFrequency { get; private set; }

        public FrequencyFilter(double maxFrequency) : this(maxFrequency, DefaultMaxCount, DefaultMaxSpan) { }

        public FrequencyFilter(double maxFrequency, int historyMaxCount, TimeSpan historyMaxSpan)
        {
            MaxFrequency = maxFrequency;
            history = new History(historyMaxCount, historyMaxSpan);
        }

        public bool CheckLap()
        {
            history.Record();

            var frequency = FrequencyMeter.GetFrequency(history);
            var isAvailable = frequency <= MaxFrequency;
            if (isAvailable)
            {
                ArrangedFrequency = frequency;
            }
            else
            {
                history.RemoveAt(history.Count - 1);
            }

            return isAvailable;
        }
    }
}
