using AutomaticSummaryCreator.Data;
using BasicLibrary;
using BasicLibrary.Unit;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutomaticSummaryCreator.Source
{
    /// <summary>
    /// Read from any text based file.
    /// </summary>
    internal class CsvSource
    {
        /// <summary>
        /// Get data from file and analyze it.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        public static void ImportFromCSV(string file, Summary summary)
        {
            // Speichert den Wert, ob die Schleife das erste Mal ausgeführt wird
            bool first = true;

            // Holt die Daten Zeile für Zeile aus der Datei
            foreach(var row in CsvSource.ReadFile(file, ";"))
            {
                // Titelzeile auswerten
                if(first)
                {
                    first = false;
                    continue;
                }

                // Daten auswerten
                try
                {
                    // ID des Zählers
                    string id = row[1];

                    // Werte einfügen
                    var item = ConvertToRow(row);

                    // Zeile hinzufügen
                    summary.Add(id, item);
                }
                catch
                {
                    continue;
                }
            }
        }

        /// <summary>
        /// Create a new row with data from string array.
        /// </summary>
        /// <param name="row">Array with data from csv.</param>
        /// <returns>Return a new full row.</returns>
        static protected Row ConvertToRow(string[] row)
        {
            Row item = new Row();

            DateTime dtTemp;
            if(DateTime.TryParse(row[0], out dtTemp))
                item.DateTime = dtTemp;

            for(int i = 2; i < row.Length - 1; i += 6)
            {
                WattHour total;
                WattHour.TryParse(row[i], out total);

                if(total == 0)
                {
                    WattHour one, two;
                    WattHour.TryParse(row[i + 2], out one);
                    WattHour.TryParse(row[i + 4], out two);
                    total = one + two;
                }
                item.Add(total);
            }
            return item;
        }

        /// <summary>
        /// Get all rows from file with IEnumerable.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="splitter">The delimiter for split the row in cells.</param>
        /// <returns>Return a row at array with cells as string[].</returns>
        public static IEnumerable<string[]> ReadFile(string path, string splitter)
        {
            if(!File.Exists(path))
                throw new FileNotFoundException("Datei nicht gefunden", path);

            using(StreamReader reader = new StreamReader(path))
            {
                while(reader.EndOfStream == false)
                {
                    yield return reader.ReadLine().Split(splitter.ToCharArray());
                }
            }
        }
    }
}
