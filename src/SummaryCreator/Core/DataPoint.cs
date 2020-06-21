using System;
using System.Globalization;

namespace SummaryCreator.Core
{
    public sealed class DataPoint
    {
        public DateTimeOffset CapturedAt { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return $"{CapturedAt.ToString(CultureInfo.InvariantCulture)}: {Value}";
        }
    }
}