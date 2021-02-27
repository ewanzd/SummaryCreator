﻿using SummaryCreator.Core;
using SummaryCreator.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SummaryCreator.IO.Csv
{
    /// <summary>
    /// Read sensor data file with new format (file name starts with dbdata, content is time series).
    /// </summary>
    public sealed class DbdataSensorCsvReader : ITimeSeriesReader
    {
        private const char fileNameSeparator = '_';
        private const char rowSeperator = ',';
        private const string dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        private static readonly CultureInfo culture = CultureInfo.InvariantCulture;

        public IEnumerable<ITimeSeries> Read(string resource, string content)
        {
            var id = ExtractId(resource, fileNameSeparator);
            var sensorTimeSeries = new SensorTimeSeries(id);

            var contentEnumerator = content.SplitLines();

            // skip first line of csv 
            contentEnumerator.MoveNext();

            // convert all data to internal data structure
            foreach (ReadOnlySpan<char> line in contentEnumerator)
            {
                var dataPoint = ConvertToEntry(line, rowSeperator);
                sensorTimeSeries.Add(dataPoint);
            }

            return new List<ITimeSeries>() { sensorTimeSeries };
        }

        /// <summary>
        /// Create a new row with data from string.
        /// </summary>
        /// <param name="line">Csv content text line.</param>
        /// <param name="separator">Line entry seperator.</param>
        /// <returns>Return a new full row.</returns>
        private static DataPoint ConvertToEntry(ReadOnlySpan<char> line, char separator)
        {
            DataPoint dataPoint = new DataPoint();

            var index = line.IndexOf(separator);

            if(index == -1)
            {
                throw new InvalidDataException($"Invalid format: {line.ToString()}");
            }

            var valueSpan = line.Slice(0, index).Trim();

            // convert value
            if (double.TryParse(valueSpan, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
            {
                dataPoint.Value = val;
            }
            else
            {
                throw new InvalidDataException($"Invalid format: {valueSpan.ToString()}");
            }

            var dateTimeSpan = line[(index + 1)..].Trim();

            // convert date
            if (DateTime.TryParseExact(dateTimeSpan, dateTimeFormat, culture, DateTimeStyles.None, out DateTime dtTemp))
            {
                dataPoint.CapturedAt = DateTime.SpecifyKind(dtTemp, DateTimeKind.Local);
            }
            else
            {
                throw new InvalidDataException($"Invalid format: {dateTimeSpan.ToString()}");
            }

            return dataPoint;
        }

        /// <summary>
        /// Extract sensor id from file name.
        /// </summary>
        /// <param name="file">Path to file with data.</param>
        /// <param name="separator"></param>
        /// <returns>Id of sensor.</returns>
        private static string ExtractId(string file, char separator)
        {
            // id of the sensor in file name
            // Example: dbdata_6F5CBF4A-FC2F-4E67-99A6-3AFB3D9C2E46.csv
            var fileName = Path.GetFileNameWithoutExtension(file);
            var fileNameParts = fileName.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if(fileNameParts.Length < 2)
            {
                throw new ArgumentException("Invalid file name", nameof(file));
            }

            return fileNameParts[1];
        }
    }
}