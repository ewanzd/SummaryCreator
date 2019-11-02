using SummaryCreator.Core;
using SummaryCreator.Resources;
using SummaryCreator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SummaryCreator.View
{
    public sealed class ConfigPresenter
    {
        private readonly IConfigView view;
        private readonly DataService dataService;
        private readonly IniConfigurationService config;

        /// <summary>
        /// For logging.
        /// </summary>
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public ConfigPresenter(IConfigView view, DataService dataService, IniConfigurationService config)
        {
            Debug.Assert(view != null, $"{nameof(view)} must not be null");
            Debug.Assert(dataService != null, $"{nameof(dataService)} must not be null");
            Debug.Assert(config != null, $"{nameof(config)} must not be null");

            this.view = view;
            this.dataService = dataService;
            this.config = config;

            view.Presenter = this;

            // load configurations into UI
            view.ExcelPath = config.ResultExcelFilePath;
            view.MeteoPath = config.MeteoFilePath;
            view.TableName = config.ResultExcelSheetName;
            view.IdRow = config.ResultExcelSheetRowIndex;
            view.SensorDirectoryPath = config.SensorDirectoryPath;

            // timer settings
            view.RemainingTime = 10.0;
            view.TimerIsEnabled = true;
        }

        public void OnSave()
        {
            OnStop();

            try
            {
                // set changed configurations
                config.ResultExcelFilePath = view.ExcelPath;
                config.MeteoFilePath = view.MeteoPath;
                config.ResultExcelSheetName = view.TableName;
                config.ResultExcelSheetRowIndex = view.IdRow;
                config.SensorDirectoryPath = view.SensorDirectoryPath;

                // save configurations
                config.Save();

                view.Status = Strings.ConfigPresenter_StatusSaved;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                view.Status = string.Format(CultureInfo.CurrentCulture, Strings.ConfigPresenter_Error, ex.Message);
            }
        }

        public async Task OnRunAsync()
        {
            Logger.Info(CultureInfo.InvariantCulture, "Creation of summary started.");

            OnStop();

            view.Status = Strings.ConfigPresenter_StatusRunning;
            view.ActionButtonEnabled = false;

            try
            {
                await Task.Run(() => OnRun());

                Logger.Info(CultureInfo.InvariantCulture, "Creation of summary finished.");
                view.Status = Strings.ConfigPresenter_StatusFinished;
            }
            catch (Exception ex) when (ex is IOException || ex is XmlException || ex is InvalidDataException)
            {
                Logger.Error(ex);

                view.Status = string.Format(CultureInfo.CurrentCulture, Strings.ConfigPresenter_Error, ex.Message);
            }
            finally
            {
                view.ActionButtonText = Strings.ConfigPresenter_Run;
                view.ActionButtonEnabled = true;
            }
        }

        private void OnRun()
        {
            var sensorSourceDirectory = new DirectoryInfo(view.SensorDirectoryPath);
            var meteoSourceFile = new FileInfo(view.MeteoPath);
            var destinationExcel = new FileInfo(view.ExcelPath);

            // load data
            var containers = new List<IContainer>();

            Logger.Info(CultureInfo.InvariantCulture, "Load sensor data.");
            containers.AddRange(dataService.ReadSensorData(sensorSourceDirectory));

            if (meteoSourceFile.Exists)
            {
                Logger.Info(CultureInfo.InvariantCulture, "Load meteo data.");
                containers.AddRange(dataService.ReadMeteoData(meteoSourceFile));
            }

            // write to excel
            Logger.Info(CultureInfo.InvariantCulture, "Write results to excel.");
            dataService.WriteToExcel(containers, destinationExcel, view.TableName, view.IdRow);
        }

        public void OnStop()
        {
            view.ActionButtonText = Strings.ConfigPresenter_Run;
            view.TimerIsEnabled = false;
        }

        public void OnExit()
        {
            Application.Exit();
        }
    }
}