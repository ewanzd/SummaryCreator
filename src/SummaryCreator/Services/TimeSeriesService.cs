using SummaryCreator.Configuration;
using SummaryCreator.Core;
using SummaryCreator.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SummaryCreator.Services
{
    public class TimeSeriesService : ITimeSeriesService
    {
        /// <summary>
        /// For logging.
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public Task<IEnumerable<ITimeSeries>> ReadAsync(TimeSeriesConfig config)
        {
            //var c = File.OpenRead("");
            //c.re
            //var content = await File.Read.ReadAllTextAsync("");


            //var sensorSourceDirectory = new DirectoryInfo(config.TimeSeries.Sensors.First().Resource.OriginalString);
            //var meteoSourceFile = new FileInfo(config.TimeSeries.Meteo.First().Resource.OriginalString);
            //var destinationExcel = new FileInfo(config.Excel.First().Resource.OriginalString);

            //// load data
            //var timeSeriesGroup = new List<ITimeSeries>();

            //Logger.Info(CultureInfo.InvariantCulture, "Load sensor data.");
            //timeSeriesGroup.AddRange(dataService.ReadSensorData(sensorSourceDirectory));

            //if (meteoSourceFile.Exists)
            //{
            //    Logger.Info(CultureInfo.InvariantCulture, "Load meteo data.");
            //    timeSeriesGroup.AddRange(dataService.ReadMeteoData(meteoSourceFile));
            //}

            return Task.FromResult(Enumerable.Empty<ITimeSeries>());
        }

        public Task WriteAsync(ExcelConfig excelConfig, IEnumerable<ITimeSeries> timeSeries)
        {
            // write to excel
            Logger.Info(CultureInfo.InvariantCulture, "Write results to excel.");
            //dataService.WriteToExcel(timeSeriesGroup, destinationExcel, config.Excel.First().Sheet, (int)config.Excel.First().Row);
            return Task.FromResult(0);
        }

        public Task WriteAsnyc(IEnumerable<ExcelConfig> excel, IEnumerable<ITimeSeries> timeSeries)
        {
            return Task.FromResult(0);
        }
    }
}
