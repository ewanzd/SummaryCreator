using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummaryCreator.Source
{
    /// <summary>
    /// Help to order and save names.
    /// </summary>
    public class IndexName
    {
        /// <summary>
        /// Display name from item.
        /// </summary>
        public string Displayname
        {
            get;
            set;
        }

        /// <summary>
        /// Id from item.
        /// </summary>
        public string ID
        {
            get;
            set;
        }

        public IndexName()
        {

        }

        public IndexName(string displayName, string id)
        {
            this.Displayname = displayName;
            this.ID = id;
        }
    }
}
