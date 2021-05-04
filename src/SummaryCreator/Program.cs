using SummaryCreator.Configuration;
using SummaryCreator.Configuration.Json;
using SummaryCreator.Input;
using SummaryCreator.Input.Xml;
using SummaryCreator.Output.Excel;
using SummaryCreator.Services;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryCreator
{
    /// <summary>
    /// Main entry point.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// For logging.
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static int Main(string[] args)
        {
            return MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task<int> MainAsync(string[] args)
        {
            try
            {
                await RunAsync(args).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return 1;
            }

            return 0;
        }

        private static async Task RunAsync(string[] args)
        {
            if (args.Length < 1) throw new InvalidDataException("Missing path to configuration file");

            var configurationFilePath = new FileInfo(args[0]);
            Logger.Info(CultureInfo.InvariantCulture, "Path to configuration file: {0}", configurationFilePath);

            // read data needed for processing time series data
            Logger.Info(CultureInfo.InvariantCulture, "Read configurations");
            var reader = new DefaultConfigurationReader(new JsonConfigurationParser());
            var configurations = await reader.ReadAsync(configurationFilePath).ConfigureAwait(false);

            var timeSeriesReaderFactory = new TimeSeriesReaderFactory();
            var meteoReader = new MeteoXmlReader();
            var excelWriter = new EppExcelWriter();
            var timeSeriesService = new TimeSeriesService(timeSeriesReaderFactory, meteoReader, excelWriter);

            // read and write time series data
            Logger.Info(CultureInfo.InvariantCulture, "Read meteo data from sources");
            var meteoSeries = await timeSeriesService.ReadAsync(configurations.MeteoConfigs).ConfigureAwait(false);
            Logger.Info(CultureInfo.InvariantCulture, "Read energy data from sources");
            var energySeries = await timeSeriesService.ReadAsync(configurations.EnergyConfigs).ConfigureAwait(false);

            var summary = meteoSeries.Concat(energySeries);

            // write time series data to target excel files
            Logger.Info(CultureInfo.InvariantCulture, "Write time series data to target files");
            await timeSeriesService.WriteAsync(summary, configurations.SummaryConfigs).ConfigureAwait(false);
        }
    }
}