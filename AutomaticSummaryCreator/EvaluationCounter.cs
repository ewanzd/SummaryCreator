using AutomaticSummaryCreator.Data;
using AutomaticSummaryCreator.Excel;
using AutomaticSummaryCreator.Source;
using BasicLibrary;
using BasicLibrary.Unit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticSummaryCreator
{
    /// <summary>
    /// Verwaltet die Daten der Zähler.
    /// </summary>
    public class EvaluationCounter : Evaluation
    {
        /// <summary>
        /// Datencontainer.
        /// </summary>
        public Summary Summary
        {
            get;
            set;
        }

        /// <summary>
        /// Fügt alle Daten der angegebenen CSV-Datei hinzu.
        /// </summary>
        /// <param name="path">Pfad zu der CSV-Datei.</param>
        public override void LoadData(string path)
        {
            // Prüft, ob bereits ein Container erstellt wurde
            if(Summary == null)
                Summary = new Summary();

            // Daten abrufen und einfügen
            CsvSource.ImportFromCSV(path, Summary);
        }

        /// <summary>
        /// Daten speichern.
        /// </summary>
        /// <param name="insert">Daten über den angegebenen Insert speichern.</param>
        /// <param name="targetRow">ID der Zeile, in der die Werte gespeichert werden soll.</param>
        public override void SaveData(SheetDataInsert insert, string targetRow)
        {
            // Prüfen, ob Daten vorhanden sind
            if(Summary == null)
                throw new Exception("Keine Daten verfügbar");
            
            // Daten einfügen
            insert.Insert(insert_GetData, targetRow);
        }

        /// <summary>
        /// Wertet den Wert für das entsprechende Feld aus.
        /// </summary>
        /// <param name="colId">Bezeichnung für die angesprochene Spalte.</param>
        /// <param name="rowId">Bezeichnung für die angesprochene Zeile.</param>
        /// <returns>Ausgewerteter Wert.</returns>
        protected virtual string insert_GetData(string colId, string rowId)
        {
            // Teilt den String in die einzelnen Tabellen auf
            string[] tableIds = colId.Split(';');

            // Welche Spalte der Tabelle genommen werden soll
            int col = 0;

            // Datencontainer für die angesprochenen Tabellen
            TableContainer container = null;

            // Prüft, ob mehrere Tabellen angesprochen wurden
            if(tableIds.Length > 1)
            {
                // Stellt ein Container für die Gruppen zur Verfügung
                Group group = new Group();

                // Alle Tabellen der Spalte
                foreach(var exId in tableIds)
                {
                    // Prüft, ob der Zähler vorhanden ist
                    var item = Summary.Where(con => con.ID == exId).FirstOrDefault();
                    if(item != null)
                        group.Add(item);
                }

                // Stellt die Gruppe als Zielcontainer zur Verfügung
                container = group;
            }
            else
            {
                // Prüft, welche Spalte angesprochen wurde beispielsweise Import / Export
                string[] cols = tableIds[0].Split('-');
                if(cols.Length > 1)
                    if(!Int32.TryParse(cols[1], out col))
                        col = 0;

                // Angesprochene Tabelle abrufen
                Table table = (Table)Summary.Where(con => con.ID == cols[0]).FirstOrDefault();

                // Stellt die Tabelle als Zielcontainer zur Verfügung
                container = table;
            }

            // Der Container muss einen Wert enthalten
            if(container == null)
                return String.Empty;

            // Sucht den richtigen Wert für das angesprochene Feld
            Row row = container.NextRow(container.FirstTime, new TimeSpan(24, 0, 0)).Where(x => x != null && x.DateTime.ToShortDateString() == rowId).FirstOrDefault();

            // Gibt den Wert im Kiloformat zurück, falls er gefunden wurde
            return (row != null) ? row[col].ToString(Unit.Kilo) : String.Empty;
        }
    }
}
