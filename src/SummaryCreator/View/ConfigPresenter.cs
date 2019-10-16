using SummaryCreator.Data;
using SummaryCreator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SummaryCreator.View
{
    public class ConfigPresenter
    {
        private readonly IConfigView view;
        private readonly IDataService dataService;
        private readonly IConfigurationService config;

        public ConfigPresenter(IConfigView view, IDataService dataService, IConfigurationService config)
        {
            Debug.Assert(view != null, $"{nameof(view)} must not be null");
            Debug.Assert(dataService != null, $"{nameof(dataService)} must not be null");
            Debug.Assert(config != null, $"{nameof(config)} must not be null");

            this.view = view;
            this.dataService = dataService;
            this.config = config;

            view.Presenter = this;

            // Bestehende Konfigurationen einfügen
            view.ExcelPath = config.ResultExcelFilePath;
            view.MeteoPath = config.MeteoFilePath;
            view.TableName = config.ResultExcelSheetName;
            view.IdRow = config.ResultExcelSheetRowIndex;
            view.SensorDirectoryPath = config.SensorDirectoryPath;

            // Timer Optionen
            view.RemainingTime = 10.0;
            view.TimerIsEnabled = true;
        }

        public void OnSave()
        {
            // Timer stoppen
            OnStop();

            try
            {
                // Daten übernehmen
                config.ResultExcelFilePath = view.ExcelPath;
                config.MeteoFilePath = view.MeteoPath;
                config.ResultExcelSheetName = view.TableName;
                config.ResultExcelSheetRowIndex = view.IdRow;
                config.SensorDirectoryPath = view.SensorDirectoryPath;

                // Daten abspeichern
                config.Save();

                // Konfigurationen wurden erfolgreich gespeichert
                view.Status = "Gespeichert";
            }
            catch (Exception ex)
            {
                // Fehler ausgeben
                view.Status = $"Fehler: {ex.Message}";
            }
        }

        public void OnRun()
        {
            OnStop();

            view.Status = "Wird ausgewertet....";
            view.ActionButtonEnabled = false;

            try
            {
                var sensorSourceDirectory = new DirectoryInfo(view.SensorDirectoryPath);
                var meteoSourceFile = new FileInfo(view.MeteoPath);
                var destinationExcel = new FileInfo(view.ExcelPath);

                // Daten sammeln
                var containers = new List<IDataContainer>();
                containers.AddRange(dataService.ReadSensorData(sensorSourceDirectory));
                containers.AddRange(dataService.ReadMeteoData(meteoSourceFile));

                // Daten in Excel schreiben
                dataService.WriteToExcel(containers, destinationExcel, view.TableName, view.IdRow);
            }
            catch (Exception ex)
            {
                // Fehler ausgeben
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
            // Beendet die Applikation
            Environment.Exit(0);
        }
    }
}