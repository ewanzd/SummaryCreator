using SummaryCreator.Configuration;
using SummaryCreator.IO.Csv;
using System;
using Xunit;

namespace SummaryCreator.IO.UnitTests
{
    public class TimeSeriesReaderFactoryUnitTests
    {
        [Fact]
        public void CreateSensorReader_SensorConfigIsNull_ThrowsException()
        {
            var factory = new TimeSeriesReaderFactory();

            Assert.Throws<ArgumentNullException>(() => factory.CreateSensorReader(null));
        }

        [Fact]
        public void CreateSensorReader_ResourceIsNull_ThrowsException()
        {
            var factory = new TimeSeriesReaderFactory();

            var config = new SensorConfig();
            Assert.Throws<ArgumentException>(() => factory.CreateSensorReader(config));
        }

        [Theory]
        [InlineData("zeus.csv")]
        [InlineData("zeus.abc")]
        [InlineData("zeus")]
        [InlineData("z/e/u/s.csv")]
        public void CreateSensorReader_ReturnSensorCsvReader(string resource)
        {
            var factory = new TimeSeriesReaderFactory();

            var config = new SensorConfig()
            {
                Resource = resource
            };

            var sensorReader = factory.CreateSensorReader(config);
            Assert.IsType<SensorCsvReader>(sensorReader);
        }

        [Theory]
        [InlineData("dbdata_apollo.csv")]
        [InlineData("dbdata.csv")]
        [InlineData("apollo/dbdata.csv")]
        [InlineData("dbdata_sel_meter_apollo.hij")]
        public void CreateSensorReader_ReturnDbdataSensorCsvReader(string resource)
        {
            var factory = new TimeSeriesReaderFactory();

            var config = new SensorConfig()
            {
                Resource = resource
            };

            var sensorReader = factory.CreateSensorReader(config);
            Assert.IsType<DbdataSensorCsvReader>(sensorReader);
        }

        [Theory]
        [InlineData(SensorContentFormat.Unknown, typeof(SensorCsvReader))]
        [InlineData(SensorContentFormat.Sel, typeof(SensorCsvReader))]
        [InlineData(SensorContentFormat.Selv2, typeof(DbdataSensorCsvReader))]
        public void CreateSensorReader_ReturnReaderBySensorContentFormat(SensorContentFormat format, Type type)
        {
            var factory = new TimeSeriesReaderFactory();

            var config = new SensorConfig()
            {
                Format = format,
                Resource = "any"
            };

            var sensorReader = factory.CreateSensorReader(config);
            Assert.IsType(type, sensorReader);
        }
    }
}
