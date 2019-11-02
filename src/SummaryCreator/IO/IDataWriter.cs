﻿using SummaryCreator.Core;
using System.Collections.Generic;

namespace SummaryCreator.IO
{
    public interface IDataWriter
    {
        void Write(IEnumerable<IContainer> containers);
    }
}