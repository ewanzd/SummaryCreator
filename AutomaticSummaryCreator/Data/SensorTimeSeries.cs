using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutomaticSummaryCreator.Data
{
    public sealed class SensorTimeSeries : IDataContainer
    {
        private readonly SortedList<DateTime, DataPoint> dataPoints = new SortedList<DateTime, DataPoint>();

        public SensorTimeSeries(string id)
        {
            Debug.Assert(string.IsNullOrEmpty(id), $"{nameof(id)} must not be null or empty");

            Id = id;
        }

        public int Count => dataPoints.Count;

        public string Id { get; private set; }

        public DataPoint First => dataPoints.Values.FirstOrDefault();

        public DataPoint Last => dataPoints.Values.LastOrDefault();

        public void Add(DataPoint dataPoint)
        {
            Debug.Assert(dataPoint != null, $"{nameof(dataPoint)} must not be null");

            dataPoints.Add(dataPoint.CapturedAt, dataPoint);
        }

        public double Total(DateTime pointInTime)
        {
            DataPoint closestPreviousDataPoint = null;

            foreach (var dataPoint in dataPoints.Values)
            {
                if(dataPoint.CapturedAt > pointInTime)
                {
                    break;
                }
                closestPreviousDataPoint = dataPoint;
            }

            return closestPreviousDataPoint == null ? 0.0 : closestPreviousDataPoint.Value;
        }

        public double Sum(DateTime start, TimeSpan range)
        {
            var end = start + range;
            return Sum(start, end);
        }

        public double Sum(DateTime start, DateTime end)
        {
            DataPoint first = null, last = null;

            foreach (var dataPoint in dataPoints.Values)
            {
                if (dataPoint.CapturedAt < end)
                {
                    last = dataPoint;
                    if(first == null && dataPoint.CapturedAt >= start)
                    {
                        first = dataPoint;
                    }
                }
                else
                {
                    break;
                }
            }
            return last == null || first == null ? 0.0 : last.Value - first.Value;
        }

        public IEnumerator<DataPoint> GetEnumerator()
        {
            return dataPoints.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
