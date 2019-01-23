using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace AutomaticSummaryCreator.Data
{
    /// <summary>
    /// Speichert alle Daten einer Auswertung.
    /// </summary>
    public class MeteoData : IEnumerable<Forecast>
    {
        /// <summary>
        /// Zeitpunkt, in der die Auswertung erstellt wurde.
        /// </summary>
        public DateTime Produced
        {
            get;
            set;
        }

        /// <summary>
        /// Autor der Quelldatei.
        /// </summary>
        public string Author
        {
            get;
            set;
        }

        /// <summary>
        /// Auf die Region, auf die sich die Daten beziehen.
        /// </summary>
        public string Location
        {
            get;
            set;
        }

        /// <summary>
        /// Liste mit allen Prognosen.
        /// </summary>
        List<Forecast> forecasts = new List<Forecast>();

        /// <summary>
        /// Neue Prognose hinzufügen.
        /// </summary>
        /// <param name="forecast">Die neue Prognose.</param>
        public void Add(Forecast forecast)
        {
            // Es dürfen keine leere Prognose eingefügt werden
            if(forecast == null)
                throw new ArgumentNullException("forecast");

            // Prognose hinzufügen
            forecasts.Add(forecast);
        }

        /// <summary>
        /// Anzahl Prognosen in dieser Auswertung.
        /// </summary>
        public int Count
        {
            get
            {
                return forecasts.Count;
            }
        }

        /// <summary>
        /// Löscht eine Prognose.
        /// </summary>
        /// <param name="item">Die Prognose, die gelöscht werden soll.</param>
        /// <returns>Gibt zurück, ob die Prognose erfolgreich gelöscht werden konnte.</returns>
        public bool Remove(Forecast item)
        {
            return forecasts.Remove(item);
        }

        /// <summary>
        /// Alle Prognosen schrittweise zurückgeben.
        /// </summary>
        /// <returns>Die einzelnen Prognosen.</returns>
        public IEnumerator<Forecast> GetEnumerator()
        {
            foreach(var forecast in forecasts)
                yield return forecast;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
