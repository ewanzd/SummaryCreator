using System;
using System.IO;

namespace SummaryCreator.Source
{
    /// <summary>
    /// Manage a file path with a number as first few char.
    /// </summary>
    public class CSVFile
    {
        /// <summary>
        /// Set or get the path of source.
        /// </summary>
        public string SourcePath
        {
            get;
            protected set;
        }

        /// <summary>
        /// Return the file name.
        /// </summary>
        public string GetFileName
        {
            get
            {
                return Path.GetFileName(SourcePath);
            }
        }

        /// <summary>
        /// Return the number of file.
        /// </summary>
        public int GetFileTypeNumber
        {
            get
            {
                int number = 0;
                Int32.TryParse(GetFileName.Split(new char[] { '_' })[0], out number);
                return number;
            }
        }

        /// <summary>
        /// Constructor: Create new CSVFile with path.
        /// </summary>
        /// <param name="path">The path of this object.</param>
        public CSVFile(string path)
        {
            this.SourcePath = path;
        }
    }
}
