using AutomaticSummaryCreator.Data;
using AutomaticSummaryCreator.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AutomaticSummaryCreator.View
{
    public class ConfigPresenter
    {
        private readonly IConfigView view;
        private readonly DataService dataService;
        private readonly Configuration configuration;

        public ConfigPresenter(IConfigView view, DataService dataService, Configuration configuration)
        {
            Debug.Assert(view != null, $"{nameof(view)} must not be null");
            Debug.Assert(dataService != null, $"{nameof(dataService)} must not be null");
            Debug.Assert(configuration != null, $"{nameof(configuration)} must not be null");

            this.view = view;
            this.dataService = dataService;
            this.configuration = configuration;

            view.Presenter = this;

            // Bestehende Konfigurationen einfügen
            view.ExcelPath = configuration.ExcelPath;
            view.MeteoPath = configuration.XmlPath;
            view.TableName = configuration.SheetName;
            view.IdRow = configuration.SheetIdRow;
            view.SensorDirectoryPath = configuration.ExcelSourceDirectory;

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
                configuration.ExcelPath = view.ExcelPath;
                configuration.XmlPath = view.MeteoPath;
                configuration.SheetName = view.TableName;
                configuration.SheetIdRow = view.IdRow;
                configuration.ExcelSourceDirectory = view.SensorDirectoryPath;

                // Daten abspeichern
                configuration.Save();

                // Konfigurationen wurden erfolgreich gespeichert
                view.Status = "Gespeichert";
            }
            catch(Exception ex)
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
            catch(Exception ex)
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
