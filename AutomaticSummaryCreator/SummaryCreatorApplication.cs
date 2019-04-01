using AutomaticSummaryCreator.GUI;
using AutomaticSummaryCreator.IO.Excel;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomaticSummaryCreator
{
    /// <summary>
    /// Verwaltet das Fenster und startet die Auswertung der XML-Datei und fügt der Excel-Datei die Daten hinzu.
    /// </summary>
    public class SummaryCreatorApplication
    {
        /// <summary>
        /// Konfigurationsdaten.
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// Ein Fenster mit einem Countdown und der Möglichkeit, die Konfigurationsdaten anzupassen.
        /// </summary>
        private ConfigTimer meteo;

        /// <summary>
        /// Erstellt das Objekt ohne ein Fenster zu öffnen.
        /// </summary>
        public SummaryCreatorApplication(string iniPath)
        {
            // Inipfad festlegen
            configuration = new Configuration(iniPath);
        }

        /// <summary>
        /// Öffnet das Countdownfenster.
        /// </summary>
        public SummaryCreatorApplication(string iniPath, int sec)
            : this(iniPath)
        {
            // Setzt die Ereignisse und startet das Fenster
            meteo = new ConfigTimer(sec, configuration);
            meteo.Start += MeteoStart;
            meteo.Exit += MeteoExit;
            meteo.Show();
        }

        /// <summary>
        /// Startet den Vorgang aus dem Fenster.
        /// </summary>
        private async void MeteoStart(object sender, EventArgs e)
        {
            // Startet den Vorgang asynchron
            await Task.Run(() =>
            {
                SheetDataInsert insert = null;

                try
                {
                    // Erstellt eine Klasse, die für das korrekte Einfügen der Daten zuständig ist
                    insert = new SheetDataInsert(configuration.ExcelPath, configuration.SheetName, configuration.SheetIdRow);

                    // Erstellt die Auswertung der Zähler
                    ExecuteCounter(configuration.ExcelSourceDirectory, insert);

                    // Erstellt die Auswertung der Meteodaten
                    ExecuteMeteo(insert);

                    // Speichert und schliesst die Excel-Datei
                    insert.Close();
                }
                catch (Exception ex)
                {
                    // Speichert und schliesst die Excel-Datei
                    if (insert != null)
                        insert.Close(false);

                    // Fehler ausgeben
                    MessageBox.Show("Es ist während der Ausführung folgender Fehler aufgetreten: " + ex.Message);
                }
            });

            // Schliesst das Fenster
            if (meteo != null)
                meteo.Close();
        }

        /// <summary>
        /// Führt der Vorgang aus.
        /// </summary>
        public void ExecuteCounter(string excelSourceDirectory, SheetDataInsert insert)
        {
            // Wertet die Zählerdaten aus
            EvaluationSensor sensor = new EvaluationSensor();

            // Speichert das Verzeichnis, wo sich die Excel-Dateien der Zähler befinden
            DirectoryInfo directory = new DirectoryInfo(excelSourceDirectory);

            // Ladet alle .csv Dateien in diesem Verzeichnis
            foreach (FileInfo file in directory.GetFiles())
                if (file.Extension == ".csv")
                    sensor.LoadData(file.FullName);

            sensor.Summary.Sort();

            // Speichert eine Zeile der Zähler
            DateTime start = DateTime.Parse(sensor.Summary.StartTime.ToShortDateString());
            DateTime current = start;
            DateTime end = DateTime.Parse(sensor.Summary.EndTime.ToShortDateString());
            while (current <= end)
            {
                sensor.SaveData(insert, current.ToShortDateString());
                current += TimeSpan.FromDays(1);
            }
        }

        public void ExecuteMeteo(SheetDataInsert insert)
        {
            // Wertet alle Meteodaten aus
            EvaluationMeteo meteo = new EvaluationMeteo();

            // Ladet alle Meteodaten in dieserDatei
            meteo.LoadData(configuration.XmlPath);

            // Speichert alle Meteo-Daten für den heutigen Tag
            meteo.SaveData(insert, meteo.Data.Produced.ToShortDateString());
        }

        private void MeteoExit(object sender, EventArgs e)
        {
            // Gibt die Daten des Fensters frei
            meteo.Dispose();

            // Beendet die Applikation
            Environment.Exit(0);
        }
    }
}
