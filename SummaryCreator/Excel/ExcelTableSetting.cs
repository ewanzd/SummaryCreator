using SummaryCreator.Basic;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SummaryCreator.Export
{
    /// <summary>
    /// Save excel settings.
    /// </summary>
    [Serializable]
    public class ExcelTableSetting
    {
        /// <summary>
        /// The name from worksheet.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The time difference between rows.
        /// </summary>
        [XmlIgnore]
        public TimeSpan Interval
        {
            get;
            set;
        }

        /// <summary>
        /// Only for xml ex- and import.
        /// </summary>
        [XmlElement("Interval")]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public long IntervalTicks
        {
            get
            {
                return Interval.Ticks;
            }
            set
            {
                Interval = new TimeSpan(value);
            }
        }

        /// <summary>
        /// Show the difference between values to one row before.
        /// </summary>
        public bool DifferenceActive
        {
            get;
            set;
        }

        /// <summary>
        /// Unit from output.
        /// </summary>
        public Unit OutputUnit
        {
            get;
            set;
        }

        /// <summary>
        /// Create new setting with default values.
        /// </summary>
        public ExcelTableSetting()
            : this(String.Empty, new TimeSpan(1,0,0))
        {

        }

        /// <summary>
        /// Create new setting with own values.
        /// </summary>
        /// <param name="name">The name from worksheet.</param>
        /// <param name="interval">The time difference between rows.</param>
        /// <param name="outputUnit">Unit from output.</param>
        /// <param name="differenceActive">Show the difference between values to one row before.</param>
        public ExcelTableSetting(string name, TimeSpan interval, Unit outputUnit = Unit.None, bool differenceActive = false)
        {
            this.Name = name;
            this.Interval = interval;
            this.DifferenceActive = differenceActive;
            this.OutputUnit = outputUnit;
        }
    }
}
