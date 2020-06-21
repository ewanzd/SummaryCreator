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

        private const string dbdataIdenticator = "dbdata";
        private const string setMeterIdenticator = "sel_meter";

        private readonly DirectoryInfo sourceDirectory;

        public DirectoryCsvReader(DirectoryInfo sourceDirectory)
        {
            this.sourceDirectory = sourceDirectory ?? throw new ArgumentNullException(nameof(sourceDirectory));
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

                if (IsDbdataSensor(file))
                {
                    var reader = new DbdataSensorCsvReader(file);
                    containers.AddRange(reader.Read());
                }
                else if(IsSelMeterSensor(file))
                {
                    var reader = new SelMeterCsvReader(file);
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

        private bool IsDbdataSensor(FileInfo file)
        {
            Debug.Assert(file != null, $"{nameof(file)} must not be null.");

            var filePrefix = file.Name.Split(fileNameSeparator).FirstOrDefault();

            return filePrefix != null && filePrefix.Equals(dbdataIdenticator, StringComparison.InvariantCulture);
        }

        private bool IsSelMeterSensor(FileInfo file)
        {
            Debug.Assert(file != null, $"{nameof(file)} must not be null.");

            return file.Name.Contains(setMeterIdenticator, StringComparison.InvariantCulture);
        }
    }
}