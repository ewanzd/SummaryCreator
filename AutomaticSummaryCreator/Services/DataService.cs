using AutomaticSummaryCreator.Data;
using AutomaticSummaryCreator.IO.Csv;
using AutomaticSummaryCreator.IO.Excel;
using AutomaticSummaryCreator.IO.Xml;
using System.Collections.Generic;
using System.IO;

namespace AutomaticSummaryCreator.Services
{
    public class DataService
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
            var sheet = new SheetDataInsert(destinationExcel.FullName, sheetName, idRow);
            var writer = new ExcelWriter(sheet);

            writer.Write(containers);
        }
    }
}
