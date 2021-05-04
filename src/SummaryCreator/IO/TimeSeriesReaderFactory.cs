﻿using SummaryCreator.Configuration;
using SummaryCreator.IO.Csv;
using System;
using System.IO;

namespace SummaryCreator.IO
{
    public class TimeSeriesReaderFactory : ITimeSeriesReaderFactory
    {
        private static readonly ITimeSeriesReader SelReader = new SensorCsvReader();
        private static readonly ITimeSeriesReader Selv2Reader = new DbdataSensorCsvReader();

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
            return format switch
            {
                EnergySourceFormat.Sel => SelReader,
                EnergySourceFormat.Selv2 => Selv2Reader,
                _ => throw new InvalidDataException($"Format {sensorConfig.Format} not found"),
            };
        }
    }
}
