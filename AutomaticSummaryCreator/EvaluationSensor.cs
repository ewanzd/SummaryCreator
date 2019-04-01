using AutomaticSummaryCreator.Data;
using AutomaticSummaryCreator.IO.Excel;
using AutomaticSummaryCreator.Source;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AutomaticSummaryCreator
{
    /// <summary>
    /// Verwaltet die Daten der Zähler.
    /// </summary>
    public sealed class EvaluationSensor : IEvaluation
    {
        /// <summary>
        /// Datencontainer.
        /// </summary>
        public Summary Summary { get; private set; }

        /// <summary>
        /// Identity des Sensors.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Werteleser.
        /// </summary>
        private CsvTimeSerieReader reader = new CsvTimeSerieReader();
        private char fileNameSeparator = '_';


        /// <summary>
        /// Fügt alle Daten der angegebenen CSV-Datei hinzu.
        /// </summary>
        /// <param name="path">Pfad zu der CSV-Datei.</param>
        public void LoadData(string path)
        {
            Id = ExtractId(path);

            // Prüft, ob bereits ein Container erstellt wurde
            if (Summary == null)
            {
                Summary = new Summary();
            }

            // Daten abrufen und einfügen
            foreach (var row in reader.ReadRowsFromFile(new FileInfo(path)))
            {
                Summary.Add(Id, row);
            }
        }

        /// <summary>
        /// Extract sensor id from file name.
        /// </summary>
        /// <param name="path">Path to file with data.</param>
        /// <returns>Id of sensor.</returns>
        private string ExtractId(string path)
        {
            // ID des Zählers from file name
            // Beispiel: dbdata_6F5CBF4A-FC2F-4E67-99A6-3AFB3D9C2E46.csv
            var fileName = Path.GetFileNameWithoutExtension(path);
            return fileName.Split(fileNameSeparator)[1];
        }

        /// <summary>
        /// Daten speichern.
        /// </summary>
        /// <param name="insert">Daten über den angegebenen Insert speichern.</param>
        /// <param name="targetRow">ID der Zeile, in der die Werte gespeichert werden soll.</param>
        public void SaveData(SheetDataInsert insert, string targetRow)
        {
            // Prüfen, ob Daten vorhanden sind
            if(Summary == null)
                throw new Exception("Keine Daten verfügbar");
            
            // Daten einfügen
            insert.Insert(InsertGetData, targetRow);
        }

        /// <summary>
        /// Wertet den Wert für das entsprechende Feld aus.
        /// </summary>
        /// <param name="colId">Bezeichnung für die angesprochene Spalte.</param>
        /// <param name="rowId">Bezeichnung für die angesprochene Zeile.</param>
        /// <returns>Ausgewerteter Wert.</returns>
        private string InsertGetData(string colId, string rowId)
        {
            // Teilt den String in die einzelnen Tabellen auf
            string[] tableIds = colId.Split(';');

            // Datencontainer für die angesprochenen Tabellen
            TableContainer container = null;

            // Prüft, ob mehrere Tabellen angesprochen wurden
            if (tableIds.Length > 1)
            {
                // Stellt ein Container für die Gruppen zur Verfügung
                Group group = new Group();

                // Alle Tabellen der Spalte
                foreach (var exId in tableIds)
                {
                    // Prüft, ob der Zähler vorhanden ist
                    var item = Summary.Where(con => con.ID.Equals(exId)).FirstOrDefault();
                    if (item != null)
                        group.Add(item);
                }

                // Stellt die Gruppe als Zielcontainer zur Verfügung
                container = group.Count == 0 ? null : group;
            }
            else if (tableIds.Length == 1)
            {
                // Angesprochene Tabelle abrufen und stellt die Tabelle als Zielcontainer zur Verfügung
                container = Summary.Where(con => con.ID.Equals(tableIds[0])).FirstOrDefault() as Table;
            }

            // Der Container muss einen Wert enthalten
            if(container == null)
            {
                return string.Empty;
            }

            // Sucht den richtigen Wert für das angesprochene Feld
            var startDateTime = DateTime.Parse(rowId);
            var endDateTime = startDateTime + TimeSpan.FromDays(1);
            return container.Sum(startDateTime, endDateTime).ToString("0.###", CultureInfo.InvariantCulture);
        }
    }
}
