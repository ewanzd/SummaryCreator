using SummaryCreator.Core;
using System;
using System.Collections.Generic;

namespace SummaryCreator.Input
{
    public interface ITimeSeriesReader
    {
        IEnumerable<ITimeSeries> Read(string id, string content);
    }
}