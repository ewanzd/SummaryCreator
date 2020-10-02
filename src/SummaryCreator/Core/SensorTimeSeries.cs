using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SummaryCreator.Core
{
    /// <summary>
    /// Time series for sensor data.
    /// </summary>
    public sealed class SensorTimeSeries : ITimeSeries
    {
        private readonly SortedList<DateTimeOffset, DataPoint> dataPoints = new SortedList<DateTimeOffset, DataPoint>();

        /// <summary>
        /// Create a new time series with sensor data points.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentException"><paramref name="id"/> must contain a valid string value (not null or white space).</exception>
        public SensorTimeSeries(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException($"{nameof(id)} must contain a value.");

            Id = id;
        }

        public int Count => dataPoints.Count;

        public string Id { get; }

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
            if (keys[0] > end || start > keys.Last())
            {
                return false;
            }

            return dataPoints.Keys.Any(capturedAt => start <= capturedAt && capturedAt < end);
        }

        public double TotalUntil(DateTimeOffset pointInTime)
        {
            var closestPreviousDataPoint = FindClosestPreviousDataPoint(pointInTime);
            return (closestPreviousDataPoint?.Value) ?? 0.0;
        }

        private DataPoint FindClosestPreviousDataPoint(DateTimeOffset pointInTime)
        {
            DataPoint closestPreviousDataPoint = null;
            foreach (var dataPoint in dataPoints.Values)
            {
                if (dataPoint.CapturedAt > pointInTime)
                {
                    break;
                }
                closestPreviousDataPoint = dataPoint;
            }
            return closestPreviousDataPoint;
        }

        private DataPoint FindClosestNextDataPoint(DateTimeOffset pointInTime)
        {
            foreach (var dataPoint in dataPoints.Values)
            {
                if (dataPoint.CapturedAt > pointInTime)
                {
                    return dataPoint;
                }
            }
            return null;
        }

        public double Sum(DateTimeOffset start, TimeSpan range)
        {
            var end = start + range;
            return Sum(start, end);
        }

        public double Sum(DateTimeOffset start, DateTimeOffset end)
        {
            DataPoint first = FindClosestPreviousDataPoint(start);
            DataPoint last = FindClosestPreviousDataPoint(end);

            if (first == null && last == null)
            {
                return 0.0;
            }
            else if (first == null && last != null)
            {
                first = FindClosestNextDataPoint(start);
                if (first == last)
                {
                    return last.Value;
                }
            }
            return last.Value - first.Value;
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