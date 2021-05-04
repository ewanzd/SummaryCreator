using SummaryCreator.Core;
using System;
using System.Collections.Generic;

namespace SummaryCreator.Input
{
    public interface ITimeSeriesReader
    {
        IEnumerable<ITimeSeries> Read(string resource, string content);
    }
}