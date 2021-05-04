using SummaryCreator.Configuration;

namespace SummaryCreator.Input
{
    public interface ITimeSeriesReaderFactory
    {
        /// <summary>
        /// Evaluates and creates the coresponding sensor reader.
        /// </summary>
        /// <param name="sensorConfig">Config of the sensor.</param>
        /// <returns>Sensor reader.</returns>
        ITimeSeriesReader CreateSensorReader(EnergyConfig sensorConfig);
    }
}
