using System;
using System.Collections.Generic;
using Xunit;

namespace SummaryCreator.Core.UnitTests
{
    public class TimeSeriesGroupUnitTests
    {
        private const double Accuracy = 0.000001;

        [Fact]
        public void TimeSeriesGroup_Empty()
        {
            var group = new TimeSeriesGroup();

            Assert.Equal(0, group.Count);
            Assert.Null(group.FirstDataPoint);
            Assert.Null(group.LastDataPoint);
            Assert.False(group.AnyBetween(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));
            Assert.InRange(group.Sum(DateTimeOffset.MinValue, DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.InRange(group.TotalUntil(DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
        }

        [Fact]
        public void TimeSeriesGroup_OneEmptyTimeSeries()
        {
            var group = new TimeSeriesGroup();

            var container = new SensorTimeSeries("1234");
            group.Add(container);

            Assert.Equal(1, group.Count);
            Assert.Null(group.FirstDataPoint);
            Assert.Null(group.LastDataPoint);
            Assert.False(group.AnyBetween(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));
            Assert.InRange(group.Sum(DateTimeOffset.MinValue, DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.InRange(group.TotalUntil(DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
        }

        [Fact]
        public void TimeSeriesGroup_AddNullElements_Throws()
        {
            var group = new TimeSeriesGroup();

            Assert.Throws<ArgumentNullException>(() => group.Add(null));
            Assert.Throws<ArgumentNullException>(() => group.AddRange(null));
            Assert.Throws<ArgumentException>(() => group.AddRange(new List<ITimeSeries>() { null }));
        }
    }
}