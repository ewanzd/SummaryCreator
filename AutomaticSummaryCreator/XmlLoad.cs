using AutomaticSummaryCreator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutomaticSummaryCreator
{
    /// <summary>
    /// Liest alle Daten aus dem XML.
    /// </summary>
    public class XmlLoad
    {
        /// <summary>
        /// Das Dokument, welches die gelesenen Daten beherbergen wird.
        /// </summary>
        protected XDocument xdocument = null;

        /// <summary>
        /// Pfad zur XML-Datei.
        /// </summary>
        public string Path
        {
            get;
            protected set;
        }

        /// <summary>
        /// XML-Dokument abrufen und abspeichern.
        /// </summary>
        /// <param name="path">Pfad zum XML-Dokument.</param>
        public XmlLoad(string path)
        {
            this.Path = path;
            xdocument = XDocument.Load(this.Path);
        }

        /// <summary>
        /// Daten des Dokumentes auswerten.
        /// </summary>
        /// <returns>Das neue Datenobjekt.</returns>
        public virtual MeteoData Evaluation()
        {
            // Datenobjekt, in der die Daten gespeichert werden.
            MeteoData data = new MeteoData();

            // Grundelement abrufen
            IEnumerable<XElement> root = xdocument.Elements();
            IEnumerable<XElement> meteodata = root.Elements("meteodata");

            // Grunddaten speichern
            data.Produced = DateTime.Parse((string)meteodata.Elements("produced").First());
            data.Author = (string)meteodata.Elements("author").First();
            data.Location = (string)meteodata.Elements("location").Elements("info").Elements("name").First();

            // Daten des Standortes abrufen
            IEnumerable<XElement> location = meteodata.Elements("location");

            // Alle Prognosen dem Datenobjekt hinzufügen
            foreach(var forecast in nextForecast(location.Elements("values")))
                data.Add(forecast);

            // Die Auswertung zurückgeben
            return data;
        }

        /// <summary>
        /// Alle Prognosen des Elementes auswerten.
        /// </summary>
        /// <param name="location">Alle Elemente des Ortes.</param>
        /// <returns>Die erstellte Prognose.</returns>
        protected virtual IEnumerable<Forecast> nextForecast(IEnumerable<XElement> location)
        {
            // Alle Prognosen des Standortes
            foreach(var values in location)
            {
                // Neue Prognose
                Forecast forecast = new Forecast();

                // Anzahl Tage nach der Auswertung
                forecast.Day = getDay(values);

                // Datum abrufen und speichern
                DateTime date;
                if(DateTime.TryParse((string)values.Element("valid").Element("date"), out date))
                    forecast.DateOne = date;
                DateTime start;
                if(DateTime.TryParse((string)values.Element("valid").Element("start"), out start))
                    forecast.DateOne = start;
                DateTime end;
                if(DateTime.TryParse((string)values.Element("valid").Element("end"), out end))
                    forecast.DateTwo = end;

                // Jeder Wert der Prognose hinzufügen
                foreach(var value in nextValue(values.Elements()))
                    forecast.Add(value);

                // Prognose zurückgeben
                yield return forecast;
            }
        }

        /// <summary>
        /// Alle Werte einer Prognose abfragen.
        /// </summary>
        /// <param name="values">Alle Elemente der Prognose.</param>
        /// <returns>Der erstellte Wert.</returns>
        protected virtual IEnumerable<Value> nextValue(IEnumerable<XElement> values)
        {
            // Alle Werte der Prognose abrufen
            foreach(var value in values)
            {
                if(value.Name == "value")
                {
                    // Neuer Wert erstellen
                    Value temp = new Value();

                    // Name hinzufügen
                    temp.Name = (string)value.Attribute("type");

                    // Einheit hinzufügen
                    temp.Unit = (string)value.Attribute("unit");

                    // Daten des Wertes speichern
                    temp.Data = (string)value;

                    // Wert zurückgeben
                    yield return temp;
                }
            }
        }

        /// <summary>
        /// Ruft den Tag der Prognose ab.
        /// </summary>
        /// <param name="element">Das Element, in dem sich die Prognose befindet.</param>
        /// <returns>Gibt den Tag zurück.</returns>
        protected virtual int getDay(XElement element)
        {
            string type = (string)element.Attribute("type");
            int returnValue = -1;
            Int32.TryParse(type.Substring(type.Length - 1), out returnValue);
            return returnValue;
        }
    }
}
