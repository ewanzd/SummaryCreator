using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SummaryCreator.Data
{
    public sealed class ContainerGroup : IEnumerable<IContainer>
    {
        private readonly List<IContainer> containers = new List<IContainer>();

        public IContainer this[string id] {
            get {
                Debug.Assert(id != null, $"{nameof(id)} must not be null.");

                return containers.FirstOrDefault(c => c.Id.Equals(id, StringComparison.InvariantCulture));
            }
        }

        public int Count {
            get {
                return containers.Count;
            }
        }

        public DataPoint FirstDataPoint {
            get {
                if (containers.Count == 0)
                {
                    return null;
                }
                return containers
                    .Aggregate((minItem, nextItem) => minItem.First.CapturedAt < nextItem.First.CapturedAt ? minItem : nextItem)?
                    .First;
            }
        }

        public DataPoint LastDataPoint {
            get {
                if (containers.Count == 0)
                {
                    return null;
                }
                return containers
                    .Aggregate((maxItem, nextItem) => maxItem.Last.CapturedAt > nextItem.Last.CapturedAt ? maxItem : nextItem)?
                    .Last;
            }
        }

        public void Add(IContainer dataContainer)
        {
            Debug.Assert(dataContainer != null, $"{nameof(dataContainer)} must not be null");

            containers.Add(dataContainer);
        }

        public void AddRange(IEnumerable<IContainer> dataContainers)
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

        public IEnumerator<IContainer> GetEnumerator()
        {
            return containers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}