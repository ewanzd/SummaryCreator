using SummaryCreator.Configuration;
using SummaryCreator.Input.Csv;
using System;
using Xunit;

namespace SummaryCreator.Input.UnitTests
{
    public class TimeSeriesReaderFactoryUnitTests
    {
        [Fact]
        public void CreateSensorReader_SensorConfigIsNull_ThrowsException()
        {
            var factory = new TimeSeriesReaderFactory();

            Assert.Throws<ArgumentNullException>(() => factory.CreateSensorReader(null));
        }

        [Theory]
        [InlineData(EnergySourceFormat.Sel, typeof(SensorCsvReader))]
        [InlineData(EnergySourceFormat.Selv2, typeof(DbdataSensorCsvReader))]
        public void CreateSensorReader_ReturnReaderBySensorContentFormat(EnergySourceFormat format, Type type)
        {
            var factory = new TimeSeriesReaderFactory();

            var config = new EnergyConfig("any", "any", format);

            var sensorReader = factory.CreateSensorReader(config);
            Assert.IsType(type, sensorReader);
        }
    }
}
