using KLibrary.Labs.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KLibrary.Labs.Mathematics
{
    public class FrequencyMeter
    {
        const int MaxCount = 100;
        static readonly TimeSpan MaxSpan = TimeSpan.FromSeconds(1);

        History<bool> history;

        public double Frequency { get; private set; }

        public double PeriodSeconds
        {
            get { return 1 / Frequency; }
        }

        public FrequencyMeter()
        {
            history = new History<bool>(MaxCount, MaxSpan);
        }

        public double Record()
        {
            history.Record(false);

            if (history.Count < 2) return (Frequency = 0);

            var frequency = (history.Count - 1) / (history.LastItem.Timestamp - history.FirstItem.Timestamp).TotalSeconds;
            return (Frequency = frequency);
        }
    }
}
