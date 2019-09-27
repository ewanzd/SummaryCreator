using SummaryCreator.Data;
using System.Collections.Generic;

namespace SummaryCreator.IO
{
    public interface IDataWriter
    {
        void Write(IEnumerable<IDataContainer> containers);
    }
}