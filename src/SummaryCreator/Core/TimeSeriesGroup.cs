using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SummaryCreator.Core
{
    /// <summary>
    /// Manage a group of time series.
    /// </summary>
    public sealed class TimeSeriesGroup : IEnumerable<ITimeSeries>
    {
        private readonly List<ITimeSeries> timeSeries = new List<ITimeSeries>();

        /// <summary>
        /// Get time series by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ITimeSeries this[string id] {
            get {
                if (id == null) return null;

                return timeSeries.Find(c => c.Id.Equals(id, StringComparison.InvariantCulture));
            }
        }

        public int Count {
            get {
                return timeSeries.Count;
            }
        }

        /// <summary>
        /// Find first <see cref="DataPoint"/>. Property return null if no data found.
        /// </summary>
        public DataPoint FirstDataPoint {
            get {
                if (timeSeries.Count == 0)
                {
                    return null;
                }
                return timeSeries.Select(x => x.First).Aggregate((dpMin, x) => (dpMin == null || (x?.CapturedAt ?? DateTime.MaxValue) < dpMin.CapturedAt ? x : dpMin));
            }
        }

        /// <summary>
        /// Find last <see cref="DataPoint"/>. Property return null if no data found.
        /// </summary>
        public DataPoint LastDataPoint {
            get {
                if (timeSeries.Count == 0)
                {
                    return null;
                }
                return timeSeries.Select(x => x.Last).Aggregate((dpMax, x) => (dpMax == null || (x?.CapturedAt ?? DateTime.MinValue) > dpMax.CapturedAt ? x : dpMax));
            }
        }

        /// <summary>
        /// Add a time series to the group.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <exception cref="ArgumentNullException">Null is not allowed.</exception>
        public void Add(ITimeSeries timeSeries)
        {
            if (timeSeries == null) throw new ArgumentNullException(nameof(timeSeries));

            this.timeSeries.Add(timeSeries);
        }

        /// <summary>
        /// Add multiple time series at once to the group.
        /// </summary>
        /// <param name="timeSeries"></param>
        /// <exception cref="ArgumentNullException">Null is not allowed.</exception>
        /// <exception cref="ArgumentException">IEnumerable contains null values.</exception>
        public void AddRange(IEnumerable<ITimeSeries> timeSeries)
        {
            if (timeSeries == null) throw new ArgumentNullException(nameof(timeSeries));
            if (timeSeries.Any(x => x == null)) throw new ArgumentException($"{nameof(timeSeries)} cannot contain null values.");

            this.timeSeries.AddRange(timeSeries);
        }

        public bool AnyBetween(DateTimeOffset start, DateTimeOffset end)
        {
            return timeSeries.Any(timeSeries => timeSeries.AnyBetween(start, end));
        }

        /// <summary>
        /// Calculate sum of all values in time range.
        /// </summary>
        /// <param name="start">Start date time to get sum.</param>
        /// <param name="end">End date time to get sum.</param>
        /// <returns>Sum of all values in time range.</returns>
        public double Sum(DateTimeOffset start, DateTimeOffset end)
        {
            return timeSeries.Sum(x => x.Sum(start, end));
        }

        public double TotalUntil(DateTimeOffset pointInTime)
        {
            return timeSeries.Sum(x => x.TotalUntil(pointInTime));
        }

        public IEnumerator<ITimeSeries> GetEnumerator()
        {
            return timeSeries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}