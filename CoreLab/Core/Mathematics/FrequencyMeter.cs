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

        public FrequencyMeter() : this(DefaultMaxCount, DefaultMaxSpan) { }

        public FrequencyMeter(int historyMaxCount, TimeSpan historyMaxSpan)
        {
            history = new History(historyMaxCount, historyMaxSpan);
        }

        public double RecordLap()
        {
            history.Record();

            return (Frequency = GetFrequency(history));
        }

        internal static double GetFrequency(History history)
        {
            if (history.Count < 2) return 0;

            return (history.Count - 1) / (history.LastItem - history.FirstItem).TotalSeconds;
        }
    }
}
