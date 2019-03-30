using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomaticSummaryCreator.Data
{
    /// <summary>
    /// Save and control all rows.
    /// </summary>
    public class Table : TableContainer
    {
        /// <summary>
        /// All rows.
        /// </summary>
        List<Row> rows = new List<Row>();

        /// <summary>
        /// Name of Container.
        /// </summary>
        public override string TypeName
        {
            get
            {
                return nameof(Table); // "Tabelle";
            }
        }

        /// <summary>
        /// The first time of first row.
        /// </summary>
        public override DateTime FirstTime
        {
            get
            {
                // Gibt die Zeit der erste Zeile zurück.
                // Es ist ratsam zuerst Sort aufzurufen
                return rows.First().CapturedAt;
            }
        }

        /// <summary>
        /// The last time of last row.
        /// </summary>
        public override DateTime LastTime
        {
            get
            {
                // Gibt die Zeit der letzten Zeile zurück.
                // Es ist ratsam zuerst Sort aufzurufen
                return rows.Last().CapturedAt;
            }
        }

        /// <summary>
        /// Add a new row.
        /// </summary>
        /// <param name="row">Add to the table.</param>
        public void Add(Row row)
        {
            // Fügt eine neue Zeile der Tabelle hinzu
            rows.Add(row);
        }

        /// <summary>
        /// Sort this table.
        /// </summary>
        public override void Sort()
        {
            // Sortiert die Liste nach der Zeit
            rows.OrderBy(x => x.CapturedAt);
        }

        /// <summary>
        /// Get the first row after the start DateTime.
        /// </summary>
        /// <param name="start">Search after that DateTime.</param>
        /// <returns>Return the row.</returns>
        public override Row GetRow(DateTime start, DateTime end)
        {
            // Ruft den ersten Durchgang auf und gibt den ersten Wert zurück
            foreach(var row in NextRow(start, end - start))
                return row;

            // Falls es in diesem Zeitraum keinen Wert gibt, wird null zurück gegeben
            return null;
        }

        /// <summary>
        /// Get the first row of list.
        /// </summary>
        /// <param name="start">Search from this time.</param>
        /// <param name="end">Search to this time.</param>
        /// <returns>Return the first row.</returns>
        public override Row FirstRow(DateTime start, DateTime end)
        {
            // Gibt die erste Zeile zurück.
            return rows.Where(x => x.CapturedAt >= start && x.CapturedAt < end).First();
        }

        /// <summary>
        /// Return the last existing item of the interval.
        /// </summary>
        /// <param name="start">Search from that DateTime.</param>
        /// <param name="interval">Interval for steps.</param>
        /// <returns>Return the last item of the interval.</returns>
        public override IEnumerable<Row> NextRow(DateTime start, TimeSpan interval)
        {
            // Die Methode gibt immer der letzte Wert des Intervalls zurück
            // Bei 08:00, wenn der letzte Wert in dieser Stunde 08:58 ist, wird dieser zurückgegeben

            // Speichert den letzten Wert
            Row lastRowOfInterval = null;

            // Der nächste Zeitpunkt des Intervalls, fängt immer bei der letzten voller Stunde an
            DateTime next = DateTime.Parse(start.ToString("dd.MM.yyyy HH:00")) + interval;
            // Geht jede Zeile der Tabelle durch
            foreach(var item in rows)
            {
                // Prüft, ob der aktuelle Wert der Zeile grösser ist als der nächste Intervall
                while(item.CapturedAt >= next)
                {
                    // Springt zum nächsten Intervall
                    next += interval;
                    // Gibt der erste Wert des Intervalls zurück. Wenn es in einem Intervall keinen Wert gibt, wird null zurückgegeben
                    yield return lastRowOfInterval;
                    // Setzt den Wert auf null zurück
                    lastRowOfInterval = null;
                }
                // Speichert die aktuelle Zeile als erster Wert des Intervalls wenn sie grösser als die Startzeit ist
                if(item.CapturedAt >= start)
                    lastRowOfInterval = item;
            }

            // Gibt der letzte Wert des letzten Intervalls zurück
            yield return lastRowOfInterval;
        }

        /// <summary>
        /// Calculate sum of all values in time range.
        /// </summary>
        /// <param name="start">Start date time to get sum.</param>
        /// <param name="end">End date time to get sum.</param>
        /// <returns>Sum of all values in time range.</returns>
        public override double Sum(DateTime start, DateTime end)
        {
            var rowsInPeriod = rows.Where(x => x.CapturedAt >= start && x.CapturedAt < end);

            var firstRow = rowsInPeriod.FirstOrDefault(x => x.CapturedAt == rowsInPeriod.Min(y => y.CapturedAt));
            var lastRow = rowsInPeriod.FirstOrDefault(x => x.CapturedAt == rowsInPeriod.Max(y => y.CapturedAt));

            return lastRow.Value - firstRow.Value;
        }
    }
}
