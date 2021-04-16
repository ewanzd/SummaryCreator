using System;

namespace SummaryCreator.Configuration
{
    public class SummaryConfig : IEquatable<SummaryConfig>
    {
        public SummaryConfig(string resource, string sheet, int row)
        {
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
            Sheet = sheet ?? throw new ArgumentNullException(nameof(sheet));
            Row = row;
        }

        public string Resource { get; }

        public string Sheet { get; }

        public int Row { get; }

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
