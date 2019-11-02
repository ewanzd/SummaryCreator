using SummaryCreator.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SummaryCreator.IO.Xml
{
    /// <summary>
    /// Reader for meteo data.
    /// </summary>
    public sealed class MeteoXmlReader : IDataReader
    {
        private readonly FileInfo sourceFile;

        public MeteoXmlReader(FileInfo sourceFile)
        {
            if (sourceFile == null) throw new ArgumentNullException(nameof(sourceFile));

            this.sourceFile = sourceFile;
        }

        public IEnumerable<IContainer> Read()
        {
            var xDocument = XDocument.Load(sourceFile.OpenRead());

            return Evaluation(xDocument.Root);
        }

        /// <summary>
        /// Evaluate meteo data from xml tree.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IContainer> Evaluation(XElement root)
        {
            // list of data containers
            var meteoDataContainers = new List<IContainer>();

            // get base element
            IEnumerable<XElement> meteodata = root.Elements("meteodata");

            // find location data
            IEnumerable<XElement> locationContainer = meteodata.Elements("location");

            // of data of this location
            foreach (var values in locationContainer.Elements("values"))
            {
                // get date and safe it
                var dateStr = (string)values.Element("valid").Element("date");
                if (dateStr == null)
                {
                    dateStr = (string)values.Element("valid").Element("end");
                }
                if (!DateTime.TryParse(dateStr, out DateTime date))
                {
                    throw new InvalidDataException($"Invalid format: {dateStr}");
                }

                // get all meteo data
                foreach (var value in values.Elements())
                {
                    if (value.Name == "value")
                    {
                        // add name
                        var type = (string)value.Attribute("type");

                        // take only irradience
                        if (!type.Equals("irradience", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        var dataPoint = new DataPoint();
                        var valueStr = (string)value;

                        if (double.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                        {
                            dataPoint.Value = val;
                        }
                        else
                        {
                            throw new InvalidDataException($"Invalid format: {valueStr}");
                        }
                        dataPoint.CapturedAt = date;

                        var container = meteoDataContainers.FirstOrDefault(x => x.Id.Equals(type, StringComparison.InvariantCultureIgnoreCase));
                        if (container == null)
                        {
                            container = new MeteoContainer(type);
                            meteoDataContainers.Add(container);
                        }
                        container.Add(dataPoint);
                    }
                }
            }

            return meteoDataContainers;
        }
    }
}