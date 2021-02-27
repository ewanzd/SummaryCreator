using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SummaryCreator.Configuration.Json.UnitTests
{
    public class JsonConfigurationConverterUniTests
    {
        [Fact]
        public async Task ConvertAsync_Empty()
        {
            var json = "{}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationConverter();
            var configuration = await configurationConverter.ConvertAsync(jsonStream);

            Assert.NotNull(configuration);
            Assert.NotNull(configuration.TimeSeries);
            Assert.Empty(configuration.TimeSeries.Meteo);
            Assert.Empty(configuration.TimeSeries.Sensors);
            Assert.Empty(configuration.Excel);
        }

        [Fact]
        public async Task ConvertAsync_EmptyTimeSeries()
        {
            var json = "{ \"timeseries\": {}}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationConverter();
            var configuration = await configurationConverter.ConvertAsync(jsonStream);

            Assert.NotNull(configuration);
            Assert.NotNull(configuration.TimeSeries);
            Assert.Empty(configuration.TimeSeries.Meteo);
            Assert.Empty(configuration.TimeSeries.Sensors);
        }

        [Fact]
        public async Task ConvertAsync_InvalidJson()
        {
            var json = "{";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationConverter();

            await Assert.ThrowsAsync<JsonException>(() => configurationConverter.ConvertAsync(jsonStream));
        }

        [Fact]
        public async Task ConvertAsync_TwoMeteoEntries()
        {
            var json = "{ \"timeseries\": { \"meteo\": [{ \"resource\": \"any.json\" }, { \"resource\": \"second.json\" }]}}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationConverter();
            var configuration = await configurationConverter.ConvertAsync(jsonStream);

            Assert.Contains(new MeteoConfig() { Resource = "any.json" }, configuration.TimeSeries.Meteo);
            Assert.Contains(new MeteoConfig() { Resource = "second.json" }, configuration.TimeSeries.Meteo);
        }

        [Fact]
        public async Task ConvertAsync_OneSensorEntry()
        {
            var json = "{ \"timeseries\": { \"sensors\": [{ \"format\": \"Sel\", \"resource\": \"heaven.json\" }]}}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationConverter();
            var configuration = await configurationConverter.ConvertAsync(jsonStream);

            Assert.Contains(
                new SensorConfig() { Format = SensorContentFormat.Sel, Resource = "heaven.json" }, 
                configuration.TimeSeries.Sensors);
        }

        [Fact]
        public async Task ConvertAsync_OneSensorEntry_InvalidFormatValue()
        {
            var json = "{ \"timeseries\": { \"sensors\": [{ \"format\": \"swiss\", \"resource\": \"heaven.json\" }]}}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationConverter();

            await Assert.ThrowsAsync<InvalidDataException>(() => configurationConverter.ConvertAsync(jsonStream));
        }

        [Fact]
        public async Task ConvertAsync_OneExcelEntry()
        {
            var json = "{ \"excel\": [{ \"resource\": \"homer.xlsx\", \"sheet\": \"atlantis\", \"row\": 1 }]}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationConverter();
            var configuration = await configurationConverter.ConvertAsync(jsonStream);

            Assert.Contains(new ExcelConfig() { Resource = "homer.xlsx", Sheet = "atlantis", Row = 1 }, configuration.Excel);
        }

        [Fact]
        public async Task ConvertAsync_OneExcelEntry_MissingSheet()
        {
            var json = "{ \"excel\": [{ \"resource\": \"homer.xlsx\", \"row\": 1 }]}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationConverter();

            await Assert.ThrowsAsync<InvalidDataException>(() => configurationConverter.ConvertAsync(jsonStream));
        }
    }
}
