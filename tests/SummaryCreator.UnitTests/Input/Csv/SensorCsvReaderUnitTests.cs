using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;

namespace SummaryCreator.Input.Csv.UnitTests
{
    public class SensorCsvReaderUnitTests
    {
        [Fact]
        public void Read_Sensor()
        {
            var id = "2";

            var content = $";Serial number;;;;;;\n" +
                $"10.12.2019 00:00:37;{id};;;;;;;\n" +
                $"10.12.2019 00:00:38;{id};;;;;;;\n";

            var reader = new SensorCsvReader();
            var timeSeries = reader.Read(id, content);

            Assert.Single(timeSeries);

            var timeSerie1 = timeSeries.Where(x => id.Equals(x.Id)).First();

            Assert.Equal(2, timeSerie1.Count);
        }

        [Theory]
        [InlineData("2")]
        [InlineData("10.12.2019 00:00:37;2;")]
        [InlineData("2_443-7")]
        [InlineData("10.12.2019 00:00:ff;2;")]
        [InlineData("29.02.2021 00:00:37;2;")]
        [InlineData("10.12.2019 00:00:37;2;ffg")]
        [InlineData("10.12.2019 00:00:37;2;4;;f;;;;")]
        public void Read_InvalidLine_ThrowsInvalidDataException(string line)
        {
            var content = $";;;;;;;\n" +
                $"${line}\n";

            var reader = new SensorCsvReader();

            Assert.Throws<InvalidDataException>(() => reader.Read("anyId", content));
        }

        [Theory]
        [InlineData("10.12.2019 00:00:37", "40221074.98668111")]
        [InlineData("28.02.2021 00:00:00", "4")]
        [InlineData("28.02.2021 23:59:59", "0")]
        [InlineData("01.01.2019 00:00:00", "0.111")]
        [InlineData("31.12.2018 23:59:59", "6.0")]
        public void Read_FirstValueEntry(string dateTimeStr, string valueStr)
        {
            var dateTime = DateTime.ParseExact(dateTimeStr, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
            var value = double.Parse(valueStr);

            var content = $"DateTime (Local Time);Serial number;Active Energy Import Total;Unit;;;;\n" +
                $"{dateTime:dd.MM.yyyy HH:mm:ss};2;{value};;;;;\n";

            var reader = new SensorCsvReader();
            var timeSeries = reader.Read("2", content);

            Assert.Single(timeSeries);

            var timeSerie = timeSeries.First();

            Assert.Single(timeSerie);

            var dataPoint = timeSerie.First();

            Assert.Equal(dataPoint.Value, value, 15);
            Assert.Equal(dataPoint.CapturedAt, dateTime);
        }

        [Theory]
        [InlineData("10.12.2019 00:00:37", "40221074.98668111", "40221074.98668111")]
        [InlineData("10.12.2019 12:00:00", "4.", ".9")]
        [InlineData("01.01.2000 00:00:00", "0", "4.9")]
        [InlineData("31.12.1999 23:59:59", "34", "0.0")]
        public void Read_SecondAndThirdValueEntry(string dateTimeStr, string value1Str, string value2Str)
        {
            var dateTime = DateTime.ParseExact(dateTimeStr, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
            var value1 = double.Parse(value1Str);
            var value2 = double.Parse(value2Str);

            var content = $"DateTime (Local Time);Serial number;;;Energy Tariff 1;Unit;Energy Tariff 2;Unit\n" +
                $"{dateTime:dd.MM.yyyy HH:mm:ss};2;;;{value1};;{value2};\n";

            var reader = new SensorCsvReader();
            var timeSeries = reader.Read("2", content);

            Assert.Single(timeSeries);

            var timeSerie = timeSeries.First();

            Assert.Single(timeSerie);

            var dataPoint = timeSerie.First();

            Assert.Equal(dataPoint.Value, value1 + value2, 15);
            Assert.Equal(dataPoint.CapturedAt, dateTime);
        }
    }
}
