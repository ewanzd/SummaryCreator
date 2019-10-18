using System;

namespace SummaryCreator.Data
{
    public class DataPoint
    {
        public DateTime CapturedAt { get; set; }
        public double Value { get; set; }

        public override string ToString()
        {
            return $"{CapturedAt.ToString()}: {Value}";
        }
    }
}