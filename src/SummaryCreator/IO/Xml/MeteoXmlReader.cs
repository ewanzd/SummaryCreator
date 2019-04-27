using SummaryCreator.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SummaryCreator.IO.Xml
{
    public sealed class MeteoXmlReader : IDataReader
    {
        private readonly FileInfo sourceFile;

        public MeteoXmlReader(FileInfo sourceFile)
        {
            Debug.Assert(sourceFile != null, $"{nameof(sourceFile)} must not be null");

            this.sourceFile = sourceFile;
        }

        public IEnumerable<IDataContainer> Read()
        {
            var xDocument = XDocument.Load(sourceFile.OpenRead());

            return Evaluation(xDocument.Root);
        }

        /// <summary>
        /// Daten des Dokumentes auswerten.
        /// </summary>
        /// <returns>Das neue Datenobjekt.</returns>
        private IEnumerable<IDataContainer> Evaluation(XElement root)
        {
            // Datenobjekt, in der die Daten gespeichert werden.
            var meteoDataContainers = new List<IDataContainer>();

            // Grundelement abrufen
            IEnumerable<XElement> meteodata = root.Elements("meteodata");

            // Daten des Standortes abrufen
            IEnumerable<XElement> locationContainer = meteodata.Elements("location");

            // Alle Prognosen des Standortes
            foreach (var values in locationContainer.Elements("values"))
            {
                // Datum abrufen und speichern
                var dateStr = (string)values.Element("valid").Element("date");
                if(dateStr == null)
                {
                    dateStr = (string)values.Element("valid").Element("end");
                }
                if (!DateTime.TryParse(dateStr, out DateTime date))
                {
                    throw new InvalidDataException($"Ungültiges Format: {dateStr}");
                }

                // Alle Werte der Prognose abrufen
                foreach (var value in values.Elements())
                {
                    if (value.Name == "value")
                    {
                        // Name hinzufügen
                        var type = (string)value.Attribute("type");

                        // nur irradience
                        if (!type.Equals("irradience", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        // Neuer Wert erstellen
                        var dataPoint = new DataPoint();

                        // Daten des Wertes speichern
                        var valueStr = (string)value;

                        // Wert konvertieren
                        if (double.TryParse(valueStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double val))
                        {
                            dataPoint.Value = val;
                        }
                        else
                        {
                            throw new InvalidDataException($"Ungültiges Format: {valueStr}");
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

            // Die Auswertung zurückgeben
            return meteoDataContainers;
        }
    }
}
