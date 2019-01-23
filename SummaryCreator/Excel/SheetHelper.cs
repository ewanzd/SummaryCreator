using System;
using Excel = Microsoft.Office.Interop.Excel;

namespace SummaryCreator.Export
{
    /// <summary>
    /// Extension methods for worksheet.
    /// </summary>
    internal class SheetHelper
    {
        /// <summary>
        /// Run with them Worksheet.
        /// </summary>
        Excel.Worksheet worksheet;

        /// <summary>
        /// Can get and set value as string.
        /// </summary>
        /// <param name="row">Row of the cell.</param>
        /// <param name="col">Column of the cell.</param>
        /// <returns>Return the value of them cell as string.</returns>
        public string this[int row, int col]
        {
            get
            {
                return (string)(worksheet.Cells[row, col] as Excel.Range).Value;
            }
            set
            {
                worksheet.Cells[row, col] = value;
            }
        }

        /// <summary>
        /// Constructor: Create the extension for the Worksheet.
        /// </summary>
        /// <param name="worksheet">The Worksheet, with them work the object.</param>
        public SheetHelper(Excel.Worksheet worksheet)
        {
            if(worksheet == null)
                throw new ArgumentNullException("worksheet");

            this.worksheet = worksheet;
        }

        /// <summary>
        /// Add a titel to the Worksheet.
        /// </summary>
        /// <param name="col">The number of the column for titel.</param>
        /// <param name="name">The name from titel.</param>
        public virtual void AddTitel(int col, string name)
        {
            // Fügt der ersten Zeile der Spalte den Titel hinzu
            worksheet.Cells[1, col] = name;
        }

        /// <summary>
        /// Create a column with DateTime values.
        /// </summary>
        /// <param name="start">Start as this DateTime.</param>
        /// <param name="end">End as this DateTime.</param>
        /// <param name="interval">Write every interval from start to end time.</param>
        /// <param name="startRow">Start as this row number.</param>
        /// <param name="col">Write in that column.</param>
        /// <returns>Return the last row number.</returns>
        public virtual int CreateDateTimeCol(DateTime start, DateTime end, TimeSpan interval, int startRow, int col)
        {
            // Speichert den ersten Wert
            DateTime current = DateTime.Parse(start.ToString("dd.MM.yyyy HH:00"));
            // Schreibt das Datum solange bis es den Endwert überschreitet
            while(current <= end)
            {
                // Schreibt das Datum im Format dd.MM.yyyy HH:mm
                if((interval.TotalHours % 24) == 0)
                    worksheet.Cells[startRow, col] = current.ToString("dd.MM.yyyy");
                else
                    worksheet.Cells[startRow, col] = current.ToString("g");
                // Erhöht DateTime um den Wert des Intervalls
                current += interval;
                // Geht zur nächsten Zeile
                startRow++;
            }
            // Gibt die Zeilennummer der letzten gefüllte Zelle zurück
            return startRow - 1;
        }

        /// <summary>
        /// Search the row number with the value or between previous and current value. If not found return -1.
        /// </summary>
        /// <param name="value">The value that should search.</param>
        /// <param name="startRow">From this row should search.</param>
        /// <param name="endRow">Search until the value of endRow.</param>
        /// <param name="col">Search in that column.</param>
        /// <returns>Return the row number with the right date time.</returns>
        public virtual int SearchDateTime(DateTime value, int startRow, int endRow, int col)
        {
            // Der Wert der Spalte kann nicht kleiner als 1 sein
            if(col < 1)
                throw new IndexOutOfRangeException("Der Wert der Spalte kann nicht kleiner als 1 sein.");

            // Der Endwert kann nicht kleiner als der Startwert sein
            if(endRow < startRow)
                throw new ArgumentException("Der Endwert kann nicht kleiner als der Startwert sein.");

            // Startwerte erstellen
            int previousNumber = -1;
            int currentNumber = startRow;
            DateTime previous = default(DateTime);

            // Läuft bis zur ersten leeren Zelle durch
            while(currentNumber <= endRow)
            {
                // Prüft, ob gültiges Datum
                DateTime current = default(DateTime);
                // Speichert den Wert der Zeile
                string val = this[currentNumber, col];
                // Prüft, ob es in ein Datum umgewandelt werden kann
                if(DateTime.TryParse(val, out current))
                {
                    // Datum und Zeit stimmen überein
                    if(current == value)
                        return currentNumber;
                    // Prüft, ob der Zeitpunkt zwischen der vorherigen und aktuellen Zeile befindet
                    else if(value < current && value >= previous)
                        return previousNumber;
                    // Speichert die aktuelle Zeile um sie mit der nächsten Zeile zu nutzen
                    previous = current;
                    previousNumber = currentNumber;
                }
                // Wenn der letzte Eintrag erreicht wurde
                else if(val == null)
                    return previousNumber;
                // Zur nächsten Zeile springen
                currentNumber++;
            }
            // Wert ist grösser als der letzte Wert
            return endRow;
        }
    }
}
