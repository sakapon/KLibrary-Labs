using KLibrary.Labs.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KLibrary.Labs.Mathematics
{
    public class FrequencyMeter
    {
        const int DefaultMaxCount = 100;
        static readonly TimeSpan DefaultMaxSpan = TimeSpan.FromSeconds(1);

        History history;

        public double Frequency { get; private set; }

        public double PeriodSeconds
        {
            get { return 1 / Frequency; }
        }

        public FrequencyMeter()
        {
            history = new History(DefaultMaxCount, DefaultMaxSpan);
        }

        public FrequencyMeter(int historyMaxCount, TimeSpan historyMaxSpan)
        {
            history = new History(historyMaxCount, historyMaxSpan);
        }

        public double Record()
        {
            history.Record();

            if (history.Count < 2) return (Frequency = 0);

            var frequency = (history.Count - 1) / (history.LastItem - history.FirstItem).TotalSeconds;
            return (Frequency = frequency);
        }
    }
}
