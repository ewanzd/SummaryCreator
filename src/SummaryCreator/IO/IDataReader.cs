using SummaryCreator.Data;
using System.Collections.Generic;

namespace SummaryCreator.IO
{
    public interface IDataReader
    {
        IEnumerable<IDataContainer> Read();
    }
}
