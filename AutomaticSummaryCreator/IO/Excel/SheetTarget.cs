using BasicLibrary.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticSummaryCreator.IO.Excel
{
    public class SheetTarget : Sheet
    {
        public int IndexOfRow(string value, int col = 1)
        {
            return IndexOfRow(value, 1, CountRow, col);
        }

        public int IndexOfRow(string value, int startRow, int endRow, int col = 1)
        {
            if(startRow < 1)
                throw new ArgumentNullException("startRow");

            if(endRow > CountRow)
                throw new ArgumentNullException("endRow");

            if(col < 1)
                throw new ArgumentNullException("col");

            // Der Endwert kann nicht kleiner als der Startwert sein
            if(endRow < startRow)
                throw new ArgumentException("Der Endwert kann nicht kleiner als der Startwert sein.");

            // Startwerte erstellen
            int currentNumber = startRow;

            // Läuft bis zur ersten leeren Zelle durch
            while(currentNumber <= endRow)
            {
                // Werte stimmen überein
                if(this[currentNumber, col] == value)
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
            if(startCol < 1)
                throw new ArgumentNullException("startRow");

            if(endCol > CountCol)
                throw new ArgumentNullException("endRow");

            if(row < 1)
                throw new ArgumentNullException("col");

            // Der Endwert kann nicht kleiner als der Startwert sein
            if(endCol < startCol)
                throw new ArgumentException("Der Endwert kann nicht kleiner als der Startwert sein.");

            // Startwerte erstellen
            int currentNumber = startCol;

            // Läuft bis zur ersten leeren Zelle durch
            while(currentNumber <= endCol)
            {
                // Werte stimmen überein
                if(this[row, currentNumber] == value)
                    return currentNumber;

                // Zur nächsten Zeile springen
                currentNumber++;
            }
            // Wert ist grösser als der letzte Wert
            return -1;
        }
    }
}
