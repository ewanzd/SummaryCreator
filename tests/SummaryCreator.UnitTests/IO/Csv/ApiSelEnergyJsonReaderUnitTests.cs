using System;
using System.Linq;
using System.Web;
using Xunit;

namespace SummaryCreator.IO.Csv.UnitTests
{
    public class ApiSelEnergyJsonReaderUnitTests
    {
        [Theory]
        [InlineData("67870006.90643704", 1598911200)]
        [InlineData("67871973.78213166", 1598913000)]
        [InlineData("67870006", 100)]
        [InlineData("0.9", 2598913000)]
        [InlineData("0", 1098913000)]
        public void Read_SingleLineOfContent(string valueStr, long timestamp)
        {
            var value = Convert.ToDouble(valueStr);
            var capturedAt = DateTimeOffset.FromUnixTimeSeconds(timestamp);

            var url = "https://api.sel.energy/api_v1/timeseries/6F5CBF66-FC55-4E44-9933-22FB3D9C2E11/Active%20Energy%20Import/";
            var content = "{\"data\":[{\"Active Energy Import\":" + valueStr + ",\"timestamp\":" + timestamp + "}]}";

            var reader = new ApiSelEnergyJsonReader();
            var timeSeries = reader.Read(url, content);

            Assert.Single(timeSeries);

            var timeSerie = timeSeries.First();

            Assert.Single(timeSeries);

            var dataPoint = timeSerie.First();

            Assert.Equal(dataPoint.Value, value, 15);
            Assert.Equal(dataPoint.CapturedAt, capturedAt);
        }
    }
}
