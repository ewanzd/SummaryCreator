using AutomaticSummaryCreator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AutomaticSummaryCreator.IO.Csv
{
    public class NewSensorCsvReader : IDataReader
    {
        private const char fileNameSeparator = '_';
        private const char rowSeperator = ',';

        private readonly FileInfo sourceFile;

        public NewSensorCsvReader(FileInfo sourceFile)
        {
            Debug.Assert(sourceFile != null, $"{nameof(sourceFile)} must not be null");

            this.sourceFile = sourceFile;
        }

        public IEnumerable<IDataContainer> Read()
        {
            var id = ExtractId(sourceFile);
            var dataContainer = new SensorTimeSeries(id);

            // enumerator holen um durch zu iterieren
            var fileEnumerator = ReadFile(sourceFile).GetEnumerator();

            // erste zeile überspringen
            fileEnumerator.MoveNext();

            // alle Zeilen in Objekte konvetieren und zurückgeben
            while (fileEnumerator.MoveNext())
            {
                var row = fileEnumerator.Current;
                var dataPoint = ConvertToEntry(row, rowSeperator);
                dataContainer.Add(dataPoint);
            }

            return new List<IDataContainer>() { dataContainer };
        }

        /// <summary>
        /// Create a new row with data from string.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="separator"></param>
        /// <returns>Return a new full row.</returns>
        private DataPoint ConvertToEntry(string row, char separator)
        {
            DataPoint dataPoint = new DataPoint();

            var fields = GetFields(row, separator);

            if (fields.Length != 2)
            {
                throw new InvalidDataException($"Ungültiges format: {row}");
            }

            // Wert konvertieren
            if (double.TryParse(fields[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
            {
                dataPoint.Value = val;
            }
            else
            {
                throw new InvalidDataException($"Ungültiges format: {fields[0]}");
            }

            // Datum konvertieren
            if (DateTime.TryParse(fields[1], out DateTime dtTemp))
            {
                dataPoint.CapturedAt = dtTemp;
            }
            else
            {
                throw new InvalidDataException($"Ungültiges format: {fields[1]}");
            }

            return dataPoint;
        }

        /// <summary>
        /// Get all rows from file with IEnumerable.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="separator">The delimiter for split the row in cells.</param>
        /// <returns>Return a row at array with cells as string[].</returns>
        private static IEnumerable<string> ReadFile(FileInfo file)
        {
            using (StreamReader reader = file.OpenText())
            {
                while (reader.EndOfStream == false)
                {
                    yield return reader.ReadLine();
                }
            }
        }

        private static string[] GetFields(string row, char separator)
        {
            return row.Split(separator);
        }

        /// <summary>
        /// Extract sensor id from file name.
        /// </summary>
        /// <param name="file">Path to file with data.</param>
        /// <returns>Id of sensor.</returns>
        private string ExtractId(FileInfo file)
        {
            // ID des Zählers des Dateinamens
            // Beispiel: dbdata_6F5CBF4A-FC2F-4E67-99A6-3AFB3D9C2E46.csv
            var fileName = Path.GetFileNameWithoutExtension(file.FullName);
            return fileName.Split(fileNameSeparator)[1];
        }
    }
}
