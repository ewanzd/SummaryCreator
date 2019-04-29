using SummaryCreator.Data;
using System;
using Xunit;

namespace SummaryCreator.UnitTests
{
    public class SensorContainerUnitTests
    {
        [Fact]
        public void SensorContainer_Empty()
        {
            var id = "1234";
            var container = new SensorContainer(id);

            Assert.Equal(id, container.Id);
            Assert.Equal(0, container.Count);
            Assert.Null(container.First);
            Assert.Null(container.Last);
            Assert.False(container.AnyBetween(DateTime.MinValue, DateTime.MaxValue));
            Assert.InRange(container.Sum(DateTime.MinValue, DateTime.MaxValue), 0.0 - 0.0001, 0.0 + 0.0001);
            Assert.InRange(container.Total(DateTime.MaxValue), 0.0 - 0.0001, 0.0 + 0.0001);
        }

        [Fact]
        public void SensorContainer_OneEntry()
        {
            var dataPoint = new DataPoint() {
                CapturedAt = new DateTime(2019, 4, 29, 12, 0, 0),
                Value = 1000
            };

            var id = "1234";
            var container = new SensorContainer(id);
            container.Add(dataPoint);

            Assert.Equal(id, container.Id);
            Assert.Equal(1, container.Count);
            Assert.Equal(dataPoint, container.First);
            Assert.Equal(dataPoint, container.Last);
            Assert.True(container.AnyBetween(new DateTime(2019, 4, 29), new DateTime(2019, 4, 30)));

            Assert.InRange(container.Sum(DateTime.MinValue, new DateTime(2019, 4, 29)), 0.0 - 0.0001, 0.0 + 0.0001);
            Assert.InRange(container.Sum(new DateTime(2019, 4, 30), DateTime.MaxValue), 0.0 - 0.0001, 0.0 + 0.0001);
            Assert.Equal(dataPoint.Value, container.Sum(new DateTime(2019, 4, 29), new DateTime(2019, 4, 30)));

            Assert.InRange(container.Total(new DateTime(2019, 4, 29)), 0.0 - 0.0001, 0.0 + 0.0001);
            Assert.Equal(dataPoint.Value, container.Total(new DateTime(2019, 4, 30)));
            Assert.Equal(dataPoint.Value, container.Total(DateTime.MaxValue));
        }
    }
}
