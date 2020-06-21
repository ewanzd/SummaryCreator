using SummaryCreator.Core;
using System;
using System.Collections.Generic;
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
            Assert.False(group.AnyBetween(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));
            Assert.InRange(group.Sum(DateTimeOffset.MinValue, DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.InRange(group.Total(DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
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
            Assert.False(group.AnyBetween(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));
            Assert.InRange(group.Sum(DateTimeOffset.MinValue, DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
            Assert.InRange(group.Total(DateTimeOffset.MaxValue), 0.0 - Accuracy, 0.0 + Accuracy);
        }

        [Fact]
        public void DataGroup_AddNullElements_Throws()
        {
            var group = new ContainerGroup();

            Assert.Throws<ArgumentNullException>(() => group.Add(null));
            Assert.Throws<ArgumentNullException>(() => group.AddRange(null));
            Assert.Throws<ArgumentException>(() => group.AddRange(new List<IContainer>() { null }));
        }
    }
}