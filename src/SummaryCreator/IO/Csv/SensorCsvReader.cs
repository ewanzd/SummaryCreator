using SummaryCreator.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SummaryCreator.IO.Csv
{
    /// <summary>
    /// Read sensor data (old format).
    /// </summary>
    public sealed class SensorCsvReader : ITimeSeriesReader
    {
        private const char rowSeperator = ';';

        private readonly FileInfo sourceFile;

        public SensorCsvReader(FileInfo sourceFile)
        {
            this.sourceFile = sourceFile ?? throw new ArgumentNullException(nameof(sourceFile));
        }

        public IEnumerable<ITimeSeries> Read()
        {
            var timeSeriesDict = new Dictionary<string, ITimeSeries>();

            // get file content enumerator
            var fileEnumerator = ReadFile(sourceFile).GetEnumerator();

            // skip first line
            fileEnumerator.MoveNext();
            while (fileEnumerator.MoveNext())
            {
                var row = fileEnumerator.Current;
                var (id, dataPoint) = ConvertToEntry(row, rowSeperator);

                // convert all row to objects
                // check if id is available, otherwise create new time series
                if (timeSeriesDict.TryGetValue(id, out ITimeSeries timeSeries))
                {
                    timeSeries.Add(dataPoint);
                }
                else
                {
                    timeSeries = new SensorTimeSeries(id);
                    timeSeries.Add(dataPoint);
                    timeSeriesDict.Add(id, timeSeries);
                }
            }

            return new List<ITimeSeries>(timeSeriesDict.Values);
        }

        /// <summary>
        /// Create a new row with data from string.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="separator"></param>
        /// <returns>Return a new full row.</returns>
        private (string id, DataPoint dp) ConvertToEntry(string row, char separator)
        {
            DataPoint dataPoint = new DataPoint();

            var fields = GetFields(row, separator);

            if (fields.Length < 8)
            {
                throw new InvalidDataException($"Invalid format: {row}");
            }

            // convert date
            if (DateTime.TryParse(fields[0], out DateTime dtTemp))
            {
                dataPoint.CapturedAt = DateTime.SpecifyKind(dtTemp, DateTimeKind.Local);
            }
            else
            {
                throw new InvalidDataException($"Invalid format: {fields[0]}");
            }

            // get sensor id
            string id = fields[1];

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
            using StreamReader reader = file.OpenText();
            while (!reader.EndOfStream)
            {
                yield return reader.ReadLine();
            }
        }

        private static string[] GetFields(string row, char separator)
        {
            return row.Split(separator);
        }
    }
}