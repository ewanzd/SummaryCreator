﻿using SummaryCreator.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SummaryCreator.IO.Csv
{
    /// <summary>
    /// Read sensor data file with new format (file name starts with dbdata, content is time series).
    /// </summary>
    public sealed class DbdataSensorCsvReader : IDataReader
    {
        private const char fileNameSeparator = '_';
        private const char rowSeperator = ',';

        private readonly FileInfo sourceFile;

        public DbdataSensorCsvReader(FileInfo sourceFile)
        {
            this.sourceFile = sourceFile ?? throw new ArgumentNullException(nameof(sourceFile));
        }

        public IEnumerable<ITimeSeries> Read()
        {
            var id = ExtractId(sourceFile);
            var sensorTimeSeries = new SensorTimeSeries(id);

            // get file content enumerator
            var fileEnumerator = ReadFile(sourceFile).GetEnumerator();

            // skip first line
            fileEnumerator.MoveNext();

            // convert all row to objects
            while (fileEnumerator.MoveNext())
            {
                var row = fileEnumerator.Current;
                var dataPoint = ConvertToEntry(row, rowSeperator);
                sensorTimeSeries.Add(dataPoint);
            }

            return new List<ITimeSeries>() { sensorTimeSeries };
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
                throw new InvalidDataException($"Invalid format: {row}");
            }

            // convert value
            if (double.TryParse(fields[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
            {
                dataPoint.Value = val;
            }
            else
            {
                throw new InvalidDataException($"Invalid format: {fields[0]}");
            }

            // convert date
            if (DateTime.TryParse(fields[1], out DateTime dtTemp))
            {
                dataPoint.CapturedAt = DateTime.SpecifyKind(dtTemp, DateTimeKind.Local);
            }
            else
            {
                throw new InvalidDataException($"Invalid format: {fields[1]}");
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
            return row.Split(separator);
        }

        /// <summary>
        /// Extract sensor id from file name.
        /// </summary>
        /// <param name="file">Path to file with data.</param>
        /// <returns>Id of sensor.</returns>
        private string ExtractId(FileInfo file)
        {
            // id of the sensor in file name
            // Example: dbdata_6F5CBF4A-FC2F-4E67-99A6-3AFB3D9C2E46.csv
            var fileName = Path.GetFileNameWithoutExtension(file.FullName);
            return fileName.Split(fileNameSeparator)[1];
        }
    }
}