﻿using SummaryCreator.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace SummaryCreator.Input.Xml
{
    /// <summary>
    /// Reader for meteo data.
    /// </summary>
    public sealed class MeteoXmlReader : ITimeSeriesReader
    {
        public IEnumerable<ITimeSeries> Read(string id, string content)
        {
            var xDocument = XDocument.Parse(content);

            return Evaluation(xDocument.Root);
        }

        /// <summary>
        /// Evaluate meteo data from xml tree.
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<ITimeSeries> Evaluation(XElement root)
        {
            // list of meteo time series
            var meteoTimeSeries = new List<ITimeSeries>();

            // get base element
            IEnumerable<XElement> meteodata = root.Elements("meteodata");

            // find location data
            IEnumerable<XElement> locationTimeSeries = meteodata.Elements("location");

            // of data of this location
            foreach (var values in locationTimeSeries.Elements("values"))
            {
                // get date and safe it
                var dateStr = (string)values.Element("valid").Element("date") ?? (string)values.Element("valid").Element("end");
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

                        var timeSeries = meteoTimeSeries.Find(x => x.Id.Equals(type, StringComparison.InvariantCultureIgnoreCase));
                        if (timeSeries == null)
                        {
                            timeSeries = new MeteoTimeSeries(type);
                            meteoTimeSeries.Add(timeSeries);
                        }
                        timeSeries.Add(dataPoint);
                    }
                }
            }

            return meteoTimeSeries;
        }
    }
}