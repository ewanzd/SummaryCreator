using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SummaryCreator.Core
{
    /// <summary>
    /// Container for meteo data.
    /// </summary>
    public sealed class MeteoContainer : IContainer
    {
        private readonly SortedList<DateTimeOffset, DataPoint> dataPoints = new SortedList<DateTimeOffset, DataPoint>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentException"><paramref name="id"/> must contain a valid string value (not null or white space).</exception>
        public MeteoContainer(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException($"{nameof(id)} must contain a value.");

            Id = id;
        }

        public string Id { get; private set; }

        public int Count => dataPoints.Count;

        public DataPoint First => dataPoints.Values.FirstOrDefault();

        public DataPoint Last => dataPoints.Values.LastOrDefault();

        public void Add(DataPoint dataPoint)
        {
            if (dataPoint == null) throw new ArgumentNullException(nameof(dataPoint));

            dataPoints.Add(dataPoint.CapturedAt, dataPoint);
        }

        public bool AnyBetween(DateTimeOffset start, DateTimeOffset end)
        {
            if (start > end || dataPoints.Count == 0)
            {
                return false;
            }

            // if start date is after the last entry or the end date before first entry
            // then there is no entry in time range
            var keys = dataPoints.Keys;
            if (keys.First() > end || start > keys.Last())
            {
                return false;
            }

            return dataPoints.Keys.Any(capturedAt => start <= capturedAt && capturedAt < end);
        }

        public double Sum(DateTimeOffset start, TimeSpan range)
        {
            var end = start + range;
            return Sum(start, end);
        }

        public double Sum(DateTimeOffset start, DateTimeOffset end)
        {
            var total = 0.0;

            foreach (var dataPoint in dataPoints.Values)
            {
                if (dataPoint.CapturedAt < start)
                {
                    continue;
                }
                if (dataPoint.CapturedAt >= end)
                {
                    break;
                }
                total += dataPoint.Value;
            }

            return total;
        }

        public double Total(DateTimeOffset pointInTime)
        {
            var total = 0.0;

            foreach (var dataPoint in dataPoints.Values)
            {
                if (dataPoint.CapturedAt >= pointInTime)
                {
                    break;
                }
                total += dataPoint.Value;
            }

            return total;
        }

        public IEnumerator<DataPoint> GetEnumerator()
        {
            return dataPoints.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return Id;
        }
    }
}