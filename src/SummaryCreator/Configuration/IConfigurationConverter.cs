using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SummaryCreator.Configuration
{
    /// <summary>
    /// Convert a stream to <see cref="SummaryCreatorConfig"/>.
    /// </summary>
    public interface IConfigurationConverter
    {
        /// <summary>
        /// Convert stream to <see cref="SummaryCreatorConfig"/>.
        /// </summary>
        /// <param name="contentStream">Stream with configurations.</param>
        /// <param name="cancellationToken">A token that may be used to cancel the read operation.</param>
        /// <returns>Converted configuration.</returns>
        Task<SummaryCreatorConfig> ConvertAsync(Stream contentStream, CancellationToken cancellationToken = default);
    }

    public enum SensorContentFormat
    {
        Unknown,
        Sel,
        Selv2
    }

    public class SummaryCreatorConfig : IEquatable<SummaryCreatorConfig>
    {
        public TimeSeriesConfig TimeSeries { get; set; }

        public IEnumerable<ExcelConfig> Excel { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SummaryCreatorConfig);
        }

        public bool Equals(SummaryCreatorConfig other)
        {
            return other != null &&
                   EqualityComparer<TimeSeriesConfig>.Default.Equals(TimeSeries, other.TimeSeries) &&
                   EqualityComparer<IEnumerable<ExcelConfig>>.Default.Equals(Excel, other.Excel);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TimeSeries, Excel);
        }
    }

    public class TimeSeriesConfig : IEquatable<TimeSeriesConfig>
    {
        public IEnumerable<MeteoConfig> Meteo { get; set; }

        public IEnumerable<SensorConfig> Sensors { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as TimeSeriesConfig);
        }

        public bool Equals(TimeSeriesConfig other)
        {
            return other != null &&
                   EqualityComparer<IEnumerable<MeteoConfig>>.Default.Equals(Meteo, other.Meteo) &&
                   EqualityComparer<IEnumerable<SensorConfig>>.Default.Equals(Sensors, other.Sensors);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Meteo, Sensors);
        }
    }

    public class MeteoConfig : IEquatable<MeteoConfig>
    {
        public string Resource { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as MeteoConfig);
        }

        public bool Equals(MeteoConfig other)
        {
            return other != null &&
                   Resource == other.Resource;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Resource);
        }
    }

    public class SensorConfig : IEquatable<SensorConfig>
    {
        public SensorContentFormat Format { get; set; }

        public string Resource { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SensorConfig);
        }

        public bool Equals(SensorConfig other)
        {
            return other != null &&
                   Format == other.Format &&
                   Resource == other.Resource;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Format, Resource);
        }
    }

    public class ExcelConfig : IEquatable<ExcelConfig>
    {
        public string Resource { get; set; }

        public string Sheet { get; set; }

        public int Row { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ExcelConfig);
        }

        public bool Equals(ExcelConfig other)
        {
            return other != null &&
                   Resource == other.Resource &&
                   Sheet == other.Sheet &&
                   Row == other.Row;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Resource, Sheet, Row);
        }
    }
}
