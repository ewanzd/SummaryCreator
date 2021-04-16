using SummaryCreator.Configuration;
using SummaryCreator.Core;
using System.Collections.Generic;
using System.IO;

namespace SummaryCreator.IO
{
    public interface IExcelWriter
    {
        void Write(IEnumerable<ITimeSeries> timeSeriesGroup, Stream contentStream, SummaryConfig excelConfig);
    }
}