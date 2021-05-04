using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.Configuration.Json
{
    public class JsonConfigurationParser : IConfigurationParser
    {
        public JsonSerializerOptions Options { get; set; } = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public async Task<SummaryCreatorConfig> ParseAsync(Stream contentStream, CancellationToken cancellationToken = default)
        {
            var jsonConfigModel = await JsonSerializer.DeserializeAsync<JsonRootModel>(contentStream, Options, cancellationToken).ConfigureAwait(false);
            return ParseJsonRootModel(jsonConfigModel);
        }

        private static SummaryCreatorConfig ParseJsonRootModel(JsonRootModel jsonModel)
        {
            var meteoConfigs = (jsonModel?.Meteo?.Select(x => ParseJsonMeteoModel(x)).ToHashSet() ?? Enumerable.Empty<MeteoConfig>()).ToHashSet();
            var energyConfigs = (jsonModel?.Energy?.Select(x => ParseJsonEnergyModel(x)).ToHashSet() ?? Enumerable.Empty<EnergyConfig>()).ToHashSet();
            var summaryConfigs = (jsonModel?.Summary?.Select(x => ParseSummaryModel(x)).ToHashSet() ?? Enumerable.Empty<SummaryConfig>()).ToHashSet();

            return new SummaryCreatorConfig(meteoConfigs, energyConfigs, summaryConfigs);
        }

        private static MeteoConfig ParseJsonMeteoModel(JsonMeteoModel meteoModel)
        {
            if (!Uri.IsWellFormedUriString(meteoModel.Resource, UriKind.RelativeOrAbsolute))
                throw new InvalidDataException($"'{meteoModel.Resource}' is not a valid uri format");

            return new MeteoConfig(meteoModel.Resource);
        }

        private static EnergyConfig ParseJsonEnergyModel(JsonEnergyModel energyModel)
        {
            if (!Enum.TryParse(energyModel.Format, out EnergySourceFormat energySourceFormat))
                throw new InvalidDataException($"'{energyModel.Format}' is not a valid value");

            if (!Uri.IsWellFormedUriString(energyModel.Resource, UriKind.RelativeOrAbsolute))
                throw new InvalidDataException($"'{energyModel.Resource}' is not a valid uri format");

            return new EnergyConfig(energyModel.Resource, energySourceFormat);
        }

        private static SummaryConfig ParseSummaryModel(JsonSummaryModel summaryModel)
        {
            
            if (!Uri.IsWellFormedUriString(summaryModel.Resource, UriKind.RelativeOrAbsolute))
                throw new InvalidDataException($"'{summaryModel.Resource}' is not a valid uri format");

            if (string.IsNullOrEmpty(summaryModel.Sheet))
                throw new InvalidDataException("Missing sheet name");

            if (summaryModel.Row < 0)
                throw new InvalidDataException($"'{summaryModel.Row}' must be a positive value");

            return new SummaryConfig(summaryModel.Resource, summaryModel.Sheet, summaryModel.Row);
        }

        private class JsonRootModel
        {
            public JsonMeteoModel[] Meteo { get; set; }

            public JsonEnergyModel[] Energy { get; set; }

            public JsonSummaryModel[] Summary { get; set; }
        }

        private class JsonSummaryModel
        {
            public string Resource { get; set; }

            public string Sheet { get; set; }

            public int Row { get; set; }
        }

        private class JsonMeteoModel
        {
            public string Resource { get; set; }
        }

        private class JsonEnergyModel
        {
            public string Format { get; set; }

            public string Resource { get; set; }
        }
    }
}
