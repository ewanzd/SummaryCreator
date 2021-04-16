using SummaryCreator.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SummaryCreator.IO.Csv
{
    /// <summary>
    /// Convert json with energy data to <see cref="SensorTimeSeries"/>.
    /// </summary>
    /// <example>
    /// Expected content format:
    /// <code>
    /// {
    ///  "data": [
    ///    {
    ///      "Active Energy Import": 67870006.90643704,
    ///      "timestamp": 1598911200
    ///    },
    ///    {
    ///      "Active Energy Import": 67870759.90990916,
    ///      "timestamp": 1598912100
    ///    }
    ///  ]
    /// }
    /// </code>
    /// </example>
    public class ApiSelEnergyJsonReader : ITimeSeriesReader
    {
        private const char resourceSeperator = '/';
        private static readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        public IEnumerable<ITimeSeries> Read(string resource, string content)
        {
            if (string.IsNullOrWhiteSpace(resource))
            {
                throw new ArgumentException($"'{nameof(resource)}' cannot be null or whitespace.", nameof(resource));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException($"'{nameof(content)}' cannot be null or whitespace.", nameof(content));
            }

            var id = ExtractId(resource, resourceSeperator);
            var sensorTimeSeries = new SensorTimeSeries(id);

            var jsonSelEnergyModel = JsonSerializer.Deserialize<JsonSelEnergyRootModel>(content, options);

            foreach(var entry in jsonSelEnergyModel.Data)
            {
                var dataPoint = new DataPoint();

                dataPoint.Value = entry.ActiveEnergyImport;
                dataPoint.CapturedAt = DateTimeOffset.FromUnixTimeSeconds(entry.Timestamp);

                sensorTimeSeries.Add(dataPoint);
            }

            return new List<ITimeSeries>() { sensorTimeSeries };
        }

        private string ExtractId(string resource, char seperator)
        {
            Debug.Assert(resource != null);
            Debug.Assert(seperator != default(char));

            var resourceParts = resource.Split(seperator);

            if (resourceParts.Length < 5)
            {
                throw new ArgumentException("Invalid resource", nameof(resource));
            }

            return resourceParts[5];
        }

        private class JsonSelEnergyRootModel
        {
            public JsonSelEnergyDataEntryModel[] Data { get; set; }
        }

        private class JsonSelEnergyDataEntryModel
        {
            [JsonPropertyName("Active Energy Import")]
            public double ActiveEnergyImport { get; set; }
            public long Timestamp { get; set; }
        }
    }
}

