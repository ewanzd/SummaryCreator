using System;

namespace SummaryCreator.Configuration
{
    public class MeteoConfig : IEquatable<MeteoConfig>
    {
        public MeteoConfig(string resource)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }

        public string Resource { get; }

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
}
