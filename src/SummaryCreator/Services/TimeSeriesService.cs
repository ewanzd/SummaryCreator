using SummaryCreator.Configuration;
using SummaryCreator.Core;
using SummaryCreator.Input;
using SummaryCreator.Output;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace SummaryCreator.Services
{
    public sealed class TimeSeriesService : ITimeSeriesService
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ITimeSeriesReaderFactory timeSeriesReaderFactory;
        private readonly ITimeSeriesReader meteoReader;
        private readonly IExcelWriter excelWriter;

        public TimeSeriesService(ITimeSeriesReaderFactory timeSeriesReaderFactory, ITimeSeriesReader meteoReader, IExcelWriter excelWriter)
        {
            this.timeSeriesReaderFactory = timeSeriesReaderFactory ?? throw new ArgumentNullException(nameof(timeSeriesReaderFactory));
            this.meteoReader = meteoReader ?? throw new ArgumentNullException(nameof(meteoReader));
            this.excelWriter = excelWriter ?? throw new ArgumentNullException(nameof(excelWriter));
        }

        public async Task<IEnumerable<ITimeSeries>> ReadAsync(IEnumerable<MeteoConfig> config)
        {
            var timeSeriesGroup = new List<ITimeSeries>();

            foreach (var meteoConfig in config)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Load sensor data from {0} with format {1}", meteoConfig.Resource);

                var content = await File.ReadAllTextAsync(meteoConfig.Resource).ConfigureAwait(false);
                var meteoTimeSeries = meteoReader.Read(meteoConfig.Resource, content);
                timeSeriesGroup.AddRange(meteoTimeSeries);
            }

            return timeSeriesGroup;
        }

            public async Task<IEnumerable<ITimeSeries>> ReadAsync(IEnumerable<EnergyConfig> config)
        {
            var timeSeriesGroup = new List<ITimeSeries>();
            
            foreach (var sensorConfig in config)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Load sensor data from {0} with format {1}", sensorConfig.Resource, sensorConfig.Format.ToString());

                var content = await File.ReadAllTextAsync(sensorConfig.Resource).ConfigureAwait(false);

                var sensorReader = timeSeriesReaderFactory.CreateSensorReader(sensorConfig);
                var sensorTimeSeries = sensorReader.Read(sensorConfig.Resource, content);
                timeSeriesGroup.AddRange(sensorTimeSeries);
            }

            return timeSeriesGroup;
        }

        public Task WriteAsync(IEnumerable<ITimeSeries> timeSeries, IEnumerable<SummaryConfig> excelConfigs)
        {
            foreach (var excelConfig in excelConfigs)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Write results to {0}", excelConfig.Resource);
                using var fileStream = File.Open(excelConfig.Resource, FileMode.Open, FileAccess.ReadWrite);
                excelWriter.Write(timeSeries, fileStream, excelConfig);
            }

            return Task.FromResult(0);
        }
    }
}
