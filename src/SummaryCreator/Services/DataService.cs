using SummaryCreator.Data;
using SummaryCreator.IO.Csv;
using SummaryCreator.IO.Excel;
using SummaryCreator.IO.Xml;
using System.Collections.Generic;
using System.IO;

namespace SummaryCreator.Services
{
    public class DataService : IDataService
    {
        public IEnumerable<IDataContainer> ReadSensorData(DirectoryInfo sourceDirectory)
        {
            var reader = new DirectoryCsvReader(sourceDirectory);

            return reader.Read();
        }

        public IEnumerable<IDataContainer> ReadMeteoData(FileInfo sourceFile)
        {
            var reader = new MeteoXmlReader(sourceFile);

            return reader.Read();
        }

        public void WriteToExcel(IEnumerable<IDataContainer> containers, FileInfo destinationExcel, string sheetName, int idRow)
        {
            var writer = new ExcelWriter();
            try
            {
                writer.InitWorksheet(destinationExcel, sheetName, idRow);
                writer.Write(containers);
                writer.SaveAndClose();
            }
            catch
            {
                writer.Close();
                throw;
            }
        }
    }
}