using KLibrary.Labs.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KLibrary.Labs.Mathematics
{
    public class FrequencyConditioner
    {
        const int DefaultMaxCount = 100;
        static readonly TimeSpan DefaultMaxSpan = TimeSpan.FromSeconds(1);

        History history;

        public double BaseFrequency { get; set; }
        public double Frequency { get; private set; }

        public FrequencyConditioner(double baseFrequency) : this(baseFrequency, DefaultMaxCount, DefaultMaxSpan) { }

        public FrequencyConditioner(double baseFrequency, int historyMaxCount, TimeSpan historyMaxSpan)
        {
            BaseFrequency = baseFrequency;
            history = new History(historyMaxCount, historyMaxSpan);
        }

        public bool CheckLap()
        {
            history.Record();

            Frequency = FrequencyMeter.GetFrequency(history);
            return Frequency == 0 || Frequency >= BaseFrequency;
        }
    }
}
