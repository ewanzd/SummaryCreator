using System;
using System.Collections.Generic;

namespace SummaryCreator.Core
{
    /// <summary>
    /// Time series of data points.
    /// </summary>
    public interface ITimeSeries : IEnumerable<DataPoint>
    {
        /// <summary>
        /// Id of sensor.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Total amount of data points in this time series.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// First data point in time.
        /// </summary>
        DataPoint First { get; }

        /// <summary>
        /// Last data point in time.
        /// </summary>
        DataPoint Last { get; }

        /// <summary>
        /// Whether there are any data points inside specified time range.
        /// </summary>
        /// <param name="start">First date time of time range.</param>
        /// <param name="end">Last date time of time range.</param>
        /// <returns>Whether there are any data points inside specified time range.</returns>
        bool AnyBetween(DateTimeOffset start, DateTimeOffset end);

        /// <summary>
        /// Add a new data point to time series.
        /// </summary>
        /// <param name="dataPoint">Data point to add.</param>
        void Add(DataPoint dataPoint);

        /// <summary>
        /// Amount of value created in this specified time range.
        /// </summary>
        /// <param name="start">First date time of time range.</param>
        /// <param name="timespan">Time range from <paramref name="start"/>.</param>
        /// <returns>Amount of value created in this specified time range.</returns>
        double Sum(DateTimeOffset start, TimeSpan timespan);

        /// <summary>
        /// Amount of value created in this specified time range.
        /// </summary>
        /// <param name="start">First date time of time range.</param>
        /// <param name="end">Last date time of time range.</param>
        /// <returns>Amount of value created in this specified time range.</returns>
        double Sum(DateTimeOffset start, DateTimeOffset end);

        /// <summary>
        /// All value created until this point in time.
        /// </summary>
        /// <param name="pointInTime">Total value until this point in time.</param>
        /// <returns>All value created until this point in time.</returns>
        double TotalUntil(DateTimeOffset pointInTime);
    }
}