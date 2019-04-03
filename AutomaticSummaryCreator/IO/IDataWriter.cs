using AutomaticSummaryCreator.Data;
using System.Collections.Generic;

namespace AutomaticSummaryCreator.IO
{
    public interface IDataWriter
    {
        void Write(IEnumerable<IDataContainer> containers);
    }
}
