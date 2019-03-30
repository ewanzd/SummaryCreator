using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomaticSummaryCreator.Data
{
    /// <summary>
    /// A group of tables.
    /// </summary>
    public class Group : TableContainer
    {
        /// <summary>
        /// All tables or group in that group.
        /// </summary>
        protected List<TableContainer> container = new List<TableContainer>();

        /// <summary>
        /// Name of Container.
        /// </summary>
        public override string TypeName => nameof(Group); // "Gruppe";

        /// <summary>
        /// Number of container inside group.
        /// </summary>
        public int Count => container.Count;

        /// <summary>
        /// Get the count of group list with 'Interval' steps.
        /// </summary>
        public int CountOfIntervalSteps(TimeSpan interval)
        {
            // Prüfen, ob die Gruppe eine Tabelle besitzt
            if(container.Count == 0)
                return 0;

            // Erstes Datum ermitteln
            DateTime first = FirstTime;

            // Letztes Datum ermitteln und auf den vollen Tag aufrunden
            DateTime last = LastTime + new TimeSpan(1, 0, 0);

            // Ruft die Anzahl Minuten zwischen der ersten und letzten Zeit ab
            double differenceMinutes = (last - first).TotalMinutes;

            // Ruft den Interval mit den total anzahl Minuten ab
            double intervalMinutes = interval.TotalMinutes;

            // Prüft, wie oft der Interval in die Anzahl Minuten passt
            return Convert.ToInt32(differenceMinutes / intervalMinutes);
        }

        /// <summary>
        /// Get the first DateTime in all tables.
        /// </summary>
        public override DateTime FirstTime
        {
            get
            {
                // Holt der tiefste Wert aller Tabellen
                return DateTime.Parse(container.Min(x => x.FirstTime).ToString("dd.MM.yyyy HH:00"));
            }
        }

        /// <summary>
        /// Get the last DateTime in all tables.
        /// </summary>
        public override DateTime LastTime
        {
            get
            {
                // Holt der hächste Wert aller Tabellen
                return DateTime.Parse(container.Max(x => x.LastTime).ToString("dd.MM.yyyy HH:00"));
            }
        }

        /// <summary>
        /// Sort all tables.
        /// </summary>
        public override void Sort()
        {
            // Sortiert jedes Element des Containers
            foreach(var item in container)
                item.Sort();
        }

        /// <summary>
        /// Sum all first row of this group.
        /// </summary>
        /// <param name="start">Search from this time.</param>
        /// <param name="end">Search to this time.</param>
        /// <returns>Return the sum of all first rows.</returns>
        public override Row FirstRow(DateTime start, DateTime end)
        {
            // Abfrage erstellen
            var rows = from c in container
                       where c.FirstTime >= start
                       where c.FirstTime < end
                       select c.FirstRow(start, end);

            // Neue Zeile erstellen
            Row returnRow = new Row();
            double sum = 0.0;

            // Zeilen zusammenrechnen und Datum anpassen
            foreach(var row in rows)
            {
                if(returnRow.CapturedAt < row.CapturedAt)
                    returnRow.CapturedAt = row.CapturedAt;
                sum += row.Value;
            }

            // Summe der Zeile hinzufügen
            returnRow.Value = sum;

            // Neue Zeile zurückgeben
            return returnRow;
        }

        /// <summary>
        /// Add new TableContainer.
        /// </summary>
        /// <param name="table">The new element of TableContainer.</param>
        public void Add(TableContainer table)
        {
            // TableContainer darf nicht null sein
            if(table == null)
                throw new ArgumentNullException("table");

            // Fügt ein von TableContainer vererbtes Objekt der Gruppe hinzu
            container.Add(table);
        }

        /// <summary>
        /// Create new row with the start DateTime and object interval.
        /// </summary>
        /// <param name="start">Start time.</param>
        /// <returns>Return the new row.</returns>
        public override Row GetRow(DateTime start, DateTime end)
        {
            // Der Endwert muss höher oder gleich dem Startwert sein
            if(end < start)
                throw new ArgumentException("Das Startdatum kann nicht höher als das Enddatum sein");

            // Die neue Zeile
            Row row = new Row();

            // Summe aller Tabellen und Gruppen
            double sum = 0.0;

            // Ruft den letzten Wert innerhalb der Start- und Endzeit ab
            foreach(var table in container)
            {
                // Holt der höchste Wert der Tabelle innerhalb des Intervals 
                Row temp = table.GetRow(start, end);

                // Prüft, ob in diesem Zeitraum einen Wert vorhanden ist
                if(temp != null)
                {
                    // Speicher die höchste Zeit in der neuen Zeile
                    if(row.CapturedAt < temp.CapturedAt)
                        row.CapturedAt = temp.CapturedAt;

                    // Der Exportwert wird addiert
                    sum += temp.Value;
                }
            }

            // Fügt die Summe der Zeile hinzu
            row.Value = sum;

            // Gibt die Zeile zurück
            return row;
        }

        /// <summary>
        /// Return the last existing item of the interval.
        /// </summary>
        /// <param name="start">Search from that DateTime.</param>
        /// <param name="interval">Interval for steps.</param>
        /// <returns>Return the last item of the interval.</returns>
        public override IEnumerable<Row> NextRow(DateTime start, TimeSpan interval)
        {
            // Der Intervall braucht einen Wert, damit der Enumerable funktioniert
            if(interval.TotalMinutes == 0)
                throw new ArgumentException("Der Intervall muss höher als 0 sein");

            // Die Startzeit wird als aktuelle Zeit gesetzt
            DateTime current = start;

            // Die Anzahl der Zeilen wird ausgerechnet
            int count = CountOfIntervalSteps(interval);

            // Die ausgerechnete anzahl Zeilen erstellen
            int i = 0;
            while(i < count)
            {
                // Zeile holen
                Row row = GetRow(current, current + interval);

                // Prüfen, ob die Zeile Inhalt besitzt
                if(row.CapturedAt != default(DateTime) || i > 0)
                    i++;

                // Die Zeile zwischen der aktuellen Zeit und der folgende Zeit zurückgeben
                yield return row;

                // Aktuelle Zeit um den Interval erhöhen
                current += interval;
            }
        }

        /// <summary>
        /// Calculate sum of all values in time range.
        /// </summary>
        /// <param name="start">Start date time to get sum.</param>
        /// <param name="end">End date time to get sum.</param>
        /// <returns>Sum of all values in time range.</returns>
        public override double Sum(DateTime start, DateTime end)
        {
            return container.Sum(x => x.Sum(start, end));
        }
    }
}
