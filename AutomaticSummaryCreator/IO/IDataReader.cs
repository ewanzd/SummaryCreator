using AutomaticSummaryCreator.Data;
using System.Collections.Generic;

namespace AutomaticSummaryCreator.IO
{
    public interface IDataReader
    {
        IEnumerable<IDataContainer> Read();
    }
}
