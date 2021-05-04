using SummaryCreator.Configuration;
using SummaryCreator.Core;
using SummaryCreator.Input;
using SummaryCreator.Output;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.Services
{
    public sealed class TimeSeriesService : ITimeSeriesService
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly ITimeSeriesReaderFactory timeSeriesReaderFactory;
        private readonly ITimeSeriesReader meteoReader;
        private readonly ISummaryWriter excelWriter;
        private readonly IFileService fileService;

        private readonly DateTimeOffset from = DateTimeOffset.Now.AddDays(7);
        private readonly DateTimeOffset to = DateTimeOffset.Now;

        public TimeSeriesService(ITimeSeriesReaderFactory timeSeriesReaderFactory, ITimeSeriesReader meteoReader, ISummaryWriter excelWriter, IFileService fileService)
        {
            this.timeSeriesReaderFactory = timeSeriesReaderFactory ?? throw new ArgumentNullException(nameof(timeSeriesReaderFactory));
            this.meteoReader = meteoReader ?? throw new ArgumentNullException(nameof(meteoReader));
            this.excelWriter = excelWriter ?? throw new ArgumentNullException(nameof(excelWriter));
            this.fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public async Task<IEnumerable<ITimeSeries>> ReadAsync(IEnumerable<MeteoConfig> config, CancellationToken cancellationToken)
        {
            var timeSeriesGroup = new List<ITimeSeries>();

            foreach (var meteoConfig in config)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Load sensor data from {0} with format {1}", meteoConfig.Resource);

                var content = await fileService.LoadAsync(meteoConfig.Resource, from, to, cancellationToken).ConfigureAwait(false);
                var meteoTimeSeries = meteoReader.Read(meteoConfig.Resource, content);
                timeSeriesGroup.AddRange(meteoTimeSeries);
            }

            return timeSeriesGroup;
        }

            public async Task<IEnumerable<ITimeSeries>> ReadAsync(IEnumerable<EnergyConfig> config, CancellationToken cancellationToken)
        {
            var timeSeriesGroup = new List<ITimeSeries>();
            
            foreach (var sensorConfig in config)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Load sensor data from {0} with format {1}", sensorConfig.Resource, sensorConfig.Format.ToString());

                var content = await fileService.LoadAsync(sensorConfig.Resource, from, to, cancellationToken).ConfigureAwait(false);

                var sensorReader = timeSeriesReaderFactory.CreateSensorReader(sensorConfig);
                var sensorTimeSeries = sensorReader.Read(sensorConfig.Resource, content);
                timeSeriesGroup.AddRange(sensorTimeSeries);
            }

            return timeSeriesGroup;
        }

        public Task WriteAsync(IEnumerable<ITimeSeries> timeSeries, IEnumerable<SummaryConfig> excelConfigs, CancellationToken cancellationToken)
        {
            foreach (var excelConfig in excelConfigs)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Write results to {0}", excelConfig.Resource);
                using var fileStream = fileService.Open(excelConfig.Resource);
                excelWriter.Write(timeSeries, fileStream, excelConfig);
            }

            return Task.FromResult(0);
        }
    }
}
