using AutomaticSummaryCreator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AutomaticSummaryCreator.IO.Csv
{
    public sealed class SensorCsvReader : IDataReader
    {
        private const char rowSeperator = ';';

        private readonly FileInfo sourceFile;

        public SensorCsvReader(FileInfo sourceFile)
        {
            Debug.Assert(sourceFile != null, $"{nameof(sourceFile)} must not be null");

            this.sourceFile = sourceFile;
        }

        public IEnumerable<IDataContainer> Read()
        {
            throw new NotImplementedException();
        }
    }
}
