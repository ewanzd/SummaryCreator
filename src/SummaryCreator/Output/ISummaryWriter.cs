using SummaryCreator.Configuration;
using SummaryCreator.Core;
using System.Collections.Generic;
using System.IO;

namespace SummaryCreator.Output
{
    public interface ISummaryWriter
    {
        void Write(IEnumerable<ITimeSeries> timeSeriesGroup, Stream contentStream, SummaryConfig excelConfig);
    }
}