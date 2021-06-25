using System;
using System.Linq;
using Xunit;

namespace SummaryCreator.Input.Csv.UnitTests
{
    public class DbdataSensorCsvReaderUnitTests
    {
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
