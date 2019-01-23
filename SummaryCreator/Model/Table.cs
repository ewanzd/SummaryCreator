﻿using SummaryCreator.Basic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SummaryCreator.Model
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
                return "Tabelle";
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
                return rows.First().DateTime;
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
                return rows.Last().DateTime;
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
            rows.OrderBy(x => x.DateTime);
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
            return rows.Where(x => x.DateTime >= start && x.DateTime < end).First();
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
                while(item.DateTime >= next)
                {
                    // Springt zum nächsten Intervall
                    next += interval;
                    // Gibt der erste Wert des Intervalls zurück. Wenn es in einem Intervall keinen Wert gibt, wird null zurückgegeben
                    yield return lastRowOfInterval;
                    // Setzt den Wert auf null zurück
                    lastRowOfInterval = null;
                }
                // Speichert die aktuelle Zeile als erster Wert des Intervalls wenn sie grösser als die Startzeit ist
                if(item.DateTime >= start)
                    lastRowOfInterval = item;
            }
            // Gibt der letzte Wert des letzten Intervalls zurück
            yield return lastRowOfInterval;
        }
    }
}
