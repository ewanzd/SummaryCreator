using System;
using System.Collections;
using System.Collections.Generic;

namespace AutomaticSummaryCreator.Data
{
    public sealed class DataGroup : IEnumerable<IDataContainer>
    {
        public void Add(IDataContainer dataContainer)
        {

        }

        public void AddRange(IEnumerable<IDataContainer> dataContainers)
        {

        }

        public IEnumerator<IDataContainer> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
