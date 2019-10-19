using SummaryCreator.Data;
using SummaryCreator.IO.Csv;
using SummaryCreator.IO.Excel;
using SummaryCreator.IO.Xml;
using System.Collections.Generic;
using System.IO;

namespace SummaryCreator.Services
{
    /// <summary>
    /// Load and write data.
    /// </summary>
    public class DataService
    {
        /// <summary>
        /// Load sensor data from all files in directory.
        /// </summary>
        /// <param name="sourceDirectory">Path to directory with sensor data.</param>
        /// <returns></returns>
        public IEnumerable<IContainer> ReadSensorData(DirectoryInfo sourceDirectory)
        {
            var reader = new DirectoryCsvReader(sourceDirectory);

            return reader.Read();
        }

        /// <summary>
        /// Read meteo data from file.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        public IEnumerable<IContainer> ReadMeteoData(FileInfo sourceFile)
        {
            var reader = new MeteoXmlReader(sourceFile);

            return reader.Read();
        }

        /// <summary>
        /// Write all data to file.
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="destinationExcel"></param>
        /// <param name="sheetName"></param>
        /// <param name="idRow"></param>
        public void WriteToExcel(IEnumerable<IContainer> containers, FileInfo destinationExcel, string sheetName, int idRow)
        {
            var writer = new EppExcelWriter(destinationExcel, sheetName, idRow);

            writer.Write(containers);
        }
    }
}