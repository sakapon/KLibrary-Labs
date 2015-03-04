using System;
using System.Collections.Generic;
using System.Linq;
using KLibrary.Labs.Collections;

namespace KLibrary.Labs.Mathematics
{
    /// <summary>
    /// Measures the frequency.
    /// </summary>
    /// <remarks>
    /// Call the <see cref="RecordLap"/> method on each frame.
    /// </remarks>
    public class FrequencyMeter
    {
        const int DefaultMaxCount = 100;
        static readonly TimeSpan DefaultMaxSpan = TimeSpan.FromSeconds(1);

        History history;

        /// <summary>
        /// Gets the frequency in which the <see cref="RecordLap"/> method was called.
        /// </summary>
        public double Frequency { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyMeter"/> class.
        /// </summary>
        public FrequencyMeter() : this(DefaultMaxCount, DefaultMaxSpan) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyMeter"/> class using the specified options.
        /// </summary>
        /// <param name="historyMaxCount">The maximum count of the history items.</param>
        /// <param name="historyMaxSpan">The maximum time span of the history.</param>
        public FrequencyMeter(int historyMaxCount, TimeSpan historyMaxSpan)
        {
            history = new History(historyMaxCount, historyMaxSpan);
        }

        /// <summary>
        /// Records a time stamp, and measures the frequency.
        /// </summary>
        /// <returns>The measured frequency.</returns>
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
