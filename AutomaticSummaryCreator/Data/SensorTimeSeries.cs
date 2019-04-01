using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticSummaryCreator.Data
{
    public sealed class SensorTimeSeries : IDataContainer
    {
        public int Count => throw new NotImplementedException();

        public string Id => throw new NotImplementedException();

        public DataPoint First => throw new NotImplementedException();

        public DataPoint Last => throw new NotImplementedException();

        public void Add(DataPoint dataPoint)
        {
            throw new NotImplementedException();
        }

        public double Total(DateTime pointInTime)
        {
            throw new NotImplementedException();
        }

        public double Sum(DateTime start, TimeSpan range)
        {
            throw new NotImplementedException();
        }

        public double Sum(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<DataPoint> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
