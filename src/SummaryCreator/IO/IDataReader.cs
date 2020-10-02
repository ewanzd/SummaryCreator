using SummaryCreator.Core;
using System.Collections.Generic;

namespace SummaryCreator.IO
{
    public interface IDataReader
    {
        IEnumerable<ITimeSeries> Read();
    }
}