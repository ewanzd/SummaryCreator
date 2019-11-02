using SummaryCreator.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SummaryCreator.IO.Csv
{
    /// <summary>
    /// Read every csv file in directory.
    /// </summary>
    public sealed class DirectoryCsvReader : IDataReader
    {
        private const char fileNameSeparator = '_';
        private const string fileExtension = ".csv";
        private const string prefix = "dbdata";

        private readonly DirectoryInfo sourceDirectory;

        public DirectoryCsvReader(DirectoryInfo sourceDirectory)
        {
            if (sourceDirectory == null) throw new ArgumentNullException(nameof(sourceDirectory));

            this.sourceDirectory = sourceDirectory;
        }

        public IEnumerable<IContainer> Read()
        {
            List<IContainer> containers = new List<IContainer>();

            foreach (var file in sourceDirectory.EnumerateFiles())
            {
                if (!file.Extension.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (IsNewSensor(file))
                {
                    var reader = new NewSensorCsvReader(file);
                    containers.AddRange(reader.Read());
                }
                else
                {
                    var reader = new SensorCsvReader(file);
                    containers.AddRange(reader.Read());
                }
            }

            return containers;
        }

        private bool IsNewSensor(FileInfo file)
        {
            Debug.Assert(file != null, $"{nameof(file)} must not be null.");

            var filePrefix = file.Name.Split(fileNameSeparator).FirstOrDefault();

            return filePrefix != null && filePrefix.Equals(prefix, StringComparison.InvariantCulture);
        }
    }
}