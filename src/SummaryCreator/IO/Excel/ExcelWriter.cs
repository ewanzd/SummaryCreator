using SummaryCreator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SummaryCreator.IO.Excel
{
    public sealed class ExcelWriter : IDataWriter
    {
        private const char colSeperator = ';';

        private FileInfo excelFile;
        private MyWorkbook workbook;
        private MyWorksheet worksheet;
        private IEnumerable<IDataContainer> containers;

        private int idRow;
        private int lastRow = 1;

        public void InitWorksheet(FileInfo targetFile, string sheetName, int idRow)
        {
            Debug.Assert(targetFile != null, $"{nameof(targetFile)} must not be null");
            Debug.Assert(!string.IsNullOrEmpty(sheetName), $"{nameof(sheetName)} must not be null");

            excelFile = targetFile;
            this.idRow = idRow;

            workbook = new MyWorkbook(targetFile.FullName);
            worksheet = workbook.GetSheet(sheetName);
        }

        public void Write(IEnumerable<IDataContainer> containers)
        {
            this.containers = containers;

            var containerGroup = new DataGroup();
            containerGroup.AddRange(containers);

            DateTime start = containerGroup.FirstDataPoint.CapturedAt.Date;
            DateTime current = start;
            DateTime end = containerGroup.LastDataPoint.CapturedAt.Date;

            while (current <= end)
            {
                var targetRow = current.ToShortDateString();

                InsertData(targetRow);

                current = NextDay(current);
            }
        }

        private DateTime NextDay(DateTime currentDateTime)
        {
            var tomorrow = currentDateTime + TimeSpan.FromDays(1);

            if (containers.Any(x => tomorrow >= x.First.CapturedAt && tomorrow < x.Last.CapturedAt))
            {
                return tomorrow;
            }

            return containers
                .OrderBy(x => x.Last.CapturedAt)
                .SkipWhile(x => tomorrow > x.Last.CapturedAt)
                .Aggregate((minItem, nextItem) => minItem.First.CapturedAt < nextItem.First.CapturedAt ? minItem : nextItem)
                .First.CapturedAt.Date;
        }

        /// <summary>
        /// Neue Daten in die Tabelle einfüllen.
        /// </summary>
        /// <param name="getData">Funktion übergeben, in der die ID ausgewertet und der fertige Wert zurückgibt, der in die Tabelle eingefügt wird(SpaltenID, ZeilenID).</param>
        /// <param name="targetRowValue">Die Bezeichnung der Spalte, in der die Daten eingefügt werden sollen.</param>
        /// <param name="targetColSearch">Sucht in dieser Spalte nach der Bezeichnung.</param>
        private int InsertData(string targetRowValue, int targetColSearch = 1)
        {
            Debug.Assert(idRow > 0, $"{nameof(idRow)} must be greater than 0");
            Debug.Assert(idRow <= worksheet.CountRow, $"{nameof(idRow)} must be less than {nameof(worksheet.CountRow)}");

            // ID-Zeile muss mindestens 1 sein
            if (idRow < 1 || idRow > worksheet.CountRow)
                throw new IndexOutOfRangeException("Ungültige ID-Zeile: " + idRow);

            // Zeile ermitteln
            int row = worksheet.IndexOfRow(targetRowValue, lastRow, worksheet.CountRow, targetColSearch);

            // Falls nicht gefunden bei Zeile 1 suchen beginnen
            if (row < 1)
            {
                row = worksheet.IndexOfRow(targetRowValue, 1, worksheet.CountRow, targetColSearch);
            }

            // Wenn die Zeile nicht gefunden werden konnte wird eine neue Zeile hinten angefügt
            if (row < 1)
            {
                row = worksheet.CountRow + 1;
                worksheet[row, targetColSearch] = targetRowValue;
            }
            else
            {
                lastRow = row;
            }

            // Alle Sensoren für das jeweilige Datum auf Daten überprüfen und einfügen
            for (int col = 1; col <= worksheet.CountCol; col++)
            {
                // Wert lesen
                var id = worksheet[idRow, col];

                // Prüfen, ob das Feld einen Wert besitzt
                if (string.IsNullOrWhiteSpace(id))
                {
                    continue;
                }

                var container = containers.Where(con => con.Id.Equals(id)).FirstOrDefault();
                if (container is MeteoContainer)
                {
                    var startDateTime = DateTime.Parse(targetRowValue);
                    var endDateTime = startDateTime + TimeSpan.FromDays(1);

                    var dataPoint = container.FirstOrDefault(x => 
                        x.CapturedAt >= startDateTime && 
                        x.CapturedAt < endDateTime &&
                        x.CapturedAt.ToUniversalTime().Hour == 12);

                    if (dataPoint != null)
                    {
                        var value = dataPoint.Value;
                        worksheet[row, col] = value.ToString("0.###", CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    // Berechnet das Total für den spezifischen Sensor
                    var total = GetTotalByIdAndDay(id, targetRowValue);
                    if (!double.IsNaN(total))
                    {
                        worksheet[row, col] = total.ToString("0.###", CultureInfo.InvariantCulture);
                    }

                    // Berechnet die Summe für den spezifischen Sensor
                    var sum = GetSumByIdAndDay(id, targetRowValue);
                    if (!double.IsNaN(sum))
                    {
                        worksheet[row, col + 1] = sum.ToString("0.###", CultureInfo.InvariantCulture);
                    }
                }
            }

            return row;
        }

        /// <summary>
        /// Wertet den Wert für das entsprechende Feld aus.
        /// </summary>
        /// <param name="containerId">Bezeichnung für die angesprochene Spalte.</param>
        /// <param name="dateTime">Bezeichnung für die angesprochene Zeile.</param>
        /// <returns>Ausgewerteter Wert.</returns>
        private double GetSumByIdAndDay(string containerId, string dateTime)
        {
            // Teilt den String in die einzelnen Tabellen auf
            string[] tableIds = containerId.Split(colSeperator);

            // Sucht den richtigen Wert für das angesprochene Feld
            var startDateTime = DateTime.Parse(dateTime);
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
                    var container = containers.Where(con => con.Id.Equals(exId)).FirstOrDefault();
                    if (container != null)
                    {
                        group.Add(container);
                    }
                }

                // Gibt die Summe im spezifizierten Zeitbereich zurück
                if(group.AnyBetween(startDateTime, endDateTime))
                {
                    return group.Sum(startDateTime, endDateTime);
                }
            }
            else if (tableIds.Length == 1)
            {
                // Angesprochene Tabelle abrufen und stellt die Tabelle als Zielcontainer zur Verfügung
                var container = containers.Where(con => con.Id.Equals(tableIds[0])).FirstOrDefault();

                if (container != null && container.AnyBetween(startDateTime, endDateTime))
                {
                    // Gibt die Summe im spezifizierten Zeitbereich zurück
                    return container.Sum(startDateTime, endDateTime);
                }
            }

            // Der Container muss einen Wert enthalten
            return double.NaN;
        }

        /// <summary>
        /// Wertet den Wert für das entsprechende Feld aus.
        /// </summary>
        /// <param name="containerId">Bezeichnung für die angesprochene Spalte.</param>
        /// <param name="dateTime">Bezeichnung für die angesprochene Zeile.</param>
        /// <returns>Ausgewerteter Wert.</returns>
        private double GetTotalByIdAndDay(string containerId, string dateTime)
        {
            // Teilt den String in die einzelnen Tabellen auf
            string[] tableIds = containerId.Split(colSeperator);

            // Sucht den richtigen Wert für das angesprochene Feld
            var startDateTime = DateTime.Parse(dateTime);
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

                if (group.AnyBetween(startDateTime, endDateTime))
                {
                    return group.Total(endDateTime);
                }
            }
            else if (tableIds.Length == 1)
            {
                // Angesprochene Tabelle abrufen und stellt die Tabelle als Zielcontainer zur Verfügung
                var container = containers.Where(con => con.Id.Equals(tableIds[0])).FirstOrDefault();

                if (container != null && container.AnyBetween(startDateTime, endDateTime))
                {
                    return container.Total(endDateTime);
                }
            }

            // Der Container muss einen Wert enthalten
            return double.NaN;
        }

        /// <summary>
        /// Speichert und schliesst die Excel-Applikation.
        /// </summary>
        public void Close()
        {
            // Prüfen ob das Arbeitsbuch erstellt wurde
            if (workbook == null)
            {
                return;
            }

            // Schliesst das Arbeitsbuch und speichert es falls gewünscht
            if (!workbook.IsClose)
            {
                workbook.Close();
            }

            // Gibt die Excel-Applikation frei
            if (!workbook.IsDispose)
            {
                workbook.Dispose();
            }
        }

        /// <summary>
        /// Speichert und schliesst die Excel-Applikation.
        /// </summary>
        public void SaveAndClose()
        {
            // Prüfen ob das Arbeitsbuch erstellt wurde
            if (workbook == null)
            {
                return;
            }

            // Schliesst das Arbeitsbuch und speichert es falls gewünscht
            if (!workbook.IsClose)
            {
                workbook.SaveAndClose(excelFile.FullName);
            }

            // Gibt die Excel-Applikation frei
            if (!workbook.IsDispose)
            {
                workbook.Dispose();
            }
        }
    }
}
