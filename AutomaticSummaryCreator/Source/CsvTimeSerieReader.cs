using AutomaticSummaryCreator.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutomaticSummaryCreator.Source
{
    /// <summary>
    /// Read from any text based file.
    /// </summary>
    public class CsvTimeSerieReader
    {
        public char FieldSeperator { get; set; } = ',';

        public IEnumerable<Row> ReadRowsFromFile(FileInfo file)
        {
            if (!file.Exists) throw new FileNotFoundException($"Datei '{file.FullName}' wurde nicht gefunden");

            // enumerator holen um durch zu iterieren
            var fileEnumerator = ReadFile(file).GetEnumerator();

            // erste zeile überspringen
            fileEnumerator.MoveNext();

            // alle Zeilen in Objekte konvetieren und zurückgeben
            while(fileEnumerator.MoveNext())
            {
                var row = fileEnumerator.Current;
                yield return ConvertToEntry(row, FieldSeperator);
            }
        }

        /// <summary>
        /// Create a new row with data from string.
        /// </summary>
        /// <param name="rowStr"></param>
        /// <param name="separator"></param>
        /// <returns>Return a new full row.</returns>
        private Row ConvertToEntry(string rowStr, char separator)
        {
            Row row = new Row();

            var fields = GetFields(rowStr, separator);

            if(fields.Length != 2)
            {
                throw new InvalidDataException($"Ungültiges format: {rowStr}");
            }

            // Wert konvertieren
            if(double.TryParse(fields[0], out double val))
            {
                row.Value = val;
            }
            else
            {
                throw new InvalidDataException($"Ungültiges format: {fields[0]}");
            }

            // Datum konvertieren
            if(DateTime.TryParse(fields[1], out DateTime dtTemp))
            {
                row.CapturedAt = dtTemp;
            }
            else
            {
                throw new InvalidDataException($"Ungültiges format: {fields[1]}");
            }

            return row;
        }

        /// <summary>
        /// Get all rows from file with IEnumerable.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="separator">The delimiter for split the row in cells.</param>
        /// <returns>Return a row at array with cells as string[].</returns>
        private static IEnumerable<string> ReadFile(FileInfo file)
        {
            using(StreamReader reader = file.OpenText())
            {
                while(reader.EndOfStream == false)
                {
                    yield return reader.ReadLine();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        private static string[] GetFields(string row, char separator)
        {
            return row.Split(separator);
        }

        ///// <summary>
        ///// Get data from file and analyze it.
        ///// </summary>
        ///// <param name="path">The path to the file.</param>
        //public static void ImportFromCSV(string file, Summary summary)
        //{
        //    // read
        //    // 

        //    // Speichert den Wert, ob die Schleife das erste Mal ausgeführt wird
        //    bool first = true;

        //    // Holt die Daten Zeile für Zeile aus der Datei
        //    foreach(var row in CsvSource.ReadFile(file))
        //    {
        //        // Titelzeile auswerten
        //        if(first)
        //        {
        //            first = false;
        //            continue;
        //        }

        //        // Daten auswerten
        //        try
        //        {
        //            // ID des Zählers from file name
        //            // example: dbdata_6F5CBF4A-FC2F-4E67-99A6-3AFB3D9C2E46.csv
        //            var fileName = Path.GetFileNameWithoutExtension(file);
        //            var id = fileName.Split(FILENAME_SEPARATOR)[1];

        //            // Werte einfügen
        //            var item = ConvertToRow(row);

        //            // Zeile hinzufügen
        //            summary.Add(id, item);
        //        }
        //        catch
        //        {
        //            continue;
        //        }
        //    }
        //}
    }
}
