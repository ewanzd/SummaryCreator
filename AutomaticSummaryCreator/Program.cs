using AutomaticSummaryCreator.Data;
using AutomaticSummaryCreator.Excel;
using AutomaticSummaryCreator.GUI;
using AutomaticSummaryCreator.Source;
using BasicLibrary;
using BasicLibrary.Excel;
using BasicLibrary.Unit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomaticSummaryCreator
{
    static class Program
    {
        /// <summary>
        /// Der Pfad zu der Ini-Datei, wo alle weitere Konfigurationen gespeichert werden.
        /// </summary>
        public static string IniPath = Path.Combine(Application.StartupPath, "SummaryCreator.ini");

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Prüfen, ob ein neuer Ini-Pfad mitgegeben wurde
            string path = args.Where(x => x.Split(':')[0] == "IniPath").FirstOrDefault();
            if(!String.IsNullOrWhiteSpace(path))
                IniPath = path;

            // Prüfen, ob die Sekunden mitgegeben wurde, die der Benutzer Zeit hat, um die Konfigurationen zu bearbeiten
            int startSec;
            if(!Int32.TryParse(args.Where(x => x.Split(':')[0] == "RestTime").FirstOrDefault(), out startSec))
                startSec = 10;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            new Root(startSec);
            Application.Run();
        }
    }

    /// <summary>
    /// Verwaltet das Fenster und startet die Auswertung der XML-Datei und fügt der Excel-Datei die Daten hinzu.
    /// </summary>
    public class Root
    {
        /// <summary>
        /// Konfigurationsdaten.
        /// </summary>
        private Ini ini;

        /// <summary>
        /// Ein Fenster mit einem Countdown und der Möglichkeit, die Konfigurationsdaten anzupassen.
        /// </summary>
        private ConfigTimer meteo;

        /// <summary>
        /// Erstellt das Objekt ohne ein Fenster zu öffnen.
        /// </summary>
        public Root()
        {
            // Inipfad festlegen
            ini = new Ini(Program.IniPath);
        }

        /// <summary>
        /// Öffnet das Countdownfenster.
        /// </summary>
        public Root(int sec)
            : this()
        {
            // Setzt die Ereignisse und startet das Fenster
            meteo = new ConfigTimer(sec, ini);
            meteo.Start += meteo_Start;
            meteo.Exit += meteo_Exit;
            meteo.Show();
        }

        /// <summary>
        /// Führt der Vorgang aus.
        /// </summary>
        public void ExecuteCounter(SheetDataInsert insert)
        {
            // Wertet die Zählerdaten aus
            EvaluationCounter counter = new EvaluationCounter();

            // Speichert das Verzeichnis, wo sich die Excel-Dateien der Zähler befinden
            DirectoryInfo directory = new DirectoryInfo(ini.ExcelSourceDirectory);

            // Ladet alle .csv Dateien in diesem Verzeichnis
            foreach(System.IO.FileInfo file in directory.GetFiles())
                if(file.Extension == ".csv")
                    counter.LoadData(file.FullName);

            counter.Summary.Sort();

            // Speichert eine Zeile der Zähler
            DateTime start = DateTime.Parse(counter.Summary.StartTime.ToShortDateString());
            DateTime current = start;
            DateTime end = DateTime.Parse(counter.Summary.EndTime.ToShortDateString());
            while(current <= end)
            {
                counter.SaveData(insert, current.ToShortDateString());
                current += new TimeSpan(24, 0, 0);
            }
        }

        public void ExecuteMeteo(SheetDataInsert insert)
        {
            // Wertet alle Meteodaten aus
            EvaluationMeteo meteo = new EvaluationMeteo();

            // Ladet alle Meteodaten in dieserDatei
            meteo.LoadData(ini.XmlPath);

            // Speichert alle Meteo-Daten für den heutigen Tag
            meteo.SaveData(insert, meteo.Data.Produced.ToShortDateString());
        }

        /// <summary>
        /// Startet den Vorgang aus dem Fenster.
        /// </summary>
        private async void meteo_Start(object sender, EventArgs e)
        {
            // Startet den Vorgang asynchron
            await Task.Run(() =>
            {
                SheetDataInsert insert = null;

                try
                {
                    // Erstellt eine Klasse, die für das korrekte Einfügen der Daten zuständig ist
                    insert = new SheetDataInsert(ini.ExcelPath, ini.SheetName, ini.SheetIdRow);

                    // Erstellt die Auswertung der Zähler
                    ExecuteCounter(insert);

                    // Erstellt die Auswertung der Meteodaten
                    ExecuteMeteo(insert);

                    // Speichert und schliesst die Excel-Datei
                    insert.Close();
                }
                catch(Exception ex)
                {
                    // Speichert und schliesst die Excel-Datei
                    if(insert != null)
                        insert.Close(false);

                    // Fehler ausgeben
                    MessageBox.Show("Es ist während der Ausführung folgender Fehler aufgetreten: " + ex.Message);
                }
            });

            // Schliesst das Fenster
            if(meteo != null)
                meteo.Close();
        }

        private void meteo_Exit(object sender, EventArgs e)
        {
            // Gibt die Daten des Fensters frei
            meteo.Dispose();

            // Beendet die Applikation
            Environment.Exit(0);
        }
    }
}
