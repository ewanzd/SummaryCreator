using SummaryCreator.Data;
using System;
using Xunit;

namespace SummaryCreator.UnitTests
{
    public class SensorContainerUnitTests
    {
        [Fact]
        public void SensorContainer_Create_HasDefaultSettings()
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
    }
}
