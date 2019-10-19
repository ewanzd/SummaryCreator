using SummaryCreator.Data;
using System;
using Xunit;

namespace SummaryCreator.UnitTests
{
    public class DataGroupUnitTests
    {
        private const double Accuracy = 0.000001;

        [Fact]
        public void DataGroup_Empty()
        {
            var group = new ContainerGroup();

            Assert.Equal(0, group.Count);
            Assert.Null(group.FirstDataPoint);
            Assert.Null(group.LastDataPoint);
            Assert.False(group.AnyBetween(DateTime.MinValue, DateTime.MaxValue));
            Assert.InRange(group.Sum(DateTime.MinValue, DateTime.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.InRange(group.Total(DateTime.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
        }

        [Fact]
        public void DataGroup_OneEmptyContainer()
        {
            var group = new ContainerGroup();

            var container = new SensorContainer("1234");
            group.Add(container);

            Assert.Equal(1, group.Count);
            Assert.Null(group.FirstDataPoint);
            Assert.Null(group.LastDataPoint);
            Assert.False(group.AnyBetween(DateTime.MinValue, DateTime.MaxValue));
            Assert.InRange(group.Sum(DateTime.MinValue, DateTime.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.InRange(group.Total(DateTime.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
        }
    }
}