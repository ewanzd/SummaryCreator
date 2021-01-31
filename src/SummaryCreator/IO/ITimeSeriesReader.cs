using SummaryCreator.Core;
using System.Collections.Generic;

namespace SummaryCreator.IO
{
    public interface ITimeSeriesReader
    {
        IEnumerable<ITimeSeries> Read();
    }
}