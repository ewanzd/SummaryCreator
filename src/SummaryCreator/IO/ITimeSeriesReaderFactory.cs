using SummaryCreator.Configuration;

namespace SummaryCreator.IO
{
    public interface ITimeSeriesReaderFactory
    {
        /// <summary>
        /// Evaluates and creates the coresponding sensor reader.
        /// </summary>
        /// <param name="sensorConfig">Config of the sensor.</param>
        /// <returns>Sensor reader.</returns>
        ITimeSeriesReader CreateSensorReader(SensorConfig sensorConfig);
    }
}
