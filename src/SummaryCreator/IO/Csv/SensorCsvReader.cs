using SummaryCreator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace SummaryCreator.IO.Csv
{
    public sealed class SensorCsvReader : IDataReader
    {
        private const char rowSeperator = ';';

        private readonly FileInfo sourceFile;

        public SensorCsvReader(FileInfo sourceFile)
        {
            Debug.Assert(sourceFile != null, $"{nameof(sourceFile)} must not be null.");

            this.sourceFile = sourceFile;
        }

        public IEnumerable<IDataContainer> Read()
        {
            var containers = new Dictionary<string, IDataContainer>();

            // get file content enumerator
            var fileEnumerator = ReadFile(sourceFile).GetEnumerator();

            // skip first line
            fileEnumerator.MoveNext();

            // convert all row to objects
            IDataContainer dataContainer;
            while (fileEnumerator.MoveNext())
            {
                var row = fileEnumerator.Current;
                var entry = ConvertToEntry(row, rowSeperator);

                // check if id is available, otherwise create new container
                if (containers.TryGetValue(entry.id, out dataContainer))
                {
                    dataContainer.Add(entry.dp);
                }
                else
                {
                    dataContainer = new SensorContainer(entry.id);
                    dataContainer.Add(entry.dp);
                    containers.Add(entry.id, dataContainer);
                }
            }

            return new List<IDataContainer>(containers.Values);
        }

        /// <summary>
        /// Create a new row with data from string.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="separator"></param>
        /// <returns>Return a new full row.</returns>
        private (string id, DataPoint dp) ConvertToEntry(string row, char separator)
        {
            string id = string.Empty;
            DataPoint dataPoint = new DataPoint();

            var fields = GetFields(row, separator);

            if (fields.Length < 8)
            {
                throw new InvalidDataException($"Invalid format: {row}");
            }

            // convert date
            if (DateTime.TryParse(fields[0], out DateTime dtTemp))
            {
                dataPoint.CapturedAt = dtTemp;
            }
            else
            {
                throw new InvalidDataException($"Invalid format: {fields[0]}");
            }

            // get sensor id
            id = fields[1];

            // convert total if available, otherwise calculate it
            if (double.TryParse(fields[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double total))
            {
                dataPoint.Value = total;
            }
            else
            {
                // convert second value
                if (double.TryParse(fields[4], NumberStyles.Any, CultureInfo.InvariantCulture, out double val1))
                {
                    dataPoint.Value = val1;
                }
                else
                {
                    throw new InvalidDataException($"Invalid format: {fields[4]}");
                }

                // convert second value
                if (double.TryParse(fields[6], NumberStyles.Any, CultureInfo.InvariantCulture, out double val2))
                {
                    dataPoint.Value += val2;
                }
                else
                {
                    throw new InvalidDataException($"Invalid format: {fields[6]}");
                }
            }

            return (id, dataPoint);
        }

        /// <summary>
        /// Get all rows from file with IEnumerable.
        /// </summary>
        /// <param name="path">The path to the file.</param>
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
    }
}