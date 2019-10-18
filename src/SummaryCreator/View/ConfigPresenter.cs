using SummaryCreator.Data;
using SummaryCreator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SummaryCreator.View
{
    public class ConfigPresenter
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

                view.Status = "Gespeichert";
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                view.Status = $"Fehler: {ex.Message}";
            }
        }

        public void OnRun()
        {
            Logger.Info("Creation of summary started.");

            OnStop();

            view.Status = "Wird ausgewertet....";
            view.ActionButtonEnabled = false;

            try
            {
                var sensorSourceDirectory = new DirectoryInfo(view.SensorDirectoryPath);
                var meteoSourceFile = new FileInfo(view.MeteoPath);
                var destinationExcel = new FileInfo(view.ExcelPath);

                // load data
                var containers = new List<IDataContainer>();

                Logger.Info("Load sensor data.");
                containers.AddRange(dataService.ReadSensorData(sensorSourceDirectory));

                if (meteoSourceFile.Exists)
                {
                    Logger.Info("Load meteo data.");
                    containers.AddRange(dataService.ReadMeteoData(meteoSourceFile));
                }

                // write to excel
                Logger.Info("Write results to excel.");
                dataService.WriteToExcel(containers, destinationExcel, view.TableName, view.IdRow);

                Logger.Info("Creation of summary finished.");
                view.Status = "Auswertung abgeschlossen.";
            }
            catch (Exception ex)
            {
                Logger.Error(ex);

                view.Status = $"Fehler: {ex.Message}";
            }
            finally
            {
                view.ActionButtonText = "Ausführen";
                view.ActionButtonEnabled = true;
            }
        }

        public void OnStop()
        {
            view.ActionButtonText = "Ausführen";
            view.TimerIsEnabled = false;
        }

        public void OnExit()
        {
            Application.Exit();
        }
    }
}