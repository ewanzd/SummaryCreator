using System;
using System.Linq;
using Xunit;

namespace SummaryCreator.IO.Csv.UnitTests
{
    public class DbdataSensorCsvReaderUnitTests
    {
        [Theory]
        [InlineData("dbdata_123.csv", "123")]
        [InlineData("dbdata_456", "456")]
        [InlineData("dbdata_6E5CBF4B-FC2F-4E67-99A6-3CFB3D1C2E46.csv", "6E5CBF4B-FC2F-4E67-99A6-3CFB3D1C2E46")]
        [InlineData("test_123-456-789.zzz", "123-456-789")]
        public void Read_ExtractId(string fileName, string id)
        {
            var reader = new DbdataSensorCsvReader();

            var timeSeries = reader.Read(fileName, string.Empty);

            Assert.Single(timeSeries);

            var timeSerie = timeSeries.First();

            Assert.Empty(timeSerie);
            Assert.Equal(id, timeSerie.Id);
        }

        [Theory]
        [InlineData("dbdata.csv")]
        [InlineData("dbdata__.csv")]
        [InlineData("dbdata_ ")]
        [InlineData(".bbb")]
        [InlineData("")]
        public void Read_ExtractId_ThrowsArgumentException(string fileName)
        {
            var reader = new DbdataSensorCsvReader();

            Assert.Throws<ArgumentException>(() => reader.Read(fileName, string.Empty));
        }

        [Fact]
        public void Read_ContentWithTitleOnly()
        {
            var content = "value,capturedAt";

            var reader = new DbdataSensorCsvReader();
            var timeSeries = reader.Read("abc_123.csv", content);

            Assert.Single(timeSeries);

            var timeSerie = timeSeries.First();

            Assert.Empty(timeSerie);
        }

        [Theory]
        [InlineData("40221074.98668111", "2018-12-15 23:00:00")]
        [InlineData("0.01", "2020-12-31 23:59:59")]
        [InlineData("4", "2021-01-01 00:00:00")]
        [InlineData("40221074.98668111456356", "2020-02-29 01:00:00")]
        public void Read_SingleLineOfContent(string valueStr, string dateTimeStr)
        {
            var value = double.Parse(valueStr);
            var dateTime = DateTime.Parse(dateTimeStr);
            var content = $"value,capturedAt\n{valueStr},{dateTimeStr}";

            var reader = new DbdataSensorCsvReader();
            var timeSeries = reader.Read("abc_123.csv", content);

            Assert.Single(timeSeries);

            var timeSerie = timeSeries.First();

            Assert.Single(timeSerie);

            var dataPoint = timeSerie.First();

            Assert.Equal(dataPoint.Value, value, 15);
            Assert.Equal(dataPoint.CapturedAt, dateTime);
        }

        [Fact]
        public void Read_MultiLineOfContent()
        {
            var content = $"value,capturedAt\n" +
                "40439006.983199716,2017-11-19 04:15:00\n" +
                "40439231.96553905,2017-11-19 04:30:00\n" +
                "40439522.98288488,2017-11-19 04:45:00\n" +
                "40439809.98479772,2017-11-19 05:00:00";

            var reader = new DbdataSensorCsvReader();
            var timeSeries = reader.Read("abc_123.csv", content);

            Assert.Single(timeSeries);

            var timeSerie = timeSeries.First();

            Assert.Equal(4, timeSerie.Count);
        }
    }
}
