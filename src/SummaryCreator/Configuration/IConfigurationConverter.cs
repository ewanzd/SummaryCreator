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

    public enum EnergySourceFormat
    {
        Unknown,
        Sel,
        Selv2
    }

    public enum ResourceType
    {
        Unknown,
        Directory,
        File,
        Web
    }

    public class SummaryCreatorConfig : IEquatable<SummaryCreatorConfig>
    {
        public IEnumerable<MeteoConfig> MeteoConfigs { get; set; }

        public IEnumerable<EnergyConfig> EnergyConfigs { get; set; }

        public IEnumerable<SummaryConfig> SummaryConfigs { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SummaryCreatorConfig);
        }

        public bool Equals(SummaryCreatorConfig other)
        {
            return other != null &&
                   EqualityComparer<IEnumerable<MeteoConfig>>.Default.Equals(MeteoConfigs, other.MeteoConfigs) &&
                   EqualityComparer<IEnumerable<EnergyConfig>>.Default.Equals(EnergyConfigs, other.EnergyConfigs) &&
                   EqualityComparer<IEnumerable<SummaryConfig>>.Default.Equals(SummaryConfigs, other.SummaryConfigs);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MeteoConfigs, EnergyConfigs, SummaryConfigs);
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

    public class EnergyConfig : IEquatable<EnergyConfig>
    {
        public EnergySourceFormat Format { get; set; }

        public ResourceType ResourceType { get; set; }

        public string Resource { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as EnergyConfig);
        }

        public bool Equals(EnergyConfig other)
        {
            return other != null &&
                   Format == other.Format &&
                   ResourceType == other.ResourceType &&
                   Resource == other.Resource;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Format, Resource);
        }
    }

    public class SummaryConfig : IEquatable<SummaryConfig>
    {
        public string Resource { get; set; }

        public string Sheet { get; set; }

        public int Row { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SummaryConfig);
        }

        public bool Equals(SummaryConfig other)
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
