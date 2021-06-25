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

            var configurationConverter = new JsonConfigurationParser();
            var configuration = await configurationConverter.ParseAsync(jsonStream);

            Assert.NotNull(configuration);
            Assert.Empty(configuration.MeteoConfigs);
            Assert.Empty(configuration.EnergyConfigs);
            Assert.Empty(configuration.SummaryConfigs);
        }

        [Fact]
        public async Task ConvertAsync_InvalidJson()
        {
            var json = "{";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationParser();

            await Assert.ThrowsAsync<JsonException>(() => configurationConverter.ParseAsync(jsonStream));
        }

        [Fact]
        public async Task ConvertAsync_TwoMeteoEntries()
        {
            var json = "{ \"meteo\": [{ \"resource\": \"any.json\" }, { \"resource\": \"second.json\" }]}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationParser();
            var configuration = await configurationConverter.ParseAsync(jsonStream);

            Assert.Contains(new MeteoConfig("any.json"), configuration.MeteoConfigs);
            Assert.Contains(new MeteoConfig("second.json"), configuration.MeteoConfigs);
        }

        [Fact]
        public async Task ConvertAsync_OneSensorEntry()
        {
            var json = "{ \"energy\": [{ \"id\": \"heaven\", \"format\": \"Sel\", \"resource\": \"heaven.json\" }]}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationParser();
            var configuration = await configurationConverter.ParseAsync(jsonStream);

            Assert.Contains(new EnergyConfig("heaven", "heaven.json", EnergySourceFormat.Sel), configuration.EnergyConfigs);
        }

        [Fact]
        public async Task ConvertAsync_OneSensorEntry_InvalidFormatValue()
        {
            var json = "{ \"energy\": [{ \"format\": \"swiss\", \"resource\": \"heaven.json\" }]}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationParser();

            await Assert.ThrowsAsync<InvalidDataException>(() => configurationConverter.ParseAsync(jsonStream));
        }

        [Fact]
        public async Task ConvertAsync_OneExcelEntry()
        {
            var json = "{ \"summary\": [{ \"resource\": \"homer.xlsx\", \"sheet\": \"atlantis\", \"row\": 1 }]}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationParser();
            var configuration = await configurationConverter.ParseAsync(jsonStream);

            Assert.Contains(new SummaryConfig("homer.xlsx", "atlantis", 1), configuration.SummaryConfigs);
        }

        [Fact]
        public async Task ConvertAsync_OneExcelEntry_MissingSheet()
        {
            var json = "{ \"summary\": [{ \"resource\": \"homer.xlsx\", \"row\": 1 }]}";
            using var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            var configurationConverter = new JsonConfigurationParser();

            await Assert.ThrowsAsync<InvalidDataException>(() => configurationConverter.ParseAsync(jsonStream));
        }
    }
}
