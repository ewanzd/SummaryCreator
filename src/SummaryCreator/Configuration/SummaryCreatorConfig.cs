using System;
using System.Collections.Generic;
using System.Linq;

namespace SummaryCreator.Configuration
{
    public class SummaryCreatorConfig : IEquatable<SummaryCreatorConfig>
    {
        public SummaryCreatorConfig(IReadOnlySet<MeteoConfig> meteoConfigs, IReadOnlySet<EnergyConfig> energyConfigs, IReadOnlySet<SummaryConfig> summaryConfigs)
        {
            MeteoConfigs = meteoConfigs ?? Enumerable.Empty<MeteoConfig>().ToHashSet();
            EnergyConfigs = energyConfigs ?? Enumerable.Empty<EnergyConfig>().ToHashSet();
            SummaryConfigs = summaryConfigs ?? Enumerable.Empty<SummaryConfig>().ToHashSet();
        }

        public IReadOnlySet<MeteoConfig> MeteoConfigs { get; }

        public IReadOnlySet<EnergyConfig> EnergyConfigs { get; }

        public IReadOnlySet<SummaryConfig> SummaryConfigs { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as SummaryCreatorConfig);
        }

        public bool Equals(SummaryCreatorConfig other)
        {
            return other != null &&
                   EqualityComparer<IReadOnlySet<MeteoConfig>>.Default.Equals(MeteoConfigs, other.MeteoConfigs) &&
                   EqualityComparer<IReadOnlySet<EnergyConfig>>.Default.Equals(EnergyConfigs, other.EnergyConfigs) &&
                   EqualityComparer<IReadOnlySet<SummaryConfig>>.Default.Equals(SummaryConfigs, other.SummaryConfigs);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MeteoConfigs, EnergyConfigs, SummaryConfigs);
        }
    }
}
