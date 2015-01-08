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

        public double ArrangedPeriodSeconds
        {
            get { return 1 / ArrangedFrequency; }
        }

        public FrequencyFilter(double maxFrequency) : this(maxFrequency, DefaultMaxCount, DefaultMaxSpan) { }

        public FrequencyFilter(double maxFrequency, int historyMaxCount, TimeSpan historyMaxSpan)
        {
            MaxFrequency = maxFrequency;
            history = new History(historyMaxCount, historyMaxSpan);
        }

        public bool Check()
        {
            history.Record();

            if (history.Count < 2)
            {
                ArrangedFrequency = 0;
                return true;
            }

            var frequency = FrequencyMeter.GetFrequency(history);
            var isActive = frequency <= MaxFrequency;
            if (!isActive)
            {
                history.RemoveAt(history.Count - 1);
                frequency = history.Count < 2 ? 0 : FrequencyMeter.GetFrequency(history);
            }

            ArrangedFrequency = frequency;
            return isActive;
        }
    }
}
