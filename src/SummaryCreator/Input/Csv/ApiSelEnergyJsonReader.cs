using SummaryCreator.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SummaryCreator.Input.Csv
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
        private static readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };

        public IEnumerable<ITimeSeries> Read(string id, string content)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException($"'{nameof(content)}' cannot be null or whitespace.", nameof(content));
            }

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

