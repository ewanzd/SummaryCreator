﻿using System;
using System.Collections.Generic;

namespace SummaryCreator.Data
{
    public interface IDataContainer : IEnumerable<DataPoint>
    {
        string Id { get; }
        int Count { get; }
        DataPoint First { get; }
        DataPoint Last { get; }

        bool AnyBetween(DateTime start, DateTime end);
        void Add(DataPoint dataPoint);
        double Sum(DateTime start, TimeSpan range);
        double Sum(DateTime start, DateTime end);
        double Total(DateTime pointInTime);
    }
}