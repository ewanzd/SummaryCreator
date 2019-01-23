using SummaryCreator.Basic;
using System;
using System.Collections.Generic;

namespace SummaryCreator.Model
{
    /// <summary>
    /// Save all data from one Row
    /// </summary>
    public class Row : IEnumerable<WattHour>
    {
        /// <summary>
        /// List with all cell values.
        /// </summary>
        protected List<WattHour> cells = new List<WattHour>();

        /// <summary>
        /// Return the cell at index.
        /// </summary>
        /// <param name="index">The index of the cell.</param>
        /// <returns>Return the item.</returns>
        public WattHour this[int index]
        {
            get
            {
                // Gibt die angegebene Spalte der Zeile zurück
                return cells[index];
            }
        }

        /// <summary>
        /// Saved DateTime of the row.
        /// </summary>
        public DateTime DateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Length of the list.
        /// </summary>
        public int Count
        {
            get
            {
                // Gibt die Anzahl der Spalten in dieser Zeile zurück
                return cells.Count;
            }
        }

        /// <summary>
        /// Add a new cell.
        /// </summary>
        /// <param name="cell">The value.</param>
        public void Add(WattHour cell)
        {
            // Fügt der Zeile eine neue Spalte an letzter Stelle hinzu
            cells.Add(cell);
        }

        /// <summary>
        /// Each every column in that row.
        /// </summary>
        /// <returns>Return every column from this row.</returns>
        public IEnumerator<WattHour> GetEnumerator()
        {
            foreach(var item in cells)
            {
                yield return item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
