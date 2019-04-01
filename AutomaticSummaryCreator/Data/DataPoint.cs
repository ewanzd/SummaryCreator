using System;

namespace AutomaticSummaryCreator.Data
{
    public sealed class DataPoint
    {
        DateTime CapturedAt { get; set; }
        double Value { get; set; }
    }
}
