using Microsoft.Office.Interop.Excel;
using System;
using System.Diagnostics;

namespace AutomaticSummaryCreator.IO.Excel
{
    /// <summary>
    /// Stellt grundlegende Funktionen eines Arbeitsblattes zur Verfügung, kann aber auch vererbt und erweitert werden.
    /// </summary>
    public sealed class MyWorksheet
    {
        /// <summary>
        /// Auf eine bestimmte Zelle des Arbeitsblattes zugreifen.
        /// </summary>
        /// <param name="row">Die Zeile der Zelle.</param>
        /// <param name="col">Die Spalte der Zelle.</param>
        /// <returns>Der Inhalt der Zelle.</returns>
        public string this[int row, int col]
        {
            get
            {
                dynamic dyn = (sheet.Cells[row, col] as Range).Value;
                if(dyn == null)
                    return String.Empty;
                else if(dyn is DateTime)
                    return dyn.ToShortDateString();
                else
                    return dyn.ToString();
            }
            set
            {
                sheet.Cells[row, col] = value;
            }
        }

        /// <summary>
        /// Vorhandene Anzahl der Zeilen.
        /// </summary>
        public int CountRow
        {
            get
            {
                return sheet.UsedRange.Rows.Count;
            }
        }

        /// <summary>
        /// Vorhandene Anzahl der Spalten.
        /// </summary>
        public int CountCol
        {
            get
            {
                return sheet.UsedRange.Columns.Count;
            }
        }

        /// <summary>
        /// Dem Arbeitsblatt einen Namen geben.
        /// </summary>
        public string Name
        {
            get
            {
                return sheet.Name;
            }
            set
            {
                sheet.Name = value;
            }
        }

        /// <summary>
        /// Das zu verwaltende Arbeitsblatt.
        /// </summary>
        private Worksheet sheet = null;

        /// <summary>
        /// Neues Arbeitsblatt erstellen, besitzt jedoch noch kein Arbeitsblatt, auf dieses zugegriffen werden könnte.
        /// </summary>
        public MyWorksheet()
        {

        }

        /// <summary>
        /// Neues vereinfachtes Arbeitsblatt mit dem mitgegeben Arbeitsblatt erstellen.
        /// </summary>
        /// <param name="sheet"></param>
        public MyWorksheet(Worksheet sheet)
        {
            SetSheet(sheet);
        }

        /// <summary>
        /// Arbeitsblatt setzen.
        /// </summary>
        /// <param name="sheet"></param>
        public void SetSheet(Worksheet sheet)
        {
            this.sheet = sheet;
        }

        public int IndexOfRow(string value, int col = 1)
        {
            return IndexOfRow(value, 1, CountRow, col);
        }

        public int IndexOfRow(string value, int startRow, int endRow, int col = 1)
        {
            Debug.Assert(startRow > 0, $"{nameof(startRow)} must be greater than 0");
            Debug.Assert(CountRow >= endRow, $"{nameof(CountRow)} must be greater or equal {nameof(endRow)}");
            Debug.Assert(col > 0, $"{nameof(col)} must be greater than 0");
            Debug.Assert(endRow >= startRow, $"{nameof(startRow)} must be greater or equal {nameof(endRow)}");

            // Startwerte erstellen
            int currentNumber = startRow;

            // Läuft bis zur ersten leeren Zelle durch
            while (currentNumber <= endRow)
            {
                // Werte stimmen überein
                if (this[currentNumber, col] == value)
                    return currentNumber;

                // Zur nächsten Zeile springen
                currentNumber++;
            }
            // Wert ist grösser als der letzte Wert
            return -1;
        }

        public int IndexOfCol(string value, int row = 1)
        {
            return IndexOfCol(value, 1, CountCol, row);
        }

        public int IndexOfCol(string value, int startCol, int endCol, int row = 1)
        {
            Debug.Assert(startCol > 0, $"{nameof(startCol)} must be greater than 0");
            Debug.Assert(CountRow >= endCol, $"{nameof(CountRow)} must be greater or equal {nameof(endCol)}");
            Debug.Assert(row > 0, $"{nameof(row)} must be greater than 0");
            Debug.Assert(endCol >= startCol, $"{nameof(startCol)} must be greater or equal {nameof(endCol)}");

            // Startwerte erstellen
            int currentNumber = startCol;

            // Läuft bis zur ersten leeren Zelle durch
            while (currentNumber <= endCol)
            {
                // Werte stimmen überein
                if (this[row, currentNumber] == value)
                    return currentNumber;

                // Zur nächsten Zeile springen
                currentNumber++;
            }
            // Wert ist grösser als der letzte Wert
            return -1;
        }
    }
}
