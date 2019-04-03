using AutomaticSummaryCreator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace AutomaticSummaryCreator.IO.Excel
{
    public class ExcelWriter : IDataWriter
    {
        private const char colSeperator = ';';

        private readonly SheetDataInsert sheetDataInsert;
        private IEnumerable<IDataContainer> containers;

        public ExcelWriter(SheetDataInsert sheetDataInsert)
        {
            Debug.Assert(sheetDataInsert != null, $"{nameof(sheetDataInsert)} must not be null");

            this.sheetDataInsert = sheetDataInsert;
        }

        public void Write(IEnumerable<IDataContainer> containers)
        {
            this.containers = containers;

            foreach (var container in containers)
            {
                DateTime start = DateTime.Parse(container.First.CapturedAt.ToShortDateString());
                DateTime current = start;
                DateTime end = DateTime.Parse(container.Last.CapturedAt.ToShortDateString());

                while (current <= end)
                {
                    var targetRow = current.ToShortDateString();

                    // Daten einfügen
                    sheetDataInsert.Insert(InsertGetData, targetRow);

                    current += TimeSpan.FromDays(1);
                }
            }
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
            string[] tableIds = colId.Split(colSeperator);

            // Sucht den richtigen Wert für das angesprochene Feld
            var startDateTime = DateTime.Parse(rowId);
            var endDateTime = startDateTime + TimeSpan.FromDays(1);

            // Prüft, ob mehrere Tabellen angesprochen wurden
            if (tableIds.Length > 1)
            {
                // Stellt ein Container für die Gruppen zur Verfügung
                DataGroup group = new DataGroup();

                // Alle Tabellen der Spalte
                foreach (var exId in tableIds)
                {
                    // Prüft, ob der Zähler vorhanden ist
                    var item = containers.Where(con => con.Id.Equals(exId)).FirstOrDefault();
                    if (item != null)
                    {
                        group.Add(item);
                    }
                }

                // Gibt die Summe im spezifizierten Zeitbereich zurück
                return group.Sum(startDateTime, endDateTime).ToString("0.###", CultureInfo.InvariantCulture);
            }
            else if (tableIds.Length == 1)
            {
                // Angesprochene Tabelle abrufen und stellt die Tabelle als Zielcontainer zur Verfügung
                var container = containers.Where(con => con.Id.Equals(tableIds[0])).FirstOrDefault();

                if(container != null)
                {
                    // Gibt die Summe im spezifizierten Zeitbereich zurück
                    return container.Sum(startDateTime, endDateTime).ToString("0.###", CultureInfo.InvariantCulture);
                }
            }

            // Der Container muss einen Wert enthalten
            return string.Empty;
        }
    }
}
