using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticSummaryCreator.Data
{
    /// <summary>
    /// Ein genauer Wert der Prognose.
    /// </summary>
    public class Value
    {
        /// <summary>
        /// Name(ID) des Wertes.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Format des Wertes.
        /// </summary>
        public string Unit
        {
            get;
            set;
        }

        /// <summary>
        /// Inhalt des Wertes.
        /// </summary>
        public string Data
        {
            get;
            set;
        }
    }
}
