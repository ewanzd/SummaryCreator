using SummaryCreator.Configuration;
using SummaryCreator.Input.Csv;
using System;
using System.IO;

namespace SummaryCreator.Input
{
    public class TimeSeriesReaderFactory : ITimeSeriesReaderFactory
    {
        private static readonly ITimeSeriesReader SelReader = new SensorCsvReader();
        private static readonly ITimeSeriesReader Selv2Reader = new DbdataSensorCsvReader();
        private static readonly ITimeSeriesReader Selv3Reader = new ApiSelEnergyJsonReader();

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
                EnergySourceFormat.Selv3 => Selv3Reader,
                _ => throw new InvalidDataException($"Format {sensorConfig.Format} not found"),
            };
        }
    }
}
