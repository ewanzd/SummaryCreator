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
            var meteoConfigs = (jsonModel?.Meteo?.Select(x => ConvertToMeteoConfig(x)).ToHashSet() ?? Enumerable.Empty<MeteoConfig>()).ToHashSet();
            var energyConfigs = (jsonModel?.Energy?.Select(x => ConvertToEnergyConfig(x)).ToHashSet() ?? Enumerable.Empty<EnergyConfig>()).ToHashSet();
            var summaryConfigs = (jsonModel?.Summary?.Select(x => ConvertToExcelConfig(x)).ToHashSet() ?? Enumerable.Empty<SummaryConfig>()).ToHashSet();

            return new SummaryCreatorConfig(meteoConfigs, energyConfigs, summaryConfigs);
        }

        private static MeteoConfig ConvertToMeteoConfig(JsonMeteoModel meteoModel)
        {
            if (!Uri.IsWellFormedUriString(meteoModel.Resource, UriKind.RelativeOrAbsolute))
                throw new InvalidDataException($"'{meteoModel.Resource}' is not a valid uri format");

            return new MeteoConfig(meteoModel.Resource);
        }

        private static EnergyConfig ConvertToEnergyConfig(JsonEnergyModel energyModel)
        {
            if (!Enum.TryParse(energyModel.Format, out EnergySourceFormat energySourceFormat))
                throw new InvalidDataException($"'{energyModel.Format}' is not a valid value");

            if (!Uri.IsWellFormedUriString(energyModel.Resource, UriKind.RelativeOrAbsolute))
                throw new InvalidDataException($"'{energyModel.Resource}' is not a valid uri format");

            return new EnergyConfig(energyModel.Resource, energySourceFormat);
        }

        private static SummaryConfig ConvertToExcelConfig(JsonExcelModel excelModel)
        {
            
            if (!Uri.IsWellFormedUriString(excelModel.Resource, UriKind.RelativeOrAbsolute))
                throw new InvalidDataException($"'{excelModel.Resource}' is not a valid uri format");

            if (string.IsNullOrEmpty(excelModel.Sheet))
                throw new InvalidDataException("Missing sheet name");

            if (excelModel.Row < 0)
                throw new InvalidDataException($"'{excelModel.Row}' must be a positive value");

            return new SummaryConfig(excelModel.Resource, excelModel.Sheet, excelModel.Row);
        }

        private class JsonSummaryCreatorModel
        {
            public JsonMeteoModel[] Meteo { get; set; }

            public JsonEnergyModel[] Energy { get; set; }

            public JsonExcelModel[] Summary { get; set; }
        }

        private class JsonExcelModel
        {
            public string Resource { get; set; }

            public string Sheet { get; set; }

            public int Row { get; set; }
        }

        private class JsonMeteoModel
        {
            public string ResourceType { get; set; }

            public string Resource { get; set; }
        }

        private class JsonEnergyModel
        {
            public string Format { get; set; }

            public string ResourceType { get; set; }

            public string Resource { get; set; }
        }
    }
}
