using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutomaticSummaryCreator.Data
{
    public sealed class DataGroup : IEnumerable<IDataContainer>
    {
        private readonly List<IDataContainer> containers = new List<IDataContainer>();

        public void Add(IDataContainer dataContainer)
        {
            Debug.Assert(dataContainer != null, $"{nameof(dataContainer)} must not be null");

            containers.Add(dataContainer);
        }

        public void AddRange(IEnumerable<IDataContainer> dataContainers)
        {
            Debug.Assert(dataContainers != null && dataContainers.Any(), $"{nameof(dataContainers)} must not be null");

            containers.AddRange(dataContainers);
        }

        public IEnumerator<IDataContainer> GetEnumerator()
        {
            return containers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
