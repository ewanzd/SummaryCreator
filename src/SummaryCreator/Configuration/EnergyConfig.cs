using System;

namespace SummaryCreator.Configuration
{
    public class EnergyConfig : IEquatable<EnergyConfig>
    {
        public EnergyConfig(string id, string resource, EnergySourceFormat format)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
            Format = format;
        }

        public string Id { get; }

        public string Resource { get; }

        public EnergySourceFormat Format { get; }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as EnergyConfig);
        }

        public bool Equals(EnergyConfig other)
        {
            return other != null &&
                   Resource == other.Resource;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Resource);
        }
    }
}
