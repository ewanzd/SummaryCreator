using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AutomaticSummaryCreator.Data
{
    public sealed class DataGroup : IEnumerable<IDataContainer>
    {
        private readonly List<IDataContainer> containers = new List<IDataContainer>();

        public int Count {
            get {
                return containers.Count;
            }
        }

        public DataPoint First => containers
            .Aggregate((minItem, nextItem) => minItem.First.CapturedAt < nextItem.First.CapturedAt ? minItem : nextItem)
            .First;

        public DataPoint Last => containers
            .Aggregate((minItem, nextItem) => minItem.Last.CapturedAt > nextItem.Last.CapturedAt ? minItem : nextItem)
            .Last;

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

        public bool AnyBetween(DateTime start, DateTime end)
        {
            return containers.Any(container => container.AnyBetween(start, end));
        }

        /// <summary>
        /// Calculate sum of all values in time range.
        /// </summary>
        /// <param name="start">Start date time to get sum.</param>
        /// <param name="end">End date time to get sum.</param>
        /// <returns>Sum of all values in time range.</returns>
        public double Sum(DateTime start, DateTime end)
        {
            return containers.Sum(x => x.Sum(start, end));
        }

        public double Total(DateTime pointInTime)
        {
            return containers.Sum(x => x.Total(pointInTime));
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
