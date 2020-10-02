using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SummaryCreator.Core
{
    /// <summary>
    /// Manage a group of containers.
    /// </summary>
    public sealed class ContainerGroup : IEnumerable<IContainer>
    {
        private readonly List<IContainer> containers = new List<IContainer>();

        /// <summary>
        /// Get container by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IContainer this[string id] {
            get {
                if (id == null) return null;

                return containers.FirstOrDefault(c => c.Id.Equals(id, StringComparison.InvariantCulture));
            }
        }

        public int Count {
            get {
                return containers.Count;
            }
        }

        /// <summary>
        /// Find first <see cref="DataPoint"/>. Property return null if no data found.
        /// </summary>
        public DataPoint FirstDataPoint {
            get {
                if (containers.Count == 0)
                {
                    return null;
                }
                return containers.Select(x => x.First).Aggregate((dpMin, x) => (dpMin == null || (x?.CapturedAt ?? DateTime.MaxValue) < dpMin.CapturedAt ? x : dpMin));
            }
        }

        /// <summary>
        /// Find last <see cref="DataPoint"/>. Property return null if no data found.
        /// </summary>
        public DataPoint LastDataPoint {
            get {
                if (containers.Count == 0)
                {
                    return null;
                }
                return containers.Select(x => x.Last).Aggregate((dpMax, x) => (dpMax == null || (x?.CapturedAt ?? DateTime.MinValue) > dpMax.CapturedAt ? x : dpMax));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataContainer"></param>
        /// <exception cref="ArgumentNullException">Null is not allowed.</exception>
        public void Add(IContainer dataContainer)
        {
            if (dataContainer == null) throw new ArgumentNullException(nameof(dataContainer));

            containers.Add(dataContainer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dataContainers"></param>
        /// <exception cref="ArgumentNullException">Null is not allowed.</exception>
        /// <exception cref="ArgumentException">IEnumerable contains null values.</exception>
        public void AddRange(IEnumerable<IContainer> dataContainers)
        {
            if (dataContainers == null) throw new ArgumentNullException(nameof(dataContainers));
            if (dataContainers.Any(x => x == null)) throw new ArgumentException($"{nameof(dataContainers)} cannot contain null values.");

            containers.AddRange(dataContainers);
        }

        public bool AnyBetween(DateTimeOffset start, DateTimeOffset end)
        {
            return containers.Any(container => container.AnyBetween(start, end));
        }

        /// <summary>
        /// Calculate sum of all values in time range.
        /// </summary>
        /// <param name="start">Start date time to get sum.</param>
        /// <param name="end">End date time to get sum.</param>
        /// <returns>Sum of all values in time range.</returns>
        public double Sum(DateTimeOffset start, DateTimeOffset end)
        {
            return containers.Sum(x => x.Sum(start, end));
        }

        public double Total(DateTimeOffset pointInTime)
        {
            return containers.Sum(x => x.TotalUntil(pointInTime));
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