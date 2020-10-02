using SummaryCreator.Core;
using System;
using Xunit;

namespace SummaryCreator.UnitTests
{
    public class SensorContainerUnitTests
    {
        private const double Accuracy = 0.000001;

        [Fact]
        public void SensorContainer_Empty()
        {
            var id = "1234";
            var container = new SensorContainer(id);

            Assert.Equal(id, container.Id);
            Assert.Equal(0, container.Count);
            Assert.Null(container.First);
            Assert.Null(container.Last);
            Assert.False(container.AnyBetween(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));
            Assert.InRange(container.Sum(DateTimeOffset.MinValue, DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.InRange(container.TotalUntil(DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
        }

        [Fact]
        public void SensorContainer_OneEntry()
        {
            var dataPoint = new DataPoint()
            {
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

            Assert.InRange(container.Sum(DateTimeOffset.MinValue, new DateTime(2019, 4, 29)), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.InRange(container.Sum(new DateTime(2019, 4, 30), DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.Equal(dataPoint.Value, container.Sum(new DateTime(2019, 4, 29), new DateTime(2019, 4, 30)));

            Assert.InRange(container.TotalUntil(new DateTime(2019, 4, 29)), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.Equal(dataPoint.Value, container.TotalUntil(new DateTime(2019, 4, 30)));
            Assert.Equal(dataPoint.Value, container.TotalUntil(DateTimeOffset.MaxValue));
        }

        [Fact]
        public void SensorContainer_MultipleEntry()
        {
            var dataPoint1 = new DataPoint()
            {
                CapturedAt = new DateTime(2019, 4, 28, 12, 0, 0),
                Value = 1000
            };
            var dataPoint2 = new DataPoint()
            {
                CapturedAt = new DateTime(2019, 4, 29, 12, 0, 0),
                Value = 1200
            };
            var dataPoint3 = new DataPoint()
            {
                CapturedAt = new DateTime(2019, 4, 30, 12, 0, 0),
                Value = 1250
            };

            var id = "1234";
            var container = new SensorContainer(id);
            container.Add(dataPoint1);
            container.Add(dataPoint2);
            container.Add(dataPoint3);

            Assert.Equal(id, container.Id);
            Assert.Equal(3, container.Count);
            Assert.Equal(dataPoint1, container.First);
            Assert.Equal(dataPoint3, container.Last);
            Assert.True(container.AnyBetween(new DateTime(2019, 4, 29), new DateTime(2019, 4, 30)));
            Assert.False(container.AnyBetween(DateTimeOffset.MinValue, new DateTime(2019, 4, 27)));
            Assert.False(container.AnyBetween(new DateTime(2019, 5, 1), DateTimeOffset.MaxValue));

            Assert.InRange(container.Sum(DateTimeOffset.MinValue, new DateTime(2019, 4, 28)), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.InRange(container.Sum(new DateTime(2019, 5, 1), DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
            var sum = dataPoint3.Value - dataPoint1.Value;
            Assert.Equal(sum, container.Sum(new DateTime(2019, 4, 29), new DateTime(2019, 4, 30, 23, 0, 0)));
            Assert.InRange(container.Sum(DateTimeOffset.MinValue, new DateTime(2019, 4, 28)), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.InRange(container.Sum(new DateTime(2019, 5, 01), DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);

            Assert.InRange(container.TotalUntil(new DateTime(2019, 4, 28)), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.Equal(dataPoint1.Value, container.TotalUntil(new DateTime(2019, 4, 29)));
            Assert.Equal(dataPoint3.Value, container.TotalUntil(new DateTime(2019, 5, 01)));
            Assert.Equal(dataPoint3.Value, container.TotalUntil(DateTimeOffset.MaxValue));
        }
    }
}