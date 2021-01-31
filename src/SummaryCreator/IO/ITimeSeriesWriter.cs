using SummaryCreator.Core;
using System.Collections.Generic;

namespace SummaryCreator.IO
{
    public interface ITimeSeriesWriter
    {
        void Write(IEnumerable<ITimeSeries> timeSeriesGroup);
    }
}