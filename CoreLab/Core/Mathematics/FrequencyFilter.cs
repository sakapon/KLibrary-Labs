using System;
using System.Collections.Generic;
using System.Linq;
using KLibrary.Labs.Collections;

namespace KLibrary.Labs.Mathematics
{
    /// <summary>
    /// Represents a filter to suppress the frequency.
    /// </summary>
    /// <remarks>
    /// Call the <see cref="CheckLap"/> method on each frame, then <see cref="ArrangedFrequency"/> will be recalculated.
    /// The return value of the <see cref="CheckLap"/> method indicates whether the frame is available.
    /// </remarks>
    public class FrequencyFilter
    {
        const int DefaultMaxCount = 100;
        static readonly TimeSpan DefaultMaxSpan = TimeSpan.FromSeconds(1);

        History history;

        /// <summary>
        /// Gets or sets the maximum frequency.
        /// </summary>
        public double MaxFrequency { get; set; }

        /// <summary>
        /// Gets the arranged frequency. The value is less than or equal to <see cref="MaxFrequency"/>.
        /// </summary>
        public double ArrangedFrequency { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyFilter"/> class.
        /// </summary>
        /// <param name="maxFrequency">The maximum frequency.</param>
        public FrequencyFilter(double maxFrequency) : this(maxFrequency, DefaultMaxCount, DefaultMaxSpan) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrequencyFilter"/> class using the specified options.
        /// </summary>
        /// <param name="maxFrequency">The maximum frequency.</param>
        /// <param name="historyMaxCount">The maximum count of the history items.</param>
        /// <param name="historyMaxSpan">The maximum time span of the history.</param>
        public FrequencyFilter(double maxFrequency, int historyMaxCount, TimeSpan historyMaxSpan)
        {
            MaxFrequency = maxFrequency;
            history = new History(historyMaxCount, historyMaxSpan);
        }

        /// <summary>
        /// Records a time stamp, and filters the current lap.
        /// </summary>
        /// <returns><see langword="true"/> if the current lap is available; otherwise, <see langword="false"/>.</returns>
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
