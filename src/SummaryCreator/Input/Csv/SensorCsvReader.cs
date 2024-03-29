﻿using SummaryCreator.Core;
using SummaryCreator.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SummaryCreator.Input.Csv
{
    /// <summary>
    /// Read sensor data (old format).
    /// </summary>
    public sealed class SensorCsvReader : ITimeSeriesReader
    {
        private const char rowSeperator = ';';
        private const string dateTimeFormat = "dd.MM.yyyy HH:mm:ss";
        private static readonly CultureInfo culture = CultureInfo.InvariantCulture;

        public IEnumerable<ITimeSeries> Read(string id, string content)
        {
            var timeSeries = new SensorTimeSeries(id);

            var contentEnumerator = content.SplitLines();

            // skip first line of csv 
            contentEnumerator.MoveNext();

            // convert all data to internal data structure
            foreach (ReadOnlySpan<char> line in contentEnumerator)
            {
                var dataPoint = ConvertToEntry(line, rowSeperator);
                timeSeries.Add(dataPoint);
            }

            return new List<ITimeSeries>() { timeSeries };
        }

        /// <summary>
        /// Create a new row with data from string.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="separator"></param>
        /// <returns>Return a new full row.</returns>
        private static DataPoint ConvertToEntry(ReadOnlySpan<char> line, char separator)
        {
            DataPoint dataPoint = new DataPoint();

            var lineEntries = ExtractLineEntries(line, separator);

            dataPoint.CapturedAt = lineEntries.DateTimeEntry;

            if(lineEntries.EnergyTotalEntry != default)
            {
                dataPoint.Value = lineEntries.EnergyTotalEntry;
            }
            else
            {
                if (lineEntries.EnergyTarif1Entry != default)
                {
                    dataPoint.Value = lineEntries.EnergyTarif1Entry;
                }
                if (lineEntries.EnergyTarif2Entry != default)
                {
                    dataPoint.Value += lineEntries.EnergyTarif2Entry;
                }
            }

            return dataPoint;
        }

        private static LineEntries ExtractLineEntries(ReadOnlySpan<char> line, char separator)
        {
            LineEntries lineEntries = default;

            for (int i = 0; i < 8; i++)
            {
                var index = line.IndexOf(separator);
                ReadOnlySpan<char> entrySpan;

                if (index == -1 && i < 7)
                {
                    throw new InvalidDataException($"Invalid format: {line.ToString()}");
                }
                else if (index == -1 && i == 7)
                {
                    entrySpan = line.Trim();
                }
                else
                {
                    entrySpan = line.Slice(0, index).Trim();
                }

                if (i == 0)
                {
                    // convert date
                    if (DateTime.TryParseExact(entrySpan, dateTimeFormat, culture, DateTimeStyles.None, out DateTime dtTemp))
                    {
                        lineEntries.DateTimeEntry = DateTime.SpecifyKind(dtTemp, DateTimeKind.Local);
                    }
                    else
                    {
                        throw new InvalidDataException($"Invalid format: {entrySpan.ToString()}");
                    }
                }
                else if (i == 1)
                {
                    // get sensor id
                    lineEntries.SerialNumberEntry = entrySpan.ToString();
                }
                else if (i == 2)
                {
                    // convert total if available, otherwise calculate it
                    if (double.TryParse(entrySpan, NumberStyles.Any, CultureInfo.InvariantCulture, out double total))
                    {
                        lineEntries.EnergyTotalEntry = total;
                    }
                }
                else if (i == 4)
                {
                    // convert total if available, otherwise calculate it
                    if (double.TryParse(entrySpan, NumberStyles.Any, CultureInfo.InvariantCulture, out double tarif1))
                    {
                        lineEntries.EnergyTarif1Entry = tarif1;
                    }
                }
                else if (i == 6)
                {
                    // convert total if available, otherwise calculate it
                    if (double.TryParse(entrySpan, NumberStyles.Any, CultureInfo.InvariantCulture, out double tarif2))
                    {
                        lineEntries.EnergyTarif2Entry = tarif2;
                    }
                }

                line = line[(index + 1)..];
            }

            return lineEntries;
        }

        private struct LineEntries
        {
            public DateTime DateTimeEntry;
            public string SerialNumberEntry;
            public double EnergyTotalEntry;
            public double EnergyTarif1Entry;
            public double EnergyTarif2Entry;
        }
    }
}