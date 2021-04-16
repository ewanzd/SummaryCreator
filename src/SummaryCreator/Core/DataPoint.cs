using System;
using System.Globalization;

namespace SummaryCreator.Core
{
    public sealed class DataPoint
    {
        public DateTimeOffset CapturedAt { get; set; }
        public double Value { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DataPoint point &&
                   CapturedAt.Equals(point.CapturedAt) &&
                   Value == point.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CapturedAt, Value);
        }

        public override string ToString()
        {
            return $"{CapturedAt.ToString(CultureInfo.InvariantCulture)}: {Value}";
        }
    }
}