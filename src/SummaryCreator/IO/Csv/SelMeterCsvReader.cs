using SummaryCreator.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SummaryCreator.IO.Csv
{
    public class SelMeterCsvReader : IDataReader
    {
        private const char fileNameSeparator = '_';
        private const char rowSeperator = ';';

        private readonly FileInfo sourceFile;

        public SelMeterCsvReader(FileInfo sourceFile)
        {
            this.sourceFile = sourceFile ?? throw new ArgumentNullException(nameof(sourceFile));
        }

        public IEnumerable<ITimeSeries> Read()
        {
            var id = ExtractId(sourceFile);
            var sensorTImeSeries = new SensorTimeSeries(id);

            // get file content enumerator
            var fileEnumerator = ReadFile(sourceFile).GetEnumerator();

            // skip first line
            fileEnumerator.MoveNext();

            // convert all row to objects
            while (fileEnumerator.MoveNext())
            {
                var row = fileEnumerator.Current;
                var dataPoint = ConvertToEntry(row, rowSeperator);
                sensorTImeSeries.Add(dataPoint);
            }

            return new List<ITimeSeries>() { sensorTImeSeries };
        }

        /// <summary>
        /// Create a new row with data from string.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="separator"></param>
        /// <returns>Return a new full row.</returns>
        private static DataPoint ConvertToEntry(string row, char separator)
        {
            DataPoint dataPoint = new DataPoint();

            var fields = GetFields(row, separator);

            if (fields.Length != 3)
            {
                throw new InvalidDataException($"Invalid format: {row}");
            }

            // convert date
            if (DateTimeOffset.TryParse(fields[0], out DateTimeOffset dtTemp))
            {
                dataPoint.CapturedAt = dtTemp;
            }
            else
            {
                throw new InvalidDataException($"Invalid format: {fields[0]}");
            }

            // convert import value
            if (double.TryParse(fields[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double importValue))
            {
                dataPoint.Value = importValue;
            }
            else
            {
                throw new InvalidDataException($"Invalid format: {fields[1]}");
            }

            // convert export value
            if (double.TryParse(fields[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double exportValue))
            {
                dataPoint.Value += exportValue;
            }
            else
            {
                throw new InvalidDataException($"Invalid format: {fields[2]}");
            }

            return dataPoint;
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
            return row.Split(separator).Select(x => x.Trim('"')).ToArray();
        }

        /// <summary>
        /// Extract sensor id from file name.
        /// </summary>
        /// <param name="file">Path to file with data.</param>
        /// <returns>Id of sensor.</returns>
        private static string ExtractId(FileInfo file)
        {
            // id of the sensor in file name
            // Example: 6F5CBF4A_sel_meter_export_2020-05-01-2020-05-13.csv
            var fileName = Path.GetFileNameWithoutExtension(file.FullName);
            return fileName.Split(fileNameSeparator)[0];
        }
    }
}
