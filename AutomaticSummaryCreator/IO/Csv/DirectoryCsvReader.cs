﻿using AutomaticSummaryCreator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AutomaticSummaryCreator.IO.Csv
{
    public class DirectoryCsvReader : IDataReader
    {
        private const char fileNameSeparator = '_';
        private const string fileExtension = ".csv";
        private const string prefix = "dbdata";

        private readonly DirectoryInfo sourceDirectory;

        public DirectoryCsvReader(DirectoryInfo sourceDirectory)
        {
            Debug.Assert(sourceDirectory != null, $"{nameof(sourceDirectory)} must not be null");

            this.sourceDirectory = sourceDirectory;
        }

        public IEnumerable<IDataContainer> Read()
        {
            List<IDataContainer> containers = new List<IDataContainer>();

            foreach(var file in sourceDirectory.EnumerateFiles())
            {
                if(!file.Extension.Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if(IsNewSensor(file))
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
            var filePrefix = file.Name.Split(fileNameSeparator).FirstOrDefault();

            return filePrefix != null && filePrefix.Equals(prefix);
        }
    }
}
