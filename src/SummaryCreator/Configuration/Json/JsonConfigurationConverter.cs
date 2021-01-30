using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.Configuration.Json
{
    public class JsonConfigurationConverter : IConfigurationConverter
    {
        public JsonSerializerOptions Options { get; set; } = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public async Task<SummaryCreatorConfig> ConvertAsync(Stream contentStream, CancellationToken cancellationToken = default)
        {
            var jsonConfigModel = await JsonSerializer.DeserializeAsync<JsonSummaryCreatorModel>(contentStream, Options, cancellationToken).ConfigureAwait(false);
            return ConvertJsonModelToConfig(jsonConfigModel);
        }

        private static SummaryCreatorConfig ConvertJsonModelToConfig(JsonSummaryCreatorModel jsonModel)
        {
            // ToList to validate entries immediately
            return new SummaryCreatorConfig()
            {
                TimeSeries = new TimeSeriesConfig()
                {
                    Meteo = (jsonModel?.Timeseries?.Meteo?.Select(x => ConvertToMeteoConfig(x)) ?? Enumerable.Empty<MeteoConfig>()).ToList(),
                    Sensors = (jsonModel?.Timeseries?.Sensors?.Select(x => ConvertToSensorConfig(x)) ?? Enumerable.Empty<SensorConfig>()).ToList()
                },
                Excel = (jsonModel?.Excel?.Select(x => ConvertToExcelConfig(x)) ?? Enumerable.Empty<ExcelConfig>()).ToList()
            };
        }

        private static MeteoConfig ConvertToMeteoConfig(JsonMeteoModel meteoModel)
        {
            if (!Uri.TryCreate(meteoModel.Resource, UriKind.RelativeOrAbsolute, out Uri resource))
                throw new InvalidDataException($"'{meteoModel.Resource}' is not a valid uri format");

            return new MeteoConfig()
            {
                Resource = resource
            };
        }

        private static SensorConfig ConvertToSensorConfig(JsonSensorModel sensorModel)
        {
            if (!Enum.TryParse(sensorModel.Format, out SensorContentFormat sensorContentFormat))
                throw new InvalidDataException($"'{sensorModel.Format}' is not a valid value");

            if (!Uri.TryCreate(sensorModel.Resource, UriKind.RelativeOrAbsolute, out Uri resource))
                throw new InvalidDataException($"'{sensorModel.Resource}' is not a valid uri format");

            return new SensorConfig()
            {
                Format = sensorContentFormat,
                Resource = resource
            };
        }

        private static ExcelConfig ConvertToExcelConfig(JsonExcelModel excelModel)
        {
            if (!Uri.TryCreate(excelModel.Resource, UriKind.RelativeOrAbsolute, out Uri resource))
                throw new InvalidDataException($"'{excelModel.Resource}' is not a valid uri format");

            if (string.IsNullOrEmpty(excelModel.Sheet))
                throw new InvalidDataException("Missing sheet name");

            if (excelModel.Row < 0)
                throw new InvalidDataException($"'{excelModel.Row}' must be a positive value");

            return new ExcelConfig()
            {
                Resource = resource,
                Sheet = excelModel.Sheet,
                Row = excelModel.Row
            };
        }
    }

    public class JsonSummaryCreatorModel
    {
        public JsonTimeseriesModel Timeseries { get; set; }

        public JsonExcelModel[] Excel { get; set; }
    }

    public class JsonExcelModel
    {
        public string Resource { get; set; }

        public string Sheet { get; set; }

        public long Row { get; set; }
    }

    public class JsonTimeseriesModel
    {
        public JsonMeteoModel[] Meteo { get; set; }

        public JsonSensorModel[] Sensors { get; set; }
    }

    public class JsonMeteoModel
    {
        public string Resource { get; set; }
    }

    public class JsonSensorModel
    {
        public string Format { get; set; }

        public string Resource { get; set; }
    }
}
