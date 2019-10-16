using SummaryCreator.Data;
using System.Collections.Generic;
using System.IO;

namespace SummaryCreator.Services
{
    /// <summary>
    /// Load and write data.
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Load sensor data from all files in directory.
        /// </summary>
        /// <param name="sourceDirectory">Path to directory with sensor data.</param>
        /// <returns></returns>
        IEnumerable<IDataContainer> ReadSensorData(DirectoryInfo sourceDirectory);

        /// <summary>
        /// Read meteo data from file.
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <returns></returns>
        IEnumerable<IDataContainer> ReadMeteoData(FileInfo sourceFile);

        /// <summary>
        /// Write all data to file.
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="destinationExcel"></param>
        /// <param name="sheetName"></param>
        /// <param name="idRow"></param>
        void WriteToExcel(IEnumerable<IDataContainer> containers, FileInfo destinationExcel, string sheetName, int idRow);
    }
}