using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticSummaryCreator.Data
{
    /// <summary>
    /// Spezifische Prognose.
    /// </summary>
    public class Forecast
    {
        /// <summary>
        /// Auf Werte mit dessen spezifischen Namen zugreifen.
        /// </summary>
        /// <param name="name">Name des Wertes.</param>
        /// <returns>Gibt das Wert-Objekt zurück.</returns>
        public Value this[string name]
        {
            get
            {
                return (from v in values
                        where v.Name == name
                        select v).First();
            }
        }

        /// <summary>
        /// Datum oder Startzeitpunkt der Prognose.
        /// </summary>
        public DateTime DateOne
        {
            get;
            set;
        }

        /// <summary>
        /// Enddatum der Prognose.
        /// </summary>
        public DateTime DateTwo
        {
            get;
            set;
        }

        /// <summary>
        /// Tage in der Zukunft ab der Auswertung.
        /// </summary>
        public int Day
        {
            get;
            set;
        }

        /// <summary>
        /// Liste mit Werten.
        /// </summary>
        List<Value> values = new List<Value>();

        /// <summary>
        /// Neuen Wert hinzufügen.
        /// </summary>
        /// <param name="value">Der neue Wert, der hinzugefügt werden soll.</param>
        public void Add(Value value)
        {
            if(value == null)
                throw new ArgumentNullException("value");

            values.Add(value);
        }

        /// <summary>
        /// Prüft, ob ein Wert mit diesem Namen vorhanden ist.
        /// </summary>
        /// <param name="name">Der Name des Wertes.</param>
        /// <returns>Gibt ein bool-Wert zurück, ob ein Wert gefunden wurde.</returns>
        public bool ContainsName(string name)
        {
            return (from v in values
                    where v.Name == name
                    select v).Any();
        }
    }
}
