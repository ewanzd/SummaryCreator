using SummaryCreator.Configuration;
using SummaryCreator.Core;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SummaryCreator.Services
{
    public class SummaryCreatorService
    {
        public SummaryCreatorService(DataService dataService)
        {
            this.dataService = dataService;
        }

        /// <summary>
        /// For logging.
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly DataService dataService;

        public void ProcessTimeSeriesData(SummaryCreatorConfig config)
        {
            var sensorSourceDirectory = new DirectoryInfo(config.TimeSeries.Sensors.First().Resource.OriginalString);
            var meteoSourceFile = new FileInfo(config.TimeSeries.Meteo.First().Resource.OriginalString);
            var destinationExcel = new FileInfo(config.Excel.First().Resource.OriginalString);

            // load data
            var timeSeriesGroup = new List<ITimeSeries>();

            Logger.Info(CultureInfo.InvariantCulture, "Load sensor data.");
            timeSeriesGroup.AddRange(dataService.ReadSensorData(sensorSourceDirectory));

            if (meteoSourceFile.Exists)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Load meteo data.");
                timeSeriesGroup.AddRange(dataService.ReadMeteoData(meteoSourceFile));
            }

            // write to excel
            Logger.Info(CultureInfo.InvariantCulture, "Write results to excel.");
            dataService.WriteToExcel(timeSeriesGroup, destinationExcel, config.Excel.First().Sheet, (int)config.Excel.First().Row);
        }
    }
}
