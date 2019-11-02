﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SummaryCreator.Core
{
    public sealed class SensorContainer : IContainer
    {
        private readonly SortedList<DateTime, DataPoint> dataPoints = new SortedList<DateTime, DataPoint>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentException"><paramref name="id"/> must contain a valid string value (not null or white space).</exception>
        public SensorContainer(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException($"{nameof(id)} must contain a value.");

            Id = id;
        }

        public int Count => dataPoints.Count;

        public string Id { get; private set; }

        public DataPoint First => dataPoints.Values.FirstOrDefault();

        public DataPoint Last => dataPoints.Values.LastOrDefault();

        public void Add(DataPoint dataPoint)
        {
            if (dataPoint == null) throw new ArgumentNullException(nameof(dataPoint));

            dataPoints.Add(dataPoint.CapturedAt, dataPoint);
        }

        public bool AnyBetween(DateTime start, DateTime end)
        {
            if (start < end || dataPoints.Count == 0)
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

        public double Total(DateTime pointInTime)
        {
            var closestPreviousDataPoint = FindClosestPreviousDataPoint(pointInTime);
            return closestPreviousDataPoint == null ? 0.0 : closestPreviousDataPoint.Value;
        }

        private DataPoint FindClosestPreviousDataPoint(DateTime pointInTime)
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

        private DataPoint FindClosestNextDataPoint(DateTime pointInTime)
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

        public double Sum(DateTime start, TimeSpan range)
        {
            var end = start + range;
            return Sum(start, end);
        }

        public double Sum(DateTime start, DateTime end)
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