using System;
using System.Collections.Generic;

namespace SummaryCreator.Core
{
    /// <summary>
    /// Container for data points of a specific sensor.
    /// </summary>
    public interface IContainer : IEnumerable<DataPoint>
    {
        /// <summary>
        /// Id of sensor.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Total amount of data points in this container.
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
        /// <param name="startDateTime">First date time of time range.</param>
        /// <param name="endDateTime">Last date time of time range.</param>
        /// <returns>Whether there are any data points inside specified time range.</returns>
        bool AnyBetween(DateTime startDateTime, DateTime endDateTime);

        /// <summary>
        /// Add a new data point to container.
        /// </summary>
        /// <param name="dataPoint">Data point to add.</param>
        void Add(DataPoint dataPoint);

        /// <summary>
        /// Amount of value created in this specified time range.
        /// </summary>
        /// <param name="startDateTime">First date time of time range.</param>
        /// <param name="range">Time range from <paramref name="startDateTime"/>.</param>
        /// <returns>Amount of value created in this specified time range.</returns>
        double Sum(DateTime startDateTime, TimeSpan range);

        /// <summary>
        /// Amount of value created in this specified time range.
        /// </summary>
        /// <param name="startDateTime">First date time of time range.</param>
        /// <param name="endDateTime">Last date time of time range.</param>
        /// <returns>Amount of value created in this specified time range.</returns>
        double Sum(DateTime startDateTime, DateTime endDateTime);

        /// <summary>
        /// All value created until this point in time.
        /// </summary>
        /// <param name="pointInTime">Total value until this point in time.</param>
        /// <returns>All value created until this point in time.</returns>
        double Total(DateTime pointInTime);
    }
}