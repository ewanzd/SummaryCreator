using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeExcel = Microsoft.Office.Interop.Excel; 

namespace BasicLibrary.Excel
{
    /// <summary>
    /// Stellt grundlegende Funktionen eines Arbeitsblattes zur Verfügung, kann aber auch vererbt und erweitert werden.
    /// </summary>
    public class Sheet
    {
        /// <summary>
        /// Auf eine bestimmte Zelle des Arbeitsblattes zugreifen.
        /// </summary>
        /// <param name="row">Die Zeile der Zelle.</param>
        /// <param name="col">Die Spalte der Zelle.</param>
        /// <returns>Der Inhalt der Zelle.</returns>
        public string this[int row, int col]
        {
            get
            {
                dynamic dyn = (sheet.Cells[row, col] as OfficeExcel.Range).Value;
                if(dyn == null)
                    return String.Empty;
                else if(dyn is DateTime)
                    return dyn.ToShortDateString();
                else
                    return dyn.ToString();
            }
            set
            {
                sheet.Cells[row, col] = value;
            }
        }

        /// <summary>
        /// Vorhandene Anzahl der Zeilen.
        /// </summary>
        public int CountRow
        {
            get
            {
                return sheet.UsedRange.Rows.Count;
            }
        }

        /// <summary>
        /// Vorhandene Anzahl der Spalten.
        /// </summary>
        public int CountCol
        {
            get
            {
                return sheet.UsedRange.Columns.Count;
            }
        }

        /// <summary>
        /// Dem Arbeitsblatt einen Namen geben.
        /// </summary>
        public string Name
        {
            get
            {
                return sheet.Name;
            }
            set
            {
                sheet.Name = value;
            }
        }

        /// <summary>
        /// Das zu verwaltende Arbeitsblatt.
        /// </summary>
        protected OfficeExcel.Worksheet sheet = null;

        /// <summary>
        /// Neues Arbeitsblatt erstellen, besitzt jedoch noch kein Arbeitsblatt, auf dieses zugegriffen werden könnte.
        /// </summary>
        public Sheet()
        {

        }

        /// <summary>
        /// Neues vereinfachtes Arbeitsblatt mit dem mitgegeben Arbeitsblatt erstellen.
        /// </summary>
        /// <param name="sheet"></param>
        public Sheet(OfficeExcel.Worksheet sheet)
        {
            SetSheet(sheet);
        }

        /// <summary>
        /// Arbeitsblatt setzen.
        /// </summary>
        /// <param name="sheet"></param>
        public virtual void SetSheet(OfficeExcel.Worksheet sheet)
        {
            this.sheet = sheet;
        }
    }
}
