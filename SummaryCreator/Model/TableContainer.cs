using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummaryCreator.Model
{
    public abstract class TableContainer
    {
        public string ID
        {
            get;
            set;
        }

        private string _headerText = String.Empty;

        public string HeaderText
        {
            get
            {
                if(String.IsNullOrEmpty(_headerText))
                    return ID;
                else
                    return _headerText;
            }
            set
            {
                _headerText = value;
            }
        }

        public abstract string TypeName
        {
            get;
        }

        public abstract Row GetRow(DateTime start, DateTime end);

        public abstract DateTime FirstTime
        {
            get;
        }

        public abstract DateTime LastTime
        {
            get;
        }

        public abstract Row FirstRow(DateTime start, DateTime end);

        public abstract void Sort();

        public abstract IEnumerable<Row> NextRow(DateTime start, TimeSpan interval);

        public override string ToString()
        {
            return HeaderText;
        }
    }
}
