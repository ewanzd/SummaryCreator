using SummaryCreator.Configuration;
using SummaryCreator.Configuration.Json;
using SummaryCreator.Services;
using System;
using System.Globalization;
using System.IO;
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

            Logger.Info(CultureInfo.InvariantCulture, "Initialize configuration reader");
            var reader = new DefaultConfigurationReader(new JsonConfigurationConverter());

            // read data needed for processing time series data
            Logger.Info(CultureInfo.InvariantCulture, "Read configurations");
            var configurations = await reader.ReadAsync(configurationFilePath).ConfigureAwait(false);
            
            Logger.Info(CultureInfo.InvariantCulture, "Initialize time series service");
            var timeSeriesService = new TimeSeriesService();

            // read and write time series data
            Logger.Info(CultureInfo.InvariantCulture, "Read time series data from sources");
            var timeSeries = await timeSeriesService.ReadAsync(configurations.TimeSeries);

            // write time series data to target excel files
            Logger.Info(CultureInfo.InvariantCulture, "Write time series data to target files");
            await timeSeriesService.WriteAsnyc(configurations.Excel, timeSeries);
        }
    }
}