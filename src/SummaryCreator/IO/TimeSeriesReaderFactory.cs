﻿using SummaryCreator.Configuration;
using SummaryCreator.IO.Csv;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SummaryCreator.IO
{
    public class TimeSeriesReaderFactory : ITimeSeriesReaderFactory
    {
        private static readonly ITimeSeriesReader SelReader = new SensorCsvReader();
        private static readonly ITimeSeriesReader Selv2Reader = new DbdataSensorCsvReader();

        private const char fileNameSeparator = '_';
        private const string dbdataIdenticator = "dbdata";

        public ITimeSeriesReader CreateSensorReader(EnergyConfig sensorConfig)
        {
            if (sensorConfig is null)
            {
                throw new ArgumentNullException(nameof(sensorConfig));
            }

            if (sensorConfig.Resource is null)
            {
                throw new ArgumentException("Must have a value", nameof(sensorConfig.Resource));
            }

            var format = sensorConfig.Format;
            var fileInfo = new FileInfo(sensorConfig.Resource);

            // if sensor content format is unknown, try to evaluate it
            if (format == EnergySourceFormat.Unknown)
                format = EvaluateContentFormat(fileInfo);

            return format switch
            {
                EnergySourceFormat.Sel => SelReader,
                EnergySourceFormat.Selv2 => Selv2Reader,
                _ => throw new InvalidDataException($"Format {sensorConfig.Format} not found"),
            };
        }

        private static EnergySourceFormat EvaluateContentFormat(FileInfo file)
        {
            Debug.Assert(file != null, $"{nameof(file)} must not be null.");

            if (IsDbdataSensor(file))
            {
                return EnergySourceFormat.Selv2;
            }
            else
            {
                return EnergySourceFormat.Sel;
            }
        }

        private static bool IsDbdataSensor(FileInfo file)
        {
            Debug.Assert(file != null, $"{nameof(file)} must not be null.");

            var fileName = Path.GetFileNameWithoutExtension(file.FullName);
            var filePrefix = fileName.Split(fileNameSeparator).FirstOrDefault();

            return filePrefix?.Equals(dbdataIdenticator, StringComparison.InvariantCulture) == true;
        }
    }
}
